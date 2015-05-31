using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a vertical slider.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.VSlider.xml")]
    public class VSlider : SliderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VSlider"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public VSlider(UltravioletContext uv, String name)
            : base(uv, name)
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
