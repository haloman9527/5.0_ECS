namespace Atom.ECS
{
    public static class EntityEx
    {
        public static bool IsValid(this Entity entity)
        {
            var world = entity.World;
            return world != null && world.Valid(entity);
        }

        public static bool HasComponent<T>(this Entity entity) where T : unmanaged, IComponent
        {
            return entity.World.HasComponent<T>(entity);
        }

        public static T GetComponent<T>(this Entity entity) where T : unmanaged, IComponent
        {
            return entity.World.GetComponent<T>(entity);
        }

        public static bool TryGetComponent<T>(this Entity entity, out T component) where T : unmanaged, IComponent
        {
            return entity.World.TryGetComponent<T>(entity, out component);
        }

        public static ref T RefComponent<T>(this Entity entity) where T : unmanaged, IComponent
        {
            return ref entity.World.RefComponent<T>(entity);
        }

        public static void SetComponent<T>(this Entity entity, T component) where T : unmanaged, IComponent
        {
            entity.World.SetComponent(entity, component);
        }

        public static void SetComponent<C, V>(this Entity entity, C component, V refValue) where C : unmanaged, IManagedComponent<V> where V : class
        {
            entity.World.SetComponent(entity, component, refValue);
        }

        public static void RemoveComponent<T>(this Entity entity) where T : unmanaged, IComponent
        {
            entity.World.RemoveComponent(entity, TypeCache<T>.TYPE);
        }

        public static void SetRefValue<C, V>(this Entity entity, V value) where C : unmanaged, IManagedComponent<V> where V : class
        {
            entity.World.SetRefValue<C, V>(entity, value);
        }

        public static void SetRefValue<C, V>(this C component, V value) where C : unmanaged, IManagedComponent<V> where V : class
        {
            var world = World.GetWorld(component.WorldId);
            if (world == null)
            {
                return;
            }

            world.SetRefValue(component, value);
        }

        public static V GetRefValue<C, V>(this Entity entity) where C : unmanaged, IManagedComponent<V> where V : class
        {
            return entity.World.GetRefValue<C, V>(entity);
        }

        public static V GetRefValue<C, V>(this C component) where C : unmanaged, IManagedComponent<V> where V : class
        {
            var world = World.GetWorld(component.WorldId);
            if (world == null)
            {
                return null;
            }

            return world.GetRefValue<C, V>(component);
        }
    }
}