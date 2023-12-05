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
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion
using UnityEngine;

namespace CZToolKit.ECS
{
    public struct PositionComponent : IComponent
    {
        public Vector3 value;

        public override string ToString()
        {
            return value.ToString();
        }
    }
}
