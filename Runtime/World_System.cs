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
        private readonly List<ISystem> beforeSystems = new List<ISystem>();
        private readonly List<ISystem> customSystems = new List<ISystem>();
        private readonly List<ISystem> afterSystems = new List<ISystem>();

        public IReadOnlyList<ISystem> CustomSystems
        {
            get { return customSystems; }
        }

        public void AddSystem(ISystem system)
        {
            if (customSystems.Contains(system))
                throw new Exception("Already exists same type system!");
            customSystems.Add(system);
            if (system is ISystemAwake sys)
                sys.OnAwake();
        }

        public void InsertSystem(int index, ISystem system)
        {
            if (customSystems.Contains(system))
                throw new Exception("Already exists same type system!");
            customSystems.Insert(index, system);
            if (system is ISystemAwake sys)
                sys.OnAwake();
        }

        public bool RemoveSystem(ISystem system)
        {
            return customSystems.Remove(system);
        }

        /// <summary> 获取所有System，包含内置System </summary>
        public IEnumerable<ISystem> GetAllSystems()
        {
            for (int i = 0; i < beforeSystems.Count; i++)
            {
                yield return beforeSystems[i];
            }
            for (int i = 0; i < customSystems.Count; i++)
            {
                yield return customSystems[i];
            }
            for (int i = 0; i < afterSystems.Count; i++)
            {
                yield return afterSystems[i];
            }
        }

        public void FixedUpdate()
        {
            foreach (var system in beforeSystems)
            {
                if (system is IFixedUpdate)
                    system.OnUpdate();
            }
            foreach (var system in customSystems)
            {
                if (system is IFixedUpdate)
                    system.OnUpdate();
            }
            foreach (var system in afterSystems)
            {
                if (system is IFixedUpdate)
                    system.OnUpdate();
            }
        }

        public void Update()
        {
            foreach (var system in beforeSystems)
            {
                if (system is IUpdate)
                    system.OnUpdate();
            }
            foreach (var system in customSystems)
            {
                if (system is IUpdate)
                    system.OnUpdate();
            }
            foreach (var system in afterSystems)
            {
                if (system is IUpdate)
                    system.OnUpdate();
            }
        }

        public void LateUpdate()
        {
            foreach (var system in beforeSystems)
            {
                if (system is ILateUpdate)
                    system.OnUpdate();
            }
            foreach (var system in customSystems)
            {
                if (system is ILateUpdate)
                    system.OnUpdate();
            }
            foreach (var system in afterSystems)
            {
                if (system is ILateUpdate)
                    system.OnUpdate();
            }
        }

        public void DestroySystem(ISystem system)
        {
            if (!customSystems.Contains(system))
                throw new Exception($"{nameof(customSystems)}中不存在该对象");
            if (system is ISystemDestroy sys)
                sys.OnDestroy();
            RemoveSystem(system);
        }
    }
}
