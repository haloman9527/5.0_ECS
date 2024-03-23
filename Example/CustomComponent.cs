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

using System.Runtime.InteropServices;
using Unity.Burst;

namespace CZToolKit.ECS.Examples
{
    public struct CustomComponent : IComponent
    {
        public int num;
    }
    
    public struct CustomComponent2 : IManagedComponent<B>
    {
        public int WorldId { get; set; }
        public uint Id { get; set; }
    }

    public class B
    {
        public int num;
    }
}
