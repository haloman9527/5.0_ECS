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
using Unity.Collections;

namespace CZToolKit.ECS
{
    public partial class World : IDisposable
    {
        private readonly IDGenerator entityIndexGenerator = new IDGenerator();
        private readonly NativeHashMap<int, Entity> entities = new NativeHashMap<int, Entity>(64, Allocator.Persistent);

        public NativeHashMap<int, Entity> Entities
        {
            get { return entities; }
        }

        private Entity NewEntity(int index)
        {
            var entity = new Entity(index);
            entities.Add(index, entity);
            return entity;
        }

        public Entity NewEntity()
        {
            var index = entityIndexGenerator.GenerateID();
            var entity = new Entity(index);
            entities.Add(index, entity);
            return entity;
        }

        public void NewEntity(out Entity entity)
        {
            var id = entityIndexGenerator.GenerateID();
            entity = new Entity(id);
            entities.Add(id, entity);
        }

        public bool Exists(int entityID)
        {
            return entities.ContainsKey(entityID);
        }

        public bool Exists(Entity entity)
        {
            return entities.ContainsKey(entity.index);
        }

        public void DestroyEntityImmediate(int entityID)
        {
            DestroyEntityImmediate(entities[entityID]);
        }

        public unsafe void DestroyEntityImmediate(Entity entity)
        {
            entities.Remove(entity.index);
            foreach (var components in componentPools.GetValueArray(Allocator.Temp))
            {
                components.Del(entity);
            }
        }
    }
}
