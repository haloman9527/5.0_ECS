using System.Collections.Generic;
using UnityEngine;

namespace CZToolKit.ECS
{
    public interface IConvertToComponent
    {
        void ConvertToComponent(World world, Entity entity);
    }

    [DisallowMultipleComponent]
    public class ConvertToEntity : MonoBehaviour
    {
        [SerializeField] 
        private bool destroyOnAwake;
        [SerializeReference] 
        public List<IComponent> components = new List<IComponent>();

        public Entity Entity { get; private set; }

        private void Awake()
        {
            Entity = World.DefaultWorld.NewEntity();

            World.DefaultWorld.SetComponent(Entity, new GameObjectComponent(gameObject));
            World.DefaultWorld.SetComponent(Entity, new PositionComponent() { value = transform.position });
            World.DefaultWorld.SetComponent(Entity, new RotationComponent() { value = transform.rotation });
            World.DefaultWorld.SetComponent(Entity, new ScaleComponent() { value = transform.localScale });

            foreach (var component in components)
            {
                World.DefaultWorld.SetComponent(Entity, component);
            }

            foreach (var convert in GetComponents<IConvertToComponent>())
            {
                convert.ConvertToComponent(World.DefaultWorld, Entity);
            }

            if (destroyOnAwake)
                GameObject.Destroy(this);
        }
    }
}