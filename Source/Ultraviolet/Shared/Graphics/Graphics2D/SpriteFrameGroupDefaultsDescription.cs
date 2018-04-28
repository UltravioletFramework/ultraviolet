using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// An intermediate representation of the default values used for sprite frame groups.
    /// </summary>
    internal sealed class SpriteFrameGroupDefaultsDescription
    {
        /// <summary>
        /// Gets or sets the asset path of the frame group's texture.
        /// </summary>
        public String Texture { get; set; }
        
        /// <summary>
        /// Gets or sets the duration of the frames in the group in milliseconds.
        /// </summary>
        public Int32? Duration { get; set; }

        /// <summary>
        /// Gets or sets the distance between the left edge of the frame's texture
        /// and the left edge of the frame group, in pixels.
        /// </summary>
        public Int32? X { get; set; }

        /// <summary>
        /// Gets or sets the distance between the top edge of the frame's texture
        /// and the top edge of the frame group, in pixels.
        /// </summary>
        public Int32? Y { get; set; }

        /// <summary>
        /// Gets or sets the frame group's width in pixels.
        /// </summary>
        public Int32? Width { get; set; }

        /// <summary>
        /// Gets or sets the frame group's height in pixels.
        /// </summary>
        public Int32? Height { get; set; }

        /// <summary>
        /// Gets or sets the number of frames in the group.
        /// </summary>
        public Int32? FrameCount { get; set; }

        /// <summary>
        /// Gets or sets the width of the frames in the group in pixels.
        /// </summary>
        public Int32? FrameWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the frame in the group in pixels.
        /// </summary>
        public Int32? FrameHeight { get; set; }

        /// <summary>
        /// Gets or sets the point of origin for the frame group's frames.
        /// </summary>
        public Point2? Origin { get; set; }
    }
}
