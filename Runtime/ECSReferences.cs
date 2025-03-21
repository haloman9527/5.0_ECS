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
            return (ulong)compnentId << 32 | entityId;
        }
        
        public void Set(uint compnentId, uint entityId, object data)
        {
            var referenceId = GetReferenceId(compnentId, entityId);
            values[referenceId] = data;
        }

        public object Get(uint compnentId, uint entityId)
        {
            var referenceId = GetReferenceId(compnentId, entityId);
            if (values.TryGetValue(referenceId, out var value))
            {
                return value;
            }

            return null;
        }

        public void Release(uint compnentId, uint entityId)
        {
            var referenceId = GetReferenceId(compnentId, entityId);
            values.Remove(referenceId);
        }

        public void Clear()
        {
            values.Clear();
        }
    }
}