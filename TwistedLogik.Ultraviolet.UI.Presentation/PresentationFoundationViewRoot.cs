using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the root layout element for a <see cref="PresentationFoundationView"/>.
    /// </summary>
    public sealed class PresentationFoundationViewRoot : Decorator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationFoundationViewRoot"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public PresentationFoundationViewRoot(UltravioletContext uv, String name) 
            : base(uv, name)
        {

        }
    }
}
