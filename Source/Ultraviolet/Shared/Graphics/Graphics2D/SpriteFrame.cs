using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents one of the images that constitutes a <see cref="SpriteAnimation"/>.
    /// </summary>
    public sealed class SpriteFrame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFrame"/> class.
        /// </summary>
        /// <param name="description">The frame description.</param>
        /// <param name="texture">The texture that contains the frame.</param>
        internal SpriteFrame(SpriteFrameDescription description, Texture2D texture)
        {
            this.atlas = description.Atlas;
            this.atlasCell = description.AtlasCell;
            this.texture = description.Texture;
            this.textureResource = texture;
            this.x = description.X ?? 0;
            this.y = description.Y ?? 0;
            this.width = description.Width ?? 0;
            this.height = description.Height ?? 0;
            this.originX = description.Origin?.X ?? 0;
            this.originY = description.Origin?.Y ?? 0;
            this.duration = description.Duration ?? 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFrame"/> class.
        /// </summary>
        /// <param name="texture">The texture that contains the frame's image.</param>
        /// <param name="x">The x-coordinate, in pixels, of the area on the texture that contains the frame's image.</param>
        /// <param name="y">The y-coordinate, in pixels, of the area on the texture that contains the frame's image.</param>
        /// <param name="width">The width, in pixels, of the area on the texture that contains the frame's image.</param>
        /// <param name="height">The height, in pixels, of the area on the texture that contains the frame's image.</param>
        /// <param name="duration">The duration of the frame in milliseconds.</param>
        public SpriteFrame(Texture2D texture, Int32 x, Int32 y, Int32 width, Int32 height, Int32 duration = 0)
            : this(texture, x, y, width, height, 0, 0, duration)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFrame"/> class.
        /// </summary>
        /// <param name="texture">The texture that contains the frame's image.</param>
        /// <param name="x">The x-coordinate, in pixels, of the area on the texture that contains the frame's image.</param>
        /// <param name="y">The y-coordinate, in pixels, of the area on the texture that contains the frame's image.</param>
        /// <param name="width">The width, in pixels, of the area on the texture that contains the frame's image.</param>
        /// <param name="height">The height, in pixels, of the area on the texture that contains the frame's image.</param>
        /// <param name="originX">The x-coordinate, in pixels, of the frame's origin point relative
        /// to the top-left corner of the frame.</param>
        /// <param name="originY">The y-coordinate, in pixels, of the frame's origin point relative
        /// to the top-left corner of the frame.</param>
        /// <param name="duration">The duration of the frame in milliseconds.</param>
        public SpriteFrame(Texture2D texture, Int32 x, Int32 y, Int32 width, Int32 height, Int32 originX, Int32 originY, Int32 duration = 0)
        {
            Contract.Require(texture, nameof(texture));

            this.texture = null;
            this.textureResource = texture;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.originX = originX;
            this.originY = originY;
            this.duration = duration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFrame"/> class.
        /// </summary>
        /// <param name="atlas">The atlas that contains the frame's image.</param>
        /// <param name="atlasCell">The atlas cell that represents the frame's image.</param>
        /// <param name="duration">The duration of the frame in milliseconds.</param>
        public SpriteFrame(TextureAtlas atlas, String atlasCell, Int32 duration = 0)
            : this(atlas, atlasCell, 0, 0, duration)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFrame"/> class.
        /// </summary>
        /// <param name="atlas">The atlas that contains the frame's image.</param>
        /// <param name="atlasCell">The atlas cell that represents the frame's image.</param>
        /// <param name="originX">The x-coordinate, in pixels, of the frame's origin point relative
        /// to the top-left corner of the frame.</param>
        /// <param name="originY">The y-coordinate, in pixels, of the frame's origin point relative
        /// to the top-left corner of the frame.</param>
        /// <param name="duration">The duration of the frame in milliseconds.</param>
        public SpriteFrame(TextureAtlas atlas, String atlasCell, Int32 originX, Int32 originY, Int32 duration = 0)
        {
            Contract.Require(atlas, nameof(atlas));
            Contract.RequireNotEmpty(atlasCell, nameof(atlasCell));

            this.atlas = null;
            this.atlasCell = atlasCell;
            this.textureResource = atlas;
            this.originX = originX;
            this.originY = originY;
            this.duration = duration;
        }

        /// <summary>
        /// Retrieves the content resource path to the frame's texture atlas.
        /// </summary>
        public String Atlas
        {
            get { return atlas; }
        }

        /// <summary>
        /// Retrieves the name of the frame's texture atlas cell.
        /// </summary>
        public String AtlasCell
        {
            get { return atlasCell; }
        }

        /// <summary>
        /// Retrieves the content resource path to the frame's texture.
        /// </summary>
        public String Texture
        {
            get { return texture; }
        }

        /// <summary>
        /// Retrieves the frame's texture resource.
        /// </summary>
        public Texture2D TextureResource
        {
            get { return textureResource; }
        }

        /// <summary>
        /// Gets a rectangle containing the frame's area on its texture.
        /// </summary>
        public Rectangle Area
        {
            get { return new Rectangle(x, y, width, height); }
        }

        /// <summary>
        /// Retrieves the distance, in pixels, between the left edge of the frame's texture
        /// and the left edge of the frame.
        /// </summary>
        public Int32 X
        {
            get { return x; }
        }

        /// <summary>
        /// Retrieves the distance, in pixels, between the top edge of the frame's texture
        /// and the top edge of the frame.
        /// </summary>
        public Int32 Y
        {
            get { return y; }
        }

        /// <summary>
        /// Retrieves the width of the frame in pixels.
        /// </summary>
        public Int32 Width
        {
            get { return width; }
        }

        /// <summary>
        /// Retrieves the height of the frame in pixels.
        /// </summary>
        public Int32 Height
        {
            get { return height; }
        }

        /// <summary>
        /// Gets the frame's origin point.
        /// </summary>
        public Vector2 Origin
        {
            get { return new Vector2(originX, originY); }
        }

        /// <summary>
        /// Retrieves the distance, in pixels, between the top left corner of the frame
        /// and its origin point.
        /// </summary>
        public Int32 OriginX
        {
            get { return originX; }
        }

        /// <summary>
        /// Retrieves the distance, in pixels, between the top left corner of the frame
        /// and its origin point.
        /// </summary>
        public Int32 OriginY
        {
            get { return originY; }
        }

        /// <summary>
        /// Retrieves the frame's duration in milliseconds.
        /// </summary>
        public Int32 Duration
        {
            get { return duration; }
        }

        // Property values.
        private readonly String atlas;
        private readonly String atlasCell;
        private readonly String texture;
        private readonly Texture2D textureResource;
        private readonly Int32 x;
        private readonly Int32 y;
        private readonly Int32 width;
        private readonly Int32 height;
        private readonly Int32 originX;
        private readonly Int32 originY;
        private readonly Int32 duration;
    }
}
