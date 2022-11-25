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

using Unity.Burst;

namespace CZToolKit.ECS.Examples
{
    [BurstCompile]
    public struct CustomComponent : IComponent
    {
        public int num;
    }
}
