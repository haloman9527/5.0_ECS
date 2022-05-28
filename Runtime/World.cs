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
using System.Linq;
using Unity.Collections;

namespace CZToolKit.ECS
{
    public partial class World : IDisposable
    {
        #region Static
        private static readonly IDGenerator worldIDGenerator = new IDGenerator();
        private static readonly Dictionary<int, World> allWorlds = new Dictionary<int, World>();

        public static IReadOnlyDictionary<int, World> AllWorlds
        {
            get { return allWorlds; }
        }

        public static World DefaultWorld
        {
            get;
            set;
        }

        public static void DisposeWorld(World world)
        {
            world.Dispose();
        }

        public static void DisposeAllWorld()
        {
            var allWorlds = AllWorlds.Values.ToArray();
            foreach (var world in allWorlds)
            {
                world.Dispose();
            }
        }
        #endregion

        public readonly int index;
        public readonly string name;
        public readonly Entity singleton;

        public World(string name)
        {
            this.index = worldIDGenerator.GenerateID();
            this.name = name;
            this.singleton = NewEntity(-1);
            allWorlds[index] = this;
        }

        public void Dispose()
        {
            allWorlds.Remove(index);
            if (DefaultWorld == this)
                DefaultWorld = null;
            systems.Clear();
            entities.Dispose();
            foreach (var componentPool in componentPools.Values)
            {
                componentPool.Dispose();
            }
        }

        #region Entity
        private IDGenerator entityIndexGenerator = new IDGenerator();
        private NativeHashMap<int, Entity> entities = new NativeHashMap<int, Entity>(64, Allocator.Persistent);

        public ref readonly NativeHashMap<int, Entity> Entities
        {
            get { return ref entities; }
        }

        private Entity NewEntity(int index)
        {
            var entity = new Entity(index, this);
            entities.Add(index, entity);
            return entity;
        }

        public Entity NewEntity()
        {
            var index = entityIndexGenerator.GenerateID();
            var entity = new Entity(index, this);
            entities.Add(index, entity);
            return entity;
        }

        public void NewEntity(out Entity entity)
        {
            var id = entityIndexGenerator.GenerateID();
            entity = new Entity(id, this);
            entities.Add(id, entity);
        }

        public bool IsValid(int entityID)
        {
            return entities.ContainsKey(entityID);
        }

        public unsafe bool IsValid(Entity entity)
        {
            return entity.worldIndex == index && entities.ContainsKey(entity.index);
        }

        public unsafe void DestroyEntityImmediate(int entityID)
        {
            DestroyEntityImmediate(entities[entityID]);
        }

        public unsafe void DestroyEntityImmediate(Entity entity)
        {
            if (entity.worldIndex != index)
                return;
            entities.Remove(entity.index);
            foreach (var componentPool in componentPools.Values)
            {
                componentPool.Del(entity);
            }
        }
        #endregion

        #region Component
        private readonly Dictionary<Type, IComponentPool> componentPools = new Dictionary<Type, IComponentPool>();

        public IReadOnlyDictionary<Type, IComponentPool> ComponentPools
        {
            get { return componentPools; }
        }

        public ComponentPool<T> NewComponentPool<T>() where T : struct, IComponent
        {
            ComponentPool<T> componentPool = new ComponentPool<T>();
            componentPools[typeof(T)] = componentPool;
            return componentPool;
        }

        public ComponentPool<T> NewComponentPool<T>(int defaultSize) where T : struct, IComponent
        {
            ComponentPool<T> componentPool = new ComponentPool<T>(defaultSize);
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

        public ComponentPool<T> GetComponentPool<T>() where T : struct, IComponent
        {
            componentPools.TryGetValue(typeof(T), out var componentPool);
            return componentPool as ComponentPool<T>;
        }

        public unsafe bool HasComponent(Entity entity, Type componentType)
        {
            var componentPool = GetComponentPool(componentType);
            if (null != componentPool && componentPool.Contains(entity))
                return true;
            return false;
        }

        public unsafe bool HasComponent<T>(Entity entity) where T : struct, IComponent
        {
            var componentPool = GetComponentPool<T>();
            if (null != componentPool && componentPool.Contains(entity))
                return true;
            return false;
        }

        public unsafe T GetComponent<T>(Entity entity) where T : struct, IComponent
        {
            return GetComponentPool<T>().Get(entity);
        }

        public unsafe bool TryGetComponent<T>(Entity entity, out T component) where T : struct, IComponent
        {
            var componentPool = GetComponentPool<T>();
            if (componentPool == null)
            {
                component = default;
                return false;
            }
            return componentPool.TryGet(entity, out component);
        }

        public unsafe ref T RefComponent<T>(Entity entity) where T : struct, IComponent
        {
            return ref GetComponentPool<T>().Ref(entity);
        }

        public unsafe void AddComponent<T>(Entity entity, in T component) where T : struct, IComponent
        {
            var componentPool = GetComponentPool<T>();
            if (componentPool == null)
                componentPool = NewComponentPool<T>();
            componentPool.Set(entity, component);
        }

        public unsafe void AddComponent(Entity entity, Type type, object component)
        {
            var componentPool = GetComponentPool(type);
            if (componentPool == null)
                componentPool = NewComponentPool(type);
            componentPool.Set(entity, component);
        }

        public unsafe void SetComponent<T>(Entity entity, in T component) where T : struct, IComponent
        {
            var componentPool = GetComponentPool<T>();
            if (componentPool == null)
                componentPool = NewComponentPool<T>();
            componentPool.Set(entity, component);
        }

        public unsafe void RemoveComponent<T>(Entity entity)
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

        public bool RemoveSystem(ISystem system)
        {
            return systems.Remove(system);
        }
        #endregion
    }
}
