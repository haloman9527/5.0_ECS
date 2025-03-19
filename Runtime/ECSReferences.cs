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
        private Dictionary<long, object> values = new Dictionary<long, object>();

        public void Set(int compnentId, int entityId, object data)
        {
            var refId = ((long)entityId) << 32 | (uint)compnentId;
            values[refId] = data;
        }

        public object Get(int compnentId, int entityId)
        {
            var refId = ((long)entityId) << 32 | (uint)compnentId;
            if (values.TryGetValue(refId, out var value))
            {
                return value;
            }

            return null;
        }

        public void Release(int compnentId, int entityId)
        {
            var refId = ((long)entityId) << 32 | (uint)compnentId;
            values.Remove(entityId);
        }

        public void Clear()
        {
            values.Clear();
        }
    }
}