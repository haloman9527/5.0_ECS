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
    public class CustomSystem : ISystem, IUpdate
    {
        private World world;
        private Filter filter;

        public CustomSystem(World world)
        {
            this.world = world;
            this.filter = new Filter(world);
        }

        public void OnUpdate()
        {
            filter.ForeachWithEntity((Entity e, ref CustomComponent c) =>
            {
                Debug.Log(c.num);
            });
            if (Input.GetButtonDown("Jump"))
            {
                filter.Foreach((ref CustomComponent c) =>
                {
                    c.num += 1;
                });
            }
        }
    }
}
