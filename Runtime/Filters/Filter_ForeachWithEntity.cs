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
using Unity.Collections;

namespace CZToolKit.ECS
{
    public delegate void ForeachWithEntityAction<C0>(Entity entity, ref C0 c0) where C0 : struct, IComponent;
    public delegate void ForeachWithEntityAction<C0, C1>(Entity entity, ref C0 c0, ref C1 c1) where C0 : struct, IComponent where C1 : struct, IComponent;
    public delegate void ForeachWithEntityAction<C0, C1, C2>(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent;
    public delegate void ForeachWithEntityAction<C0, C1, C2, C3>(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent;
    public delegate void ForeachWithEntityAction<C0, C1, C2, C3, C4>(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent;
    public delegate void ForeachWithEntityAction<C0, C1, C2, C3, C4, C5>(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent;

    public partial class Filter
    {
        public void ForeachWithEntity<C0>(ForeachWithEntityAction<C0> action)
            where C0 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0);
            if (!world.ExistsComponentContainer(componentType0))
                return;
            var componentPool0 = world.GetComponentContainer(componentType0);
            if (componentPool0.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!componentPool0.Contains(entity))
                    continue;
                action(entity, ref componentPool0.Get<C0>(entity));
            }
        }

        public void ForeachWithEntity<C0, C1>(ForeachWithEntityAction<C0, C1> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0);
            var componentType1 = typeof(C1);
            if (!world.ExistsComponentContainer(componentType0))
                return;
            if (!world.ExistsComponentContainer(componentType1))
                return;
            var componentPool0 = world.GetComponentContainer(componentType0);
            if (componentPool0.Count() <= 0)
                return;
            var componentPool1 = world.GetComponentContainer(componentType1);
            if (componentPool1.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!componentPool0.Contains(entity))
                    continue;
                if (!componentPool1.Contains(entity))
                    continue;
                action(entity,
                    ref componentPool0.Get<C0>(entity),
                    ref componentPool1.Get<C1>(entity));
            }
        }

