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
 *  Github: https://github.com/haloman9527
 *  Blog: https://www.haloman.net/
 *
 */
#endregion

using System.Collections.Generic;
using Unity.Collections;

namespace Jiange.ECS
{
    public partial struct Query
    {
        private readonly World world;

        public Query(World world)
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
