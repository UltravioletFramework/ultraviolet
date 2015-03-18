using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a vertical slider.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.VSlider.xml")]
    public class VSlider : SliderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VSlider"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public VSlider(UltravioletContext uv, String id)
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
