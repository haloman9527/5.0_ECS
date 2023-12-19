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
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion

using System.Runtime.InteropServices;
using Unity.Burst;

namespace CZToolKit.ECS.Examples
{
    public struct CustomComponent : IComponent
    {
        public int num;
    }
}
