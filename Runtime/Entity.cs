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

namespace CZToolKit.ECS
{
    public struct Entity : IEquatable<Entity>
    {
        public int ID { get; }
        public World World { get; }

        public Entity(int id, World world)
        {
            this.ID = id;
            this.World = world;
        }

        public static bool operator ==(in Entity lhs, in Entity rhs)
        {
            return lhs.ID == rhs.ID && lhs.World == rhs.World;
        }

        public static bool operator !=(in Entity lhs, in Entity rhs)
        {
            return lhs.ID != rhs.ID && lhs.World != rhs.World;
        }

        public override bool Equals(object other)
        {
            return other is Entity otherEntity && Equals(otherEntity);
        }

        public bool Equals(Entity other)
        {
            return this.ID == other.ID && this.World == other.World;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ID * 397) ^ (World != null ? World.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public static class ExtityExtension
    {
        public static bool IsValid(this in Entity entity)
        {
            var world = entity.World;
            if (null == world)
                return false;
            return world.ContainsEntity(entity.ID);
        }

        public static bool HasComponent<T>(this in Entity entity) where T : struct, IComponent
        {
            var componentPool = entity.World.GetComponentPool<T>();
            if (null != componentPool && componentPool.Contains(entity.ID))
                return true;
            return false;
        }

        public static bool HasComponent(this in Entity entity, Type componentType)
        {
            var componentPool = entity.World.GetComponentPool(componentType);
            if (null != componentPool && componentPool.Contains(entity.ID))
                return true;
            return false;
        }

        public static void AddComponent<T>(this in Entity entity, T component) where T : struct, IComponent
        {
            var componentPool = entity.World.GetComponentPool<T>();
            if (componentPool == null)
                componentPool = entity.World.NewComponentPool<T>();
            else if (componentPool.Contains(entity.ID))
                throw new Exception($"Alreay had {nameof(T)} component!");
            componentPool.Set(entity.ID, component);
        }

        public static void AddComponent(this in Entity entity, Type type, object component)
        {
            var componentPool = entity.World.GetComponentPool(type);
            if (componentPool == null)
                componentPool = entity.World.NewComponentPool(type);
            else if (componentPool.Contains(entity.ID))
                throw new Exception($"Alreay had type component!");
            componentPool.Set(entity.ID, component);
        }

        public static T GetComponent<T>(this in Entity entity) where T : struct, IComponent
        {
            return entity.World.GetComponentPool<T>().Get(entity.ID);
        }

        public static ref T RefComponent<T>(this in Entity entity) where T : struct, IComponent
        {
            return ref entity.World.GetComponentPool<T>().Ref(entity.ID);
        }

        public static void SetComponent<T>(this in Entity entity, T component) where T : struct, IComponent
        {
            var entityID = entity.ID;
            var world = entity.World;
            var componentPool = world.GetComponentPool<T>();
            if (componentPool == null)
                componentPool = world.NewComponentPool<T>();
            componentPool.Set(entityID, component);
        }

        public static void RemoveComponent<T>(this in Entity entity)
        {
            entity.World.GetComponentPool(typeof(T)).Del(entity.ID);
        }
    }
}