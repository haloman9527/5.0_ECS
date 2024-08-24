using System;
using System.Collections.Generic;
using CZToolKit.UnsafeEx;

namespace CZToolKit.ECS
{
    public static partial class TypeManager
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
        /// 标志位.
        /// </summary>
        public const int FLAG_MASK = int.MaxValue << (32 - 2);

        /// <summary>
        /// 下标位.
        /// </summary>
        public const int INDEX_MASK = ~FLAG_MASK;

        #endregion

        private static bool s_Initialized;
        private static TypeInfo[] s_TypeInfos;
        private static Dictionary<Type, int> s_ComponentTypeIdMap;
        private static Dictionary<Type, Type> s_ManagedTypeMap;
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
            s_ComponentTypeIdMap = new Dictionary<Type, int>(MAXIMUN_TYPE_COUNT);
            s_ManagedTypeMap = new Dictionary<Type, Type>();
            s_Types = new List<Type>(MAXIMUN_TYPE_COUNT);

            AddComponentType(typeof(Entity));
            for (int i = 0; i < Util_TypeCache.AllTypes.Count; i++)
            {
                var type = (Type)null;
                var fixedType = GetFixedTypeByIndex(s_TypeCount);
                if (fixedType == null)
                {
                    type = Util_TypeCache.AllTypes[i];
                    var fixedIndex = GetFixedIndexByType(type);
                    if (fixedIndex >= 0)
                        continue;
                }
                else
                {
                    type = fixedType;
                    i--;
                }

                if (!type.IsValueType)
                    continue;

                if (type.ContainsGenericParameters)
                    continue;

                if (!typeof(IComponent).IsAssignableFrom(type))
                    continue;

                if (!UnsafeUtil.IsUnmanaged(type))
                {
                    Log.Error($"{type.FullName}不属于非托管类型");
                    continue;
                }

                if (s_ComponentTypeIdMap.ContainsKey(type))
                    continue;

                AddComponentType(type);
            }

            s_Initialized = true;
        }

        private static void AddComponentType(Type type)
        {
            s_Types.Add(type);

            var typeIndex = s_TypeCount;
            var typeId = s_TypeCount;
            var componentSize = UnsafeUtil.SizeOf(type);
            var alignInBytes = CalculateAlignmentInChunk(componentSize);
            var isZeroSize = componentSize == 0;
            var isManagedComponentType = typeof(IManagedComponent).IsAssignableFrom(type);
            var worldIdOffset = 0;
            var idOffset = 0;

            if (isManagedComponentType)
            {
                var worldIdField = type.GetField(nameof(ManagedComponentBridge.worldId));

                if (worldIdField == null)
                {
                    Log.Error($"{type.FullName}需要包含int worldId字段");
                    return;
                }

                var idField = type.GetField(nameof(ManagedComponentBridge.entityId));
                if (idField == null)
                {
                    Log.Error($"{type.FullName}需要包含uint {nameof(ManagedComponentBridge.entityId)}字段");
                    return;
                }

                var interfaces = type.GetInterfaces();
                for (int j = 0; j < interfaces.Length; j++)
                {
                    var interfaceType = interfaces[j];

                    if (interfaceType == typeof(IManagedComponent))
                    {
                        continue;
                    }

                    if (!typeof(IManagedComponent).IsAssignableFrom(interfaceType))
                    {
                        continue;
                    }

                    s_ManagedTypeMap[type] = interfaceType.GetGenericArguments()[0];

                    break;
                }

                worldIdOffset = UnsafeUtil.GetFieldOffset(worldIdField);
                idOffset = UnsafeUtil.GetFieldOffset(idField);
            }

            if (isZeroSize)
                typeId |= ZERO_SIZE_FLAG;

            if (isManagedComponentType)
                typeId |= MANAGED_COMPONENT_FLAG;

            var typeInfo = new TypeInfo()
            {
                index = typeIndex,
                id = typeId,
                componentSize = componentSize,
                alignInBytes = alignInBytes,
                isZeroSize = isZeroSize,
                isManagedComponentType = isManagedComponentType,
                worldIdOffset = worldIdOffset,
                idOffset = idOffset,
            };


            s_TypeInfos[s_TypeCount] = typeInfo;
            s_ComponentTypeIdMap[type] = typeId;
            s_TypeCount++;
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
            if (s_ComponentTypeIdMap.TryGetValue(type, out var id))
                return id;
            return -1;
        }

        public static int GetTypeId<T>()
        {
            if (s_ComponentTypeIdMap.TryGetValue(typeof(T), out var id))
            {
                return id;
            }

            return -1;
        }

        public static Type GetType(int typeId)
        {
            return s_Types[typeId & INDEX_MASK];
        }

        public static Type GetManagedType(int typeId)
        {
            var t = s_Types[typeId & INDEX_MASK];
            if (s_ManagedTypeMap.TryGetValue(t, out var type))
            {
                return type;
            }

            return null;
        }

        public static TypeInfo GetTypeInfo(int typeId)
        {
            return s_TypeInfos[typeId & INDEX_MASK];
        }

        public static TypeInfo GetTypeInfo<T>()
        {
            var typeId = GetTypeId<T>();
            return s_TypeInfos[typeId & INDEX_MASK];
        }

        public static TypeInfo GetTypeInfo(Type type)
        {
            var typeId = GetTypeId(type);
            return s_TypeInfos[typeId & INDEX_MASK];
        }

        private static bool IsPowerOfTwo(int value)
        {
            return (value & (value - 1)) == 0;
        }
    }

    public static partial class TypeManager
    {
        public static Type GetFixedTypeByIndex(int index)
        {
            return null;
        }

        public static int GetFixedIndexByType(Type type)
        {
            return -1;
        }
    }
}