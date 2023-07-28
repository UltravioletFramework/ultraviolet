using System;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a framework element which renders a rectangle.
    /// </summary>
    [UvmlKnownType]
    public class Rectangle : Shape
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Rectangle(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            DrawBlank(dc, null, FillColor);

            base.DrawOverride(time, dc);
        }
    }
}
