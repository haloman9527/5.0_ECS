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
            var componentType0 = typeof(C0).GetHashCode();
            if (!world.ComponentContainers.TryGetValue(componentType0, out var componentPool0))
                return;
            if (componentPool0.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!componentPool0.Contains(entity))
                    continue;
                action(ref componentPool0.Ref<C0>(entity));
            }
        }

        public void Foreach<C0, C1>(ForeachAction<C0, C1> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0).GetHashCode();
            var componentType1 = typeof(C1).GetHashCode();
            if (!world.ComponentContainers.TryGetValue(componentType0, out var componentPool0))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType1, out var componentPool1))
                return;
            if (componentPool0.Count() <= 0)
                return;
            if (componentPool1.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!componentPool0.Contains(entity))
                    continue;
                if (!componentPool1.Contains(entity))
                    continue;
                action(ref componentPool0.Ref<C0>(entity),
                    ref componentPool1.Ref<C1>(entity));
            }
        }

        public void Foreach<C0, C1, C2>(ForeachAction<C0, C1, C2> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0).GetHashCode();
            var componentType1 = typeof(C1).GetHashCode();
            var componentType2 = typeof(C2).GetHashCode();
            if (!world.ComponentContainers.TryGetValue(componentType0, out var componentPool0))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType1, out var componentPool1))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType2, out var componentPool2))
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
            var componentType0 = typeof(C0).GetHashCode();
            var componentType1 = typeof(C1).GetHashCode();
            var componentType2 = typeof(C2).GetHashCode();
            var componentType3 = typeof(C3).GetHashCode();
            if (!world.ComponentContainers.TryGetValue(componentType0, out var componentPool0))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType1, out var componentPool1))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType2, out var componentPool2))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType3, out var componentPool3))
                return;
            if (componentPool0.Count() <= 0)
                return;
            if (componentPool1.Count() <= 0)
                return;
            if (componentPool2.Count() <= 0)
                return;
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
                action(ref componentPool0.Ref<C0>(entity),
                    ref componentPool1.Ref<C1>(entity),
                    ref componentPool2.Ref<C2>(entity),
                    ref componentPool3.Ref<C3>(entity));
            }
        }

        public void Foreach<C0, C1, C2, C3, C4>(ForeachAction<C0, C1, C2, C3, C4> action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
            where C4 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0).GetHashCode();
            var componentType1 = typeof(C1).GetHashCode();
            var componentType2 = typeof(C2).GetHashCode();
            var componentType3 = typeof(C3).GetHashCode();
            var componentType4 = typeof(C4).GetHashCode();
            if (!world.ComponentContainers.TryGetValue(componentType0, out var componentPool0))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType1, out var componentPool1))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType2, out var componentPool2))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType3, out var componentPool3))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType4, out var componentPool4))
                return;
            if (componentPool0.Count() <= 0)
                return;
            if (componentPool1.Count() <= 0)
                return;
            if (componentPool2.Count() <= 0)
                return;
            if (componentPool3.Count() <= 0)
                return;
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
                action(ref componentPool0.Ref<C0>(entity),
                    ref componentPool1.Ref<C1>(entity),
                    ref componentPool2.Ref<C2>(entity),
                    ref componentPool3.Ref<C3>(entity),
                    ref componentPool4.Ref<C4>(entity));
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
            var componentType0 = typeof(C0).GetHashCode();
            var componentType1 = typeof(C1).GetHashCode();
            var componentType2 = typeof(C2).GetHashCode();
            var componentType3 = typeof(C3).GetHashCode();
            var componentType4 = typeof(C4).GetHashCode();
            var componentType5 = typeof(C5).GetHashCode();
            if (!world.ComponentContainers.TryGetValue(componentType0, out var componentPool0))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType1, out var componentPool1))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType2, out var componentPool2))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType3, out var componentPool3))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType4, out var componentPool4))
                return;
            if (!world.ComponentContainers.TryGetValue(componentType5, out var componentPool5))
                return;
            if (componentPool0.Count() <= 0)
                return;
            if (componentPool1.Count() <= 0)
                return;
            if (componentPool2.Count() <= 0)
                return;
            if (componentPool3.Count() <= 0)
                return;
            if (componentPool4.Count() <= 0)
                return;
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
                action(ref componentPool0.Ref<C0>(entity),
                    ref componentPool1.Ref<C1>(entity),
                    ref componentPool2.Ref<C2>(entity),
                    ref componentPool3.Ref<C3>(entity),
                    ref componentPool4.Ref<C4>(entity),
                    ref componentPool5.Ref<C5>(entity));
            }
        }
    }
}