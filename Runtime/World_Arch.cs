#region 注 释

/***
 *
 *  Title:
 *
 *  Description:
 *
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/haloman9527
 *  Blog: https://www.haloman.net/
 *
 */

#endregion

using System;
using Atom.UnsafeEx;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Atom.ECS
{
    public unsafe partial class World
    {
        private UnsafeParallelHashMap<int, IntPtr> archetypes = new UnsafeParallelHashMap<int, IntPtr>();
        private UnsafeParallelHashMap<int, EntityInChunk> entityInChunks = new UnsafeParallelHashMap<int, EntityInChunk>();

        public void CreateEntities(Archetype* archetype, Entity* entities, int count)
        {
            while (count != 0)
            {
                // 拿一个有容量的Chunk
                var chunk = GetChunkWithEmptySlots(archetype);
                // 这个Chunk有多少容量
                var allocateCount = Math.Min(count, chunk->UnusedCount);

                for (int i = 0; i < allocateCount; i++)
                {
                    var entity = entities + i;
                    var entityInChunk = new EntityInChunk();
                    entityInChunk.chunk = chunk;
                    entityInChunk.indexInChunk = chunk->count;
                    UnsafeUtil.CopyBlock(chunk->buffer + archetype->componentsSize * i, entity, sizeof(Entity));
                    chunk->count++;
                    entityInChunks.Add(entity->id, entityInChunk);
                }

                count -= allocateCount;

                if (entities != null)
                    entities += allocateCount;
            }
        }

        public Entity CreateEntity(Archetype* archetype)
        {
            Entity entity;
            CreateEntities(archetype, &entity, 1);
            return entity;
        }

        public Entity CreateEntity(params TypeInfo[] types)
        {
            fixed (TypeInfo* typesPtr = types)
            {
                return CreateEntity(GetOrCreateArchetype(typesPtr, types.Length));
            }
        }

        public void SetAComponent<T>(Entity entity, T component) where T : unmanaged, IComponent
        {
            if (!entityInChunks.TryGetValue(entity.id, out var oldEntityInChunk))
            {
                return;
            }

            var chunk = oldEntityInChunk.chunk;
            var archetype = chunk->archetype;
            var typeInfo = TypeManager.GetTypeInfo<T>();
            var hasComponent = archetype->typeInArchetypeIndexMap.TryGetValue(typeInfo.id, out var componentIndexInChunk);
            if (!hasComponent)
            {
                var count = archetype->typesCount + 1;
                var types = stackalloc TypeInfo[count];
                types[count - 1] = typeInfo;
                SortTypes(types, count);

                var sortedTypes = types;

                // 重新分配Archetype
                // 根据entity现有类型和新增类型，计算出新的hash
                // 根据现有hash找到或新建archetype
                // 把现有数据迁移到新archetype中
                var hash = CalculateTypesHash(sortedTypes, count);
                if (!TryGetArchetype(sortedTypes, count, out var newArchetype))
                {
                    newArchetype = CreateArchetype(sortedTypes, count);
                }

                // 这里可以优化，可以先整体拷贝，然后插入拷贝一次，这样只需要三次内存移动就可以完成增加组件操作了
                var newChunk = GetChunkWithEmptySlots(newArchetype);
                var oldChunkComponentBaseOffset = archetype->componentsSize * oldEntityInChunk.indexInChunk;
                var newChunkComponentBaseOffset = newArchetype->componentsSize * newChunk->count;
                for (int i = 0; i < archetype->typesCount; i++)
                {
                    var t = archetype->sortedTypes + i;
                    var newIndex = newArchetype->typeInArchetypeIndexMap[t->id];
                    var newExistsComponentOffset = newChunkComponentBaseOffset + newArchetype->offsets[newIndex];
                    var oldExistsComponentOffset = oldChunkComponentBaseOffset + archetype->offsets[i];
                    UnsafeUtil.CopyBlock(newChunk->buffer + newExistsComponentOffset, chunk->buffer + oldExistsComponentOffset, t->componentSize);
                }

                // 把新加的组件拷贝到新chunk中，然后count+1
                var newComponentIndex = newChunkComponentBaseOffset + newArchetype->typeInArchetypeIndexMap[typeInfo.id];
                var newComponentOffset = newChunkComponentBaseOffset + newArchetype->offsets[newComponentIndex];
                UnsafeUtil.CopyBlock(newChunk->buffer + newComponentOffset, &component, typeInfo.componentSize);
                newChunk->count += 1;

                // 把Chunk中最后一个Entity移动到目标Entity，然后count-1，相当于直接舍弃掉一个chunk的内存单元，有值也无所谓
                UnsafeUtil.CopyBlock(chunk->buffer + (archetype->componentsSize * (chunk->count - 1)),
                    chunk->buffer + (archetype->componentsSize * oldEntityInChunk.indexInChunk),
                    archetype->componentsSize);
                chunk->count -= 1;

                // 更新Entity所在的chunk以及下标
                var newEntityInChunk = new EntityInChunk();
                newEntityInChunk.chunk = newChunk;
                newEntityInChunk.indexInChunk = newChunk->count - 1;
                entityInChunks[entity.id] = newEntityInChunk;
            }
            else
            {
                var componentOffsetInChunk = archetype->offsets[componentIndexInChunk];
                UnsafeUtil.CopyBlock(chunk->buffer + componentOffsetInChunk, &component, typeInfo.componentSize);
            }
        }

        public Chunk* GetChunkWithEmptySlots(Archetype* archetype)
        {
            var chunk = archetype->GetExistingChunkWithEmptySlots();
            if (chunk == null)
                chunk = GetCleanChunk(archetype);
            return chunk;
        }

        public Chunk* GetCleanChunk(Archetype* archetype)
        {
            var chunk = (Chunk*)UnsafeUtil.Malloc(sizeof(Chunk));
            chunk->archetype = archetype;
            chunk->capacity = Chunk.BUFFER_SIZE / archetype->componentsSize;
            chunk->listIndex = archetype->chunks.Length;
            chunk->flags = 0;
            chunk->sequenceNumber = 0;
            archetype->chunks.Add(chunk);
            archetype->chunksWithEmptySlots.Add(chunk);
            return chunk;
        }

        public Archetype* GetOrCreateArchetype(TypeInfo* types, int count)
        {
            // 排序types, 保证相同的Archetype的ComponentType顺序一致
            var sortedTypes = stackalloc TypeInfo[count + 1];
            var sortedCount = FillSortedArchetypeArray(sortedTypes, types, count);

            // 查找是否已经存在相同的Archetype
            if (TryGetArchetype(sortedTypes, sortedCount, out var archetype))
                return archetype;

            // 创建新的Archetype
            archetype = CreateArchetype(sortedTypes, sortedCount);
            return archetype;
        }

        /// <summary>
        /// 插入排序
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="src"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int FillSortedArchetypeArray(TypeInfo* dst, TypeInfo* src, int count)
        {
            if (count + 1 > 1024)
                throw new ArgumentException($"Archetypes can't hold more than 1024 components");

            dst[0] = TypeManager.GetTypeInfo<Entity>();
            SortTypes(dst, count);

            return count + 1;
        }

        /// <summary>
        /// 查找是否已经存在相同的Archetype
        /// </summary>
        /// <param name="types"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool TryGetArchetype(TypeInfo* types, int count, out Archetype* archetype)
        {
            var hash = CalculateTypesHash(types, count);
            if (archetypes.TryGetValue(hash, out var ptr))
            {
                archetype = (Archetype*)ptr;
                return true;
            }

            archetype = default;
            return false;
        }

        /// <summary>
        /// 创建新的Archetype
        /// </summary>
        /// <param name="sortedTypes"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Archetype* CreateArchetype(TypeInfo* sortedTypes, int count)
        {
            var hash = CalculateTypesHash(sortedTypes, count);
            var dstArchetype = (Archetype*)UnsafeUtil.Malloc(sizeof(Archetype));
            {
                var size = sizeof(TypeInfo) * count;
                var typesPtr = (Archetype*)UnsafeUtil.Malloc(size);
                UnsafeUtil.CopyBlock(typesPtr, sortedTypes, (uint)size);
                dstArchetype->sortedTypes = (TypeInfo*)typesPtr;
            }
            {
                var size = sizeof(int) * count;
                var offsetsPtr = (int*)UnsafeUtil.Malloc(size);
                var offset = 0;
                for (int i = 0; i < count; i++)
                {
                    var t = (sortedTypes + i);
                    offsetsPtr[i] = t->componentSize;
                    offset += t->componentSize;
                }

                dstArchetype->offsets = offsetsPtr;
            }
            {
                var size = 0;
                for (int i = 0; i < count; i++)
                {
                    var t = (sortedTypes + i);
                    size += t->componentSize;
                }

                dstArchetype->componentsSize = size;
            }
            dstArchetype->typesCount = count;
            dstArchetype->archetypeHash = hash;
            dstArchetype->typeInArchetypeIndexMap = new UnsafeParallelHashMap<int, int>();
            dstArchetype->chunks = new UnsafePtrList<Chunk>();
            dstArchetype->chunksWithEmptySlots = new UnsafePtrList<Chunk>();
            for (int i = 0; i < count; i++)
            {
                var t = (sortedTypes + i);
                dstArchetype->typeInArchetypeIndexMap.Add(t->id, i);
            }

            archetypes.Add(hash, new IntPtr(dstArchetype));
            return dstArchetype;
        }

        public static void SortTypes(TypeInfo* types, int count)
        {
            var dst = types;
            for (int i = 0; i < count; i++)
            {
                InsertSorted(dst, i + 1, types[i]);
            }
        }

        public static void InsertSorted(TypeInfo* data, int length, TypeInfo newValue)
        {
            var newVal = newValue;
            while (length > 0 && newValue < data[length - 1])
            {
                data[length] = data[length - 1];
                --length;
            }

            data[length] = newVal;
        }

        public static int CalculateTypesHash(TypeInfo* types, int count)
        {
            var hash = 5483;

            for (int i = 0; i < count; i++)
            {
                var typeInfo = (types + i);
                hash ^= typeInfo->id;
            }

            return hash;
        }
    }
}