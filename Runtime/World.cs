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
 *  Blog: https://www.haloman.net/
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

        private static IndexGenerator s_WorldIDGenerator = new IndexGenerator();
        private static World s_DefaultWorld;
        private readonly static List<World> s_AllWorlds = new List<World>();

        public static World DefaultWorld
        {
            get
            {
                if (s_DefaultWorld == null)
                    s_DefaultWorld = new World("Main World");
                return s_DefaultWorld;
            }
        }

        public static IReadOnlyList<World> AllWorlds
        {
            get { return s_AllWorlds; }
        }

        public static void DisposeAllWorld()
        {
            while (s_AllWorlds.Count > 0)
            {
                s_AllWorlds[0].Dispose();
            }

            s_AllWorlds.Clear();
        }

        #endregion

        public readonly int id;
        public readonly string name;
        public readonly Entity singleton;
        public readonly Systems systems = new Systems();

        public bool IsDisposed
        {
            get { return id == 0; }
        }

        private World(string name)
        {
            this.name = name;
            this.id = s_WorldIDGenerator.Next();
            this.singleton = NewEntity();
            if (s_DefaultWorld == null)
                s_DefaultWorld = this;
            s_AllWorlds.Add(this);
        }

        ~World()
        {
            Dispose();
        }

        public void Dispose()
        {
            DestroyEntities();

            entities.Dispose();
            componentContainers.Dispose();
            if (s_DefaultWorld == this)
                s_DefaultWorld = null;
            s_AllWorlds.Remove(this);

            systems.Clear();
        }
    }
}