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
    public interface ISystem { }

    public interface IOnAwake
    {
        void OnAwake();
    }

    public interface IFixedUpdate
    {
        void OnFixedUpdate();
    }

    public interface IUpdate
    {
        void OnUpdate();
    }

    public interface ILateUpdate
    {
        void OnLateUpdate();
    }

    public interface IDestroy
    {
        void OnDestroy();
    }
}
