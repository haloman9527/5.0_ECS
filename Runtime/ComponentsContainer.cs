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
 *  Github: https://github.com/haloman9527
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion
using System;
using CZToolKit.UnsafeEx;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace CZToolKit.ECS
{
    public unsafe struct ComponentsContainer : IDisposable
    {
        public TypeInfo componentTypeInfo;
        public IntPtr componentsPtr;

        public ComponentsContainer(TypeInfo componentTypeInfo, int capacity)
        {
            this.componentTypeInfo = componentTypeInfo;
            var components = new UnsafeParallelHashMap<Entity, IntPtr>(capacity, Allocator.Persistent);
            var componentsSize = UnsafeUtility.SizeOf<UnsafeParallelHashMap<Entity, IntPtr>>();
            var componentsAlign = UnsafeUtility.AlignOf<UnsafeParallelHashMap<Entity, IntPtr>>();
            var ptr = UnsafeUtility.Malloc(componentsSize, componentsAlign, Allocator.Persistent);
            UnsafeUtility.CopyStructureToPtr(ref components, ptr);
            componentsPtr = new IntPtr(ptr);
        }

        public bool Contains(Entity entity)
        {
            ref var components = ref UnsafeUtil.AsRef<UnsafeParallelHashMap<Entity, IntPtr>>((void*)componentsPtr);
            return components.ContainsKey(entity);
        }

        public ref T Ref<T>(Entity entity) where T : unmanaged, IComponent
        {
            ref var components = ref UnsafeUtil.AsRef<UnsafeParallelHashMap<Entity, IntPtr>>((void*)componentsPtr);
            return ref UnsafeUtil.AsRef<T>(components[entity]);
        }

        public T Get<T>(Entity entity) where T : unmanaged, IComponent
        {
            ref var components = ref UnsafeUtil.AsRef<UnsafeParallelHashMap<Entity, IntPtr>>((void*)componentsPtr);
            return *((T*)components[entity]);
        }

        public bool TryGet<T>(Entity entity, out T value) where T : unmanaged, IComponent
        {
            ref var components = ref UnsafeUtil.AsRef<UnsafeParallelHashMap<Entity, IntPtr>>((void*)componentsPtr);
            if (components.TryGetValue(entity, out var ptr))
            {
                value = *((T*)ptr);
                return true;
            }
            value = default;
            return false;
        }

        public void Set<T>(Entity entity, T component) where T : struct, IComponent
        {
            var p = UnsafeUtility.Malloc(componentTypeInfo.componentSize, 4, Allocator.Persistent);
            UnsafeUtility.CopyStructureToPtr(ref component, p);
            ref var components = ref UnsafeUtil.AsRef<UnsafeParallelHashMap<Entity, IntPtr>>((void*)componentsPtr);
            components[entity] = new IntPtr(p);
        }

        public ref UnsafeParallelHashMap<Entity, IntPtr> GetMap()
        {
            return ref UnsafeUtil.AsRef<UnsafeParallelHashMap<Entity, IntPtr>>((void*)componentsPtr);
        }

        public void Del(Entity entity)
        {
            ref var components = ref UnsafeUtil.AsRef<UnsafeParallelHashMap<Entity, IntPtr>>((void*)componentsPtr);
            var ptr = (void*)components[entity];
            var typeInfo = TypeManager.GetTypeInfo(componentTypeInfo.typeIndex);
            if (typeInfo.IsManagedComponentType)
            {
                
            }
            UnsafeUtility.Free(ptr, Allocator.Persistent);
            components.Remove(entity);
        }

        public NativeArray<Entity> GetEntities(Allocator allocator)
        {
            ref var components = ref UnsafeUtil.AsRef<UnsafeParallelHashMap<Entity, IntPtr>>((void*)componentsPtr);
            return components.GetKeyArray(allocator);
        }

        public int Count()
        {
            ref var components = ref UnsafeUtil.AsRef<UnsafeParallelHashMap<Entity, IntPtr>>((void*)componentsPtr);
            return components.Count();
        }

        public void Reset()
        {
            ref var components = ref UnsafeUtil.AsRef<UnsafeParallelHashMap<Entity, IntPtr>>((void*)componentsPtr);
            var ptrs = components.GetValueArray(Allocator.Temp);
            foreach (var ptr in ptrs)
            {
                UnsafeUtility.Free((void*)ptr, Allocator.Persistent);
            }
            components.Clear();
        }

        public void Dispose()
        {
            ref var components = ref UnsafeUtil.AsRef<UnsafeParallelHashMap<Entity, IntPtr>>((void*)componentsPtr);
            var ptrs = components.GetValueArray(Allocator.Temp);
            foreach (var ptr in ptrs)
            {
                UnsafeUtility.Free((void*)ptr, Allocator.Persistent);
            }
            components.Dispose();
        }
    }
}
