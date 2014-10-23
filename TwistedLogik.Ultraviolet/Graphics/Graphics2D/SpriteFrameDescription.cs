using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Describes a <see cref="SpriteFrame"/> object during deserialization.
    /// </summary>
    public struct SpriteFrameDescription
    {
        /// <summary>
        /// Retrieves the content resource path to the frame's texture atlas.
        /// </summary>
        public String Atlas
        {
            get { return atlas; }
            set { atlas = value; }
        }

        /// <summary>
        /// Retreives the name of the frame's texture atlas cell.
        /// </summary>
        public String AtlasCell
        {
            get { return atlasCell; }
            set { atlasCell = value; }
        }

        /// <summary>
        /// Retrieves the content resource path to the frame's texture.
        /// </summary>
        public String Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        /// <summary>
        /// Retrieves the frame's texture resource.
        /// </summary>
        public Texture2D TextureResource
        {
            get { return textureResource; }
            set { textureResource = value; }
        }

        /// <summary>
        /// Retrieves the distance, in pixels, between the left edge of the frame's texture
        /// and the left edge of the frame.
        /// </summary>
        public Int32 X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Retrieves the distance, in pixels, between the top edge of the frame's texture
        /// and the top edge of the frame.
        /// </summary>
        public Int32 Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Retrieves the width of the frame in pixels.
        /// </summary>
        public Int32 Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// Retrieves the height of the frame in pixels.
        /// </summary>
        public Int32 Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// Retrieves the distance, in pixels, between the top left corner of the frame
        /// and its origin point.
        /// </summary>
        public Int32 OriginX
        {
            get { return originX; }
            set { originX = value; }
        }

        /// <summary>
        /// Retrieves the distance, in pixels, between the top left corner of the frame
        /// and its origin point.
        /// </summary>
        public Int32 OriginY
        {
            get { return originY; }
            set { originY = value; }
        }

        /// <summary>
        /// Retrieves the frame's duration in milliseconds.
        /// </summary>
        public Int32 Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        // Property values.
        private String atlas;
        private String atlasCell;
        private String texture;
        private Texture2D textureResource;
        private Int32 x;
        private Int32 y;
        private Int32 width;
        private Int32 height;
        private Int32 originX;
        private Int32 originY;
        private Int32 duration;
    }
}
