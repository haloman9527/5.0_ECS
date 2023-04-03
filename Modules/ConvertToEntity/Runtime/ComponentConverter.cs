using UnityEngine;

namespace CZToolKit.ECS
{
    [RequireComponent(typeof(ConvertToEntity))]
    public abstract class ComponentConverter : MonoBehaviour
    {
        public abstract void ConvertToComponent(World world, Entity entity);
    }
}