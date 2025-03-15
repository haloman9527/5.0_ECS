using UnityEngine;

namespace Atom.ECS
{
    [DisallowMultipleComponent]
    public class EntityPreset : MonoBehaviour
    {
        public void GenerateEntity(World world)
        {
            var entity = world.CreateEntity();

            foreach (var task in GetComponents<IEntityBuildTask>())
            {
                task.Execute(world, in entity);
            }
        }
    }

    public interface IEntityBuildTask
    {
        void Execute(World world, in Entity entity);
    }
}