        public void ForeachWithEntity<C0, C1, C2>(ForeachWithEntityAction<C0, C1, C2> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0);
            var componentType1 = typeof(C1);
            var componentType2 = typeof(C2);
            if (!world.ExistsComponentContainer(componentType0))
                return;
            if (!world.ExistsComponentContainer(componentType1))
                return;
            if (!world.ExistsComponentContainer(componentType2))
                return;
            var componentPool0 = world.GetComponentContainer(componentType0);
            if (componentPool0.Count() <= 0)
                return;
            var componentPool1 = world.GetComponentContainer(componentType1);
            if (componentPool1.Count() <= 0)
                return;
            var componentPool2 = world.GetComponentContainer(componentType2);
            if (componentPool2.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!componentPool0.Contains(entity))
                    continue;
                if (!componentPool1.Contains(entity))
                    continue;
                if (!componentPool2.Contains(entity))
                    continue;
                action(entity,
                    ref componentPool0.Get<C0>(entity),
                    ref componentPool1.Get<C1>(entity),
                    ref componentPool2.Get<C2>(entity));
            }
        }

        public void ForeachWithEntity<C0, C1, C2, C3>(ForeachWithEntityAction<C0, C1, C2, C3> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0);
            var componentType1 = typeof(C1);
            var componentType2 = typeof(C2);
            var componentType3 = typeof(C3);
            if (!world.ExistsComponentContainer(componentType0))
                return;
            if (!world.ExistsComponentContainer(componentType1))
                return;
            if (!world.ExistsComponentContainer(componentType2))
                return;
            if (!world.ExistsComponentContainer(componentType3))
                return;
            var componentPool0 = world.GetComponentContainer(componentType0);
            if (componentPool0.Count() <= 0)
                return;
            var componentPool1 = world.GetComponentContainer(componentType1);
            if (componentPool1.Count() <= 0)
                return;
            var componentPool2 = world.GetComponentContainer(componentType2);
            if (componentPool2.Count() <= 0)
                return;
            var componentPool3 = world.GetComponentContainer(componentType3);
            if (componentPool3.Count() <= 0)
                return;
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
                action(entity,
                    ref componentPool0.Get<C0>(entity),
                    ref componentPool1.Get<C1>(entity),
                    ref componentPool2.Get<C2>(entity),
                    ref componentPool3.Get<C3>(entity));
            }
        }

        public void ForeachWithEntity<C0, C1, C2, C3, C4>(ForeachWithEntityAction<C0, C1, C2, C3, C4> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
            where C4 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0);
            var componentType1 = typeof(C1);
            var componentType2 = typeof(C2);
            var componentType3 = typeof(C3);
            var componentType4 = typeof(C4);
            if (!world.ExistsComponentContainer(componentType0))
                return;
            if (!world.ExistsComponentContainer(componentType1))
                return;
            if (!world.ExistsComponentContainer(componentType2))
                return;
            if (!world.ExistsComponentContainer(componentType3))
                return;
            if (!world.ExistsComponentContainer(componentType4))
                return;
            var componentPool0 = world.GetComponentContainer(componentType0);
            if (componentPool0.Count() <= 0)
                return;
            var componentPool1 = world.GetComponentContainer(componentType1);
            if (componentPool1.Count() <= 0)
                return;
            var componentPool2 = world.GetComponentContainer(componentType2);
            if (componentPool2.Count() <= 0)
                return;
            var componentPool3 = world.GetComponentContainer(componentType3);
            if (componentPool3.Count() <= 0)
                return;
            var componentPool4 = world.GetComponentContainer(componentType4);
            if (componentPool4.Count() <= 0)
                return;
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
                if (!componentPool4.Contains(entity))
                    continue;
                action(entity,
                    ref componentPool0.Get<C0>(entity),
                    ref componentPool1.Get<C1>(entity),
                    ref componentPool2.Get<C2>(entity),
                    ref componentPool3.Get<C3>(entity),
                    ref componentPool4.Get<C4>(entity));
            }
        }

        public void ForeachWithEntity<C0, C1, C2, C3, C4, C5>(ForeachWithEntityAction<C0, C1, C2, C3, C4, C5> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
            where C4 : unmanaged, IComponent
            where C5 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0);
            var componentType1 = typeof(C1);
            var componentType2 = typeof(C2);
            var componentType3 = typeof(C3);
            var componentType4 = typeof(C4);
            var componentType5 = typeof(C5);
            if (!world.ExistsComponentContainer(componentType0))
                return;
            if (!world.ExistsComponentContainer(componentType1))
                return;
            if (!world.ExistsComponentContainer(componentType2))
                return;
            if (!world.ExistsComponentContainer(componentType3))
                return;
            if (!world.ExistsComponentContainer(componentType4))
                return;
            if (!world.ExistsComponentContainer(componentType5))
                return;
            var componentPool0 = world.GetComponentContainer(componentType0);
            if (componentPool0.Count() <= 0)
                return;
            var componentPool1 = world.GetComponentContainer(componentType1);
            if (componentPool1.Count() <= 0)
                return;
            var componentPool2 = world.GetComponentContainer(componentType2);
            if (componentPool2.Count() <= 0)
                return;
            var componentPool3 = world.GetComponentContainer(componentType3);
            if (componentPool3.Count() <= 0)
                return;
            var componentPool4 = world.GetComponentContainer(componentType4);
            if (componentPool4.Count() <= 0)
                return;
            var componentPool5 = world.GetComponentContainer(componentType5);
            if (componentPool5.Count() <= 0)
                return;
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
                if (!componentPool4.Contains(entity))
                    continue;
                if (!componentPool5.Contains(entity))
                    continue;
                action(entity,
                    ref componentPool0.Get<C0>(entity),
                    ref componentPool1.Get<C1>(entity),
                    ref componentPool2.Get<C2>(entity),
                    ref componentPool3.Get<C3>(entity),
                    ref componentPool4.Get<C4>(entity),
                    ref componentPool5.Get<C5>(entity));
            }
        }
    }
}
