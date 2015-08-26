using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Documents
{
    /// <summary>
    /// Represents an element which decorates another selement.
    /// </summary>
    [UvmlKnownType]
    public abstract class Adorner : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Adorner"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        /// <param name="adornedElement">The element which is being adorned by this element.</param>
        protected Adorner(UltravioletContext uv, String name, UIElement adornedElement)
            : base(uv, name)
        {
            Contract.Require(adornedElement, "adornedElement");

            this.adornedElement = adornedElement;
        }

        /// <summary>
        /// Gets the element which is being adorned by this element.
        /// </summary>
        public UIElement AdornedElement
        {
            get { return adornedElement; }
        }

        // Property values.
        private readonly UIElement adornedElement;
    }
}
