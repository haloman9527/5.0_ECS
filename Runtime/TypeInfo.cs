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
        public readonly int hash;
        public readonly int componentSize;
        public readonly int alignInBytes;
        public readonly bool isZeroSize;
        public readonly bool isManagedComponentType;

        public TypeInfo(int typeIndex, int typeId, int typeHash, int componentSize, int alignInBytes, bool isZeroSize, bool isManagedComponentType)
        {
            this.index = typeIndex;
            this.id = typeId;
            this.hash = typeHash;
            this.componentSize = componentSize;
            this.alignInBytes = alignInBytes;
            this.isZeroSize = isZeroSize;
            this.isManagedComponentType = isManagedComponentType;
        }

        public Type Type
        {
            get { return TypeManager.GetType(id); }
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

            Index = typeInfo.index;
            Size = typeInfo.componentSize;
            HashCode = typeInfo.hash;
            IsManagedType = typeInfo.isManagedComponentType;
        }
    }
}