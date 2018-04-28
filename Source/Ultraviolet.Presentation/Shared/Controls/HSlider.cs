using System;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a horizontal slider.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.HSlider.xml")]
    public class HSlider : OrientedSlider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HSlider"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public HSlider(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }
    }
}
