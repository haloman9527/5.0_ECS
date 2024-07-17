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

namespace CZToolKit.ECS
{
    public partial class World : IDisposable
    {
        #region Static

        private static IndexGenerator s_WorldIDGenerator = new IndexGenerator();
        private readonly static List<World> s_AllWorlds = new List<World>();
        private readonly static Dictionary<int, World> s_AllWorldsMap = new Dictionary<int, World>();

        public static IReadOnlyList<World> AllWorlds
        {
            get { return s_AllWorlds; }
        }

        public static World GetWorld(int worldId)
        {
            s_AllWorldsMap.TryGetValue(worldId, out var world);
            return world;
        }

        public static void DisposeAllWorld()
        {
            while (s_AllWorlds.Count > 0)
            {
                s_AllWorlds[0].Dispose();
            }

            s_AllWorlds.Clear();
            s_AllWorldsMap.Clear();
            s_WorldIDGenerator.Reset();
        }

        #endregion

        public readonly int id;

        public readonly Entity singleton;

        public bool IsDisposed
        {
            get { return id == 0; }
        }

        public World()
        {
            this.id = s_WorldIDGenerator.Next();
            s_AllWorlds.Add(this);
            s_AllWorldsMap.Add(this.id, this);
            this.singleton = this.CreateEntity();
        }

        public virtual void Dispose()
        {
            s_AllWorlds.Remove(this);
            s_AllWorldsMap.Remove(this.id);

            DisposeAllEntities();
            RemoveAllComponents();
        }
    }
}