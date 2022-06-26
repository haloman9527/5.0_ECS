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
    delegate int Func1<T>();

    public static class UnsafeUtil
    {
        public unsafe static int SizeOf<T>()
        {
            return Unsafe.SizeOf<T>();
        }
    }
}
