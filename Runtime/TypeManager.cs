﻿using System;
using System.Collections.Generic;
using CZToolKit.UnsafeEx;

namespace CZToolKit.ECS
{
    public static class TypeManager
    {
        #region Const

        /// <summary>
        /// 最大组件数量
        /// </summary>
        private const int MAXIMUN_TYPE_COUNT = 1024;

        private const int MAXIMUN_SUPPORTED_ALIGNMENT = 16;

        /// <summary>
        /// 32位表示是否size为0.
        /// </summary>
        public const int ZERO_SIZE_FLAG = 1 << 31;

        /// <summary>
        /// 31位表示是否时托管类型组件.
        /// </summary>
        public const int MANAGED_COMPONENT_FLAG = 1 << 30;

        /// <summary>
        /// 清理标志位.
        /// </summary>
        public const int CLEAR_FLAG_MASK = int.MaxValue >> 2;

        #endregion

        private static bool s_Initialized;
        private static TypeInfo[] s_TypeInfos;
        private static Dictionary<int, int> s_TypeHashToTypeId;
        private static int s_TypeCount;
        private static List<Type> s_Types;

        static TypeManager()
        {
            Init(true);
        }

        public static void Init(bool force)
        {
            if (!force && s_Initialized)
                return;

            s_TypeInfos = new TypeInfo[MAXIMUN_TYPE_COUNT];
            s_TypeHashToTypeId = new Dictionary<int, int>(MAXIMUN_TYPE_COUNT);
            s_Types = new List<Type>(MAXIMUN_TYPE_COUNT);

            var managedComponentType = typeof(IManagedComponent);
            foreach (var type in Util_TypeCache.AllTypes)
            {
                if (!type.IsValueType)
                    continue;

                if (type.ContainsGenericParameters)
                    continue;

                if (!typeof(IComponent).IsAssignableFrom(type))
                    continue;

                if (!UnsafeUtil.IsUnmanaged(type))
                    continue;

                var typeId = s_TypeCount;
                var typeHash = type.GetHashCode();
                var componentSize = UnsafeUtil.SizeOf(type);
                var alighInBytes = CalculateAlignmentInChunk(componentSize);
                var isZeroSize = componentSize == 0;
                var isManagedComponentType = managedComponentType.IsAssignableFrom(type);
                if (isZeroSize)
                    typeId |= ZERO_SIZE_FLAG;

                if (isManagedComponentType)
                    typeId |= MANAGED_COMPONENT_FLAG;

                var typeInfo = new TypeInfo(typeId, typeHash, componentSize, alighInBytes, isZeroSize, isManagedComponentType);
                s_Types.Add(type);
                s_TypeInfos[s_TypeCount] = typeInfo;
                s_TypeHashToTypeId[type.GetHashCode()] = typeId;
                s_TypeCount++;
            }

            s_Initialized = true;
        }

        private static int CalculateAlignmentInChunk(int sizeOfTypeInBytes)
        {
            var alignmentInBytes = MAXIMUN_SUPPORTED_ALIGNMENT;
            if (sizeOfTypeInBytes < alignmentInBytes && IsPowerOfTwo(sizeOfTypeInBytes))
                alignmentInBytes = sizeOfTypeInBytes;

            return alignmentInBytes;
        }

        public static int GetTypeCount()
        {
            return s_TypeCount;
        }

        public static int GetTypeId(Type type)
        {
            if (s_TypeHashToTypeId.TryGetValue(type.GetHashCode(), out var id))
                return id;
            return -1;
        }

        public static int GetTypeId<T>()
        {
            return TypeInfo<T>.Id;
        }

        public static Type GetType(int typeId)
        {
            return s_Types[typeId & CLEAR_FLAG_MASK];
        }

        public static TypeInfo GetTypeInfo(int typeId)
        {
            return s_TypeInfos[typeId & CLEAR_FLAG_MASK];
        }

        public static TypeInfo GetTypeInfo<T>()
        {
            return s_TypeInfos[TypeInfo<T>.Id & CLEAR_FLAG_MASK];
        }

        public static TypeInfo GetTypeInfo(Type type)
        {
            var typeIndex = GetTypeId(type);
            return s_TypeInfos[typeIndex & CLEAR_FLAG_MASK];
        }

        private static bool IsPowerOfTwo(int value)
        {
            return (value & (value - 1)) == 0;
        }
    }
}