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
            private set;
        }

        static World()
        {
            NewWorld("Main World");
        }

        /// <summary> 创建一个标准World </summary>
        public static World NewWorld(string worldName)
        {
            World world = new World(worldName);
            world.AddAfterSystem<TransformSystem>();
            return world;
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
            if (DefaultWorld == null)
                DefaultWorld = this;
            allWorlds.Add(this);
        }

        ~World()
        {
            Dispose();
        }

        public void Reset()
        {
            customSystems.Clear();
            entities.Clear();
            foreach (var components in componentContainers.GetValueArray(Allocator.Temp))
            {
                components.Reset();
            }
        }

        public void Dispose()
        {
            customSystems.Clear();
            entities.Dispose();
            foreach (var components in componentContainers.GetValueArray(Allocator.Temp))
            {
                components.Dispose();
            }

            componentContainers.Dispose();
            if (DefaultWorld == this)
                DefaultWorld = null;
            allWorlds.Remove(this);
        }
    }
}