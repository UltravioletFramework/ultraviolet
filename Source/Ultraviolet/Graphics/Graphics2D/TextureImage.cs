using System;
using System.Globalization;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Core.Data;
using Ultraviolet.Platform;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a region on a texture which corresponds to a particular image.
    /// </summary>
    public abstract class TextureImage
    {
        /// <summary>
        /// Initializes the <see cref="TextureImage"/> type.
        /// </summary>
        static TextureImage()
        {
            ObjectResolver.RegisterValueResolver<TextureImage>(ImageResolver);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureImage"/> class.
        /// </summary>
        internal TextureImage() { }

        /// <summary>
        /// Loads the image's texture resource from the specified content manager.
        /// </summary>
        /// <param name="content">The content manager with which to load the image's texture resource.</param>
        /// <param name="watch">A value indicating whether the <see cref="TextureImage"/> should watch the file
        /// system for changes to its underlying resources.</param>
        public void Load(ContentManager content, Boolean watch = false)
        {
            Contract.Require(content, nameof(content));

            if (!TextureID.IsValid)
                return;

            texture = watch ? content.Watchers.GetSharedWatchedAsset<Texture2D>(TextureID) : 
                (WatchableAssetReference<Texture2D>)content.Load<Texture2D>(TextureID);            

            if (TextureRegion.IsEmpty && !texture.IsNullReference)
                TextureRegion = new Rectangle(0, 0, texture.Value.Width, texture.Value.Height);
        }

        /// <summary>
        /// Loads the image's texture resource from the specified content manager.
        /// </summary>
        /// <param name="content">The content manager with which to load the image's texture resource.</param>
        /// <param name="density"></param>
        /// <param name="watch">A value indicating whether the <see cref="TextureImage"/> should watch the file
        /// system for changes to its underlying resources.</param>
        public void Load(ContentManager content, ScreenDensityBucket density, Boolean watch = false)
        {
            Contract.Require(content, nameof(content));

            if (!TextureID.IsValid)
                return;

            texture = watch ? content.Watchers.GetSharedWatchedAsset<Texture2D>(TextureID, density) :
                (WatchableAssetReference<Texture2D>)content.Load<Texture2D>(TextureID, density);

            if (TextureRegion.IsEmpty && !texture.IsNullReference)
                TextureRegion = new Rectangle(0, 0, texture.Value.Width, texture.Value.Height);
        }

        /// <summary>
        /// Gets the image's texture resource.
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
            protected set
            {
                texture = value;
                textureID = AssetID.Invalid;
            }
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
                    texture = WatchableAssetReference<Texture2D>.Null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the region of the image's texture which contains the image.
        /// </summary>
        public Rectangle TextureRegion { get; set; }

        /// <summary>
        /// Gets the size of the image's texture region.
        /// </summary>
        public Size2 TextureRegionSize => TextureRegion.Size;

        /// <summary>
        /// Gets the image's minimum recommended size. Texture images may be drawn at sizes smaller than that specified
        /// by this property, but doing so will degrade their graphical quality.
        /// </summary>
        public Size2 MinimumRecommendedSize { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this object represents a valid image.
        /// </summary>
        public Boolean IsValid => textureID.IsValid;

        /// <summary>
        /// Gets a value indicating whether the image's texture resource has been loaded.
        /// </summary>
        public Boolean IsLoaded => !texture.IsNullReference;

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
        internal abstract void Draw<VertexType, SpriteData>(SpriteBatchBase<VertexType, SpriteData> spriteBatch, Vector2 position, Single width, Single height, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth, SpriteData data)
            where VertexType : struct, IVertexType
            where SpriteData : struct;

        /// <summary>
        /// Resolves a string into an instance of the <see cref="TextureImage"/> class.
        /// </summary>
        /// <param name="value">The string value to resolve.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>The resolved object.</returns>
        private static Object ImageResolver(String value, IFormatProvider provider)
        {
            var numericComponents = CountNumericComponents(value);

            switch (numericComponents)
            {
                case 4:
                    return StaticImage.Parse(value, NumberStyles.Integer, provider);

                case 6:
                    return StretchableImage3.Parse(value, NumberStyles.Integer, provider);

                case 8:
                    return StretchableImage9.Parse(value, NumberStyles.Integer, provider);
            }

            throw new FormatException();
        }

        /// <summary>
        /// Counts the number of numeric components in the specified image string.
        /// </summary>
        /// <param name="value">The string containing the image being parsed.</param>
        /// <returns>The number of numeric components in the specified image string.</returns>
        private static Int32 CountNumericComponents(String value)
        {
            var components = value.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);

            var numericComponents = 0;
            for (int i = 1; i < components.Length; i++)
            {
                Int32 integer;
                if (!Int32.TryParse(components[i], out integer))
                    break;

                numericComponents++;
            }

            return numericComponents;
        }

        // Property values.
        private WatchableAssetReference<Texture2D> texture;
        private AssetID textureID;
    }
}
