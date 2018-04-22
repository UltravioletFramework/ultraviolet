using System;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    static partial class FreeTypeNative
    {
        public const Int32 FT_LOAD_DEFAULT = 0x0;
        public const Int32 FT_LOAD_NO_SCALE = (1 << 0);
        public const Int32 FT_LOAD_NO_HINTING = (1 << 1);
        public const Int32 FT_LOAD_RENDER = (1 << 2);
        public const Int32 FT_LOAD_NO_BITMAP = (1 << 3);
        public const Int32 FT_LOAD_VERTICAL_LAYOUT = (1 << 4);
        public const Int32 FT_LOAD_FORCE_AUTOHINT = (1 << 5);
        public const Int32 FT_LOAD_CROP_BITMAP = (1 << 6);
        public const Int32 FT_LOAD_PEDANTIC = (1 << 7);
        public const Int32 FT_LOAD_IGNORE_GLOBAL_ADVANCE_WIDTH = (1 << 9);
        public const Int32 FT_LOAD_NO_RECURSE = (1 << 10);
        public const Int32 FT_LOAD_IGNORE_TRANSFORM = (1 << 11);
        public const Int32 FT_LOAD_MONOCHROME = (1 << 12);
        public const Int32 FT_LOAD_LINEAR_DESIGN = (1 << 13);
        public const Int32 FT_LOAD_NO_AUTOHINT = (1 << 15);
        public const Int32 FT_LOAD_COLOR = (1 << 20);
        public const Int32 FT_LOAD_COMPUTE_METRICS = (1 << 21);
        public const Int32 FT_LOAD_BITMAP_METRICS_ONLY = (1 << 22);
        public const Int32 FT_LOAD_ADVANCE_ONLY = (1 << 8);
        public const Int32 FT_LOAD_SBITS_ONLY = (1 << 14);
    }
#pragma warning restore 1591
}
