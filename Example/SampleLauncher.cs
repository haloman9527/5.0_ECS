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
using UnityEngine;
using CZToolKit.ECS;

public class SampleLauncher : MonoBehaviour
{
    private World world;
    private float startTime;
    private int fixedUpdateCount;

    public World World
    {
        get { return world; }
    }
    public float Time
    {
        get { return UnityEngine.Time.time - startTime; }
    }
    public float FixedTime
    {
        get { return fixedUpdateCount * UnityEngine.Time.fixedDeltaTime; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        world = WorldManager.MainWorld;
        startTime = UnityEngine.Time.time;
        fixedUpdateCount = 0;

        world.AddSystem(new SampleSystem(world));
    }

    private void Update()
    {
        for (; Time / UnityEngine.Time.fixedDeltaTime >= fixedUpdateCount;)
        {
            world.FixedUpdate();
            fixedUpdateCount++;
        }
        world.Update();
        world.LateUpdate();
    }
}

public struct RotateSpeedComponent : IComponent
{
    public float value;
}

public class SampleSystem : IUpdateSystem
{
    private World world;
    private Filter filter;
    private SampleJob job;

    public SampleSystem(World world)
    {
        this.world = world;
        this.filter = new Filter(world);
        job = new SampleJob();
    }

    public void OnUpdate()
    {
        filter.RefForeach<SampleJob, GameObjectComponent, RotateSpeedComponent>(job);
    }

    public struct SampleJob : IRefForeach<GameObjectComponent, RotateSpeedComponent>
    {
        public void Execute(ref GameObjectComponent arg0, ref RotateSpeedComponent arg1)
        {
            arg0.gameObject.transform.eulerAngles += Vector3.up * arg1.value * Time.deltaTime;
        }
    }
}