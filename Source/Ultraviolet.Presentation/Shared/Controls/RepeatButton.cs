using System;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a button that raises its <see cref="Primitives.ButtonBase.Click"/> event repeatedly while it is pressed.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.RepeatButton.xml")]
    public class RepeatButton : Button
    {
        /// <summary>
        /// Initializes the <see cref="RepeatButton"/> type.
        /// </summary>
        static RepeatButton()
        {
            ClickModeProperty.OverrideMetadata(typeof(RepeatButton), new PropertyMetadata<ClickMode>(ClickMode.Press));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatButton"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public RepeatButton(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, that the button waits prior to 
        /// beginning its repetitions.
        /// </summary>
        /// <value>A <see cref="Double"/> value that represents the amount of time, in milliseconds,
        /// that the button waits prior to beginning its repetitions.</value>
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
        /// Gets or sets the interval between repeated <see cref="Primitives.ButtonBase.Click"/> events, in milliseconds.
        /// </summary>
        /// <value>A <see cref="Double"/> value that represents the interval, in milliseconds, 
        /// between <see cref="Primitives.ButtonBase.Click"/> events.</value>
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
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(Double), typeof(RepeatButton),
            new PropertyMetadata<Double>(SystemParameters.KeyboardDelay));

        /// <summary>
        /// Identifies the <see cref="Interval"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Interval"/> dependency property.</value>
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register("Interval", typeof(Double), typeof(RepeatButton),
            new PropertyMetadata<Double>(SystemParameters.KeyboardSpeed));

        /// <inheritdoc/>
        protected override void OnUpdating(UltravioletTime time)
        {
            if (IsPressed && AreAnyCursorsOver)
            {
                repeatTimer += time.ElapsedTime.TotalMilliseconds;
                if (repeating)
                {
                    var interval = Interval;
                    if (repeatTimer >= interval)
                    {
                        repeatTimer %= interval;
                        OnClick();
                        OnClickByUser();
                    }
                }
                else
                {
                    var delay = Delay;
                    if (repeatTimer >= delay)
                    {
                        repeatTimer %= delay;
                        repeating = true;
                        OnClick();
                        OnClickByUser();
                    }
                }
            }

            base.OnUpdating(time);
        }
        
        /// <inheritdoc/>
        protected override void OnIsPressedChanged()
        {
            if (IsPressed)
            {
                repeating = false;
                repeatTimer = 0;
            }
            base.OnIsPressedChanged();
        }

        // State values.
        private Double repeatTimer;
        private Boolean repeating;
    }
}