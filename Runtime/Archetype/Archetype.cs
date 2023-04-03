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

using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace CZToolKit.ECS
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct Archetype
    {
        [FieldOffset(0)]
        public int typeCount;
        [FieldOffset(4)]
        public int* offsets;
        [FieldOffset(12)]
        public ComponentType* types;
        [FieldOffset(20)]
        public UnsafeList chunks;

        public void AddChunk(Chunk* chunk)
        {
            chunk->inArchetypeIndex = chunks.Length;
            chunks.Add((ulong)chunk);
        }

        public void RemoveChunk(Chunk* chunk)
        {
            chunks.RemoveAtSwapBack<ulong>(chunk->inArchetypeIndex);
        }

        public void Sorted<T>(T* array) where T : unmanaged
        {
            
        }
    }
}