using System;
using System.Diagnostics;
using System.Globalization;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a stretchable 2D image with nine segments.
    /// </summary>
    [DebuggerDisplay(@"\{Texture:{Texture} TextureRegion:{TextureRegion} Left:{Left} Top:{Top} Right:{Right} Bottom:{Bottom}\}")]
    public sealed class StretchableImage9 : StretchableImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StretchableImage9"/> class.
        /// </summary>
        private StretchableImage9()
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage9"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="top">The distance in pixels between the top edge of the image and the top edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <param name="bottom">The distance in pixels between the bottom edge of the image and the bottom edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage9"/> that was created.</returns>
        public static StretchableImage9 Create(AssetID texture, Int32 left, Int32 top, Int32 right, Int32 bottom)
        {
            return Create(texture, 0, 0, 0, 0, left, top, right, bottom);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage9"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <param name="x">The x-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="y">The y-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="width">The width of the region on the image's texture that contains the image.</param>
        /// <param name="height">The height of the region on the image's texture that contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="top">The distance in pixels between the top edge of the image and the top edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <param name="bottom">The distance in pixels between the bottom edge of the image and the bottom edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage9"/> that was created.</returns>
        public static StretchableImage9 Create(AssetID texture, Int32 x, Int32 y, Int32 width, Int32 height, Int32 left, Int32 top, Int32 right, Int32 bottom)
        {
            var img           = new StretchableImage9();
            img.TextureID     = texture;
            img.TextureRegion = new Rectangle(x, y, width, height);
            img.Left          = left;
            img.Top           = top;
            img.Right         = right;
            img.Bottom        = bottom;
            return img;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage9"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <param name="textureRegion">The region of the image's texture which contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="top">The distance in pixels between the top edge of the image and the top edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <param name="bottom">The distance in pixels between the bottom edge of the image and the bottom edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage9"/> that was created.</returns>
        public static StretchableImage9 Create(AssetID texture, Rectangle textureRegion, Int32 left, Int32 top, Int32 right, Int32 bottom)
        {
            return Create(texture, textureRegion.X, textureRegion.Y, textureRegion.Width, textureRegion.Height, left, top, right, bottom);
        }

        /// <summary>
        /// Converts the string representation of a stretchable image into an instance of the <see cref="StretchableImage9"/> class.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="image">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out StretchableImage9 image)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out image);
        }

        /// <summary>
        /// Converts the string representation of a stretchable image to an equivalent instance of the <see cref="StretchableImage9"/> class.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <returns>An instance of the <see cref="StretchableImage9"/> class that is equivalent to the specified string.</returns>
        public static StretchableImage9 Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a stretchable image into an instance of the <see cref="StretchableImage9"/> class.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="image">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out StretchableImage9 image)
        {
            Contract.Require(s, "s");

            var components = s.Split(' ');

            var texture    = AssetID.Invalid;
            var x          = 0;
            var y          = 0;
            var width      = 0;
            var height     = 0;
            var left       = 0;
            var top        = 0;
            var right      = 0;
            var bottom     = 0;
            var tileCenter = false;
            var tileEdges  = false;

            image = null;

            switch (components.Length)
            {
                case 9:
                case 10:
                case 11:
                    if (!AssetID.TryParse(components[0], out texture))
                        return false;
                    if (!Int32.TryParse(components[1], out x))
                        return false;
                    if (!Int32.TryParse(components[2], out y))
                        return false;
                    if (!Int32.TryParse(components[3], out width))
                        return false;
                    if (!Int32.TryParse(components[4], out height))
                        return false;
                    if (!Int32.TryParse(components[5], out left))
                        return false;
                    if (!Int32.TryParse(components[6], out top))
                        return false;
                    if (!Int32.TryParse(components[7], out right))
                        return false;
                    if (!Int32.TryParse(components[8], out bottom))
                        return false;
                    if (components.Length > 9)
                    {
                        if (!ParseTilingParameter(components[9], ref tileCenter, ref tileEdges))
                            return false;
                    }
                    if (components.Length > 10)
                    {
                        if (!ParseTilingParameter(components[10], ref tileCenter, ref tileEdges))
                            return false;
                    }
                    break;

                default:
                    return false;
            }

            image            = Create(texture, x, y, width, height, left, top, right, bottom);
            image.TileCenter = tileCenter;
            image.TileEdges  = tileEdges;
            return true;
        }

        /// <summary>
        /// Converts the string representation of a stretchable image to an equivalent instance of the <see cref="StretchableImage9"/> class.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>An instance of the <see cref="StretchableImage9"/> class that is equivalent to the specified string.</returns>
        public static StretchableImage9 Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            StretchableImage9 value;
            if (!TryParse(s, style, provider, out value))
            {
                throw new FormatException();
            }
            return value;
        }

        /// <summary>
        /// Gets the distance in pixels between the left edge of the image and the left edge of the image's center segment.
        /// </summary>
        public Int32 Left
        {
            get { return left; }
            set
            {
                Contract.EnsureRange(value >= 0, "value");

                left = value;
                UpdateMinimumRecommendedSize();
            }
        }

        /// <summary>
        /// Gets the distance in pixels between the top edge of the image and the top edge of the image's center segment.
        /// </summary>
        public Int32 Top
        {
            get { return top; }
            set
            {
                Contract.EnsureRange(value >= 0, "value");

                top = value;
                UpdateMinimumRecommendedSize();
            }
        }

        /// <summary>
        /// Gets the distance in pixels between the right edge of the image and the right edge of the image's center segment.
        /// </summary>
        public Int32 Right
        {
            get { return right; }
            set
            {
                Contract.EnsureRange(value >= 0, "value");

                right = value;
                UpdateMinimumRecommendedSize();
            }
        }

        /// <summary>
        /// Gets the distance in pixels between the bottom edge of the image and the bottom edge of the image's center segment.
        /// </summary>
        public Int32 Bottom
        {
            get { return bottom; }
            set
            {
                Contract.EnsureRange(value >= 0, "value");

                bottom = value;
                UpdateMinimumRecommendedSize();
            }
        }

        /// <inheritdoc/>
        internal override void Draw<VertexType, SpriteData>(SpriteBatchBase<VertexType, SpriteData> spriteBatch, Vector2 position, Int32 width, Int32 height, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            effects |= SpriteEffects.OriginRelativeToDestination;

            var srcLeft   = this.Left;
            var srcTop    = this.Top;
            var srcRight  = this.Right;
            var srcBottom = this.Bottom;

            var dstLeft   = srcLeft;
            var dstTop    = srcTop;
            var dstRight  = srcRight;
            var dstBottom = srcBottom;

            if (width < MinimumRecommendedSize.Width)
            {
                var scale = width / (float)MinimumRecommendedSize.Width;
                dstLeft   = (Int32)Math.Floor(dstLeft * scale);
                dstRight  = (Int32)Math.Ceiling(dstRight * scale);
            }
            if (height < MinimumRecommendedSize.Height)
            {
                var scale = height / (float)MinimumRecommendedSize.Height;
                dstTop    = (Int32)Math.Floor(dstTop * scale);
                dstBottom = (Int32)Math.Ceiling(dstBottom * scale);
            }

            var srcStretchableWidth  = this.TextureRegion.Width - (srcLeft + srcRight);
            var srcStretchableHeight = this.TextureRegion.Height - (srcTop + srcBottom);

            var dstStretchableWidth  = width - (dstLeft + dstRight);
            var dstStretchableHeight = height - (dstTop + dstBottom);

            // Center
            var centerSource   = new Rectangle(this.TextureRegion.Left + srcLeft, this.TextureRegion.Top + srcTop, srcStretchableWidth, srcStretchableHeight);
            var centerRegion   = new RectangleF(position.X, position.Y, dstStretchableWidth, dstStretchableHeight);
            var centerPosition = new Vector2(dstLeft, dstTop);
            if (this.TileCenter)
            {
                TileImageSegment(TilingMode.Both,
                    spriteBatch, this.Texture, centerPosition, centerRegion, centerSource, color, rotation, origin, effects, layerDepth, data);
            }
            else
            {
                var centerOrigin = origin - centerPosition;
                spriteBatch.Draw(this.Texture, centerRegion, centerSource, color, rotation, centerOrigin, effects, layerDepth, data);
            }

            // Edges
            var leftSource = new Rectangle(this.TextureRegion.Left, this.TextureRegion.Top + srcTop, srcLeft, srcStretchableHeight);
            var leftRegion = new RectangleF(position.X, position.Y, dstLeft, dstStretchableHeight);
            var leftPosition = new Vector2(0, dstTop);

            var rightSource = new Rectangle(this.TextureRegion.Right - srcRight, this.TextureRegion.Top + srcTop, srcRight, srcStretchableHeight);
            var rightRegion = new RectangleF(position.X, position.Y, dstRight, dstStretchableHeight);
            var rightPosition = new Vector2(width - dstRight, dstTop);

            var topSource = new Rectangle(this.TextureRegion.Left + srcLeft, this.TextureRegion.Top, srcStretchableWidth, srcTop);
            var topRegion = new RectangleF(position.X, position.Y, dstStretchableWidth, dstTop);
            var topPosition = new Vector2(dstLeft, 0);

            var bottomSource = new Rectangle(this.TextureRegion.Left + srcLeft, this.TextureRegion.Bottom - srcBottom, srcStretchableWidth, srcBottom);
            var bottomRegion = new RectangleF(position.X, position.Y, dstStretchableWidth, dstBottom);
            var bottomPosition = new Vector2(dstLeft, height - dstBottom);

            if (this.TileEdges)
            {
                TileImageSegment(TilingMode.Both,
                    spriteBatch, this.Texture, leftPosition, leftRegion, leftSource, color, rotation, origin, effects, layerDepth, data);
                TileImageSegment(TilingMode.Both,
                    spriteBatch, this.Texture, rightPosition, rightRegion, rightSource, color, rotation, origin, effects, layerDepth, data);
                TileImageSegment(TilingMode.Both,
                    spriteBatch, this.Texture, topPosition, topRegion, topSource, color, rotation, origin, effects, layerDepth, data);
                TileImageSegment(TilingMode.Both,
                    spriteBatch, this.Texture, bottomPosition, bottomRegion, bottomSource, color, rotation, origin, effects, layerDepth, data);
            }
            else
            {
                var leftOrigin = origin - leftPosition;
                spriteBatch.Draw(this.Texture, leftRegion, leftSource, color, rotation, leftOrigin, effects, layerDepth, data);
                var rightOrigin = origin - rightPosition;
                spriteBatch.Draw(this.Texture, rightRegion, rightSource, color, rotation, rightOrigin, effects, layerDepth, data);
                var topOrigin = origin - topPosition;
                spriteBatch.Draw(this.Texture, topRegion, topSource, color, rotation, topOrigin, effects, layerDepth, data);
                var bottomOrigin = origin - bottomPosition;
                spriteBatch.Draw(this.Texture, bottomRegion, bottomSource, color, rotation, bottomOrigin, effects, layerDepth, data);
            }

            // Corners
            var cornerTLRegion   = new RectangleF(position.X, position.Y, dstLeft, dstTop);
            var cornerTLPosition = new Vector2(0, 0);
            var cornerTLOrigin   = origin - cornerTLPosition;
            var cornerTLSource   = new Rectangle(this.TextureRegion.Left, this.TextureRegion.Top, srcLeft, srcTop);
            spriteBatch.Draw(this.Texture, cornerTLRegion, cornerTLSource, color, rotation, cornerTLOrigin, effects, layerDepth, data);

            var cornerTRRegion   = new RectangleF(position.X, position.Y, dstRight, dstTop);
            var cornerTRPosition = new Vector2(width - dstRight, 0);
            var cornerTROrigin   = origin - cornerTRPosition;
            var cornerTRSource   = new Rectangle(this.TextureRegion.Right - srcRight, this.TextureRegion.Top, srcRight, srcTop);
            spriteBatch.Draw(this.Texture, cornerTRRegion, cornerTRSource, color, rotation, cornerTROrigin, effects, layerDepth, data);

            var cornerBLRegion   = new RectangleF(position.X, position.Y, dstLeft, dstBottom);
            var cornerBLPosition = new Vector2(0, height - dstBottom);
            var cornerBLOrigin   = origin - cornerBLPosition;
            var cornerBLSource   = new Rectangle(this.TextureRegion.Left, this.TextureRegion.Bottom - srcBottom, srcLeft, srcBottom);
            spriteBatch.Draw(this.Texture, cornerBLRegion, cornerBLSource, color, rotation, cornerBLOrigin, effects, layerDepth, data);

            var cornerBRRegion   = new RectangleF(position.X, position.Y, dstRight, dstBottom);
            var cornerBRPosition = new Vector2(width - dstRight, height - dstBottom);
            var cornerBROrigin   = origin - cornerBRPosition;
            var cornerBRSource   = new Rectangle(this.TextureRegion.Right - srcRight, this.TextureRegion.Bottom - srcBottom, srcLeft, srcBottom);
            spriteBatch.Draw(this.Texture, cornerBRRegion, cornerBRSource, color, rotation, cornerBROrigin, effects, layerDepth, data);
        }

        /// <summary>
        /// Updates the value of the <see cref="TextureImage.MinimumRecommendedSize"/> property.
        /// </summary>
        private void UpdateMinimumRecommendedSize()
        {
            MinimumRecommendedSize = new Size2(left + right, top + bottom);
        }

        // Property values.
        private Int32 left;
        private Int32 top;
        private Int32 right;
        private Int32 bottom;
    }
}
