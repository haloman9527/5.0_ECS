
namespace Atom.ECS
{
    public enum ComponentOpoeration
    {
        Added,
        Changed,
        Remove,
    }
    
    public interface IWorldOperationListener
    {
        void OnCreateEntity(World world, Entity entity);

        void OnDestroyEntity(World world, Entity entity);
        
        void OnComponentOperate(World world, Entity entity, TypeInfo componentType, ComponentOpoeration componentOpoeration);

        void OnWorldDispose(World world);
    }
}