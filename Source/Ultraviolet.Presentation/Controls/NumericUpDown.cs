using System;
using Ultraviolet.Core.Text;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a button on a user interface.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.NumericUpDown.xml")]
    public class NumericUpDown : RangeBase
    {
        /// <summary>
        /// Initializes the <see cref="NumericUpDown"/> type.
        /// </summary>
        static NumericUpDown()
        {
            EventManager.RegisterClassHandler(typeof(NumericUpDown), Mouse.PreviewMouseWheelEvent, new UpfMouseWheelEventHandler(HandlePreviewMouseWheel));
            EventManager.RegisterClassHandler(typeof(NumericUpDown), Keyboard.PreviewKeyDownEvent, new UpfKeyDownEventHandler(HandlePreviewKeyDown));
            EventManager.RegisterClassHandler(typeof(NumericUpDown), TextEditor.TextEntryValidationEvent, new UpfTextEntryValidationHandler(HandleTextEntryValidation));

            FocusableProperty.OverrideMetadata(typeof(NumericUpDown), new PropertyMetadata<Boolean>(false));
            MinimumProperty.OverrideMetadata(typeof(NumericUpDown), new PropertyMetadata<Double>(0.0));
            MaximumProperty.OverrideMetadata(typeof(NumericUpDown), new PropertyMetadata<Double>(100.0));
            SmallChangeProperty.OverrideMetadata(typeof(NumericUpDown), new PropertyMetadata<Double>(1.0));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericUpDown"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public NumericUpDown(UltravioletContext uv, String name)
            : base(uv, name)
        {
            InvalidateFormatString();
        }

        /// <summary>
        /// Gets or sets the number of decimal places that are displayed by the control.
        /// </summary>
        /// <value>An <see cref="Int32"/> value which specifies the number of decimal places
        /// that are displayed by the control.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="DecimalPlacesProperty"/></dpropField>
        ///     <dpropStylingName>decimal-places</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Int32 DecimalPlaces
        {
            get { return GetValue<Int32>(DecimalPlacesProperty); }
            set { SetValue(DecimalPlacesProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DecimalPlaces"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="DecimalPlaces"/> dependency property.</value>
        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register("DecimalPlaces", typeof(Int32), typeof(NumericUpDown),
            new PropertyMetadata<Int32>(HandleDecimalPlacesChanged));

        /// <inheritdoc/>
        protected override void OnValueChanged()
        {
            if (PART_Input != null)
            {
                PART_Input.DigestImmediately(TextBox.TextProperty);
                PART_Input.CaretIndex = PART_Input.TextLength;
            }
            base.OnValueChanged();
        }

        /// <inheritdoc/>
        protected override void OnGotKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            if (PART_Input != null)
            {
                PART_Input.Focus();
            }
            base.OnGotKeyboardFocus(device, oldFocus, newFocus, data);
        }

        /// <inheritdoc/>
        protected override void OnLostKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            if (PART_Input != null)
            {
                PART_Input.InvalidateDisplayCache(TextBox.TextProperty);
                PART_Input.CaretIndex = 0;
            }
            base.OnLostKeyboardFocus(device, oldFocus, newFocus, data);
        }
        
        /// <inheritdoc/>
        protected override void OnGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {
            switch (button)
            {
                case GamePadButton.LeftStickUp:
                    Value += SmallChange;
                    data.Handled = true;
                    break;

                case GamePadButton.LeftStickDown:
                    Value -= SmallChange;
                    data.Handled = true;
                    break;
            }

            base.OnGamePadButtonDown(device, button, repeat, data);
        }
        
        /// <summary>
        /// Occurs when the value of the <see cref="DecimalPlaces"/> dependency property changes.
        /// </summary>
        private static void HandleDecimalPlacesChanged(DependencyObject dobj, Int32 oldValue, Int32 newValue)
        {
            var updown = (NumericUpDown)dobj;
            updown.InvalidateFormatString();
        }

        /// <summary>
        /// Called when the ButtonUp component is clicked.
        /// </summary>
        private void Increment(DependencyObject element, RoutedEventData data)
        {
            Value += SmallChange;
        }

        /// <summary>
        /// Called when the ButtonDown component is clicked.
        /// </summary>
        private void Decrement(DependencyObject element, RoutedEventData data)
        {
            Value -= SmallChange;
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
        /// Handles the <see cref="Mouse.PreviewMouseWheelEvent"/> routed event.
        /// </summary>
        private static void HandlePreviewMouseWheel(DependencyObject element, MouseDevice device, Double x, Double y, RoutedEventData data)
        {
            var numericUpDown = (NumericUpDown)element;
            if (numericUpDown.PART_Input.IsKeyboardFocused)
            {
                numericUpDown.Value += y;
            }
        }

        /// <summary>
        /// Handles the <see cref="Keyboard.PreviewKeyDownEvent"/> routed event.
        /// </summary>
        private static void HandlePreviewKeyDown(DependencyObject element, KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            var numericUpDown = (NumericUpDown)element;
            if (numericUpDown.PART_Input == data.OriginalSource)
            {
                switch (key)
                {
                    case Key.Up:
                        numericUpDown.Value += numericUpDown.SmallChange;
                        data.Handled = true;
                        break;

                    case Key.Down:
                        numericUpDown.Value -= numericUpDown.SmallChange;
                        data.Handled = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Handles text entry validation for the updown's text editor.
        /// </summary>
        private static void HandleTextEntryValidation(DependencyObject element, StringSegment text, Int32 offset, Char character, TextEntryValidationRoutedEventData data)
        {
            var numericUpDown = (NumericUpDown)element;
            if (numericUpDown.PART_Input != data.OriginalSource)
                return;

            data.Handled = true;

            // Negative sign must be inserted at the beginning of the text.
            // Negative sign is only allowed if Minimum is less than zero.
            if (character == '-')
            {
                if (offset > 0 || numericUpDown.Minimum >= 0)
                    data.Valid = false;

                return;
            }

            // Nothing can be inserted before the negative sign, if there is one.
            var negativeSignPos = text.IndexOf('-');
            if (negativeSignPos >= 0 && offset < 1)
            {
                data.Valid = false;
                return;
            }
            
            // Decimal separator can only be inserted if we allow decimal points.
            // Decimal separator can only be inserted if it doesn't introduce more than the allowed number of decimals.
            var decimalSeparatorPos = text.IndexOf('.');
            if (character == '.')
            {
                if (decimalSeparatorPos >= 0 || numericUpDown.DecimalPlaces == 0)
                {
                    data.Valid = false;
                    return;
                }

                var decimalsIntroduced = text.Length - offset;
                if (decimalsIntroduced > numericUpDown.DecimalPlaces)
                    data.Valid = false;

                return;
            }

            // Non-digit characters cannot be inserted.
            if (!Char.IsDigit(character))
            {
                data.Valid = false;
                return;
            }

            // Post-decimal digits can only be inserted if we have fewer than DecimalPlaces digits there already.
            var decimalCount = (decimalSeparatorPos < 0) ? 0 : text.Length - (decimalSeparatorPos + 1);
            if (decimalSeparatorPos >= 0 && decimalSeparatorPos < offset && decimalCount >= numericUpDown.DecimalPlaces)
            {
                data.Valid = false;
                return;
            }
        }

        // Component references.
        private readonly TextBox PART_Input = null;
    }
}
