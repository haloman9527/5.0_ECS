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

namespace CZToolKit.ECS
{
    public class ECSReferences
    {
        private uint lastIndex = 0;
        private Dictionary<uint, object> map = new Dictionary<uint, object>();

        public uint Alloc()
        {
            lastIndex++;
            map[lastIndex] = null;
            return lastIndex;
        }
        
        public void Set(uint id, object data)
        {
            map[id] = data;
        }

        public object Get(uint id)
        {
            return map[id];
        }

        public void Release(uint id)
        {
            map.Remove(id);
        }
        
        public void Clear()
        {
            lastIndex = 0;
            map.Clear();
        }
    }
}