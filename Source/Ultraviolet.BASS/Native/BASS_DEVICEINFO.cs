using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct BASS_DEVICEINFO
    {
        // NOTE: These are defined as wchar_t in Windows Store apps, but Ultraviolet
        // doesn't currently support those, so who cares?
        public Char* name;
        public Char* driver;
        public UInt32 flags;
        public MARSHALLED_BASS_DEVICEINFO ToMarshalledStruct() { return new MARSHALLED_BASS_DEVICEINFO(this); }
    }

    public unsafe struct MARSHALLED_BASS_DEVICEINFO
    {
        public MARSHALLED_BASS_DEVICEINFO(BASS_DEVICEINFO deviceinfo)
        {
            this.name = Marshal.PtrToStringAnsi((IntPtr)deviceinfo.name);
            this.driver = Marshal.PtrToStringAnsi((IntPtr)deviceinfo.driver);
            this.flags = deviceinfo.flags;
        }
        public String name;
        public String driver;
        public UInt32 flags;
    }
#pragma warning restore 1591
}
