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
using CZToolKit.UnsafeEx;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace CZToolKit.ECS
{
    public unsafe partial class World
    {
        private UnsafePtrList<Archetype> archetype = new UnsafePtrList<Archetype>(32, Allocator.Persistent);


        public void CreateEntities(Archetype* archetype, Entity* entities, int count)
        {
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

        public Archetype* GetOrCreateArchetype(TypeInfo* types, int count)
        {
            // 排序types, 保证相同的Archetype的ComponentType顺序一致
            var sortedTypes = stackalloc TypeInfo[count + 1];
            var sortedCount = FillSortedArchetypeArray(sortedTypes, types, count);

            // 查找是否已经存在相同的Archetype
            var archetype = GetExsitingArchetype(sortedTypes, sortedCount);
            if (archetype != null)
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

            dst[0] = new ComponentType(ComponentType.ReadWrite<Entity>());
            for (int i = 0; i < count; i++)
            {
                InsertSorted(dst, i + 1, src[i]);
            }

            return count + 1;
        }

        /// <summary>
        /// 查找是否已经存在相同的Archetype
        /// </summary>
        /// <param name="sortedTypes"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Archetype* GetExsitingArchetype(TypeInfo* sortedTypes, int count)
        {
            return null;
        }

        /// <summary>
        /// 创建新的Archetype
        /// </summary>
        /// <param name="sortedTypes"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Archetype* CreateArchetype(TypeInfo* sortedTypes, int count)
        {
            Archetype* dstArchetype = (Archetype*)UnsafeUtil.Malloc(sizeof(Archetype));

            var size = sizeof(TypeInfo) * count;
            var dstPtr = (void*)UnsafeUtil.Malloc(size);
            UnsafeUtil.CopyBlock(dstPtr, sortedTypes, (uint)size);
            dstArchetype->types = (TypeInfo*)dstPtr;
            dstArchetype->typeCount = count;
            // dstArchetype->offsets
            dstArchetype->chunks = new UnsafeList();

            return dstArchetype;
        }

        public static void InsertSorted(TypeInfo* data, int length, TypeInfo newValue)
        {
            var newVal = new ComponentType(newValue);
            while (length > 0 && newVal < data[length - 1])
            {
                data[length] = data[length - 1];
                --length;
            }

            data[length] = newVal;
        }
    }
}