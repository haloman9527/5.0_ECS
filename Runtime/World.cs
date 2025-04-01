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

namespace Atom.ECS
{
    public partial class World : IDisposable
    {
        #region Static

        private static readonly List<World> s_AllWorlds = new List<World>();

        public static World DefaultWorld => s_AllWorlds[0];

        public static IReadOnlyList<World> AllWorlds => s_AllWorlds;

        public static World GetWorld(int worldId)
        {
            if (worldId <= 0 || worldId > s_AllWorlds.Count)
            {
                return null;
            }

            return s_AllWorlds[worldId - 1];
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

        public int Id { get; private set; }
        public Entity Singleton { get; private set; }

        public bool IsDisposed => Id <= 0;

        public World()
        {
            lock (s_AllWorlds)
            {
                for (int i = 0; i < s_AllWorlds.Count; i++)
                {
                    if (s_AllWorlds[i] == null)
                    {
                        s_AllWorlds[i] = this;
                        this.Id = i + 1;
                        this.Singleton = this.CreateEntity();
                        return;
                    }
                }

                s_AllWorlds.Add(this);
                this.Id = s_AllWorlds.Count;
                this.Singleton = this.CreateEntity();
            }
        }

        public virtual void Dispose()
        {
            s_AllWorlds[this.Id - 1] = null;
            DisposeAllEntities();
            RemoveAllComponents();
            worldOperationListener?.OnWorldDispose(this);
            this.Id = 0;
        }
    }
}