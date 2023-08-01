using System;

namespace Ultraviolet.Graphics
{
    partial class DynamicTextureAtlas
    {
        /// <summary>
        /// Represents an area on a texture atlas which has been successfully reserved.
        /// </summary>
        public struct Reservation
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Reservation"/> structure.
            /// </summary>
            /// <param name="atlas">The <see cref="DynamicTextureAtlas"/> which owns the reservation.</param>
            /// <param name="x">The x-coordinate of the reservation on the atlas.</param>
            /// <param name="y">The y-coordinate of the reservation on the atlas.</param>
            /// <param name="width">The width of the reservation in pixels.</param>
            /// <param name="height">The height of the reservation in pixels.</param>
            public Reservation(DynamicTextureAtlas atlas, Int32 x, Int32 y, Int32 width, Int32 height)
            {
                this.Atlas = atlas;
                this.X = x;
                this.Y = y;
                this.Width = width;
                this.Height = height;
            }

            /// <summary>
            /// Gets the <see cref="DynamicTextureAtlas"/> which owns the reservation.
            /// </summary>
            public DynamicTextureAtlas Atlas { get; }

            /// <summary>
            /// Gets the x-coordinate of the reservation on the atlas.
            /// </summary>
            public Int32 X { get; }

            /// <summary>
            /// Gets the y-coordinate of the reservation on the atlas.
            /// </summary>
            public Int32 Y { get; }

            /// <summary>
            /// Gets the width of the reservation in pixels.
            /// </summary>
            public Int32 Width { get; }

            /// <summary>
            /// Gets the height of the reservation in pixels.
            /// </summary>
            public Int32 Height { get; }
        }
    }
}
