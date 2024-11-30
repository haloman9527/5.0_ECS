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
using UnityEngine;

namespace Moyo.ECS
{
    public struct ScaleComponent : IComponent
    {
        public Vector3 value;

        public override string ToString()
        {
            return value.ToString();
        }
    }
}
