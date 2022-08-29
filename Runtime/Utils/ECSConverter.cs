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
using UnityEngine;

namespace CZToolKit.ECS
{
    public static class ECSConverter
    {
        public static Entity ToEntity(GameObject target)
        {
            World.DefaultWorld.NewEntity(out var entity);
            World.DefaultWorld.SetComponent(entity, new GameObjectComponent() { id = ECSReferences.Set(target) });
            World.DefaultWorld.SetComponent(entity, new TransformComponent() { id = ECSReferences.Set(target.transform) });
            World.DefaultWorld.SetComponent(entity, new PositionComponent() { value = target.transform.position });
            World.DefaultWorld.SetComponent(entity, new RotationComponent() { value = target.transform.rotation });
            World.DefaultWorld.SetComponent(entity, new ScaleComponent() { value = target.transform.localScale });
            return entity;
        }
    }
}
