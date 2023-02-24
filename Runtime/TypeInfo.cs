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

namespace CZToolKit.ECS
{
    public struct TypeInfo
    {
        public int typeIndex;
        public int componentSize;
        public int alignInBytes;

        public TypeInfo(int typeIndex, int componentSize, int alignInBytes)
        {
            this.typeIndex = typeIndex;
            this.componentSize = componentSize;
            this.alignInBytes = alignInBytes;
        }
    }
}