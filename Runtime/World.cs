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

namespace Jiange.ECS
{
    public partial class World : IDisposable
    {
        #region Static

        private static IndexGenerator s_WorldIDGenerator = new IndexGenerator();
        private static readonly List<World> s_AllWorlds = new List<World>();
        private static readonly Dictionary<int, World> s_AllWorldsMap = new Dictionary<int, World>();

        public static World DefaultWorld => s_AllWorlds[0];

        public static IReadOnlyList<World> AllWorlds => s_AllWorlds;

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

        public int Id { get; private set; }
        public Entity Singleton { get; private set; }

        public bool IsDisposed => Id == 0;

        public World()
        {
            this.Id = s_WorldIDGenerator.Next();
            s_AllWorlds.Add(this);
            s_AllWorldsMap.Add(this.Id, this);
            this.Singleton = this.CreateEntity();
        }

        public virtual void Dispose()
        {
            s_AllWorlds.Remove(this);
            s_AllWorldsMap.Remove(this.Id);

            DisposeAllEntities();
            RemoveAllComponents();
        }
    }
}