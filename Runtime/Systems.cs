using System.Collections.Generic;

namespace CZToolKit.ECS
{
    public class Systems : ComponentSystem
    {
        protected readonly List<ISystem> systems = new List<ISystem>();

        public virtual Systems Add(ISystem system)
        {
            systems.Add(system);
            return this;
        }

        public void Remove(ISystem system)
        {
            systems.Remove(system);
        }

        public void Clear()
        {
            for (var i = 0; i < systems.Count; i++)
            {
                var system = systems[i];

                if (system is Systems nestedSystems)
                    nestedSystems.Clear();
            }
        }

        public override void Execute()
        {
            foreach (var system in systems)
            {
                if (system.Active)
                {
                    system.Execute();
                }
            }
        }
    }
}