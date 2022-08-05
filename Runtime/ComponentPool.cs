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
using System.Runtime.InteropServices;
using Unity.Collections;

namespace CZToolKit.ECS
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ComponentPool : IDisposable
    {
        private const int DEFAULT_CAPACITY = 128;

        private int componentSize;
        private void* componentsPtr;

        public ComponentPool(Type componentType, int defaultCapacity = DEFAULT_CAPACITY)
        {
            this.componentSize = Marshal.SizeOf(componentType);


            var components = new NativeHashMap<int, IntPtr>(defaultCapacity, Allocator.Persistent);
            this.componentsPtr = Unsafe.AsPointer(ref components);
        }

        public bool Contains(Entity entity)
        {
            ref var components = ref Unsafe.AsRef<NativeHashMap<Entity, IntPtr>>(componentsPtr);
            return components.ContainsKey(entity);
        }

        public unsafe T* Get<T>(Entity entity) where T : unmanaged, IComponent
        {
            var components = Unsafe.Read<NativeHashMap<Entity, IntPtr>>(componentsPtr);
            return (T*)components[entity];
        }

        public unsafe void Set<T>(Entity entity, T component) where T : unmanaged, IComponent
        {
            ref var components = ref Unsafe.AsRef<NativeHashMap<Entity, IntPtr>>(componentsPtr);
            components[entity] = new IntPtr(&component);
        }

        public unsafe void Set(Entity entity, object component)
        {
            var ptr = Marshal.AllocHGlobal(componentSize);
            Unsafe.Copy((void*)ptr, ref component);
            ref var components = ref Unsafe.AsRef<NativeHashMap<Entity, IntPtr>>(componentsPtr);
            components[entity] = ptr;
        }

        public void Del(Entity entity)
        {
            ref var components = ref Unsafe.AsRef<NativeHashMap<Entity, IntPtr>>(componentsPtr);
            Marshal.FreeHGlobal(components[entity]);
            components.Remove(entity);
        }

        public NativeArray<Entity> GetEntities(Allocator allocator)
        {
            var components = Unsafe.Read<NativeHashMap<Entity, IntPtr>>(componentsPtr);
            return components.GetKeyArray(allocator);
        }

        public void Dispose()
        {
            //ref var components = ref Unsafe.AsRef<NativeHashMap<Entity, IntPtr>>(componentsPtr);
            //var ptrs = components.GetValueArray(Allocator.Temp);
            //for (int i = 0; i < ptrs.Length; i++)
            //{
            //    Marshal.FreeHGlobal(ptrs[i]);
            //}
            //components.Dispose();
        }
    }
}
