using System;
using System.Text;
using TwistedLogik.Ultraviolet.Input;
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
        public NumericUpDown(UltravioletContext uv, String id)
            : base(uv, id)
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
        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register("DecimalPlaces", typeof(Int32), typeof(NumericUpDown),
            new DependencyPropertyMetadata(HandleDecimalPlacesChanged, () => 0, DependencyPropertyOptions.None));

        /// <summary>
        /// Occurs when the value of the <see cref="DecimalPlaces"/> property changes.
        /// </summary>
        public event UpfEventHandler DecimalPlacesChanged;

        /// <inheritdoc/>
        protected override void OnValueChanged()
        {
            if (Input != null)
            {
                Input.DigestImmediately(TextBox.TextProperty);
                Input.MoveEnd();
            }
            base.OnValueChanged();
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, ref Boolean handled)
        {
            switch (key)
            {
                case Key.Up:
                    IncreaseSmall();
                    handled = true;
                    break;

                case Key.Down:
                    DecreaseSmall();
                    handled = true;
                    break;
            }

            base.OnKeyDown(device, key, modifiers, ref handled);
        }

        /// <summary>
        /// Raises the <see cref="DecimalPlacesChanged"/> event.
        /// </summary>
        protected virtual void OnDecimalPlacesChanged()
        {
            var temp = DecimalPlacesChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="DecimalPlaces"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleDecimalPlacesChanged(DependencyObject dobj)
        {
            var updown = (NumericUpDown)dobj;
            updown.InvalidatePattern();
            updown.InvalidateFormatString();
            updown.OnDecimalPlacesChanged();
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

        /// <summary>
        /// Invalidates the updown's input pattern.
        /// </summary>
        private void InvalidatePattern()
        {
            if (Input == null)
                return;

            var allowNegative = Minimum < 0;
            var allowDecimals = DecimalPlaces > 0;

            var sb = new StringBuilder();

            if (allowNegative)
                sb.Append("-?");

            sb.Append("[0-9]*");

            if (allowDecimals)
                sb.Append("(\\.[0-9]{0," + DecimalPlaces + ")?");

            Input.Pattern = sb.ToString();
        }

        /// <summary>
        /// Invalidates the updown's format string.
        /// </summary>
        private void InvalidateFormatString()
        {
            if (Input == null)
                return;

            Input.SetFormatString(TextBox.TextProperty, DecimalPlaces > 0 ? 
                "0." + new String('0', DecimalPlaces) : "0");
        }

        // Component references.
        private readonly TextBox Input = null;
    }
}
