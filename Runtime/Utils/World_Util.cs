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
    public partial class World
    {
        /// <summary> 创建一个标准World </summary>
        public static World NewBasicWorld(string worldName)
        {
            World world = new World(worldName);
            world.internalAfterSystems.Add(new TransformSystem(world));
            return world;
        }
    }
}
