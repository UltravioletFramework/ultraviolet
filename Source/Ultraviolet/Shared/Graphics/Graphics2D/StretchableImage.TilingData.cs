using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    partial class StretchableImage
    {
        /// <summary>
        /// Represents the parameters of an invocation of 
        /// the <see cref="TileImageSegment{VertexType, SpriteData}(SpriteBatchBase{VertexType, SpriteData}, TilingMode, ref TilingData, SpriteData)"/> method.
        /// </summary>
        protected struct TilingData
        {
            /// <summary>
            /// Gets or sets the texture which contains the image segment.
            /// </summary>
            public Texture2D Texture { get; set; }

            /// <summary>
            /// Gets or sets the image segment's position in screen coordinates.
            /// </summary>
            public Vector2 Position { get; set; }

            /// <summary>
            /// Gets or sets the image segment's origin point.
            /// </summary>
            public Vector2 Origin { get; set; }

            /// <summary>
            /// Gets or sets a rectangle which represents where on the screen the image segment will be drawn.
            /// </summary>
            public RectangleF DestinationRectangle { get; set; }

            /// <summary>
            /// Gets or sets a rectangle which represents the position of the image segment on its texture.
            /// </summary>
            public Rectangle SourceRectangle { get; set; }

            /// <summary>
            /// Gets or sets the image segment's color.
            /// </summary>
            public Color Color { get; set; }

            /// <summary>
            /// Gets or sets the image segment's rotation in radians.
            /// </summary>
            public Single Rotation { get; set; }

            /// <summary>
            /// Gets or sets the image segment's layer depth.
            /// </summary>
            public Single LayerDepth { get; set; }

            /// <summary>
            /// Gets or sets the image segment's rendering effects.
            /// </summary>
            public SpriteEffects Effects { get; set; }
        }
    }
}
