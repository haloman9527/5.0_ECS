namespace CZToolKit.ECS
{
    public static class EntityEx
    {
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

        // public static object GetValue(this IManagedComponent component, World world)
        // {
        //     return world.references.Get(component.Id);
        // }
        //
        // public static void SetValue(this IManagedComponent component, World world, object value)
        // {
        //     world.references.Set(component.Id, value);
        // }

        public static bool IsValid(this Entity entity)
        {
            var world = entity.World;
            return world.Exists(entity);
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

        public static void SetComponent<TC, TR>(this Entity entity, TC component, TR value) where TC : unmanaged, IManagedComponent<TR> where TR : class
        {
            entity.World.SetComponent(entity, component, value);
        }

        public static void SetComponent<TC, TR>(this Entity entity, ref TC component, TR value) where TC : unmanaged, IManagedComponent<TR> where TR : class
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