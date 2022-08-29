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
        private readonly NativeHashMap<uint, Entity> entities = new NativeHashMap<uint, Entity>(64, Allocator.Persistent);

        public NativeHashMap<uint, Entity> Entities
        {
            get { return entities; }
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

        public bool Exists(Entity entity)
        {
            return entities.ContainsKey(entity.index);
        }

        public void DestroyEntityImmediate(Entity entity)
        {
            if (entity.index == singleton.index)
                throw new Exception("Can't Destory Singleton Entity!!!");
            entities.Remove(entity.index);
            foreach (var components in componentPools.GetValueArray(Allocator.Temp))
            {
                components.Del(entity);
            }
        }
    }
}
