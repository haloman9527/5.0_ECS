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
        private bool enable = false;

        public bool Enable
        {
            get { return enable; }
            set
            {
                if (enable == value)
                    return;
                enable = value;
                if (enable)
                    OnEnable();
                else
                    OnDisable();
            }
        }

        public World World { get; internal set; }

        public Filter Filter { get; internal set; }
        
        public virtual void OnCreate() { }

        public virtual void OnEnable() { }

        public abstract void OnUpdate();

        public virtual void OnDisable() { }
    }
}