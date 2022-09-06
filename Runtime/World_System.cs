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
        private readonly List<ComponentSystem> beforeSystems = new List<ComponentSystem>();
        private readonly List<ComponentSystem> customSystems = new List<ComponentSystem>();
        private readonly List<ComponentSystem> afterSystems = new List<ComponentSystem>();
        private readonly HashSet<Type> existSystemTypes = new HashSet<Type>();

        public IReadOnlyList<ComponentSystem> CustomSystems
        {
            get { return customSystems; }
        }

        private ComponentSystem CreateSystem(Type systemType)
        {
            var system = Activator.CreateInstance(systemType) as ComponentSystem;
            system.Enable = true;
            system.World = this;
            system.Filter = new Filter(this);
            return system;
        }

        private void CheckExists(Type systemType)
        {
            if (existSystemTypes.Contains(systemType))
                throw new Exception("已存在");
        }

        private T AddBeforeSystem<T>() where T : ComponentSystem, new()
        {
            return AddBeforeSystem(typeof(T)) as T;
        }

        private ComponentSystem AddBeforeSystem(Type systemType)
        {
            CheckExists(systemType);
            var system = CreateSystem(systemType);
            beforeSystems.Add(system);
            existSystemTypes.Add(systemType);
            system.OnCreate();
            return system;
        }

        private T AddAfterSystem<T>() where T : ComponentSystem, new()
        {
            return AddAfterSystem(typeof(T)) as T;
        }

        private ComponentSystem AddAfterSystem(Type systemType)
        {
            CheckExists(systemType);
            var system = CreateSystem(systemType);
            afterSystems.Add(system);
            existSystemTypes.Add(systemType);
            system.OnCreate();
            return system;
        }

        public T AddSystem<T>() where T : ComponentSystem, new()
        {
            return AddSystem(typeof(T)) as T;
        }

        public ComponentSystem AddSystem(Type systemType)
        {
            CheckExists(systemType);
            var system = CreateSystem(systemType);
            afterSystems.Add(system);
            existSystemTypes.Add(systemType);
            system.OnCreate();
            return system;
        }

        /// <summary> 获取所有System，包含内置System </summary>
        public IEnumerable<ComponentSystem> GetAllSystems()
        {
            foreach (var system in beforeSystems)
            {
                yield return system;
            }
            foreach (var system in customSystems)
            {
                yield return system;
            }
            foreach (var system in afterSystems)
            {
                yield return system;
            }
        }

        public void FixedUpdate()
        {
            foreach (var system in GetAllSystems())
            {
                if (system.Enable && system is IFixedUpdate)
                    system.OnUpdate();
            }
        }

        public void Update()
        {
            foreach (var system in GetAllSystems())
            {
                if (system.Enable && system is IUpdate)
                    system.OnUpdate();
            }
        }

        public void LateUpdate()
        {
            foreach (var system in GetAllSystems())
            {
                if (system.Enable && system is ILateUpdate)
                    system.OnUpdate();
            }
        }
    }
}
