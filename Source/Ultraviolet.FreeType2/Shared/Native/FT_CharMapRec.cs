using System.Runtime.InteropServices;
using Ultraviolet.Core;

using FT_UShort = System.UInt16;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_CharMapRec
    {
        public FT_FaceRec* face;
        public FT_Encoding encoding;
        public FT_UShort platform_id;
        public FT_UShort encoding_id;
    }
#pragma warning restore 1591
}
