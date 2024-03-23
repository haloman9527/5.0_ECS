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

using System;
using UnityEngine;

namespace CZToolKit.ECS
{
    public struct GameObjectComponent : IManagedComponent<GameObject>
    {
        public uint Id { get; set; }
        
        public int WorldId { get; set; }
    }
}