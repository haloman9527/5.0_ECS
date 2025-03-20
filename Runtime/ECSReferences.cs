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
        private Dictionary<ulong, object> values = new Dictionary<ulong, object>();

        private ulong GetReferenceId(uint compnentId, uint entityId)
        {
            return (ulong)entityId << 32 | compnentId;
        }
        
        public void Set(uint compnentId, uint entityId, object data)
        {
            values[GetReferenceId(compnentId, entityId)] = data;
        }

        public object Get(uint compnentId, uint entityId)
        {
            if (values.TryGetValue(GetReferenceId(compnentId, entityId), out var value))
            {
                return value;
            }

            return null;
        }

        public void Release(uint compnentId, uint entityId)
        {
            values.Remove(GetReferenceId(compnentId, entityId));
        }

        public void Clear()
        {
            values.Clear();
        }
    }
}