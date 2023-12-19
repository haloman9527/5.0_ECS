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
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion
using UnityEngine;

namespace CZToolKit.ECS.Examples
{
    public class CustomSystem : ComponentSystem, IUpdate
    {
        protected override void Update()
        {
            Filter.ForeachWithEntity((Entity e, ref CustomComponent c) =>
            {
                Debug.Log(c.num);
            });
            if (Input.GetButtonDown("Jump"))
            {
                Filter.ForeachWithEntity((Entity e, ref CustomComponent c) =>
                {
                    c.num += 1;
                });
            }
        }
    }
}
