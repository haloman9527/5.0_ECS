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
    public unsafe struct EntityInArchetype : IComparable<EntityInArchetype>, IEquatable<EntityInArchetype>
    {
        public Archetype* archetype;
        public int indexInArchetype;
    
        public int CompareTo(EntityInArchetype other)
        {
            ulong lhs = (ulong)archetype;
            ulong rhs = (ulong)other.archetype;
            int archetypeCompare = lhs < rhs ? -1 : 1;
            int indexCompare = indexInArchetype - other.indexInArchetype;
            return (lhs != rhs) ? archetypeCompare : indexCompare;
        }
    
        public bool Equals(EntityInArchetype other)
        {
            return CompareTo(other) == 0;
        }
    }
}