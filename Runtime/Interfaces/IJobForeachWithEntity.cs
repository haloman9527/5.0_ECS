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

namespace Atom.ECS
{
    public interface IJobForeachWithEntity_EC<C0> where C0 : unmanaged, IComponent
    {
        void Execute(Entity entity, ref C0 c0);
    }

    public interface IJobForeachWithEntity_ECC<C0, C1> where C0 : unmanaged, IComponent where C1 : unmanaged, IComponent
    {
        void Execute(Entity entity, ref C0 c0, ref C1 c1);
    }

    public interface IJobForeachWithEntity_ECCC<C0, C1, C2> where C0 : unmanaged, IComponent where C1 : unmanaged, IComponent where C2 : unmanaged, IComponent
    {
        void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2);
    }

    public interface IJobForeachWithEntity_ECCCC<C0, C1, C2, C3> where C0 : unmanaged, IComponent where C1 : unmanaged, IComponent where C2 : unmanaged, IComponent where C3 : unmanaged, IComponent
    {
        void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3);
    }

    public interface IJobForeachWithEntity_ECCCCC<C0, C1, C2, C3, C4> where C0 : unmanaged, IComponent where C1 : unmanaged, IComponent where C2 : unmanaged, IComponent where C3 : unmanaged, IComponent where C4 : unmanaged, IComponent
    {
        void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4);
    }

    public interface IJobForeachWithEntity_ECCCCCC<C0, C1, C2, C3, C4, C5> where C0 : unmanaged, IComponent where C1 : unmanaged, IComponent where C2 : unmanaged, IComponent where C3 : unmanaged, IComponent where C4 : unmanaged, IComponent where C5 : unmanaged, IComponent
    {
        void Execute(Entity entity, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5);
    }
}
