using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    /// <summary>
    /// Contains BASS helper methods.
    /// </summary>
    unsafe static partial class BASSNative
    {
        public static UInt32 BASS_StreamCreate(UInt32 freq, UInt32 chans, UInt32 flags, StreamProc proc, IntPtr user) => 
            BASS_StreamCreate(freq, chans, flags, Marshal.GetFunctionPointerForDelegate(proc), user);

        public static UInt32 BASS_StreamCreateFile(String file, UInt32 flags) => 
            BASS_StreamCreateFile(false, file, 0, 0, flags);

        public static UInt32 BASS_SampleLoad(String file, UInt32 max, UInt32 flags)
        {
            var pFile = IntPtr.Zero;
            try
            {
                pFile = Marshal.StringToHGlobalAnsi(file);
                return BASS_SampleLoad(false, pFile, 0, 0, max, flags);
            }
            finally
            {
                if (pFile != IntPtr.Zero)
                    Marshal.FreeHGlobal(pFile);
            }
        }

        public static UInt32 BASS_SampleLoad(Byte[] data, UInt64 offset, UInt32 length, UInt32 max, UInt32 flags)
        {
            fixed (Byte* pData = data)
            {
                return BASS_SampleLoad(true, (IntPtr)pData, offset, length, max, flags);
            }
        }

        public static Boolean BASS_SampleGetData(UInt32 handle, Byte[] buffer)
        {
            fixed (Byte* pBuffer = buffer)
            {
                return BASS_SampleGetData(handle, (IntPtr)pBuffer);
            }
        }
    }
}
