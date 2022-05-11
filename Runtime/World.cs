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
    public sealed class World : IDisposable
    {
        public readonly int id;
        public readonly string name;
        public readonly Entity singleton;

        public World(string name)
        {
            this.id = worldIDGenerator.GenerateID();
            this.name = name;
            this.singleton = NewEntity(-1);
            worlds[id] = this;
        }

        public void Dispose()
        {
            DisposeWorld(this);
        }

        #region Entity
        private IDGenerator entityIDGenerator = new IDGenerator();
        private NativeHashMap<int, Entity> entities = new NativeHashMap<int, Entity>(64, Allocator.Persistent);

        public ref readonly NativeHashMap<int, Entity> Entities
        {
            get { return ref entities; }
        }

        private Entity NewEntity(int id)
        {
            var entity = new Entity(id, this);
            entities.Add(id, entity);
            return entity;
        }

        public Entity NewEntity()
        {
            var id = entityIDGenerator.GenerateID();
            var entity = new Entity(id, this);
            entities.Add(id, entity);
            return entity;
        }

        public void NewEntity(out Entity entity)
        {
            var id = entityIDGenerator.GenerateID();
            entity = new Entity(id, this);
            entities.Add(id, entity);
        }

        public bool IsValid(int entityID)
        {
            return entities.ContainsKey(entityID);
        }

        public unsafe bool IsValid(Entity* entityPtr)
        {
            return entityPtr->WorldID == id && entities.ContainsKey(entityPtr->ID);
        }

        public unsafe void DestroyEntityImmediate(int entityID)
        {
            DestroyEntityImmediate(entities[entityID]);
        }

        public unsafe void DestroyEntityImmediate(Entity entity)
        {
            if (entity.WorldID != id)
                return;
            entities.Remove(entity.ID);
            foreach (var componentPool in componentPools.Values)
            {
                componentPool.Del(&entity);
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

        public unsafe bool HasComponent(Entity* entityPtr, Type componentType)
        {
            var componentPool = GetComponentPool(componentType);
            if (null != componentPool && componentPool.Contains(entityPtr))
                return true;
            return false;
        }

        public unsafe bool HasComponent<T>(Entity* entityPtr) where T : struct, IComponent
        {
            var componentPool = GetComponentPool<T>();
            if (null != componentPool && componentPool.Contains(entityPtr))
                return true;
            return false;
        }

        public unsafe T GetComponent<T>(Entity* entityPtr) where T : struct, IComponent
        {
            return GetComponentPool<T>().Get(entityPtr);
        }

        public unsafe bool TryGetComponent<T>(Entity* entityPtr, out T component) where T : struct, IComponent
        {
            component = default;
            var componentPool = GetComponentPool<T>();
            if (componentPool == null)
                return false;
            return componentPool.TryGet(entityPtr, out component);
        }

        public unsafe ref T RefComponent<T>(Entity* entityPtr) where T : struct, IComponent
        {
            return ref GetComponentPool<T>().Ref(entityPtr);
        }

        public unsafe void AddComponent<T>(Entity* entityPtr, in T component) where T : struct, IComponent
        {
            var componentPool = GetComponentPool<T>();
            if (componentPool == null)
                componentPool = NewComponentPool<T>();
            componentPool.Set(entityPtr, component);
        }

        public unsafe void AddComponent(Entity* entityPtr, Type type, object component)
        {
            var componentPool = GetComponentPool(type);
            if (componentPool == null)
                componentPool = NewComponentPool(type);
            componentPool.Set(entityPtr, component);
        }

        public unsafe void SetComponent<T>(Entity* entityPtr, in T component) where T : struct, IComponent
        {
            var componentPool = GetComponentPool<T>();
            if (componentPool == null)
                componentPool = NewComponentPool<T>();
            componentPool.Set(entityPtr, component);
        }

        public unsafe void RemoveComponent<T>(Entity* entityPtr)
        {
            GetComponentPool(typeof(T)).Del(entityPtr);
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
        private static readonly IDGenerator worldIDGenerator = new IDGenerator();
        private static readonly Dictionary<int, World> worlds = new Dictionary<int, World>();

        public static IReadOnlyDictionary<int, World> Worlds
        {
            get { return worlds; }
        }
        public static World DefaultWorld
        {
            get;
            set;
        }

        public static void DisposeWorld(World world)
        {
            foreach (var componentPool in world.componentPools.Values)
            {
                componentPool.Dispose();
            }
            world.entities.Dispose();
            worlds.Remove(world.id);
        }

        public static void DisposeAllWorld()
        {
            foreach (var world in Worlds.Values)
            {
                foreach (var componentPool in world.componentPools.Values)
                {
                    componentPool.Dispose();
                }
                world.entities.Dispose();
            }
            worlds.Clear();
        }
        #endregion
    }
}
