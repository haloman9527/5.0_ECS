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

namespace CZToolKit.ECS
{
    public abstract class ComponentSystem : ISystem
    {
        private bool active = true;

        public bool Active
        {
            get => active;
            set => active = value;
        }

        public abstract void Execute();
    }
}