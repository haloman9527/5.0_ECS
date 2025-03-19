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

using System;
using System.Diagnostics;
using UnityEngine;

namespace Atom.ECS.Examples
{
    public class ECSTest : MonoBehaviour
    {
        private CustomWorld world;
        public int entityCount = 10000;

        private void Update()
        {
            world?.logicSystems.Execute();
        }

        private void OnDestroy()
        {
            world?.Dispose();
        }

        [Sirenix.OdinInspector.Button]
        public void A()
        {
            if (world != null)
            {
                this.world.Dispose();
            }
            world = new CustomWorld();
            world.logicSystems.Add(new CustomSystem(world));
            
            var ent = world.CreateEntity();
            var component = new CustomComponent() { num = 10 };
            var component2 = new CustomComponent2() ;
            world.SetComponent(ent, component);
            world.SetComponent(ent, component2, new B());

            var container = world.GetComponents<CustomComponent>();

            var sw = new Stopwatch();
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

    public class CustomWorld : World
    {
        public readonly Systems logicSystems = new Systems();
    }
}