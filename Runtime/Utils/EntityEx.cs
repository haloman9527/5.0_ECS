namespace Jiange.ECS
{
    public static class EntityEx
    {
        public static T GetValue<C, T>(this C component) where C : unmanaged, IManagedComponent where T : class
        {
            var world = World.GetWorld(component.WorldId);
            if (world == null)
            {
                return null;
            }

            var typeInfo = TypeManager.GetTypeInfo(component.GetType());
            return world.references.Get(typeInfo.id, component.EntityId) as T;
        }

        public static void SetValue<C, T>(this C component, T value) where C : unmanaged, IManagedComponent where T : class
        {
            var world = World.GetWorld(component.WorldId);
            var typeInfo = TypeManager.GetTypeInfo(component.GetType());
            world.references.Set(typeInfo.id, component.EntityId, value);
        }

        public static T GetValue<T>(this IManagedComponent<T> component) where T : class
        {
            var world = World.GetWorld(component.WorldId);
            if (world == null)
            {
                return null;
            }

            var typeInfo = TypeManager.GetTypeInfo(component.GetType());
            return world.references.Get(typeInfo.id, component.EntityId) as T;
        }

        public static void SetValue<T>(this IManagedComponent<T> component, T value) where T : class
        {
            var world = World.GetWorld(component.WorldId);
            var typeInfo = TypeManager.GetTypeInfo(component.GetType());
            world.references.Set(typeInfo.id, component.EntityId, value);
        }

        public static object GetValue(this IManagedComponent component)
        {
            var world = World.GetWorld(component.WorldId);
            if (world == null)
            {
                return null;
            }

            var typeInfo = TypeManager.GetTypeInfo(component.GetType());
            return world.references.Get(typeInfo.id, component.EntityId);
        }

        public static void SetValue(this IManagedComponent component, object value)
        {
            var world = World.GetWorld(component.WorldId);
            var typeInfo = TypeManager.GetTypeInfo(component.GetType());
            world.references.Set(typeInfo.id, component.EntityId, value);
        }

        public static bool IsValid(this Entity entity)
        {
            var world = entity.World;
            return world != null && world.Exists(entity);
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

        public static void SetComponent<T>(this Entity entity, ref T component) where T : unmanaged, IComponent
        {
            entity.World.SetComponent(entity, ref component);
        }

        public static void SetComponent<T>(this Entity entity, T component) where T : unmanaged, IComponent
        {
            entity.World.SetComponent(entity, component);
        }

        public static void SetComponent<C, R>(this Entity entity, C component, R value) where C : unmanaged, IManagedComponent<R> where R : class
        {
            entity.World.SetComponent(entity, component, value);
        }

        public static void SetComponent<C, R>(this Entity entity, ref C component, R value) where C : unmanaged, IManagedComponent<R> where R : class
        {
            entity.World.SetComponent(entity, ref component, value);
        }

        public static void RemoveComponent<T>(this Entity entity) where T : unmanaged, IComponent
        {
            entity.World.RemoveComponent(entity, typeof(T));
        }

        public static bool HasComponent<T>(this Entity entity) where T : unmanaged, IComponent
        {
            return entity.World.HasComponent<T>(entity);
        }
    }
}