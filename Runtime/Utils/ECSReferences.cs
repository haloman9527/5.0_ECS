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
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */
#endregion
using System.Collections.Generic;

namespace CZToolKit.ECS
{
    public class ECSReferences
    {
        private static uint index = 0;

        private static readonly Dictionary<uint, object> references = new Dictionary<uint, object>();

        public static uint Set(object data)
        {
            references[++index] = data;
            return index;
        }

        public static object Get(uint id)
        {
            return references[id];
        }

        public static void Release(uint id)
        {
            references.Remove(id);
        }
    }
}
