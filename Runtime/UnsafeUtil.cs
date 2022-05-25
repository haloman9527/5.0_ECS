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
using System.Runtime.CompilerServices;

public static class UnsafeUtil
{
    public unsafe static void* AddressOf<T>(ref T output) where T : struct
    {
        return Unsafe.AsPointer(ref output);
    }

    public unsafe static int SizeOf<T>() where T : struct
    {
        return Unsafe.SizeOf<T>();
    }
}
