using System.Collections.Generic;
using UnityEngine;

namespace CZToolKit.ECS.ConvertToEntity
{
    public class ConvertToEntity : MonoBehaviour
    {
        [SerializeReference]
        public List<IComponent> components = new List<IComponent>();

        private void Awake()
        {
            WorldManager.Instance.mainWorld.NewEntity(out var entity);
            foreach (var component in components)
            {
                entity.AddComponent(component.GetType(), component);
            }
            GameObject.Destroy(this);
        }
    }
}
