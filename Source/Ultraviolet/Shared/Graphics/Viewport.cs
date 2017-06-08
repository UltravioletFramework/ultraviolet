using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a region of the screen in which rendering takes place.
    /// </summary>
    [Serializable]
    public partial struct Viewport : IEquatable<Viewport>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Viewport"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the upper-left corner of the viewport on the render target surface.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the viewport on the render target surface.</param>
        /// <param name="width">The width of the viewport in pixels.</param>
        /// <param name="height">The height of the viewport in pixels.</param>
        public Viewport(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{X:{X} Y:{Y} Width:{Width} Height:{Height}}}";
                
        /// <summary>
        /// The x-coordinate of the upper-left corner of the viewport on the render target surface.
        /// </summary>
        public Int32 X;

        /// <summary>
        /// The y-coordinate of the upper-left corner of the viewport on the render target surface.
        /// </summary>
        public Int32 Y;

        /// <summary>
        /// The width of the viewport in pixels.
        /// </summary>
        public Int32 Width;

        /// <summary>
        /// The height of the viewport in pixels.
        /// </summary>
        public Int32 Height;        
    }
}
