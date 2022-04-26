
namespace CZToolKit.ECS
{
    public class WorldManager
    {
        public static readonly World MainWorld;

        static WorldManager()
        {
            MainWorld = World.NewWorld();
        }
    }
}
