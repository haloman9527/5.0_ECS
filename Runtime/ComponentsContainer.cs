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
    public unsafe struct ComponentsContainer : IDisposable
    {
        public const int DEFAULT_CAPACITY = 128;

        public int componentType;
        public int componentSize;
        public int alignment;
        public IntPtr componentsPtr;

        public ComponentsContainer(Type componentType, int capacity = DEFAULT_CAPACITY)
        {
            this.componentType = componentType.GetHashCode();
            this.componentSize = UnsafeUtility.SizeOf(componentType);
            this.alignment = 4;
            var components = new UnsafeHashMap<Entity, IntPtr>(capacity, Allocator.Persistent);

            var componentsSize = UnsafeUtility.SizeOf<UnsafeHashMap<Entity, IntPtr>>();
            var componentsAlign = UnsafeUtility.AlignOf<UnsafeHashMap<Entity, IntPtr>>();
            var ptr = UnsafeUtility.Malloc(componentsSize, componentsAlign, Allocator.Persistent);
            UnsafeUtility.CopyStructureToPtr(ref components, ptr);

            componentsPtr = new IntPtr(ptr);
        }

        public bool Contains(Entity entity)
        {
            ref var components = ref Unsafe.AsRef<UnsafeHashMap<Entity, IntPtr>>((void*)componentsPtr);
            return components.ContainsKey(entity);
        }

        public unsafe ref T Get<T>(Entity entity) where T : unmanaged, IComponent
        {
            ref var components = ref Unsafe.AsRef<UnsafeHashMap<Entity, IntPtr>>((void*)componentsPtr);
            return ref Unsafe.AsRef<T>((T*)components[entity]);
        }

        public unsafe void Set<T>(Entity entity, T component) where T : struct, IComponent
        {
            var p = UnsafeUtility.Malloc(componentSize, 4, Allocator.Persistent);
            UnsafeUtility.CopyStructureToPtr(ref component, p);
            ref var components = ref Unsafe.AsRef<UnsafeHashMap<Entity, IntPtr>>((void*)componentsPtr);
            components[entity] = new IntPtr(p);
        }

        public void Del(Entity entity)
        {
            ref var components = ref Unsafe.AsRef<UnsafeHashMap<Entity, IntPtr>>((void*)componentsPtr);
            UnsafeUtility.Free((void*)components[entity], Allocator.Persistent);
            components.Remove(entity);
        }

        public NativeArray<Entity> GetEntities(Allocator allocator)
        {
            ref var components = ref Unsafe.AsRef<UnsafeHashMap<Entity, IntPtr>>((void*)componentsPtr);
            return components.GetKeyArray(allocator);
        }

        public int Count()
        {
            ref var components = ref Unsafe.AsRef<UnsafeHashMap<Entity, IntPtr>>((void*)componentsPtr);
            return components.Count();
        }

        public void Reset()
        {
            ref var components = ref Unsafe.AsRef<UnsafeHashMap<Entity, IntPtr>>((void*)componentsPtr);
            var ptrs = components.GetValueArray(Allocator.Temp);
            foreach (var ptr in ptrs)
            {
                UnsafeUtility.Free((void*)ptr, Allocator.Persistent);
            }
        }

        public void Dispose()
        {
            ref var components = ref Unsafe.AsRef<UnsafeHashMap<Entity, IntPtr>>((void*)componentsPtr);
            var ptrs = components.GetValueArray(Allocator.Temp);
            foreach (var ptr in ptrs)
            {
                UnsafeUtility.Free((void*)ptr, Allocator.Persistent);
            }
            components.Dispose();
        }
    }
}
