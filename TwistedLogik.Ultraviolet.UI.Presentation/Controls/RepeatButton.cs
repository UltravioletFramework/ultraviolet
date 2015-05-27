using System;
using System.Runtime.InteropServices;
using System.Security;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a button that raises its <see cref="ButtonBase.Click"/> event repeatedly while it is pressed.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.RepeatButton.xml")]
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
        /// <param name="name">The element's identifying name within its namescope.</param>
        public RepeatButton(UltravioletContext uv, String name)
            : base(uv, name)
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
        /// Identifies the <see cref="Delay"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'delay'.</remarks>
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(Double), typeof(RepeatButton),
            new PropertyMetadata<Double>(GetDefaultDelay()));
        
        /// <summary>
        /// Identifies the <see cref="Interval"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'interval'.</remarks>
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register("Interval", typeof(Double), typeof(RepeatButton),
            new PropertyMetadata<Double>(GetDefaultInterval()));

        /// <inheritdoc/>
        protected override void OnUpdating(UltravioletTime time)
        {
            UpdateRepetitions(time);

            base.OnUpdating(time);
        }

        /// <inheritdoc/>
        protected override void OnLostMouseCapture(ref RoutedEventData data)
        {
            repeating   = false;
            repeatTimer = 0;

            base.OnLostMouseCapture(ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                repeating   = false;
                repeatTimer = 0;

                data.Handled = true;
            }
            base.OnMouseDown(device, button, ref data);
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
        /// Updates the button's repetition state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        private void UpdateRepetitions(UltravioletTime time)
        {
            if (!IsPressed)
                return;

            var input = Ultraviolet.GetInput();
            if (input.IsMouseSupported())
            {
                var mouse = input.GetMouse();
                if (!AbsoluteBounds.Contains(mouse.X, mouse.Y))
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
