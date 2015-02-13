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

        /// <summary>
        /// Gets or sets a value specifying the visibility of the scroll viewer's horizontal scroll bar.
        /// </summary>
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return GetValue<ScrollBarVisibility>(HorizontalScrollBarVisibilityProperty); }
            set { SetValue<ScrollBarVisibility>(HorizontalScrollBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value specifying the visibility of the scroll viewer's vertical scroll bar.
        /// </summary>
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return GetValue<ScrollBarVisibility>(VerticalScrollBarVisibilityProperty); }
            set { SetValue<ScrollBarVisibility>(VerticalScrollBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the scroll viewer's horizontal scroll bar is visible.
        /// </summary>
        public Visibility ComputedHorizontalScrollBarVisibility
        {
            get 
            {
                switch (HorizontalScrollBarVisibility)
                {
                    case ScrollBarVisibility.Auto:
                        return ExtentWidth > ViewportWidth ? Visibility.Visible : Visibility.Collapsed;

                    case ScrollBarVisibility.Hidden:
                        return Visibility.Collapsed;
                }
                return Visibility.Visible; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether the scroll viewer's vertical scroll bar is visible.
        /// </summary>
        public Visibility ComputedVerticalScrollBarVisibility
        {
            get 
            {
                switch (VerticalScrollBarVisibility)
                {
                    case ScrollBarVisibility.Auto:
                        return ExtentHeight > ViewportHeight ? Visibility.Visible : Visibility.Collapsed;

                    case ScrollBarVisibility.Hidden:
                        return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalScrollBarVisibility"/> property changes.
        /// </summary>
        public event UIElementEventHandler HorizontalScrollBarVisibilityChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalScrollBarVisibility"/> property changes.
        /// </summary>
        public event UIElementEventHandler VerticalScrollBarVisibilityChanged;

        /// <summary>
        /// Identifies the <see cref="HorizontalScrollBarVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register("HorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(ScrollViewer),
            new DependencyPropertyMetadata(HandleHorizontalScrollBarVisibilityChanged, () => ScrollBarVisibility.Disabled, DependencyPropertyOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="VerticalScrollBarVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(ScrollViewer),
            new DependencyPropertyMetadata(HandleVerticalScrollBarVisibilityChanged, () => ScrollBarVisibility.Visible, DependencyPropertyOptions.AffectsArrange));

        /// <summary>
        /// Raises the <see cref="HorizontalScrollBarVisibilityChanged"/> event.
        /// </summary>
        protected virtual void OnHorizontalScrollBarVisibilityChanged()
        {
            var temp = HorizontalScrollBarVisibilityChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="VerticalScrollBarVisibilityChanged"/> event.
        /// </summary>
        protected virtual void OnVerticalScrollBarVisibilityChanged()
        {
            var temp = VerticalScrollBarVisibilityChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the scroll viewer's vertical scroll bar is enabled.
        /// </summary>
        protected Boolean ComputedVerticalScrollBarEnabled
        {
            get 
            { 
                return ComputedVerticalScrollBarVisibility == Visibility.Visible &&
                    VerticalScrollBarVisibility != ScrollBarVisibility.Disabled; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether the scroll viewer's horizontal scroll bar is enabled.
        /// </summary>
        protected Boolean ComputedHorizontalScrollBarEnabled
        {
            get
            {
                return ComputedHorizontalScrollBarVisibility == Visibility.Visible &&
                    HorizontalScrollBarVisibility != ScrollBarVisibility.Disabled; 
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalScrollBarVisibility"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleHorizontalScrollBarVisibilityChanged(DependencyObject dobj)
        {
            var scrollViewer = (ScrollViewer)dobj;
            scrollViewer.OnHorizontalScrollBarVisibilityChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalScrollBarVisibility"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleVerticalScrollBarVisibilityChanged(DependencyObject dobj)
        {
            var scrollViewer = (ScrollViewer)dobj;
            scrollViewer.OnVerticalScrollBarVisibilityChanged();
        }

        // Property values.
        private Double horizontalOffset;
        private Double verticalOffset;
    }
}
