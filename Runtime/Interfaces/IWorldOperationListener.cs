
namespace CZToolKit.ECS
{
    public interface IWorldOperationListener
    {
        void OnCreateEntity(World world, Entity entity);

        void OnDestroyEntity(World world, Entity entity);
        
        void OnSetComponent(World world, Entity entity, TypeInfo component);

        void BeforeRemoveComponent(World world, Entity entity, TypeInfo componentType);

        void AfterRemoveComponent(World world, Entity entity, TypeInfo componentType);

        void BeforeWorldDispose(World world);

        void AfterWorldDispose(World world);
    }
}