using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a vertical slider.
    /// </summary>
    [Preserve(AllMembers = true)]
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.VSlider.xml")]
    public class VSlider : OrientedSlider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VSlider"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public VSlider(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            switch (key)
            {
                case Key.Up:
                    Value -= SmallChange;
                    data.Handled = true;
                    break;

                case Key.Down:
                    Value += SmallChange;
                    data.Handled = true;
                    break;
            }

            base.OnKeyDown(device, key, modifiers, data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadAxisDown(GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, RoutedEventData data)
        {
            if (GamePad.UseAxisForDirectionalNavigation)
            {
                var direction = device.GetJoystickDirectionFromAxis(axis);
                switch (direction)
                {
                    case GamePadJoystickDirection.Up:
                        Value -= SmallChange;
                        data.Handled = true;
                        break;

                    case GamePadJoystickDirection.Down:
                        Value += SmallChange;
                        data.Handled = true;
                        break;
                }
            }
            base.OnGamePadAxisDown(device, axis, value, repeat, data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {
            if (!GamePad.UseAxisForDirectionalNavigation)
            {
                switch (button)
                {
                    case GamePadButton.DPadUp:
                        Value -= SmallChange;
                        data.Handled = true;
                        break;

                    case GamePadButton.DPadDown:
                        Value += SmallChange;
                        data.Handled = true;
                        break;
                }
            }
            base.OnGamePadButtonDown(device, button, repeat, data);
        }
    }
}
