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

using Unity.Collections;

namespace Atom.ECS
{
    public interface IJob
    {
        void Execute(Entity entity);
    }

    public interface IJob<C0>
        where C0 : struct, IComponent
    {
        void Execute(Entity entity, ref C0 c0);
    }

    public interface IJob<C0, C1>
        where C0 : struct, IComponent
        where C1 : struct, IComponent
    {
        void Execute(Entity entity, ref C0 c0, ref C1 c1);
    }

    public interface IJob<C0, C1, C2>
        where C0 : struct, IComponent
        where C1 : struct, IComponent
        where C2 : struct, IComponent
    {
        void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2);
    }

    public interface IJob<C0, C1, C2, C3>
        where C0 : struct, IComponent
        where C1 : struct, IComponent
        where C2 : struct, IComponent
        where C3 : struct, IComponent
    {
        void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3);
    }

    public interface IJob<C0, C1, C2, C3, C4>
        where C0 : struct, IComponent
        where C1 : struct, IComponent
        where C2 : struct, IComponent
        where C3 : struct, IComponent
        where C4 : struct, IComponent
    {
        void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4);
    }

    public interface IJob<C0, C1, C2, C3, C4, C5>
        where C0 : struct, IComponent
        where C1 : struct, IComponent
        where C2 : struct, IComponent
        where C3 : struct, IComponent
        where C4 : struct, IComponent
        where C5 : struct, IComponent
    {
        void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5);
    }

    public partial struct Query
    {
        public void ForeachWithJob<Job>(Job job)
            where Job : struct, IJob
        {
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                job.Execute(entity);
            }
        }

        public void ForeachWithJob<Job, C0>(Job job)
            where Job : struct, IJob<C0>
            where C0 : unmanaged, IComponent
        {
            if (!world.ExistsComponentContainer<C0>())
                return;
            var components0 = new ComponentsOperator(world.GetComponents<C0>());
            if (components0.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!components0.Contains(entity))
                    continue;
                job.Execute(entity, ref components0.Ref<C0>(entity));
            }
        }

        public void ForeachWithJob<Job, C0, C1>(Job action)
            where Job : struct, IJob<C0, C1>
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
        {
            if (!world.ExistsComponentContainer<C0>())
                return;
            if (!world.ExistsComponentContainer<C1>())
                return;
            var components0 = new ComponentsOperator(world.GetComponents<C0>());
            if (components0.Count() <= 0)
                return;
            var components1 = new ComponentsOperator(world.GetComponents<C1>());
            if (components1.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!components0.Contains(entity))
                    continue;
                if (!components1.Contains(entity))
                    continue;
                action.Execute(entity,
                    ref components0.Ref<C0>(entity),
                    ref components1.Ref<C1>(entity));
            }
        }

        public void ForeachWithJob<Job, C0, C1, C2>(Job action)
            where Job : struct, IJob<C0, C1, C2>
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
            var components0 = new ComponentsOperator(world.GetComponents<C0>());
            if (components0.Count() <= 0)
                return;
            var components1 = new ComponentsOperator(world.GetComponents<C1>());
            if (components1.Count() <= 0)
                return;
            var components2 = new ComponentsOperator(world.GetComponents<C2>());
            if (components2.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!components0.Contains(entity))
                    continue;
                if (!components1.Contains(entity))
                    continue;
                if (!components2.Contains(entity))
                    continue;
                action.Execute(entity,
                    ref components0.Ref<C0>(entity),
                    ref components1.Ref<C1>(entity),
                    ref components2.Ref<C2>(entity));
            }
        }

        public void ForeachWithJob<Job, C0, C1, C2, C3>(Job action)
            where Job : struct, IJob<C0, C1, C2, C3>
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
            var components0 = new ComponentsOperator(world.GetComponents<C0>());
            if (components0.Count() <= 0)
                return;
            var components1 = new ComponentsOperator(world.GetComponents<C1>());
            if (components1.Count() <= 0)
                return;
            var components2 = new ComponentsOperator(world.GetComponents<C2>());
            if (components2.Count() <= 0)
                return;
            var components3 = new ComponentsOperator(world.GetComponents<C3>());
            if (components3.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!components0.Contains(entity))
                    continue;
                if (!components1.Contains(entity))
                    continue;
                if (!components2.Contains(entity))
                    continue;
                if (!components3.Contains(entity))
                    continue;
                action.Execute(entity,
                    ref components0.Ref<C0>(entity),
                    ref components1.Ref<C1>(entity),
                    ref components2.Ref<C2>(entity),
                    ref components3.Ref<C3>(entity));
            }
        }

        public void ForeachWithJob<Job, C0, C1, C2, C3, C4>(Job action)
            where Job : struct, IJob<C0, C1, C2, C3, C4>
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
            var components0 = new ComponentsOperator(world.GetComponents<C0>());
            if (components0.Count() <= 0)
                return;
            var components1 = new ComponentsOperator(world.GetComponents<C1>());
            if (components1.Count() <= 0)
                return;
            var components2 = new ComponentsOperator(world.GetComponents<C2>());
            if (components2.Count() <= 0)
                return;
            var components3 = new ComponentsOperator(world.GetComponents<C3>());
            if (components3.Count() <= 0)
                return;
            var components4 = new ComponentsOperator(world.GetComponents<C4>());
            if (components4.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!components0.Contains(entity))
                    continue;
                if (!components1.Contains(entity))
                    continue;
                if (!components2.Contains(entity))
                    continue;
                if (!components3.Contains(entity))
                    continue;
                if (!components4.Contains(entity))
                    continue;
                action.Execute(entity,
                    ref components0.Ref<C0>(entity),
                    ref components1.Ref<C1>(entity),
                    ref components2.Ref<C2>(entity),
                    ref components3.Ref<C3>(entity),
                    ref components4.Ref<C4>(entity));
            }
        }

        public void ForeachWithJob<Job, C0, C1, C2, C3, C4, C5>(Job action)
            where Job : struct, IJob<C0, C1, C2, C3, C4, C5>
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
            var components0 = new ComponentsOperator(world.GetComponents<C0>());
            if (components0.Count() <= 0)
                return;
            var components1 = new ComponentsOperator(world.GetComponents<C1>());
            if (components1.Count() <= 0)
                return;
            var components2 = new ComponentsOperator(world.GetComponents<C2>());
            if (components2.Count() <= 0)
                return;
            var components3 = new ComponentsOperator(world.GetComponents<C3>());
            if (components3.Count() <= 0)
                return;
            var components4 = new ComponentsOperator(world.GetComponents<C4>());
            if (components4.Count() <= 0)
                return;
            var components5 = new ComponentsOperator(world.GetComponents<C5>());
            if (components5.Count() <= 0)
                return;
            foreach (var entity in world.Entities.GetValueArray(Allocator.Temp))
            {
                if (!components0.Contains(entity))
                    continue;
                if (!components1.Contains(entity))
                    continue;
                if (!components2.Contains(entity))
                    continue;
                if (!components3.Contains(entity))
                    continue;
                if (!components4.Contains(entity))
                    continue;
                if (!components5.Contains(entity))
                    continue;
                action.Execute(entity,
                    ref components0.Ref<C0>(entity),
                    ref components1.Ref<C1>(entity),
                    ref components2.Ref<C2>(entity),
                    ref components3.Ref<C3>(entity),
                    ref components4.Ref<C4>(entity),
                    ref components5.Ref<C5>(entity));
            }
        }
    }
}