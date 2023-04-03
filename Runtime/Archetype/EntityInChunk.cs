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
    public unsafe struct EntityInChunk : IComparable<EntityInChunk>, IEquatable<EntityInChunk>
    {
        public Chunk* chunk;
        public int indexInChunk;
    
        public int CompareTo(EntityInChunk other)
        {
            ulong lhs = (ulong)chunk;
            ulong rhs = (ulong)other.chunk;
            int archetypeCompare = lhs < rhs ? -1 : 1;
            int indexCompare = indexInChunk - other.indexInChunk;
            return (lhs != rhs) ? archetypeCompare : indexCompare;
        }
    
        public bool Equals(EntityInChunk other)
        {
            return CompareTo(other) == 0;
        }
    }
}