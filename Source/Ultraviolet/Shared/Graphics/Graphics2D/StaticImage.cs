using System;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a static 2D image.
    /// </summary>
    public sealed partial class StaticImage : TextureImage
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
        /// Creates a new instance of the <see cref="StaticImage"/> class.
        /// </summary>
        /// <param name="texture">The texture that contains the image.</param>
        /// <returns>The new instance of <see cref="StaticImage"/> that was created.</returns>
        public static StaticImage Create(Texture2D texture)
        {
            return Create(texture, 0, 0, 0, 0);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StaticImage"/> class.
        /// </summary>
        /// <param name="texture">The texture that contains the image.</param>
        /// <param name="x">The x-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="y">The y-coordinate of the region on the image's texture that contains the image.</param>
        /// <param name="width">The width of the region on the image's texture that contains the image.</param>
        /// <param name="height">The height of the region on the image's texture that contains the image.</param>
        /// <returns>The new instance of <see cref="StaticImage"/> that was created.</returns>
        public static StaticImage Create(Texture2D texture, Int32 x, Int32 y, Int32 width, Int32 height)
        {
            var img = new StaticImage();
            img.Texture = texture;
            img.TextureRegion = new Rectangle(x, y, width, height);
            return img;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StaticImage"/> class.
        /// </summary>
        /// <param name="texture">The texture that contains the image.</param>
        /// <param name="textureRegion">The region of the image's texture which contains the image.</param>
        /// <returns>The new instance of <see cref="StaticImage"/> that was created.</returns>
        public static StaticImage Create(Texture2D texture, Rectangle textureRegion)
        {
            return Create(texture, textureRegion.X, textureRegion.Y, textureRegion.Width, textureRegion.Height);
        }

        /// <inheritdoc/>
        internal override void Draw<VertexType, SpriteData>(SpriteBatchBase<VertexType, SpriteData> spriteBatch, Vector2 position, Single width, Single height, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth, SpriteData data)
        {
            effects |= SpriteEffects.OriginRelativeToDestination;

            position -= origin;

            var realOrigin = new Vector2(
                (int)((origin.X / width) * TextureRegion.Width), 
                (int)((origin.Y / height) * TextureRegion.Height));

            position += realOrigin;

            var srcRect = TextureRegion;
            var dstRect = new RectangleF(position.X, position.Y, width, height);
            spriteBatch.Draw(Texture, dstRect, srcRect, color, rotation, realOrigin, effects, layerDepth, data);
        }
    }
}
