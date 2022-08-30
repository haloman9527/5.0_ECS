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
using Unity.Jobs;

namespace CZToolKit.ECS
{
    public partial class Filter
    {
        public void JobForeachWithEntity<J, C0>(J job) where J : IJob, IJobForeachWithEntity_EC<C0> where C0 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0);
            if (!world.ExistsComponentPool(componentType0))
                return;
            var componentPool0 = world.GetComponentPool(componentType0);
            if (componentPool0.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!componentPool0.Contains(entity))
                    continue;
                job.Execute(entity,
                    ref componentPool0.Get<C0>(entity));
            }
        }

        public void JobForeachWithEntity<J, C0, C1>(J job) where J : IJob, IJobForeachWithEntity_ECC<C0, C1> where C0 : unmanaged, IComponent where C1 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0);
            var componentType1 = typeof(C1);
            if (!world.ExistsComponentPool(componentType0))
                return;
            if (!world.ExistsComponentPool(componentType1))
                return;
            var componentPool0 = world.GetComponentPool(componentType0);
            if (componentPool0.Count() <= 0)
                return;
            var componentPool1 = world.GetComponentPool(componentType1);
            if (componentPool1.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!componentPool0.Contains(entity))
                    continue;
                if (!componentPool1.Contains(entity))
                    continue;
                job.Execute(entity,
                    ref componentPool0.Get<C0>(entity),
                    ref componentPool1.Get<C1>(entity));
            }
        }

        public void JobForeachWithEntity<J, C0, C1, C2>(J job) where J : IJob, IJobForeachWithEntity_ECCC<C0, C1, C2> where C0 : unmanaged, IComponent where C1 : unmanaged, IComponent where C2 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0);
            var componentType1 = typeof(C1);
            var componentType2 = typeof(C2);
            if (!world.ExistsComponentPool(componentType0))
                return;
            if (!world.ExistsComponentPool(componentType1))
                return;
            if (!world.ExistsComponentPool(componentType2))
                return;
            var componentPool0 = world.GetComponentPool(componentType0);
            if (componentPool0.Count() <= 0)
                return;
            var componentPool1 = world.GetComponentPool(componentType1);
            if (componentPool1.Count() <= 0)
                return;
            var componentPool2 = world.GetComponentPool(componentType2);
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
                job.Execute(entity,
                    ref componentPool0.Get<C0>(entity),
                    ref componentPool1.Get<C1>(entity),
                    ref componentPool2.Get<C2>(entity));
            }
        }

        public void JobForeachWithEntity<J, C0, C1, C2, C3>(J job) where J : IJob, IJobForeachWithEntity_ECCCC<C0, C1, C2, C3> where C0 : unmanaged, IComponent where C1 : unmanaged, IComponent where C2 : unmanaged, IComponent where C3 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0);
            var componentType1 = typeof(C1);
            var componentType2 = typeof(C2);
            var componentType3 = typeof(C3);
            if (!world.ExistsComponentPool(componentType0))
                return;
            if (!world.ExistsComponentPool(componentType1))
                return;
            if (!world.ExistsComponentPool(componentType2))
                return;
            if (!world.ExistsComponentPool(componentType3))
                return;
            var componentPool0 = world.GetComponentPool(componentType0);
            if (componentPool0.Count() <= 0)
                return;
            var componentPool1 = world.GetComponentPool(componentType1);
            if (componentPool1.Count() <= 0)
                return;
            var componentPool2 = world.GetComponentPool(componentType2);
            if (componentPool2.Count() <= 0)
                return;
            var componentPool3 = world.GetComponentPool(componentType3);
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
                job.Execute(entity,
                    ref componentPool0.Get<C0>(entity),
                    ref componentPool1.Get<C1>(entity),
                    ref componentPool2.Get<C2>(entity),
                    ref componentPool3.Get<C3>(entity));
            }
        }

        public void JobForeachWithEntity<J, C0, C1, C2, C3, C4>(J job) where J : IJob, IJobForeachWithEntity_ECCCCC<C0, C1, C2, C3, C4> where C0 : unmanaged, IComponent where C1 : unmanaged, IComponent where C2 : unmanaged, IComponent where C3 : unmanaged, IComponent where C4 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0);
            var componentType1 = typeof(C1);
            var componentType2 = typeof(C2);
            var componentType3 = typeof(C3);
            var componentType4 = typeof(C4);
            if (!world.ExistsComponentPool(componentType0))
                return;
            if (!world.ExistsComponentPool(componentType1))
                return;
            if (!world.ExistsComponentPool(componentType2))
                return;
            if (!world.ExistsComponentPool(componentType3))
                return;
            if (!world.ExistsComponentPool(componentType4))
                return;
            var componentPool0 = world.GetComponentPool(componentType0);
            if (componentPool0.Count() <= 0)
                return;
            var componentPool1 = world.GetComponentPool(componentType1);
            if (componentPool1.Count() <= 0)
                return;
            var componentPool2 = world.GetComponentPool(componentType2);
            if (componentPool2.Count() <= 0)
                return;
            var componentPool3 = world.GetComponentPool(componentType3);
            if (componentPool3.Count() <= 0)
                return;
            var componentPool4 = world.GetComponentPool(componentType4);
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
                job.Execute(entity,
                    ref componentPool0.Get<C0>(entity),
                    ref componentPool1.Get<C1>(entity),
                    ref componentPool2.Get<C2>(entity),
                    ref componentPool3.Get<C3>(entity),
                    ref componentPool4.Get<C4>(entity));
            }
        }

        public void JobForeachWithEntity<J, C0, C1, C2, C3, C4, C5>(J job) where J : IJob, IJobForeachWithEntity_ECCCCCC<C0, C1, C2, C3, C4, C5> where C0 : unmanaged, IComponent where C1 : unmanaged, IComponent where C2 : unmanaged, IComponent where C3 : unmanaged, IComponent where C4 : unmanaged, IComponent where C5 : unmanaged, IComponent
        {
            var componentType0 = typeof(C0);
            var componentType1 = typeof(C1);
            var componentType2 = typeof(C2);
            var componentType3 = typeof(C3);
            var componentType4 = typeof(C4);
            var componentType5 = typeof(C5);
            if (!world.ExistsComponentPool(componentType0))
                return;
            if (!world.ExistsComponentPool(componentType1))
                return;
            if (!world.ExistsComponentPool(componentType2))
                return;
            if (!world.ExistsComponentPool(componentType3))
                return;
            if (!world.ExistsComponentPool(componentType4))
                return;
            if (!world.ExistsComponentPool(componentType5))
                return;
            var componentPool0 = world.GetComponentPool(componentType0);
            if (componentPool0.Count() <= 0)
                return;
            var componentPool1 = world.GetComponentPool(componentType1);
            if (componentPool0.Count() <= 0)
                return;
            var componentPool2 = world.GetComponentPool(componentType2);
            if (componentPool2.Count() <= 0)
                return;
            var componentPool3 = world.GetComponentPool(componentType3);
            if (componentPool3.Count() <= 0)
                return;
            var componentPool4 = world.GetComponentPool(componentType4);
            if (componentPool4.Count() <= 0)
                return;
            var componentPool5 = world.GetComponentPool(componentType5);
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
                job.Execute(entity,
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
