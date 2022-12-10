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

    public abstract class ComponentSystem: ISystem
    {
        private bool enable = true;

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

        public World World { get; set; }

        public Filter Filter { get; set; }

        public void OnCreate()
        {
            Enable = true;
            Create();
        }
        
        public void OnUpdate()
        {
            if (!enable)
                return;
            Update();
        }

        public void OnDestroy()
        {
            Enable = false;
            Destroy();
        }
        
        protected virtual void Create() { }

        protected virtual void OnEnable() { }

        protected abstract void Update();

        protected virtual void OnDisable() { }

        protected virtual void Destroy() { }
    }
}