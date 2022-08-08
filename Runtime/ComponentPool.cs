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
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace CZToolKit.ECS
{
    public unsafe struct ComponentPool : IDisposable
    {
        private const int DEFAULT_CAPACITY = 128;

        public int componentSize;
        public int alignment;
        public IntPtr componentsPtr;

        public ComponentPool(Type componentType, int defaultCapacity = DEFAULT_CAPACITY)
        {
            this.componentSize = UnsafeUtility.SizeOf(componentType);
            this.alignment = 4;
            var components = new NativeHashMap<Entity, IntPtr>(defaultCapacity, Allocator.Persistent);
            var ptr = UnsafeUtility.Malloc(UnsafeUtility.SizeOf<NativeHashMap<Entity, IntPtr>>(), UnsafeUtility.AlignOf<NativeHashMap<Entity, IntPtr>>(), Allocator.Persistent);
            UnsafeUtility.CopyStructureToPtr(ref components, ptr);
            componentsPtr = new IntPtr(ptr);
        }

        public bool Contains(Entity entity)
        {
            UnsafeUtility.CopyPtrToStructure<NativeHashMap<Entity, IntPtr>>((void*)componentsPtr, out var components);
            return components.ContainsKey(entity);
        }

        public unsafe T* Get<T>(Entity entity) where T : unmanaged, IComponent
        {
            var components = UnsafeUtility.ReadArrayElement<NativeHashMap<Entity, IntPtr>>((void*)componentsPtr, 0);
            return (T*)components[entity];
        }

        public unsafe void Set<T>(Entity entity, T component) where T : unmanaged, IComponent
        {
            var p = UnsafeUtility.Malloc(componentSize, 4, Allocator.Persistent);
            UnsafeUtility.CopyStructureToPtr(ref component, p);
            UnsafeUtility.CopyPtrToStructure<NativeHashMap<Entity, IntPtr>>((void*)componentsPtr, out var components);
            components[entity] = new IntPtr(p);
        }

        public unsafe void Set(Entity entity, object component)
        {
            var p = UnsafeUtility.Malloc(componentSize, 4, Allocator.Persistent);
            Unsafe.Copy(ref component, p);
            UnsafeUtility.CopyPtrToStructure<NativeHashMap<Entity, IntPtr>>((void*)componentsPtr, out var components);
            components[entity] = new IntPtr(p);
        }

        public void Del(Entity entity)
        {
            UnsafeUtility.CopyPtrToStructure<NativeHashMap<Entity, IntPtr>>((void*)componentsPtr, out var components);
            UnsafeUtility.Free((void*)components[entity], Allocator.Persistent);
            components.Remove(entity);
        }

        public NativeArray<Entity> GetEntities(Allocator allocator)
        {
            var components = UnsafeUtility.ReadArrayElement<NativeHashMap<Entity, IntPtr>>((void*)componentsPtr, 0);
            return components.GetKeyArray(allocator);
        }

        public void Dispose()
        {
            UnsafeUtility.CopyPtrToStructure<NativeHashMap<Entity, IntPtr>>((void*)componentsPtr, out var components);
            var ptrs = components.GetValueArray(Allocator.Temp);
            foreach (var ptr in ptrs)
            {
                UnsafeUtility.Free((void*)ptr, Allocator.Persistent);
            }
            components.Dispose();
            UnsafeUtility.Free((void*)componentsPtr, Allocator.Persistent);
        }
    }
}
