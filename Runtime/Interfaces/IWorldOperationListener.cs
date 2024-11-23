
namespace Jiange.ECS
{
    public interface IWorldOperationListener
    {
        void AfterCreateEntity(World world, Entity entity);

        void BeforeDestroyEntity(World world, Entity entity);
        
        void OnSetComponent(World world, Entity entity, TypeInfo componentType);

        void BeforeRemoveComponent(World world, Entity entity, TypeInfo componentType);

        void AfterRemoveComponent(World world, Entity entity, TypeInfo componentType);

        void BeforeWorldDispose(World world);

        void AfterWorldDispose(World world);
    }
}