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
        private readonly Dictionary<Type, ComponentSystem> systems = new Dictionary<Type, ComponentSystem>();
        private readonly HashSet<Type> existSystemTypes = new HashSet<Type>();

        public IReadOnlyDictionary<Type, ComponentSystem> Systems
        {
            get { return systems; }
        }

        public T CreateSystem<T>() where T : ComponentSystem, new()
        {
            return CreateSystem(typeof(T)) as T;
        }

        public ComponentSystem CreateSystem(Type type)
        {
            if (!systems.TryGetValue(type, out var system))
            {
                systems[type] = system = Activator.CreateInstance(type, true) as ComponentSystem;
                existSystemTypes.Add(type);
                system.World = this;
                system.Filter = new Filter(this);
                system.OnCreate();
                system.Enable = true;
            }

            return system;
        }

        public void FixedUpdate()
        {
            using (var enumerator = systems.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var system = enumerator.Current.Value;
                    if (system.Enable && system is IFixedUpdate)
                        system.OnUpdate();
                }
            }
        }

        public void Update()
        {
            using (var enumerator = systems.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var system = enumerator.Current.Value;
                    if (system.Enable && system is IUpdate)
                        system.OnUpdate();
                }
            }
        }

        public void LateUpdate()
        {
            using (var enumerator = systems.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var system = enumerator.Current.Value;
                    if (system.Enable && system is ILateUpdate)
                        system.OnUpdate();
                }
            }
        }
    }
}