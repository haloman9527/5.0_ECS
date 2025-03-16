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
using Atom.UnsafeEx;
using Unity.Collections;

namespace Atom.ECS
{
    public partial class World
    {
        public readonly ECSReferences references = new ECSReferences();

        private NativeParallelHashMap<int, ComponentsContainer> componentContainers =
            new NativeParallelHashMap<int, ComponentsContainer>(128, Allocator.Persistent);

        private IWorldOperationListener worldOperationListener;

        public void SetWorldOperationListener(IWorldOperationListener worldOperationListener)
        {
            this.worldOperationListener = worldOperationListener;
        }

        public NativeParallelHashMap<int, ComponentsContainer> ComponentContainers => componentContainers;

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

        public bool HasComponent(Entity entity, int componentTypeId)
        {
            if (!componentContainers.TryGetValue(componentTypeId, out var components))
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
            return HasComponent(entity, TypeCache<T>.TYPE);
        }

        #endregion

        #region Get

        public T GetComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(TypeInfo<T>.Id, out var components))
            {
                return default;
                // throw new Exception($"实体{entity.ToString()}没有{TypeCache<T>.TYPE}组件");
            }

            return components.Get<T>(entity);
        }

        public ref T RefComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            if (!componentContainers.TryGetValue(TypeInfo<T>.Id, out var components))
            {
                throw new Exception($"实体{entity.ToString()}没有{TypeCache<T>.TYPE}组件");
            }

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

        public V GetRefValue<C, V>(Entity entity) where C : unmanaged, IManagedComponent<V> where V : class
        {
            return this.references.Get(TypeInfo<C>.Id, entity.id) as V;
        }

        public object GetRefValue<C>(Entity entity) where C : unmanaged, IManagedComponent
        {
            return this.references.Get(TypeInfo<C>.Id, entity.id);
        }

        public V GetRefValue<C, V>(C component) where C : unmanaged, IManagedComponent<V> where V : class
        {
            return this.references.Get(TypeInfo<C>.Id, component.EntityId) as V;
        }

        public object GetRefValue<C>(C component) where C : unmanaged, IManagedComponent
        {
            return this.references.Get(TypeInfo<C>.Id, component.EntityId);
        }

        #endregion

        #region Set

        public void SetComponent<C>(Entity entity, C component) where C : unmanaged, IComponent
        {
            var typeInfo = TypeInfo<C>.CachedTypeInfo;
            if (!componentContainers.TryGetValue(typeInfo.id, out var components))
            {
                components = NewComponentContainer<C>();
            }
            else
            {
                if (components.Contains(entity))
                {
                    RemoveComponent<C>(entity);
                }
            }

            components.Set(entity, ref component);
            worldOperationListener?.OnSetComponent(this, entity, typeInfo);
        }

        public void SetComponent<C, V>(Entity entity, C component, V refValue) where C : unmanaged, IManagedComponent<V> where V : class
        {
            component.WorldId = this.Id;
            component.EntityId = entity.id;

            var typeInfo = TypeInfo<C>.CachedTypeInfo;
            if (!componentContainers.TryGetValue(typeInfo.id, out var components))
            {
                components = NewComponentContainer<C>();
            }
            else if (components.Contains(entity))
            {
                RemoveComponent<C>(entity);
            }

            components.Set(entity, ref component);
            references.Set(typeInfo.id, component.EntityId, refValue);
            worldOperationListener?.OnSetComponent(this, entity, typeInfo);
        }

        public void SetRefValue<C>(Entity entity, object refValue) where C : unmanaged, IManagedComponent
        {
            references.Set(TypeInfo<C>.Id, entity.id, refValue);
        }

        public void SetRefValue<C, V>(Entity entity, V refValue) where C : unmanaged, IManagedComponent<V> where V : class
        {
            references.Set(TypeInfo<C>.Id, entity.id, refValue);
        }

        public void SetRefValue<C>(C component, object refValue) where C : unmanaged, IManagedComponent
        {
            references.Set(TypeInfo<C>.Id, component.EntityId, refValue);
        }

        public void SetRefValue<C, V>(C component, V refValue) where C : unmanaged, IManagedComponent<V> where V : class
        {
            references.Set(TypeInfo<C>.Id, component.EntityId, refValue);
        }

        #endregion

        #region Remove

        public void RemoveComponent(Entity entity, Type componentType)
        {
            var typeInfo = TypeManager.GetTypeInfo(componentType);
            if (!componentContainers.TryGetValue(typeInfo.id, out var components))
                return;

            worldOperationListener?.BeforeRemoveComponent(this, entity, typeInfo);
            if (typeInfo.isManagedComponentType)
            {
                references.Release(typeInfo.id, entity.id);
            }

            components.Del(entity);
            worldOperationListener?.AfterRemoveComponent(this, entity, typeInfo);
        }

        public void RemoveComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            var typeInfo = TypeManager.GetTypeInfo(TypeCache<T>.TYPE);
            if (!componentContainers.TryGetValue(typeInfo.id, out var components))
                return;

            worldOperationListener?.BeforeRemoveComponent(this, entity, typeInfo);
            if (typeInfo.isManagedComponentType)
            {
                references.Release(typeInfo.id, entity.id);
            }

            components.Del(entity);
            worldOperationListener?.AfterRemoveComponent(this, entity, typeInfo);
        }

        #endregion

        private void RemoveAllComponents()
        {
            worldOperationListener?.BeforeWorldDispose(this);

            foreach (var components in componentContainers.GetValueArray(Allocator.Temp))
            {
                components.Dispose();
            }

            componentContainers.Clear();
            componentContainers.Dispose();
            references.Clear();

            worldOperationListener?.AfterWorldDispose(this);
        }
    }
}