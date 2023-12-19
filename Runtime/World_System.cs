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
 *  Blog: https://www.mindgear.net/
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
        private readonly Dictionary<Type, int> systemsMap = new Dictionary<Type, int>();

        public IReadOnlyList<ISystem> Systems
        {
            get { return systems; }
        }

        public IReadOnlyDictionary<Type, int> SystemsMap
        {
            get { return systemsMap; }
        }

        public T AddSystem<T>() where T : ISystem, new()
        {
            var systemType = typeof(T);
            if (systemsMap.TryGetValue(systemType, out var i))
                return (T)systems[i];

            var system = new T();
            system.World = this;
            system.Filter = new Filter(this);
            system.OnCreate();
            systems.Add(system);
            systemsMap[systemType] = systems.Count - 1;
            return system;
        }

        public T InsertSystem<T>(int index) where T : ISystem, new()
        {
            var systemType = typeof(T);
            if (systemsMap.TryGetValue(systemType, out var i))
                return (T)systems[i];

            var system = new T();
            system.World = this;
            system.Filter = new Filter(this);
            system.OnCreate();
            systems.Insert(index, system);
            for (int j = 0; j < systems.Count; j++)
            {
                systemsMap[system.GetType()] = j;
            }

            return system;
        }

        public int GetSystemIndex(Type systemType)
        {
            if (systemsMap.TryGetValue(systemType, out var order))
                return order;
            return -1;
        }
        
        public void DestroySystems()
        {
            foreach (var system in systems)
            {
                system.OnDestroy();
            }

            systems.Clear();
        }
    }
}