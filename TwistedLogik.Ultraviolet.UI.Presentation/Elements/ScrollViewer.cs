using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a control which provides a scrollable view of its content.
    /// </summary>
    [UIElement("ScrollViewer", "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.ScrollViewer.xml")]
    public class ScrollViewer : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public ScrollViewer(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        public override Point2D ContentOffset
        {
            get { return new Point2D(-horizontalOffset, -verticalOffset); }
        }

        /// <summary>
        /// Gets the width of the content which is being displayed by the scroll viewer.
        /// </summary>
        public Double ExtentWidth
        {
            get 
            {
                if (Content == null)
                    return 0;

                // TODO: Handle non-UIElement content
                return ((UIElement)Content).RenderSize.Width;
            }
        }

        /// <summary>
        /// Gets the height of the content which is being displayed by the scroll viewer.
        /// </summary>
        public Double ExtentHeight
        {
            get
            {
                if (Content == null)
                    return 0;

                // TODO: Handle non-UIElement content
                return ((UIElement)Content).RenderSize.Height;
            }
        }

        /// <summary>
        /// Gets the width of the scroll viewer's scrollable area.
        /// </summary>
        public Double ScrollableWidth
        {
            get { return ExtentWidth - ViewportWidth; }
        }

        /// <summary>
        /// Gets the height of the scroll viewer's scrollable area.
        /// </summary>
        public Double ScrollableHeight
        {
            get { return ExtentHeight - ViewportHeight; }
        }

        /// <summary>
        /// Gets the width of the scroll viewer's viewport.
        /// </summary>
        public Double ViewportWidth
        {
            get { return RelativeContentRegion.Width; }
        }

        /// <summary>
        /// Gets the height of the scroll viewer's viewport.
        /// </summary>
        public Double ViewportHeight
        {
            get { return RelativeContentRegion.Height; }
        }

        /// <summary>
        /// Gets the horizontal offset of the scrolled content.
        /// </summary>
        public Double HorizontalOffset
        {
            get { return horizontalOffset; }
            private set
            {
                if (horizontalOffset != value)
                {
                    horizontalOffset = value;
                    InvalidatePosition();
                }
            }
        }

        /// <summary>
        /// Gets the vertical offset of the scrolled content.
        /// </summary>
        public Double VerticalOffset
        {
            get { return verticalOffset; }
            private set
            {
                if (verticalOffset != value)
                {
                    verticalOffset = value;
                    InvalidatePosition();
                }
            }
        }

        // Property values.
        private Double horizontalOffset;
        private Double verticalOffset;
    }
}
