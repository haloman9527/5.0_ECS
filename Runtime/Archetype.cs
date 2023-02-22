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
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace CZToolKit.ECS
{
    public unsafe class ArchetypeContainer
    { 
        UnsafeList<Archetype>* archetypes;
        
        public unsafe ArchetypeContainer(int capacity)
        {
            archetypes = new UnsafeList<Archetype>(capacity, Allocator.Invalid);
            
            var ptr = UnsafeUtility.Malloc(4, 4, Allocator.Persistent);
            UnsafeUtility.CopyStructureToPtr(ref components, ptr);
        }
    }

    public unsafe struct Archetype
    {
        public Archetype(int typeID)
        {
            var chunkAlign = 10;

            var components = new UnsafeHashMap<Entity, IntPtr>(16, Allocator.Persistent);
            var ptrSize = UnsafeUtility.SizeOf<UnsafeHashMap<Entity, IntPtr>>();
            var ptr = UnsafeUtility.Malloc(ptrSize, chunkAlign, Allocator.Persistent);
            UnsafeUtility.CopyStructureToPtr(ref components, ptr);

            componentsPtr = new IntPtr(ptr);
        }
    }
}