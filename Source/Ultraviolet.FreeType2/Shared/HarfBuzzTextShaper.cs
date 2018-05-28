using System;
using System.Runtime.InteropServices;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.FreeType2.Native;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
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
        public override void Append(String str)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            unsafe
            {
                fixed (byte* pbytes = System.Text.Encoding.Unicode.GetBytes(str))
                {
                    hb_buffer_add_utf16(native, (IntPtr)pbytes, str.Length, 0, str.Length);
                    length = (Int32)hb_buffer_get_length(native);
                }
            }


            /*
            var pstr = Marshal.StringToHGlobalUni(str);
            try
            {
                hb_buffer_add_utf16(native, pstr, str.Length, 0, str.Length);
                length = (Int32)hb_buffer_get_length(native);
            }
            finally
            {
                if (pstr != IntPtr.Zero)
                    Marshal.FreeHGlobal(pstr);
            }
            */
        }

        /// <inheritdoc/>
        public override void Append(String str, Int32 start, Int32 length)
        {
            Append(new StringSegment(str, start, length));
        }

        /// <inheritdoc/>
        public override void Append(StringBuilder str)
        {
            Append(new StringSegment(str));
        }

        /// <inheritdoc/>
        public override void Append(StringBuilder str, Int32 start, Int32 length)
        {
            Append(new StringSegment(str, start, length));
        }

        /// <inheritdoc/>
        public override void Append(StringSegment str)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            unsafe
            {
                const Int32 BufferSize = 64;

                var buffer = stackalloc Char[BufferSize];
                for (int i = 0; i < str.Length; i += BufferSize)
                {
                    var remaining = Math.Min(BufferSize, str.Length - i);
                    for (int j = 0; j < remaining; j++)
                        buffer[j] = str[i + j];

                    hb_buffer_add_utf16(native, (IntPtr)buffer, remaining, 0, remaining);
                    length = (Int32)hb_buffer_get_length(native);
                }
            }
        }

        /// <inheritdoc/>
        public override ShapedString CreateShapedString(UltravioletFontFace fontFace)
        {
            Contract.Require(fontFace, nameof(fontFace));
            Contract.EnsureNotDisposed(this, Disposed);

            var ftFontFace = fontFace as FreeTypeFontFace;
            if (ftFontFace == null)
                throw new NotSupportedException(FreeTypeStrings.TextShaperRequiresFreeTypeFont);

            if (lastUsedFont != ftFontFace && lastUsedFontNative != IntPtr.Zero)
                hb_font_destroy(lastUsedFontNative);

            var fontNative = hb_ft_font_create(ftFontFace.NativePointer, IntPtr.Zero);
            lastUsedFont = ftFontFace;
            lastUsedFontNative = fontNative;

            GuessUnicodeProperties();
            hb_shape(fontNative, native, IntPtr.Zero, 0);

            unsafe
            {
                var glyphCount = 0u;
                var glyphInfo =
                    (hb_glyph_info_t*)hb_buffer_get_glyph_infos(native, (IntPtr)(&glyphCount));
                var glyphPosition =
                    (hb_glyph_position_t*)hb_buffer_get_glyph_positions(native, IntPtr.Zero);

                var chars = new ShapedChar[glyphCount];

                for (var i = 0; i < glyphCount; i++)
                {
                    var cGlyphIndex = (Int32)glyphInfo->codepoint;
                    var cOffsetX = (Int16)(glyphPosition->x_offset / 64);
                    var cOffsetY = (Int16)(glyphPosition->y_offset / 64);
                    var cAdvanceX = (Int16)(glyphPosition->x_advance / 64);
                    var cAdvanceY = (Int16)(glyphPosition->y_advance / 64);
                    chars[i] = new ShapedChar(cGlyphIndex, cOffsetX, cOffsetY, cAdvanceX, cAdvanceY);

                    glyphInfo++;
                    glyphPosition++;
                }

                return new ShapedString(ftFontFace, GetLanguage(), GetScript(), GetDirection(), chars);
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

        // The native HarfBuzz buffer.
        private IntPtr native;
        private Int32 length;

        // The native HarfBuzz font.
        private FreeTypeFontFace lastUsedFont;
        private IntPtr lastUsedFontNative;
    }
}
