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
    public class CustomSystem : ComponentSystem, IUpdate
    {
        public override void OnUpdate()
        {
            Filter.ForeachWithEntity((Entity e, ref CustomComponent c) =>
            {
                Debug.Log(c.num);
            });
            if (Input.GetButtonDown("Jump"))
            {
                Filter.Foreach((ref CustomComponent c) =>
                {
                    c.num += 1;
                });
            }
        }
    }
}
