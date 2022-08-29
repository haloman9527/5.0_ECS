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
        private readonly List<ISystem> internalBeforeSystems = new List<ISystem>();
        private readonly List<ISystem> customSystems = new List<ISystem>();
        private readonly List<ISystem> internalAfterSystems = new List<ISystem>();

        public IReadOnlyList<ISystem> CustomSystems
        {
            get { return customSystems; }
        }

        public void AddSystem(ISystem system)
        {
            if (customSystems.Contains(system))
                throw new Exception("Already exists same type system!");
            customSystems.Add(system);
        }

        public void InsertSystem(int index, ISystem system)
        {
            if (customSystems.Contains(system))
                throw new Exception("Already exists same type system!");
            customSystems.Insert(index, system);
            if (system is IOnAwake sys)
                sys.OnAwake();
        }

        public bool RemoveSystem(ISystem system)
        {
            return customSystems.Remove(system);
        }

        /// <summary> 获取所有System，包含内置System </summary>
        public IEnumerable<ISystem> GetAllSystems()
        {
            foreach (var system in internalBeforeSystems)
            {
                yield return system;
            }
            foreach (var system in customSystems)
            {
                yield return system;
            }
            foreach (var system in internalAfterSystems)
            {
                yield return system;
            }
        }

        public void FixedUpdate()
        {
            foreach (var system in GetAllSystems())
            {
                if (system is IFixedUpdate sys)
                    sys.OnFixedUpdate();
            }
        }

        public void Update()
        {
            foreach (var system in GetAllSystems())
            {
                if (system is IUpdate sys)
                    sys.OnUpdate();
            }
        }

        public void LateUpdate()
        {
            foreach (var system in GetAllSystems())
            {
                if (system is ILateUpdate sys)
                    sys.OnLateUpdate();
            }
        }

        public void DestroySystem(ISystem system)
        {
            if (!customSystems.Contains(system))
                throw new Exception($"{nameof(customSystems)}中不存在该对象");
            if (system is IDestroy sys)
                sys.OnDestroy();
            RemoveSystem(system);
        }
    }
}
