using System;
using System.Runtime.InteropServices;

namespace TwistedLogik.Ultraviolet.BASS.Native
{
    internal delegate void FileCloseProc(IntPtr user);
    internal delegate UInt64 FileLenProc(IntPtr user);
    internal delegate UInt32 FileReadProc(IntPtr buffer, UInt32 length, IntPtr user);
    internal delegate Boolean FileSeekProc(UInt64 offset, IntPtr user);

    [StructLayout(LayoutKind.Sequential)]
    internal struct BASS_FILEPROCS
    {
        public BASS_FILEPROCS(FileCloseProc close, FileLenProc length, FileReadProc read, FileSeekProc seek)
        {
            this.close  = Marshal.GetFunctionPointerForDelegate(close);
            this.length = Marshal.GetFunctionPointerForDelegate(length);
            this.read   = Marshal.GetFunctionPointerForDelegate(read);
            this.seek   = Marshal.GetFunctionPointerForDelegate(seek);
        }

        public IntPtr close;
        public IntPtr length;
        public IntPtr read;
        public IntPtr seek;
    }
}