using System;
using UnityEngine;

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
            fixed(ComponentType* typesPtr = types)
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

        public int FillSortedArchetypeArray(ComponentType* dst, ComponentType* src, int count)
        {
            return 0;
        }
        
        public Archetype* GetExsitingArchetype(ComponentType* sortedTypes, int count)
        {
            return null;
        }
        
        public Archetype* CreateArchetype(ComponentType* sortedTypes, int count)
        {
            return null;
        }
    }
}
