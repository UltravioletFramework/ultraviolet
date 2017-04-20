using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents the base class for horizontal and vertical sliders.
    /// </summary>
    [Preserve(AllMembers = true)]
    [UvmlKnownType]
    public abstract class OrientedSlider : RangeBase
    {
        /// <summary>
        /// Initializes the <see cref="OrientedSlider"/> type.
        /// </summary>
        static OrientedSlider()
        {
            // Dependency properties
            MinimumProperty.AddOwner(typeof(OrientedSlider), new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.AffectsMeasure));
            MaximumProperty.AddOwner(typeof(OrientedSlider), new PropertyMetadata<Double>(10.0, PropertyMetadataOptions.AffectsMeasure));
            ValueProperty.AddOwner(typeof(OrientedSlider), new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.AffectsArrange));

            // Event handlers
            EventManager.RegisterClassHandler(typeof(OrientedSlider), Mouse.PreviewMouseDownEvent, new UpfMouseButtonEventHandler(HandlePreviewMouseDown));
            EventManager.RegisterClassHandler(typeof(OrientedSlider), Thumb.DragStartedEvent, new UpfDragStartedEventHandler(HandleThumbDragStarted));
            EventManager.RegisterClassHandler(typeof(OrientedSlider), Thumb.DragDeltaEvent, new UpfDragDeltaEventHandler(HandleThumbDragDelta));
            EventManager.RegisterClassHandler(typeof(OrientedSlider), Thumb.DragCompletedEvent, new UpfDragCompletedEventHandler(HandleThumbDragCompleted));

            // Commands - decrease
            CommandManager.RegisterClassBindings(typeof(OrientedSlider), Slider.DecreaseLargeCommand, ExecutedDecreaseLargeCommand,
                new KeyGesture(Key.PageUp, ModifierKeys.None, "PageUp"));
            CommandManager.RegisterClassBindings(typeof(OrientedSlider), Slider.DecreaseSmallCommand, ExecutedDecreaseSmallCommand,
                new ConditionalGesture(src => (src is VSlider) && !((VSlider)src).IsDirectionReversed, new KeyGesture(Key.Up, ModifierKeys.None, "Up")),
                new ConditionalGesture(src => (src is VSlider) && !((VSlider)src).IsDirectionReversed, new GamePadGesture(GamePadButton.LeftStickUp, 0, "LeftStickUp")),
                new ConditionalGesture(src => (src is HSlider) && !((HSlider)src).IsDirectionReversed, new KeyGesture(Key.Left, ModifierKeys.None, "Left")),
                new ConditionalGesture(src => (src is HSlider) && !((HSlider)src).IsDirectionReversed, new GamePadGesture(GamePadButton.LeftStickLeft, 0, "LeftStickLeft")),
                new ConditionalGesture(src => (src is VSlider) && ((VSlider)src).IsDirectionReversed, new KeyGesture(Key.Down, ModifierKeys.None, "Down")),
                new ConditionalGesture(src => (src is VSlider) && ((VSlider)src).IsDirectionReversed, new GamePadGesture(GamePadButton.LeftStickDown, 0, "LeftStickDown")),
                new ConditionalGesture(src => (src is HSlider) && ((HSlider)src).IsDirectionReversed, new KeyGesture(Key.Right, ModifierKeys.None, "Right")),
                new ConditionalGesture(src => (src is HSlider) && ((HSlider)src).IsDirectionReversed, new GamePadGesture(GamePadButton.LeftStickRight, 0, "LeftStickRight")));

            // Commands - increase
            CommandManager.RegisterClassBindings(typeof(OrientedSlider), Slider.IncreaseLargeCommand, ExecutedIncreaseLargeCommand,
                new KeyGesture(Key.PageDown, ModifierKeys.None, "PageDown"));
            CommandManager.RegisterClassBindings(typeof(OrientedSlider), Slider.IncreaseSmallCommand, ExecutedIncreaseSmallCommand,
                new ConditionalGesture(src => (src is VSlider) && !((VSlider)src).IsDirectionReversed, new KeyGesture(Key.Down, ModifierKeys.None, "Down")),
                new ConditionalGesture(src => (src is VSlider) && !((VSlider)src).IsDirectionReversed, new GamePadGesture(GamePadButton.LeftStickDown, 0, "LeftStickDown")),
                new ConditionalGesture(src => (src is HSlider) && !((HSlider)src).IsDirectionReversed, new KeyGesture(Key.Right, ModifierKeys.None, "Right")),
                new ConditionalGesture(src => (src is HSlider) && !((HSlider)src).IsDirectionReversed, new GamePadGesture(GamePadButton.LeftStickRight, 0, "LeftStickRight")),
                new ConditionalGesture(src => (src is VSlider) && ((VSlider)src).IsDirectionReversed, new KeyGesture(Key.Up, ModifierKeys.None, "up")),
                new ConditionalGesture(src => (src is VSlider) && ((VSlider)src).IsDirectionReversed, new GamePadGesture(GamePadButton.LeftStickUp, 0, "LeftStickUp")),
                new ConditionalGesture(src => (src is HSlider) && ((HSlider)src).IsDirectionReversed, new KeyGesture(Key.Left, ModifierKeys.None, "Left")),
                new ConditionalGesture(src => (src is HSlider) && ((HSlider)src).IsDirectionReversed, new GamePadGesture(GamePadButton.LeftStickLeft, 0, "LeftStickLeft")));

            // Commands - min/max
            CommandManager.RegisterClassBindings(typeof(OrientedSlider), Slider.MaximizeValueCommand, ExecutedMaximizeValueCommand,
                new KeyGesture(Key.End, ModifierKeys.None, "End"));
            CommandManager.RegisterClassBindings(typeof(OrientedSlider), Slider.MinimizeValueCommand, ExecutedMinimizeValueCommand,
                new KeyGesture(Key.Home, ModifierKeys.None, "Home"));

            // Commands - track
            CommandManager.RegisterClassBindings(typeof(OrientedSlider), Track.IncreaseCommand, ExecutedIncreaseLargeCommand);
            CommandManager.RegisterClassBindings(typeof(OrientedSlider), Track.DecreaseCommand, ExecutedDecreaseLargeCommand);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrientedSlider"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public OrientedSlider(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets the slider's <see cref="Track"/> component.
        /// </summary>
        public Track Track
        {
            get { return PART_Track; }
        }

        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, that one of the slider's repeat buttons waits prior to 
        /// issuing a command to move the slider's thumb.
        /// </summary>
        /// <value>A <see cref="Double"/> value that represents the amount of time, in milliseconds,
        /// that one of the slider's repeat buttons waits prior to issuing a command to move the slider's thumb.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="DelayProperty"/></dpropField>
        ///     <dpropStylingName>delay</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double Delay
        {
            get { return GetValue<Double>(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, between subsequent commands issued by one of the slider's
        /// repeat buttons to move the slider's thumb.
        /// </summary>
        /// <value>A <see cref="Double"/> value that represents the amount of time, in milliseconds, between subsequent 
        /// commands issued by one of the slider's repeat buttons to move the slider's thumb..</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="IntervalProperty"/></dpropField>
        ///     <dpropStylingName>interval</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double Interval
        {
            get { return GetValue<Double>(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the slider's direction of increasing value is reversed.
        /// </summary>
        /// <value>A <see cref="Boolean"/> value indicating whether the slider's direction of increasing value is reversed.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="IsDirectionReversedProperty"/></dpropField>
        ///     <dpropStylingName>direction-reversed</dpropStylingName>
        ///     <dpropMetadata>AffectsMeasure</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsDirectionReversed
        {
            get { return GetValue<Boolean>(IsDirectionReversedProperty); }
            set { SetValue(IsDirectionReversedProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Delay"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Delay"/> dependency property.</value>
        public static readonly DependencyProperty DelayProperty = RepeatButton.DelayProperty.AddOwner(typeof(OrientedSlider));

        /// <summary>
        /// Identifies the <see cref="Interval"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Interval"/> dependency property.</value>
        public static readonly DependencyProperty IntervalProperty = RepeatButton.IntervalProperty.AddOwner(typeof(OrientedSlider));

        /// <summary>
        /// Identifies the <see cref="IsDirectionReversed"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Interval"/> dependency property.</value>
        public static readonly DependencyProperty IsDirectionReversedProperty = Slider.IsDirectionReversedProperty.AddOwner(typeof(OrientedSlider));

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            PART_Track?.InvalidateMeasure();
            return base.MeasureOverride(availableSize);
        }
        
        /// <summary>
        /// Occurs when the user begins dragging the slider's thumb button.
        /// </summary>
        /// <param name="hoffset">The horizontal offset at which the thumb was clicked, in relative coordinates.</param>
        /// <param name="voffset">The vertical offset at which the thumb was clicked, in relative coordinates.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnThumbDragStarted(Double hoffset, Double voffset, RoutedEventData data)
        {

        }

        /// <summary>
        /// Occurs when the slider's thumb button is moved as a result of being dragged by the user.
        /// </summary>
        /// <param name="hchange">The distance that the thumb moved horizontally.</param>
        /// <param name="vchange">The distance that the thumb moved vertically.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnThumbDragDelta(Double hchange, Double vchange, RoutedEventData data)
        {
            if (hchange == 0 && vchange == 0)
                return;

            var valueDelta = Track.ValueFromDistance(hchange, vchange);
            if (!Double.IsNaN(valueDelta) && valueDelta != 0.0)
            {
                Value += valueDelta;
            }
        }

        /// <summary>
        /// Occurs when the user finishes dragging the slider's thumb button.
        /// </summary>
        /// <param name="hchange">The total distance that the thumb moved horizontally.</param>
        /// <param name="vchange">The total distance that the thumb moved vertically.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnThumbDragCompleted(Double hchange, Double vchange, RoutedEventData data)
        {

        }
        
        /// <summary>
        /// Responds to the <see cref="Slider.DecreaseLargeCommand"/> command.
        /// </summary>
        protected virtual void OnDecreaseLarge()
        {
            Value -= LargeChange;
        }

        /// <summary>
        /// Responds to the <see cref="Slider.DecreaseSmallCommand"/> command.
        /// </summary>
        protected virtual void OnDecreaseSmall()
        {
            Value -= SmallChange;
        }

        /// <summary>
        /// Responds to the <see cref="Slider.IncreaseLargeCommand"/> command.
        /// </summary>
        protected virtual void OnIncreaseLarge()
        {
            Value += LargeChange;
        }

        /// <summary>
        /// Responds to the <see cref="Slider.IncreaseSmallCommand"/> command.
        /// </summary>
        protected virtual void OnIncreaseSmall()
        {
            Value += SmallChange;
        }

        /// <summary>
        /// Responds to the <see cref="Slider.MaximizeValueCommand"/> command.
        /// </summary>
        protected virtual void OnMaximizeValue()
        {
            Value = Maximum;
        }

        /// <summary>
        /// Responds to the <see cref="Slider.MinimizeValueCommand"/> command.
        /// </summary>
        protected virtual void OnMinimizeValue()
        {
            Value = Minimum;
        }

        /// <summary>
        /// Handles the <see cref="Mouse.PreviewMouseDownEvent"/>.
        /// </summary>
        private static void HandlePreviewMouseDown(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            var slider = element as OrientedSlider;
            if (slider == null)
                return;

            if (button == MouseButton.Left)
                slider.Focus();
        }

        /// <summary>
        /// Occurs when the user starts a thumb drag operation.
        /// </summary>
        private static void HandleThumbDragStarted(DependencyObject element, Double hoffset, Double voffset, RoutedEventData data)
        {
            var slider = (OrientedSlider)element;
            slider.OnThumbDragStarted(hoffset, voffset, data);
        }

        /// <summary>
        /// Occurs when the user moves the thumb during a drag operation.
        /// </summary>
        private static void HandleThumbDragDelta(DependencyObject element, Double hchange, Double vchange, RoutedEventData data)
        {
            var slider = (OrientedSlider)element;
            slider.OnThumbDragDelta(hchange, vchange, data);
        }

        /// <summary>
        /// Occurs when the user completes a thumb drag operation.
        /// </summary>
        private static void HandleThumbDragCompleted(DependencyObject element, Double hchange, Double vchange, RoutedEventData data)
        {
            var slider = (OrientedSlider)element;
            slider.OnThumbDragCompleted(hchange, vchange, data);
        }

        /// <summary>
        /// Executes the <see cref="Slider.DecreaseLargeCommand"/> command.
        /// </summary>
        private static void ExecutedDecreaseLargeCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var slider = element as OrientedSlider;
            if (slider == null)
                return;

            slider.OnDecreaseLarge();
            data.Handled = true;
        }

        /// <summary>
        /// Executes the <see cref="Slider.DecreaseSmallCommand"/> command.
        /// </summary>
        private static void ExecutedDecreaseSmallCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var slider = element as OrientedSlider;
            if (slider == null)
                return;

            slider.OnDecreaseSmall();
            data.Handled = true;
        }

        /// <summary>
        /// Executes the <see cref="Slider.IncreaseLargeCommand"/> command.
        /// </summary>
        private static void ExecutedIncreaseLargeCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var slider = element as OrientedSlider;
            if (slider == null)
                return;

            slider.OnIncreaseLarge();
            data.Handled = true;
        }

        /// <summary>
        /// Executes the <see cref="Slider.IncreaseSmallCommand"/> command.
        /// </summary>
        private static void ExecutedIncreaseSmallCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var slider = element as OrientedSlider;
            if (slider == null)
                return;

            slider.OnIncreaseSmall();
            data.Handled = true;
        }

        /// <summary>
        /// Executes the <see cref="Slider.MaximizeValueCommand"/> command.
        /// </summary>
        private static void ExecutedMaximizeValueCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var slider = element as OrientedSlider;
            if (slider == null)
                return;

            slider.OnMaximizeValue();
            data.Handled = true;
        }

        /// <summary>
        /// Executes the <see cref="Slider.MinimizeValueCommand"/> command.
        /// </summary>
        private static void ExecutedMinimizeValueCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var slider = element as OrientedSlider;
            if (slider == null)
                return;

            slider.OnMinimizeValue();
            data.Handled = true;
        }

        // Component references.
        private readonly Track PART_Track = null;
    }
}
