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

namespace CZToolKit.ECS
{
    public struct Entity : IEquatable<Entity>, IComparable<Entity>
    {
        public readonly int index;

        internal Entity(int index)
        {
            this.index = index;
        }

        public static bool operator ==(in Entity lhs, in Entity rhs)
        {
            return lhs.index == rhs.index;
        }

        public static bool operator !=(in Entity lhs, in Entity rhs)
        {
            return lhs.index != rhs.index;
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
            return this.index == other.index;
        }

        public override int GetHashCode()
        {
            return index;
        }

        public override string ToString()
        {
            return $"Entity:{index}";
        }
    }
}