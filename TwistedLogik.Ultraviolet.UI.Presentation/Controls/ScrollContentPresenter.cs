using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents the content presenter for a <see cref="ScrollViewer"/>.
    /// </summary>
    [UvmlKnownType]
    public class ScrollContentPresenter : ContentPresenter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollContentPresenter"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ScrollContentPresenter(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        public override Point2D ContentOffset
        {
            get
            {
                var scrollViewer = Control as ScrollViewer;
                if (scrollViewer != null)
                {
                    return new Point2D(-scrollViewer.HorizontalOffset, -scrollViewer.VerticalOffset);
                }
                return Point2D.Zero;
            }
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

            var hCanScroll = CanScrollHorizontally;
            var vCanScroll = CanScrollVertically;

            var contentAvailableSize = new Size2D(
                hCanScroll ? Double.PositiveInfinity : availableSize.Width,
                vCanScroll ? Double.PositiveInfinity : availableSize.Height
            );

            var contentSize = base.MeasureOverride(contentAvailableSize);

            var hSizeToContent = !hCanScroll || Double.IsPositiveInfinity(availableSize.Width);
            var vSizeToContent = !vCanScroll || Double.IsPositiveInfinity(availableSize.Height);

            var presenterSize = new Size2D(
                hSizeToContent ? Math.Min(availableSize.Width, contentSize.Width) : availableSize.Width,
                vSizeToContent ? Math.Min(availableSize.Height, contentSize.Height) : availableSize.Height
            );

            var extentChanged = (extentWidth != contentSize.Width || extentHeight != contentSize.Height);

            extentWidth  = contentSize.Width;
            extentHeight = contentSize.Height;

            var viewportChanged = (viewportWidth != presenterSize.Width || viewportHeight != presenterSize.Height);

            viewportWidth  = presenterSize.Width;
            viewportHeight = presenterSize.Height;

            if (Control != null && (extentChanged || viewportChanged))
            {
                Control.InvalidateMeasure();
            }

            return presenterSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            if (Control == null)
                return finalSize;

            var contentFinalSize = new Size2D(
                CanScrollHorizontally ? extentWidth : finalSize.Width,
                CanScrollVertically ? extentHeight : finalSize.Height
            );

            var contentSize   = base.ArrangeOverride(contentFinalSize, options);
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
