using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Documents
{
    /// <summary>
    /// Represents a layer for containing adorners
    /// </summary>
    [UvmlKnownType]
    public class AdornerLayer : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdornerLayer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public AdornerLayer(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }
    }
}
