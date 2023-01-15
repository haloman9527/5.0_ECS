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

using System;
using UnityEngine;

namespace CZToolKit.ECS
{
    public struct GameObjectComponent : IComponent, IDisposable
    {
        [HideInInspector] public uint id;

        public uint ID
        {
            get { return id; }
        }

        public GameObjectComponent(GameObject gameObject)
        {
            this.id = ECSReferences.Set(gameObject);
        }

        public GameObject Value
        {
            get { return ECSReferences.Get(id) as GameObject; }
            set { id = ECSReferences.Set(Value); }
        }

        public void Dispose()
        {
            ECSReferences.Release(id);
        }
    }
}