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
    
    public interface IManagedComponent : IComponent
    {
        int WorldId { get; set; }

        uint EntityId { get; set; }
    }

    public interface IManagedComponent<V> : IManagedComponent where V : class
    {
    }
}