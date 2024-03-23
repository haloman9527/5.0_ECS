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

using Unity.Collections;

namespace CZToolKit.ECS
{
    public partial class World
    {
        private IndexGenerator entityIndexGenerator = new IndexGenerator();
        private NativeParallelHashMap<int, Entity> entities = new NativeParallelHashMap<int, Entity>(256, Allocator.Persistent);

        public NativeParallelHashMap<int, Entity> Entities
        {
            get { return entities; }
        }

        public Entity CreateEntity()
        {
            var index = entityIndexGenerator.Next();
            var entity = new Entity(this.id, index);
            entities.Add(index, entity);
            return entity;
        }

        public bool Exists(Entity entity)
        {
            return entities.ContainsKey(entity.index);
        }

        public void DestroyEntity(Entity entity)
        {
            entities.Remove(entity.index);
            foreach (var components in componentContainers.GetValueArray(Allocator.Temp))
            {
                if (components.typeInfo.isManagedComponentType)
                {
                    var managedComponent = components.Get(entity) as IManagedComponent;
                    if (managedComponent != null)
                    {
                        references.Release(managedComponent.Id);
                    }
                }

                components.Del(entity);
            }
        }

        private void DisposeAllEntities()
        {
            entities.Clear();
            entities.Dispose();
            s_WorldIDGenerator.Reset();
        }
    }
}