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

namespace CZToolKit.ECS
{
    public class Filter
    {
        private readonly World world;

        public Filter(World world)
        {
            this.world = world;
        }

        #region RefForeach
        public void RefForeach<ForeachJobType, ComponentType0>(ForeachJobType action)
            where ComponentType0 : struct, IComponent
            where ForeachJobType : struct, IRefForeach<ComponentType0>
        {
            var componentPool = world.GetComponentPool<ComponentType0>();
            if (null == componentPool)
                return;
            foreach (var entityID in componentPool.GetEntityIDs())
            {
                action.Execute(ref world.GetEntity(entityID).RefComponent<ComponentType0>());
            }
        }

        public void RefForeach<ForeachJobType, ComponentType0, ComponentType1>(ForeachJobType action)
            where ComponentType0 : struct, IComponent
            where ComponentType1 : struct, IComponent
            where ForeachJobType : struct, IRefForeach<ComponentType0, ComponentType1>
        {
            var componentPool0 = world.GetComponentPool<ComponentType0>();
            if (null == componentPool0)
                return;
            var componentPool1 = world.GetComponentPool<ComponentType1>();
            if (null == componentPool1)
                return;
            foreach (var entity in world.Entities)
            {
                var entityID = entity.ID;
                if (!componentPool0.Contains(entityID))
                    continue;
                if (!componentPool1.Contains(entityID))
                    continue;
                action.Execute(ref componentPool0.Ref(entityID)
                    , ref componentPool1.Ref(entityID));
            }
        }

        public void RefForeach<ForeachJobType, ComponentType0, ComponentType1, ComponentType2>(ForeachJobType action)
            where ComponentType0 : struct, IComponent
            where ComponentType1 : struct, IComponent
            where ComponentType2 : struct, IComponent
            where ForeachJobType : struct, IRefForeach<ComponentType0, ComponentType1, ComponentType2>
        {
            var componentPool0 = world.GetComponentPool<ComponentType0>();
            if (null == componentPool0)
                return;
            var componentPool1 = world.GetComponentPool<ComponentType1>();
            if (null == componentPool1)
                return;
            var componentPool2 = world.GetComponentPool<ComponentType2>();
            if (null == componentPool2)
                return;
            foreach (var entity in world.Entities)
            {
                var entityID = entity.ID;
                if (!componentPool0.Contains(entityID))
                    continue;
                if (!componentPool1.Contains(entityID))
                    continue;
                if (!componentPool2.Contains(entityID))
                    continue;
                action.Execute(ref componentPool0.Ref(entityID)
                    , ref componentPool1.Ref(entityID)
                    , ref componentPool2.Ref(entityID));
            }
        }

        public void RefForeach<ForeachJobType, ComponentType0, ComponentType1, ComponentType2, ComponentType3>(ForeachJobType action)
            where ComponentType0 : struct, IComponent
            where ComponentType1 : struct, IComponent
            where ComponentType2 : struct, IComponent
            where ComponentType3 : struct, IComponent
            where ForeachJobType : struct, IRefForeach<ComponentType0, ComponentType1, ComponentType2, ComponentType3>
        {
            var componentPool0 = world.GetComponentPool<ComponentType0>();
            if (null == componentPool0)
                return;
            var componentPool1 = world.GetComponentPool<ComponentType1>();
            if (null == componentPool1)
                return;
            var componentPool2 = world.GetComponentPool<ComponentType2>();
            if (null == componentPool2)
                return;
            var componentPool3 = world.GetComponentPool<ComponentType3>();
            if (null == componentPool3)
                return;
            foreach (var entity in world.Entities)
            {
                var entityID = entity.ID;
                if (!componentPool0.Contains(entityID))
                    continue;
                if (!componentPool1.Contains(entityID))
                    continue;
                if (!componentPool2.Contains(entityID))
                    continue;
                if (!componentPool3.Contains(entityID))
                    continue;
                action.Execute(ref componentPool0.Ref(entityID)
                    , ref componentPool1.Ref(entityID)
                    , ref componentPool2.Ref(entityID)
                    , ref componentPool3.Ref(entityID));
            }
        }
        #endregion

        public IEnumerable<Entity> Query<T>() where T : struct, IComponent
        {
            var componentPool = world.GetComponentPool<T>();
            if (null != componentPool)
            {
                foreach (var entityID in componentPool.GetEntityIDs())
                {
                    if (world.TryGetEntity(entityID, out var entity))
                        yield return entity;
                }
            }
        }

        public IEnumerable<Entity> Query(params Type[] componentTypes)
        {
            foreach (var entity in world.Entities)
            {
                bool all = true;
                foreach (var componentType in componentTypes)
                {
                    if (!entity.HasComponent(componentType))
                    {
                        all = false;
                        break;
                    }
                }
                if (all)
                    yield return entity;
            }
        }

        public IEnumerable<Entity> Query(Query query)
        {
            foreach (var entity in world.Entities)
            {
                var entityID = entity.ID;
                bool none = true;
                if (query.none != null)
                {
                    foreach (var componentType in query.none)
                    {
                        var componentPool = world.GetComponentPool(componentType);
                        if (null != componentPool && componentPool.Contains(entityID))
                        {
                            none = false;
                            break;
                        }
                    }
                }
                if (!none)
                    continue;

                bool any = true;
                if (query.any != null)
                {
                    any = false;
                    foreach (var componentType in query.any)
                    {
                        var componentPool = world.GetComponentPool(componentType);
                        if (null != componentPool && componentPool.Contains(entityID))
                        {
                            any = true;
                            break;
                        }
                    }
                }

                bool all = true;
                if (query.all != null)
                {
                    foreach (var componentType in query.all)
                    {
                        var componentPool = world.GetComponentPool(componentType);

                        if (null != componentPool && !componentPool.Contains(entityID))
                        {
                            all = false;
                            break;
                        }
                    }
                }
                if (any && all)
                    yield return entity;
            }
        }
    }

    public struct Query
    {
        public Type[] none;
        public Type[] any;
        public Type[] all;
    }

    public interface IRefForeach<T0>
    {
        void Execute(ref T0 arg0);
    }

    public interface IRefForeach<T0, T1>
    {
        void Execute(ref T0 arg0, ref T1 arg1);
    }

    public interface IRefForeach<T0, T1, T2>
    {
        void Execute(ref T0 arg0, ref T1 arg1, ref T2 arg2);
    }

    public interface IRefForeach<T0, T1, T2, T3>
    {
        void Execute(ref T0 arg0, ref T1 arg1, ref T2 arg2, ref T3 arg3);
    }
}