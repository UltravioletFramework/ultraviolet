using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a horizontal slider.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.HSlider.xml")]
    public class HSlider : SliderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HSlider"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public HSlider(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (Track != null)
            {
                Track.InvalidateMeasure();
            }
            return base.MeasureOverride(availableSize);
        }

        // Component references.
        private readonly Track Track = null;
    }
}
