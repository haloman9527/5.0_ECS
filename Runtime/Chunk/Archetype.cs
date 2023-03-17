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

using CZToolKit.Common.UnsafeEx;
using System;
using Unity.Collections.LowLevel.Unsafe;

namespace CZToolKit.ECS
{
    public unsafe struct Chunk
    {
        public const int CHUNK_SIZE = 1024 * 16;
        
        public Archetype* archetype;
        public int inArchetypeIndex;
    }

    public unsafe struct ChunkList
    {
        public Chunk** chunk;
        public int length;
        public int capacity;

        public UnsafeList ListData()
        {
            return UnsafeUtil.As<ChunkList, UnsafeList>(ref this);
        }
    }
    
    public unsafe struct Archetype
    {
        public readonly int typeCount;
        public readonly int* offsets;
        public readonly ComponentType* types;

        public readonly ChunkList chunkList;
    }
}