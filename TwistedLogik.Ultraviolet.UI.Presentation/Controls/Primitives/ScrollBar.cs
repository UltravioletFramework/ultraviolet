using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents a scroll bar with a sliding thumb.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives.Templates.ScrollBar.xml")]
    public class ScrollBar : RangeBase
    {
        /// <summary>
        /// Initializes the <see cref="ScrollBar"/> type.
        /// </summary>
        static ScrollBar()
        {
            ValueProperty.OverrideMetadata(typeof(ScrollBar), new PropertyMetadata<Double>(HandleValueChanged));
            MinimumProperty.OverrideMetadata(typeof(ScrollBar), new PropertyMetadata<Double>(HandleMinimumChanged));
            MaximumProperty.OverrideMetadata(typeof(ScrollBar), new PropertyMetadata<Double>(HandleMaximumChanged));
            SmallChangeProperty.OverrideMetadata(typeof(ScrollBar), new PropertyMetadata<Double>(HandleSmallChangeChanged));
            LargeChangeProperty.OverrideMetadata(typeof(ScrollBar), new PropertyMetadata<Double>(HandleLargeChangeChanged));

            EventManager.RegisterClassHandler(typeof(ScrollBar), ScrollEvent, new UpfScrollEventHandler(HandleScrollEvent));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBar"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ScrollBar(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets the scroll bar's track.
        /// </summary>
        public Track Track
        {
            get
            {
                if (Orientation == Orientation.Horizontal)
                {
                    return PART_HScrollBar?.Track;
                }
                else
                {
                    return PART_VScrollBar?.Track;
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of scrollable content that is currently visible.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the amount of scrollable content that is currently
        /// visible in device-independent pixels.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="ViewportSizeProperty"/></dpropField>
        ///		<dpropStylingName>viewport-size</dpropStylingName>
        ///		<dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double ViewportSize
        {
            get { return GetValue<Double>(ViewportSizeProperty); }
            set { SetValue(ViewportSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies whether the scroll bar is oriented vertically or horizontally.
        /// </summary>
        /// <value>An <see cref="Orientation"/> that specifies whether the scroll bar is oriented vertically or horizontally.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="OrientationProperty"/></dpropField>
        ///		<dpropStylingName>orientation</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Orientation Orientation
        {
            get { return GetValue<Orientation>(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Occurs when the scroll bar's content is scrolled as a result of the user moving the thumb.
        /// </summary>
        /// <remarks>
        /// <para>This event is not raised when the scroll bar's value is changed programatically.</para>
        /// <revt>
        ///		<revtField><see cref="ScrollEvent"/></revtField>
        ///		<revtStylingName>scroll</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfScrollEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfScrollEventHandler Scroll
        {
            add { AddHandler(ScrollEvent, value); }
            remove { RemoveHandler(ScrollEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ViewportSize"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ViewportSize"/> dependency property.</value>
        public static readonly DependencyProperty ViewportSizeProperty = DependencyProperty.Register("ViewportSize", typeof(Double), typeof(ScrollBar), 
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.AffectsMeasure, HandleViewportSizeChanged));

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Orientation"/> dependency property.</value>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ScrollBar),
            new PropertyMetadata<Orientation>(Orientation.Vertical, HandleOrientationChanged));

        /// <summary>
        /// Identifies the <see cref="Scroll"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="Scroll"/> routed event.</value>
        public static readonly RoutedEvent ScrollEvent = EventManager.RegisterRoutedEvent("Scroll", RoutingStrategy.Bubble,
            typeof(UpfScrollEventHandler), typeof(ScrollBar));

        /// <summary>
        /// Called when the <see cref="RangeBase.Value"/> property of one of the scroll bar's child scroll bars changes.
        /// </summary>
        internal void OnChildValueChanged(ScrollBarBase child, Double value)
        {
            if (Orientation == Orientation.Horizontal)
            {
                if (child == PART_HScrollBar)
                {
                    Value = value;
                }
            }
            else
            {
                if (child == PART_VScrollBar)
                {
                    Value = value;
                }
            }
        }

        /// <summary>
        /// Called when the <see cref="RangeBase.Minimum"/> property of one of the scroll bar's child scroll bars changes.
        /// </summary>
        internal void OnChildMinimumChanged(ScrollBarBase child, Double value)
        {
            if (Orientation == Orientation.Horizontal)
            {
                if (child == PART_HScrollBar)
                {
                    Minimum = value;
                }
            }
            else
            {
                if (child == PART_VScrollBar)
                {
                    Minimum = value;
                }
            }
        }

        /// <summary>
        /// Called when the <see cref="RangeBase.Maximum"/> property of one of the scroll bar's child scroll bars changes.
        /// </summary>
        internal void OnChildMaximumChanged(ScrollBarBase child, Double value)
        {
            if (Orientation == Orientation.Horizontal)
            {
                if (child == PART_HScrollBar)
                {
                    Maximum = value;
                }
            }
            else
            {
                if (child == PART_VScrollBar)
                {
                    Maximum = value;
                }
            }
        }

        /// <summary>
        /// Called when the <see cref="RangeBase.SmallChange"/> property of one of the scroll bar's child scroll bars changes.
        /// </summary>
        internal void OnChildSmallChangeChanged(ScrollBarBase child, Double value)
        {
            if (Orientation == Orientation.Horizontal)
            {
                if (child == PART_HScrollBar)
                {
                    SmallChange = value;
                }
            }
            else
            {
                if (child == PART_VScrollBar)
                {
                    SmallChange = value;
                }
            }
        }

        /// <summary>
        /// Called when the <see cref="RangeBase.LargeChange"/> property of one of the scroll bar's child scroll bars changes.
        /// </summary>
        internal void OnChildLargeChangeChanged(ScrollBarBase child, Double value)
        {
            if (Orientation == Orientation.Horizontal)
            {
                if (child == PART_HScrollBar)
                {
                    LargeChange = value;
                }
            }
            else
            {
                if (child == PART_VScrollBar)
                {
                    LargeChange = value;
                }
            }
        }

        /// <summary>
        /// Called when the <see cref="ScrollBarBase.ViewportSize"/> property of one of the scroll bar's child scroll bars changes.
        /// </summary>
        internal void OnChildViewportSizeChanged(ScrollBarBase child, Double value)
        {
            if (Orientation == Orientation.Horizontal)
            {
                if (child == PART_HScrollBar)
                {
                    ViewportSize = value;
                }
            }
            else
            {
                if (child == PART_VScrollBar)
                {
                    ViewportSize = value;
                }
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RangeBase.Value"/> dependency property changes.
        /// </summary>
        private static void HandleValueChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            if (!(element is ScrollBar))
                return;

            var hscroll = ((ScrollBar)element).PART_HScrollBar;
            if (hscroll != null)
                hscroll.Value = newValue;

            var vscroll = ((ScrollBar)element).PART_VScrollBar;
            if (vscroll != null)
                vscroll.Value = newValue;
        }
        
        /// <summary>
        /// Occurs when the value of the <see cref="RangeBase.Minimum"/> dependency property changes.
        /// </summary>
        private static void HandleMinimumChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            if (!(element is ScrollBar))
                return;

            var hscroll = ((ScrollBar)element).PART_HScrollBar;
            if (hscroll != null)
                hscroll.Minimum = newValue;

            var vscroll = ((ScrollBar)element).PART_VScrollBar;
            if (vscroll != null)
                vscroll.Minimum = newValue;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RangeBase.Maximum"/> dependency property changes.
        /// </summary>
        private static void HandleMaximumChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            if (!(element is ScrollBar))
                return;

            var hscroll = ((ScrollBar)element).PART_HScrollBar;
            if (hscroll != null)
                hscroll.Maximum = newValue;

            var vscroll = ((ScrollBar)element).PART_VScrollBar;
            if (vscroll != null)
                vscroll.Maximum = newValue;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RangeBase.SmallChange"/> dependency property changes.
        /// </summary>
        private static void HandleSmallChangeChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            if (!(element is ScrollBar))
                return;

            var hscroll = ((ScrollBar)element).PART_HScrollBar;
            if (hscroll != null)
                hscroll.SmallChange = newValue;

            var vscroll = ((ScrollBar)element).PART_VScrollBar;
            if (vscroll != null)
                vscroll.SmallChange = newValue;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RangeBase.LargeChange"/> dependency property changes.
        /// </summary>
        private static void HandleLargeChangeChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            if (!(element is ScrollBar))
                return;

            var hscroll = ((ScrollBar)element).PART_HScrollBar;
            if (hscroll != null)
                hscroll.LargeChange = newValue;

            var vscroll = ((ScrollBar)element).PART_VScrollBar;
            if (vscroll != null)
                vscroll.LargeChange = newValue;
        }
        /// <summary>
        /// Occurs when the value of the <see cref="ViewportSize"/> dependency property changes.
        /// </summary>
        private static void HandleViewportSizeChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            if (!(element is ScrollBar))
                return;

            var hscroll = ((ScrollBar)element).PART_HScrollBar;
            if (hscroll != null)
                hscroll.ViewportSize = newValue;

            var vscroll = ((ScrollBar)element).PART_VScrollBar;
            if (vscroll != null)
                vscroll.ViewportSize = newValue;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation"/> dependency property changes.
        /// </summary>
        private static void HandleOrientationChanged(DependencyObject element, Orientation oldValue, Orientation newValue)
        {
            if (!(element is ScrollBar))
                return;

            ((ScrollBar)element).ChangeOrientation(newValue);
        }

        /// <summary>
        /// Handles <see cref="Scroll"/> events raised by the scroll bar's child scroll bars.
        /// </summary>
        private static void HandleScrollEvent(DependencyObject element, ScrollEventType type, RoutedEventData data)
        {
            var scrollbar = (ScrollBar)element;
            var hscroll = scrollbar.PART_HScrollBar;
            var vscroll = scrollbar.PART_VScrollBar;
            if (data.OriginalSource == hscroll || data.OriginalSource == vscroll)
            {
                // Re-raise the event with this control as the source
                var evtData = RoutedEventData.Retrieve(scrollbar);
                var evtDelegate = EventManager.GetInvocationDelegate<UpfScrollEventHandler>(ScrollEvent);
                evtDelegate(scrollbar, type, evtData);

                data.Handled = true;
            }
        }

        /// <summary>
        /// Switches the scroll bar to the specified orientation.
        /// </summary>
        private void ChangeOrientation(Orientation orientation)
        {
            if (orientation == Orientation.Horizontal)
            {
                if (PART_HScrollBar != null)
                {
                    PART_HScrollBar.Visibility = Visibility.Visible;
                    PART_HScrollBar.IsEnabled = true;
                }
                if (PART_VScrollBar != null)
                {
                    PART_VScrollBar.Visibility = Visibility.Collapsed;
                    PART_VScrollBar.IsEnabled = false;
                }
                this.ViewportSize = PART_HScrollBar?.ViewportSize ?? 0;
            }
            else
            {
                if (PART_HScrollBar != null)
                {
                    PART_HScrollBar.Visibility = Visibility.Collapsed;
                    PART_HScrollBar.IsEnabled = false;
                }
                if (PART_VScrollBar != null)
                {
                    PART_VScrollBar.Visibility = Visibility.Visible;
                    PART_VScrollBar.IsEnabled = true;
                }
                this.ViewportSize = PART_VScrollBar?.ViewportSize ?? 0;
            }
        }
        
        // Component references.
        private readonly HScrollBar PART_HScrollBar = null;
        private readonly VScrollBar PART_VScrollBar = null;
    }
}
