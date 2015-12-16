using System;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a horizontal slider.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.HSlider.xml")]
    public class HSlider : SliderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HSlider"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public HSlider(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (PART_Track != null)
            {
                PART_Track.InvalidateMeasure();
            }
            return base.MeasureOverride(availableSize);
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            switch (key)
            {
                case Key.Left:
                    DecreaseSmall();
                    data.Handled = true;
                    break;

                case Key.Right:
                    IncreaseSmall();
                    data.Handled = true;
                    break;
            }

            base.OnKeyDown(device, key, modifiers, ref data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadAxisDown(GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, ref RoutedEventData data)
        {
            if (GamePad.UseAxisForDirectionalNavigation)
            {
                var direction = device.GetJoystickDirectionFromAxis(axis);
                switch (direction)
                {
                    case GamePadJoystickDirection.Left:
                        DecreaseSmall();
                        data.Handled = true;
                        break;

                    case GamePadJoystickDirection.Right:
                        IncreaseSmall();
                        data.Handled = true;
                        break;
                }
            }
            base.OnGamePadAxisDown(device, axis, value, repeat, ref data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, ref RoutedEventData data)
        {
            if (!GamePad.UseAxisForDirectionalNavigation)
            {
                switch (button)
                {
                    case GamePadButton.DPadLeft:
                        DecreaseSmall();
                        data.Handled = true;
                        break;

                    case GamePadButton.DPadRight:
                        IncreaseSmall();
                        data.Handled = true;
                        break;
                }
            }
            base.OnGamePadButtonDown(device, button, repeat, ref data);
        }

        // Component references.
        private readonly Track PART_Track = null;
    }
}
