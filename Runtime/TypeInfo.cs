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
 *  Github: https://github.com/haloman9527
 *  Blog: https://www.haloman.net/
 *
 */

#endregion

using System;

namespace Atom.ECS
{
    public struct TypeInfo
    {
        public int index;
        public uint id;
        public int componentSize;
        public int alignInBytes;
        public bool isZeroSize;
        public bool isManagedComponentType;
        public int worldIdOffset;
        public int idOffset;

        public Type Type => TypeManager.GetType(id);

        public static implicit operator TypeInfo(Type d) => TypeManager.GetTypeInfo(d);
        public static explicit operator Type(TypeInfo b) => TypeManager.GetType(b.id);

        public static bool operator <(TypeInfo lhs, TypeInfo rhs) => lhs.index < rhs.index;

        public static bool operator >(TypeInfo lhs, TypeInfo rhs) => rhs < lhs;

        public static bool operator ==(TypeInfo lhs, TypeInfo rhs) => lhs.index == rhs.index;

        public static bool operator !=(TypeInfo lhs, TypeInfo rhs) => lhs.index != rhs.index;
        public bool Equals(TypeInfo other) => id == other.id;

        public override bool Equals(object obj) => obj is TypeInfo && (TypeInfo)obj == this;
    }

    public static class TypeInfo<TComponent> where TComponent : struct, IComponent
    {
        public static readonly TypeInfo CachedTypeInfo;
        public static readonly uint Id;
        public static readonly int Size;
        public static readonly bool IsZeroSize;
        public static readonly bool IsManagedType;

        static TypeInfo()
        {
            CachedTypeInfo = TypeManager.GetTypeInfo<TComponent>();
            Id = CachedTypeInfo.id;
            Size = CachedTypeInfo.componentSize;
            IsZeroSize = CachedTypeInfo.isZeroSize;
            IsManagedType = CachedTypeInfo.isManagedComponentType;
        }
    }
}