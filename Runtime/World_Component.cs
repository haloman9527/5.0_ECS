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
using System.ComponentModel;
using System.Reflection;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace CZToolKit.ECS
{
    public partial class World
    {
        #region Static

        private static Dictionary<int, Type> m_ComponentTypes = new Dictionary<int, Type>();
        private static Dictionary<int, MethodInfo> SetMethods = new Dictionary<int, MethodInfo>();
        private static Dictionary<int, MethodInfo> GetMethods = new Dictionary<int, MethodInfo>();
        private static Dictionary<int, MethodInfo> TryGetMethods = new Dictionary<int, MethodInfo>();

        public static IReadOnlyDictionary<int, Type> ComponentTypes
        {
            get { return m_ComponentTypes; }
        }

        #endregion

        private NativeHashMap<int, ComponentsContainer> componentContainers =
            new NativeHashMap<int, ComponentsContainer>(128, Allocator.Persistent);

        public NativeHashMap<int, ComponentsContainer> ComponentContainers
        {
            get { return componentContainers; }
        }

        public ComponentsContainer NewComponentContainer<T>() where T : unmanaged, IComponent
        {
            var componentType = typeof(T);
            var componentTypeID = SharedTypeHash<T>.Data;
            var componentSize = SharedTypeSize<T>.Data;
            var componentPool = new ComponentsContainer(componentTypeID, componentSize);
            componentContainers[componentType.GetHashCode()] = componentPool;
            m_ComponentTypes[componentType.GetHashCode()] = componentType;
            return componentPool;
        }

        public ComponentsContainer NewComponentContainer<T>(int defaultCapacity) where T : unmanaged, IComponent
        {
            var componentType = typeof(T);
            var componentTypeID = SharedTypeHash<T>.Data;
            var componentSize = SharedTypeSize<T>.Data;
            var componentPool = new ComponentsContainer(componentTypeID, componentSize, defaultCapacity);
            componentContainers[componentTypeID] = componentPool;
            m_ComponentTypes[componentTypeID] = componentType;
            return componentPool;
        }

        public ComponentsContainer NewComponentContainer(Type componentType)
        {
            if (!UnsafeUtility.IsUnmanaged(componentType))
                throw new Exception($"The type [{componentType.Name}] isn't Unmanaged Type!");
            if (!typeof(IComponent).IsAssignableFrom(componentType))
                throw new NotImplementedException($"The type [{componentType.Name}] isn't implement IComponent!");
            var componentPool = (ComponentsContainer)Activator.CreateInstance(typeof(ComponentsContainer),
                new object[] { componentType, ComponentsContainer.DEFAULT_CAPACITY });
            componentContainers[componentType.GetHashCode()] = componentPool;
            m_ComponentTypes[componentType.GetHashCode()] = componentType;
            return componentPool;
        }

        public ComponentsContainer NewComponentContainer(Type componentType, int defaultSize)
        {
            if (!UnsafeUtility.IsUnmanaged(componentType))
                throw new Exception($"The type [{componentType.Name}] isn't Unmanaged Type!");
            if (!componentType.IsAssignableFrom(typeof(IComponent)))
                throw new NotImplementedException($"The type [{componentType.Name}] isn't implement IComponent!");
            var componentPool = (ComponentsContainer)Activator.CreateInstance(typeof(ComponentsContainer),
                new object[] { componentType, defaultSize });
            componentContainers[componentType.GetHashCode()] = componentPool;
            m_ComponentTypes[componentType.GetHashCode()] = componentType;
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
            return componentContainers[typeof(T).GetHashCode()];
        }

        public ComponentsContainer GetComponentContainer(Type componentType)
        {
            return componentContainers[componentType.GetHashCode()];
        }

        #region Has

        public bool HasComponent(Entity entity, int typeHash)
        {
            if (!componentContainers.TryGetValue(typeHash, out var components))
                return false;
            return components.Contains(entity);
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

        #endregion

        #region Ref

        public ref T RefComponent<T>(Entity entity, int typeHash) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(typeHash, out var components))
                throw new Exception("AAA");
            return ref components.Ref<T>(entity);
        }

        public ref T RefComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(typeof(T).GetHashCode(), out var components))
                throw new Exception("AAA");
            return ref components.Ref<T>(entity);
        }

        #endregion

        #region Get

        public T GetComponent<T>(Entity entity, int typeHash) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(typeHash, out var components))
                throw new Exception("AAA");
            return components.Get<T>(entity);
        }

        public T GetComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(typeof(T).GetHashCode(), out var components))
                throw new Exception("AAA");
            return components.Get<T>(entity);
        }

        public IComponent GetComponent(Entity entity, Type componentType)
        {
            var componentTypeHash = componentType.GetHashCode();
            if (!UnsafeUtility.IsUnmanaged(componentType))
                throw new Exception($"The type '{componentType.Name}' must be a unmanaged type");
            if (!componentContainers.TryGetValue(componentTypeHash, out var components))
                components = NewComponentContainer(componentType);
            if (!GetMethods.TryGetValue(componentTypeHash, out var method))
            {
                var m = typeof(ComponentsContainer).GetMethod("Get", BindingFlags.Public | BindingFlags.Instance);
                GetMethods[componentTypeHash] = method = m.MakeGenericMethod(new Type[] { componentType });
            }

            return (IComponent)method.Invoke(components, new object[] { entity });
        }

        #endregion

        #region TryGet

        public bool TryGetComponent<T>(Entity entity, int typeHash, out T component) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(typeHash, out var components))
            {
                component = default;
                return false;
            }

            return components.TryGet<T>(entity, out component);
        }

        public bool TryGetComponent<T>(Entity entity, out T component) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(typeof(T).GetHashCode(), out var components))
            {
                component = default;
                return false;
            }

            return components.TryGet<T>(entity, out component);
        }

        public bool TryGetComponent(Entity entity, Type componentType, out IComponent component)
        {
            var typeHash = componentType.GetHashCode();
            var componentTypeHash = componentType.GetHashCode();
            if (!UnsafeUtility.IsUnmanaged(componentType))
                throw new Exception($"The type '{componentType.Name}' must be a unmanaged type");
            if (!componentContainers.TryGetValue(componentTypeHash, out var components))
                components = NewComponentContainer(componentType);
            if (!TryGetMethods.TryGetValue(componentTypeHash, out var method))
            {
                var m = typeof(ComponentsContainer).GetMethod("TryGet", BindingFlags.Public | BindingFlags.Instance);
                TryGetMethods[componentTypeHash] = method = m.MakeGenericMethod(new Type[] { componentType });
            }

            var args = new object[] { entity, null };
            var result = (bool)method.Invoke(components, args);
            component = args[1] as IComponent;
            return result;
        }

        #endregion

        #region Set

        public void SetComponent<T>(Entity entity, T component, int typeHash) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(typeHash, out var components))
                components = NewComponentContainer<T>();
            components.Set(entity, component);
        }

        public void SetComponent<T>(Entity entity, T component) where T : unmanaged, IComponent
        {
            SetComponent(entity, component, typeof(T).GetHashCode());
        }

        public void SetComponent(Entity entity, IComponent component)
        {
            var componentType = component.GetType();
            var componentTypeHash = componentType.GetHashCode();
            if (!UnsafeUtility.IsUnmanaged(componentType))
                throw new Exception($"The type '{componentType.Name}' must be a unmanaged type");
            if (!componentContainers.TryGetValue(componentTypeHash, out var components))
                components = NewComponentContainer(componentType);
            if (!SetMethods.TryGetValue(componentTypeHash, out var method))
            {
                var m = typeof(ComponentsContainer).GetMethod("Set", BindingFlags.Public | BindingFlags.Instance);
                SetMethods[componentTypeHash] = method = m.MakeGenericMethod(new Type[] { componentType });
            }

            method.Invoke(components, new object[] { entity, component });
        }

        #endregion

        #region Remove

        public void RemoveComponent(Entity entity, int typeHash)
        {
            if (!componentContainers.TryGetValue(typeHash, out var components))
                return;
            components.Del(entity);
        }

        public void RemoveComponent(Entity entity, Type componentType)
        {
            if (!componentContainers.TryGetValue(componentType.GetHashCode(), out var components))
                return;
            components.Del(entity);
        }

        public void RemoveComponent<T>(Entity entity)
        {
            if (!componentContainers.TryGetValue(typeof(T).GetHashCode(), out var components))
                return;
            components.Del(entity);
        }

        #endregion
    }
}