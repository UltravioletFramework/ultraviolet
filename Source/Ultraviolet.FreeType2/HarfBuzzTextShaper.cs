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
            : base(uv)
        {
            this.native = hb_buffer_create();
            this.rawstr = new HarfBuzzNativeStringBuffer(uv, capacity);

            if (capacity > 0)
                hb_buffer_pre_allocate(native, (UInt32)capacity);

            this.direction = TextDirection.Invalid;
            this.script = TextScript.Invalid;
            this.language = null;

            SetNativeUnicodeProperties();
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            rawstr.Clear();

            hb_buffer_clear_contents(native);
            SetNativeUnicodeProperties();

            populatedVersion = 0;
        }

        /// <inheritdoc/>
        public override void SetUnicodeProperties(TextDirection direction, TextScript script, String language)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            this.direction = direction;
            this.script = script;
            this.language = language;
        }

        /// <inheritdoc/>
        public override void SetDirection(TextDirection direction)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            this.direction = direction;
        }

        /// <inheritdoc/>
        public override void SetScript(TextScript script)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            this.script = script;
        }

        /// <inheritdoc/>
        public override void SetLanguage(String language)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            this.language = language;
        }

        /// <inheritdoc/>
        public override void GetUnicodeProperties(out TextDirection direction, out TextScript script, out String language)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            direction = this.direction;
            script = this.script;
            language = this.language;
        }

        /// <inheritdoc/>
        public override TextDirection GetDirection()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.direction;
        }

        /// <inheritdoc/>
        public override TextScript GetScript()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.script;
        }

        /// <inheritdoc/>
        public override String GetLanguage()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.language;
        }

        /// <inheritdoc/>
        public override TextShaper Append(Char c)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            rawstr.Append(c);

            return this;
        }

        /// <inheritdoc/>
        public override TextShaper Append(String str)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            rawstr.Append(str);

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

            var itemOffset = (UInt32)rawstr.Length;
            var itemLength = str.Length;

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
                }
            }

            return this;
        }

        /// <inheritdoc/>
        public override void AppendTo(ShapedStringBuilder builder, UltravioletFontFace fontFace, Int32 sourceIndexOffset = 0) =>
            AppendTo(builder, fontFace, 0, rawstr.Length, sourceIndexOffset);

        /// <inheritdoc/>
        public override void AppendTo(ShapedStringBuilder builder, UltravioletFontFace fontFace, Int32 start, Int32 length, Int32 sourceIndexOffset = 0)
        {
            Contract.Require(builder, nameof(builder));
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
                    if (cluster >= start)
                    {
                        if (cluster >= end)
                            break;

                        CreateShapedChar(glyphInfo, glyphPosition, sourceIndexOffset + cluster, out var sc);
                        builder.Append(sc);
                    }
                    glyphInfo++;
                    glyphPosition++;
                }
            }
        }

        /// <inheritdoc/>
        public override ShapedString CreateShapedString(UltravioletFontFace fontFace, Int32 sourceIndexOffset = 0) =>
            CreateShapedString(fontFace, 0, rawstr.Length, sourceIndexOffset);

        /// <inheritdoc/>
        public override ShapedString CreateShapedString(UltravioletFontFace fontFace, Int32 start, Int32 length, Int32 sourceIndexOffset = 0)
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
                var charsCount = 0;
                for (var i = 0; i < glyphCount; i++)
                {
                    var cluster = (Int32)glyphInfo->cluster;
                    if (cluster >= start)
                    {
                        if (cluster >= end)
                            break;

                        switch (rawstr[cluster])
                        {
                            case '\n':
                                chars[i] = new ShapedChar('\n', sourceIndexOffset + cluster, Int16.MaxValue, Int16.MaxValue, Int16.MaxValue);
                                break;

                            case '\t':
                                chars[i] = new ShapedChar('\t', sourceIndexOffset + cluster, Int16.MaxValue, Int16.MaxValue, Int16.MaxValue);
                                break;

                            default:
                                CreateShapedChar(glyphInfo, glyphPosition, sourceIndexOffset + cluster, out chars[i]);
                                break;
                        }
                        charsCount++;
                    }
                    glyphInfo++;
                    glyphPosition++;
                }

                return new ShapedString(fontFace, GetLanguage(), GetScript(), GetDirection(), chars, 0, charsCount);
            }
        }

        /// <inheritdoc/>
        public override Int32 RawLength => rawstr.Length;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                rawstr.Dispose();
            }
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
        private unsafe void CreateShapedChar(hb_glyph_info_t* info, hb_glyph_position_t* position, Int32 cluster, out ShapedChar result)
        {
            var cGlyphIndex = (Int32)info->codepoint;
            var cOffsetX = (Int16)Math.Round(position->x_offset / 64f, MidpointRounding.AwayFromZero);
            var cOffsetY = (Int16)Math.Round(position->y_offset / 64f, MidpointRounding.AwayFromZero);
            var cAdvance = (Int16)Math.Round((position->x_advance == 0 ? position->y_advance : position->x_advance) / 64f, MidpointRounding.AwayFromZero);
            result = new ShapedChar(cGlyphIndex, cluster, cOffsetX, cOffsetY, cAdvance);
        }

        /// <summary>
        /// Performs shaping on the native buffer using the specified font..
        /// </summary>
        private unsafe void Shape(UltravioletFontFace fontFace, out hb_glyph_info_t* infos, out hb_glyph_position_t* positions, out UInt32 count)
        {
            ValidateUnicodeProperties();

            var ftFontFace = fontFace as FreeTypeFontFace;
            if (ftFontFace == null)
                throw new NotSupportedException(FreeTypeStrings.TextShaperRequiresFreeTypeFont);

            PopulateNativeBuffer();

            if (lastUsedFont != ftFontFace && lastUsedFontNative != IntPtr.Zero)
            {
                hb_font_destroy(lastUsedFontNative);
                lastUsedFont = null;
                lastUsedFontNative = IntPtr.Zero;
            }

            var fontNative = (lastUsedFontNative == IntPtr.Zero) ? hb_ft_font_create(ftFontFace.NativePointer, IntPtr.Zero) : lastUsedFontNative;
            var fontLoadFlags = ftFontFace.IsStroked ? FT_LOAD_NO_BITMAP : FT_LOAD_COLOR;
            hb_ft_font_set_load_flags(fontNative, fontLoadFlags);
            lastUsedFont = ftFontFace;
            lastUsedFontNative = fontNative;

            hb_shape(fontNative, native, IntPtr.Zero, 0);

            var glyphCount = 0u;
            infos = (hb_glyph_info_t*)hb_buffer_get_glyph_infos(native, (IntPtr)(&glyphCount));
            positions = (hb_glyph_position_t*)hb_buffer_get_glyph_positions(native, IntPtr.Zero);
            count = glyphCount;
        }

        /// <summary>
        /// Ensures that the buffer has valid Unicode properties set.
        /// </summary>
        private void ValidateUnicodeProperties()
        {
            if (direction == TextDirection.Invalid)
                throw new InvalidOperationException(FreeTypeStrings.InvalidBufferDirection);

            if (script == TextScript.Invalid)
                throw new InvalidOperationException(FreeTypeStrings.InvalidBufferScript);

            if (language == null)
                throw new InvalidOperationException(FreeTypeStrings.InvalidBufferLanguage);
        }

        /// <summary>
        /// Populates the native buffer with data.
        /// </summary>
        private void PopulateNativeBuffer()
        {
            if (rawstr.Version == populatedVersion)
                return;

            hb_buffer_clear_contents(native);
            SetNativeUnicodeProperties();
            hb_buffer_add_utf16(native, rawstr.Native, rawstr.Length, 0, rawstr.Length);

            populatedVersion = rawstr.Version;
        }

        /// <summary>
        /// Applies the selected direction, script, and language to the native buffer.
        /// </summary>
        private void SetNativeUnicodeProperties()
        {
            SetNativeDirection();
            SetNativeScript();
            SetNativeLanguage();
        }

        /// <summary>
        /// Applies the selected direction to the native buffer.
        /// </summary>
        private void SetNativeDirection()
        {
            var hbdir = TextDirectionUtil.UltravioletToHarfBuzz(direction);
            hb_buffer_set_direction(native, hbdir);
        }

        /// <summary>
        /// Applies the selected script to the native buffer.
        /// </summary>
        private void SetNativeScript()
        {
            var hbscript = TextScriptUtil.UltravioletToHarfBuzz(script);
            hb_buffer_set_script(native, hbscript);
        }

        /// <summary>
        /// Applies the selected language to the native buffer.
        /// </summary>
        private void SetNativeLanguage()
        {
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

        // The native HarfBuzz buffer.
        private IntPtr native;
        private TextDirection direction;
        private TextScript script;
        private String language;

        // The native HarfBuzz font.
        private FreeTypeFontFace lastUsedFont;
        private IntPtr lastUsedFontNative;

        // The string builder which contains the raw text.
        private readonly HarfBuzzNativeStringBuffer rawstr;
        private Int64 populatedVersion;
    }
}
