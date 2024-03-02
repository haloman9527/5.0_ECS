using UnityEngine;

namespace CZToolKit.ECS
{
    [DisallowMultipleComponent]
    public class ConvertToEntity : MonoBehaviour
    {
        [SerializeField] 
        private bool destroyOnAwake;
        public Entity Entity { get; private set; }

        private void Awake()
        {
            Entity = World.DefaultWorld.NewEntity();

            var goc = new GameObjectComponent();
            World.DefaultWorld.SetComponent(Entity, ref goc);
            goc.SetValue(World.DefaultWorld, gameObject);
            
            World.DefaultWorld.SetComponent(Entity, new PositionComponent() { value = transform.position });
            World.DefaultWorld.SetComponent(Entity, new RotationComponent() { value = transform.rotation });
            World.DefaultWorld.SetComponent(Entity, new ScaleComponent() { value = transform.localScale });

            foreach (var convert in GetComponents<ComponentConverter>())
            {
                convert.ConvertToComponent(World.DefaultWorld, Entity);
            }

            if (destroyOnAwake)
                GameObject.Destroy(this);
        }
    }
}