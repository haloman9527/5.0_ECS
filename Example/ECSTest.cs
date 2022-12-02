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
               ref var a = ref container.Ref<CustomComponent>(ent);
            }
            sw.Stop();
            UnityEngine.Debug.Log(sw.ElapsedMilliseconds);
            
            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
            for (int i = 0; i < 1000000; i++)
            {
               var a = container.Get<CustomComponent>(ent);
            }
            sw1.Stop();
            UnityEngine.Debug.Log(sw1.ElapsedMilliseconds);
            
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            for (int i = 0; i < 1000000; i++)
            {
               container.TryGet<CustomComponent>(ent, out var a);
            }
            sw2.Stop();
            UnityEngine.Debug.Log(sw2.ElapsedMilliseconds);
        }
    }
}