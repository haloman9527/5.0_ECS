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
//     public unsafe struct ArchetypeContainer : IDisposable
//     {
//         UnsafeList<Archetype> archetypes;
//         public int typeCount;
//         public ComponentType* componentTypes;
//         public int archetypeCount;
//         
//         public ArchetypeContainer(params ComponentType[] types)
//         {
//             typeCount = types.Length;
//             archetypeCount = 0;
//             fixed (ComponentType* typesPtr = types)
//             {
//                 componentTypes = (ComponentType*)UnsafeUtil.Malloc(sizeof(ComponentType) * typeCount);
//                 for (int i = 0; i < typeCount; i++)
//                 {
//                     componentTypes[i] = typesPtr[i];
//                 }
//             }
//
//             archetypes = new UnsafeList<Archetype>();
//         }
//
//         public void Dispose()
//         {
//             archetypes.Dispose();
//             UnsafeUtil.Free(new IntPtr(componentTypes));
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