using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    public delegate void FileCloseProc(IntPtr user);
    public delegate UInt64 FileLenProc(IntPtr user);
    public delegate UInt32 FileReadProc(IntPtr buffer, UInt32 length, IntPtr user);
    public delegate Boolean FileSeekProc(UInt64 offset, IntPtr user);
    [StructLayout(LayoutKind.Sequential)]
    public struct BASS_FILEPROCS
    {
        public BASS_FILEPROCS(FileCloseProc close, FileLenProc length, FileReadProc read, FileSeekProc seek)
        {
            this.close = Marshal.GetFunctionPointerForDelegate(close);
            this.length = Marshal.GetFunctionPointerForDelegate(length);
            this.read = Marshal.GetFunctionPointerForDelegate(read);
            this.seek = Marshal.GetFunctionPointerForDelegate(seek);
        }
        public IntPtr close;
        public IntPtr length;
        public IntPtr read;
        public IntPtr seek;
    }
#pragma warning restore 1591
}