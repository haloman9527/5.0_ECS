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

public class TransformSystem : ISystem, ILateUpdate
{
    private World world;
    private Filter filter;

    public TransformSystem(World world)
    {
        this.world = world;
        this.filter = new Filter(world);
    }

    public void OnLateUpdate()
    {
        filter.Foreach((ref TransformComponent t, ref PositionComponent p) =>
        {
            var transform = ECSReferences.Get(t.id) as Transform;
            transform.position = p.value;
        });
        filter.Foreach((ref TransformComponent t, ref RotationComponent r) =>
        {
            var transform = ECSReferences.Get(t.id) as Transform;
            transform.rotation = r.value;
        });
        filter.Foreach((ref TransformComponent t, ref ScaleComponent s) =>
        {
            var transform = ECSReferences.Get(t.id) as Transform;
            transform.localScale = s.value;
        });
    }
}
