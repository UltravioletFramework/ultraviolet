namespace Ultraviolet.Presentation
{
    partial class PresentationFoundation
    {
        /// <summary>
        /// Represents an entry in the table of elements waiting a RenderSizeChanged event.
        /// </summary>
        private struct RenderSizeChangedEntry
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RenderSizeChangedEntry"/> structure.
            /// </summary>
            /// <param name="element">The element which has requested a RenderSizeChanged event.</param>
            /// <param name="previousSize">The element's size prior to the layout change.</param>
            public RenderSizeChangedEntry(UIElement element, Size2D previousSize)
            {
                this.Element = element;
                this.PreviousSize = previousSize;
            }

            /// <summary>
            /// Gets the element that requested the RenderSizeChanged event.
            /// </summary>
            public UIElement Element { get; }

            /// <summary>
            /// Gets the element's size prior to the layout change.
            /// </summary>
            public Size2D PreviousSize { get; }

            /// <summary>
            /// Gets the element's current size.
            /// </summary>
            public Size2D CurrentSize => Element.RenderSize;
        }
    }
}
