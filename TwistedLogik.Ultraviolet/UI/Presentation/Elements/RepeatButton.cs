using System;
using System.Runtime.InteropServices;
using System.Security;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a button that raises its <see cref="ButtonBase.Click"/> event repeatedly while it is pressed.
    /// </summary>
    [UIElement("RepeatButton")]
    public class RepeatButton : Button
    {
        /// <summary>
        /// Contains native Win32 functions.
        /// </summary>
        private static class Native
        {
            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool SystemParametersInfo(uint action, uint param, ref uint vparam, uint init);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatButton"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public RepeatButton(UltravioletContext uv, String id)
            : base(uv, id)
        {
            SetDefaultValue<ClickMode>(ClickModeProperty, ClickMode.Press);
        }

        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, that the button waits prior to 
        /// beginning its repetitions.
        /// </summary>
        public Double Delay
        {
            get { return GetValue<Double>(DelayProperty); }
            set { SetValue<Double>(DelayProperty, value); }
        }

        /// <summary>
        /// Gets or sets the interval between repeated <see cref="ButtonBase.Click"/> events, in milliseconds.
        /// </summary>
        public Double Interval
        {
            get { return GetValue<Double>(IntervalProperty); }
            set { SetValue<Double>(IntervalProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Delay"/> property changes.
        /// </summary>
        public event UIElementEventHandler DelayChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Interval"/> property changes.
        /// </summary>
        public event UIElementEventHandler IntervalChanged;

        /// <summary>
        /// Identifies the <see cref="Delay"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(Double), typeof(RepeatButton),
            new DependencyPropertyMetadata(HandleDelayChanged, () => GetDefaultDelay(), DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="Interval"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register("Interval", typeof(Double), typeof(RepeatButton),
            new DependencyPropertyMetadata(HandleIntervalChanged, () => GetDefaultInterval(), DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected override void OnUpdating(UltravioletTime time)
        {
            UpdateRepetitions(time);

            base.OnUpdating(time);
        }

        /// <inheritdoc/>
        protected internal override void OnLostMouseCapture()
        {
            repeating = false;
            base.OnLostMouseCapture();
        }

        /// <inheritdoc/>
        protected internal override void OnMouseButtonReleased(MouseDevice device, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                repeating = false;
            }
            base.OnMouseButtonReleased(device, button);
        }

        /// <summary>
        /// Raises the <see cref="DelayChanged"/> event.
        /// </summary>
        protected virtual void OnDelayChanged()
        {
            var temp = DelayChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="IntervalChanged"/> event.
        /// </summary>
        protected virtual void OnIntervalChanged()
        {
            var temp = IntervalChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Gets the default value for the <see cref="Delay"/> dependency property.
        /// </summary>
        /// <returns>The property's default value.</returns>
        [SecuritySafeCritical]
        private static Double GetDefaultDelay()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                const UInt32 SPI_GETKEYBOARDDELAY = 0x0016;

                uint delay = 0;
                Native.SystemParametersInfo(SPI_GETKEYBOARDDELAY, 0, ref delay, 0);

                return (1 + delay) * 250.0;
            }
            return 500.0;
        }

        /// <summary>
        /// Gets the default value for the <see cref="Interval"/> dependency property.
        /// </summary>
        /// <returns>The property's default value.</returns>
        [SecuritySafeCritical]
        private static Double GetDefaultInterval()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                const UInt32 SPI_GETKEYBOARDSPEED = 0x000A;

                uint speed = 0;
                Native.SystemParametersInfo(SPI_GETKEYBOARDSPEED, 0, ref speed, 0);

                return 33.0 + ((31 - speed) * (367.0 / 31));
            }
            return 33.0;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Delay"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleDelayChanged(DependencyObject dobj)
        {
            var button = (RepeatButton)dobj;
            button.OnDelayChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Interval"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleIntervalChanged(DependencyObject dobj)
        {
            var button = (RepeatButton)dobj;
            button.OnIntervalChanged();
        }

        /// <summary>
        /// Updates the button's repetition state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        private void UpdateRepetitions(UltravioletTime time)
        {
            if (!Depressed)
                return;

            var input = Ultraviolet.GetInput();
            if (input.IsMouseSupported())
            {
                var mouse = input.GetMouse();
                if (!ScreenBounds.Contains(mouse.X, mouse.Y))
                    return;
            }

            repeatTimer += time.ElapsedTime.TotalMilliseconds;
            if (repeating)
            {
                var interval = Interval;
                if (repeatTimer >= interval)
                {
                    repeatTimer %= interval;
                    OnClick();
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
                }
            }
        }

        // State values.
        private Double repeatTimer;
        private Boolean repeating;
    }
}
