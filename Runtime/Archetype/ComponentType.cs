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

namespace CZToolKit.ECS
{
    public struct ComponentType : IEquatable<ComponentType>
    {
        public int typeIndex;

        public bool IsZeroSized => (typeIndex & TypeManager.ZERO_SIZE_FLAG) != 0;
        public bool IsManagedComponent => (typeIndex & TypeManager.MANAGED_COMPONENT_FLAG) != 0;
        
        public ComponentType(Type type)
        {
            typeIndex = TypeManager.GetTypeIndex(type);
        }

        public TypeInfo GetTypeInfo()
        {
            return TypeManager.GetTypeInfo(typeIndex);
        }
        
        public Type GetManagedType()
        {
            return TypeManager.GetType(typeIndex);
        }
        
        public static ComponentType ReadWrite<T>()
        {
            return FromTypeIndex(TypeManager.GetTypeIndex<T>());
        }

        public static ComponentType ReadWrite(Type type)
        {
            return FromTypeIndex(TypeManager.GetTypeIndex(type));
        }

        public static ComponentType ReadWrite(int typeIndex)
        {
            return FromTypeIndex(typeIndex);
        }

        public static ComponentType FromTypeIndex(int typeIndex)
        {
            ComponentType type;
            type.typeIndex = typeIndex;
            return type;
        }

        public static implicit operator ComponentType(Type type)
        {
            return new ComponentType(type);
        }

        public static bool operator<(ComponentType lhs, ComponentType rhs)
        {
            return lhs.typeIndex < rhs.typeIndex;
        }

        public static bool operator>(ComponentType lhs, ComponentType rhs)
        {
            return rhs < lhs;
        }

        public static bool operator==(ComponentType lhs, ComponentType rhs)
        {
            return lhs.typeIndex == rhs.typeIndex;
        }

        public static bool operator!=(ComponentType lhs, ComponentType rhs)
        {
            return lhs.typeIndex != rhs.typeIndex;
        }

        public bool Equals(ComponentType other)
        {
            return typeIndex == other.typeIndex;
        }

        public override bool Equals(object obj)
        {
            return obj is ComponentType && (ComponentType)obj == this;
        }

        public override int GetHashCode()
        {
            return (typeIndex * 5813);
        }
    }
}