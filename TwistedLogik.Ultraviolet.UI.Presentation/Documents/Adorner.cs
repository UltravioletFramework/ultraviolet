﻿using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Media;
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
        /// <param name="adornedElement">The element which is being adorned by this element.</param>
        protected Adorner(UIElement adornedElement)
            : base(adornedElement.Ultraviolet, null)
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

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var desiredSize = new Size2D(AdornedElement.RenderSize.Width, AdornedElement.RenderSize.Height);

            var childrenCount = VisualTreeHelper.GetChildrenCount(this);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(this, i) as UIElement;
                if (child != null)
                {
                    child.Measure(desiredSize);
                }
            }

            return desiredSize;
        }

        // Property values.
        private readonly UIElement adornedElement;
    }
}
