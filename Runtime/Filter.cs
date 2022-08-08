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
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace CZToolKit.ECS
{
    public delegate void RefAction<Arg0>(ref Arg0 arg0) where Arg0 : struct, IComponent;
    public delegate void RefAction<Arg0, Arg1>(ref Arg0 arg0, ref Arg1 arg1) where Arg0 : struct, IComponent where Arg1 : struct, IComponent;
    public delegate void RefAction<Arg0, Arg1, Arg2>(ref Arg0 arg0, ref Arg1 arg1, ref Arg2 arg2) where Arg0 : struct, IComponent where Arg1 : struct, IComponent where Arg2 : struct, IComponent;
    public delegate void RefAction<Arg0, Arg1, Arg2, Arg3>(ref Arg0 arg0, ref Arg1 arg1, ref Arg2 arg2, ref Arg3 arg3) where Arg0 : struct, IComponent where Arg1 : struct, IComponent where Arg2 : struct, IComponent where Arg3 : struct, IComponent;

    public class Filter
    {
        private readonly World world;

        public Filter(World world)
        {
            this.world = world;
        }

        #region RefForeach
        public unsafe void Foreach<ComponentType0>(RefAction<ComponentType0> action)
            where ComponentType0 : unmanaged, IComponent
        {
            var componentType0 = typeof(ComponentType0);
            if (!world.ExistsComponentPool(componentType0))
                return;
            var componentPool = world.GetComponentPool(componentType0);
            foreach (var entity in componentPool.GetEntities(Allocator.Temp))
            {
                action(ref Unsafe.AsRef<ComponentType0>(componentPool.Get<ComponentType0>(entity)));
            }
        }

        public unsafe void Foreach<ComponentType0, ComponentType1>(RefAction<ComponentType0, ComponentType1> action)
            where ComponentType0 : unmanaged, IComponent
            where ComponentType1 : unmanaged, IComponent
        {
            var componentType0 = typeof(ComponentType0);
            var componentType1 = typeof(ComponentType1);
            if (!world.ExistsComponentPool(componentType0))
                return;
            if (!world.ExistsComponentPool(componentType1))
                return;
            var componentPool0 = world.GetComponentPool(componentType0);
            var componentPool1 = world.GetComponentPool(componentType1);
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!componentPool0.Contains(entity))
                    continue;
                if (!componentPool1.Contains(entity))
                    continue;
                action(ref Unsafe.AsRef<ComponentType0>(componentPool0.Get<ComponentType0>(entity)),
                    ref Unsafe.AsRef<ComponentType1>(componentPool1.Get<ComponentType1>(entity)));
            }
        }

        public unsafe void Foreach<ComponentType0, ComponentType1, ComponentType2>(RefAction<ComponentType0, ComponentType1, ComponentType2> action)
            where ComponentType0 : unmanaged, IComponent
            where ComponentType1 : unmanaged, IComponent
            where ComponentType2 : unmanaged, IComponent
        {
            var componentType0 = typeof(ComponentType0);
            var componentType1 = typeof(ComponentType1);
            var componentType2 = typeof(ComponentType2);
            if (!world.ExistsComponentPool(componentType0))
                return;
            if (!world.ExistsComponentPool(componentType1))
                return;
            if (!world.ExistsComponentPool(componentType2))
                return;
            var componentPool0 = world.GetComponentPool(componentType0);
            var componentPool1 = world.GetComponentPool(componentType1);
            var componentPool2 = world.GetComponentPool(componentType2);
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!componentPool0.Contains(entity))
                    continue;
                if (!componentPool1.Contains(entity))
                    continue;
                if (!componentPool2.Contains(entity))
                    continue;
                action(ref Unsafe.AsRef<ComponentType0>(componentPool0.Get<ComponentType0>(entity)),
                    ref Unsafe.AsRef<ComponentType1>(componentPool1.Get<ComponentType1>(entity)),
                    ref Unsafe.AsRef<ComponentType2>(componentPool2.Get<ComponentType2>(entity)));
            }
        }

        public unsafe void Foreach<ComponentType0, ComponentType1, ComponentType2, ComponentType3>(RefAction<ComponentType0, ComponentType1, ComponentType2, ComponentType3> action)
            where ComponentType0 : unmanaged, IComponent
            where ComponentType1 : unmanaged, IComponent
            where ComponentType2 : unmanaged, IComponent
            where ComponentType3 : unmanaged, IComponent
        {
            var componentType0 = typeof(ComponentType0);
            var componentType1 = typeof(ComponentType1);
            var componentType2 = typeof(ComponentType2);
            var componentType3 = typeof(ComponentType3);
            if (!world.ExistsComponentPool(componentType0))
                return;
            if (!world.ExistsComponentPool(componentType1))
                return;
            if (!world.ExistsComponentPool(componentType2))
                return;
            if (!world.ExistsComponentPool(componentType3))
                return;
            var componentPool0 = world.GetComponentPool(componentType0);
            var componentPool1 = world.GetComponentPool(componentType1);
            var componentPool2 = world.GetComponentPool(componentType2);
            var componentPool3 = world.GetComponentPool(componentType3);
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!componentPool0.Contains(entity))
                    continue;
                if (!componentPool1.Contains(entity))
                    continue;
                if (!componentPool2.Contains(entity))
                    continue;
                if (!componentPool3.Contains(entity))
                    continue;
                action(ref Unsafe.AsRef<ComponentType0>(componentPool0.Get<ComponentType0>(entity)),
                    ref Unsafe.AsRef<ComponentType1>(componentPool1.Get<ComponentType1>(entity)),
                    ref Unsafe.AsRef<ComponentType2>(componentPool2.Get<ComponentType2>(entity)),
                    ref Unsafe.AsRef<ComponentType3>(componentPool3.Get<ComponentType3>(entity)));
            }
        }
        #endregion
    }
}
