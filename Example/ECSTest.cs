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

using System.Diagnostics;
using UnityEngine;

namespace CZToolKit.ECS.Examples
{
    public class ECSTest : MonoBehaviour
    {
        private Entity ent;
        public int entityCount = 10000;

        void Awake()
        {
            // World.DefaultWorld.CreateSystem<CustomSystem>();
            // for (int i = 0; i < entityCount; i++)
            // {
            //     World.DefaultWorld.NewEntity(out var entity);
            //     World.DefaultWorld.SetComponent(entity, new CustomComponent() { num = 10 });
            // }
        }

        [Sirenix.OdinInspector.Button]
        public void A()
        {
            if (ent == default)
            {
                ent = World.DefaultWorld.NewEntity();
                World.DefaultWorld.SetComponent(ent, new CustomComponent() { num = 10 });
            }

            var container = World.DefaultWorld.GetComponentContainer<CustomComponent>();
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000000; i++)
            {
                container.TryGet<CustomComponent>(ent, out var v);
            }
            sw.Stop();
            UnityEngine.Debug.Log(sw.ElapsedMilliseconds);
        }
    }
}