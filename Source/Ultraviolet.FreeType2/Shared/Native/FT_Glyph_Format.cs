using System;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    public enum FT_Glyph_Format : uint
    {
        FT_GLYPH_FORMAT_NONE = 0,

        FT_GLYPH_FORMAT_COMPOSITE = ((UInt32)'c' << 24) | ((UInt32)'o' << 16) | ((UInt32)'m' << 8) | 'p',
        FT_GLYPH_FORMAT_BITMAP = ((UInt32)'b' << 24) | ((UInt32)'i' << 16) | ((UInt32)'t' << 8) | 's',
        FT_GLYPH_FORMAT_OUTLINE = ((UInt32)'o' << 24) | ((UInt32)'u' << 16) | ((UInt32)'t' << 8) | 'l',
        FT_GLYPH_FORMAT_PLOTTER = ((UInt32)'p' << 24) | ((UInt32)'l' << 16) | ((UInt32)'o' << 8) | 't',
    }
#pragma warning restore 1591
}
