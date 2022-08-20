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
using Unity.Collections.LowLevel.Unsafe;

namespace CZToolKit.ECS
{
    public partial class World : IDisposable
    {
        #region Static
        private static Dictionary<int, Type> componentTypes = new Dictionary<int, Type>();

        public static IReadOnlyDictionary<int, Type> ComponentTypes
        {
            get { return componentTypes; }
        }
        #endregion

        private NativeHashMap<int, ComponentPool> componentPools = new NativeHashMap<int, ComponentPool>(128, Allocator.Persistent);

        public NativeHashMap<int, ComponentPool> ComponentPools
        {
            get { return componentPools; }
        }

        public unsafe ComponentPool NewComponentPool<T>() where T : unmanaged, IComponent
        {
            var componentType = typeof(T);
            var componentPool = new ComponentPool(componentType);
            componentPools[componentType.GetHashCode()] = componentPool;
            componentTypes[componentType.GetHashCode()] = componentType;
            return componentPool;
        }

        public unsafe ComponentPool NewComponentPool<T>(int defaultCapacity) where T : unmanaged, IComponent
        {
            var componentType = typeof(T);
            var componentPool = new ComponentPool(componentType, defaultCapacity);
            componentPools[componentType.GetHashCode()] = componentPool;
            componentTypes[componentType.GetHashCode()] = componentType;
            return componentPool;
        }

        public unsafe ComponentPool NewComponentPool(Type componentType)
        {
            if (!UnsafeUtility.IsUnmanaged(componentType))
                throw new Exception($"The type [{componentType.Name}] is'n Unmanaged Type!");
            if (!componentType.IsAssignableFrom(typeof(IComponent)))
                throw new NotImplementedException($"The type [{componentType.Name}] is'n implement IComponent!");
            var componentPool = (ComponentPool)Activator.CreateInstance(typeof(ComponentPool), new object[] { componentType });
            componentPools[componentType.GetHashCode()] = componentPool;
            componentTypes[componentType.GetHashCode()] = componentType;
            return componentPool;
        }

        public unsafe ComponentPool NewComponentPool(Type componentType, int defaultSize)
        {
            if (!UnsafeUtility.IsUnmanaged(componentType))
                throw new Exception($"The type [{componentType.Name}] is'n Unmanaged Type!");
            if (!componentType.IsAssignableFrom(typeof(IComponent)))
                throw new NotImplementedException($"The type [{componentType.Name}] is'n implement IComponent!");
            var componentPool = (ComponentPool)Activator.CreateInstance(typeof(ComponentPool), new object[] { componentType, defaultSize });
            componentPools[componentType.GetHashCode()] = componentPool;
            componentTypes[componentType.GetHashCode()] = componentType;
            return componentPool;
        }

        public bool ExistsComponentPool(Type componentType)
        {
            return componentPools.ContainsKey(componentType.GetHashCode());
        }

        public ComponentPool GetComponentPool<T>()
        {
            return GetComponentPool(typeof(T));
        }

        public ComponentPool GetComponentPool(Type componentType)
        {
            return componentPools[componentType.GetHashCode()];
        }

        public bool HasComponent(Entity entity, Type componentType)
        {
            if (!componentPools.TryGetValue(componentType.GetHashCode(), out var components))
                return false;
            return components.Contains(entity);
        }

        public bool HasComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            return HasComponent(entity, typeof(T));
        }

        public unsafe T* GetComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            var componentType = typeof(T);
            if (!componentPools.TryGetValue(componentType.GetHashCode(), out var components))
                throw new Exception("AAA");
            return components.Get<T>(entity);
        }

        public void SetComponent<T>(Entity entity, T component) where T : unmanaged, IComponent
        {
            var componentType = typeof(T);
            if (!componentPools.TryGetValue(componentType.GetHashCode(), out var components))
                components = NewComponentPool<T>();
            components.Set(entity, component);
        }

        public void SetComponent(Entity entity, IComponent component)
        {
            var componentType = component.GetType();
            if (!componentPools.TryGetValue(componentType.GetHashCode(), out var components))
                components = NewComponentPool(componentType);
            components.Set(entity, component);
        }

        public void RemoveComponent(Entity entity, Type componentType)
        {
            if (!componentPools.TryGetValue(componentType.GetHashCode(), out var components))
                return;
            components.Del(entity);
        }

        public void RemoveComponent<T>(Entity entity)
        {
            RemoveComponent(entity, typeof(T));
        }
    }
}
