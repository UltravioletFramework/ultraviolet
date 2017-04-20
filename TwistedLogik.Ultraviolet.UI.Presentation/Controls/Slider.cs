using System;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a slider.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.Slider.xml")]
    public class Slider : RangeBase
    {
        /// <summary>
        /// Initializes the <see cref="Slider"/> type.
        /// </summary>
        static Slider()
        {
            // Dependency property overrides
            ValueProperty.OverrideMetadata(typeof(Slider), new PropertyMetadata<Double>(HandleValueChanged));
            MinimumProperty.OverrideMetadata(typeof(Slider), new PropertyMetadata<Double>(HandleMinimumChanged));
            MaximumProperty.OverrideMetadata(typeof(Slider), new PropertyMetadata<Double>(HandleMaximumChanged));
            SmallChangeProperty.OverrideMetadata(typeof(Slider), new PropertyMetadata<Double>(HandleSmallChangeChanged));
            LargeChangeProperty.OverrideMetadata(typeof(Slider), new PropertyMetadata<Double>(HandleLargeChangeChanged));

            // Commands - decrease
            CommandManager.RegisterClassBindings(typeof(Slider), DecreaseLargeCommand, ExecutedDecreaseLargeCommand,
                new ConditionalGesture(src => (src as Slider)?.Orientation == Orientation.Vertical, new KeyGesture(Key.PageUp, ModifierKeys.None, "PageUp")));
            CommandManager.RegisterClassBindings(typeof(Slider), DecreaseSmallCommand, ExecutedDecreaseSmallCommand,
                new ConditionalGesture(src => (src as Slider)?.Orientation == Orientation.Vertical, new KeyGesture(Key.Up, ModifierKeys.None, "Up")),
                new ConditionalGesture(src => (src as Slider)?.Orientation == Orientation.Vertical, new GamePadGesture(GamePadButton.LeftStickUp, 0, "LeftStickUp")),
                new ConditionalGesture(src => (src as Slider)?.Orientation == Orientation.Horizontal, new KeyGesture(Key.Left, ModifierKeys.None, "Left")),
                new ConditionalGesture(src => (src as Slider)?.Orientation == Orientation.Horizontal, new GamePadGesture(GamePadButton.LeftStickLeft, 0, "LeftStickLeft")));

            // Commands - increase
            CommandManager.RegisterClassBindings(typeof(Slider), IncreaseLargeCommand, ExecutedIncreaseLargeCommand,
                new ConditionalGesture(src => (src as Slider)?.Orientation == Orientation.Vertical, new KeyGesture(Key.PageUp, ModifierKeys.None, "PageDown")));
            CommandManager.RegisterClassBindings(typeof(Slider), IncreaseSmallCommand, ExecutedIncreaseSmallCommand,
                new ConditionalGesture(src => (src as Slider)?.Orientation == Orientation.Vertical, new KeyGesture(Key.Down, ModifierKeys.None, "Down")),
                new ConditionalGesture(src => (src as Slider)?.Orientation == Orientation.Vertical, new GamePadGesture(GamePadButton.LeftStickDown, 0, "LeftStickDown")),
                new ConditionalGesture(src => (src as Slider)?.Orientation == Orientation.Horizontal, new KeyGesture(Key.Right, ModifierKeys.None, "Right")),
                new ConditionalGesture(src => (src as Slider)?.Orientation == Orientation.Horizontal, new GamePadGesture(GamePadButton.LeftStickRight, 0, "LeftStickRight")));

            // Commands - min/max
            CommandManager.RegisterClassBindings(typeof(Slider), MaximizeValueCommand, ExecutedMaximizeValueCommand,
                new KeyGesture(Key.End, ModifierKeys.None, "End"));
            CommandManager.RegisterClassBindings(typeof(Slider), MinimizeValueCommand, ExecutedMinimizeValueCommand,
                new KeyGesture(Key.Home, ModifierKeys.None, "Home"));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Slider"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Slider(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }
        
        /// <summary>
        /// Gets the slider's <see cref="Track"/> component.
        /// </summary>
        public Track Track
        {
            get { return (Orientation == Orientation.Horizontal) ? PART_HSlider?.Track : PART_VSlider?.Track; }
        }

        /// <summary>
        /// Gets or sets a value that specifies whether the slider is oriented vertically or horizontally.
        /// </summary>
        /// <value>An <see cref="Orientation"/> that specifies whether the slider is oriented vertically or horizontally.</value>
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
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Orientation"/> dependency property.</value>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Slider),
            new PropertyMetadata<Orientation>(Orientation.Vertical, HandleOrientationChanged));

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
        /// Identifies the <see cref="Delay"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Delay"/> dependency property.</value>
        public static readonly DependencyProperty DelayProperty = RepeatButton.DelayProperty.AddOwner(typeof(Slider),
            new PropertyMetadata<Double>(HandleDelayChanged));

        /// <summary>
        /// Identifies the <see cref="Interval"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Interval"/> dependency property.</value>
        public static readonly DependencyProperty IntervalProperty = RepeatButton.IntervalProperty.AddOwner(typeof(Slider),
            new PropertyMetadata<Double>(HandleIntervalChanged));

        /// <summary>
        /// A command that decreases the value of the slider by a large amount.
        /// </summary>
        public static readonly RoutedCommand DecreaseLargeCommand = new RoutedCommand("DecreaseLarge", typeof(Slider));

        /// <summary>
        /// A command that decreases the value of the slider by a small amount.
        /// </summary>
        public static readonly RoutedCommand DecreaseSmallCommand = new RoutedCommand("DecreaseSmall", typeof(Slider));

        /// <summary>
        /// A command that increases the value of the slider by a large amount.
        /// </summary>
        public static readonly RoutedCommand IncreaseLargeCommand = new RoutedCommand("IncreaseLarge", typeof(Slider));

        /// <summary>
        /// A command that increases the value of the slider by a small amount.
        /// </summary>
        public static readonly RoutedCommand IncreaseSmallCommand = new RoutedCommand("IncreaseSmall", typeof(Slider));

        /// <summary>
        /// A command that sets the slider's value to the maximum value.
        /// </summary>
        public static readonly RoutedCommand MaximizeValueCommand = new RoutedCommand("MaximizeValue", typeof(Slider));

        /// <summary>
        /// A command that sets the slider's value to the minimum value.
        /// </summary>
        public static readonly RoutedCommand MinimizeValueCommand = new RoutedCommand("MinimizeValue", typeof(Slider));
        
        /// <summary>
        /// Responds to the <see cref="DecreaseLargeCommand"/> command.
        /// </summary>
        protected virtual void OnDecreaseLarge()
        {

        }

        /// <summary>
        /// Responds to the <see cref="DecreaseSmallCommand"/> command.
        /// </summary>
        protected virtual void OnDecreaseSmall()
        {

        }

        /// <summary>
        /// Responds to the <see cref="IncreaseLargeCommand"/> command.
        /// </summary>
        protected virtual void OnIncreaseLarge()
        {

        }

        /// <summary>
        /// Responds to the <see cref="IncreaseSmallCommand"/> command.
        /// </summary>
        protected virtual void OnIncreaseSmall()
        {

        }

        /// <summary>
        /// Responds to the <see cref="MaximizeValueCommand"/> command.
        /// </summary>
        protected virtual void OnMaximizeValue()
        {

        }

        /// <summary>
        /// Responds to the <see cref="MinimizeValueCommand"/> command.
        /// </summary>
        protected virtual void OnMinimizeValue()
        {

        }
        
        /// <summary>
        /// Occurs when the value of the <see cref="RangeBase.Value"/> dependency property changes.
        /// </summary>
        private static void HandleValueChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            if (!(element is Slider))
                return;

            var hscroll = ((Slider)element).PART_HSlider;
            if (hscroll != null)
                hscroll.Value = newValue;

            var vscroll = ((Slider)element).PART_VSlider;
            if (vscroll != null)
                vscroll.Value = newValue;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RangeBase.Minimum"/> dependency property changes.
        /// </summary>
        private static void HandleMinimumChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            if (!(element is Slider))
                return;

            var hscroll = ((Slider)element).PART_HSlider;
            if (hscroll != null)
                hscroll.Minimum = newValue;

            var vscroll = ((Slider)element).PART_VSlider;
            if (vscroll != null)
                vscroll.Minimum = newValue;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RangeBase.Maximum"/> dependency property changes.
        /// </summary>
        private static void HandleMaximumChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            if (!(element is Slider))
                return;

            var hscroll = ((Slider)element).PART_HSlider;
            if (hscroll != null)
                hscroll.Maximum = newValue;

            var vscroll = ((Slider)element).PART_VSlider;
            if (vscroll != null)
                vscroll.Maximum = newValue;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RangeBase.SmallChange"/> dependency property changes.
        /// </summary>
        private static void HandleSmallChangeChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            if (!(element is Slider))
                return;

            var hscroll = ((Slider)element).PART_HSlider;
            if (hscroll != null)
                hscroll.SmallChange = newValue;

            var vscroll = ((Slider)element).PART_VSlider;
            if (vscroll != null)
                vscroll.SmallChange = newValue;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RangeBase.LargeChange"/> dependency property changes.
        /// </summary>
        private static void HandleLargeChangeChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            if (!(element is Slider))
                return;

            var hscroll = ((Slider)element).PART_HSlider;
            if (hscroll != null)
                hscroll.LargeChange = newValue;

            var vscroll = ((Slider)element).PART_VSlider;
            if (vscroll != null)
                vscroll.LargeChange = newValue;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation"/> dependency property changes.
        /// </summary>
        private static void HandleOrientationChanged(DependencyObject element, Orientation oldValue, Orientation newValue)
        {
            (element as Slider)?.ChangeOrientation(newValue);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Delay"/> dependency property changes.
        /// </summary>
        private static void HandleDelayChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            if (!(element is Slider))
                return;

            var hscroll = ((Slider)element).PART_HSlider;
            if (hscroll != null)
                hscroll.Delay = newValue;

            var vscroll = ((Slider)element).PART_VSlider;
            if (vscroll != null)
                vscroll.Delay = newValue;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Interval"/> dependency property changes.
        /// </summary>
        private static void HandleIntervalChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            if (!(element is Slider))
                return;

            var hscroll = ((Slider)element).PART_HSlider;
            if (hscroll != null)
                hscroll.Interval = newValue;

            var vscroll = ((Slider)element).PART_VSlider;
            if (vscroll != null)
                vscroll.Interval = newValue;
        }

        /// <summary>
        /// Executes the <see cref="DecreaseLargeCommand"/> command.
        /// </summary>
        private static void ExecutedDecreaseLargeCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var slider = element as Slider;
            if (slider == null || data.OriginalSource != slider)
                return;

            slider.OnDecreaseLarge();
            slider.ForwardCommandToOrientedSlider(command, parameter);
            data.Handled = true;
        }

        /// <summary>
        /// Executes the <see cref="DecreaseSmallCommand"/> command.
        /// </summary>
        private static void ExecutedDecreaseSmallCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var slider = element as Slider;
            if (slider == null || data.OriginalSource != slider)
                return;

            slider.OnDecreaseSmall();
            slider.ForwardCommandToOrientedSlider(command, parameter);
            data.Handled = true;
        }

        /// <summary>
        /// Executes the <see cref="IncreaseLargeCommand"/> command.
        /// </summary>
        private static void ExecutedIncreaseLargeCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var slider = element as Slider;
            if (slider == null || data.OriginalSource != slider)
                return;

            slider.OnIncreaseLarge();
            slider.ForwardCommandToOrientedSlider(command, parameter);
            data.Handled = true;
        }

        /// <summary>
        /// Executes the <see cref="IncreaseSmallCommand"/> command.
        /// </summary>
        private static void ExecutedIncreaseSmallCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var slider = element as Slider;
            if (slider == null || data.OriginalSource != slider)
                return;

            slider.OnIncreaseSmall();
            slider.ForwardCommandToOrientedSlider(command, parameter);
            data.Handled = true;
        }

        /// <summary>
        /// Executes the <see cref="MaximizeValueCommand"/> command.
        /// </summary>
        private static void ExecutedMaximizeValueCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var slider = element as Slider;
            if (slider == null || data.OriginalSource != slider)
                return;

            slider.OnMaximizeValue();
            slider.ForwardCommandToOrientedSlider(command, parameter);
            data.Handled = true;
        }

        /// <summary>
        /// Executes the <see cref="MinimizeValueCommand"/> command.
        /// </summary>
        private static void ExecutedMinimizeValueCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var slider = element as Slider;
            if (slider == null || data.OriginalSource != slider)
                return;

            slider.OnMinimizeValue();
            slider.ForwardCommandToOrientedSlider(command, parameter);
            data.Handled = true;
        }

        /// <summary>
        /// Forwards the specified command to the currently active oriented slider.
        /// </summary>
        private void ForwardCommandToOrientedSlider(ICommand command, Object parameter)
        {
            var orientedScrollBar = (Orientation == Orientation.Horizontal) ? PART_HSlider : PART_VSlider;
            if (orientedScrollBar != null)
            {
                ((RoutedCommand)command).Execute(View, parameter, orientedScrollBar);
            }
        }

        /// <summary>
        /// Switches the slider to the specified orientation.
        /// </summary>
        private void ChangeOrientation(Orientation orientation)
        {
            var childHidden = (orientation == Orientation.Horizontal) ? PART_VSlider : PART_HSlider;
            if (childHidden != null)
            {
                childHidden.Visibility = Visibility.Collapsed;
                childHidden.IsEnabled = false;
            }

            var childActive = (orientation == Orientation.Horizontal) ? PART_HSlider : PART_VSlider;
            if (childActive != null)
            {
                childActive.Visibility = Visibility.Visible;
                childActive.IsEnabled = true;

                Value = childActive.Value;
                Minimum = childActive.Minimum;
                Maximum = childActive.Maximum;
                SmallChange = childActive.SmallChange;
                LargeChange = childActive.LargeChange;
            }
        }

        // Component references.
        private readonly OrientedSlider PART_HSlider = null;
        private readonly OrientedSlider PART_VSlider = null;
    }
}
