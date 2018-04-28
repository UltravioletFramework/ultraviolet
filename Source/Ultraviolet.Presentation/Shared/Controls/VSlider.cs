using System;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a vertical slider.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.VSlider.xml")]
    public class VSlider : OrientedSlider
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
    }
}
