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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;

namespace CZToolKit.ECS
{
    public partial class World : IDisposable
    {
        #region Static
        private static readonly List<World> allWorlds = new List<World>();

        public static IReadOnlyList<World> AllWorlds
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
            foreach (var world in allWorlds)
            {
                world.Dispose();
            }
            allWorlds.Clear();
        }
        #endregion

        public readonly string name;
        public readonly Entity singleton;

        public World(string name)
        {
            this.name = name;
            this.singleton = NewEntity(-1);
            if (DefaultWorld == null)
                DefaultWorld = this;
            allWorlds.Add(this);
        }

        public unsafe void Dispose()
        {
            systems.Clear();
            entities.Dispose();
            foreach (var poolPtr in componentPools.GetValueArray(Allocator.Temp))
            {
                ref var componentPool = ref Unsafe.AsRef<ComponentPool>((void*)poolPtr);
                componentPool.Dispose();
            }
            componentPools.Clear();
            if (DefaultWorld == this)
                DefaultWorld = null;
            allWorlds.Remove(this);
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
            var entity = new Entity(index);
            entities.Add(index, entity);
            return entity;
        }

        public Entity NewEntity()
        {
            var index = entityIndexGenerator.GenerateID();
            var entity = new Entity(index);
            entities.Add(index, entity);
            return entity;
        }

        public void NewEntity(out Entity entity)
        {
            var id = entityIndexGenerator.GenerateID();
            entity = new Entity(id);
            entities.Add(id, entity);
        }

        public bool Exists(int entityID)
        {
            return entities.ContainsKey(entityID);
        }

        public bool Exists(Entity entity)
        {
            return entities.ContainsKey(entity.index);
        }

        public void DestroyEntityImmediate(int entityID)
        {
            DestroyEntityImmediate(entities[entityID]);
        }

        public unsafe void DestroyEntityImmediate(Entity entity)
        {
            entities.Remove(entity.index);
            foreach (var poolPtr in componentPools.GetValueArray(Allocator.Temp))
            {
                ref var componentPool = ref Unsafe.AsRef<ComponentPool>((void*)poolPtr);
                componentPool.Del(entity);
            }
        }
        #endregion

        #region Component
        private NativeHashMap<int, IntPtr> componentPools = new NativeHashMap<int, IntPtr>(128, Allocator.Persistent);

        public ref NativeHashMap<int, IntPtr> ComponentPools
        {
            get { return ref componentPools; }
        }

        public unsafe ComponentPool* NewComponentPool<T>() where T : unmanaged, IComponent
        {
            var componentType = typeof(T);
            var componentPool = new ComponentPool(componentType);
            var componentsPointer = &componentPool;
            componentPools[componentType.GetHashCode()] = new IntPtr(componentsPointer);
            return componentsPointer;
        }

        public unsafe ComponentPool* NewComponentPool<T>(int defaultCapacity) where T : unmanaged, IComponent
        {
            var componentType = typeof(T);
            var componentPool = new ComponentPool(componentType, defaultCapacity);
            var componentsPointer = &componentPool;
            componentPools[componentType.GetHashCode()] = new IntPtr(componentsPointer);
            return componentsPointer;
        }

        public unsafe ComponentPool* NewComponentPool(Type componentType)
        {
            if (!UnsafeUtil.IsUnManaged(componentType))
                throw new Exception($"The type [{componentType.Name}] is not UnManaged Type");
            if (!componentType.IsAssignableFrom(typeof(IComponent)))
                throw new NotImplementedException($"The type [{componentType.Name}] is not Implement IComponent");
            var componentPool = (ComponentPool)Activator.CreateInstance(typeof(ComponentPool), new object[] { componentType });
            var componentsPointer = &componentPool;
            componentPools[componentType.GetHashCode()] = new IntPtr(componentsPointer);
            return componentsPointer;
        }

