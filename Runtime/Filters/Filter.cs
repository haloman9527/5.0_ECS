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
using Unity.Collections;

namespace CZToolKit.ECS
{
    public partial class Filter
    {
        private readonly World world;

        public Filter()
        {
            
        }

        public Filter(World world)
        {
            this.world = world;
        }

        public bool GetEntities<ComponentType0>(Allocator allocator, out NativeArray<Entity> entities) where ComponentType0 : unmanaged, IComponent
        {
            var componentType0 = typeof(ComponentType0);
            if (!world.ExistsComponentContainer(componentType0))
            {
                entities = default;
                return false;
            }
            var componentPool0 = world.GetComponentContainer(componentType0);
            entities = componentPool0.GetEntities(allocator);
            return true;
        }
    }
}
