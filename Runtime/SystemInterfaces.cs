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

namespace CZToolKit.ECS
{
    public interface IFixedUpdate
    {
        void OnFixedUpdate();
    }

    public interface IUpdate
    {
        void OnUpdate();
    }

    public interface ILateUpdate
    {
        void OnLateUpdate();
    }

    public static partial class WorldExtension
    {
        public static void FixedUpdate(this World world)
        {
            for (int i = 0; i < world.Systems.Count; i++)
            {
                var system = world.Systems[i];
                if (system is IFixedUpdate sys)
                    sys.OnFixedUpdate();
            }
        }

        public static void Update(this World world)
        {
            for (int i = 0; i < world.Systems.Count; i++)
            {
                var system = world.Systems[i];
                if (system is IUpdate sys)
                    sys.OnUpdate();
            }
        }

        public static void LateUpdate(this World world)
        {
            for (int i = 0; i < world.Systems.Count; i++)
            {
                var system = world.Systems[i];
                if (system is ILateUpdate sys)
                    sys.OnLateUpdate();
            }
            world.CheckDestroyEntities();
        }
    }
}
