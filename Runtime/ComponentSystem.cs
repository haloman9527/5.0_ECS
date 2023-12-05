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
 *  Blog: https://www.mindgear.net/
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
            get { return active; }
            set
            {
                if (active == value)
                    return;
                active = value;
                if (active)
                    OnEnable();
                else
                    OnDisable();
            }
        }

        public World World { get; set; }

        public Filter Filter { get; set; }

        public void OnCreate()
        {
            Active = true;
            Create();
        }
        
        public void OnUpdate()
        {
            if (!active)
                return;
            Update();
        }

        public void OnDestroy()
        {
            Active = false;
            Destroy();
        }
        
        protected virtual void Create() { }

        protected virtual void OnEnable() { }

        protected abstract void Update();

        protected virtual void OnDisable() { }

        protected virtual void Destroy() { }
    }
}