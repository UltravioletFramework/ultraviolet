using System;
using System.ComponentModel;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents an element which allows the user to edit multiple lines of text.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.TextArea.xml")]
    [DefaultProperty("Text")]
    public class TextArea : Control
    {
        /// <summary>
        /// Initializes the <see cref="TextArea"/> type.
        /// </summary>
        static TextArea()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextArea"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TextArea(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }
        
        /// <summary>
        /// Gets the text area's text.
        /// </summary>
        /// <returns>A <see cref="String"/> instance containing the text area's text.</returns>
        public String GetText()
        {
            return GetValue<VersionedStringSource>(TextProperty).ToString();
        }

        /// <summary>
        /// Gets the text area's text.
        /// </summary>
        /// <param name="stringBuilder">A <see cref="StringBuilder"/> instance to populate with the text area's text.</param>
        public void GetText(StringBuilder stringBuilder)
        {
            Contract.Require(stringBuilder, "stringBuilder");

            var value = GetValue<VersionedStringSource>(TextProperty);

            stringBuilder.Length = 0;
            stringBuilder.AppendVersionedStringSource(value);
        }

        /// <summary>
        /// Sets the text area's text.
        /// </summary>
        /// <param name="value">A <see cref="String"/> instance to set as the text area's text.</param>
        public void SetText(String value)
        {
            SetValue(TextProperty, new VersionedStringSource(value));
        }

        /// <summary>
        /// Sets the text area's text.
        /// </summary>
        /// <param name="value">A <see cref="StringBuilder"/> instance whose contents will be set as the text area's text.</param>
        public void SetText(StringBuilder value)
        {
            if (PART_Editor != null)
                PART_Editor.HandleTextChanged(value);
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
        /// Selects the word, whitespace, or symbol at the current caret position.
        /// </summary>
        public void SelectCurrentToken()
        {
            if (PART_Editor != null)
                PART_Editor.SelectCurrentToken();
        }

        /// <summary>
        /// Copies the currently selected text onto the clipboard.
        /// </summary>
        public void Copy()
        {
            if (PART_Editor != null)
                PART_Editor.Copy();
        }

        /// <summary>
        /// Cuts the currently selected text onto the clipboard.
        /// </summary>
        public void Cut()
        {
            if (PART_Editor != null)
                PART_Editor.Cut();
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
        /// Gets or sets the text area's current insertion mode.
        /// </summary>
        public TextBoxInsertionMode InsertionMode
        {
            get { return GetValue<TextBoxInsertionMode>(InsertionModeProperty); }
            set { SetValue(InsertionModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value specifying how the text area's text wraps when it reaches the edge of its container.
        /// </summary>
        public TextWrapping TextWrapping
        {
            get { return GetValue<TextWrapping>(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a <see cref="ScrollBarVisibility"/> value which specifies whether the text area's horizontal scroll bar is visible.
        /// </summary>
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return GetValue<ScrollBarVisibility>(HorizontalScrollBarVisibilityProperty); }
            set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets a <see cref="ScrollBarVisibility"/> value which specifies whether the text area's vertical scroll bar is visible.
        /// </summary>
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return GetValue<ScrollBarVisibility>(VerticalScrollBarVisibilityProperty); }
            set { SetValue(VerticalScrollBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text area will accept the return key as a normal character.
        /// </summary>
        public Boolean AcceptsReturn
        {
            get { return GetValue<Boolean>(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text area will accept the tab key as a normal character. If false,
        /// tab will instead be used for tab navigation.
        /// </summary>
        public Boolean AcceptsTab
        {
            get { return GetValue<Boolean>(AcceptsTabProperty); }
            set { SetValue(AcceptsTabProperty, value); }
        }

        /// <summary>
        /// Gets or sets the starting point of the selected text.
        /// </summary>
        public Int32 SelectionStart
        {
            get
            {
                if (PART_Editor != null)
                    return PART_Editor.SelectionStart;

                return 0;
            }
            set
            {
                if (PART_Editor != null)
                    PART_Editor.SelectionStart = value;
            }
        }

        /// <summary>
        /// Gets or sets the length of the selected text.
        /// </summary>
        public Int32 SelectionLength
        {
            get
            {
                if (PART_Editor != null)
                    return PART_Editor.SelectionLength;

                return 0;
            }
            set
            {
                if (PART_Editor != null)
                    PART_Editor.SelectionLength = value;
            }
        }

        /// <summary>
        /// Gets the currently selected text.
        /// </summary>
        public String SelectedText
        {
            get
            {
                if (PART_Editor != null)
                    return PART_Editor.SelectedText;

                return String.Empty;
            }
            set
            {
                if (PART_Editor != null)
                    PART_Editor.SelectedText = value;
            }
        }

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(VersionedStringSource), typeof(TextArea),
            new PropertyMetadata<VersionedStringSource>(VersionedStringSource.Invalid, PropertyMetadataOptions.None, HandleTextChanged));

        /// <summary>
        /// Identifies the <see cref="InsertionMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InsertionModeProperty = DependencyProperty.Register("InsertionMode", typeof(TextBoxInsertionMode), typeof(TextArea),
            new PropertyMetadata<TextBoxInsertionMode>(TextBoxInsertionMode.Insert, PropertyMetadataOptions.None, HandleInsertionModeChanged));

        /// <summary>
        /// Identifies the <see cref="TextWrapping"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(TextArea),
            new PropertyMetadata<TextWrapping>(TextWrapping.NoWrap, PropertyMetadataOptions.AffectsMeasure, HandleTextWrappingChanged));

        /// <summary>
        /// Identifies the <see cref="HorizontalScrollBarVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register("HorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(TextArea),
            new PropertyMetadata<ScrollBarVisibility>(ScrollBarVisibility.Hidden, PropertyMetadataOptions.None, null, CoerceHorizontalScrollBarVisibility));

        /// <summary>
        /// Identifies the <see cref="VerticalScrollBarVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(TextArea),
            new PropertyMetadata<ScrollBarVisibility>(ScrollBarVisibility.Hidden, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="AcceptsReturn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AcceptsReturnProperty = KeyboardNavigation.AcceptsReturnProperty.AddOwner(typeof(TextArea));

        /// <summary>
        /// Identifies the <see cref="AcceptsTab"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AcceptsTabProperty = DependencyProperty.Register("AcceptsTab", typeof(Boolean), typeof(TextArea),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                Focus();
                CaptureMouse();
            }

            if (PART_Editor != null && IsMouseWithinEditor())
                PART_Editor.HandleMouseDown(device, button, ref data);

            data.Handled = true;
            base.OnMouseDown(device, button, ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                ReleaseMouseCapture();
            }

            if (PART_Editor != null && IsMouseWithinEditor())
                PART_Editor.HandleMouseUp(device, button, ref data);

            data.Handled = true;
            base.OnMouseUp(device, button, ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseDoubleClick(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (IsMouseWithinEditor() && PART_Editor != null)
                PART_Editor.HandleMouseDoubleClick(device, button, ref data);

            base.OnMouseDoubleClick(device, button, ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, ref RoutedEventData data)
        {
            if (PART_Editor != null)
                PART_Editor.HandleMouseMove(device, ref data);

            data.Handled = true;
            base.OnMouseMove(device, x, y, dx, dy, ref data);
        }

        /// <inheritdoc/>
        protected override void OnLostMouseCapture(ref RoutedEventData data)
        {
            if (PART_Editor != null)
                PART_Editor.HandleLostMouseCapture();

            data.Handled = true;
            base.OnLostMouseCapture(ref data);
        }

        /// <inheritdoc/>
        protected override void OnGotKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, ref RoutedEventData data)
        {
            if (PART_Editor != null)
                PART_Editor.HandleGotKeyboardFocus();

            base.OnGotKeyboardFocus(device, oldFocus, newFocus, ref data);
        }

        /// <inheritdoc/>
        protected override void OnLostKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, ref RoutedEventData data)
        {
            if (PART_Editor != null)
                PART_Editor.HandleLostKeyboardFocus();

            base.OnLostKeyboardFocus(device, oldFocus, newFocus, ref data);
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            if (PART_Editor != null)
                PART_Editor.HandleKeyDown(device, key, modifiers, ref data);

            if (!data.Handled)
            {
                switch (key)
                {
                    case Key.Insert:
                        InsertionMode = (InsertionMode == TextBoxInsertionMode.Insert) ?
                            TextBoxInsertionMode.Overwrite :
                            TextBoxInsertionMode.Insert;
                        data.Handled = true;
                        break;
                }
            }
               
            base.OnKeyDown(device, key, modifiers, ref data);
        }

        /// <inheritdoc/>
        protected override void OnTextInput(KeyboardDevice device, ref RoutedEventData data)
        {
            if (PART_Editor != null)
                PART_Editor.HandleTextInput(device, ref data);

            base.OnTextInput(device, ref data);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Text"/> dependency property changes.
        /// </summary>
        private static void HandleTextChanged(DependencyObject dobj, VersionedStringSource oldValue, VersionedStringSource newValue)
        {
            var textArea = (TextArea)dobj;
            if (textArea.PART_Editor != null)
                textArea.PART_Editor.HandleTextChanged(newValue);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="InsertionMode"/> dependency property changes.
        /// </summary>
        private static void HandleInsertionModeChanged(DependencyObject dobj, TextBoxInsertionMode oldValue, TextBoxInsertionMode newValue)
        {
            var textArea = (TextArea)dobj;
            if (textArea.PART_Editor != null)
                textArea.PART_Editor.InsertionMode = newValue;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TextWrapping"/> dependency property changes.
        /// </summary>
        private static void HandleTextWrappingChanged(DependencyObject dobj, TextWrapping oldValue, TextWrapping newValue)
        {
            var textArea = (TextArea)dobj;
            textArea.CoerceValue(HorizontalScrollBarVisibilityProperty);

            if (textArea.PART_Editor != null)
                textArea.PART_Editor.InvalidateMeasure();
        }

        /// <summary>
        /// Coerces the value of the <see cref="HorizontalScrollBarVisibility"/> property to force the scroll bar
        /// to a disabled state when wrapping is enabled.
        /// </summary>
        private static ScrollBarVisibility CoerceHorizontalScrollBarVisibility(DependencyObject dobj, ScrollBarVisibility value)
        {
            var textArea = (TextArea)dobj;
            if (textArea.TextWrapping == TextWrapping.Wrap)
            {
                return ScrollBarVisibility.Disabled;
            }
            return value;
        }

        /// <summary>
        /// Gets a value indicating whether the mouse is currently inside of the editor.
        /// </summary>
        private Boolean IsMouseWithinEditor()
        {
            var mouseTarget = (Control)PART_ScrollViewer ?? this;
            var mouseBounds = mouseTarget.Bounds;

            return mouseBounds.Contains(Mouse.GetPosition(mouseTarget));
        }

        // Component references.
        private readonly ScrollViewer PART_ScrollViewer = null;
        private readonly TextAreaEditor PART_Editor = null;
    }
}
