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
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CZToolKit.ECS
{
    public static class UnsafeUtil
    {
        public unsafe static int SizeOf<T>()
        {
            return Unsafe.SizeOf<T>();
        }

        private static Dictionary<Type, bool> CachedTypes = new Dictionary<Type, bool>();
        public static bool IsUnManaged(Type type)
        {
            var result = false;
            if (CachedTypes.ContainsKey(type))
                return CachedTypes[type];
            else if (type.IsPrimitive || type.IsPointer || type.IsEnum)
                result = true;
            else if (type.IsGenericType || !type.IsValueType)
                result = false;
            else
                result = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).All(x => IsUnManaged(x.FieldType));
            CachedTypes[type] = result;
            return result;
        }
    }
}
