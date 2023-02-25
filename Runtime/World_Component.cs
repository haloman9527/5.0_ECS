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

namespace CZToolKit.ECS
{
    public partial class World
    {
        #region Const
        
        #endregion
        
        #region Static

        private static Dictionary<int, MethodInfo> SetMethods = new Dictionary<int, MethodInfo>();
        private static Dictionary<int, MethodInfo> GetMethods = new Dictionary<int, MethodInfo>();
        private static Dictionary<int, MethodInfo> TryGetMethods = new Dictionary<int, MethodInfo>();
        
        #endregion

        private NativeHashMap<int, ComponentsContainer> componentContainers =
            new NativeHashMap<int, ComponentsContainer>(128, Allocator.Persistent);

        public NativeHashMap<int, ComponentsContainer> ComponentContainers
        {
            get { return componentContainers; }
        }

        public ComponentsContainer NewComponentContainer<T>(int capacity) where T : unmanaged, IComponent
        {
            TypeInfo typeInfo = TypeManager.GetTypeInfo<T>();
            var componentContainer = new ComponentsContainer(typeInfo.typeIndex, typeInfo.componentSize, capacity);
            componentContainers[typeInfo.typeIndex] = componentContainer;
            return componentContainer;
        }

        public ComponentsContainer NewComponentContainer<T>() where T : unmanaged, IComponent
        {
            return NewComponentContainer<T>(ComponentsContainer.DEFAULT_CAPACITY);
        }

        public ComponentsContainer NewComponentContainer(Type componentType, int defaultSize)
        {
            var typeIndex = TypeManager.FindTypeIndex(componentType);
            if (typeIndex == -1)
                throw new Exception($"The type [{componentType.Name}] is unsupported Type!");
            
            var typeInfo = TypeManager.GetTypeInfo(typeIndex);
            var componentContainer = (ComponentsContainer)Activator.CreateInstance(typeof(ComponentsContainer),
                new object[] { typeInfo.typeIndex, typeInfo.componentSize, defaultSize });
            componentContainers[typeIndex] = componentContainer;
            return componentContainer;
        }

        public ComponentsContainer NewComponentContainer(Type componentType)
        {
            return NewComponentContainer(componentType, ComponentsContainer.DEFAULT_CAPACITY);
        }

        public bool ExistsComponentContainer<T>() where T : unmanaged, IComponent
        {
            return componentContainers.ContainsKey(SharedTypeIndex<T>.Data);
        }

        public bool ExistsComponentContainer(Type componentType)
        {
            return componentContainers.ContainsKey(TypeManager.FindTypeIndex(componentType));
        }

        public ComponentsContainer GetComponentContainer<T>()
        {
            return componentContainers[SharedTypeIndex<T>.Data];
        }

        public ComponentsContainer GetComponentContainer(Type componentType)
        {
            return componentContainers[TypeManager.FindTypeIndex(componentType)];
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
            if (!componentContainers.TryGetValue(TypeManager.FindTypeIndex(componentType), out var components))
                return false;
            return components.Contains(entity);
        }

        public bool HasComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            return HasComponent(entity, typeof(T));
        }

        #endregion

        #region Ref

        public ref T RefComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(SharedTypeIndex<T>.Data, out var components))
                throw new Exception();
            return ref components.Ref<T>(entity);
        }

        #endregion

        #region Get

        public T GetComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(SharedTypeIndex<T>.Data, out var components))
                throw new Exception();
            return components.Get<T>(entity);
        }

        public IComponent GetComponent(Entity entity, Type componentType)
        {
            var typeIndex = TypeManager.FindTypeIndex(componentType);
            if (typeIndex == -1)
                throw new Exception($"The type [{componentType.Name}] is unsupported Type!");
            
            if (!componentContainers.TryGetValue(typeIndex, out var components))
                components = NewComponentContainer(componentType);
            if (!GetMethods.TryGetValue(typeIndex, out var method))
            {
                var m = typeof(ComponentsContainer).GetMethod("Get", BindingFlags.Public | BindingFlags.Instance);
                GetMethods[typeIndex] = method = m.MakeGenericMethod(new Type[] { componentType });
            }

            return (IComponent)method.Invoke(components, new object[] { entity });
        }

        #endregion

        #region TryGet

        public bool TryGetComponent<T>(Entity entity, out T component) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(SharedTypeIndex<T>.Data, out var components))
            {
                component = default;
                return false;
            }

            return components.TryGet(entity, out component);
        }

        public bool TryGetComponent(Entity entity, Type componentType, out IComponent component)
        {
            var typeIndex = TypeManager.FindTypeIndex(componentType);
            if (typeIndex == -1)
                throw new Exception($"The type [{componentType.Name}] is unsupported Type!");
            
            if (!componentContainers.TryGetValue(typeIndex, out var components))
                components = NewComponentContainer(componentType);
            if (!TryGetMethods.TryGetValue(typeIndex, out var method))
            {
                var m = typeof(ComponentsContainer).GetMethod("TryGet", BindingFlags.Public | BindingFlags.Instance);
                TryGetMethods[typeIndex] = method = m.MakeGenericMethod(new Type[] { componentType });
            }

            var args = new object[] { entity, null };
            var result = (bool)method.Invoke(components, args);
            component = args[1] as IComponent;
            return result;
        }

        #endregion

        #region Set

        public void SetComponent<T>(Entity entity, T component) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(SharedTypeIndex<T>.Data, out var components))
                components = NewComponentContainer<T>();
            components.Set(entity, component);
        }

        public void SetComponent(Entity entity, IComponent component)
        {
            var componentType = component.GetType();
            var typeIndex = TypeManager.FindTypeIndex(componentType);
            if (typeIndex == -1)
                throw new Exception($"The type [{componentType.Name}] is unsupported Type!");
            
            if (!componentContainers.TryGetValue(typeIndex, out var components))
                components = NewComponentContainer(componentType);
            if (!SetMethods.TryGetValue(typeIndex, out var method))
            {
                var m = typeof(ComponentsContainer).GetMethod("Set", BindingFlags.Public | BindingFlags.Instance);
                SetMethods[typeIndex] = method = m.MakeGenericMethod(new Type[] { componentType });
            }

            method.Invoke(components, new object[] { entity, component });
        }

        #endregion

        #region Remove

        public void RemoveComponent(Entity entity, Type componentType)
        {
            if (!componentContainers.TryGetValue(TypeManager.FindTypeIndex(componentType), out var components))
                return;
            components.Del(entity);
        }

        public void RemoveComponent<T>(Entity entity)
        {
            if (!componentContainers.TryGetValue(SharedTypeIndex<T>.Data, out var components))
                return;
            components.Del(entity);
        }

        #endregion
    }
}