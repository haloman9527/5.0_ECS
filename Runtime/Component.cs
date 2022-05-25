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
            foreach (var item in componentsIndexMap.GetValueArray(Allocator.Temp))
            {
                if (components[item] is IDisposable disposable)
                    disposable.Dispose();
            }
            componentsIndexMap.Dispose();
        }

        public ComponentPool() : this(DEFAULT_SIZE) { }

        public unsafe bool Contains(Entity entity)
        {
            return componentsIndexMap.ContainsKey(entity);
        }

        unsafe object IComponentPool.Get(Entity entity)
        {
            return Get(entity);
        }

        public unsafe T Get(Entity entity)
        {
            return components[componentsIndexMap[entity]];
        }

        public unsafe bool TryGet(Entity entity, out T component)
        {
            if (!componentsIndexMap.TryGetValue(entity, out int index))
            {
                component = default;
                return false;
            }
            component = components[index];
            return true;
        }

        public unsafe ref T Ref(Entity entity)
        {
            return ref components[componentsIndexMap[entity]];
        }

        public unsafe void Set(Entity entity, object value)
        {
            Set(entity, (T)value);
        }

        public unsafe void Set(Entity entity, T value)
        {
            // 如果没有与entity对应的组件
            if (!componentsIndexMap.TryGetValue(entity, out int index))
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
            componentsIndexMap[entity] = index;
        }

        public unsafe void Del(Entity entity)
        {
            if (componentsIndexMap.TryGetValue(entity, out var index))
            {
                componentsIndexMap.Remove(entity);
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
