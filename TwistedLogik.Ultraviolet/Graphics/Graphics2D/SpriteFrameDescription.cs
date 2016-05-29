using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Describes a <see cref="SpriteFrame"/> object during deserialization.
    /// </summary>
    internal sealed class SpriteFrameDescription
    {
        /// <summary>
        /// Retrieves the content resource path to the frame's texture atlas.
        /// </summary>
        public String Atlas { get; set; }

        /// <summary>
        /// Retreives the name of the frame's texture atlas cell.
        /// </summary>
        public String AtlasCell { get; set; }

        /// <summary>
        /// Retrieves the content resource path to the frame's texture.
        /// </summary>
        public String Texture { get; set; }
        
        /// <summary>
        /// Retrieves the distance, in pixels, between the left edge of the frame's texture
        /// and the left edge of the frame.
        /// </summary>
        public Int32 X { get; set; }

        /// <summary>
        /// Retrieves the distance, in pixels, between the top edge of the frame's texture
        /// and the top edge of the frame.
        /// </summary>
        public Int32 Y { get; set; }

        /// <summary>
        /// Retrieves the width of the frame in pixels.
        /// </summary>
        public Int32 Width { get; set; }

        /// <summary>
        /// Retrieves the height of the frame in pixels.
        /// </summary>
        public Int32 Height { get; set; }

        /// <summary>
        /// Retrieves the distance, in pixels, between the top left corner of the frame
        /// and its origin point.
        /// </summary>
        public Int32 OriginX { get; set; }

        /// <summary>
        /// Retrieves the distance, in pixels, between the top left corner of the frame
        /// and its origin point.
        /// </summary>
        public Int32 OriginY { get; set; }

        /// <summary>
        /// Retrieves the frame's duration in milliseconds.
        /// </summary>
        public Int32 Duration { get; set; }
    }
}
