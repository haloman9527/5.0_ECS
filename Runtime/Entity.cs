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
        public readonly int index;
        public readonly int worldIndex;
        public World World
        {
            get
            {
                if (World.AllWorlds.TryGetValue(worldIndex, out var world) && world.IsValid(index))
                    return world;
                return null;
            }
        }

        internal Entity(int index, World world)
        {
            this.index = index;
            this.worldIndex = world.index;
        }

        internal Entity(int index, int worldIndex)
        {
            this.index = index;
            this.worldIndex = worldIndex;
        }

        public static bool operator ==(in Entity lhs, in Entity rhs)
        {
            return lhs.index == rhs.index && lhs.worldIndex == rhs.worldIndex;
        }

        public static bool operator !=(in Entity lhs, in Entity rhs)
        {
            return lhs.index != rhs.index || lhs.worldIndex != rhs.worldIndex;
        }

        public override bool Equals(object other)
        {
            return Equals((Entity)other);
        }

        public bool Equals(Entity other)
        {
            return this.index == other.index && this.worldIndex == other.worldIndex;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (index * 397) ^ worldIndex.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"{worldIndex}->{index}";
        }
    }

    public static class ExtityExtension
    {
        public static bool IsValid(this in Entity entity)
        {
            if (entity.worldIndex == 0)
                return false;
            var world = entity.World;
            return world != null;
        }

        public unsafe static bool HasComponent<T>(this Entity entity) where T : struct, IComponent
        {
            return entity.World.HasComponent<T>(entity);
        }

        public unsafe static bool HasComponent(this Entity entity, Type componentType)
        {
            return entity.World.HasComponent(entity, componentType);
        }

        public unsafe static T GetComponent<T>(this Entity entity) where T : struct, IComponent
        {
            return entity.World.GetComponent<T>(entity);
        }

        public unsafe static bool TryGetComponent<T>(this Entity entity, out T component) where T : struct, IComponent
        {
            return entity.World.TryGetComponent<T>(entity, out component);
        }

        public unsafe static ref T RefComponent<T>(this Entity entity) where T : struct, IComponent
        {
            return ref entity.World.RefComponent<T>(entity);
        }

        public unsafe static void AddComponent<T>(this Entity entity, T component) where T : struct, IComponent
        {
            entity.World.AddComponent(entity, in component);
        }

        public unsafe static void AddComponent(this Entity entity, Type type, object component)
        {
            entity.World.AddComponent(entity, type, component);
        }

        public unsafe static void SetComponent<T>(this Entity entity, T component) where T : struct, IComponent
        {
            entity.World.SetComponent(entity, in component);
        }

        public unsafe static void RemoveComponent<T>(this Entity entity)
        {
            entity.World.RemoveComponent<T>(entity);
        }
    }
}