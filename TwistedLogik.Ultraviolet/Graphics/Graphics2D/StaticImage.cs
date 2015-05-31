using System;
using System.Diagnostics;
using System.Globalization;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a static 2D image.
    /// </summary>
    [DebuggerDisplay(@"\{Texture:{Texture} TextureRegion:{TextureRegion}\}")]
    public sealed class StaticImage : TextureImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticImage"/> class.
        /// </summary>
        private StaticImage()
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="StaticImage"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <returns>The new instance of <see cref="StaticImage"/> that was created.</returns>
        public static StaticImage Create(AssetID texture)
        {
            return Create(texture, 0, 0, 0, 0);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StaticImage"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <param name="x">The x-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="y">The y-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="width">The width of the region on the image's texture that contains the image.</param>
        /// <param name="height">The height of the region on the image's texture that contains the image.</param>
        /// <returns>The new instance of <see cref="StaticImage"/> that was created.</returns>
        public static StaticImage Create(AssetID texture, Int32 x, Int32 y, Int32 width, Int32 height)
        {
            var img           = new StaticImage();
            img.TextureID     = texture;
            img.TextureRegion = new Rectangle(x, y, width, height);
            return img;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StaticImage"/> class.
        /// </summary>
        /// <param name="texture">The asset identifier of the texture that contains the image.</param>
        /// <param name="textureRegion">The region of the image's texture which contains the image.</param>
        /// <returns>The new instance of <see cref="StaticImage"/> that was created.</returns>
        public static StaticImage Create(AssetID texture, Rectangle textureRegion)
        {
            return Create(texture, textureRegion.X, textureRegion.Y, textureRegion.Width, textureRegion.Height);
        }

        /// <summary>
        /// Converts the string representation of a static image into an instance of the <see cref="StaticImage"/> class.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="image">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out StaticImage image)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out image);
        }

        /// <summary>
        /// Converts the string representation of a static image to an equivalent instance of the <see cref="StaticImage"/> class.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <returns>An instance of the <see cref="StretchableImage3"/> class that is equivalent to the specified string.</returns>
        public static StaticImage Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a static image into an instance of the <see cref="StaticImage"/> class.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="image">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out StaticImage image)
        {
            Contract.Require(s, "s");

            var components = s.Split(' ');

            var texture    = AssetID.Invalid;
            var x          = 0;
            var y          = 0;
            var width      = 0;
            var height     = 0;

            image = null;

            if (components.Length != 5)
                return false;

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

            image = Create(texture, x, y, width, height);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a static image to an equivalent instance of the <see cref="StaticImage"/> class.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>An instance of the <see cref="StaticImage"/> class that is equivalent to the specified string.</returns>
        public static StaticImage Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            StaticImage value;
            if (!TryParse(s, style, provider, out value))
            {
                throw new FormatException();
            }
            return value;
        }

        /// <inheritdoc/>
        internal override void Draw<VertexType, SpriteData>(SpriteBatchBase<VertexType, SpriteData> spriteBatch, Vector2 position, Int32 width, Int32 height, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            effects |= SpriteEffects.OriginRelativeToDestination;

            var srcRect = TextureRegion;
            var dstRect = new Rectangle((Int32)position.X, (Int32)position.Y, width, height);
            spriteBatch.Draw(Texture, dstRect, srcRect, color, rotation, origin, effects, layerDepth, data);
        }
    }
}
