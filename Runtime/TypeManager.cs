using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace CZToolKit.ECS
{
    public class TypeManager
    {
        #region Const
        
        private const int MAXIMUN_TYPE_COUNT = 1024 * 10;
        private const int MAXIMUN_SUPPORTED_ALIGNMENT = 16;
        
        #endregion

        private static bool s_Initialized;

        private static NativeArray<TypeInfo> s_TypeInfos;
        private static NativeHashMap<int, int> s_TypeHashToTypeIndex;
        private static int s_TypeCount;

        private static Dictionary<Type, int> s_ManagedTypeToIndex;
        private static Dictionary<int, Type> s_IndexToManagedType;

        static TypeManager()
        {
            Initialize();
        }
        
        private static void Initialize()
        {
            if (s_Initialized)
                return;

            s_TypeInfos = new NativeArray<TypeInfo>(MAXIMUN_TYPE_COUNT, Allocator.Persistent);
            s_TypeHashToTypeIndex = new NativeHashMap<int, int>(MAXIMUN_TYPE_COUNT, Allocator.Persistent);
            
            s_ManagedTypeToIndex = new Dictionary<Type, int>(1000);
            s_IndexToManagedType = new Dictionary<int, Type>(1000);

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
            foreach (var type in componentTypes)
            {
                var typeIndex = s_TypeCount;
                var typeHash = type.GetHashCode();
                var componentSize = UnsafeUtility.SizeOf(type);
                var alighInBytes = CalculateAlignmentInChunk(componentSize);
                TypeInfo typeInfo = new TypeInfo(typeIndex, typeHash, componentSize, alighInBytes);
                s_TypeInfos[typeIndex] = typeInfo;
                s_TypeHashToTypeIndex[type.GetHashCode()] = typeIndex;
                s_ManagedTypeToIndex[type] = typeIndex;
                s_IndexToManagedType[typeIndex] = type;
                s_TypeCount++;
            }
        }

        internal static int CalculateAlignmentInChunk(int sizeOfTypeInBytes)
        {
            int alignmentInBytes = MAXIMUN_SUPPORTED_ALIGNMENT;
            if (sizeOfTypeInBytes < alignmentInBytes && CollectionHelper.IsPowerOfTwo(sizeOfTypeInBytes))
                alignmentInBytes = sizeOfTypeInBytes;

            return alignmentInBytes;
        }

        public static int GetTypeCount()
        {
            return s_TypeCount;
        }

        public static int FindTypeIndex(Type type)
        {
            if (s_ManagedTypeToIndex.TryGetValue(type, out var index))
                return index;
            return -1;
        }

        public static Type FindType(int typeIndex)
        {
            if (s_IndexToManagedType.TryGetValue(typeIndex, out var type))
                return type;
            return null;
        }

        public static TypeInfo GetTypeInfo(int typeIndex)
        {
            return s_TypeInfos[typeIndex];
        }
        
        public static TypeInfo GetTypeInfo<T>()
        {
            var typeHash = SharedTypeHash<T>.Data;
            var typeIndex = s_TypeHashToTypeIndex[typeHash];
            return s_TypeInfos[typeIndex];
        }
        
        public static TypeInfo GetTypeInfo(Type componentType)
        {
            var typeIndex = FindTypeIndex(componentType);
            return s_TypeInfos[typeIndex];
        }
    }
}