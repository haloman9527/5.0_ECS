using System;
using System.Reflection;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Atom.ECS
{
    public unsafe static class UnsafeUtil
    {
        /// <summary>
        /// 获取值类型的地址
        /// </summary>
        /// <param name="tf"></param>
        /// <returns></returns>
        public static IntPtr GetValueAddr(TypedReference tf)
        {
            return *(IntPtr*)&tf;
        }

        /// <summary>
        /// 获取值类型的地址
        /// </summary>
        /// <param name="tf"></param>
        /// <returns></returns>
        public static void* GetValueAddrVoidPtr(TypedReference tf)
        {
            return *(void**)&tf;
        }

        /// <summary>
        /// 获取引用类型的地址
        /// </summary>
        /// <param name="tf"></param>
        /// <returns></returns>
        public unsafe static IntPtr GetObjectAddr(TypedReference tf)
        {
            return *(IntPtr*)*(IntPtr*)&tf;
        }

        public static int AlignOf<T>() where T : struct
        {
            return UnsafeUtility.AlignOf<T>();
        }

        public static bool IsUnmanaged<T>()
        {
            return UnsafeUtility.IsUnmanaged<T>();
        }

        public static bool IsUnmanaged(Type type)
        {
            return UnsafeUtility.IsUnmanaged(type);
        }

        public static int SizeOf<T>() where T : struct
        {
            return UnsafeUtility.SizeOf<T>();
        }

        public static int SizeOf(Type type)
        {
            return UnsafeUtility.SizeOf(type);
        }

        public static void* Malloc(long size)
        {
            return UnsafeUtility.Malloc(size, 4, Allocator.Persistent);
        }

        public static void* Malloc(long size, int alignment, Allocator allocator)
        {
            return UnsafeUtility.Malloc(size, alignment, allocator);
        }

        public static void Free(void* ptr, Allocator allocator)
        {
            UnsafeUtility.Free(ptr, allocator);
        }

        public static void Wirte<T>(void* dest, T value) where T : unmanaged
        {
            ((T*)dest)[0] = value;
        }

        public static T Read<T>(void* ptr) where T : unmanaged
        {
            return *(T*)ptr;
        }

        public static T Read<T>(IntPtr ptr) where T : unmanaged
        {
            return *(T*)ptr;
        }

        public static ref T AsRef<T>(void* ptr) where T : unmanaged
        {
            return ref *(T*)ptr;
        }

        public static ref T AsRef<T>(IntPtr ptr) where T : unmanaged
        {
            return ref *(T*)ptr;
        }

        public static ref TTo As<TFrom, TTo>(ref TFrom from) where TFrom : unmanaged where TTo : unmanaged
        {
            fixed (TFrom* ptr = &from)
            {
                return ref *(TTo*)ptr;
            }
        }

        public static int GetFieldOffset(FieldInfo field)
        {
            return UnsafeUtility.GetFieldOffset(field);
        }

        public static void CopyBlock(void* destination, void* source, int byteCount)
        {
            UnsafeUtility.MemCpy(destination, source, byteCount);
        }

        public static void CopyBlock(void* destination, void* source, long byteCount)
        {
            UnsafeUtility.MemCpy(destination, source, byteCount);
        }

        public static void CopyStructureToPtr<T>(ref T input, void* ptr) where T : struct
        {
            UnsafeUtility.CopyStructureToPtr(ref input, ptr);
        }
    }
}