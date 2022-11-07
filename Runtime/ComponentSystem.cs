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

namespace CZToolKit.ECS
{
    public abstract class ComponentSystem
    {
        public bool Enable
        {
            get;
            set;
        }
        
        public World World
        {
            get;
            internal set;
        }
        
        public Filter Filter
        {
            get;
            internal set;
        }

        public virtual void OnCreate() { }

        public abstract void OnUpdate();
    }
}
