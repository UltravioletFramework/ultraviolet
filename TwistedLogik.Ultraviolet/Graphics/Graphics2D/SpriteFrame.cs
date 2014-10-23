using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
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
        public SpriteFrame(SpriteFrameDescription description)
        {
            this.atlas = description.Atlas;
            this.atlasCell = description.AtlasCell;
            this.texture = description.Texture;
            this.textureResource = description.TextureResource;
            this.x = description.X;
            this.y = description.Y;
            this.width = description.Width;
            this.height = description.Height;
            this.originX = description.OriginX;
            this.originY = description.OriginY;
            this.duration = description.Duration;
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
