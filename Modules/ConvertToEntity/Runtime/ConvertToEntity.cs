using System.Collections.Generic;
using UnityEngine;

namespace CZToolKit.ECS
{
    public class ConvertToEntity : MonoBehaviour
    {
        [SerializeField]
        private bool destroyOnAwake;
        [SerializeReference]
        public List<IComponent> components = new List<IComponent>();
        [HideInInspector]
        public Entity entity;

        private void Start()
        {
            var entity = ECSConverter.ToEntity(gameObject);
            foreach (var component in components)
            {
                World.DefaultWorld.SetComponent(entity, component);
            }
            if (destroyOnAwake)
                GameObject.Destroy(this);
        }
    }
}
