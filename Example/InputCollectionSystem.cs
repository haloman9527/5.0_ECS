using CZToolKit.ECS;
using System.Collections.Generic;
using UnityEngine;

public class InputCollectionSystem : ISystem, IUpdate
{
    private readonly World world;

    public InputCollectionSystem(World world)
    {
        this.world = world;
    }

    public void OnUpdate()
    {
        var inputInfo = world.global.GetComponent<InputInfoComponent>();

        inputInfo.SetDownButton(ButtonDefine.Fire1, Input.GetButtonDown("Fire1"));
        inputInfo.SetDownButton(ButtonDefine.Fire2, Input.GetButtonDown("Fire2"));
        inputInfo.SetDownButton(ButtonDefine.Fire3, Input.GetButtonDown("Fire3"));
        inputInfo.SetDownButton(ButtonDefine.Jump, Input.GetButtonDown("Jump"));

        inputInfo.SetButton(ButtonDefine.Fire1, Input.GetButton("Fire1"));
        inputInfo.SetButton(ButtonDefine.Fire2, Input.GetButton("Fire2"));
        inputInfo.SetButton(ButtonDefine.Fire3, Input.GetButton("Fire3"));
        inputInfo.SetButton(ButtonDefine.Jump, Input.GetButton("Jump"));

        inputInfo.SetUpButton(ButtonDefine.Fire1, Input.GetButtonUp("Fire1"));
        inputInfo.SetUpButton(ButtonDefine.Fire2, Input.GetButtonUp("Fire2"));
        inputInfo.SetUpButton(ButtonDefine.Fire3, Input.GetButtonUp("Fire3"));
        inputInfo.SetUpButton(ButtonDefine.Jump, Input.GetButtonUp("Jump"));

        inputInfo.SetAxis(AxisDefine.Horizontal, Input.GetAxis("Horizontal"));
        inputInfo.SetAxis(AxisDefine.Vertical, Input.GetAxis("Vertical"));

        inputInfo.SetAxisRaw(AxisDefine.Horizontal, Input.GetAxisRaw("Horizontal"));
        inputInfo.SetAxisRaw(AxisDefine.Vertical, Input.GetAxisRaw("Vertical"));

        world.global.SetComponent(inputInfo);
        // 发出去，然后清理掉，现在还没有转发的功能，所以不清理
        //inputInfo.Clear();
    }
}

public static class InputExtension
{
    public static void SetDownButton(ref this InputInfoComponent inputInfo, int buttonID, bool value)
    {
        if (value)
            inputInfo.downButtons |= 1 << buttonID;
        else
            inputInfo.downButtons &= ~(1 << buttonID);
    }

    public static bool GetButtonDown(ref this InputInfoComponent inputInfo, int buttonID)
    {
        return (1 & (inputInfo.downButtons >> buttonID)) == 1;
    }

    public static void SetButton(ref this InputInfoComponent inputInfo, int buttonID, bool value)
    {
        if (value)
            inputInfo.pressedButtons |= 1 << buttonID;
        else
            inputInfo.pressedButtons &= ~(1 << buttonID);
    }

    public static bool GetButton(ref this InputInfoComponent inputInfo, int buttonID)
    {
        return (1 & (inputInfo.pressedButtons >> buttonID)) == 1;
    }

    public static void SetUpButton(ref this InputInfoComponent inputInfo, int buttonID, bool value)
    {
        if (value)
            inputInfo.upButtons |= 1 << buttonID;
        else
            inputInfo.upButtons &= ~(1 << buttonID);
    }

    public static bool GetButtonUp(ref this InputInfoComponent inputInfo, int buttonID)
    {
        return (1 & (inputInfo.upButtons >> buttonID)) == 1;
    }

    public static void SetAxis(ref this InputInfoComponent inputInfo, int axisID, float value)
    {
        inputInfo.axis[axisID] = value;
    }

    public static float GetAxis(ref this InputInfoComponent inputInfo, int axisID)
    {
        inputInfo.axis.TryGetValue(axisID, out var value);
        return value;
    }

    public static void SetAxisRaw(ref this InputInfoComponent inputInfo, int axisID, float value)
    {
        inputInfo.axisRaw[axisID] = value;
    }

    public static float GetAxisRaw(ref this InputInfoComponent inputInfo, int axisID)
    {
        inputInfo.axisRaw.TryGetValue(axisID, out var value);
        return value;
    }

    public static void Clear(ref this InputInfoComponent inputInfo)
    {
        inputInfo.downButtons = 0;
        inputInfo.pressedButtons = 0;
        inputInfo.upButtons = 0;
        inputInfo.axis.Clear();
        inputInfo.axisRaw.Clear();
    }
}

public struct InputInfoComponent : IComponent
{
    public int downButtons;
    public int pressedButtons;
    public int upButtons;
    public Dictionary<int, float> axis;
    public Dictionary<int, float> axisRaw;
}

public class ButtonDefine
{
    public static int Fire1 = 1;
    public static int Fire2 = 2;
    public static int Fire3 = 3;
    public static int Jump = 4;
}

public class AxisDefine
{
    public static int Horizontal = 1;
    public static int Vertical = 2;
}