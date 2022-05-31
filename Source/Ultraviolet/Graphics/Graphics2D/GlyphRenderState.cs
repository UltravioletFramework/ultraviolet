using System;
using System.Runtime.CompilerServices;
using Ultraviolet.Core.Text;
using Ultraviolet.Graphics.Graphics2D.Text;

namespace Ultraviolet.Graphics.Graphics2D
{
    public partial class SpriteBatchBase<VertexType, SpriteData>
    {
        /// <summary>
        /// Represents the internal state of a <see cref="SpriteBatch"/> when it is rendering a string of glyphs.
        /// </summary>
        internal struct GlyphRenderState
        {
            /// <summary>
            /// Creates a new <see cref="GlyphRenderState"/> structure from the parameters to the <see cref="DrawStringInternal{TSource}"/> method.
            /// </summary>
            public static GlyphRenderState FromDrawStringParameters(UltravioletFontFace fontFace,
                Vector2 position, Vector2 origin, Vector2 scale, Single rotation, SpriteEffects effects, Size2 measure)
            {
                // Determine whether the text is flipped.
                var isFlippedHorizontally = (effects & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally;
                var isFlippedVertically = (effects & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically;

                // Calculate the text area from its measurement.
                var areaTL = new Vector2(position.X - origin.X, position.Y - origin.Y);
                var areaBR = new Vector2(position.X - origin.X + measure.Width, position.Y - origin.Y + measure.Height);

                // Calculate the transformation matrix.
                var transformRotation = Matrix.CreateRotationZ(rotation);
                var transformScale = Matrix.CreateScale(scale.X, scale.Y, 1f);
                Matrix.Multiply(ref transformRotation, ref transformScale, out var transform);

                // Transform the text area.
                Vector2.Transform(ref areaTL, ref transform, out var areaTransformedTL);
                Vector2.Transform(ref areaBR, ref transform, out var areaTransformedBR);
                var areaSize = new Size2F(areaBR.X - areaTL.X, areaBR.Y - areaTL.Y);
                var areaTransformedSize = new Size2F(
                    areaTransformedBR.X - areaTransformedTL.X,
                    areaTransformedBR.Y - areaTransformedTL.Y);
                var textStartPosition = new Vector2(
                    isFlippedHorizontally ? areaTransformedBR.X : areaTransformedTL.X,
                    isFlippedVertically ? areaTransformedBR.Y : areaTransformedTL.Y);

                // Create the new render state object.
                return new GlyphRenderState()
                {
                    FontFace = fontFace,
                    TextStartPosition = textStartPosition,
                    TextRenderPosition = textStartPosition,
                    TextPosition = position,
                    TextOrigin = origin,
                    TextScale = scale,
                    TextIsFlippedHorizontally = isFlippedHorizontally,
                    TextIsFlippedVertically = isFlippedVertically,
                    TextDirection = new Vector2(isFlippedHorizontally ? -1 : 1, isFlippedVertically ? -1 : 1),
                    TextTransform = transform,
                    TextArea = new RectangleF(areaTL, areaSize),
                    TextAreaTransformed = new RectangleF(areaTransformedTL, areaTransformedSize),
                };
            }

            /// <summary>
            /// Updates the state object with the next glyph to draw.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void RetrieveGlyph<TSource>(TSource text, Int32 i, ref Int32 length)
                where TSource : IStringSource<Char>
            {
                var iNext = i + 1;
                var characterIsSurrogatePair = iNext < text.Length && Char.IsSurrogatePair(text[i], text[iNext]);
                if (characterIsSurrogatePair)
                {
                    if (iNext >= text.Length)
                    {
                        GlyphCharacter = FontFace.SubstitutionCharacter;
                        GlyphIndexOrCodePoint = GlyphCharacter;
                    }
                    else
                    {
                        GlyphCharacter = Char.MinValue;
                        GlyphIndexOrCodePoint = Char.ConvertToUtf32(text[i], text[iNext]);
                    }
                    length = 2;
                }
                else
                {
                    GlyphCharacter = text[i];
                    GlyphIndexOrCodePoint = GlyphCharacter;
                    length = 1;
                }
            }

            /// <summary>
            /// Updates the state object with the next glyph to draw.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void RetrieveGlyph<TSource>(TSource text, Int32 i)
                where TSource : IStringSource<ShapedChar>
            {
                var sc = text[i];
                GlyphCharacter = sc.GetSpecialCharacter();
                GlyphIndexOrCodePoint = sc.GlyphIndex;
                GlyphAdvance = new Vector2(sc.Advance, 0);
                GlyphOffset = new Vector2(sc.OffsetX, sc.OffsetY);
            }

            /// <summary>
            /// Updates the state object from the rendering information for the current glyph.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void UpdateFromRenderInfo()
            {
                GlyphRenderInfo glyphRenderInfo;
                if (TextIsShaped)
                {
                    FontFace.GetGlyphIndexRenderInfo(GlyphIndexOrCodePoint, out glyphRenderInfo);
                }
                else
                {
                    FontFace.GetCodePointRenderInfo(GlyphIndexOrCodePoint, out glyphRenderInfo);
                    GlyphAdvance = new Vector2(glyphRenderInfo.Advance, 0);
                }

                GlyphTexture = glyphRenderInfo.Texture;
                GlyphTextureRegion = glyphRenderInfo.TextureRegion;
                GlyphPosition.X = TextIsFlippedHorizontally ?
                    (TextRenderPosition.X - (glyphRenderInfo.OffsetX + GlyphOffset.X)) - GlyphTextureRegion.Width :
                    (TextRenderPosition.X + (glyphRenderInfo.OffsetX + GlyphOffset.X));
                GlyphPosition.Y = TextIsFlippedVertically ?
                    (TextRenderPosition.Y - (glyphRenderInfo.OffsetY - GlyphOffset.Y) + GlyphKerning.Y) - GlyphTextureRegion.Height :
                    (TextRenderPosition.Y + (glyphRenderInfo.OffsetY - GlyphOffset.Y) + GlyphKerning.Y);
                GlyphOrigin = new Vector2(GlyphTextureRegion.Width / 2, GlyphTextureRegion.Height / 2);
            }
            
            /// <summary>
            /// Updates the state object from the specified glyph data.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void UpdateFromGlyphData(ref GlyphData glyphData)
            {
                if (glyphData.DirtyUnicodeCodePoint)
                {
                    GlyphIndexOrCodePoint = glyphData.UnicodeCodePoint;
                    UpdateFromRenderInfo();
                    return;
                }

                if (glyphData.DirtyPosition)
                {
                    GlyphPosition.X = glyphData.X;
                    GlyphPosition.Y = glyphData.Y;
                }

                if (glyphData.DirtyScale)
                    GlyphScale = GlyphScale * new Vector2(glyphData.ScaleX, glyphData.ScaleY);

                if (glyphData.DirtyColor)
                    GlyphColor = glyphData.Color;
            }
            
            /// <summary>
            /// Populates a glyph data object from the current state.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void PopulateGlyphData(ref GlyphData glyphData)
            {
                glyphData.UnicodeCodePoint = TextIsShaped ? GlyphCharacter : GlyphIndexOrCodePoint;
                glyphData.Pass = GlyphShaderPass;
                glyphData.X = GlyphPosition.X;
                glyphData.Y = GlyphPosition.Y;
                glyphData.ScaleX = 1.0f;
                glyphData.ScaleY = 1.0f;
                glyphData.Color = GlyphColor;
                glyphData.ClearDirtyFlags();
            }

            /// <summary>
            /// Processes special characters.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Boolean ProcessSpecialCharacters()
            {
                switch (GlyphCharacter)
                {
                    case '\t':
                        GlyphShaderPass = 0;
                        TextRenderPosition.X = TextRenderPosition.X + (FontFace.TabWidth * TextDirection.X);
                        return true;

                    case '\r':
                        GlyphShaderPass = 0;
                        return true;

                    case '\n':
                        GlyphShaderPass = 0;
                        TextRenderPosition.X = TextStartPosition.X;
                        TextRenderPosition.Y = TextRenderPosition.Y + (FontFace.LineSpacing * TextDirection.Y);
                        return true;
                }
                return false;
            }

            /// <summary>
            /// Processes a glyph shader pass.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Boolean ProcessGlyphShaderPass(ref GlyphShaderContext glyphShaderContext, ref GlyphData glyphData, Int32 i, ref Int32 length)
            {
                if (glyphShaderContext.IsValid)
                {
                    PopulateGlyphData(ref glyphData);
                    GlyphShaderPass++;

                    glyphShaderContext.Execute(ref glyphData, glyphShaderContext.SourceOffset + i);

                    UpdateFromGlyphData(ref glyphData);
                    if (glyphData.DirtyUnicodeCodePoint)
                    {
                        length = 0;
                        return true;
                    }
                }

                GlyphShaderPass = 0;
                return false;
            }

            /// <summary>
            /// The font face with which the string is being drawn.
            /// </summary>
            public UltravioletFontFace FontFace;

            /// <summary>
            /// The position at which text rendering starts.
            /// </summary>
            public Vector2 TextStartPosition;

            /// <summary>
            /// The position at which text is being rendered.
            /// </summary>
            public Vector2 TextRenderPosition;

            /// <summary>
            /// The position which was passed to the <see cref="DrawStringInternal{TSource}"/> method.
            /// </summary>
            public Vector2 TextPosition;

            /// <summary>
            /// The origin around which the text is transformed.
            /// </summary>
            public Vector2 TextOrigin;

            /// <summary>
            /// The scaling factor which is applied to the text.
            /// </summary>
            public Vector2 TextScale;

            /// <summary>
            /// The direction in which the rendering position advances.
            /// </summary>
            public Vector2 TextDirection;

            /// <summary>
            /// The area in which the text is being drawn.
            /// </summary>
            public RectangleF TextArea;

            /// <summary>
            /// The area in which the text is being drawn, after the transformation matrix is applied.
            /// </summary>
            public RectangleF TextAreaTransformed;

            /// <summary>
            /// A value indicating whether the text being drawn has been shaped.
            /// </summary>
            public Boolean TextIsShaped;

            /// <summary>
            /// A value indicating whether the text is being flipped horizontally.
            /// </summary>
            public Boolean TextIsFlippedHorizontally;

            /// <summary>
            /// A value indicating whether the text is being flipped vertically.
            /// </summary>
            public Boolean TextIsFlippedVertically;

            /// <summary>
            /// The matrix which is used to transform the text.
            /// </summary>
            public Matrix TextTransform;

            /// <summary>
            /// Gets the character which this glyph represents.
            /// </summary>
            public Char GlyphCharacter;

            /// <summary>
            /// The glyph index or Unicode code point of the glyph being drawn.
            /// </summary>
            public Int32 GlyphIndexOrCodePoint;

            /// <summary>
            /// The index of the current glyph shader pass.
            /// </summary>
            public Int32 GlyphShaderPass;

            /// <summary>
            /// The position at which the current glyph is being drawn.
            /// </summary>
            public Vector2 GlyphPosition;

            /// <summary>
            /// The offset applied to the glyph when drawn.
            /// </summary>
            public Vector2 GlyphOffset;

            /// <summary>
            /// The kerning offsets for the current glyph.
            /// </summary>
            public Vector2 GlyphKerning;

            /// <summary>
            /// The advance for the current glyph.
            /// </summary>
            public Vector2 GlyphAdvance;

            /// <summary>
            /// The origin around which the current glyph is transformed.
            /// </summary>
            public Vector2 GlyphOrigin;

            /// <summary>
            /// The scaling factor for the current glyph.
            /// </summary>
            public Vector2 GlyphScale;

            /// <summary>
            /// The color of the current glyph.
            /// </summary>
            public Color GlyphColor;

            /// <summary>
            /// The texture that contains the current glyph.
            /// </summary>
            public Texture2D GlyphTexture;

            /// <summary>
            /// The region of the glyph texture that represents the current glyph.
            /// </summary>
            public Rectangle GlyphTextureRegion;
        }
    }
}
