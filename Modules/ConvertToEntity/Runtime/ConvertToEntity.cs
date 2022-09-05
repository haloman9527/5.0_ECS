using System.Collections.Generic;
using UnityEngine;

namespace CZToolKit.ECS
{
    public interface IConvertToComponent
    {
        void ConvertToComponent(World world, Entity entity);
    }

    public class ConvertToEntity : MonoBehaviour
    {
        [SerializeField]
        private bool destroyOnAwake;
        [SerializeReference]
        public List<IComponent> components = new List<IComponent>();
        [HideInInspector]
        private Entity entity;

        public Entity Entity
        {
            get { return entity; }
        }

        private void Start()
        {
            entity = ECSConverter.ToEntity(gameObject);
            foreach (var component in components)
            {
                World.DefaultWorld.SetComponent(entity, component);
            }
            foreach (var convert in GetComponents<IConvertToComponent>())
            {
                convert.ConvertToComponent(World.DefaultWorld, entity);
            }
            if (destroyOnAwake)
                GameObject.Destroy(this);
        }
    }
}
