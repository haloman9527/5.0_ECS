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
        public World World
        {
            get
            {
                if (World.Worlds.TryGetValue(WorldID, out var world) && world.IsValid(ID))
                    return world;
                return null;
            }
        }

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
                return (ID * 397) ^ WorldID.GetHashCode();
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
            if (entity.WorldID == 0)
                return false;
            var world = entity.World;
            return world != null;
        }

        public unsafe static bool HasComponent<T>(this Entity entity) where T : struct, IComponent
        {
            return entity.World.HasComponent<T>(&entity);
        }

        public unsafe static bool HasComponent(this Entity entity, Type componentType)
        {
            return entity.World.HasComponent(&entity, componentType);
        }

        public unsafe static T GetComponent<T>(this Entity entity) where T : struct, IComponent
        {
            return entity.World.GetComponent<T>(&entity);
        }

        public unsafe static bool TryGetComponent<T>(this Entity entity, out T component) where T : struct, IComponent
        {
            return entity.World.TryGetComponent<T>(&entity, out component);
        }

        public unsafe static ref T RefComponent<T>(this Entity entity) where T : struct, IComponent
        {
            return ref entity.World.RefComponent<T>(&entity);
        }

        public unsafe static void AddComponent<T>(this Entity entity, T component) where T : struct, IComponent
        {
            entity.World.AddComponent(&entity, in component);
        }

        public unsafe static void AddComponent(this Entity entity, Type type, object component)
        {
            entity.World.AddComponent(&entity, type, component);
        }

        public unsafe static void SetComponent<T>(this Entity entity, T component) where T : struct, IComponent
        {
            entity.World.SetComponent(&entity, in component);
        }

        public unsafe static void RemoveComponent<T>(this Entity entity)
        {
            entity.World.RemoveComponent<T>(&entity);
        }
    }
}