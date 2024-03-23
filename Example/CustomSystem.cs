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
 *  Github: https://github.com/haloman9527
 *  Blog: https://www.haloman.net/
 *
 */

#endregion

using UnityEngine;

namespace CZToolKit.ECS.Examples
{
    public class CustomSystem : ComponentSystem
    {
        private World world;
        private Filter filter;

        public CustomSystem(World world)
        {
            this.world = world;
            this.filter = new Filter(world);
        }

        public override void Execute()
        {
            filter.ForeachWithEntity((Entity e, ref CustomComponent c) => { Debug.Log(c.num); });
            
            filter.ForeachWithEntity((Entity e, ref CustomComponent2 c) =>
            {
                Debug.Log(c.GetValue()?.num);
            });
            
            if (Input.GetButtonDown("Jump"))
            {
                filter.ForeachWithEntity((Entity e, ref CustomComponent c) => { c.num += 1; });
                filter.ForeachWithEntity((Entity e, ref CustomComponent2 c) => { c.GetValue().num += 1; });
            }
        }
    }
}