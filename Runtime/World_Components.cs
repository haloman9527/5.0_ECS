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
using Moyo.UnsafeEx;
using Unity.Collections;

namespace Moyo.ECS
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

        #endregion

        #region Set

        public void SetComponent<T>(Entity entity, ref T component) where T : unmanaged, IComponent
        {
            var typeInfo = TypeManager.GetTypeInfo<T>();
            if (!componentContainers.TryGetValue(typeInfo.id, out var components))
            {
                components = NewComponentContainer<T>();
            }
            else
            {
                if (components.Contains(entity))
                {
                    RemoveComponent<T>(entity);
                }
            }

            components.Set(entity, ref component);
            worldOperationListener?.OnSetComponent(this, entity, typeInfo);
        }

        public void SetComponent<T>(Entity entity, T component) where T : unmanaged, IComponent
        {
            var typeInfo = TypeManager.GetTypeInfo<T>();
            if (!componentContainers.TryGetValue(typeInfo.id, out var components))
            {
                components = NewComponentContainer<T>();
            }
            else
            {
                if (components.Contains(entity))
                {
                    RemoveComponent<T>(entity);
                }
            }

            components.Set(entity, ref component);
            worldOperationListener?.OnSetComponent(this, entity, typeInfo);
        }

        public void SetComponent<TC, TR>(Entity entity, TC component, TR value) where TC : unmanaged, IManagedComponent<TR> where TR : class
        {
            component.WorldId = this.Id;
            component.EntityId = entity.id;

            var typeInfo = TypeManager.GetTypeInfo<TC>();
            if (!componentContainers.TryGetValue(typeInfo.id, out var components))
            {
                components = NewComponentContainer<TC>();
            }
            else if (components.Contains(entity))
            {
                RemoveComponent<TC>(entity);
            }

            components.Set(entity, ref component);
            references.Set(typeInfo.id, component.EntityId, value);
            worldOperationListener?.OnSetComponent(this, entity, typeInfo);
        }

        public void SetComponent<TC, TR>(Entity entity, ref TC component, TR value) where TC : unmanaged, IManagedComponent<TR> where TR : class
        {
            component.WorldId = this.Id;
            component.EntityId = entity.id;

            var typeInfo = TypeManager.GetTypeInfo<TC>();
            if (!componentContainers.TryGetValue(typeInfo.id, out var components))
            {
                components = NewComponentContainer<TC>();
            }
            else
            {
                if (components.Contains(entity))
                {
                    RemoveComponent<TC>(entity);
                }
            }

            components.Set(entity, ref component);
            references.Set(typeInfo.id, component.EntityId, value);
            worldOperationListener?.OnSetComponent(this, entity, typeInfo);
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
        
        #region Query

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