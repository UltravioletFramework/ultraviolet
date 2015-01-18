using System;

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
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var bounds  = (RectangleF)display.DipsToPixels(AbsoluteBounds);

            dc.SpriteBatch.Draw(FrameworkResources.BlankTexture, bounds, FillColor * dc.Opacity);

            base.DrawOverride(time, dc);
        }
    }
}
