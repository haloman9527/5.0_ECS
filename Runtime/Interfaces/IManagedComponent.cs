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

using System;

namespace CZToolKit.ECS
{
    public interface IManagedComponent : IComponent
    {
        void Release();
    }
}
