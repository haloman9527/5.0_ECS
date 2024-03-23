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
        public readonly int index;

        internal Entity(int worldId, int index)
        {
            this.worldId = worldId;
            this.index = index;
        }

        public World World
        {
            get { return World.GetWorld(worldId); }
        }

        public static bool operator ==(in Entity lhs, in Entity rhs)
        {
            return lhs.worldId == rhs.worldId && lhs.index == rhs.index;
        }

        public static bool operator !=(in Entity lhs, in Entity rhs)
        {
            return lhs.worldId != rhs.worldId || lhs.index != rhs.index;
        }

        public int CompareTo(Entity other)
        {
            return this.index.CompareTo(other.index);
        }

        public override bool Equals(object other)
        {
            return Equals((Entity)other);
        }

        public bool Equals(Entity other)
        {
            return this.worldId == other.worldId && this.index == other.index;
        }

        public override int GetHashCode()
        {
            return int.MinValue + index;
        }

        public override string ToString()
        {
            return $"World:{worldId}  Entity:{index}";
        }
    }
}