using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a button on a user interface.
    /// </summary>
    [UIElement("NumericUpDown", "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.NumericUpDown.xml")]
    public class NumericUpDown : RangeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericUpDown"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public NumericUpDown(UltravioletContext uv, String id)
            : base(uv, id)
        {
            SetDefaultValue<Double>(MinimumProperty, 0.0);
            SetDefaultValue<Double>(MaximumProperty, 100.0);
        }

        /// <summary>
        /// Handles the <see cref="UIElement.KeyPressed"/> event for the Input component.
        /// </summary>
        private void InputKeyPressed(UIElement element, KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat)
        {
            switch (key)
            {
                case Key.Up:
                    IncreaseSmall();
                    break;

                case Key.Down:
                    DecreaseSmall();
                    break;
            }

            base.OnKeyPressed(device, key, ctrl, alt, shift, repeat);
        }

        /// <summary>
        /// Called when the ButtonUp component is clicked.
        /// </summary>
        private void Increment(UIElement element)
        {
            IncreaseSmall();
        }

        /// <summary>
        /// Called when the ButtonDown component is clicked.
        /// </summary>
        private void Decrement(UIElement element)
        {
            DecreaseSmall();
        }
    }
}
