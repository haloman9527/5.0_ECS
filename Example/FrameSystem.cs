using CZToolKit.ECS;
using System.Collections.Generic;
using Unity.Collections;

public class FrameSystem : ISystem, IFixedUpdate
{
    public World world;
    public Filter filter;
    public InputInfoComponent input;

    public FrameSystem(World world)
    {
        this.world = world;
        this.filter = new Filter(world);
        input = new InputInfoComponent()
        {
            downButtons = 0,
            pressedButtons = 0,
            upButtons = 0,
            axis = new Dictionary<int, float>(),
            axisRaw = new Dictionary<int, float>()
        };
    }

    public void OnFixedUpdate()
    {
        var inputInfo = world.singleton.GetComponent<InputInfoComponent>();
        input.downButtons = inputInfo.downButtons;
        input.pressedButtons = inputInfo.pressedButtons;
        input.upButtons = inputInfo.upButtons;
        input.axis.Clear();
        input.axisRaw.Clear();
        foreach (var pair in inputInfo.axis)
        {
            input.axis[pair.Key] = pair.Value;
        }
        foreach (var pair in inputInfo.axisRaw)
        {
            input.axisRaw[pair.Key] = pair.Value;
        }
        foreach (var entity in filter.Query<Role>(Allocator.Temp))
        {
            entity.SetComponent(input);
        }
    }
}

/// <summary> 表示是一个受控制的角色 </summary>
public struct Role : IComponent { }
