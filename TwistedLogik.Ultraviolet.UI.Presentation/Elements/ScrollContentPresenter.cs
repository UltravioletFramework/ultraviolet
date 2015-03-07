using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents the content presenter for a <see cref="ScrollViewer"/>.
    /// </summary>
    [UIElement("ScrollContentPresenter")]
    public class ScrollContentPresenter : ContentPresenter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollContentPresenter"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public ScrollContentPresenter(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Gets or sets a value indicating whether the presenter's content can scroll horizontally.
        /// </summary>
        public Boolean CanScrollHorizontally
        {
            get { return canScrollHorizontally; }
            set { canScrollHorizontally = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the presenter's content can scroll vertically.
        /// </summary>
        public Boolean CanScrollVertically
        {
            get { return canScrollVertically; }
            set { canScrollVertically = value; }
        }

        /// <summary>
        /// Gets the width of the scrolled content.
        /// </summary>
        public Double ExtentWidth
        {
            get { return extentWidth; }
        }

        /// <summary>
        /// Gets the height of the scrolled content.
        /// </summary>
        public Double ExtentHeight
        {
            get { return extentHeight; }
        }

        /// <summary>
        /// Gets the width of the scrolling viewport.
        /// </summary>
        public Double ViewportWidth
        {
            get { return viewportWidth; }
        }

        /// <summary>
        /// Gets the height of the scrolling viewport.
        /// </summary>
        public Double ViewportHeight
        {
            get { return viewportHeight; }
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (Control == null)
                return availableSize;

            var contentAvailableSize = new Size2D(
                CanScrollHorizontally ? Double.PositiveInfinity : availableSize.Width,
                CanScrollVertically ? Double.PositiveInfinity : availableSize.Height
            );

            var contentSize   = Control.OnContentPresenterMeasure(contentAvailableSize);
            var presenterSize = new Size2D(
                Math.Min(availableSize.Width, contentSize.Width),
                Math.Min(availableSize.Height, contentSize.Height)
            );

            extentWidth  = contentSize.Width;
            extentHeight = contentSize.Height;

            viewportWidth  = presenterSize.Width;
            viewportHeight = presenterSize.Height;

            return presenterSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            if (Control == null)
                return finalSize;

            // TODO: extentWidth/Height is "correct"... but causes content to overflow... ContentControl is screwed up?

            var contentFinalSize = new Size2D(
                CanScrollHorizontally ? extentWidth : finalSize.Width,
                CanScrollVertically ? extentHeight : finalSize.Height
            );

            var contentSize   = Control.OnContentPresenterArrange(contentFinalSize, options);
            var presenterSize = new Size2D(
                Math.Min(finalSize.Width, contentSize.Width),
                Math.Min(finalSize.Height, contentSize.Height)
            );

            extentWidth  = contentSize.Width;
            extentHeight = contentSize.Height;

            viewportWidth  = presenterSize.Width;
            viewportHeight = presenterSize.Height;

            return presenterSize;
        }

        // Property values.
        private Boolean canScrollHorizontally;
        private Boolean canScrollVertically;
        private Double extentWidth;
        private Double extentHeight;
        private Double viewportWidth;
        private Double viewportHeight;
    }
}
