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
using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class ExampleLogicSystem : ISystem, IFixedUpdate
{
    public static bool isAsync;

    public readonly World world;
    public readonly Filter filter;
    Type[] componentTypes = new Type[] { typeof(EulerAngleComponent), typeof(RotateSpeedComponent) };
    NativeList<Entity> entities;

    public ExampleLogicSystem(World world)
    {
        this.world = world;
        this.filter = new Filter(world);
    }

    ~ExampleLogicSystem()
    {
        if (entities.IsCreated)
            entities.Dispose();
    }

    public void OnFixedUpdate()
    {
        if (isAsync)
        {
            entities = filter.Query(Allocator.TempJob, componentTypes);
            var job = new RotateJob() { entities = entities, deltaTime = Time.fixedDeltaTime };
            var handle = job.Schedule(entities.Length, 32);
            handle.Complete();
            entities.Dispose();
        }
        else
        {
            var deltaTime = Time.fixedDeltaTime;
            entities = filter.Query(Allocator.TempJob, componentTypes);
            foreach (var entity in entities)
            {
                entity.RefComponent<EulerAngleComponent>().eulerAngle += entity.RefComponent<RotateSpeedComponent>().rotateSpeed * deltaTime;
            }
            entities.Dispose();
        }
    }

    public struct RotateJob : IJobParallelFor
    {
        [ReadOnly] public NativeList<Entity> entities;
        public float deltaTime;

        public void Execute(int index)
        {
            var entity = entities[index];
            Execute(entity, ref entity.RefComponent<RotateSpeedComponent>(), ref entity.RefComponent<EulerAngleComponent>());
        }

        public void Execute(in Entity entity, ref RotateSpeedComponent speed, ref EulerAngleComponent eulerAngle)
        {
            var inputInfo = entity.GetComponent<InputInfoComponent>();
            speed.rotateSpeed += Vector3.up * inputInfo.GetAxis(AxisDefine.Vertical) * 10;
            eulerAngle.eulerAngle += speed.rotateSpeed * deltaTime;
        }
    }
}

public class ExampleRenderSystem : ISystem, IUpdate
{
    public readonly World world;
    public readonly Filter filter;
    Type[] componentTypes = new Type[] { typeof(TransformComponent), typeof(RotateSpeedComponent) };

    public ExampleRenderSystem(World world)
    {
        this.world = world;
        this.filter = new Filter(world);
    }

    public void OnUpdate()
    {
        var entities = filter.Query(Allocator.Temp, componentTypes);
        foreach (var entity in entities)
        {
            var transform = entity.GetComponent<TransformComponent>();
            var euler = entity.GetComponent<EulerAngleComponent>();
            transform.transform.eulerAngles = euler.eulerAngle;
        }
        entities.Dispose();
    }
}
