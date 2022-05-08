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
using CZToolKit.ECS;
using System.Collections.Generic;
using UnityEngine;

public struct TransformComponent : IComponent
{
    public Transform transform;
}

public struct RotateSpeedComponent : IComponent
{
    public Vector3 rotateSpeed;
}

public struct EulerAngleComponent : IComponent
{
    public Vector3 eulerAngle;
}
