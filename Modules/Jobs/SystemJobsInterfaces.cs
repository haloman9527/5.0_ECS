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
using System;
using Unity.Jobs;

namespace CZToolKit.ECS
{
    public interface IJobsFixedUpdate
    {
        JobHandle OnFixedUpdate();
        void OnAfterFixedUpdate();
    }

    public interface IJobsUpdate
    {
        JobHandle OnUpdate();
        void OnAfterUpdate();
    }

    public interface IJobsLateUpdate
    {
        JobHandle OnLateUpdate();
        void OnAfterLateUpdate();
    }

    public static partial class WorldExtension
    {
        public static void JobsFixedUpdate(this World world)
        {
            for (int i = 0; i < world.Systems.Count; i++)
            {
                var system = world.Systems[i];
                switch (system)
                {
                    case IJobsFixedUpdate jobSys:
                        {
                            jobSys.OnFixedUpdate().Complete();
                            jobSys.OnAfterFixedUpdate();
                        }
                        break;
                    case IFixedUpdate sys:
                        {
                            sys.OnFixedUpdate();
                        }
                        break;
                }
            }
        }

        public static void JobsUpdate(this World world)
        {
            for (int i = 0; i < world.Systems.Count; i++)
            {
                var system = world.Systems[i];
                switch (system)
                {
                    case IJobsUpdate jobSys:
                        {
                            jobSys.OnUpdate().Complete();
                            jobSys.OnAfterUpdate();
                        }
                        break;
                    case IUpdate sys:
                        {
                            sys.OnUpdate();
                        }
                        break;
                }
            }
        }

        public static void JobsLateUpdate(this World world)
        {
            for (int i = 0; i < world.Systems.Count; i++)
            {
                var system = world.Systems[i];
                switch (system)
                {
                    case IJobsLateUpdate jobSys:
                        {
                            jobSys.OnLateUpdate().Complete();
                            jobSys.OnAfterLateUpdate();
                        }
                        break;
                    case ILateUpdate sys:
                        {
                            sys.OnLateUpdate();
                        }
                        break;
                }
            }
            world.CheckDestroyEntities();
        }
    }
}
