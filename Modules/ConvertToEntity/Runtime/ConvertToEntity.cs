using System.Collections.Generic;
using UnityEngine;

namespace CZToolKit.ECS
{
    public class ConvertToEntity : MonoBehaviour
    {
        [SerializeReference]
        public List<IComponent> components = new List<IComponent>();

        private void Awake()
        {
            WorldManager.MainWorld.NewEntity(out var entity);
            foreach (var component in components)
            {
                entity.AddComponent(component.GetType(), component);
            }
            GameObject.Destroy(this);
        }
    }
}
