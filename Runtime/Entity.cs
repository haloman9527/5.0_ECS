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

namespace CZToolKit.ECS
{
    public struct Entity : IEquatable<Entity>, IComparable<Entity>
    {
        public readonly int worldId;
        public readonly int id;

        internal Entity(int worldId, int id)
        {
            this.worldId = worldId;
            this.id = id;
        }

        public World World
        {
            get { return World.GetWorld(worldId); }
        }

        public static bool operator ==(in Entity lhs, in Entity rhs)
        {
            return lhs.worldId == rhs.worldId && lhs.id == rhs.id;
        }

        public static bool operator !=(in Entity lhs, in Entity rhs)
        {
            return lhs.worldId != rhs.worldId || lhs.id != rhs.id;
        }

        public int CompareTo(Entity other)
        {
            return this.id.CompareTo(other.id);
        }

        public override bool Equals(object other)
        {
            return Equals((Entity)other);
        }

        public bool Equals(Entity other)
        {
            return this.worldId == other.worldId && this.id == other.id;
        }

        public override int GetHashCode()
        {
            return int.MinValue + id;
        }

        public override string ToString()
        {
            return $"World:{worldId}  Entity:{id}";
        }
    }
}