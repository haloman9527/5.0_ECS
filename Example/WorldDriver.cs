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

using System;
using UnityEngine;

namespace CZToolKit.ECS.Examples
{
    public class WorldDriver : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            for (int i = 0; i < World.AllWorlds.Count; i++)
            {
                World.AllWorlds[i].Update();
            }
        }

        void FixedUpdate()
        {
            for (int i = 0; i < World.AllWorlds.Count; i++)
            {
                World.AllWorlds[i].FixedUpdate();
            }
        }

        void LateUpdate()
        {
            for (int i = 0; i < World.AllWorlds.Count; i++)
            {
                World.AllWorlds[i].LateUpdate();
            }
        }

        void OnDestroy()
        {
            World.DisposeAllWorld();
        }
    }
}
