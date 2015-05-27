using System;
using System.Text;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a button on a user interface.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.NumericUpDown.xml")]
    public class NumericUpDown : RangeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericUpDown"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public NumericUpDown(UltravioletContext uv, String name)
            : base(uv, name)
        {
            SetDefaultValue<Double>(MinimumProperty, 0.0);
            SetDefaultValue<Double>(MaximumProperty, 100.0);
            SetDefaultValue<Double>(SmallChangeProperty, 1.0);

            InvalidateFormatString();
            InvalidatePattern();
        }

        /// <summary>
        /// Gets or sets the number of decimal places that are displayed by the control.
        /// </summary>
        public Int32 DecimalPlaces
        {
            get { return GetValue<Int32>(DecimalPlacesProperty); }
            set { SetValue<Int32>(DecimalPlacesProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DecimalPlaces"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'decimal-places'.</remarks>
        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register("DecimalPlaces", typeof(Int32), typeof(NumericUpDown),
            new PropertyMetadata<Int32>(HandleDecimalPlacesChanged));

        /// <inheritdoc/>
        protected override void OnValueChanged()
        {
            if (PART_Input != null)
            {
                PART_Input.DigestImmediately(TextBox.TextProperty);
                PART_Input.MoveEnd();
            }
            base.OnValueChanged();
        }

        /// <inheritdoc/>
        protected override void OnLostKeyboardFocus(ref RoutedEventData data)
        {
            if (PART_Input != null)
            {
                PART_Input.InvalidateDisplayCache(TextBox.TextProperty);
            }
            base.OnLostKeyboardFocus(ref data);
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            switch (key)
            {
                case Key.Up:
                    IncreaseSmall();
                    data.Handled = true;
                    break;

                case Key.Down:
                    DecreaseSmall();
                    data.Handled = true;
                    break;
            }

            base.OnKeyDown(device, key, modifiers, ref data);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="DecimalPlaces"/> dependency property changes.
        /// </summary>
        private static void HandleDecimalPlacesChanged(DependencyObject dobj, Int32 oldValue, Int32 newValue)
        {
            var updown = (NumericUpDown)dobj;
            updown.InvalidatePattern();
            updown.InvalidateFormatString();
        }

        /// <summary>
        /// Called when the ButtonUp component is clicked.
        /// </summary>
        private void Increment(DependencyObject element, ref RoutedEventData data)
        {
            IncreaseSmall();
        }

        /// <summary>
        /// Called when the ButtonDown component is clicked.
        /// </summary>
        private void Decrement(DependencyObject element, ref RoutedEventData data)
        {
            DecreaseSmall();
        }

        /// <summary>
        /// Invalidates the updown's input pattern.
        /// </summary>
        private void InvalidatePattern()
        {
            if (PART_Input == null)
                return;

            var allowNegative = Minimum < 0;
            var allowDecimals = DecimalPlaces > 0;

            var sb = new StringBuilder();

            if (allowNegative)
                sb.Append("-?");

            sb.Append("[0-9]*");

            if (allowDecimals)
                sb.Append("(\\.[0-9]{0," + DecimalPlaces + ")?");

            PART_Input.Pattern = sb.ToString();
        }

        /// <summary>
        /// Invalidates the updown's format string.
        /// </summary>
        private void InvalidateFormatString()
        {
            if (PART_Input == null)
                return;

            PART_Input.SetFormatString(TextBox.TextProperty, DecimalPlaces > 0 ? 
                "0." + new String('0', DecimalPlaces) : "0");
        }

        /// <summary>
        /// Handles the input box's <see cref="UIElement.PreviewMouseWheel"/> event.
        /// </summary>
        private void PART_Input_PreviewMouseWheel(DependencyObject element, MouseDevice device, Double x, Double y, ref RoutedEventData data)
        {
            if (PART_Input != null && PART_Input.IsFocused)
            {
                Value += y;
            }
        }

        // Component references.
        private readonly TextBox PART_Input = null;
    }
}
