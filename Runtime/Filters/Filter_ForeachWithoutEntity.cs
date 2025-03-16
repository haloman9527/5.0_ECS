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
    public struct ForeachWithoutEntityJob<C0> : IJob<C0> where C0 : struct, IComponent
    {
        public delegate void JobAction(ref C0 c0);

        private JobAction action;

        public ForeachWithoutEntityJob(JobAction action)
        {
            this.action = action;
        }

        public void Execute(Entity entity, ref C0 c0)
        {
            this.action(ref c0);
        }
    }
    
    public struct ForeachWithoutEntityJob<C0, C1> : IJob<C0, C1> 
        where C0 : struct, IComponent
        where C1 : struct, IComponent
    {
        public delegate void JobAction(ref C0 c0, ref C1 c1) ;
        
        private JobAction action;

        public ForeachWithoutEntityJob(JobAction action)
        {
            this.action = action;
        }

        public void Execute(Entity entity, ref C0 c0, ref C1 c1)
        {
            this.action(ref c0, ref c1);
        }
    }
    
    public struct ForeachWithoutEntityJob<C0, C1, C2> : IJob<C0, C1, C2> 
        where C0 : struct, IComponent
        where C1 : struct, IComponent
        where C2 : struct, IComponent
    {
        public delegate void JobAction(ref C0 c0, ref C1 c1, ref C2 c2) ;
        
        private JobAction action;

        public ForeachWithoutEntityJob(JobAction action)
        {
            this.action = action;
        }

        public void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2)
        {
            this.action(ref c0, ref c1, ref c2);
        }
    }
    
    public struct ForeachWithoutEntityJob<C0, C1, C2, C3> : IJob<C0, C1, C2, C3> 
        where C0 : struct, IComponent
        where C1 : struct, IComponent
        where C2 : struct, IComponent
        where C3 : struct, IComponent
    {
        public delegate void JobAction(ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3) ;
        
        private JobAction action;

        public ForeachWithoutEntityJob(JobAction action)
        {
            this.action = action;
        }

        public void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3)
        {
            this.action(ref c0, ref c1, ref c2, ref c3);
        }
    }
    
    public struct ForeachWithoutEntityJob<C0, C1, C2, C3, C4> : IJob<C0, C1, C2, C3, C4> 
        where C0 : struct, IComponent
        where C1 : struct, IComponent
        where C2 : struct, IComponent
        where C3 : struct, IComponent
        where C4 : struct, IComponent
    {
        public delegate void JobAction(ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4) ;
        
        private JobAction action;

        public ForeachWithoutEntityJob(JobAction action)
        {
            this.action = action;
        }

        public void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4)
        {
            this.action(ref c0, ref c1, ref c2, ref c3, ref c4);
        }
    }
    
    public struct ForeachWithoutEntityJob<C0, C1, C2, C3, C4, C5> : IJob<C0, C1, C2, C3, C4, C5> 
        where C0 : struct, IComponent
        where C1 : struct, IComponent
        where C2 : struct, IComponent
        where C3 : struct, IComponent
        where C4 : struct, IComponent
        where C5 : struct, IComponent
    {
        public delegate void JobAction(ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5) ;
        
        private JobAction action;

        public ForeachWithoutEntityJob(JobAction action)
        {
            this.action = action;
        }

        public void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5)
        {
            this.action(ref c0, ref c1, ref c2, ref c3, ref c4, ref c5);
        }
    }

    public partial struct Query
    {
        public void ForeachWithoutEntity<C0>(ForeachWithoutEntityJob<C0>.JobAction action)
            where C0 : unmanaged, IComponent
        {
            ForeachWithJob<ForeachWithoutEntityJob<C0>, C0>(new ForeachWithoutEntityJob<C0>(action));
        }

        public void ForeachWithEntity<C0, C1>(ForeachWithoutEntityJob<C0, C1>.JobAction action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
        {
            ForeachWithJob<ForeachWithoutEntityJob<C0, C1>, C0, C1>(new ForeachWithoutEntityJob<C0, C1>(action));
        }

        public void ForeachWithEntity<C0, C1, C2>(ForeachWithoutEntityJob<C0, C1, C2>.JobAction action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
        {
            ForeachWithJob<ForeachWithoutEntityJob<C0, C1, C2>, C0, C1, C2>(new ForeachWithoutEntityJob<C0, C1, C2>(action));
        }

        public void ForeachWithEntity<C0, C1, C2, C3>(ForeachWithoutEntityJob<C0, C1, C2, C3>.JobAction action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
        {
            ForeachWithJob<ForeachWithoutEntityJob<C0, C1, C2, C3>, C0, C1, C2, C3>(new ForeachWithoutEntityJob<C0, C1, C2, C3>(action));
        }

        public void ForeachWithEntity<C0, C1, C2, C3, C4>(ForeachWithoutEntityJob<C0, C1, C2, C3, C4>.JobAction action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
            where C4 : unmanaged, IComponent
        {
            ForeachWithJob<ForeachWithoutEntityJob<C0, C1, C2, C3, C4>, C0, C1, C2, C3, C4>(new ForeachWithoutEntityJob<C0, C1, C2, C3, C4>(action));
        }

        public void ForeachWithEntity<C0, C1, C2, C3, C4, C5>(ForeachWithoutEntityJob<C0, C1, C2, C3, C4, C5>.JobAction action)
            where C0 : unmanaged, IComponent
            where C1 : unmanaged, IComponent
            where C2 : unmanaged, IComponent
            where C3 : unmanaged, IComponent
            where C4 : unmanaged, IComponent
            where C5 : unmanaged, IComponent
        {
            ForeachWithJob<ForeachWithoutEntityJob<C0, C1, C2, C3, C4, C5>, C0, C1, C2, C3, C4, C5>(new ForeachWithoutEntityJob<C0, C1, C2, C3, C4, C5>(action));
        }
    }
}