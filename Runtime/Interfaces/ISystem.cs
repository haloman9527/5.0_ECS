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
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion

namespace CZToolKit.ECS
{
    public interface ISystem
    {
        bool Active { get; set; }
        
        World World { get; set; }

        Filter Filter { get; set; }
        
        void OnCreate();

        void OnUpdate();

        void OnDestroy();
    }
}
