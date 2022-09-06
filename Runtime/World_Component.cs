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
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace CZToolKit.ECS
{
    public partial class World
    {
        #region Static
        private static Dictionary<int, Type> componentTypes = new Dictionary<int, Type>();
        private static Dictionary<Type, MethodInfo> methods = new Dictionary<Type, MethodInfo>();

        public static IReadOnlyDictionary<int, Type> ComponentTypes
        {
            get { return componentTypes; }
        }
        #endregion

        private NativeHashMap<int, ComponentsContainer> componentContainers = new NativeHashMap<int, ComponentsContainer>(128, Allocator.Persistent);

        public NativeHashMap<int, ComponentsContainer> ComponentContainers
        {
            get { return componentContainers; }
        }

        public ComponentsContainer NewComponentContainer<T>() where T : unmanaged, IComponent
        {
            var componentType = typeof(T);
            var componentPool = new ComponentsContainer(componentType);
            componentContainers[componentType.GetHashCode()] = componentPool;
            componentTypes[componentType.GetHashCode()] = componentType;
            return componentPool;
        }

        public ComponentsContainer NewComponentContainer<T>(int defaultCapacity) where T : unmanaged, IComponent
        {
            var componentType = typeof(T);
            var componentPool = new ComponentsContainer(componentType, defaultCapacity);
            componentContainers[componentType.GetHashCode()] = componentPool;
            componentTypes[componentType.GetHashCode()] = componentType;
            return componentPool;
        }

        public ComponentsContainer NewComponentContainer(Type componentType)
        {
            if (!UnsafeUtility.IsUnmanaged(componentType))
                throw new Exception($"The type [{componentType.Name}] isn't Unmanaged Type!");
            if (!typeof(IComponent).IsAssignableFrom(componentType))
                throw new NotImplementedException($"The type [{componentType.Name}] isn't implement IComponent!");
            var componentPool = (ComponentsContainer)Activator.CreateInstance(typeof(ComponentsContainer), new object[] { componentType, ComponentsContainer.DEFAULT_CAPACITY });
            componentContainers[componentType.GetHashCode()] = componentPool;
            componentTypes[componentType.GetHashCode()] = componentType;
            return componentPool;
        }

        public ComponentsContainer NewComponentContainer(Type componentType, int defaultSize)
        {
            if (!UnsafeUtility.IsUnmanaged(componentType))
                throw new Exception($"The type [{componentType.Name}] isn't Unmanaged Type!");
            if (!componentType.IsAssignableFrom(typeof(IComponent)))
                throw new NotImplementedException($"The type [{componentType.Name}] isn't implement IComponent!");
            var componentPool = (ComponentsContainer)Activator.CreateInstance(typeof(ComponentsContainer), new object[] { componentType, defaultSize });
            componentContainers[componentType.GetHashCode()] = componentPool;
            componentTypes[componentType.GetHashCode()] = componentType;
            return componentPool;
        }

        public bool ExistsComponentContainer<T>() where T : unmanaged, IComponent
        {
            return ExistsComponentContainer(typeof(T));
        }

        public bool ExistsComponentContainer(Type componentType)
        {
            return componentContainers.ContainsKey(componentType.GetHashCode());
        }

        public ComponentsContainer GetComponentContainer<T>()
        {
            return GetComponentContainer(typeof(T));
        }

        public ComponentsContainer GetComponentContainer(Type componentType)
        {
            return componentContainers[componentType.GetHashCode()];
        }

        public bool HasComponent(Entity entity, Type componentType)
        {
            if (!componentContainers.TryGetValue(componentType.GetHashCode(), out var components))
                return false;
            return components.Contains(entity);
        }

        public bool HasComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            return HasComponent(entity, typeof(T));
        }

        public ref T GetComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            var componentType = typeof(T);
            if (!componentContainers.TryGetValue(componentType.GetHashCode(), out var components))
                throw new Exception("AAA");
            return ref components.Get<T>(entity);
        }

        public void SetComponent<T>(Entity entity, T component) where T : unmanaged, IComponent
        {
            var componentType = typeof(T);
            if (!componentContainers.TryGetValue(componentType.GetHashCode(), out var components))
                components = NewComponentContainer<T>();
            components.Set(entity, component);
        }

        public void SetComponent(Entity entity, IComponent component)
        {
            var componentType = component.GetType();
            if (!UnsafeUtility.IsUnmanaged(componentType))
                throw new Exception($"The type '{componentType.Name}' must be a unmanaged type");
            if (!componentContainers.TryGetValue(componentType.GetHashCode(), out var components))
                components = NewComponentContainer(componentType);
            if (!methods.TryGetValue(componentType, out var method))
            {
                var m = typeof(ComponentsContainer).GetMethod("Set", BindingFlags.Public | BindingFlags.Instance);
                methods[componentType] = method = m.MakeGenericMethod(new Type[] { component.GetType() });
            }
            method.Invoke(components, new object[] { entity, component });
        }

        public void RemoveComponent(Entity entity, Type componentType)
        {
            if (!componentContainers.TryGetValue(componentType.GetHashCode(), out var components))
                return;
            components.Del(entity);
        }

        public void RemoveComponent<T>(Entity entity)
        {
            RemoveComponent(entity, typeof(T));
        }
    }
}
