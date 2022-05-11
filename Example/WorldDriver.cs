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
using CZToolKit.ECS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class WorldDriver : MonoBehaviour
{
    World world;
    public bool isAsync;

    void Start()
    {
        world = new World("Default World");
        world.singleton.SetComponent(new InputInfoComponent()
        {
            axis = new Dictionary<int, float>(),
            axisRaw = new Dictionary<int, float>()
        });
        for (int i = 0; i < transform.childCount; i++)
        {
            world.NewEntity(out var entity);
            entity.SetComponent(new TransformComponent() { transform = transform.GetChild(i) });
            entity.SetComponent(new RotateSpeedComponent() { rotateSpeed = Vector3.up * 50 });
            entity.SetComponent(new EulerAngleComponent() { eulerAngle = Vector3.zero });
            entity.SetComponent(new Role());
        }
        // ----- LogicSystems -----
        world.AddSystem(new FrameSystem(world));
        world.AddSystem(new ExampleLogicSystem(world));

        // ----- RenderSystems -----
        world.AddSystem(new ExampleRenderSystem(world));
        world.AddSystem(new InputCollectionSystem(world));
    }

    private void FixedUpdate()
    {
        ExampleLogicSystem.isAsync = isAsync;
        foreach (var world in World.Worlds.Values)
        {
            Profiler.BeginSample("ECS LogicUpdate");
            world.FixedUpdate();
            Profiler.EndSample();
        }
    }

    private void Update()
    {
        foreach (var world in World.Worlds.Values)
        {
            Profiler.BeginSample("ECS RenderUpdate");
            world.Update();
            Profiler.EndSample();
        }
    }

    private void LateUpdate()
    {
        foreach (var world in World.Worlds.Values)
        {
            world.LateUpdate();
        }
    }

    private void OnDestroy()
    {
        foreach (var world in World.Worlds.Values)
        {
            world.Dispose();
        }
    }
}
