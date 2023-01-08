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
using System.Security.Claims;
using Unity.Collections;

namespace CZToolKit.ECS
{
    public partial class World : IDisposable
    {
        #region Static

        private static World defaultWorld;
        private static readonly List<World> allWorlds = new List<World>();

        public static World DefaultWorld
        {
            get
            {
                if (defaultWorld == null)
                    defaultWorld = new World("Main World");
                return defaultWorld;
            }
        }

        public static IReadOnlyList<World> AllWorlds
        {
            get { return allWorlds; }
        }

        public static World NewWorld(string worldName)
        {
            return new World(worldName);
        }

        public static void DisposeWorld(World world)
        {
            world.Dispose();
        }

        public static void DisposeAllWorld()
        {
            while (allWorlds.Count > 0)
            {
                allWorlds[0].Dispose();
            }

            allWorlds.Clear();
        }

        #endregion

        public readonly string name;
        public readonly Entity singleton;

        private World(string name)
        {
            this.name = name;
            this.singleton = NewEntity();
            if (defaultWorld == null)
                defaultWorld = this;
            allWorlds.Add(this);
        }

        ~World()
        {
            Dispose();
        }

        public void Reset()
        {
            DestroySystems();
            DestroyEntities();
        }

        public void Dispose()
        {
            DestroySystems();
            DestroyEntities();
            
            entities.Dispose();
            componentContainers.Dispose();
            if (defaultWorld == this)
                defaultWorld = null;
            allWorlds.Remove(this);
        }
    }
}