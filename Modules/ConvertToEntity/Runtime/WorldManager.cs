using CZToolKit.Core.Singletons;

namespace CZToolKit.ECS.ConvertToEntity
{
    public class WorldManager : CZNormalSingleton<WorldManager>
    {
        public readonly World mainWorld;

        public WorldManager()
        {
            mainWorld = new World();
        }
    }
}
