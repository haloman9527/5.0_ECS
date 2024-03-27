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
 *  Github: https://github.com/haloman9527
 *  Blog: https://www.haloman.net/
 *
 */

#endregion

using System;
using CZToolKit.UnsafeEx;
using Unity.Collections;

namespace CZToolKit.ECS
{
    public partial class World
    {
        public readonly ECSReferences references = new ECSReferences();

        private NativeParallelHashMap<int, ComponentsContainer> componentContainers =
            new NativeParallelHashMap<int, ComponentsContainer>(128, Allocator.Persistent);

        private NativeParallelHashMap<int, ComponentsContainer> ComponentContainers
        {
            get { return componentContainers; }
        }

        private ComponentsContainer NewComponentContainer<T>(int capacity) where T : unmanaged, IComponent
        {
            var typeInfo = TypeManager.GetTypeInfo<T>();
            if (!componentContainers.TryGetValue(typeInfo.id, out var container))
            {
                container = new ComponentsContainer(typeInfo, capacity);
                componentContainers[typeInfo.id] = container;
            }

            return container;
        }

        private ComponentsContainer NewComponentContainer<T>() where T : unmanaged, IComponent
        {
            return NewComponentContainer<T>(128);
        }

        public bool ExistsComponentContainer<T>() where T : unmanaged, IComponent
        {
            return componentContainers.ContainsKey(TypeInfo<T>.Id);
        }

        public bool ExistsComponentContainer(Type componentType)
        {
            return componentContainers.ContainsKey(TypeManager.GetTypeId(componentType));
        }

        public ComponentsContainer GetComponentContainer<T>()
        {
            return componentContainers[TypeInfo<T>.Id];
        }

        public ComponentsContainer GetComponentContainer(Type componentType)
        {
            return componentContainers[TypeManager.GetTypeId(componentType)];
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
            if (!componentContainers.TryGetValue(TypeManager.GetTypeId(componentType), out var components))
                return false;
            return components.Contains(entity);
        }

        public bool HasComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            return HasComponent(entity, typeof(T));
        }

        #endregion

        #region Get

        public T GetComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(TypeInfo<T>.Id, out var components))
                throw new Exception();
            return components.Get<T>(entity);
        }

        public ref T RefComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(TypeInfo<T>.Id, out var components))
                throw new Exception();
            return ref components.Ref<T>(entity);
        }

        public bool TryGetComponent<T>(Entity entity, out T component) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(TypeInfo<T>.Id, out var components))
            {
                component = default;
                return false;
            }

            return components.TryGet(entity, out component);
        }

        #endregion

        #region Set

        public void SetComponent<T>(Entity entity, ref T component) where T : unmanaged, IComponent
        {
            var typeInfo = TypeManager.GetTypeInfo<T>();
            if (!componentContainers.TryGetValue(typeInfo.id, out var components))
                components = NewComponentContainer<T>();
            components.Set(entity, ref component);
        }

        public void SetComponent<T>(Entity entity, T component) where T : unmanaged, IComponent
        {
            var typeInfo = TypeManager.GetTypeInfo<T>();
            if (!componentContainers.TryGetValue(typeInfo.id, out var components))
                components = NewComponentContainer<T>();
            components.Set(entity, ref component);
        }

        public void SetComponent<TC, TR>(Entity entity, TC component, TR value) where TC : unmanaged, IManagedComponent<TR> where TR : class
        {
            component.WorldId = this.id;
            component.EntityId = entity.id;

            var typeInfo = TypeManager.GetTypeInfo<TC>();
            if (!componentContainers.TryGetValue(typeInfo.id, out var components))
                components = NewComponentContainer<TC>();
            components.Set(entity, ref component);
            references.Set(typeInfo.id, component.EntityId, value);
        }

        public void SetComponent<TC, TR>(Entity entity, ref TC component, TR value) where TC : unmanaged, IManagedComponent<TR> where TR : class
        {
            component.WorldId = this.id;
            component.EntityId = entity.id;

            var typeInfo = TypeManager.GetTypeInfo<TC>();
            if (!componentContainers.TryGetValue(typeInfo.id, out var components))
                components = NewComponentContainer<TC>();
            components.Set(entity, ref component);
            references.Set(typeInfo.id, component.EntityId, value);
        }

        #endregion

        #region Remove

        public void RemoveComponent(Entity entity, Type componentType)
        {
            var typeInfo = TypeManager.GetTypeInfo(componentType);
            if (!componentContainers.TryGetValue(typeInfo.id, out var components))
                return;
            if (typeInfo.isManagedComponentType)
            {
                references.Release(typeInfo.id, entity.id);
            }
            components.Del(entity);
        }

        public void RemoveComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            var typeInfo = TypeManager.GetTypeInfo(typeof(T));
            if (!componentContainers.TryGetValue(typeInfo.id, out var components))
                return;
            if (typeInfo.isManagedComponentType)
            {
                references.Release(typeInfo.id, entity.id);
            }

            components.Del(entity);
        }

        #endregion

        private void RemoveAllComponents()
        {
            foreach (var components in componentContainers.GetValueArray(Allocator.Temp))
            {
                components.Dispose();
            }

            componentContainers.Clear();
            componentContainers.Dispose();
            references.Clear();
        }
    }
}