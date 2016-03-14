using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents the method that is called in response to a <see cref="ScrollViewer.ScrollChanged"/> routed event.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="scrollInfo">A <see cref="ScrollChangedInfo"/> structure that describes the changes to the scrolling state.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfScrollChangedEventHandler(DependencyObject element, ref ScrollChangedInfo scrollInfo, ref RoutedEventData data);

    /// <summary>
    /// Represents a control which provides a scrollable view of its content.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.ScrollViewer.xml")]
    public class ScrollViewer : ContentControl
    {
        /// <summary>
        /// Initializes the <see cref="ScrollViewer"/> type.
        /// </summary>
        static ScrollViewer()
        {
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ScrollViewer), new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Local));
            KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(ScrollViewer), new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));

            EventManager.RegisterClassHandler(typeof(ScrollViewer), RangeBase.ValueChangedEvent, new UpfRoutedEventHandler(HandleScrollBarValueChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ScrollViewer(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Scrolls the viewer's content up by one line.
        /// </summary>
        public void LineUp()
        {
            ScrollToVerticalOffset(VerticalOffset - ScrollDeltaKey);
        }

        /// <summary>
        /// Scrolls the viewer's content down by one line.
        /// </summary>
        public void LineDown()
        {
            ScrollToVerticalOffset(VerticalOffset + ScrollDeltaKey);
        }

        /// <summary>
        /// Scrolls the viewer's content left by one line.
        /// </summary>
        public void LineLeft()
        {
            ScrollToHorizontalOffset(HorizontalOffset - ScrollDeltaKey);
        }

        /// <summary>
        /// Scrolls the viewer's content right by one line.
        /// </summary>
        public void LineRight()
        {
            ScrollToHorizontalOffset(HorizontalOffset + ScrollDeltaKey);
        }

        /// <summary>
        /// Scrolls the viewer's content up by one page.
        /// </summary>
        public void PageUp()
        {
            ScrollToVerticalOffset(VerticalOffset - ViewportHeight);
        }

        /// <summary>
        /// Scrolls the viewer's content down by one page.
        /// </summary>
        public void PageDown()
        {
            ScrollToVerticalOffset(VerticalOffset + ViewportHeight);
        }

        /// <summary>
        /// Scrolls the viewer's content left by one page.
        /// </summary>
        public void PageLeft()
        {
            ScrollToHorizontalOffset(HorizontalOffset - ViewportWidth);
        }

        /// <summary>
        /// Scrolls the viewer's content right by one page.
        /// </summary>
        public void PageRight()
        {
            ScrollToHorizontalOffset(HorizontalOffset + ViewportWidth);
        }

        /// <summary>
        /// Scrolls the viewer to the beginning of its content.
        /// </summary>
        public void ScrollToHome()
        {
            ScrollToHorizontalOffset(Double.NegativeInfinity);
            ScrollToVerticalOffset(Double.NegativeInfinity);
        }

        /// <summary>
        /// Scrolls the viewer to the end of its content.
        /// </summary>
        public void ScrollToEnd()
        {
            ScrollToHorizontalOffset(Double.PositiveInfinity);
            ScrollToVerticalOffset(Double.PositiveInfinity);
        }

        /// <summary>
        /// Moves the scroll viewer to the specified horizontal offset.
        /// </summary>
        /// <param name="offset">The horizontal offset to which to move the scroll viewer.</param>
        public void ScrollToHorizontalOffset(Double offset)
        {
            if (PART_HScroll == null)
                return;

            PART_HScroll.Value = offset;
        }

        /// <summary>
        /// Moves the scroll viewer to the specified vertical offset.
        /// </summary>
        /// <param name="offset">The vertical offset to which to move the scroll viewer.</param>
        public void ScrollToVerticalOffset(Double offset)
        {
            if (PART_VScroll == null)
                return;

            PART_VScroll.Value = offset;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this scroll viewer applies a clipping rectangle to its content pane.
        /// </summary>
        /// <value><see langword="true"/> if the scroll viewer's content is clipped; otherwise,
        /// <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ContentClippedProperty"/></dpropField>
        ///     <dpropStylingName>content-clipped</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean ContentClipped
        {
            get { return GetValue<Boolean>(ContentClippedProperty); }
            set { SetValue(ContentClippedProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets the margin which is applied to the scroll viewer's content pane.
        /// </summary>
        /// <value>A <see cref="Thickness"/> value which represents the margin that is applied
        /// to the scroll viewer's content pane.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ContentMarginProperty"/></dpropField>
        ///     <dpropStylingName>content-margin</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Thickness ContentMargin
        {
            get { return GetValue<Thickness>(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets a value specifying the visibility of the scroll viewer's horizontal scroll bar.
        /// </summary>
        /// <value>A <see cref="ScrollBarVisibility"/> value that specifies the visibility of the scroll
        /// viewer's horizontal scroll bar. The default value is <see cref="ScrollBarVisibility.Disabled"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="HorizontalScrollBarVisibilityProperty"/></dpropField>
        ///     <dpropStylingName>hscrollbar-visibility</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return GetValue<ScrollBarVisibility>(HorizontalScrollBarVisibilityProperty); }
            set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value specifying the visibility of the scroll viewer's vertical scroll bar.
        /// </summary>
        /// <value>A <see cref="ScrollBarVisibility"/> value that specifies the visibility of the scroll
        /// viewer's vertical scroll bar. The default value is <see cref="ScrollBarVisibility.Disabled"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="VerticalScrollBarVisibilityProperty"/></dpropField>
        ///     <dpropStylingName>vscrollbar-visibility</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return GetValue<ScrollBarVisibility>(VerticalScrollBarVisibilityProperty); }
            set { SetValue(VerticalScrollBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets the width of the content which is being displayed by the scroll viewer.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the width in device-independent
        /// pixels of the content which is being displayed by the scroll viewer.</value>
        public Double ExtentWidth
        {
            get { return (PART_ContentPresenter == null) ? 0 : PART_ContentPresenter.ExtentWidth; }
        }

        /// <summary>
        /// Gets the height of the content which is being displayed by the scroll viewer.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the height in device-independent
        /// pixels of the content which is being displayed by the scroll viewer.</value>
        public Double ExtentHeight
        {
            get { return (PART_ContentPresenter == null) ? 0 : PART_ContentPresenter.ExtentHeight; }
        }

        /// <summary>
        /// Gets the width of the scroll viewer's scrollable area.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the width in device-independent
        /// pixels of the scroll viewer's scrollable area.</value>
        public Double ScrollableWidth
        {
            get { return ExtentWidth - ViewportWidth; }
        }

        /// <summary>
        /// Gets the height of the scroll viewer's scrollable area.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the height in device-independent
        /// pixels of the scroll viewer's scrollable area.</value>
        public Double ScrollableHeight
        {
            get { return ExtentHeight - ViewportHeight; }
        }

        /// <summary>
        /// Gets the width of the scroll viewer's viewport.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the width in device-independent
        /// pixels of the scroll viewer's viewport.</value>
        public Double ViewportWidth
        {
            get { return (PART_ContentPresenter == null) ? 0 : PART_ContentPresenter.ViewportWidth; }
        }

        /// <summary>
        /// Gets the height of the scroll viewer's viewport.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the height in device-independent
        /// pixels of the scroll viewer's viewport.</value>
        public Double ViewportHeight
        {
            get { return (PART_ContentPresenter == null) ? 0 : PART_ContentPresenter.ViewportHeight; }
        }

        /// <summary>
        /// Gets the horizontal offset of the scrolled content.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the horizontal offset
        /// of the scrolled content in device-independent pixels.</value>
        public Double HorizontalOffset
        {
            get { return PART_HScroll == null || !PART_HScroll.IsEnabled ? 0 : PART_HScroll.Value; }
        }

        /// <summary>
        /// Gets the vertical offset of the scrolled content.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the vertical offset
        /// of the scrolled content in device-independent pixels.</value>
        public Double VerticalOffset
        {
            get { return PART_VScroll == null || !PART_VScroll.IsEnabled ? 0 : PART_VScroll.Value; }
        }

        /// <summary>
        /// Occurs when the scroll viewer's scrolling properties are changed.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="ScrollChangedEvent"/></revtField>
        ///     <revtStylingName>scroll-changed</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfScrollChangedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfScrollChangedEventHandler ScrollChanged
        {
            add { AddHandler(ScrollChangedEvent, value); }
            remove { RemoveHandler(ScrollChangedEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ContentClipped"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ContentClipped"/> dependency property.</value>
        public static readonly DependencyProperty ContentClippedProperty = DependencyProperty.Register("ContentClipped", typeof(Boolean), typeof(ScrollViewer),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleContentClippedChanged));

        /// <summary>
        /// Identifies the <see cref="ContentMargin"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ContentMargin"/> dependency property.</value>
        public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(ScrollViewer),
            new PropertyMetadata<Thickness>(PresentationBoxedValues.Thickness.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="HorizontalScrollBarVisibility"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="HorizontalScrollBarVisibility"/> dependency property.</value>
        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register("HorizontalScrollBarVisibility", "hscrollbar-visibility", typeof(ScrollBarVisibility), typeof(ScrollViewer),
            new PropertyMetadata<ScrollBarVisibility>(PresentationBoxedValues.ScrollBarVisibility.Disabled, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="VerticalScrollBarVisibility"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="VerticalScrollBarVisibility"/> dependency property.</value>
        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register("VerticalScrollBarVisibility", "vscrollbar-visibility", typeof(ScrollBarVisibility), typeof(ScrollViewer),
            new PropertyMetadata<ScrollBarVisibility>(PresentationBoxedValues.ScrollBarVisibility.Visible, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="ScrollChanged"/> event.
        /// </summary>
        /// <value>The identifier for the <see cref="ScrollChanged"/> routed event.</value>
        public static readonly RoutedEvent ScrollChangedEvent = EventManager.RegisterRoutedEvent("ScrollChanged", RoutingStrategy.Bubble, 
            typeof(UpfScrollChangedEventHandler), typeof(ScrollViewer));

        /// <summary>
        /// Scrolls in response to keyboard input.
        /// </summary>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="modifiers">A <see cref="ModifierKeys"/> value indicating which of the key modifiers are currently active.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        internal void HandleKeyScrolling(Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            switch (key)
            {
                case Key.Up:
                    if (PART_VScroll.Value > PART_VScroll.Minimum)
                    {
                        PART_VScroll.Value -= ScrollDeltaKey;
                        data.Handled = true;
                    }
                    break;

                case Key.Down:
                    if (PART_VScroll.Value < PART_VScroll.Maximum)
                    {
                        PART_VScroll.Value += ScrollDeltaKey;
                        data.Handled = true;
                    }
                    break;

                case Key.Left:
                    if (PART_HScroll.Value > PART_HScroll.Minimum)
                    {
                        PART_HScroll.Value -= ScrollDeltaKey;
                        data.Handled = true;
                    }
                    break;

                case Key.Right:
                    if (PART_HScroll.Value < PART_HScroll.Maximum)
                    {
                        PART_HScroll.Value += ScrollDeltaKey;
                        data.Handled = true;
                    }
                    break;
            }
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (PART_ContentPresenter == null || PART_HScroll == null || PART_VScroll == null)
                return Size2D.Zero;

            var child = GetVisualChild(0);

            var hVisibility = HorizontalScrollBarVisibility;
            var vVisibility = VerticalScrollBarVisibility;

            PART_HScroll.Visibility = (hVisibility == ScrollBarVisibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
            PART_VScroll.Visibility = (vVisibility == ScrollBarVisibility.Visible) ? Visibility.Visible : Visibility.Collapsed;

            var hAuto = (hVisibility == ScrollBarVisibility.Auto);
            var vAuto = (vVisibility == ScrollBarVisibility.Auto);

            PART_ContentPresenter.CanScrollHorizontally = (hVisibility != ScrollBarVisibility.Disabled);
            PART_ContentPresenter.CanScrollVertically   = (vVisibility != ScrollBarVisibility.Disabled);

            var availableSizeSansMargins = availableSize - Margin;
            child.Measure(availableSizeSansMargins);

            if (hAuto || vAuto)
            {
                var hAutoVisible = hAuto && PART_ContentPresenter.ExtentWidth > PART_ContentPresenter.ViewportWidth;
                if (hAutoVisible)
                {
                    PART_HScroll.Visibility = Visibility.Visible;
                }

                var vAutoVisible = vAuto && PART_ContentPresenter.ExtentHeight > PART_ContentPresenter.ViewportHeight;
                if (vAutoVisible)
                {
                    PART_VScroll.Visibility = Visibility.Visible;
                }

                if (hAutoVisible || vAutoVisible)
                {
                    child.InvalidateMeasure();
                    child.Measure(availableSizeSansMargins);
                }

                if (hAuto && vAuto && (hAutoVisible != vAutoVisible))
                {
                    hAutoVisible = !hAutoVisible && PART_ContentPresenter.ExtentWidth > PART_ContentPresenter.ViewportWidth;
                    if (hAutoVisible)
                    {
                        PART_HScroll.Visibility = Visibility.Visible;
                    }

                    vAutoVisible = !vAutoVisible && PART_ContentPresenter.ExtentHeight > PART_ContentPresenter.ViewportHeight;
                    if (vAutoVisible)
                    {
                        PART_VScroll.Visibility = Visibility.Visible;
                    }

                    if (hAutoVisible || vAutoVisible)
                    {
                        child.InvalidateMeasure();
                        child.Measure(availableSizeSansMargins);
                    }
                }
            }

            PART_HScroll.Minimum = 0;
            PART_VScroll.Minimum = 0;

            PART_HScroll.Maximum = ScrollableWidth;
            PART_VScroll.Maximum = ScrollableHeight;

            PART_HScroll.ViewportSize = ViewportWidth;
            PART_VScroll.ViewportSize = ViewportHeight;

            PART_HScroll.IsEnabled = PART_ContentPresenter.CanScrollHorizontally && ScrollableWidth > 0;
            PART_VScroll.IsEnabled = PART_ContentPresenter.CanScrollVertically && ScrollableHeight > 0;

            child.InvalidateMeasure();
            child.Measure(availableSizeSansMargins);

            return child.DesiredSize;
        }

        /// <inheritdoc/>
        protected override void PositionOverride()
        {
            if (PART_ContentPresenter != null)
                PART_ContentPresenter.PositionChildren();

            var newHorizontalOffset = HorizontalOffset;
            var newVerticalOffset = VerticalOffset;
            var newExtentWidth = ExtentWidth;
            var newExtentHeight = ExtentHeight;
            var newViewportWidth = ViewportWidth;
            var newViewportHeight = ViewportHeight;

            var scrollChanged =
                !MathUtil.AreApproximatelyEqual(oldHorizontalOffset, newHorizontalOffset) ||
                !MathUtil.AreApproximatelyEqual(oldVerticalOffset, newVerticalOffset) ||
                !MathUtil.AreApproximatelyEqual(oldExtentWidth, newExtentWidth) ||
                !MathUtil.AreApproximatelyEqual(oldExtentHeight, newExtentHeight) ||
                !MathUtil.AreApproximatelyEqual(oldViewportWidth, newViewportWidth) ||
                !MathUtil.AreApproximatelyEqual(oldViewportHeight, newViewportHeight);

            if (scrollChanged)
            {
                var evtDelegate = EventManager.GetInvocationDelegate<UpfScrollChangedEventHandler>(ScrollChangedEvent);
                var evtData = new RoutedEventData(this);

                var scrollInfo = new ScrollChangedInfo(
                    newHorizontalOffset, newHorizontalOffset - oldHorizontalOffset,
                    newVerticalOffset, newVerticalOffset - oldVerticalOffset,
                    newExtentWidth, newExtentWidth - oldExtentWidth,
                    newExtentHeight, newExtentHeight - oldExtentHeight,
                    newViewportWidth, newViewportWidth - oldViewportWidth,
                    newViewportHeight, newViewportHeight - oldViewportHeight);

                oldHorizontalOffset = newHorizontalOffset;
                oldVerticalOffset = newVerticalOffset;
                oldExtentWidth = newExtentWidth;
                oldExtentHeight = newExtentHeight;
                oldViewportWidth = newViewportWidth;
                oldViewportHeight = newViewportHeight;

                evtDelegate(this, ref scrollInfo, ref evtData);
            }

            base.PositionOverride();
        }

        /// <inheritdoc/>
        protected override void OnMouseWheel(MouseDevice device, Double x, Double y, ref RoutedEventData data)
        {
            if (!data.Handled)
            {
                if (x != 0 && PART_HScroll != null)
                {
                    PART_HScroll.Value += ScrollDeltaMouseWheel * x;
                }
                if (y != 0 && PART_VScroll != null)
                {
                    PART_VScroll.Value += ScrollDeltaMouseWheel * -y;
                }
                data.Handled = true;
            }
            base.OnMouseWheel(device, x, y, ref data);
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            var templatedParent = TemplatedParent as Control;
            if (templatedParent == null || !templatedParent.HandlesScrolling)
            {
                HandleKeyScrolling(key, modifiers, ref data);
            }

            base.OnKeyDown(device, key, modifiers, ref data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadAxisDown(GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, ref RoutedEventData data)
        {
            var templatedParent = TemplatedParent as Control;
            if (templatedParent == null || !templatedParent.HandlesScrolling)
            {
                if (GamePad.UseAxisForDirectionalNavigation)
                {
                    var direction = device.GetJoystickDirectionFromAxis(axis);
                    switch (direction)
                    {
                        case GamePadJoystickDirection.Up:
                            HandleKeyScrolling(Key.Up, ModifierKeys.None, ref data);
                            break;

                        case GamePadJoystickDirection.Down:
                            HandleKeyScrolling(Key.Down, ModifierKeys.None, ref data);
                            break;

                        case GamePadJoystickDirection.Left:
                            HandleKeyScrolling(Key.Left, ModifierKeys.None, ref data);
                            break;

                        case GamePadJoystickDirection.Right:
                            HandleKeyScrolling(Key.Right, ModifierKeys.None, ref data);
                            break;
                    }
                    data.Handled = true;
                }
            }
            
            base.OnGamePadAxisDown(device, axis, value, repeat, ref data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, ref RoutedEventData data)
        {
            var templatedParent = TemplatedParent as Control;
            if (templatedParent == null || !templatedParent.HandlesScrolling)
            {
                if (!GamePad.UseAxisForDirectionalNavigation)
                {
                    switch (button)
                    {
                        case GamePadButton.DPadUp:
                            HandleKeyScrolling(Key.Up, ModifierKeys.None, ref data);
                            break;

                        case GamePadButton.DPadDown:
                            HandleKeyScrolling(Key.Down, ModifierKeys.None, ref data);
                            break;

                        case GamePadButton.DPadLeft:
                            HandleKeyScrolling(Key.Left, ModifierKeys.None, ref data);
                            break;

                        case GamePadButton.DPadRight:
                            HandleKeyScrolling(Key.Right, ModifierKeys.None, ref data);
                            break;
                    }
                    data.Handled = true;
                }
            }

            base.OnGamePadButtonDown(device, button, repeat, ref data);
        }

        /// <inheritdoc/>
        protected override void OnFingerMotion(TouchDevice device, Int64 fingerID, Double x, Double y, Double dx, Double dy, Single pressure, ref RoutedEventData data)
        {
            if (!data.Handled && fingerID == 0)
            {
                if (dx != 0 && PART_HScroll != null)
                {
                    PART_HScroll.Value -= dx;
                }
                if (dy != 0 && PART_VScroll != null)
                {
                    PART_VScroll.Value -= dy;
                }
                data.Handled = true;
            }
            base.OnFingerMotion(device, fingerID, x, y, dx, dy, pressure, ref data);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ContentClipped"/> dependency property changes.
        /// </summary>
        private static void HandleContentClippedChanged(DependencyObject element, Boolean oldValue, Boolean newValue)
        {
            var scrollViewer = (ScrollViewer)element;
            if (scrollViewer.PART_ContentPresenter != null)
                scrollViewer.PART_ContentPresenter.Clip();
        }

        /// <summary>
        /// Handles the <see cref="RangeBase.ValueChanged"/> event for the scroll viewer's scroll bars.
        /// </summary>
        private static void HandleScrollBarValueChanged(DependencyObject element, ref RoutedEventData data)
        {
            var scrollViewer = (ScrollViewer)element;
            if (scrollViewer.PART_HScroll == data.OriginalSource || scrollViewer.PART_VScroll == data.OriginalSource)
            {
                scrollViewer.Position(scrollViewer.MostRecentPositionOffset);
                data.Handled = true;
            }            
        }

        // Scroll deltas for various input events.
        private const Double ScrollDeltaMouseWheel = 48.0;
        private const Double ScrollDeltaKey = 16.0;

        // Tracks the most recent scroll info values for ScrollChanged events.
        private Double oldHorizontalOffset;
        private Double oldVerticalOffset;
        private Double oldExtentWidth;
        private Double oldExtentHeight;
        private Double oldViewportWidth;
        private Double oldViewportHeight;

        // Control component references.
        private readonly ScrollContentPresenter PART_ContentPresenter = null;
        private readonly HScrollBar PART_HScroll = null;
        private readonly VScrollBar PART_VScroll = null;
    }
}
