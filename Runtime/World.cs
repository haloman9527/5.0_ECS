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

namespace CZToolKit.ECS
{
    public sealed class World
    {
        #region Entity
        private readonly IDGenerator entityIDGenerator = new IDGenerator();
        private readonly Dictionary<int, Entity> entities = new Dictionary<int, Entity>();

        public IEnumerable<Entity> Entities
        {
            get { return entities.Values; }
        }

        public Entity NewEntity()
        {
            var entity = new Entity(entityIDGenerator.GenerateID(), this);
            entities[entity.ID] = entity;
            return entity;
        }

        public void NewEntity(out Entity entity)
        {
            entity = new Entity(entityIDGenerator.GenerateID(), this);
            entities[entity.ID] = entity;
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

        public void DelEntity(int entityID)
        {
            if (!entities.Remove(entityID))
                return;
            foreach (var componentPool in componentPools.Values)
            {
                componentPool.Del(entityID);
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
        #endregion

        #region System
        private readonly List<ISystem> systems = new List<ISystem>();

        public IReadOnlyList<ISystem> Systems
        {
            get { return systems; }
        }


        public void FixedUpdate()
        {
            foreach (var system in systems)
            {
                if (system is IFixedUpdateSystem sys)
                    sys.OnFixedUpdate();
            }
        }

        public void Update()
        {
            foreach (var system in systems)
            {
                if (system is IUpdateSystem sys)
                    sys.OnUpdate();
            }
        }

        public void LateUpdate()
        {
            foreach (var system in systems)
            {
                if (system is ILateUpdateSystem sys)
                    sys.OnLateUpdate();
            }
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

        #region Define
        public class IDGenerator
        {
            int id = 1;

            public int GenerateID()
            {
                return id++;
            }
        }
        #endregion
    }
}
