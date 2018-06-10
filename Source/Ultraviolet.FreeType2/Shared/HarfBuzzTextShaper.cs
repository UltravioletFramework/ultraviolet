using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.FreeType2.Native;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using static Ultraviolet.FreeType2.Native.FreeTypeNative;
using static Ultraviolet.FreeType2.Native.HarfBuzzNative;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents an implementation of the <see cref="TextShaper"/> class based on the HarfBuzz library.
    /// </summary>
    public sealed class HarfBuzzTextShaper : TextShaper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HarfBuzzTextShaper"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="capacity">The initial capacity of the text builder.</param>
        public HarfBuzzTextShaper(UltravioletContext uv, Int32 capacity = 0)
            : base(uv, capacity)
        {
            native = hb_buffer_create();

            if (capacity > 0)
                hb_buffer_pre_allocate(native, (UInt32)capacity);
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            rawstr.Clear();
            hb_buffer_clear_contents(native);
            this.length = 0;
        }

        /// <inheritdoc/>
        public override void GuessUnicodeProperties()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            hb_buffer_guess_segment_properties(native);
        }

        /// <inheritdoc/>
        public override void SetDirection(TextDirection direction)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var hbdir = TextDirectionUtil.UltravioletToHarfBuzz(direction);
            hb_buffer_set_direction(native, hbdir);
        }

        /// <inheritdoc/>
        public override void SetScript(TextScript script)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var hbscript = TextScriptUtil.UltravioletToHarfBuzz(script);
            hb_buffer_set_script(native, hbscript);
        }

        /// <inheritdoc/>
        public override void SetLanguage(String language)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var langstr = Marshal.StringToHGlobalAnsi(language);
            try
            {
                var langptr = hb_language_from_string(langstr, -1);
                hb_buffer_set_language(native, langptr);
            }
            finally
            {
                if (langstr != IntPtr.Zero)
                    Marshal.FreeHGlobal(langstr);
            }
        }

        /// <inheritdoc/>
        public override TextDirection GetDirection()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var hbdir = hb_buffer_get_direction(native);
            var uvdir = TextDirectionUtil.HarfBuzzToUltraviolet(hbdir);
            return uvdir;
        }

        /// <inheritdoc/>
        public override TextScript GetScript()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var hbscript = hb_buffer_get_script(native);
            var uvscript = TextScriptUtil.HarfBuzzToUltraviolet(hbscript);
            return uvscript;
        }

        /// <inheritdoc/>
        public override String GetLanguage()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var lang = hb_buffer_get_language(native);
            var langname = hb_language_to_string(lang);
            return Marshal.PtrToStringAnsi(langname);
        }

        /// <inheritdoc/>
        public override TextShaper Append(String str)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var pstr = Marshal.StringToHGlobalUni(str);
            try
            {
                rawstr.Append(str);
                hb_buffer_add_utf16(native, pstr, str.Length, 0, str.Length);
                length = (Int32)hb_buffer_get_length(native);
            }
            finally
            {
                if (pstr != IntPtr.Zero)
                    Marshal.FreeHGlobal(pstr);
            }

            return this;
        }

        /// <inheritdoc/>
        public override TextShaper Append(String str, Int32 start, Int32 length)
        {
            return Append(new StringSegment(str, start, length));
        }

        /// <inheritdoc/>
        public override TextShaper Append(StringBuilder str)
        {
            return Append(new StringSegment(str));
        }

        /// <inheritdoc/>
        public override TextShaper Append(StringBuilder str, Int32 start, Int32 length)
        {
            return Append(new StringSegment(str, start, length));
        }

        /// <inheritdoc/>
        public override TextShaper Append(StringSegment str)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (rawstr.Capacity < rawstr.Length + str.Length)
                rawstr.Capacity = rawstr.Length + str.Length;

            unsafe
            {
                const Int32 BufferSize = 64;

                var buffer = stackalloc Char[BufferSize];
                for (int i = 0; i < str.Length; i += BufferSize)
                {
                    var remaining = Math.Min(BufferSize, str.Length - i);
                    for (int j = 0; j < remaining; j++)
                    {
                        var c = str[i + j];
                        buffer[j] = c;
                        rawstr.Append(c);
                    }

                    hb_buffer_add_utf16(native, (IntPtr)buffer, remaining, 0, remaining);
                    length = (Int32)hb_buffer_get_length(native);
                }
            }

            return this;
        }

        /// <inheritdoc/>
        public override void AppendTo(ShapedStringBuilder builder, UltravioletFontFace fontFace) =>
            AppendTo(builder, fontFace, 0, rawstr.Length);

        /// <inheritdoc/>
        public override void AppendTo(ShapedStringBuilder builder, UltravioletFontFace fontFace, Int32 start, Int32 length)
        {
            Contract.Require(fontFace, nameof(fontFace));
            Contract.EnsureRange(start >= 0 && start < rawstr.Length, nameof(start));
            Contract.EnsureRange(length >= 0 && start + length <= rawstr.Length, nameof(length));
            Contract.EnsureNotDisposed(this, Disposed);

            unsafe
            {
                Shape(fontFace, out var glyphInfo, out var glyphPosition, out var glyphCount);

                var end = start + length;
                for (var i = 0; i < glyphCount; i++)
                {
                    var cluster = (Int32)glyphInfo->cluster;
                    if (cluster < start)
                        continue;

                    if (cluster >= end)
                        break;

                    CreateShapedChar(glyphInfo, glyphPosition, out var sc);
                    builder.Append(sc);

                    glyphInfo++;
                    glyphPosition++;
                }
            }
        }

        /// <inheritdoc/>
        public override ShapedString CreateShapedString(UltravioletFontFace fontFace) =>
            CreateShapedString(fontFace, 0, rawstr.Length);

        /// <inheritdoc/>
        public override ShapedString CreateShapedString(UltravioletFontFace fontFace, Int32 start, Int32 length)
        {
            Contract.Require(fontFace, nameof(fontFace));
            Contract.EnsureRange(start >= 0 && start < rawstr.Length, nameof(start));
            Contract.EnsureRange(length >= 0 && start + length <= rawstr.Length, nameof(length));
            Contract.EnsureNotDisposed(this, Disposed);

            unsafe
            {
                Shape(fontFace, out var glyphInfo, out var glyphPosition, out var glyphCount);

                var end = start + length;
                var chars = new ShapedChar[glyphCount];
                for (var i = 0; i < glyphCount; i++)
                {
                    var cluster = (Int32)glyphInfo->cluster;
                    if (cluster < start)
                        continue;

                    if (cluster >= end)
                        break;

                    switch (rawstr[cluster])
                    {
                        case '\n':
                            chars[i] = ShapedChar.Newline;
                            break;

                        case '\t':
                            chars[i] = ShapedChar.Tab;
                            break;

                        default:
                            CreateShapedChar(glyphInfo, glyphPosition, out chars[i]);
                            break;
                    }
                    glyphInfo++;
                    glyphPosition++;
                }

                return new ShapedString(fontFace, GetLanguage(), GetScript(), GetDirection(), chars);
            }
        }

        /// <inheritdoc/>
        public override Int32 Length
        {
            get => length;
            set
            {
                Contract.EnsureRange(value >= 0 && value <= length, nameof(value));
                Contract.EnsureNotDisposed(this, Disposed);

                if (!hb_buffer_set_length(native, (UInt32)value))
                    throw new OutOfMemoryException();

                length = value;
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (native != IntPtr.Zero)
            {
                hb_buffer_destroy(native);
                native = IntPtr.Zero;
            }
            if (lastUsedFontNative != IntPtr.Zero)
            {
                hb_font_destroy(lastUsedFontNative);
                lastUsedFontNative = IntPtr.Zero;
                lastUsedFont = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates a <see cref="ShapedChar"/> instance from the specified shaping information.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void CreateShapedChar(hb_glyph_info_t* info, hb_glyph_position_t* position, out ShapedChar result)
        {
            var cGlyphIndex = (Int32)info->codepoint;
            var cOffsetX = (Int16)Math.Round(position->x_offset / 64f, MidpointRounding.AwayFromZero);
            var cOffsetY = (Int16)Math.Round(position->y_offset / 64f, MidpointRounding.AwayFromZero);
            var cAdvance = (Int16)Math.Round((position->x_advance == 0 ? position->y_advance : position->x_advance) / 64f, MidpointRounding.AwayFromZero);
            result = new ShapedChar(cGlyphIndex, cOffsetX, cOffsetY, cAdvance);
        }

        /// <summary>
        /// Performs shaping on the native buffer using the specified font..
        /// </summary>
        private unsafe void Shape(UltravioletFontFace fontFace, out hb_glyph_info_t* infos, out hb_glyph_position_t* positions, out UInt32 count)
        {
            var ftFontFace = fontFace as FreeTypeFontFace;
            if (ftFontFace == null)
                throw new NotSupportedException(FreeTypeStrings.TextShaperRequiresFreeTypeFont);

            if (lastUsedFont != ftFontFace && lastUsedFontNative != IntPtr.Zero)
                hb_font_destroy(lastUsedFontNative);

            var fontNative = hb_ft_font_create(ftFontFace.NativePointer, IntPtr.Zero);
            var fontLoadFlags = ftFontFace.IsStroked ? FT_LOAD_NO_BITMAP : FT_LOAD_COLOR;
            hb_ft_font_set_load_flags(fontNative, fontLoadFlags);
            lastUsedFont = ftFontFace;
            lastUsedFontNative = fontNative;

            GuessUnicodeProperties();
            hb_shape(fontNative, native, IntPtr.Zero, 0);

            var glyphCount = 0u;
            infos = (hb_glyph_info_t*)hb_buffer_get_glyph_infos(native, (IntPtr)(&glyphCount));
            positions = (hb_glyph_position_t*)hb_buffer_get_glyph_positions(native, IntPtr.Zero);
            count = glyphCount;
        }

        // The native HarfBuzz buffer.
        private IntPtr native;
        private Int32 length;

        // The native HarfBuzz font.
        private FreeTypeFontFace lastUsedFont;
        private IntPtr lastUsedFontNative;

        // The string builder which contains the raw text.
        private readonly StringBuilder rawstr = new StringBuilder();
    }
}
