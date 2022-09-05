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
using UnityEngine;

public class TransformSystem : ComponentSystem, ILateUpdate
{
    public override void OnUpdate()
    {
        Filter.Foreach((ref TransformComponent t, ref PositionComponent p) =>
        {
            var transform = ECSReferences.Get(t.id) as Transform;
            transform.position = p.value;
        });
        Filter.Foreach((ref TransformComponent t, ref RotationComponent r) =>
        {
            var transform = ECSReferences.Get(t.id) as Transform;
            transform.rotation = r.value;
        });
        Filter.Foreach((ref TransformComponent t, ref ScaleComponent s) =>
        {
            var transform = ECSReferences.Get(t.id) as Transform;
            transform.localScale = s.value;
        });
    }
}
