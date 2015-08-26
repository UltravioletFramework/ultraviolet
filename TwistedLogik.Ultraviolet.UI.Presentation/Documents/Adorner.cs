using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Documents
{
    /// <summary>
    /// Represents an element which decorates another selement.
    /// </summary>
    [UvmlKnownType]
    public class Adorner : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Adorner"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Adorner(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }
    }
}
