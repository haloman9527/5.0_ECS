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

        void Awake()
        {
            world = new World("MainWorld");

            world.AddSystem(new CustomSystem(world));

            world.NewEntity(out Entity entity);
            //world.SetComponent(entity, new CustomComponent() { num = 10 });
        }
    }
}