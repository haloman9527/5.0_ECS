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
    public int entityCount = 100;
    World world;

    void Start()
    {
        world = World.NewWorld();
        Entity entity = default;
        for (int i = 0; i < entityCount; i++)
        {
            world.NewEntity(out entity);
            entity.SetComponent(new InputComponent() { input = Vector2.zero });
        }
        world.AddSystem(new InputSystem(world));
        var e = entity;
        Debug.Log(world.GetComponentPool<InputComponent>().Contains(e));
    }

    private void FixedUpdate()
    {
        world.FixedUpdate();
    }

    private void Update()
    {
        Profiler.BeginSample("NormalUpdateAAA");
        world.Update();
        Profiler.EndSample();

        Profiler.BeginSample("JobsUpdateAAA");
        world.JobsUpdate();
        Profiler.EndSample();
    }

    private void LateUpdate()
    {
        world.LateUpdate();
    }
}
