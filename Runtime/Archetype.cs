// #region 注 释
//
// /***
//  *
//  *  Title:
//  *  
//  *  Description:
//  *  
//  *  Date:
//  *  Version:
//  *  Writer: 半只龙虾人
//  *  Github: https://github.com/HalfLobsterMan
//  *  Blog: https://www.crosshair.top/
//  *
//  */
//
// #endregion
//
// using System;
// using Unity.Collections;
// using Unity.Collections.LowLevel.Unsafe;
//
// namespace CZToolKit.ECS
// {
//     public unsafe class ArchetypeContainer : IDisposable
//     {
//         UnsafeList<Archetype>* archetypes;
//         
//         public int typeCount;
//         public int archetypeCount;
//         
//         public int 
//         
//         public unsafe ArchetypeContainer()
//         {
//             
//         }
//
//         public void Dispose()
//         {
//             
//         }
//     }
//     
//     public unsafe struct Archetype : IDisposable
//     {
//         public Archetype(int typeID)
//         {
//             var chunkAlign = 10;
//     
//             var components = new UnsafeHashMap<Entity, IntPtr>(16, Allocator.Persistent);
//             var ptrSize = UnsafeUtility.SizeOf<UnsafeHashMap<Entity, IntPtr>>();
//             var ptr = UnsafeUtility.Malloc(ptrSize, chunkAlign, Allocator.Persistent);
//             UnsafeUtility.CopyStructureToPtr(ref components, ptr);
//     
//             componentsPtr = new IntPtr(ptr);
//         }
//
//         public void Dispose()
//         {
//         }
//     }
// }