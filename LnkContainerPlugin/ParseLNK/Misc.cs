﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace ParseLnk
{
  static class Misc
  {
    public static T ReadStruct<T>(this Stream stream) where T : struct
    {
      var sz = Marshal.SizeOf(typeof(T));
      var buffer = new byte[sz];

      stream.Read(buffer, 0, sz);

      return buffer.ReadStruct<T>(0, (uint)sz);
    }

    public static GCHandle GetGCHandle(this byte[] buffer)
    {
      return GCHandle.Alloc(buffer, GCHandleType.Pinned);
    }

    public static T ReadStruct<T>(this byte[] buffer) where T : struct
    {
      return buffer.ReadStruct<T>(0);
    }

    public static T ReadStruct<T>(this byte[] buffer, uint offset, uint size = 0) where T : struct
    {
      if (size == 0)
        size = (uint)Marshal.SizeOf<T>();

      if (offset + size > buffer.Length)
        throw new IndexOutOfRangeException("Offset and size are outside buffer");

      var pinnedBuffer = buffer.GetGCHandle();
      var structure = pinnedBuffer.ReadStruct<T>(offset);

      pinnedBuffer.Free();

      return structure;
    }

    public static T ReadStruct<T>(this GCHandle pinnedBuffer, uint offset = 0) where T : struct
    {
      var ptr = IntPtr.Add(pinnedBuffer.AddrOfPinnedObject(), (int)offset);

      return Marshal.PtrToStructure<T>(ptr);
    }

    public static DateTime ToDateTime(this FILETIME time)
    {
      var high = (ulong)time.dwHighDateTime;
      var low = (uint)time.dwLowDateTime;
      var fileTime = (long)((high << 32) + low);
      try
      {
        return DateTime.FromFileTimeUtc(fileTime);
      }
      catch
      {
        return DateTime.FromFileTimeUtc(0xFFFFFFFF);
      }
    }

    public static T CastToType<T>(this object input)
    {
      return (T)input;
    }
  }
}
