using System;
using System.Diagnostics;
using System.Globalization;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a stretchable 2D image with three segments.
    /// </summary>
    [DebuggerDisplay(@"\{Texture:{Texture} TextureRegion:{TextureRegion} Left:{Left} Right:{Right}\}")]
    public sealed class StretchableImage3 : StretchableImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        private StretchableImage3()
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage3"/> that was created.</returns>
        public static StretchableImage3 Create(AssetID texture, Int32 left, Int32 right)
        {
            return Create(texture, 0, 0, 0, 0, left, right);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <param name="x">The x-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="y">The y-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="width">The width of the region on the image's texture that contains the image.</param>
        /// <param name="height">The height of the region on the image's texture that contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage3"/> that was created.</returns>
        public static StretchableImage3 Create(AssetID texture, Int32 x, Int32 y, Int32 width, Int32 height, Int32 left, Int32 right)
        {
            var img           = new StretchableImage3();
            img.TextureID     = texture;
            img.TextureRegion = new Rectangle(x, y, width, height);
            img.Left          = left;
            img.Right         = right;
            return img;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <param name="textureRegion">The region of the image's texture which contains the image.</param>
        /// <param name="left">The distance in pixels between the left edge of the image and the left edge of the image's center segment.</param>
        /// <param name="right">The distance in pixels between the right edge of the image and the right edge of the image's center segment.</param>
        /// <returns>The new instance of <see cref="StretchableImage3"/> that was created.</returns>
        public static StretchableImage3 Create(AssetID texture, Rectangle textureRegion, Int32 left, Int32 right)
        {
            return Create(texture, textureRegion.X, textureRegion.Y, textureRegion.Width, textureRegion.Height, left, right);
        }

        /// <summary>
        /// Converts the string representation of a stretchable image into an instance of the <see cref="StretchableImage3"/> class.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="image">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out StretchableImage3 image)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out image);
        }

        /// <summary>
        /// Converts the string representation of a stretchable image to an equivalent instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <returns>An instance of the <see cref="StretchableImage3"/> class that is equivalent to the specified string.</returns>
        public static StretchableImage3 Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a stretchable image into an instance of the <see cref="StretchableImage3"/> class.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="image">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out StretchableImage3 image)
        {
            Contract.Require(s, "s");

            var components = s.Split(' ');

            var texture    = AssetID.Invalid;
            var x          = 0;
            var y          = 0;
            var width      = 0;
            var height     = 0;
            var left       = 0;
            var right      = 0;
            var tileCenter = false;
            var tileEdges  = false;

            image = null;

            switch (components.Length)
            {
                case 7:
                case 8:
                case 9:
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
                    if (!Int32.TryParse(components[6], out right))
                        return false;
                    if (components.Length > 7)
                    {
                        if (!ParseTilingParameter(components[7], ref tileCenter, ref tileEdges))
                            return false;
                    }
                    if (components.Length > 8)
                    {
                        if (!ParseTilingParameter(components[8], ref tileCenter, ref tileEdges))
                            return false;
                    }
                    break;

                default:
                    return false;
            }

            image            = Create(texture, x, y, width, height, left, right);
            image.TileCenter = tileCenter;
            image.TileEdges  = tileEdges;
            return true;
        }

        /// <summary>
        /// Converts the string representation of a stretchable image to an equivalent instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>An instance of the <see cref="StretchableImage3"/> class that is equivalent to the specified string.</returns>
        public static StretchableImage3 Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            StretchableImage3 value;
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
                UpdateMinimumSize();
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
                UpdateMinimumSize();
            }
        }

        /// <inheritdoc/>
        internal override void Draw<VertexType, SpriteData>(SpriteBatchBase<VertexType, SpriteData> spriteBatch, Vector2 position, Int32 width, Int32 height, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            effects |= SpriteEffects.OriginRelativeToDestination;

            if (MinimumSize.Width > 0 && width < MinimumSize.Width)
                width = MinimumSize.Width;

            if (MinimumSize.Height > 0 && height < MinimumSize.Height)
                height = MinimumSize.Height;

            var srcStretchableWidth  = this.TextureRegion.Width - (this.Left + this.Right);
            var srcStretchableHeight = this.TextureRegion.Height;

            var dstStretchableWidth  = width - (this.Left + this.Right);
            var dstStretchableHeight = height;

            // Center
            var centerSource   = new Rectangle(this.TextureRegion.Left + this.Left, this.TextureRegion.Top, srcStretchableWidth, srcStretchableHeight);
            var centerRegion   = new RectangleF(position.X, position.Y, dstStretchableWidth, dstStretchableHeight);
            var centerPosition = new Vector2(this.Left, 0);
            if (this.TileCenter)
            {
                TileImageSegment(spriteBatch, this.Texture, centerPosition, centerRegion, centerSource, color, rotation, origin, effects, layerDepth, data);
            }
            else
            {
                var centerOrigin   = origin - centerPosition;
                spriteBatch.Draw(this.Texture, centerRegion, centerSource, color, rotation, centerOrigin, effects, layerDepth, data);
            }

            // Edges
            var leftSource = new Rectangle(this.TextureRegion.Left, this.TextureRegion.Top, this.Left, srcStretchableHeight);
            var leftRegion = new RectangleF(position.X, position.Y, this.Left, dstStretchableHeight);
            var leftPosition = new Vector2(0, 0);

            var rightSource = new Rectangle(this.TextureRegion.Right - this.Right, this.TextureRegion.Top, this.Right, srcStretchableHeight);
            var rightRegion = new RectangleF(position.X, position.Y, this.Right, dstStretchableHeight);
            var rightPosition = new Vector2(width - this.Right, 0);

            if (this.TileEdges)
            {
                TileImageSegment(spriteBatch, this.Texture, leftPosition, leftRegion, leftSource, color, rotation, origin, effects, layerDepth, data);
                TileImageSegment(spriteBatch, this.Texture, rightPosition, rightRegion, rightSource, color, rotation, origin, effects, layerDepth, data);
            }
            else
            {
                var leftOrigin = origin - leftPosition;
                spriteBatch.Draw(this.Texture, leftRegion, leftSource, color, rotation, leftOrigin, effects, layerDepth, data);
                var rightOrigin = origin - rightPosition;
                spriteBatch.Draw(this.Texture, rightRegion, rightSource, color, rotation, rightOrigin, effects, layerDepth, data);
            }
        }

        /// <summary>
        /// Updates the value of the <see cref="MinimumSize"/> property.
        /// </summary>
        private void UpdateMinimumSize()
        {
            MinimumSize = new Size2(left + right, 0);
        }

        // Property values.
        private Int32 left;
        private Int32 right;
    }
}
