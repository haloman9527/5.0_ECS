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

namespace CZToolKit.ECS
{
    public struct TypeInfo
    {
        public readonly int index;
        public readonly int id;
        public readonly int componentSize;
        public readonly int alignInBytes;
        public readonly bool isZeroSize;
        public readonly bool isManagedComponentType;
        public readonly int worldIdOffset;
        public readonly int idOffset;

        public TypeInfo(int index, int id, int componentSize, int alignInBytes, bool isZeroSize, bool isManagedComponentType, int worldIdOffset, int idOffset)
        {
            this.index = index;
            this.id = id;
            this.componentSize = componentSize;
            this.alignInBytes = alignInBytes;
            this.isZeroSize = isZeroSize;
            this.isManagedComponentType = isManagedComponentType;
            this.worldIdOffset = worldIdOffset;
            this.idOffset = idOffset;
        }

        public Type Type
        {
            get { return TypeManager.GetType(id); }
        }
        
        public static implicit operator TypeInfo(Type d) => TypeManager.GetTypeInfo(d);
        public static explicit operator Type(TypeInfo b) => TypeManager.GetType(b.id);
        
        public static bool operator<(TypeInfo lhs, TypeInfo rhs)
        {
            return lhs.index < rhs.index;
        }

        public static bool operator>(TypeInfo lhs, TypeInfo rhs)
        {
            return rhs < lhs;
        }

        public static bool operator==(TypeInfo lhs, TypeInfo rhs)
        {
            return lhs.index == rhs.index;
        }

        public static bool operator!=(TypeInfo lhs, TypeInfo rhs)
        {
            return lhs.index != rhs.index;
        }

        public bool Equals(TypeInfo other)
        {
            return id == other.id;
        }

        public override bool Equals(object obj)
        {
            return obj is TypeInfo && (TypeInfo)obj == this;
        }

        public override int GetHashCode()
        {
            return id;
        }
    }

    public class TypeInfo<TComponent>
    {
        public readonly static int Id;
        public readonly static int Size;
        public readonly static bool IsZeroSize;
        public readonly static bool IsManagedType;

        static TypeInfo()
        {
            var typeInfo = TypeManager.GetTypeInfo<TComponent>();

            Id = typeInfo.id;
            Size = typeInfo.componentSize;
            IsZeroSize = typeInfo.isZeroSize;
            IsManagedType = typeInfo.isManagedComponentType;
        }
    }
}