        public unsafe ComponentPool* NewComponentPool(Type componentType, int defaultSize)
        {
            if (!UnsafeUtil.IsUnManaged(componentType))
                throw new Exception($"The type [{componentType.Name}] is not UnManaged Type");
            if (!componentType.IsAssignableFrom(typeof(IComponent)))
                throw new NotImplementedException($"The type [{componentType.Name}] is not Implement IComponent");
            var componentPool = (ComponentPool)Activator.CreateInstance(typeof(ComponentPool), new object[] { componentType, defaultSize });
            var componentsPointer = &componentPool;
            componentPools[componentType.GetHashCode()] = new IntPtr(componentsPointer);
            return componentsPointer;
        }

        public bool ExistsComponentPool(Type componentType)
        {
            return componentPools.ContainsKey(componentType.GetHashCode());
        }

        public unsafe ComponentPool* GetComponentPool(Type componentType)
        {
            if (!componentPools.TryGetValue(componentType.GetHashCode(), out var poolPtr))
                throw new Exception("AAA");
            return (ComponentPool*)poolPtr;
        }

        public unsafe bool HasComponent(Entity entity, Type componentType)
        {
            if (!componentPools.TryGetValue(componentType.GetHashCode(), out var poolPtr))
                return false;
            var components = (ComponentPool*)poolPtr;
            return components->Contains(entity);
        }

        public unsafe bool HasComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            return HasComponent(entity, typeof(T));
        }

        public unsafe T* GetComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            var componentType = typeof(T);
            if (!componentPools.TryGetValue(componentType.GetHashCode(), out var poolPtr))
                throw new Exception("AAA");
            var components = (ComponentPool*)poolPtr;
            return components->Get<T>(entity);
        }

        public unsafe void SetComponent<T>(Entity entity, T component) where T : unmanaged, IComponent
        {
            var componentType = typeof(T);
            ComponentPool* components;
            if (componentPools.TryGetValue(componentType.GetHashCode(), out var poolPtr))
                components = (ComponentPool*)poolPtr;
            else
                components = NewComponentPool<T>();
            components->Set(entity, component);
        }

        public unsafe void SetComponent(Entity entity, IComponent component)
        {
            var componentType = component.GetType();
            ComponentPool* components;
            if (componentPools.TryGetValue(componentType.GetHashCode(), out var poolPtr))
                components = (ComponentPool*)poolPtr;
            else
                components = NewComponentPool(componentType);
            components->Set(entity, component);
        }

        public unsafe void RemoveComponent(Entity entity, Type componentType)
        {
            if (!componentPools.TryGetValue(componentType.GetHashCode(), out var poolPtr))
                return;
            var components = (ComponentPool*)poolPtr;
            components->Del(entity);
        }

        public unsafe void RemoveComponent<T>(Entity entity)
        {

            RemoveComponent(entity, typeof(T));
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
            if (systems.Contains(system))
                throw new Exception("systems中已经存在一个相同的对象!");
            systems.Add(system);
        }

        public void InsertSystem(int index, ISystem system)
        {
            if (systems.Contains(system))
                throw new Exception("systems中已经存在一个相同的对象!");
            systems.Insert(index, system);
            if (system is IOnAwake sys)
                sys.OnAwake();
        }

        public bool RemoveSystem(ISystem system)
        {
            return systems.Remove(system);
        }

        public void FixedUpdate()
        {
            for (int i = 0; i < systems.Count; i++)
            {
                var system = systems[i];
                if (system is IFixedUpdate sys)
                    sys.OnFixedUpdate();
            }
        }

        public void Update()
        {
            for (int i = 0; i < systems.Count; i++)
            {
                var system = systems[i];
                if (system is IUpdate sys)
                    sys.OnUpdate();
            }
        }

        public void LateUpdate()
        {
            for (int i = 0; i < systems.Count; i++)
            {
                var system = systems[i];
                if (system is ILateUpdate sys)
                    sys.OnLateUpdate();
            }
        }

        public void DestroySystem(ISystem system)
        {
            if (!systems.Contains(system))
                throw new Exception("systems中不存在该对象");
            if (system is IDestroy sys)
                sys.OnDestroy();
            RemoveSystem(system);
        }
        #endregion
    }
}
