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

namespace CZToolKit.ECS
{
    public interface IComponent { }

    public interface IComponentPool
    {
        bool Contains(int entityID);

        void Del(int entityID);

        void Set(int entityID, object value);

        void Clear();

        IEnumerable<int> GetEntityIDs();
    }

    public interface IComponentPool<T> : IComponentPool where T : struct, IComponent
    {
        T Get(int entityID);

        ref T Ref(int entityID);

        void Set(int entityID, T value);
    }

    public class ComponentPool<T> : IComponentPool, IComponentPool<T> where T : struct, IComponent
    {
        private const int DEFAULT_SIZE = 128;

        private T[] components;
        private HashSet<int> unusedIndexs;
        private readonly Dictionary<int, int> componentsIndexMap = new Dictionary<int, int>();

        public int Count
        {
            get { return componentsIndexMap.Count; }
        }

        public ComponentPool(int defaultSize)
        {
            this.components = new T[defaultSize];
            unusedIndexs = new HashSet<int>();
            for (int i = 0; i < components.Length; i++)
            {
                unusedIndexs.Add(i);
            }
        }

        public ComponentPool() : this(DEFAULT_SIZE) { }

        public bool Contains(int entityID)
        {
            return componentsIndexMap.ContainsKey(entityID);
        }

        public T Get(int entityID)
        {
            return components[componentsIndexMap[entityID]];
        }

        public ref T Ref(int entityID)
        {
            return ref components[componentsIndexMap[entityID]];
        }

        public void Set(int entity, object value)
        {
            if (value is T tValue)
                Set(entity, tValue);
            else
                throw new Exception($"The value is not [{typeof(T).Name}]");
        }

        public void Set(int entityID, T value)
        {
            // 如果没有与entityID对应的组件
            if (!componentsIndexMap.TryGetValue(entityID, out int index))
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
            componentsIndexMap[entityID] = index;
        }

        public void Del(int entityID)
        {
            if (componentsIndexMap.TryGetValue(entityID, out var index))
            {
                componentsIndexMap.Remove(entityID);
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

        public IEnumerable<int> GetEntityIDs()
        {
            foreach (var key in componentsIndexMap.Keys)
            {
                yield return key;
            }
        }
    }
}
