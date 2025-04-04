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

namespace Atom.ECS
{
    public partial class World
    {
        private IdGenerator entityIdGenerator = new IdGenerator();
        private NativeParallelHashMap<uint, Entity> entities = new NativeParallelHashMap<uint, Entity>(256, Allocator.Persistent);

        public NativeParallelHashMap<uint, Entity> Entities => entities;

        public Entity CreateEntity()
        {
            var id = entityIdGenerator.Next();
            var entity = new Entity(this.Id, id);
            entities.Add(id, entity);
            worldOperationListener?.OnCreateEntity(this, entity);
            return entity;
        }

        public bool Valid(Entity entity)
        {
            return entity.worldId == this.Id && entities.ContainsKey(entity.id);
        }

        public Entity GetEntity(uint entityId)
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
            worldOperationListener?.OnDestroyEntity(this, entity);
            foreach (var components in componentContainers.GetValueArray(Allocator.Temp))
            {
                worldOperationListener?.OnComponentOperate(this, entity, components.typeInfo, ComponentOpoeration.Remove);
                if (components.typeInfo.isManagedComponentType)
                {
                    references.Release(components.typeInfo.id, entity.id);
                }

                components.Del(entity);
            }
            entities.Remove(entity.id);
        }

        public void DestroyEntity(uint entityId)
        {
            if (!entities.TryGetValue(entityId, out var entity))
            {
                return;
            }

            DestroyEntity(entity);
        }

        private void DisposeAllEntities()
        {
            entities.Clear();
            entities.Dispose();
        }
    }
}