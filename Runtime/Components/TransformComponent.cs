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

using UnityEngine;

namespace CZToolKit.ECS
{
    public struct TransformComponent : IComponent
    {
        [HideInInspector]
        public uint id;

        public TransformComponent(Transform cutscene)
        {
            this.id = ECSReferences.Set(cutscene);
        }

        public Transform Value
        {
            get { return ECSReferences.Get(id) as Transform; }
            set { id = ECSReferences.Set(Value); }
        }

        public void Release()
        {
            ECSReferences.Release(id);
        }
    }
}
