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
        public int WorldID { get; }
        public World World { get { return World.Worlds[WorldID]; } }

        internal Entity(int id, World world)
        {
            this.ID = id;
            this.WorldID = world.id;
        }

        internal Entity(int id, int worldID)
        {
            this.ID = id;
            this.WorldID = worldID;
        }

        public static bool operator ==(in Entity lhs, in Entity rhs)
        {
            return lhs.ID == rhs.ID && lhs.WorldID == rhs.WorldID;
        }

        public static bool operator !=(in Entity lhs, in Entity rhs)
        {
            return lhs.ID != rhs.ID || lhs.WorldID != rhs.WorldID;
        }

        public override bool Equals(object other)
        {
            return Equals((Entity)other);
        }

        public bool Equals(Entity other)
        {
            return this.ID == other.ID && this.WorldID == other.WorldID;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ID * 397) ^ (World == null ? 0 : WorldID.GetHashCode());
            }
        }

        public override string ToString()
        {
            return $"{WorldID}->{ID}";
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
            return entity.World.HasComponent<T>(entity);
        }

        public static bool HasComponent(this in Entity entity, Type componentType)
        {
            return entity.World.HasComponent(entity, componentType);
        }

        public static void AddComponent<T>(this in Entity entity, T component) where T : struct, IComponent
        {
            entity.World.AddComponent<T>(entity, in component);
        }

        public static void AddComponent(this in Entity entity, Type type, object component)
        {
            entity.World.AddComponent(entity, type, component);
        }

        public static T GetComponent<T>(this in Entity entity) where T : struct, IComponent
        {
            return entity.World.GetComponentPool<T>().Get(entity);
        }

        public static ref T RefComponent<T>(this in Entity entity) where T : struct, IComponent
        {
            return ref entity.World.RefComponent<T>(entity);
        }

        public static void SetComponent<T>(this in Entity entity, T component) where T : struct, IComponent
        {
            entity.World.SetComponent(entity, in component);
        }

        public static void RemoveComponent<T>(this in Entity entity)
        {
            entity.World.RemoveComponent<T>(entity);
        }
    }
}