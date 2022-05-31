using System;
using Ultraviolet.Core;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents the base class for horizontal and vertical sliders.
    /// </summary>
    [UvmlKnownType]
    public abstract class OrientedSlider : RangeBase
    {
        /// <summary>
        /// Initializes the <see cref="OrientedSlider"/> type.
        /// </summary>
        static OrientedSlider()
        {
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
        /// Gets or sets a value that specifies whether the slider is oriented vertically or horizontally.
        /// </summary>
        /// <value>An <see cref="TickPlacement"/> that specifies whether the slider is oriented vertically or horizontally.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="TickPlacementProperty"/></dpropField>
        ///		<dpropStylingName>tick-placement</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public TickPlacement TickPlacement
        {
            get { return GetValue<TickPlacement>(TickPlacementProperty); }
            set { SetValue(TickPlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the interval between tick marks.
        /// </summary>
        /// <value>A <see cref="Double"/> value that represents the interval between tick marks.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="TickFrequencyProperty"/></dpropField>
        ///     <dpropStylingName>tick-frequency</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double TickFrequency
        {
            get { return GetValue<Double>(TickFrequencyProperty); }
            set { SetValue(TickFrequencyProperty, value); }
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
        /// Gets or sets a value indicating whether the slider's thumb is automatically snapped to the slider's ticks.
        /// </summary>
        /// <value>A <see cref="Boolean"/> value indicating whether the slider's thumb is automatically snapped to the slider's ticks.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="IsSnapToTickEnabledProperty"/></dpropField>
        ///     <dpropStylingName>snap-to-tick-enabled</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsSnapToTickEnabled
        {
            get { return GetValue<Boolean>(IsSnapToTickEnabledProperty); }
            set { SetValue(IsSnapToTickEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether left-clicking on the slider with the mouse causes the thumb
        /// to immediately move to the clicked point.
        /// </summary>
        /// <value>A <see cref="Boolean"/> value indicating whether left-clicking on the slider with the mouse causes the thumb
        /// to immediately move to the clicked point.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="IsMoveToPointEnabledProperty"/></dpropField>
        ///     <dpropStylingName>move-to-point-enabled</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsMoveToPointEnabled
        {
            get { return GetValue<Boolean>(IsMoveToPointEnabledProperty); }
            set { SetValue(IsMoveToPointEnabledProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TickPlacement"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="TickPlacement"/> dependency property.</value>
        public static readonly DependencyProperty TickPlacementProperty = Slider.TickPlacementProperty.AddOwner(typeof(OrientedSlider));

        /// <summary>
        /// Identifies the <see cref="TickFrequency"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="TickFrequency"/> dependency property.</value>
        public static readonly DependencyProperty TickFrequencyProperty = Slider.TickFrequencyProperty.AddOwner(typeof(OrientedSlider));

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
        /// <value>The identifier for the <see cref="IsDirectionReversed"/> dependency property.</value>
        public static readonly DependencyProperty IsDirectionReversedProperty = Slider.IsDirectionReversedProperty.AddOwner(typeof(OrientedSlider));

        /// <summary>
        /// Identifies the <see cref="IsSnapToTickEnabled"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsSnapToTickEnabled"/> dependency property.</value>
        public static readonly DependencyProperty IsSnapToTickEnabledProperty = Slider.IsSnapToTickEnabledProperty.AddOwner(typeof(OrientedSlider));

        /// <summary>
        /// Identifies the <see cref="IsMoveToPointEnabled"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsMoveToPointEnabled"/> dependency property.</value>
        public static readonly DependencyProperty IsMoveToPointEnabledProperty = Slider.IsMoveToPointEnabledProperty.AddOwner(typeof(OrientedSlider));

        /// <inheritdoc/>
        internal override void OnPreApplyTemplate()
        {
            if (TemplatedParent is Slider parent)
            {
                var templateWrapperType = PresentationFoundation.GetDataSourceWrapper(parent).GetType();

                if (HasDefaultValue(ValueProperty))
                    BindValue(ValueProperty, templateWrapperType, "{{Value}}");
                if (HasDefaultValue(MinimumProperty))
                    BindValue(MinimumProperty, templateWrapperType, "{{Minimum}}");
                if (HasDefaultValue(MaximumProperty))
                    BindValue(MaximumProperty, templateWrapperType, "{{Maximum}}");
                if (HasDefaultValue(SmallChangeProperty))
                    BindValue(SmallChangeProperty, templateWrapperType, "{{SmallChange}}");
                if (HasDefaultValue(LargeChangeProperty))
                    BindValue(LargeChangeProperty, templateWrapperType, "{{LargeChange}}");
                if (HasDefaultValue(TickPlacementProperty))
                    BindValue(TickPlacementProperty, templateWrapperType, "{{TickPlacement}}");
                if (HasDefaultValue(TickFrequencyProperty))
                    BindValue(TickFrequencyProperty, templateWrapperType, "{{TickFrequency}}");
                if (HasDefaultValue(IsDirectionReversedProperty))
                    BindValue(IsDirectionReversedProperty, templateWrapperType, "{{IsDirectionReversed}}");
                if (HasDefaultValue(IsSnapToTickEnabledProperty))
                    BindValue(IsSnapToTickEnabledProperty, templateWrapperType, "{{IsSnapToTickEnabled}}");
            }
            base.OnPreApplyTemplate();
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            PART_Track?.InvalidateMeasure();
            return base.MeasureOverride(availableSize);
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseDown(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (IsMoveToPointEnabled && Track != null && Track.Thumb != null && !Track.Thumb.IsMouseOver)
            {
                var pos = Mouse.GetPosition(Track);
                var val = Track.ValueFromPoint(pos);
                if (!Double.IsNaN(val) && !Double.IsInfinity(val))
                {
                    SetValue(val);
                }
                data.Handled = true;
            }
            base.OnPreviewMouseDown(device, button, data);
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
            var valueAfterChange = Value + valueDelta;
            if (!Double.IsNaN(valueAfterChange) && !Double.IsInfinity(valueAfterChange))
            {
                SetValue(valueAfterChange);
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
            SetValueToNextTick(-LargeChange);  
        }

        /// <summary>
        /// Responds to the <see cref="Slider.DecreaseSmallCommand"/> command.
        /// </summary>
        protected virtual void OnDecreaseSmall()
        {
            SetValueToNextTick(-SmallChange);
        }

        /// <summary>
        /// Responds to the <see cref="Slider.IncreaseLargeCommand"/> command.
        /// </summary>
        protected virtual void OnIncreaseLarge()
        {
            SetValueToNextTick(LargeChange);
        }

        /// <summary>
        /// Responds to the <see cref="Slider.IncreaseSmallCommand"/> command.
        /// </summary>
        protected virtual void OnIncreaseSmall()
        {
            SetValueToNextTick(SmallChange);
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

        /// <summary>
        /// Snaps the specified slider value to the nearest tick.
        /// </summary>
        private Double SnapToTick(Double value)
        {
            if (IsSnapToTickEnabled)
            {
                var min = Minimum;
                var max = Maximum;

                var prev = min;
                var next = max;

                if (MathUtil.IsApproximatelyGreaterThan(TickFrequency, 0.0))
                {
                    var frequency = TickFrequency;
                    prev = min + (Math.Round(((value - min) / frequency)) * frequency);
                    next = Math.Min(max, prev + frequency);
                }

                value = MathUtil.IsApproximatelyGreaterThanOrEqual(value, (prev + next) * 0.5) ? next : prev;
            }
            return value;
        }

        /// <summary>
        /// Sets the slider's value, snapping it to the nearest tick if necessary.
        /// </summary>
        private void SetValue(Double value)
        {
            var snapped = SnapToTick(value);
            if (snapped != Value)
            {
                Value = MathUtil.Clamp(snapped, Minimum, Maximum);
            }
        }

        /// <summary>
        /// Sets the slider's value, snapping it to the next tick in the direction of movement if necessary.
        /// </summary>
        private void SetValueToNextTick(Double delta)
        {
            if (MathUtil.IsApproximatelyZero(delta))
                return;

            var dir = Math.Sign(delta);
            var val = Value;
            var min = Minimum;
            var max = Maximum;
            var frequency = TickFrequency;

            var next = SnapToTick(MathUtil.Clamp(val + delta, min, max));
            if (next == val && !(dir > 0 && val == max) && !(dir < 0 && val == min))
            {
                if (MathUtil.IsApproximatelyGreaterThan(frequency, 0.0))
                {
                    var tickIndex = Math.Round((val - min) / frequency) + dir;                    
                    next = min + tickIndex * frequency;
                }
            }

            if (next != val)
                Value = next;
        }

        // Component references.
        private readonly Track PART_Track = null;
    }
}
