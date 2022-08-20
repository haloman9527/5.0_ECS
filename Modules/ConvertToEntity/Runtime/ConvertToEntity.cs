using System.Collections.Generic;
using UnityEngine;

namespace CZToolKit.ECS
{
    public class ConvertToEntity : MonoBehaviour
    {
        public bool destroyOnAwake;
        [SerializeReference]
        public List<IComponent> components = new List<IComponent>();

        private void Start()
        {
            World.DefaultWorld.NewEntity(out var entity);
            foreach (var component in components)
            {
                World.DefaultWorld.SetComponent(entity, component);
            }
            if (destroyOnAwake)
                GameObject.Destroy(this);
        }
    }
}
