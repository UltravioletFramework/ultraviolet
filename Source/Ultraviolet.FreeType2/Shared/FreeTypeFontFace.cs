using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.FreeType2.Native;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using static Ultraviolet.FreeType2.Native.FreeTypeNative;
using static Ultraviolet.FreeType2.Native.FT_Error;
using static Ultraviolet.FreeType2.Native.FT_Pixel_Mode;
using static Ultraviolet.FreeType2.Native.FT_Kerning_Mode;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents one of a FreeType font's font faces.
    /// </summary>
    public unsafe class FreeTypeFontFace : UltravioletFontFace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreeTypeFontFace"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="face">The FreeType2 face which this instance represents.</param>
        /// <param name="sizeInPoints">The size of the font face in points.</param>
        /// <param name="sizeInPixels">The size of the font face in pixels.</param>
        /// <param name="substitutionCharacter">The substitution character for this font face, 
        /// or <see langword="null"/> to use a default character.</param>
        internal FreeTypeFontFace(UltravioletContext uv, IntPtr face, Int32 sizeInPoints, Int32 sizeInPixels, Char? substitutionCharacter = null)
            : base(uv)
        {
            Contract.Require(face, nameof(face));

            this.face = face;
            this.facade = new FT_FaceRecFacade(face);

            this.IsColorFont = facade.HasColorFlag;
            this.HasKerningInfo = facade.HasKerningFlag;
            this.FamilyName = facade.MarshalFamilyName();
            this.StyleName = facade.MarshalStyleName();
            this.LineSpacing = facade.LineSpacing;

            if (GetGlyphInfo(' ', out var spaceGlyphInfo))
                this.SpaceWidth = spaceGlyphInfo.Width;

            this.SizeInPoints = sizeInPoints;
            this.SizeInPixels = SizeInPixels;
            this.TabWidth = SpaceWidth * 4;

            this.SubstitutionCharacter = substitutionCharacter ??
                facade.GetCharIfDefined('�') ?? facade.GetCharIfDefined('□') ?? '?';

            PopulateGlyph(SubstitutionCharacter);
        }

        /// <inheritdoc/>
        public override String ToString() => String.Format("{0} {1} {2}pt", FamilyName, StyleName, SizeInPoints);

        /// <inheritdoc/>
        public override void GetGlyphRenderInfo(Char c, out GlyphRenderInfo info)
        {
            if (GetGlyphInfo(c, out var ginfo))
            {
                info = new GlyphRenderInfo
                {
                    Texture = ginfo.Texture,
                    TextureRegion = ginfo.TextureRegion,
                    OffsetX = ginfo.OffsetX,
                    OffsetY = ginfo.OffsetY,
                    Advance = ginfo.Advance,
                };
            }
            else
            {
                info = default(GlyphRenderInfo);
            }
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(String text)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, 0, text.Length);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(String text, Int32 start, Int32 count)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, start, count);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(StringBuilder text)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, 0, text.Length);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(StringBuilder text, Int32 start, Int32 count)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, start, count);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(StringSegment text)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, 0, text.Length);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(StringSegment text, Int32 start, Int32 count)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, start, count);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(ref StringSource source, Int32 start, Int32 count)
        {
            if (count == 0)
                return Size2.Zero;

            Contract.EnsureRange(start >= 0 && start < source.Length, nameof(start));
            Contract.EnsureRange(count >= 0 && start + count <= source.Length, nameof(count));

            var cx = 0;
            var cy = 0;
            for (var i = 0; i < count; i++)
            {
                var character = source[start + i];
                switch (character)
                {
                    case '\r':
                        continue;

                    case '\n':
                        cx = 0;
                        cy = cy + LineSpacing;
                        continue;

                    case '\t':
                        cx = cx + TabWidth;
                        continue;
                }
                cx += MeasureGlyph(ref source, start + i).Width;
            }

            return new Size2(cx, cy + LineSpacing);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(String text, Int32 ix)
        {
            var source = new StringSource(text);
            return MeasureGlyph(ref source, ix);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(StringBuilder text, Int32 ix)
        {
            var source = new StringSource(text);
            return MeasureGlyph(ref source, ix);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(StringSegment text, Int32 ix)
        {
            var source = new StringSource(text);
            return MeasureGlyph(ref source, ix);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(ref StringSource source, Int32 ix)
        {
            if (!GetGlyphInfo(source[ix], out var cinfo))
                return Size2.Zero;
            
            return new Size2(cinfo.Advance, LineSpacing);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(Char c1, Char? c2 = null)
        {
            if (!GetGlyphInfo(c1, out var c1Info))
                return Size2.Zero;

            return new Size2(c1Info.Advance, LineSpacing);
        }

        /// <inheritdoc/>
        public override Size2 GetKerningInfo(Char c1, Char c2)
        {
            if (face == IntPtr.Zero || !HasKerningInfo)
                return Size2.Zero;

            var c1Index = facade.GetCharIndex(c1);
            if (c1Index == 0)
                return Size2.Zero;

            var c2Index = facade.GetCharIndex(c2);
            if (c2Index == 0)
                return Size2.Zero;

            if (Use64BitInterface)
            {
                var kerning = default(FT_Vector64);
                var err = FT_Get_Kerning(face, c1Index, c2Index, (UInt32)FT_KERNING_DEFAULT, (IntPtr)(&kerning));
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                var x = FreeTypeCalc.F26Dot6ToInt32(kerning.x);
                var y = FreeTypeCalc.F26Dot6ToInt32(kerning.y);
                return new Size2(x, y);
            }
            else
            {
                var kerning = default(FT_Vector32);
                var err = FT_Get_Kerning(face, c1Index, c2Index, (UInt32)FT_KERNING_DEFAULT, (IntPtr)(&kerning));
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                var x = FreeTypeCalc.F26Dot6ToInt32(kerning.x);
                var y = FreeTypeCalc.F26Dot6ToInt32(kerning.y);
                return new Size2(x, y);
            }
        }

        /// <inheritdoc/>
        public override Size2 GetKerningInfo(SpriteFontKerningPair pair)
        {
            return GetKerningInfo(pair.FirstCharacter, pair.SecondCharacter);
        }

        /// <summary>
        /// Ensures that the specified character's associated glyph has been added to the font's texture atlases.
        /// </summary>
        /// <param name="c">The character for which to ensure that a glyph has been populated.</param>
        public void PopulateGlyph(Char c)
        {
            GetGlyphInfo(c, out var info);
        }

        /// <summary>
        /// Gets the font's family name.
        /// </summary>
        public String FamilyName { get; }

        /// <summary>
        /// Gets the font's style name.
        /// </summary>
        public String StyleName { get; }

        /// <summary>
        /// Gets the font's size in points.
        /// </summary>
        public Int32 SizeInPoints { get; }

        /// <summary>
        /// Gets the font's size in pixels.
        /// </summary>
        public Int32 SizeInPixels { get; }

        /// <summary>
        /// Gets a value indicating whether this is a color font.
        /// </summary>
        public Boolean IsColorFont { get; }

        /// <summary>
        /// Gets a value indicating whether this font has kerning information.
        /// </summary>
        public Boolean HasKerningInfo { get; }

        /// <inheritdoc/>
        public override Int32 Characters => glyphInfoCache.Count;

        /// <inheritdoc/>
        public override Int32 SpaceWidth { get; }

        /// <inheritdoc/>
        public override Int32 TabWidth { get; }

        /// <inheritdoc/>
        public override Int32 LineSpacing { get; }

        /// <inheritdoc/>
        public override Char SubstitutionCharacter { get; }

        /// <summary>
        /// The width of the texture atlases used by FreeType2 font faces.
        /// </summary>
        public const Int32 AtlasWidth = 1024;

        /// <summary>
        /// The height of the texture atlases used by FreeType2 font faces.
        /// </summary>
        public const Int32 AtlasHeight = 1024;

        /// <summary>
        /// The spacing between cells on the atlases used by FreeType2 font faces.
        /// </summary>
        public const Int32 AtlasSpacing = 4;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                var err = FT_Done_Face(face);
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                face = IntPtr.Zero;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets the font face's metadata for the specified glyph.
        /// </summary>
        private Boolean GetGlyphInfo(Char c, out FreeTypeGlyphInfo info)
        {
            if (glyphInfoCache.TryGetValue(c, out var cached))
            {
                info = cached;
            }
            else
            {
                var index = facade.GetCharIndex(c);
                if (index == 0)
                {
                    if (c != SubstitutionCharacter)
                        return GetGlyphInfo(SubstitutionCharacter, out info);

                    info = default(FreeTypeGlyphInfo);
                    return false;
                }
                else
                {
                    LoadGlyphMetadata(index);
                    LoadGlyphTexture(c, out info);
                    glyphInfoCache[c] = info;
                }
            }
            return true;
        }

        /// <summary>
        /// Loads the metadata for the specified glyph into the face's glyph slot.
        /// </summary>
        private void LoadGlyphMetadata(UInt32 glyphIndex)
        {
            var flags = FT_LOAD_RENDER | FT_LOAD_COLOR;
            var err = FT_Load_Glyph(face, glyphIndex, flags);
            if (err != FT_Err_Ok)
                throw new FreeTypeException(err);
        }

        /// <summary>
        /// Loads the texture data for the specified glyph.
        /// </summary>
        private void LoadGlyphTexture(Char c, out FreeTypeGlyphInfo info)
        {
            var reservation = default(DynamicTextureAtlas.Reservation);
            var reservationFound = false;

            var bmp = facade.GlyphBitmap;
            var bmpWidth = (Int32)bmp.width;
            var bmpHeight = (Int32)bmp.rows;
            var bmpPitch = bmp.pitch;

            // If the glyph is not whitespace, we need to add it to one of our atlases.
            if (!Char.IsWhiteSpace(c))
            {
                // Attempt to reserve space on one of the font's existing atlases.
                foreach (var atlas in atlases)
                {
                    if (atlas.TryReserveCell(bmpWidth, bmpHeight, out reservation))
                    {
                        reservationFound = true;
                        break;
                    }
                }

                // Attempt to create a new atlas if we weren't able to make a reservation.
                if (!reservationFound)
                {
                    var atlas = DynamicTextureAtlas.Create(AtlasWidth, AtlasHeight, AtlasSpacing);
                    atlas.Surface.Clear(Color.Transparent);
                    atlases.Add(atlas);

                    if (!atlas.TryReserveCell(bmpWidth, bmpHeight, out reservation))
                        throw new InvalidOperationException(FreeTypeStrings.GlyphTooBigForAtlas.Format(c));
                }

                // Update the atlas surface.
                switch ((FT_Pixel_Mode)bmp.pixel_mode)
                {
                    case FT_PIXEL_MODE_MONO:
                        BlitGlyphBitmapMono(ref bmp, bmpWidth, bmpHeight, bmpPitch, ref reservation);
                        break;

                    case FT_PIXEL_MODE_GRAY:
                        BlitGlyphBitmapGray(ref bmp, bmpWidth, bmpHeight, bmpPitch, ref reservation);
                        break;

                    case FT_PIXEL_MODE_BGRA:
                        BlitGlyphBitmapBgra(ref bmp, bmpWidth, bmpHeight, bmpPitch, ref reservation);
                        break;

                    default:
                        throw new NotSupportedException(FreeTypeStrings.PixelFormatNotSupported);
                }
                reservation.Atlas.Invalidate();
            }

            // Calculate the glyph's metrics.
            var glyphWidth = facade.GlyphMetricWidth;
            var glyphHeight = facade.GlyphMetricHeight;
            var glyphOffsetX = facade.GlyphBitmapLeft;
            var glyphOffsetY = facade.Ascender - facade.GlyphBitmapTop;
            var glyphAdvance = facade.GlyphMetricHorizontalAdvance;
            if (c == '\t')
                glyphAdvance *= 4;

            info = new FreeTypeGlyphInfo
            {
                Character = c,
                Advance = glyphAdvance,
                Width = glyphWidth,
                Height = glyphHeight,
                OffsetX = glyphOffsetX,
                OffsetY = glyphOffsetY,
                Texture = reservation.Atlas,
                TextureRegion = reservation.Atlas == null ? Rectangle.Empty : reservation.Atlas.IsFlipped ?
                    new Rectangle(reservation.X, reservation.Atlas.Height - (reservation.Y + reservation.Height), reservation.Width, reservation.Height) :
                    new Rectangle(reservation.X, reservation.Y, reservation.Width, reservation.Height),
            };
        }

        /// <summary>
        /// Blits a mono glyph bitmap to the specified atlas' surface.
        /// </summary>
        private void BlitGlyphBitmapMono(ref FT_Bitmap bmp, Int32 bmpWidth, Int32 bmpHeight, Int32 bmpPitch, ref DynamicTextureAtlas.Reservation reservation)
        {
            for (int y = 0; y < bmpHeight; y++)
            {
                var atlas = reservation.Atlas;
                var pSrcY = atlas.IsFlipped ? (bmpHeight - 1) - y : y;
                var pSrc = (Byte*)bmp.buffer + (pSrcY * bmpPitch);
                var pDst = (Color*)atlas.Surface.Pixels + ((reservation.Y + y) * atlas.Width) + reservation.X;
                for (int x = 0; x < bmpWidth; x += 8)
                {
                    var bits = *pSrc++;

                    for (int b = 0; b < 8; b++)
                    {
                        var color = ((bits >> (7 - b)) & 1) == 0 ? Color.Transparent : Color.White;
                        *pDst++ = color;
                    }
                }
            }
        }

        /// <summary>
        /// Blits a grayscale glyph bitmap to the specified atlas' surface.
        /// </summary>
        private void BlitGlyphBitmapGray(ref FT_Bitmap bmp, Int32 bmpWidth, Int32 bmpHeight, Int32 bmpPitch, ref DynamicTextureAtlas.Reservation reservation)
        {
            for (int y = 0; y < bmpHeight; y++)
            {
                var atlas = reservation.Atlas;
                var pSrcY = atlas.IsFlipped ? (bmpHeight - 1) - y : y;
                var pSrc = (Byte*)bmp.buffer + (pSrcY * bmpPitch);
                var pDst = (Color*)atlas.Surface.Pixels + ((reservation.Y + y) * atlas.Width) + reservation.X;
                for (int x = 0; x < bmpWidth; x++)
                {
                    var value = *pSrc++;
                    var color = new Color(value, value, value, value);
                    *pDst++ = color;
                }
            }
        }

        /// <summary>
        /// Blits a BGRA color glyph bitmap to the specified atlas' surface.
        /// </summary>
        private void BlitGlyphBitmapBgra(ref FT_Bitmap bmp, Int32 bmpWidth, Int32 bmpHeight, Int32 bmpPitch, ref DynamicTextureAtlas.Reservation reservation)
        {
            for (int y = 0; y < bmpHeight; y++)
            {
                var atlas = reservation.Atlas;
                var pSrcY = atlas.IsFlipped ? (bmpHeight - 1) - y : y;
                var pSrc = (Color*)((Byte*)bmp.buffer + (pSrcY * bmpPitch));
                var pDst = (Color*)atlas.Surface.Pixels + ((reservation.Y + y) * atlas.Width) + reservation.X;
                for (int x = 0; x < bmpWidth; x++)
                {
                    var cSrc = *pSrc++;
                    var cDst = new Color(cSrc.B, cSrc.G, cSrc.R, cSrc.A);
                    *pDst++ = cDst;
                }
            }
        }

        // The FreeType2 face which this instance represents.
        private readonly FT_FaceRecFacade facade;
        private IntPtr face;

        // Cache of atlases used to store glyph images.
        private readonly List<DynamicTextureAtlas> atlases = 
            new List<DynamicTextureAtlas>();

        // Cache of metadata for loaded glyphs.
        private readonly Dictionary<Char, FreeTypeGlyphInfo> glyphInfoCache =
            new Dictionary<Char, FreeTypeGlyphInfo>();
    }
}
