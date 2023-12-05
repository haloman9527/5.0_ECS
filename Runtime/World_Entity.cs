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
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion
using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CZToolKit.ECS
{
    public partial class World : IDisposable
    {
        private IndexGenerator entityIndexGenerator = new IndexGenerator();
        private NativeHashMap<int, Entity> entities = new NativeHashMap<int, Entity>(256, Allocator.Persistent);

        public NativeHashMap<int, Entity> Entities
        {
            get { return entities; }
        }

        public Entity NewEntity()
        {
            var index = entityIndexGenerator.Next();
            var entity = new Entity(index);
            entities.Add(index, entity);
            return entity;
        }

        public void NewEntity(out Entity entity)
        {
            var index = entityIndexGenerator.Next();
            entity = new Entity(index);
            entities.Add(entity.index, entity);
        }

        public bool Exists(Entity entity)
        {
            return entities.ContainsKey(entity.index);
        }

        public void DestroyEntity(Entity entity)
        {
            if (entity.index == singleton.index)
                throw new Exception("Can't Destory Singleton Entity!!!");
            entities.Remove(entity.index);
            foreach (var components in componentContainers.GetValueArray(Allocator.Temp))
            {
                components.Del(entity);
            }
        }

        public void DestroyEntities()
        {
            entities.Clear();
            foreach (var components in componentContainers.GetValueArray(Allocator.Temp))
            {
                components.Dispose();
            }
            componentContainers.Clear();
        }
    }
}
