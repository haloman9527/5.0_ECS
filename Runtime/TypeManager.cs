using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace CZToolKit.ECS
{
    public static class TypeManager
    {
        #region Const
        /// <summary>
        /// 最大组件数量
        /// </summary>
        private const int MAXIMUN_TYPE_COUNT = 1024 * 10;
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
        private static NativeArray<TypeInfo> s_TypeInfos;
        private static NativeHashMap<int, int> s_TypeHashToTypeIndex;
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

            s_TypeInfos = new NativeArray<TypeInfo>(MAXIMUN_TYPE_COUNT, Allocator.Persistent);
            s_TypeHashToTypeIndex = new NativeHashMap<int, int>(MAXIMUN_TYPE_COUNT, Allocator.Persistent);
            s_Types = new List<Type>(1024);

            InitializeAllComponentTypes();
            
            s_Initialized = true;
        }

        private static void InitializeAllComponentTypes()
        {
            var componentTypeSet = new HashSet<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var assemblyTypes = assembly.GetTypes();

                foreach (var type in assemblyTypes)
                {
                    if (type.IsAbstract)
                        continue;
                    
                    if (!type.IsValueType && !typeof(IComponent).IsAssignableFrom(type))
                        continue;
                    
                    if (type.ContainsGenericParameters)
                        continue;

                    componentTypeSet.Add(type);
                }
            }

            var componentTypes = new Type[componentTypeSet.Count];
            componentTypeSet.CopyTo(componentTypes);
            
            AddAllComponentTypes(componentTypes);
        }

        private static void AddAllComponentTypes(Type[] componentTypes)
        {
            var managedComponentType = typeof(IManagedComponent);
            foreach (var type in componentTypes)
            {
                var typeIndex = s_TypeCount;
                var typeHash = type.GetHashCode();
                var componentSize = UnsafeUtility.SizeOf(type);
                var alighInBytes = CalculateAlignmentInChunk(componentSize);

                if (componentSize == 0)
                    typeIndex |= ZERO_SIZE_FLAG;

                if (managedComponentType.IsAssignableFrom(type))
                    typeIndex |= MANAGED_COMPONENT_FLAG;
                
                var typeInfo = new TypeInfo(typeIndex, typeHash, componentSize, alighInBytes);
                s_TypeInfos[s_TypeCount] = typeInfo;
                s_TypeHashToTypeIndex[type.GetHashCode()] = typeIndex;
                s_Types.Add(type);
                s_TypeCount++;
            }
        }

        private static int CalculateAlignmentInChunk(int sizeOfTypeInBytes)
        {
            var alignmentInBytes = MAXIMUN_SUPPORTED_ALIGNMENT;
            if (sizeOfTypeInBytes < alignmentInBytes && CollectionHelper.IsPowerOfTwo(sizeOfTypeInBytes))
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

        public static Type GetType(int typeIndex)
        {
            return s_Types[typeIndex & CLEAR_FLAG_MASK];
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
    }
}