using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the header data for a batched <see cref="Sprite"/>.
    /// </summary>
    public struct SpriteHeader
    {
        /// <summary>
        /// The x-coordinate of the top-left corner of the sprite's source rectangle.
        /// </summary>
        public Int32 SourceX;

        /// <summary>
        /// The y-coordinate of the top-left corner of the sprite's source rectangle.
        /// </summary>
        public Int32 SourceY;

        /// <summary>
        /// The width of the sprite's source rectangle.
        /// </summary>
        public Int32 SourceWidth;

        /// <summary>
        /// The height of the sprite's source rectangle.
        /// </summary>
        public Int32 SourceHeight;

        /// <summary>
        /// The x-coordinate of the top-left corner of the sprite's destination rectangle.
        /// </summary>
        public Single DestinationX;

        /// <summary>
        /// The y-coordinate of the top-left corner of the sprite's destination rectangle.
        /// </summary>
        public Single DestinationY;

        /// <summary>
        /// The width of the sprite's destination rectangle.
        /// </summary>
        public Single DestinationWidth;

        /// <summary>
        /// The height of the sprite's destination rectangle.
        /// </summary>
        public Single DestinationHeight;

        /// <summary>
        /// The x-coordinate of the sprite's origin point.
        /// </summary>
        public Single OriginX;

        /// <summary>
        /// The y-coordinate of the sprite's origin point.
        /// </summary>
        public Single OriginY;

        /// <summary>
        /// The sprite's rotation in radians.
        /// </summary>
        public Single Rotation;

        /// <summary>
        /// The sprite's layer depth.
        /// </summary>
        public Single Depth;

        /// <summary>
        /// The sprite's tint color.
        /// </summary>
        public Color Color;

        /// <summary>
        /// The sprite's rendering effect.
        /// </summary>
        public SpriteEffects Effects;
    }
}
