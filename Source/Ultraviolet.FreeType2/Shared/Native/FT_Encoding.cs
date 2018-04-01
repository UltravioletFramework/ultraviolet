using System;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    public enum FT_Encoding : uint
    {
        FT_ENCODING_NONE = 0,

        FT_ENCODING_MS_SYMBOL = ((UInt32)'s' << 24) | ((UInt32)'y' << 16) | ((UInt32)'m' << 8) | 'b',
        FT_ENCODING_UNICODE = ((UInt32)'u' << 24) | ((UInt32)'n' << 16) | ((UInt32)'i' << 8) | 'c',

        FT_ENCODING_SJIS = ((UInt32)'s' << 24) | ((UInt32)'j' << 16) | ((UInt32)'i' << 8) | 's',
        FT_ENCODING_PRC = ((UInt32)'g' << 24) | ((UInt32)'b' << 16) | ((UInt32)' ' << 8) | ' ',
        FT_ENCODING_BIG5 = ((UInt32)'b' << 24) | ((UInt32)'i' << 16) | ((UInt32)'g' << 8) | '5',
        FT_ENCODING_WANSUNG = ((UInt32)'w' << 24) | ((UInt32)'a' << 16) | ((UInt32)'n' << 8) | 's',
        FT_ENCODING_JOHAB = ((UInt32)'j' << 24) | ((UInt32)'o' << 16) | ((UInt32)'h' << 8) | 'a',

        FT_ENCODING_GB2312 = FT_ENCODING_PRC,
        FT_ENCODING_MS_SJIS = FT_ENCODING_SJIS,
        FT_ENCODING_MS_GB2312 = FT_ENCODING_PRC,
        FT_ENCODING_MS_BIG5 = FT_ENCODING_BIG5,
        FT_ENCODING_MS_WANSUNG = FT_ENCODING_WANSUNG,
        FT_ENCODING_MS_JOHAB = FT_ENCODING_JOHAB,

        FT_ENCODING_ADOBE_STANDARD = ((UInt32)'A' << 24) | ((UInt32)'D' << 16) | ((UInt32)'O' << 8) | 'B',
        FT_ENCODING_ADOBE_EXPERT = ((UInt32)'A' << 24) | ((UInt32)'D' << 16) | ((UInt32)'B' << 8) | 'E',
        FT_ENCODING_ADOBE_CUSTOM = ((UInt32)'A' << 24) | ((UInt32)'D' << 16) | ((UInt32)'B' << 8) | 'C',
        FT_ENCODING_ADOBE_LATIN_1 = ((UInt32)'l' << 24) | ((UInt32)'a' << 16) | ((UInt32)'t' << 8) | '1',

        FT_ENCODING_OLD_LATIN_2 = ((UInt32)'l' << 24) | ((UInt32)'a' << 16) | ((UInt32)'t' << 8) | '2',

        FT_ENCODING_APPLE_ROMAN = ((UInt32)'a' << 24) | ((UInt32)'r' << 16) | ((UInt32)'m' << 8) | 'n',
    }
#pragma warning restore 1591
}
