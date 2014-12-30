using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents an image.
    /// </summary>
    public abstract class Image
    {
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
                    textureID = value;
                    texture   = null;
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
        /// Gets a value indicating whether the image's texture resource has been loaded.
        /// </summary>
        public Boolean IsLoaded
        {
            get { return texture != null; }
        }

        /// <summary>
        /// Draws the image using the specified sprite batch.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatchBase{VertexType, SpriteData}"/> with which to draw the image.</param>
        /// <param name="position">The position at which to draw the image.</param>
        /// <param name="width">The width of the image in pixels.</param>
        /// <param name="height">The height of the image in pixels.</param>
        /// <param name="color">The image's color.</param>
        /// <param name="rotation">The image's rotation in radians.</param>
        /// <param name="origin">The image's point of origin.</param>
        /// <param name="effects">The image's rendering effects.</param>
        /// <param name="layerDepth">The image's layer depth.</param>
        /// <param name="data">The image's custom data.</param>
        internal abstract void Draw<VertexType, SpriteData>(SpriteBatchBase<VertexType, SpriteData> spriteBatch, Vector2 position, Int32 width, Int32 height, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth, SpriteData data)
            where VertexType : struct, IVertexType
            where SpriteData : struct;

        // Property values.
        private Texture2D texture;
        private AssetID textureID;
        private Rectangle textureRegion;
    }
}
