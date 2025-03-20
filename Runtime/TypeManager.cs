using System;
using System.Collections.Generic;
using Atom.UnsafeEx;

namespace Atom.ECS
{
    public static partial class TypeManager
    {
        #region Const

        private const int MAXIMUN_SUPPORTED_ALIGNMENT = 16;

        /// <summary>
        /// 最大组件数量
        /// </summary>
        private const int MAXIMUN_COMPONENTS = 1023;
        
        /// <summary>
        /// 组件Id位数
        /// </summary>
        private const int COMPONENT_LENGTH = 32;

        /// <summary>
        /// 下标位.
        /// </summary>
        public const uint ID_MASK = uint.MaxValue >> 2;
        
        /// <summary>
        /// 标志位.
        /// </summary>
        public const uint FLAG_MASK = ~ID_MASK;

        /// <summary>
        /// 32位表示是否size为0.
        /// </summary>
        public const uint ZERO_SIZE_FLAG = ((uint)1) << (COMPONENT_LENGTH - 1);

        /// <summary>
        /// 31位表示是否时托管类型组件.
        /// </summary>
        public const uint MANAGED_COMPONENT_FLAG = ((uint)1) << (COMPONENT_LENGTH - 2);

        #endregion

        private static bool s_Initialized;
        private static TypeInfo[] s_TypeInfos;
        private static Dictionary<Type, uint> s_ComponentTypeIdMap;
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

            s_TypeInfos = new TypeInfo[MAXIMUN_COMPONENTS];
            s_ComponentTypeIdMap = new Dictionary<Type, uint>(MAXIMUN_COMPONENTS);
            s_ManagedTypeMap = new Dictionary<Type, Type>();
            s_Types = new List<Type>(MAXIMUN_COMPONENTS);

            AddComponentType(typeof(Entity));
            for (int i = 0; i < TypesCache.AllTypes.Count; i++)
            {
                var type = (Type)null;
                var fixedType = GetFixedTypeByIndex(s_TypeCount);
                if (fixedType == null)
                {
                    type = TypesCache.AllTypes[i];
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
            var typeId = (uint)(s_TypeCount + 1);
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
            s_ComponentTypeIdMap[type] = (uint)typeId;
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

        public static uint GetTypeId(Type type)
        {
            if (s_ComponentTypeIdMap.TryGetValue(type, out var id))
                return id;
            return 0;
        }

        public static uint GetTypeId<T>()
        {
            if (s_ComponentTypeIdMap.TryGetValue(TypeCache<T>.TYPE, out var id))
            {
                return id;
            }

            return 0;
        }

        public static Type GetType(uint typeId)
        {
            return s_Types[GetTypeIndex(typeId)];
        }

        public static Type GetManagedType(uint typeId)
        {
            var t = s_Types[GetTypeIndex(typeId)];
            if (s_ManagedTypeMap.TryGetValue(t, out var type))
            {
                return type;
            }

            return null;
        }

        public static TypeInfo GetTypeInfo(uint typeId)
        {
            return s_TypeInfos[GetTypeIndex(typeId)];
        }

        public static TypeInfo GetTypeInfo<T>()
        {
            var typeId = GetTypeId<T>();
            return s_TypeInfos[GetTypeIndex(typeId)];
        }

        public static TypeInfo GetTypeInfo(Type type)
        {
            var typeId = GetTypeId(type);
            return s_TypeInfos[GetTypeIndex(typeId)];
        }

        public static int GetTypeIndex(uint typeId)
        {
            return ((int)(typeId & ID_MASK)) - 1;
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