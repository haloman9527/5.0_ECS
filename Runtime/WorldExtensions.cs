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
    public static partial class WorldExtensions
    {
        public static void FixedUpdate(this World self)
        {
            for (int i = 0; i < self.Systems.Count; i++)
            {
                var system = self.Systems[i];
                if (system is IFixedUpdate sys)
                    sys.OnFixedUpdate();
            }
        }

        public static void Update(this World self)
        {
            for (int i = 0; i < self.Systems.Count; i++)
            {
                var system = self.Systems[i];
                if (system is IUpdate sys)
                    sys.OnUpdate();
            }
        }

        public static void LateUpdate(this World self)
        {
            for (int i = 0; i < self.Systems.Count; i++)
            {
                var system = self.Systems[i];
                if (system is ILateUpdate sys)
                    sys.OnLateUpdate();
            }
        }

        public static void DestroySystem(this World self, ISystem system)
        {
            if (self.RemoveSystem(system) && system is IDestroy sys)
                sys.OnDestroy();
        }
    }
}