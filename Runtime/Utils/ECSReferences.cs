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
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion
using System.Collections.Generic;

namespace CZToolKit.ECS
{
    public class ECSReferences
    {
        private static uint s_Index = 0;

        private static readonly Dictionary<uint, object> s_References = new Dictionary<uint, object>();

        public static uint Set(object data)
        {
            s_References[++s_Index] = data;
            return s_Index;
        }

        public static object Get(uint id)
        {
            return s_References[id];
        }

        public static void Release(uint id)
        {
            s_References.Remove(id);
        }
    }
}
