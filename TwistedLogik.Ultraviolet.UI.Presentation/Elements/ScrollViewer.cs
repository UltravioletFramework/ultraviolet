using System;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a control which provides a scrollable view of its content.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.ScrollViewer.xml")]
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

        /// <summary>
        /// Gets the width of the content which is being displayed by the scroll viewer.
        /// </summary>
        public Double ExtentWidth
        {
            get { return (Presenter == null) ? 0 : Presenter.ExtentWidth; }
        }

        /// <summary>
        /// Gets the height of the content which is being displayed by the scroll viewer.
        /// </summary>
        public Double ExtentHeight
        {
            get { return (Presenter == null) ? 0 : Presenter.ExtentHeight; }
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
            get { return (Presenter == null) ? 0 : Presenter.ViewportWidth; }
        }

        /// <summary>
        /// Gets the height of the scroll viewer's viewport.
        /// </summary>
        public Double ViewportHeight
        {
            get { return (Presenter == null) ? 0 : Presenter.ViewportHeight; }
        }

        /// <summary>
        /// Gets the horizontal offset of the scrolled content.
        /// </summary>
        public Double HorizontalOffset
        {
            get { return HScroll == null ? 0 : HScroll.Value; }
        }

        /// <summary>
        /// Gets the vertical offset of the scrolled content.
        /// </summary>
        public Double VerticalOffset
        {
            get { return VScroll == null ? 0 : VScroll.Value; }
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

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (Presenter == null || HScroll == null || VScroll == null)
                return Size2D.Zero;

            var child = GetVisualChild(0);

            var hVisibility = HorizontalScrollBarVisibility;
            var vVisibility = VerticalScrollBarVisibility;

            HScroll.Visibility = (hVisibility == ScrollBarVisibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
            VScroll.Visibility = (vVisibility == ScrollBarVisibility.Visible) ? Visibility.Visible : Visibility.Collapsed;

            var hAuto = (hVisibility == ScrollBarVisibility.Auto);
            var vAuto = (vVisibility == ScrollBarVisibility.Auto);

            var hNoScroll = (hVisibility == ScrollBarVisibility.Disabled);
            var vNoScroll = (vVisibility == ScrollBarVisibility.Disabled);

            Presenter.CanScrollHorizontally = (hVisibility != ScrollBarVisibility.Disabled);
            Presenter.CanScrollVertically   = (vVisibility != ScrollBarVisibility.Disabled);

            child.Measure(availableSize);

            if (hAuto || vAuto)
            {
                var hAutoVisible = hAuto && Presenter.ExtentWidth > Presenter.ViewportWidth;
                if (hAutoVisible)
                {
                    HScroll.Visibility = Visibility.Visible;
                }

                var vAutoVisible = vAuto && Presenter.ExtentHeight > Presenter.ViewportHeight;
                if (vAutoVisible)
                {
                    VScroll.Visibility = Visibility.Visible;
                }

                if (hAutoVisible || vAutoVisible)
                {
                    child.InvalidateMeasure();
                    child.Measure(availableSize);
                }

                if (hAuto && vAuto && (hAutoVisible != vAutoVisible))
                {
                    hAutoVisible = !hAutoVisible && Presenter.ExtentWidth > Presenter.ViewportWidth;
                    if (hAutoVisible)
                    {
                        HScroll.Visibility = Visibility.Visible;
                    }

                    vAutoVisible = !vAutoVisible && Presenter.ExtentHeight > Presenter.ViewportHeight;
                    if (vAutoVisible)
                    {
                        VScroll.Visibility = Visibility.Visible;
                    }

                    if (hAutoVisible || vAutoVisible)
                    {
                        child.InvalidateMeasure();
                        child.Measure(availableSize);
                    }
                }
            }

            HScroll.Minimum = 0;
            VScroll.Minimum = 0;

            HScroll.Maximum = ScrollableWidth;
            VScroll.Maximum = ScrollableHeight;

            HScroll.ViewportSize = ViewportWidth;
            VScroll.ViewportSize = ViewportHeight;

            HScroll.IsEnabled = Presenter.CanScrollHorizontally && ScrollableWidth > 0;
            VScroll.IsEnabled = Presenter.CanScrollVertically && ScrollableHeight > 0;

            child.InvalidateMeasure();
            child.Measure(availableSize);

            return child.DesiredSize;
        }

        /// <inheritdoc/>
        protected override void OnMouseWheel(MouseDevice device, Double x, Double y, ref Boolean handled)
        {
            if (x != 0 && HScroll != null)
            {
                HScroll.Value += ScrollDeltaMouseWheel * x;
            }
            if (y != 0 && VScroll != null)
            {
                VScroll.Value += ScrollDeltaMouseWheel * -y;
            }
            handled = true;

            base.OnMouseWheel(device, x, y, ref handled);
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, KeyModifiers modifiers, ref Boolean handled)
        {
            switch (key)
            {
                case Key.Up:
                    VScroll.Value -= ScrollDeltaKey;
                    handled = true;
                    break;

                case Key.Down:
                    VScroll.Value += ScrollDeltaKey;
                    handled = true;
                    break;

                case Key.Left:
                    HScroll.Value -= ScrollDeltaKey;
                    handled = true;
                    break;

                case Key.Right:
                    HScroll.Value += ScrollDeltaKey;
                    handled = true;
                    break;
            }

            base.OnKeyDown(device, key, modifiers, ref handled);
        }

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

        // Scroll deltas for various input events.
        private const Double ScrollDeltaMouseWheel = 48.0;
        private const Double ScrollDeltaKey = 16.0;

        // Control component references.
        private readonly ScrollContentPresenter Presenter = null;
        private readonly HScrollBar HScroll = null;
        private readonly VScrollBar VScroll = null;
    }
}
