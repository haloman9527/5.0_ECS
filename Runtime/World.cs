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
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */
#endregion
using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CZToolKit.ECS
{
    public sealed class World
    {
        public readonly int id;

        private World(int id)
        {
            this.id = id;
        }

        #region Entity
        private readonly IDGenerator EntityIDGenerator = new IDGenerator();
        private readonly NativeHashMap<int, Entity> entities = new NativeHashMap<int, Entity>(64, Allocator.Persistent);
        private readonly HashSet<int> willDeleteEntities = new HashSet<int>();

        public Entity NewEntity()
        {
            var id = EntityIDGenerator.GenerateID();
            var entity = new Entity(id, this);
            entities.Add(id, entity);
            return entity;
        }

        public void NewEntity(out Entity entity)
        {
            var id = EntityIDGenerator.GenerateID();
            entity = new Entity(id, this);
            entities.Add(id, entity);
        }

        public NativeArray<Entity> GetEntities()
        {
            return entities.GetValueArray(Allocator.Temp);
        }

        public bool ContainsEntity(int entityID)
        {
            return entities.ContainsKey(entityID);
        }

        public Entity GetEntity(int entityID)
        {
            return entities[entityID];
        }

        public bool TryGetEntity(int entityID, out Entity entity)
        {
            return entities.TryGetValue(entityID, out entity);
        }

        public void DestroyEntity(int entityID)
        {
            willDeleteEntities.Add(entityID);
        }

        public void DestroyEntity(Entity entity)
        {
            willDeleteEntities.Add(entity.ID);
        }

        public void DestroyEntityImmediate(int entityID)
        {
            if (!entities.Remove(entityID))
                return;
            foreach (var componentPool in componentPools.Values)
            {
                componentPool.Del(entities[entityID]);
            }
        }

        public void DestroyEntityImmediate(Entity entity)
        {
            if (!entities.Remove(entity.ID))
                return;
            foreach (var componentPool in componentPools.Values)
            {
                componentPool.Del(entities[entity.ID]);
            }
        }

        public void CheckDestroyEntities()
        {
            if (willDeleteEntities.Count > 0)
            {
                foreach (var entityID in willDeleteEntities)
                {
                    DestroyEntityImmediate(entityID);
                }
                willDeleteEntities.Clear();
            }
        }
        #endregion

        #region Component
        private readonly Dictionary<Type, IComponentPool> componentPools = new Dictionary<Type, IComponentPool>();

        public IReadOnlyDictionary<Type, IComponentPool> ComponentPools
        {
            get { return componentPools; }
        }

        public IComponentPool<T> NewComponentPool<T>() where T : struct, IComponent
        {
            IComponentPool<T> componentPool = new ComponentPool<T>();
            componentPools[typeof(T)] = componentPool;
            return componentPool;
        }

        public IComponentPool<T> NewComponentPool<T>(int defaultSize) where T : struct, IComponent
        {
            IComponentPool<T> componentPool = new ComponentPool<T>(defaultSize);
            componentPools[typeof(T)] = componentPool;
            return componentPool;
        }

        public IComponentPool NewComponentPool(Type componentType)
        {
            if (!componentType.IsValueType)
                throw new Exception($"The type [{componentType.Name}] is not struct");
            if (componentType.IsAssignableFrom(typeof(IComponent)))
                throw new NotImplementedException($"The type [{componentType.Name}] is not Implement IComponent");

            var componentPoolType = typeof(ComponentPool<>).MakeGenericType(componentType);
            var componentPool = Activator.CreateInstance(componentPoolType) as IComponentPool;
            componentPools[componentType] = componentPool;
            return componentPool;
        }

        public IComponentPool NewComponentPool(Type componentType, int defaultSize)
        {
            if (!componentType.IsValueType)
                throw new Exception($"The type [{componentType.Name}] is not struct");
            if (componentType.IsAssignableFrom(typeof(IComponent)))
                throw new NotImplementedException($"The type [{componentType.Name}] is not Implement IComponent");

            var componentPoolType = typeof(ComponentPool<>).MakeGenericType(componentType);
            var componentPool = Activator.CreateInstance(componentPoolType, defaultSize) as IComponentPool;
            componentPools[componentType] = componentPool;
            return componentPool;
        }

        public bool ContainsComponentPool(Type componentType)
        {
            return componentPools.ContainsKey(componentType);
        }

        public IComponentPool GetComponentPool(Type componentType)
        {
            componentPools.TryGetValue(componentType, out var componentPool);
            return componentPool;
        }

        public IComponentPool<T> GetComponentPool<T>() where T : struct, IComponent
        {
            componentPools.TryGetValue(typeof(T), out var componentPool);
            return componentPool as IComponentPool<T>;
        }

        public bool HasComponent<T>(Entity entity) where T : struct, IComponent
        {
            var componentPool = GetComponentPool<T>();
            if (null != componentPool && componentPool.Contains(entity))
                return true;
            return false;
        }

        public bool HasComponent(Entity entity, Type componentType)
        {
            var componentPool = GetComponentPool(componentType);
            if (null != componentPool && componentPool.Contains(entity))
                return true;
            return false;
        }

        public void AddComponent<T>(Entity entity, in T component) where T : struct, IComponent
        {
            var componentPool = GetComponentPool<T>();
            if (componentPool == null)
                componentPool = NewComponentPool<T>();
            else if (componentPool.Contains(entity))
                throw new Exception($"Alreay had {nameof(T)} component!");
            componentPool.Set(entity, component);
        }

        public void AddComponent(Entity entity, Type type, object component)
        {
            var componentPool = GetComponentPool(type);
            if (componentPool == null)
                componentPool = NewComponentPool(type);
            else if (componentPool.Contains(entity))
                throw new Exception($"Alreay had type component!");
            componentPool.Set(entity, component);
        }

        public ref T RefComponent<T>(Entity entity) where T : struct, IComponent
        {
            return ref GetComponentPool<T>().Ref(entity);
        }

        public void SetComponent<T>(Entity entity, in T component) where T : struct, IComponent
        {
            var componentPool = GetComponentPool<T>();
            if (componentPool == null)
                componentPool = NewComponentPool<T>();
            componentPool.Set(entity, component);
        }

        public void RemoveComponent<T>(Entity entity)
        {
            GetComponentPool(typeof(T)).Del(entity);
        }
        #endregion

        #region System
        private readonly List<ISystem> systems = new List<ISystem>();

        public IReadOnlyList<ISystem> Systems
        {
            get { return systems; }
        }

        public void AddSystem(ISystem system)
        {
            systems.Add(system);
        }

        public void InsertSystem(int index, ISystem system)
        {
            systems.Insert(index, system);
        }

        public void RemoveSystem(ISystem system)
        {
            systems.Remove(system);
        }
        #endregion

        #region Static
        private static readonly IDGenerator WorldIDGenerator = new IDGenerator();
        public static readonly Dictionary<int, World> Worlds = new Dictionary<int, World>();

        public static World NewWorld()
        {
            var id = WorldIDGenerator.GenerateID();
            var world = new World(id);
            Worlds[id] = world;
            return world;
        }
        #endregion
    }
}
