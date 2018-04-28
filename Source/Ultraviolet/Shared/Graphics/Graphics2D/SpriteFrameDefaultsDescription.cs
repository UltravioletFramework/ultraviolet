using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// An intermediate representation of the default values used for sprite frames.
    /// </summary>
    internal sealed class SpriteFrameDefaultsDescription
    {
        /// <summary>
        /// Gets or sets the asset path of the frame's atlas.
        /// </summary>
        public String Atlas { get; set; }

        /// <summary>
        /// Gets or sets the name of the frame's atlas cell.
        /// </summary>
        public String AtlasCell { get; set; }

        /// <summary>
        /// Gets or sets the asset path of the frame's texture.
        /// </summary>
        public String Texture { get; set; }
        
        /// <summary>
        /// Gets or sets the frame's duration in milliseconds.
        /// </summary>
        public Int32? Duration { get; set; }

        /// <summary>
        /// Gets or sets the distance between the left edge of the frame's texture
        /// and the left edge of the frame, in pixels.
        /// </summary>
        public Int32? X { get; set; }

        /// <summary>
        /// Gets or sets the distance between the top edge of the frame's texture
        /// and the top edge of the frame, in pixels.
        /// </summary>
        public Int32? Y { get; set; }

        /// <summary>
        /// Gets or sets the frame's width in pixels.
        /// </summary>
        public Int32? Width { get; set; }

        /// <summary>
        /// Gets or sets the frame's height in pixels.
        /// </summary>
        public Int32? Height { get; set; }

        /// <summary>
        /// Gets or sets the frame's point of origin.
        /// </summary>
        public Point2? Origin { get; set; }
    }
}
