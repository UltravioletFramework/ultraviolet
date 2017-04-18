using System;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the method that handles a <see cref="ScrollBar.Scroll"/> event.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="type">The scroll event type.</param>
    /// <param name="data">The routed event data.</param>
    public delegate void UpfScrollEventHandler(DependencyObject element, ScrollEventType type, RoutedEventData data);

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
            // Dependency property overrides
            ValueProperty.OverrideMetadata(typeof(ScrollBar), new PropertyMetadata<Double>(HandleValueChanged));
            MinimumProperty.OverrideMetadata(typeof(ScrollBar), new PropertyMetadata<Double>(HandleMinimumChanged));
            MaximumProperty.OverrideMetadata(typeof(ScrollBar), new PropertyMetadata<Double>(HandleMaximumChanged));
            SmallChangeProperty.OverrideMetadata(typeof(ScrollBar), new PropertyMetadata<Double>(HandleSmallChangeChanged));
            LargeChangeProperty.OverrideMetadata(typeof(ScrollBar), new PropertyMetadata<Double>(HandleLargeChangeChanged));

            // Event handlers
            EventManager.RegisterClassHandler(typeof(ScrollBar), ScrollEvent, new UpfScrollEventHandler(HandleScrollEvent));

            // Commands - vertical scroll
            CommandManager.RegisterClassBindings(typeof(ScrollBar), LineDownCommand, ExecutedScrollCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Down, ModifierKeys.None, "Down"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), LineUpCommand, ExecutedScrollCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Up, ModifierKeys.None, "Up"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), PageDownCommand, ExecutedScrollCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.PageDown, ModifierKeys.None, "PageDown"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), PageUpCommand, ExecutedScrollCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.PageUp, ModifierKeys.None, "PageUp"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollToBottomCommand, ExecutedScrollCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.End, ModifierKeys.Control, "Ctrl+End"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollToTopCommand, ExecutedScrollCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Home, ModifierKeys.Control, "Ctrl+Home"));

            // Commands - horizontal scroll
            CommandManager.RegisterClassBindings(typeof(ScrollBar), LineRightCommand, ExecutedScrollCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Right, ModifierKeys.None, "Right"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), LineLeftCommand, ExecutedScrollCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Left, ModifierKeys.None, "Left"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), PageRightCommand, ExecutedScrollCommand, CanExecuteScrollCommand,
                null);
            CommandManager.RegisterClassBindings(typeof(ScrollBar), PageLeftCommand, ExecutedScrollCommand, CanExecuteScrollCommand,
                null);
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollToRightEndCommand, ExecutedScrollCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.End, ModifierKeys.None, "End"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollToLeftEndCommand, ExecutedScrollCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Home, ModifierKeys.None, "Home"));

            // Commands - misc
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollHereCommand, ExecutedScrollCommand, CanExecuteScrollCommand,
                null);
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
        /// A command that is executed when the scroll bar's thumb is being dragged to the horizontal offset specified in the command parameter.
        /// </summary>
        public static readonly RoutedCommand DeferScrollToHorizontalOffsetCommand = new RoutedCommand("DeferScrollToHorizontalOffset", typeof(ScrollBar));

        /// <summary>
        /// A command that is executed when the scroll bar's thumb is being dragged to the vertical offset specified in the command parameter.
        /// </summary>
        public static readonly RoutedCommand DeferScrollToVerticalOffsetCommand = new RoutedCommand("DeferScrollToVerticalOffset", typeof(ScrollBar));

        /// <summary>
        /// A command that increases the value of the scroll bar by a small amount in the vertical direction.
        /// </summary>
        public static readonly RoutedCommand LineDownCommand = new RoutedCommand("LineDown", typeof(ScrollBar));

        /// <summary>
        /// A command that decreases the value of the scroll bar by a small amount in the vertical direction.
        /// </summary>
        public static readonly RoutedCommand LineUpCommand = new RoutedCommand("LineUp", typeof(ScrollBar));

        /// <summary>
        /// A command that increases the value of the scroll bar by a small amount in the horizontal direction.
        /// </summary>
        public static readonly RoutedCommand LineRightCommand = new RoutedCommand("LineRight", typeof(ScrollBar));

        /// <summary>
        /// A command that decreases the value of the scroll bar by a small amount in the horizontal direction.
        /// </summary>
        public static readonly RoutedCommand LineLeftCommand = new RoutedCommand("LineLeft", typeof(ScrollBar));

        /// <summary>
        /// A command that increases the value of the scroll bar by a large amount in the vertical direction.
        /// </summary>
        public static readonly RoutedCommand PageDownCommand = new RoutedCommand("PageDown", typeof(ScrollBar));

        /// <summary>
        /// A command that decreases the value of the scroll bar by a large amount in the vertical direction.
        /// </summary>
        public static readonly RoutedCommand PageUpCommand = new RoutedCommand("PageUp", typeof(ScrollBar));

        /// <summary>
        /// A command that increases the value of the scroll bar by a large amount in the horizontal direction.
        /// </summary>
        public static readonly RoutedCommand PageRightCommand = new RoutedCommand("PageRight", typeof(ScrollBar));

        /// <summary>
        /// A command that decreases the value of the scroll bar by a large amount in the horizontal direction.
        /// </summary>
        public static readonly RoutedCommand PageLeftCommand = new RoutedCommand("PageLeft", typeof(ScrollBar));

        /// <summary>
        /// A command that scrolls the scroll bar to the last position on the track which was right clicked.
        /// </summary>
        public static readonly RoutedCommand ScrollHereCommand = new RoutedCommand("ScrollHere", typeof(ScrollBar));

        /// <summary>
        /// A command that scrolls a vertical scroll bar to its maximum value.
        /// </summary>
        public static readonly RoutedCommand ScrollToBottomCommand = new RoutedCommand("ScrollToBottom", typeof(ScrollBar));

        /// <summary>
        /// A command that scrolls a vertical scroll bar to its minimum value.
        /// </summary>
        public static readonly RoutedCommand ScrollToTopCommand = new RoutedCommand("ScrollToTop", typeof(ScrollBar));

        /// <summary>
        /// A command that scrolls a horizontal scroll bar to its maximum value.
        /// </summary>
        public static readonly RoutedCommand ScrollToRightEndCommand = new RoutedCommand("ScrollToRightEnd", typeof(ScrollBar));

        /// <summary>
        /// A command that scrolls a horizontal scroll bar to its minimum value.
        /// </summary>
        public static readonly RoutedCommand ScrollToLeftEndCommand = new RoutedCommand("ScrollToLeftEnd", typeof(ScrollBar));
        
        /// <summary>
        /// A command that scrolls a scroll viewer to the bottom right corner of its content.
        /// </summary>
        public static readonly RoutedCommand ScrollToEndCommand = new RoutedCommand("ScrollToEnd", typeof(ScrollBar));

        /// <summary>
        /// A command that scrolls a scroll viewer to the top left corner of its content.
        /// </summary>
        public static readonly RoutedCommand ScrollToHomeCommand = new RoutedCommand("ScrollToHome", typeof(ScrollBar));
        
        /// <summary>
        /// A command that scrolls a scroll viewer to the horizontal position specified in the command parameter.
        /// </summary>
        public static readonly RoutedCommand ScrollToHorizontalOffsetCommand = new RoutedCommand("ScrollToHorizontalOffset", typeof(ScrollBar));

        /// <summary>
        /// A command that scrolls a scroll viewer to the vertical position specified in the command parameter.
        /// </summary>
        public static readonly RoutedCommand ScrollToVerticalOffsetCommand = new RoutedCommand("ScrollToVerticalOffset", typeof(ScrollBar));
        
        /// <summary>
        /// Called when the <see cref="RangeBase.Value"/> property of one of the scroll bar's child scroll bars changes.
        /// </summary>
        internal void OnChildValueChanged(OrientedScrollBar child, Double value)
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
        internal void OnChildMinimumChanged(OrientedScrollBar child, Double value)
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
        internal void OnChildMaximumChanged(OrientedScrollBar child, Double value)
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
        internal void OnChildSmallChangeChanged(OrientedScrollBar child, Double value)
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
        internal void OnChildLargeChangeChanged(OrientedScrollBar child, Double value)
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
        /// Called when the <see cref="OrientedScrollBar.ViewportSize"/> property of one of the scroll bar's child scroll bars changes.
        /// </summary>
        internal void OnChildViewportSizeChanged(OrientedScrollBar child, Double value)
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
        /// Executes a scroll command.
        /// </summary>
        private static void ExecutedScrollCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var scrollBar = element as ScrollBar;
            if (scrollBar == null || data.OriginalSource != scrollBar)
                return;

            if (scrollBar.Orientation == Orientation.Horizontal)
            {
                var hscroll = scrollBar.PART_HScrollBar;
                if (hscroll != null)
                {
                    ((RoutedCommand)command).Execute(scrollBar.View, parameter, hscroll);
                    data.Handled = true;
                }
            }
            else
            {
                var vscroll = scrollBar.PART_VScrollBar;
                if (vscroll != null)
                {
                    ((RoutedCommand)command).Execute(scrollBar.View, parameter, vscroll);
                    data.Handled = true;
                }
            }
        }

        /// <summary>
        /// Determines whether a scroll command can execute.
        /// </summary>
        private static void CanExecuteScrollCommand(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var scrollBar = element as ScrollBar;
            if (scrollBar == null || data.OriginalSource != scrollBar)
                return;

            var @continue = data.ContinueRouting;

            if (scrollBar.Orientation == Orientation.Horizontal)
            {
                var hscroll = scrollBar.PART_HScrollBar;
                if (hscroll != null)
                {
                    data.CanExecute = ((RoutedCommand)command).CanExecute(scrollBar.View, parameter, hscroll, out @continue);
                    data.Handled = true;
                }
            }
            else
            {
                var vscroll = scrollBar.PART_VScrollBar;
                if (vscroll != null)
                {
                    data.CanExecute = ((RoutedCommand)command).CanExecute(scrollBar.View, parameter, vscroll, out @continue);
                    data.Handled = true;
                }
            }

            data.ContinueRouting = @continue;
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
