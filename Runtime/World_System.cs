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
using System;
using System.Collections.Generic;

namespace CZToolKit.ECS
{
    public partial class World : IDisposable
    {
        private readonly List<ISystem> systems = new List<ISystem>();

        public IReadOnlyList<ISystem> Systems
        {
            get { return systems; }
        }

        public void AddSystem(ISystem system)
        {
            if (systems.Contains(system))
                throw new Exception("Already exists same type system!");
            systems.Add(system);
        }

        public void InsertSystem(int index, ISystem system)
        {
            if (systems.Contains(system))
                throw new Exception("Already exists same type system!");
            systems.Insert(index, system);
            if (system is IOnAwake sys)
                sys.OnAwake();
        }

        public bool RemoveSystem(ISystem system)
        {
            return systems.Remove(system);
        }

        public void FixedUpdate()
        {
            for (int i = 0; i < systems.Count; i++)
            {
                var system = systems[i];
                if (system is IFixedUpdate sys)
                    sys.OnFixedUpdate();
            }
        }

        public void Update()
        {
            for (int i = 0; i < systems.Count; i++)
            {
                var system = systems[i];
                if (system is IUpdate sys)
                    sys.OnUpdate();
            }
        }

        public void LateUpdate()
        {
            for (int i = 0; i < systems.Count; i++)
            {
                var system = systems[i];
                if (system is ILateUpdate sys)
                    sys.OnLateUpdate();
            }
        }

        public void DestroySystem(ISystem system)
        {
            if (!systems.Contains(system))
                throw new Exception("systems中不存在该对象");
            if (system is IDestroy sys)
                sys.OnDestroy();
            RemoveSystem(system);
        }
    }
}
