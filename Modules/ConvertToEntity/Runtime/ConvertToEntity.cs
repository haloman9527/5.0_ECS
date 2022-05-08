using System.Collections.Generic;
using UnityEngine;

namespace CZToolKit.ECS
{
    public class ConvertToEntity : MonoBehaviour
    {
        public bool destroyOnAwake;
        [SerializeReference]
        public List<IComponent> components = new List<IComponent>();

        private void Awake()
        {
            World.DefaultWorld.NewEntity(out var entity);
            foreach (var component in components)
            {
                entity.AddComponent(component.GetType(), component);
            }
            if (destroyOnAwake)
                GameObject.Destroy(this);
        }
    }
}
