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
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class InputSystem : ISystem, IJobsUpdate, IUpdate
{
    public readonly World world;
    public readonly Filter filter;

    public InputSystem(World world)
    {
        this.world = world;
        this.filter = new Filter(world);
    }

    public JobHandle OnUpdate()
    {
        var entities = filter.Query<InputComponent>();
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        return new InputJob() { entities = entities, input = input }.Schedule(entities.Length, 64);
    }

    public void OnAfterUpdate() { }

    void IUpdate.OnUpdate()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        foreach (var entity in filter.Query<InputComponent>())
        {
            entity.RefComponent<InputComponent>().input = input;
        }
    }

    public struct InputJob : IJobParallelFor
    {
        public NativeArray<Entity> entities;
        public Vector2 input;

        public void Execute(int index)
        {
            var entity = entities[index];
            entity.RefComponent<InputComponent>().input = input;
        }
    }
}
