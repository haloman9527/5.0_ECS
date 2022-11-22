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

namespace CZToolKit.ECS.Examples
{
    public class ECSTest : MonoBehaviour
    {
        private World world;
        public int entityCount = 10000;

        void Awake()
        {
            world = World.NewWorld("MainWorld");
            world.CreateSystem<CustomSystem>();
            for (int i = 0; i < entityCount; i++)
            {
                world.NewEntity(out var entity);
                world.SetComponent(entity, new CustomComponent() { num = 10 });
            }
        }
    }
}