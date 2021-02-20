using System;
using System.Security;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a control designed for securely entering passwords.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.PasswordBox.xml")]
    public sealed class PasswordBox : TextEditingControl
    {
        /// <summary>
        /// Initializes the <see cref="PasswordBox"/> type.
        /// </summary>
        static PasswordBox()
        {
            EventManager.RegisterClassHandler(typeof(PasswordBox), TextBoxBase.SelectionChangedEvent, new UpfRoutedEventHandler(HandleSelectionChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordBox"/> control.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public PasswordBox(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets the password which has been entered into the password box.
        /// </summary>
        /// <returns>A string containing the password which has been entered into the password box.</returns>
        public String GetPassword()
        {
            if (PART_Editor != null)
                return PART_Editor.GetPassword();

            return String.Empty;
        }

        /// <summary>
        /// Gets the password which has been entered into the password box.
        /// </summary>
        /// <param name="stringBuilder">A <see cref="StringBuilder"/> to populate with the password which has been entered into the password box.</param>
        public void GetPassword(StringBuilder stringBuilder)
        {
            Contract.Require(stringBuilder, nameof(stringBuilder));

            if (PART_Editor != null)
                PART_Editor.GetPassword(stringBuilder);
        }

        /// <summary>
        /// Sets the password which has been entered into the password box.
        /// </summary>
        /// <param name="value">A <see cref="String"/> containing the password to set.</param>
        public void SetPassword(String value)
        {
            if (PART_Editor != null)
                PART_Editor.SetPassword(value);
        }

        /// <summary>
        /// Sets the password which has been entered into the password box.
        /// </summary>
        /// <param name="value">A <see cref="StringBuilder"/> containing password to set.</param>
        public void SetPassword(StringBuilder value)
        {
            if (PART_Editor != null)
                PART_Editor.SetPassword(value);
        }

        /// <summary>
        /// Sets the password which has been entered into the password box.
        /// </summary>
        /// <param name="value">A <see cref="StringSegment"/> containing password to set.</param>
        public void SetPassword(StringSegment value)
        {
            if (PART_Editor != null)
                PART_Editor.SetPassword(value);
        }

        /// <summary>
        /// Clears the password box's content.
        /// </summary>
        public void Clear()
        {
            if (PART_Editor != null)
                PART_Editor.Clear();
        }

        /// <summary>
        /// Selects the specified range of text.
        /// </summary>
        /// <param name="start">The index of the first character to select.</param>
        /// <param name="length">The number of characters to select.</param>
        public void Select(Int32 start, Int32 length)
        {
            if (PART_Editor != null)
                PART_Editor.Select(start, length);
        }

        /// <summary>
        /// Selects the entirety of the editor's text.
        /// </summary>
        public void SelectAll()
        {
            if (PART_Editor != null)
                PART_Editor.SelectAll();
        }
        
        /// <summary>
        /// Pastes the contents of the clipboard at the current caret position.
        /// </summary>
        public void Paste()
        {
            if (PART_Editor != null)
                PART_Editor.Paste();
        }

        /// <summary>
        /// Gets or sets the character which is used to mask the entered password.
        /// </summary>
        /// <value>A <see cref="Char"/> value which represents the character that is used
        /// to make the password box's password. The default value is '*'.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="PasswordCharProperty"/></dpropField>
        ///     <dpropStylingName>password-char</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Char PasswordChar
        {
            get { return GetValue<Char>(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }
        
        /// <summary>
        /// Gets an instance of <see cref="SecureString"/> that represents the password box's current content.
        /// </summary>
        /// <value>A <see cref="SecureString"/> that represents the password box's current content.</value>
        public SecureString SecurePassword
        {
            get
            {
                if (PART_Editor != null)
                    return PART_Editor.SecurePassword;

                return null;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the selection highlight is displayed when the password box does not have focus.
        /// </summary>
        /// <value><see langword="true"/> if the selection highlight is displayed when the text box does not have focus;
        /// otherwise, <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="IsInactiveSelectionHighlightEnabledProperty"/></dpropField>
        ///		<dpropStylingName>inactive-selection-highlight-enabled</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsInactiveSelectionHighlightEnabled
        {
            get { return GetValue<Boolean>(IsInactiveSelectionHighlightEnabledProperty); }
            set { SetValue(IsInactiveSelectionHighlightEnabledProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the password box has focus and selected text.
        /// </summary>
        /// <value><see langword="true"/> if the password box is focused and has selected text; otherwise,
        /// <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="IsSelectionActiveProperty"/></dpropField>
        ///		<dpropStylingName>selection-active</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsSelectionActive
        {
            get { return GetValue<Boolean>(IsSelectionActiveProperty); }
        }

        /// <summary>
        /// Gets or sets the maximum length of the password which is entered into the password box.
        /// </summary>
        /// <value>An <see cref="Int32"/> which specifies the maximum length of the password which is entered
        /// into the password box, or 0 if the password has no maximum length. The default value is 0.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="MaxLengthProperty"/></dpropField>
        ///     <dpropStylingName>max-length</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Int32 MaxLength
        {
            get { return GetValue<Int32>(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        /// <summary>
        /// Occurs when the password box's password changes.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PasswordChangedEvent"/></revtField>
        ///     <revtStylingName>password-changed</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler PasswordChanged
        {
            add { AddHandler(PasswordChangedEvent, value); }
            remove { RemoveHandler(PasswordChangedEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="PasswordChar"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="PasswordChar"/> dependency property.</value>
        public static readonly DependencyProperty PasswordCharProperty = DependencyProperty.Register("PasswordChar", typeof(Char), typeof(PasswordBox),
            new PropertyMetadata<Char>('*', PropertyMetadataOptions.None, HandlePasswordCharChanged));

        /// <summary>
        /// Identifies the <see cref="IsInactiveSelectionHighlightEnabled"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsInactiveSelectionHighlightEnabled"/> dependency property.</value>
        public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty = 
            TextBoxBase.IsInactiveSelectionHighlightEnabledProperty.AddOwner(typeof(PasswordBox));

        /// <summary>
        /// Identifies the <see cref="IsSelectionActive"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsSelectionActive"/> dependency property.</value>
        public static readonly DependencyProperty IsSelectionActiveProperty =
            TextBoxBase.IsSelectionActiveProperty.AddOwner(typeof(PasswordBox));

        /// <summary>
        /// Identifies the <see cref="MaxLength"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="MaxLength"/> dependency property.</value>
        public static readonly DependencyProperty MaxLengthProperty = 
            TextBox.MaxLengthProperty.AddOwner(typeof(PasswordBox));

        /// <summary>
        /// Identifies the <see cref="PasswordChanged"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="PasswordChanged"/> routed event.</value>
        public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent("PasswordChanged",
            RoutingStrategy.Bubble, typeof(UpfRoutedEventHandler), typeof(PasswordBox));
        
        /// <inheritdoc/>
        protected override void OnQueryCursor(MouseDevice device, CursorQueryRoutedEventData data)
        {
            if (IsMouseCaptured && IsMouseWithinEditor())
            {
                data.Cursor = PART_Editor?.Cursor.Resource?.Cursor ?? data.Cursor;
                data.Handled = true;
            }
            base.OnQueryCursor(device, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseDown(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left)
                Focus();                

            if (PART_Editor != null && IsMouseWithinEditor())
            {
                CaptureMouse();
                PART_Editor.HandleMouseDown(device, button, data);
                data.Handled = true;
            }

            base.OnPreviewMouseDown(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseUp(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left)
                ReleaseMouseCapture();

            if (PART_Editor != null && IsMouseWithinEditor())
            {
                PART_Editor.HandleMouseUp(device, button, data);
                data.Handled = true;
            }

            base.OnPreviewMouseUp(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseDoubleClick(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (PART_Editor != null && IsMouseWithinEditor())
            {
                PART_Editor.HandleMouseDoubleClick(device, button, data);
                data.Handled = true;
            }

            base.OnPreviewMouseDoubleClick(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, RoutedEventData data)
        {
            if (PART_Editor != null)
            {
                var capture = Mouse.GetCaptured(View);
                if (capture == null || capture == this)
                {
                    PART_Editor.HandleMouseMove(device, data);
                    data.Handled = true;
                }
            }

            base.OnPreviewMouseMove(device, x, y, dx, dy, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewTouchDown(TouchDevice device, Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            if (!Ultraviolet.GetInput().IsMouseCursorAvailable && device.IsFirstTouchInGesture(id))
                Focus();

            if (PART_Editor != null && IsTouchWithinEditor(id))
            {
                CaptureTouch(id);
                PART_Editor.HandleTouchDown(device, id, x, y, pressure, data);
                data.Handled = true;
            }

            UpdateTextInputRegion();
            Ultraviolet.GetInput().ShowSoftwareKeyboard();

            base.OnPreviewTouchDown(device, id, x, y, pressure, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewTouchUp(TouchDevice device, Int64 id, RoutedEventData data)
        {
            if (PART_Editor != null && IsTouchWithinEditor(id))
            {
                PART_Editor.HandleTouchUp(device, id, data);
                data.Handled = true;
            }

            base.OnPreviewTouchUp(device, id, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewTouchLongPress(TouchDevice device, Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            if (PART_Editor != null && IsTouchWithinEditor(id))
            {
                PART_Editor.HandleTouchLongPress(device, id, x, y, pressure, data);
                data.Handled = true;
            }

            base.OnPreviewTouchLongPress(device, id, x, y, pressure, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewTouchMove(TouchDevice device, Int64 id, Double x, Double y, Double dx, Double dy, Single pressure, RoutedEventData data)
        {
            if (PART_Editor != null)
            {
                var capture = Touch.GetCaptured(View, id);
                if (capture == this || capture == null)
                {
                    PART_Editor.HandleTouchMove(device, id, x, y, dx, dy, pressure, data);
                    data.Handled = true;
                }
            }

            base.OnPreviewTouchMove(device, id, x, y, dx, dy, pressure, data);
        }

        /// <inheritdoc/>
        protected override void OnLostMouseCapture(RoutedEventData data)
        {
            if (PART_Editor != null)
                PART_Editor.HandleLostMouseCapture();

            data.Handled = true;
            base.OnLostMouseCapture(data);
        }

        /// <inheritdoc/>
        protected override void OnGotKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            UpdateTextInputRegion();
            Ultraviolet.GetInput().ShowSoftwareKeyboard(KeyboardMode.Text);

            if (PART_Editor != null)
                PART_Editor.HandleGotKeyboardFocus();

            UpdateIsSelectionActive();

            base.OnGotKeyboardFocus(device, oldFocus, newFocus, data);
        }

        /// <inheritdoc/>
        protected override void OnLostKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            Ultraviolet.GetInput().HideSoftwareKeyboard();

            if (PART_Editor != null)
                PART_Editor.HandleLostKeyboardFocus();

            UpdateIsSelectionActive();

            base.OnLostKeyboardFocus(device, oldFocus, newFocus, data);
        }

        /// <inheritdoc/>
        protected override void OnTextInput(KeyboardDevice device, RoutedEventData data)
        {
            if (PART_Editor != null)
                PART_Editor.HandleTextInput(device, data);

            base.OnTextInput(device, data);
        }

        /// <inheritdoc/>
        protected override void OnTextEditing(KeyboardDevice device, RoutedEventData data)
        {
            if (PART_Editor != null)
                PART_Editor.HandleTextEditing(device, data);

            base.OnTextEditing(device, data);
        }

        /// <inheritdoc/>
        protected override TextEditor TextEditor => PART_Editor;

        /// <summary>
        /// Occurs when the control handles a <see cref="TextBoxBase.SelectionChangedEvent"/> routed event.
        /// </summary>
        private static void HandleSelectionChanged(DependencyObject dobj, RoutedEventData data)
        {
            var passwordBox = (PasswordBox)dobj;
            passwordBox.UpdateIsSelectionActive();

            if (passwordBox.PART_Editor != null && data.OriginalSource == passwordBox.PART_Editor)
                data.Handled = true;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="PasswordChar"/> dependency property changes.
        /// </summary>
        private static void HandlePasswordCharChanged(DependencyObject element, Char oldValue, Char newValue)
        {
            var passwordBox = (PasswordBox)element;
            if (passwordBox.PART_Editor != null)
                passwordBox.PART_Editor.ReplaceTextWithMask(newValue);
        }
        
        /// <summary>
        /// Gets a value indicating whether the mouse is currently inside of the editor.
        /// </summary>
        private Boolean IsMouseWithinEditor()
        {
            var mouseTarget = (UIElement)PART_Editor ?? this;
            var mouseBounds = mouseTarget.Bounds;

            return mouseBounds.Contains(Mouse.GetPosition(mouseTarget));
        }

        /// <summary>
        /// Gets a value indicating whether the specified touch is currently inside of the editor.
        /// </summary>
        private Boolean IsTouchWithinEditor(Int64 id)
        {
            var mouseTarget = (UIElement)PART_Editor ?? this;
            if (Mouse.GetCaptured(View) != null && !mouseTarget.IsMouseCaptureWithin)
                return false;

            var mouseBounds = mouseTarget.Bounds;
            return mouseBounds.Contains(Mouse.GetPosition(mouseTarget));
        }

        /// <summary>
        /// Updates the value of the <see cref="IsSelectionActive"/> property.
        /// </summary>
        private void UpdateIsSelectionActive()
        {
            var isSelectionActive = IsKeyboardFocusWithin;

            if (PART_Editor == null || PART_Editor.SelectionLength == 0)
                isSelectionActive = false;

            var oldValue = GetValue<Boolean>(IsSelectionActiveProperty);
            if (oldValue != isSelectionActive)
            {
                SetValue(TextBoxBase.IsSelectionActivePropertyKey, isSelectionActive);
            }
        }

        /// <summary>
        /// Updates the text input region so that this control will be panned into view while
        /// the software keyboard is open.
        /// </summary>
        private void UpdateTextInputRegion(Boolean clear = false)
        {
            var service = SoftwareKeyboardService.Create();
            service.TextInputRegion = clear ? (Ultraviolet.Rectangle?)null :
                (Ultraviolet.Rectangle)Display.DipsToPixels(CalculateTransformedVisualBounds());
        }

        // Component references.
        private readonly PasswordEditor PART_Editor = null;        
    }
}
