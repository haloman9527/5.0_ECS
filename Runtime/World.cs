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
using Unity.Collections;

namespace CZToolKit.ECS
{
    public partial class World : IDisposable
    {
        #region Static
        private static readonly List<World> allWorlds = new List<World>();

        public static IReadOnlyList<World> AllWorlds
        {
            get { return allWorlds; }
        }

        public static World DefaultWorld
        {
            get;
            set;
        }

        public static void DisposeWorld(World world)
        {
            world.Dispose();
        }

        public static void DisposeAllWorld()
        {
            foreach (var world in allWorlds)
            {
                world.Dispose();
            }
            allWorlds.Clear();
        }
        #endregion

        public readonly string name;
        public readonly Entity singleton;

        public World(string name)
        {
            this.name = name;
            this.singleton = NewEntity(-1);
            if (DefaultWorld == null)
                DefaultWorld = this;
            allWorlds.Add(this);
        }

        public void Dispose()
        {
            systems.Clear();
            entities.Dispose();
            foreach (var components in componentPools.GetValueArray(Allocator.Temp))
            {
                components.Dispose();
            }
            componentPools.Dispose();
            if (DefaultWorld == this)
                DefaultWorld = null;
            allWorlds.Remove(this);
        }
    }
}
