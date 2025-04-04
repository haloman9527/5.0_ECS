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
    public struct ForeachJob<C0> : IJob<C0> where C0 : struct, IComponent
    {
        public delegate void JobAction(Entity entity, ref C0 c0);

        private JobAction action;

        public ForeachJob(JobAction action)
        {
            this.action = action;
        }

        public void Execute(Entity entity, ref C0 c0)
        {
            this.action(entity, ref c0);
        }
    }
    
    public struct ForeachJob<C0, C1> : IJob<C0, C1> 
        where C0 : struct, IComponent
        where C1 : struct, IComponent
    {
        public delegate void JobAction(Entity entity, ref C0 c0, ref C1 c1) ;
        
        private JobAction action;

        public ForeachJob(JobAction action)
        {
            this.action = action;
        }

        public void Execute(Entity entity, ref C0 c0, ref C1 c1)
        {
            this.action(entity, ref c0, ref c1);
        }
    }
    
    public struct ForeachJob<C0, C1, C2> : IJob<C0, C1, C2> 
        where C0 : struct, IComponent
        where C1 : struct, IComponent
        where C2 : struct, IComponent
    {
        public delegate void JobAction(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2) ;
        
        private JobAction action;

        public ForeachJob(JobAction action)
        {
            this.action = action;
        }

        public void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2)
        {
            this.action(entity, ref c0, ref c1, ref c2);
        }
    }
    
    public struct ForeachJob<C0, C1, C2, C3> : IJob<C0, C1, C2, C3> 
        where C0 : struct, IComponent
        where C1 : struct, IComponent
        where C2 : struct, IComponent
        where C3 : struct, IComponent
    {
        public delegate void JobAction(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3) ;
        
        private JobAction action;

        public ForeachJob(JobAction action)
        {
            this.action = action;
        }

        public void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3)
        {
            this.action(entity, ref c0, ref c1, ref c2, ref c3);
        }
    }
    
    public struct ForeachJob<C0, C1, C2, C3, C4> : IJob<C0, C1, C2, C3, C4> 
        where C0 : struct, IComponent
        where C1 : struct, IComponent
        where C2 : struct, IComponent
        where C3 : struct, IComponent
        where C4 : struct, IComponent
    {
        public delegate void JobAction(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4) ;
        
        private JobAction action;

        public ForeachJob(JobAction action)
        {
            this.action = action;
        }

        public void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4)
        {
            this.action(entity, ref c0, ref c1, ref c2, ref c3, ref c4);
        }
    }
    
    public struct ForeachJob<C0, C1, C2, C3, C4, C5> : IJob<C0, C1, C2, C3, C4, C5> 
        where C0 : struct, IComponent
        where C1 : struct, IComponent
        where C2 : struct, IComponent
        where C3 : struct, IComponent
        where C4 : struct, IComponent
        where C5 : struct, IComponent
    {
        public delegate void JobAction(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5) ;
        
        private JobAction action;

        public ForeachJob(JobAction action)
        {
            this.action = action;
        }

        public void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5)
        {
            this.action(entity, ref c0, ref c1, ref c2, ref c3, ref c4, ref c5);
        }
    }

    public partial struct Query
    {
        public void ForeachWithEntity<C0>(ForeachJob<C0>.JobAction action)
            where C0 : unmanaged, IComponent
        {
            ForeachWithJob<ForeachJob<C0>, C0>(new ForeachJob<C0>(action));
        }

        public void ForeachWithEntity<C0, C1>(ForeachJob<C0, C1>.JobAction action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
        {
            ForeachWithJob<ForeachJob<C0, C1>, C0, C1>(new ForeachJob<C0, C1>(action));
        }

        public void ForeachWithEntity<C0, C1, C2>(ForeachJob<C0, C1, C2>.JobAction action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
        {
            ForeachWithJob<ForeachJob<C0, C1, C2>, C0, C1, C2>(new ForeachJob<C0, C1, C2>(action));
        }

        public void ForeachWithEntity<C0, C1, C2, C3>(ForeachJob<C0, C1, C2, C3>.JobAction action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
        {
            ForeachWithJob<ForeachJob<C0, C1, C2, C3>, C0, C1, C2, C3>(new ForeachJob<C0, C1, C2, C3>(action));
        }

        public void ForeachWithEntity<C0, C1, C2, C3, C4>(ForeachJob<C0, C1, C2, C3, C4>.JobAction action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
            where C4 : unmanaged, IComponent
        {
            ForeachWithJob<ForeachJob<C0, C1, C2, C3, C4>, C0, C1, C2, C3, C4>(new ForeachJob<C0, C1, C2, C3, C4>(action));
        }

        public void ForeachWithEntity<C0, C1, C2, C3, C4, C5>(ForeachJob<C0, C1, C2, C3, C4, C5>.JobAction action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
            where C4 : unmanaged, IComponent
            where C5 : unmanaged, IComponent
        {
            ForeachWithJob<ForeachJob<C0, C1, C2, C3, C4, C5>, C0, C1, C2, C3, C4, C5>(new ForeachJob<C0, C1, C2, C3, C4, C5>(action));
        }
    }
}