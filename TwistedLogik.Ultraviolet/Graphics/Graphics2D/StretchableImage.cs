using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents an image made of segments and designed to be stretched over an arbitrary area.
    /// </summary>
    public abstract class StretchableImage : TextureImage
    {
        /// <summary>
        /// Represents the axes along which an image can be tiled.
        /// </summary>
        protected enum TilingMode
        {
            /// <summary>
            /// Specifies that the image should be tiled along the horizontal axis.
            /// </summary>
            Horizontal = 0x01,

            /// <summary>
            /// Specifies that the image should be tiled along the vertical axis.
            /// </summary>
            Vertical = 0x02,

            /// <summary>
            /// Specifies that the image should be tiled along both axes.
            /// </summary>
            Both = Vertical | Horizontal,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StretchableImage"/> class.
        /// </summary>
        internal StretchableImage()
        {

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
        /// Parses a tiling parameter included in a string which represents a stretchable image.
        /// </summary>
        /// <param name="parameter">The parameter string to parse.</param>
        /// <param name="tileCenter">A value indicating whether the image is set to tile its center piece.</param>
        /// <param name="tileEdges">A value indicating whether the image is set to tile its edges.</param>
        /// <returns><c>true</c> if the parameter was parsed successfully; otherwise, <c>false</c>.</returns>
        protected static Boolean ParseTilingParameter(String parameter, ref Boolean tileCenter, ref Boolean tileEdges)
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

        /// <summary>
        /// Draws a tiled image segment.
        /// </summary>
        /// <typeparam name="VertexType">The type of vertex used to render the batch's sprites.</typeparam>
        /// <typeparam name="SpriteData">The type of data object associated with each of the batch's sprite instances.</typeparam>
        /// <param name="mode">A <see cref="TilingMode"/> value which specifies how to tile the image.</param>
        /// <param name="spriteBatch">The sprite batch with which to draw the segment.</param>
        /// <param name="texture">The segment's texture.</param>
        /// <param name="position">The segment's position in screen coordinates.</param>
        /// <param name="destinationRectangle">A rectangle which indicates where on the screen the segment will be drawn.</param>
        /// <param name="sourceRectangle">The segment's position on its texture, or <c>null</c> to draw the entire texture.</param>
        /// <param name="color">The segment's tint color.</param>
        /// <param name="rotation">The segment's rotation in radians.</param>
        /// <param name="origin">The segment's origin point.</param>
        /// <param name="effects">The segment's rendering effects.</param>
        /// <param name="layerDepth">The segment's layer depth.</param>
        /// <param name="data">The segment's custom data.</param>
        protected static void TileImageSegment<VertexType, SpriteData>(TilingMode mode, SpriteBatchBase<VertexType, SpriteData> spriteBatch,
            Texture2D texture, Vector2 position, RectangleF destinationRectangle, Rectangle sourceRectangle, Color color, Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth, SpriteData data)
            where VertexType : struct, IVertexType
            where SpriteData : struct
        {
            var tileHorizontally = (mode & TilingMode.Horizontal) == TilingMode.Horizontal;
            var tileVertically   = (mode & TilingMode.Vertical) == TilingMode.Vertical;

            var tileCountX = tileHorizontally ?                
                (Int32)Math.Ceiling(destinationRectangle.Width / (Single)sourceRectangle.Width) : 1;

            var tileCountY = tileVertically ?                
                (Int32)Math.Ceiling(destinationRectangle.Height / (Single)sourceRectangle.Height) : 1;

            var cx = 0f;
            var cy = 0f;

            for (int y = 0; y < tileCountY; y++)
            {
                for (int x = 0; x < tileCountX; x++)
                {
                    var srcTileWidth  = Math.Min(sourceRectangle.Width, destinationRectangle.Width - cx);
                    var srcTileHeight = Math.Min(sourceRectangle.Height, destinationRectangle.Height - cy);

                    var dstTileWidth  = tileHorizontally ? srcTileWidth : destinationRectangle.Width;
                    var dstTileHeight = tileVertically ? srcTileHeight : destinationRectangle.Height;

                    var tileRegion   = new RectangleF(destinationRectangle.X, destinationRectangle.Y, dstTileWidth, dstTileHeight);
                    var tileSource   = new Rectangle(sourceRectangle.X, sourceRectangle.Y, (Int32)srcTileWidth, (Int32)srcTileHeight);
                    var tilePosition = new Vector2(position.X + cx, position.Y + cy);
                    var tileOrigin   = origin - tilePosition;
                    spriteBatch.Draw(texture, tileRegion, tileSource, color, rotation, tileOrigin, effects, layerDepth, data);

                    cx = cx + sourceRectangle.Width;
                }
                cx = 0;
                cy = cy + sourceRectangle.Height;
            }
        }

        // Property values.
        private Boolean tileEdges;
        private Boolean tileCenter;
    }
}
