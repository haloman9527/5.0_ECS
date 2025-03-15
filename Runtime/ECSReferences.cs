#region 注 释

/***
 *
 *  Title:
 *
 *  Description:
 *
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/haloman9527
 *  Blog: https://www.haloman.net/
 *
 */

#endregion

using System.Collections.Generic;

namespace Atom.ECS
{
    public class ECSReferences
    {
        private Dictionary<int, Dictionary<int, object>> map = new Dictionary<int, Dictionary<int, object>>();

        public void Set(int compnentId, int entityId, object data)
        {
            if (!map.TryGetValue(compnentId, out var container))
            {
                map[compnentId] = container = new Dictionary<int, object>();
            }

            container[entityId] = data;
        }

        public object Get(int compnentId, int entityId)
        {
            if (!map.TryGetValue(compnentId, out var container))
            {
                return null;
            }

            if (!container.TryGetValue(entityId, out var o))
            {
                return null;
            }

            return o;
        }

        public void Release(int compnentId, int entityId)
        {
            if (!map.TryGetValue(compnentId, out var container))
            {
                return;
            }

            container.Remove(entityId);
        }

        public void Clear()
        {
            foreach (var container in map.Values)
            {
                container.Clear();
            }
        }
    }
}