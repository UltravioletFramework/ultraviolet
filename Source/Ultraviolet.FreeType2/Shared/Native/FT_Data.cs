using System.Runtime.InteropServices;
using Ultraviolet.Core;

using FT_Byte = System.Byte;
using FT_Int = System.Int32;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Data
    {
        public FT_Byte* data;
        public FT_Int length;
    }
#pragma warning restore 1591
}
