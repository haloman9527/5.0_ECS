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
        public int componentTypeIndex;
        public int componentSize;
        public IntPtr componentsPtr;

        public ComponentsContainer(int componentTypeID, int componentSize, int capacity)
        {
            this.componentTypeIndex = componentTypeID;
            this.componentSize = componentSize;
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

        public ref T Ref<T>(Entity entity) where T : unmanaged, IComponent
        {
            ref var components = ref Unsafe.AsRef<UnsafeHashMap<Entity, IntPtr>>((void*)componentsPtr);
            return ref Unsafe.AsRef(*((T*)components[entity]));
        }

        public T Get<T>(Entity entity) where T : unmanaged, IComponent
        {
            ref var components = ref Unsafe.AsRef<UnsafeHashMap<Entity, IntPtr>>((void*)componentsPtr);
            return *((T*)components[entity]);
        }

        public bool TryGet<T>(Entity entity, out T value) where T : unmanaged, IComponent
        {
            ref var components = ref Unsafe.AsRef<UnsafeHashMap<Entity, IntPtr>>((void*)componentsPtr);
            if (components.TryGetValue(entity, out var ptr))
            {
                value = *((T*)ptr);
                return true;
            }
            value = default;
            return false;
        }

        public void Set<T>(Entity entity, T component) where T : unmanaged, IComponent
        {
            var p = UnsafeUtility.Malloc(componentSize, 4, Allocator.Persistent);
            UnsafeUtility.CopyStructureToPtr(ref component, p);
            ref var components = ref Unsafe.AsRef<UnsafeHashMap<Entity, IntPtr>>((void*)componentsPtr);
            components[entity] = new IntPtr(p);
        }

        public ref UnsafeHashMap<Entity, IntPtr> GetMap()
        {
            return ref Unsafe.AsRef<UnsafeHashMap<Entity, IntPtr>>((void*)componentsPtr);
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
            components.Clear();
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
