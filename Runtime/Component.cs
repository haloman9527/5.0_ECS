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
    public interface IComponent { }

    public interface IComponentPool : IDisposable
    {
        Type ComponentType { get; }

        unsafe bool Contains(Entity entity);

        unsafe object Get(Entity entity);

        unsafe void Set(Entity entity, object value);

        unsafe void Del(Entity entity);

        void Clear();

        NativeArray<Entity> GetEntities(Allocator allocator);
    }

    public interface IComponentPool<T> : IDisposable where T : struct
    {
        Type ComponentType { get; }

        unsafe bool Contains(Entity entity);

        unsafe T Get(Entity entity);

        unsafe bool TryGet(Entity entity, out T component);

        unsafe ref T Ref(Entity entity);

        unsafe void Set(Entity entity, T value);

        unsafe void Del(Entity entity);

        void Clear();

        NativeArray<Entity> GetEntities(Allocator allocator);
    }

    public class ComponentPool<T> : IComponentPool, IComponentPool<T>, IDisposable where T : struct
    {
        private const int DEFAULT_CAPACITY = 128;

        private int componentSize;
        private NativeHashMap<Entity, IntPtr> components;

        public Type ComponentType { get { return typeof(T); } }

        public ComponentPool(int defaultCapacity = DEFAULT_CAPACITY)
        {
            this.componentSize = Marshal.SizeOf<T>();
            this.components = new NativeHashMap<Entity, IntPtr>(defaultCapacity, Allocator.Persistent);
        }

        public unsafe bool Contains(Entity entity)
        {
            return components.ContainsKey(entity);
        }

        unsafe object IComponentPool.Get(Entity entity)
        {
            return Get(entity);
        }

        public unsafe T Get(Entity entity)
        {
            return Marshal.PtrToStructure<T>(components[entity]);
        }

        public unsafe bool TryGet(Entity entity, out T component)
        {
            if (!components.TryGetValue(entity, out var ptr))
            {
                component = default;
                return false;
            }
            component = Marshal.PtrToStructure<T>(ptr);
            return true;
        }

        public unsafe ref T Ref(Entity entity)
        {
            return ref Unsafe.AsRef<T>(components[entity].ToPointer());
        }

        public unsafe void Set(Entity entity, object value)
        {
            Set(entity, (T)value);
        }

        public unsafe void Set(Entity entity, T value)
        {
            if (!components.TryGetValue(entity, out var ptr))
                components[entity] = ptr = Marshal.AllocHGlobal(componentSize);
            Unsafe.Copy(ptr.ToPointer(), ref value);
        }

        public unsafe void Del(Entity entity)
        {
            if (components.TryGetValue(entity, out var ptr))
            {
                Marshal.FreeHGlobal(ptr);
                components.Remove(entity);
            }
        }

        public void Clear()
        {
            foreach (var ptr in components.GetValueArray(Allocator.Temp))
            {
                Marshal.FreeHGlobal(ptr);
            }
            components.Clear();
        }

        public NativeArray<Entity> GetEntities(Allocator allocator)
        {
            return components.GetKeyArray(allocator);
        }

        public void Dispose()
        {
            foreach (var ptr in components.GetValueArray(Allocator.Temp))
            {
                Marshal.FreeHGlobal(ptr);
            }
            components.Dispose();
        }
    }
}
