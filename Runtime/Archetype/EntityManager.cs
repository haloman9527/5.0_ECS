using System;
using CZToolKit.Common.UnsafeEx;
using Unity.Collections.LowLevel.Unsafe;

namespace CZToolKit.ECS
{
    public unsafe partial struct EntityManager
    {
        public void SetComponent<T>(Entity entity, T component) where T : IComponent
        {
        }

        public void CreateEntities(Archetype* archetype, Entity* entities, int count)
        {
        }

        public Entity CreateEntity(Archetype* archetype)
        {
            Entity entity;
            CreateEntities(archetype, &entity, 1);
            return entity;
        }

        public Entity CreateEntity(params ComponentType[] types)
        {
            fixed (ComponentType* typesPtr = types)
            {
                return CreateEntity(GetOrCreateArchetype(typesPtr, types.Length));
            }
        }

        public Archetype* GetOrCreateArchetype(ComponentType* types, int count)
        {
            // 排序types, 保证相同的Archetype的ComponentType顺序一致
            ComponentType* sortedTypes = stackalloc ComponentType[count + 1];

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
        public int FillSortedArchetypeArray(ComponentType* dst, ComponentType* src, int count)
        {
// #if ENABLE_UNITY_COLLECTIONS_CHECKS
//             if (count + 1 > 1024)
//                 throw new ArgumentException($"Archetypes can't hold more than 1024 components");
// #endif

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
        public Archetype* GetExsitingArchetype(ComponentType* sortedTypes, int count)
        {
            return null;
        }

        /// <summary>
        /// 创建新的Archetype
        /// </summary>
        /// <param name="sortedTypes"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Archetype* CreateArchetype(ComponentType* sortedTypes, int count)
        {
            Archetype* dstArchetype = (Archetype*)UnsafeUtil.Malloc(sizeof(Archetype));

            var size = sizeof(ComponentType) * count;
            var dstPtr = (void*)UnsafeUtil.Malloc(size);
            UnsafeUtil.CopyBlock(dstPtr, sortedTypes, (uint)size);
            dstArchetype->types = (ComponentType*)dstPtr;
            dstArchetype->typeCount = count;
            // dstArchetype->offsets
            dstArchetype->chunks = new UnsafeList();

            return dstArchetype;
        }

        // 1231223
        public static void InsertSorted(ComponentType* data, int length, ComponentType newValue)
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