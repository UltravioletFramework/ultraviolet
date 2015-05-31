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
        public ScrollContentPresenter(UltravioletContext uv, String name)
            : base(uv, name)
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
            var templatedParentElement = TemplatedParent as UIElement;
            if (templatedParentElement == null)
                return availableSize;

            var hCanScroll = CanScrollHorizontally;
            var vCanScroll = CanScrollVertically;

            var contentAvailableSize = new Size2D(
                hCanScroll ? Double.PositiveInfinity : availableSize.Width,
                vCanScroll ? Double.PositiveInfinity : availableSize.Height
            );

            var contentSize = base.MeasureOverride(contentAvailableSize);

            var presenterSize = new Size2D(
                Math.Min(availableSize.Width, contentSize.Width),
                Math.Min(availableSize.Height, contentSize.Height)
            );

            var extentChanged = (extentWidth != contentSize.Width || extentHeight != contentSize.Height);

            extentWidth  = contentSize.Width;
            extentHeight = contentSize.Height;

            var viewportChanged = (viewportWidth != presenterSize.Width || viewportHeight != presenterSize.Height);

            viewportWidth  = presenterSize.Width;
            viewportHeight = presenterSize.Height;

            if (templatedParentElement != null && (extentChanged || viewportChanged))
            {
                templatedParentElement.InvalidateMeasure();
            }

            return presenterSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            if (TemplatedParent == null)
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

        /// <inheritdoc/>
        protected override void PositionChildrenOverride()
        {
            VisualTreeHelper.ForEachChild<UIElement>(this, this, (child, state) =>
            {
                var scrollCP     = (ScrollContentPresenter)state;
                var scrollViewer = scrollCP.TemplatedParent as ScrollViewer;
                var scrollOffset = scrollViewer == null ? Size2D.Zero : new Size2D(-scrollViewer.HorizontalOffset, -scrollViewer.VerticalOffset);

                var frameworkElement = child as FrameworkElement;
                if (frameworkElement != null)
                {
                    child.Position(frameworkElement.TemplatedParent == state ? Size2D.Zero : scrollOffset);
                }
                else
                {
                    child.Position(Size2D.Zero);
                }
                child.PositionChildren();
            });
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
