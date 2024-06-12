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
            worldOperationListener?.AfterCreateEntity(this, entity);
            return entity;
        }

        public bool Exists(Entity entity)
        {
            return entities.ContainsKey(entity.id);
        }

        public Entity GetEntity(int entityId)
        {
            entities.TryGetValue(entityId, out var entity);
            return entity;
        }

        public void DestroyEntity(Entity entity)
        {
            if (!entities.ContainsKey(entity.id))
            {
                return;
            }
            worldOperationListener?.BeforeDestroyEntity(this, entity);
            foreach (var components in componentContainers.GetValueArray(Allocator.Temp))
            {
                worldOperationListener?.BeforeRemoveComponent(this, entity, components.typeInfo);
                if (components.typeInfo.isManagedComponentType)
                {
                    references.Release(components.typeInfo.id, entity.id);
                }

                components.Del(entity);
                worldOperationListener?.AfterRemoveComponent(this, entity, components.typeInfo);
            }
            entities.Remove(entity.id);
        }

        public void DestroyEntity(int entityId)
        {
            if (!entities.TryGetValue(entityId, out var entity))
            {
                return;
            }

            DestroyEntity(entity);
        }

        private void DisposeAllEntities()
        {
            worldOperationListener?.BeforeWorldDispose(this);
            entities.Clear();
            entities.Dispose();
            s_WorldIDGenerator.Reset();
            worldOperationListener?.AfterWorldDispose(this);
        }
    }
}