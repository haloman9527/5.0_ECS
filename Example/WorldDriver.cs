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
    public class WorldDriver : MonoBehaviour
    {
        void Update()
        {
            foreach (var world in World.AllWorlds)
            {
                world.Update();
            }
        }

        void FixedUpdate()
        {
            foreach (var world in World.AllWorlds)
            {
                world.FixedUpdate();
            }
        }

        void OnDestroy()
        {
            World.DisposeAllWorld();
        }
    }
}
