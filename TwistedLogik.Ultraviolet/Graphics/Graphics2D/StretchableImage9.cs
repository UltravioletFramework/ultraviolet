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
    public sealed class StretchableImage9
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
            img.TextureID       = texture;
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
        /// <param name="s">A string containing an image to convert.</param>
        /// <param name="image">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out StretchableImage9 image)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out image);
        }

        /// <summary>
        /// Converts the string representation of a stretchable image to an equivalent instance of the <see cref="StretchableImage9"/> class.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <returns>An instance of the <see cref="StretchableImage9"/> structure that is equivalent to the specified string.</returns>
        public static StretchableImage9 Parse(String s)
        {
            return Parse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a stretchable image into an instance of the <see cref="StretchableImage9"/> class.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a stretchable image to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="image">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out StretchableImage9 image)
        {
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
                case 5:
                case 6:
                case 7:
                    if (!AssetID.TryParse(components[0], out texture))
                        return false;
                    if (!Int32.TryParse(components[1], out left))
                        return false;
                    if (!Int32.TryParse(components[2], out top))
                        return false;
                    if (!Int32.TryParse(components[3], out right))
                        return false;
                    if (!Int32.TryParse(components[4], out bottom))
                        return false;
                    if (components.Length > 5)
                    {
                        if (!ParseTilingParameter(components[5], ref tileCenter, ref tileEdges))
                            return false;
                    }
                    if (components.Length > 6)
                    {
                        if (!ParseTilingParameter(components[6], ref tileCenter, ref tileEdges))
                            return false;
                    }
                    break;

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
        /// <param name="s">A string containing the angle to convert.</param>
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
        /// Loads the image's texture resource from the specified content manager.
        /// </summary>
        /// <param name="content">The content manager with which to load the image's texture resource.</param>
        public void Load(ContentManager content)
        {
            Contract.Require(content, "content");

            texture = content.Load<Texture2D>(TextureID);
            if (textureRegion.IsEmpty && texture != null)
            {
                textureRegion = new Rectangle(0, 0, texture.Width, texture.Height);
            }
        }

        /// <summary>
        /// Gets the image's texture resource.
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
        }

        /// <summary>
        /// Gets or sets the asset identifier of the texture which contains the stretchable image data.
        /// </summary>
        public AssetID TextureID
        {
            get { return textureID; }
            set 
            {
                if (!textureID.Equals(value))
                {
                    textureID         = value;
                    texture = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the region of the image's texture which contains the image.
        /// </summary>
        public Rectangle TextureRegion
        {
            get { return textureRegion; }
            set { textureRegion = value; }
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
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the image should be drawn with tiled edges.
        /// </summary>
        public Boolean TileEdges
        {
            get { return tileEdges; }
            set { tileEdges = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the image should be drawn with a tiled center piece.
        /// </summary>
        public Boolean TileCenter
        {
            get { return tileCenter; }
            set { tileCenter = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the image's texture resource has been loaded.
        /// </summary>
        public Boolean IsLoaded
        {
            get { return texture != null; }
        }

        /// <summary>
        /// Parses a tiling parameter included in a string which represents a stretchable image.
        /// </summary>
        /// <param name="parameter">The parameter string to parse.</param>
        /// <param name="tileCenter">A value indicating whether the image is set to tile its center piece.</param>
        /// <param name="tileEdges">A value indicating whether the image is set to tile its edges.</param>
        /// <returns><c>true</c> if the parameter was parsed successfully; otherwise, <c>false</c>.</returns>
        private static Boolean ParseTilingParameter(String parameter, ref Boolean tileCenter, ref Boolean tileEdges)
        {
            if (String.Equals(parameter, "tile-center", StringComparison.OrdinalIgnoreCase))
            {
                if (tileCenter)
                {
                    return false;
                }
                tileCenter = true;
                return true;
            }
            if (String.Equals(parameter, "tile-edges", StringComparison.OrdinalIgnoreCase))
            {
                if (tileEdges)
                {
                    return false;
                }
                tileEdges = true;
                return true;
            }
            return false;
        }

        // Property values.
        private Texture2D texture;
        private AssetID textureID;
        private Rectangle textureRegion;
        private Int32 left;
        private Int32 top;
        private Int32 right;
        private Int32 bottom;
        private Boolean tileEdges;
        private Boolean tileCenter;
    }
}
