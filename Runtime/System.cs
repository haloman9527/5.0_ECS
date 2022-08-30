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
    public interface ISystemAwake
    {
        void OnAwake();
    }

    public interface ISystem
    {
        void OnUpdate();
    }

    public interface ISystemDestroy
    {
        void OnDestroy();
    }
}
