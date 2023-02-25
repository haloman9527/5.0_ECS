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
    public delegate void ForeachAction<C0>(ref C0 c0) where C0 : struct, IComponent;

    public delegate void ForeachAction<C0, C1>(ref C0 c0, ref C1 c1) where C0 : struct, IComponent where C1 : struct, IComponent;

    public delegate void ForeachAction<C0, C1, C2>(ref C0 c0, ref C1 c1, ref C2 c2) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent;

    public delegate void ForeachAction<C0, C1, C2, C3>(ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent;

    public delegate void ForeachAction<C0, C1, C2, C3, C4>(ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent;

    public delegate void ForeachAction<C0, C1, C2, C3, C4, C5>(ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent;

    public partial class Filter
    {
        public void Foreach<C0>(ForeachAction<C0> action)
            where C0 : unmanaged, IComponent
        {
            var typeIndex0 = SharedTypeIndex<C0>.Data;
            if (!world.ComponentContainers.TryGetValue(typeIndex0, out var container0))
                return;
            if (container0.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!container0.Contains(entity))
                    continue;
                action(ref container0.Ref<C0>(entity));
            }
        }

        public void Foreach<C0, C1>(ForeachAction<C0, C1> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
        {
            var typeIndex0 = SharedTypeIndex<C0>.Data;
            var typeIndex1 = SharedTypeIndex<C1>.Data;
            if (!world.ComponentContainers.TryGetValue(typeIndex0, out var container0))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex1, out var container1))
                return;
            if (container0.Count() <= 0)
                return;
            if (container1.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!container0.Contains(entity))
                    continue;
                if (!container1.Contains(entity))
                    continue;
                action(ref container0.Ref<C0>(entity),
                    ref container1.Ref<C1>(entity));
            }
        }

        public void Foreach<C0, C1, C2>(ForeachAction<C0, C1, C2> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
        {
            var typeIndex0 = SharedTypeIndex<C0>.Data;
            var typeIndex1 = SharedTypeIndex<C1>.Data;
            var typeIndex2 = SharedTypeIndex<C2>.Data;
            if (!world.ComponentContainers.TryGetValue(typeIndex0, out var componentPool0))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex1, out var componentPool1))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex2, out var componentPool2))
                return;
            if (componentPool0.Count() <= 0)
                return;
            if (componentPool1.Count() <= 0)
                return;
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
                action(ref componentPool0.Ref<C0>(entity),
                    ref componentPool1.Ref<C1>(entity),
                    ref componentPool2.Ref<C2>(entity));
            }
        }

        public void Foreach<C0, C1, C2, C3>(ForeachAction<C0, C1, C2, C3> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
        {
            var typeIndex0 = SharedTypeIndex<C0>.Data;
            var typeIndex1 = SharedTypeIndex<C1>.Data;
            var typeIndex2 = SharedTypeIndex<C2>.Data;
            var typeIndex3 = SharedTypeIndex<C3>.Data;
            if (!world.ComponentContainers.TryGetValue(typeIndex0, out var container0))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex1, out var container1))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex2, out var container2))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex3, out var container3))
                return;
            if (container0.Count() <= 0)
                return;
            if (container1.Count() <= 0)
                return;
            if (container2.Count() <= 0)
                return;
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
                action(ref container0.Ref<C0>(entity),
                    ref container1.Ref<C1>(entity),
                    ref container2.Ref<C2>(entity),
                    ref container3.Ref<C3>(entity));
            }
        }

        public void Foreach<C0, C1, C2, C3, C4>(ForeachAction<C0, C1, C2, C3, C4> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
            where C4 : unmanaged, IComponent
        {
            var typeIndex0 = SharedTypeIndex<C0>.Data;
            var typeIndex1 = SharedTypeIndex<C1>.Data;
            var typeIndex2 = SharedTypeIndex<C2>.Data;
            var typeIndex3 = SharedTypeIndex<C3>.Data;
            var typeIndex4 = SharedTypeIndex<C4>.Data;
            if (!world.ComponentContainers.TryGetValue(typeIndex0, out var container0))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex1, out var container1))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex2, out var container2))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex3, out var container3))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex4, out var container4))
                return;
            if (container0.Count() <= 0)
                return;
            if (container1.Count() <= 0)
                return;
            if (container2.Count() <= 0)
                return;
            if (container3.Count() <= 0)
                return;
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
                action(ref container0.Ref<C0>(entity),
                    ref container1.Ref<C1>(entity),
                    ref container2.Ref<C2>(entity),
                    ref container3.Ref<C3>(entity),
                    ref container4.Ref<C4>(entity));
            }
        }

        public void Foreach<C0, C1, C2, C3, C4, C5>(ForeachAction<C0, C1, C2, C3, C4, C5> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
            where C4 : unmanaged, IComponent
            where C5 : unmanaged, IComponent
        {
            var typeIndex0 = SharedTypeIndex<C0>.Data;
            var typeIndex1 = SharedTypeIndex<C1>.Data;
            var typeIndex2 = SharedTypeIndex<C2>.Data;
            var typeIndex3 = SharedTypeIndex<C3>.Data;
            var typeIndex4 = SharedTypeIndex<C4>.Data;
            var typeIndex5 = SharedTypeIndex<C5>.Data;
            if (!world.ComponentContainers.TryGetValue(typeIndex0, out var container0))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex1, out var container1))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex2, out var container2))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex3, out var container3))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex4, out var container4))
                return;
            if (!world.ComponentContainers.TryGetValue(typeIndex4, out var container5))
                return;
            if (container0.Count() <= 0)
                return;
            if (container1.Count() <= 0)
                return;
            if (container2.Count() <= 0)
                return;
            if (container3.Count() <= 0)
                return;
            if (container4.Count() <= 0)
                return;
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
                action(ref container0.Ref<C0>(entity),
                    ref container1.Ref<C1>(entity),
                    ref container2.Ref<C2>(entity),
                    ref container3.Ref<C3>(entity),
                    ref container4.Ref<C4>(entity),
                    ref container4.Ref<C5>(entity));
            }
        }
    }
}