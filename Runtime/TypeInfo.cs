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
    public struct TypeInfo
    {
        public int typeIndex;
        public int typeHash;
        public int componentSize;
        public int alignInBytes;

        public TypeInfo(int typeIndex, int typeHash, int componentSize, int alignInBytes)
        {
            this.typeIndex = typeIndex;
            this.typeHash = typeHash;
            this.componentSize = componentSize;
            this.alignInBytes = alignInBytes;
        }

        public bool IsZeroSize
        {
            get { return (typeIndex & TypeManager.ZERO_SIZE_FLAG) != 0; }
        }

        public bool IsManagedComponentType
        {
            get { return (typeIndex & TypeManager.MANAGED_COMPONENT_FLAG) != 0; }
        }

        public Type TypeIndex
        {
            get { return TypeManager.GetType(typeIndex); }
        }
    }

    public class TypeInfo<TComponent>
    {
        public readonly static int Index;
        public readonly static int Size;
        public readonly static int HashCode;
        public readonly static bool IsManagedType;

        static TypeInfo()
        {
            var typeInfo = TypeManager.GetTypeInfo<TComponent>();
            
            Index = typeInfo.typeIndex;
            Size = typeInfo.componentSize;
            HashCode = typeInfo.typeHash;
            IsManagedType = typeInfo.IsManagedComponentType;
        }
    }
}