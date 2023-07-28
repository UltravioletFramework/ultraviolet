using System;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    static partial class FreeTypeNative
    {
        public const Int32 FT_FACE_FLAG_SCALABLE = 1 << 0;
        public const Int32 FT_FACE_FLAG_FIXED_SIZES = 1 << 1;
        public const Int32 FT_FACE_FLAG_FIXED_WIDTH = 1 << 2;
        public const Int32 FT_FACE_FLAG_SFNT = 1 << 3;
        public const Int32 FT_FACE_FLAG_HORIZONTAL = 1 << 4;
        public const Int32 FT_FACE_FLAG_VERTICAL = 1 << 5;
        public const Int32 FT_FACE_FLAG_KERNING = 1 << 6;
        public const Int32 FT_FACE_FLAG_FAST_GLYPHS = 1 << 7;
        public const Int32 FT_FACE_FLAG_MULTIPLE_MASTERS = 1 << 8;
        public const Int32 FT_FACE_FLAG_GLYPH_NAMES = 1 << 9;
        public const Int32 FT_FACE_FLAG_EXTERNAL_STREAM = 1 << 10;
        public const Int32 FT_FACE_FLAG_HINTER = 1 << 11;
        public const Int32 FT_FACE_FLAG_CID_KEYED = 1 << 12;
        public const Int32 FT_FACE_FLAG_TRICKY = 1 << 13;
        public const Int32 FT_FACE_FLAG_COLOR = 1 << 14;
        public const Int32 FT_FACE_FLAG_VARIATION = 1 << 15;
    }
#pragma warning restore 1591
}
