using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a framework element which renders a rectangle.
    /// </summary>
    [UIElement("Rectangle")]
    public class Rectangle : Shape
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The unique identifier of this element within its layout.</param>
        public Rectangle(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            var bounds = (RectangleF)AbsoluteBounds;

            spriteBatch.Draw(FrameworkResources.BlankTexture, bounds, FillColor * opacity);

            base.DrawOverride(time, spriteBatch, opacity);
        }
    }
}
