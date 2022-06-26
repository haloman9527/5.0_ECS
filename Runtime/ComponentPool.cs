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
    public unsafe struct ComponentPool : IDisposable
    {
        private const int DEFAULT_CAPACITY = 128;

        private int componentSize;
        private NativeHashMap<Entity, IntPtr> components;

        public ComponentPool(Type componentType, int defaultCapacity = DEFAULT_CAPACITY)
        {
            this.componentSize = Marshal.SizeOf(componentType);
            this.components = new NativeHashMap<Entity, IntPtr>(defaultCapacity, Allocator.Persistent);
        }

        public bool Contains(Entity entity)
        {
            return components.ContainsKey(entity);
        }

        public unsafe ref T Get<T>(Entity entity) where T : struct, IComponent
        {
            return ref Unsafe.AsRef<T>((void*)components[entity]);
        }

        public unsafe void Set<T>(Entity entity, T component) where T : struct, IComponent
        {
            if (!components.TryGetValue(entity, out var ptr))
                components[entity] = ptr = Marshal.AllocHGlobal(componentSize);
            Unsafe.Copy((void*)ptr, ref component);
        }

        public unsafe void Set(Entity entity, object component)
        {
            if (!components.TryGetValue(entity, out var ptr))
                components[entity] = ptr = Marshal.AllocHGlobal(componentSize);
            Unsafe.Copy((void*)ptr, ref component);
        }

        public void Del(Entity entity)
        {
            Marshal.FreeHGlobal(components[entity]);
            components.Remove(entity);
        }

        public NativeArray<Entity> GetEntities(Allocator allocator)
        {
            return components.GetKeyArray(allocator);
        }

        public void Dispose()
        {
            var ptrs = components.GetValueArray(Allocator.Temp);
            for (int i = 0; i < ptrs.Length; i++)
            {
                Marshal.FreeHGlobal(ptrs[i]);
            }
            components.Dispose();
        }
    }
}
