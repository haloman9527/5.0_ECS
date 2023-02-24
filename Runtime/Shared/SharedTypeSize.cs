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

using Unity.Collections.LowLevel.Unsafe;

namespace CZToolKit.ECS
{
    public static class SharedTypeSize<TComponent>
    {
        public static int Data { get; }

        static SharedTypeSize()
        {
            Data = UnsafeUtility.SizeOf(typeof(TComponent));
        }
    }
}