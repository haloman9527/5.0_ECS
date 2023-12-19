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
 *  Blog: https://www.mindgear.net/
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
            if (!world.ExistsComponentContainer<C0>())
                return;
            var componentPool0 = world.GetComponentContainer<C0>();
            if (componentPool0.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!componentPool0.Contains(entity))
                    continue;
                action(entity, ref componentPool0.Ref<C0>(entity));
            }
        }

        public void ForeachWithEntity<C0, C1>(ForeachWithEntityAction<C0, C1> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
        {
            if (!world.ExistsComponentContainer<C0>())
                return;
            if (!world.ExistsComponentContainer<C1>())
                return;
            var container0 = world.GetComponentContainer<C0>();
            if (container0.Count() <= 0)
                return;
            var container1 = world.GetComponentContainer<C1>();
            if (container1.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!container0.Contains(entity))
                    continue;
                if (!container1.Contains(entity))
                    continue;
                action(entity,
                    ref container0.Ref<C0>(entity),
                    ref container1.Ref<C1>(entity));
            }
        }

        public void ForeachWithEntity<C0, C1, C2>(ForeachWithEntityAction<C0, C1, C2> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
        {
            if (!world.ExistsComponentContainer<C0>())
                return;
            if (!world.ExistsComponentContainer<C1>())
                return;
            if (!world.ExistsComponentContainer<C2>())
                return;
            var container0 = world.GetComponentContainer<C0>();
            if (container0.Count() <= 0)
                return;
            var container1 = world.GetComponentContainer<C1>();
            if (container1.Count() <= 0)
                return;
            var container2 = world.GetComponentContainer<C2>();
            if (container2.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!container0.Contains(entity))
                    continue;
                if (!container1.Contains(entity))
                    continue;
                if (!container2.Contains(entity))
                    continue;
                action(entity,
                    ref container0.Ref<C0>(entity),
                    ref container1.Ref<C1>(entity),
                    ref container2.Ref<C2>(entity));
            }
        }

        public void ForeachWithEntity<C0, C1, C2, C3>(ForeachWithEntityAction<C0, C1, C2, C3> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
        {
            if (!world.ExistsComponentContainer<C0>())
                return;
            if (!world.ExistsComponentContainer<C1>())
                return;
            if (!world.ExistsComponentContainer<C2>())
                return;
            if (!world.ExistsComponentContainer<C3>())
                return;
            var container0 = world.GetComponentContainer<C0>();
            if (container0.Count() <= 0)
                return;
            var container1 = world.GetComponentContainer<C1>();
            if (container1.Count() <= 0)
                return;
            var container2 = world.GetComponentContainer<C2>();
            if (container2.Count() <= 0)
                return;
            var container3 = world.GetComponentContainer<C3>();
            if (container3.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!container0.Contains(entity))
                    continue;
                if (!container1.Contains(entity))
                    continue;
                if (!container2.Contains(entity))
                    continue;
                if (!container3.Contains(entity))
                    continue;
                action(entity,
                    ref container0.Ref<C0>(entity),
                    ref container1.Ref<C1>(entity),
                    ref container2.Ref<C2>(entity),
                    ref container3.Ref<C3>(entity));
            }
        }

        public void ForeachWithEntity<C0, C1, C2, C3, C4>(ForeachWithEntityAction<C0, C1, C2, C3, C4> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
            where C4 : unmanaged, IComponent
        {
            if (!world.ExistsComponentContainer<C0>())
                return;
            if (!world.ExistsComponentContainer<C1>())
                return;
            if (!world.ExistsComponentContainer<C2>())
                return;
            if (!world.ExistsComponentContainer<C3>())
                return;
            if (!world.ExistsComponentContainer<C4>())
                return;
            var container0 = world.GetComponentContainer<C0>();
            if (container0.Count() <= 0)
                return;
            var container1 = world.GetComponentContainer<C1>();
            if (container1.Count() <= 0)
                return;
            var container2 = world.GetComponentContainer<C2>();
            if (container2.Count() <= 0)
                return;
            var container3 = world.GetComponentContainer<C3>();
            if (container3.Count() <= 0)
                return;
            var container4 = world.GetComponentContainer<C4>();
            if (container4.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!container0.Contains(entity))
                    continue;
                if (!container1.Contains(entity))
                    continue;
                if (!container2.Contains(entity))
                    continue;
                if (!container3.Contains(entity))
                    continue;
                if (!container4.Contains(entity))
                    continue;
                action(entity,
                    ref container0.Ref<C0>(entity),
                    ref container1.Ref<C1>(entity),
                    ref container2.Ref<C2>(entity),
                    ref container3.Ref<C3>(entity),
                    ref container4.Ref<C4>(entity));
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
            if (!world.ExistsComponentContainer<C0>())
                return;
            if (!world.ExistsComponentContainer<C1>())
                return;
            if (!world.ExistsComponentContainer<C2>())
                return;
            if (!world.ExistsComponentContainer<C3>())
                return;
            if (!world.ExistsComponentContainer<C4>())
                return;
            if (!world.ExistsComponentContainer<C5>())
                return;
            var container0 = world.GetComponentContainer<C0>();
            if (container0.Count() <= 0)
                return;
            var container1 = world.GetComponentContainer<C1>();
            if (container1.Count() <= 0)
                return;
            var container2 = world.GetComponentContainer<C2>();
            if (container2.Count() <= 0)
                return;
            var container3 = world.GetComponentContainer<C3>();
            if (container3.Count() <= 0)
                return;
            var container4 = world.GetComponentContainer<C4>();
            if (container4.Count() <= 0)
                return;
            var container5 = world.GetComponentContainer<C5>();
            if (container5.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!container0.Contains(entity))
                    continue;
                if (!container1.Contains(entity))
                    continue;
                if (!container2.Contains(entity))
                    continue;
                if (!container3.Contains(entity))
                    continue;
                if (!container4.Contains(entity))
                    continue;
                if (!container5.Contains(entity))
                    continue;
                action(entity,
                    ref container0.Ref<C0>(entity),
                    ref container1.Ref<C1>(entity),
                    ref container2.Ref<C2>(entity),
                    ref container3.Ref<C3>(entity),
                    ref container4.Ref<C4>(entity),
                    ref container5.Ref<C5>(entity));
            }
        }
    }
}
