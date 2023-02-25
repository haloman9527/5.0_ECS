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
    public static class SharedTypeIndex<TComponent>
    {
        public static int Data { get; }

        static SharedTypeIndex()
        {
            Data = TypeManager.FindTypeIndex(typeof(TComponent));
        }
    }
}