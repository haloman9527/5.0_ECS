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
using System.Runtime.CompilerServices;

namespace CZToolKit.ECS
{
    public static class UnsafeUtil
    {
        public static unsafe ref T AsRef<T>(IntPtr ptr)
        {
            return ref Unsafe.AsRef<T>((void*)ptr);
        }
    }
}