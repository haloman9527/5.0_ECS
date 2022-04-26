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

    public interface IComponentPool
    {
        bool Contains(Entity entity);

        void Del(Entity entity);

        void Set(Entity entity, object value);

        void Clear();

        NativeArray<Entity> GetEntities();
    }

    public interface IComponentPool<T> : IComponentPool where T : struct, IComponent
    {
        T Get(Entity entity);

        ref T Ref(Entity entity);

        void Set(Entity entity, T value);
    }

    public class ComponentPool<T> : IComponentPool, IComponentPool<T> where T : struct, IComponent
    {
        private const int DEFAULT_SIZE = 128;

        private T[] components;
        private HashSet<int> unusedIndexs;
        private NativeHashMap<Entity, int> componentsIndexMap;

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

        public ComponentPool() : this(DEFAULT_SIZE) { }

        public bool Contains(Entity entity)
        {
            return componentsIndexMap.ContainsKey(entity);
        }

        public T Get(Entity entity)
        {
            return components[componentsIndexMap[entity]];
        }

        public ref T Ref(Entity entity)
        {
            return ref components[componentsIndexMap[entity]];
        }

        public void Set(Entity entity, object value)
        {
            if (value is T tValue)
                Set(entity, tValue);
            else
                throw new Exception($"The value is not [{typeof(T).Name}]");
        }

        public void Set(Entity entity, T value)
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

        public void Del(Entity entity)
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

        public NativeArray<Entity> GetEntities()
        {
            return componentsIndexMap.GetKeyArray(Allocator.Temp);
        }
    }
}
