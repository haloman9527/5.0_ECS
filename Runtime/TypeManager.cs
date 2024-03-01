using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        private static Dictionary<int, int> s_TypeHashToTypeIndex;
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
            s_TypeHashToTypeIndex = new Dictionary<int, int>(MAXIMUN_TYPE_COUNT);
            s_Types = new List<Type>(MAXIMUN_TYPE_COUNT);

            var managedComponentType = typeof(IManagedComponent);
            foreach (var type in Util_TypeCache.AllTypes)
            {
                if (type.IsAbstract)
                    continue;

                if (!type.IsValueType && !typeof(IComponent).IsAssignableFrom(type))
                    continue;

                if (type.ContainsGenericParameters)
                    continue;
                var typeIndex = s_TypeCount;
                var typeHash = type.GetHashCode();
                var componentSize = UnsafeUtil.SizeOf(type);
                var alighInBytes = CalculateAlignmentInChunk(componentSize);
                var isZeroSize = componentSize == 0;
                var isManagedComponentType = managedComponentType.IsAssignableFrom(type);
                var typeId = typeIndex;
                if (isZeroSize)
                    typeId |= ZERO_SIZE_FLAG;

                if (isManagedComponentType)
                    typeId |= MANAGED_COMPONENT_FLAG;

                var typeInfo = new TypeInfo(typeIndex, typeId, typeHash, componentSize, alighInBytes, isZeroSize, isManagedComponentType);
                s_Types.Add(type);
                s_TypeInfos[s_TypeCount] = typeInfo;
                s_TypeHashToTypeIndex[type.GetHashCode()] = typeIndex;
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

        public static int GetTypeIndex(Type type)
        {
            if (s_TypeHashToTypeIndex.TryGetValue(type.GetHashCode(), out var index))
                return index;
            return -1;
        }

        public static int GetTypeIndex<T>()
        {
            return TypeInfo<T>.Index;
        }

        public static Type GetType(int typeId)
        {
            return s_Types[typeId & CLEAR_FLAG_MASK];
        }

        public static TypeInfo GetTypeInfo(int typeIndex)
        {
            return s_TypeInfos[typeIndex & CLEAR_FLAG_MASK];
        }

        public static TypeInfo GetTypeInfo<T>()
        {
            return s_TypeInfos[TypeInfo<T>.Index & CLEAR_FLAG_MASK];
        }

        public static TypeInfo GetTypeInfo(Type type)
        {
            var typeIndex = GetTypeIndex(type);
            return s_TypeInfos[typeIndex & CLEAR_FLAG_MASK];
        }

        private static bool IsPowerOfTwo(int value)
        {
            return (value & (value - 1)) == 0;
        }
    }
}