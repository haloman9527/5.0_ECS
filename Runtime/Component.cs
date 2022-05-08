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
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

namespace CZToolKit.ECS
{
    public interface IComponent { }

    public interface IComponentPool : IDisposable
    {
        Type ComponentType { get; }

        unsafe bool Contains(Entity* entityPtr);

        unsafe object Get(Entity* entityPtr);

        unsafe void Set(Entity* entityPtr, object value);

        unsafe void Del(Entity* entityPtr);

        void Clear();

        NativeArray<Entity> GetEntities(Allocator allocator);
    }

    public interface IComponentPool<T> : IDisposable where T : struct
    {
        Type ComponentType { get; }

        unsafe bool Contains(Entity* entityPtr);

        unsafe T Get(Entity* entityPtr);

        unsafe bool TryGet(Entity* entityPtr, out T component);

        unsafe ref T Ref(Entity* entityPtr);

        unsafe void Set(Entity* entityPtr, T value);

        unsafe void Del(Entity* entityPtr);

        void Clear();

        NativeArray<Entity> GetEntities(Allocator allocator);
    }

    public class ComponentPool<T> : IComponentPool, IComponentPool<T>, IDisposable where T : struct
    {
        private const int DEFAULT_SIZE = 128;

        private T[] components;
        private HashSet<int> unusedIndexs;
        private NativeHashMap<Entity, int> componentsIndexMap;

        public Type ComponentType
        {
            get
            {
                return typeof(T);
            }
        }

        public ComponentPool(int defaultSize)
        {
            this.components = new T[defaultSize];
            this.componentsIndexMap = new NativeHashMap<Entity, int>(defaultSize, Allocator.Persistent);
            unusedIndexs = new HashSet<int>();
            for (int i = 0; i < components.Length; i++)
            {
                unusedIndexs.Add(i);
            }
        }

        ~ComponentPool()
        {
            Dispose();
        }

        public void Dispose()
        {
            componentsIndexMap.Dispose();
        }

        public ComponentPool() : this(DEFAULT_SIZE) { }

        public unsafe bool Contains(Entity* entityPtr)
        {
            return componentsIndexMap.ContainsKey(*entityPtr);
        }

        unsafe object IComponentPool.Get(Entity* entityPtr)
        {
            return Get(entityPtr);
        }

        public unsafe T Get(Entity* entityPtr)
        {
            return components[componentsIndexMap[*entityPtr]];
        }

        public unsafe bool TryGet(Entity* entityPtr, out T component)
        {
            if (!componentsIndexMap.TryGetValue(*entityPtr, out int index))
            {
                component = default;
                return false;
            }
            component = components[index];
            return true;
        }

        public unsafe ref T Ref(Entity* entityPtr)
        {
            return ref components[componentsIndexMap[*entityPtr]];
        }

        public unsafe void Set(Entity* entityPtr, object value)
        {
            Set(entityPtr, (T)value);
        }

        public unsafe void Set(Entity* entityPtr, T value)
        {
            // 如果没有与entity对应的组件
            if (!componentsIndexMap.TryGetValue(*entityPtr, out int index))
            {
                // 查找未使用的Index
                bool foundUnsedIndex = false;

                if (unusedIndexs.Count > 0)
                {
                    index = unusedIndexs.First();
                    unusedIndexs.Remove(index);
                    foundUnsedIndex = true;
                }

                // 如果没有找到则扩容
                if (!foundUnsedIndex)
                {
                    index = components.Length;
                    Array.Resize(ref components, components.Length * 2);
                    for (int i = index; i < components.Length; i++)
                    {
                        unusedIndexs.Add(i);
                    }
                }
            }

            components[index] = value;
            componentsIndexMap[*entityPtr] = index;
        }

        public unsafe void Del(Entity* entityPtr)
        {
            if (componentsIndexMap.TryGetValue(*entityPtr, out var index))
            {
                componentsIndexMap.Remove(*entityPtr);
                unusedIndexs.Add(index);
            }
        }

        public void Clear()
        {
            componentsIndexMap.Clear();
            for (int i = 0; i < components.Length; i++)
            {
                unusedIndexs.Add(i);
            }
        }

        public NativeArray<Entity> GetEntities(Allocator allocator)
        {
            return componentsIndexMap.GetKeyArray(allocator);
        }
    }
}
