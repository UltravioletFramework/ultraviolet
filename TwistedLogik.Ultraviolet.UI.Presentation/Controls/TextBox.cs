using System;
using System.ComponentModel;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents an element which allows the user to edit a string of text.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.TextBox.xml")]
    [DefaultProperty("Text")]
    public class TextBox : TextBoxBase
    {
        /// <summary>
        /// Initializes the <see cref="TextBox"/> type.
        /// </summary>
        static TextBox()
        {
            EventManager.RegisterClassHandler(typeof(TextBox), ScrollViewer.ScrollChangedEvent, new UpfScrollChangedEventHandler(HandleScrollChanged));

            HorizontalScrollBarVisibilityProperty.OverrideMetadata(typeof(TextBox),
                new PropertyMetadata<ScrollBarVisibility>(ScrollBarVisibility.Hidden, null, CoerceHorizontalScrollBarVisibility));

            IsReadOnlyProperty.OverrideMetadata(typeof(TextBox),
                new PropertyMetadata<Boolean>(HandleIsReadOnlyChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TextBox(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets the text box's text.
        /// </summary>
        /// <returns>A <see cref="String"/> instance containing the text box's text.</returns>
        public String GetText()
        {
            return GetValue<VersionedStringSource>(TextProperty).ToString();
        }

        /// <summary>
        /// Gets the text box's text.
        /// </summary>
        /// <param name="stringBuilder">A <see cref="StringBuilder"/> instance to populate with the text box's text.</param>
        public void GetText(StringBuilder stringBuilder)
        {
            Contract.Require(stringBuilder, "stringBuilder");

            var value = GetValue<VersionedStringSource>(TextProperty);

            stringBuilder.Length = 0;
            stringBuilder.AppendVersionedStringSource(value);
        }

        /// <summary>
        /// Sets the text box's text.
        /// </summary>
        /// <param name="value">A <see cref="String"/> instance to set as the text box's text.</param>
        public void SetText(String value)
        {
            SetValue(TextProperty, new VersionedStringSource(value));
        }

        /// <summary>
        /// Sets the text box's text.
        /// </summary>
        /// <param name="value">A <see cref="StringBuilder"/> instance whose contents will be set as the text box's text.</param>
        public void SetText(StringBuilder value)
        {
            if (TextEditor != null)
                TextEditor.HandleTextChanged(value);
        }

        /// <summary>
        /// Gets the selected text.
        /// </summary>
        /// <returns>A string containing the selected text.</returns>
        public String GetSelectedText()
        {
            if (TextEditor != null)
                return TextEditor.GetSelectedText();

            return null;
        }

        /// <summary>
        /// Gets the selected text.
        /// </summary>
        /// <param name="stringBuilder">A <see cref="StringBuilder"/> to populate with the contents of the selection.</param>
        public void GetSelectedText(StringBuilder stringBuilder)
        {
            if (TextEditor != null)
                TextEditor.GetSelectedText(stringBuilder);
        }

        /// <summary>
        /// Sets the selected text.
        /// </summary>
        /// <param name="value">The text to set.</param>
        public void SetSelectedText(String value)
        {
            if (TextEditor != null)
                TextEditor.SetSelectedText(value);
        }

        /// <summary>
        /// Sets the selected text.
        /// </summary>
        /// <param name="value">A <see cref="StringBuilder"/> containing the text to set.</param>
        public void SetSelectedText(StringBuilder value)
        {
            if (TextEditor != null)
                TextEditor.SetSelectedText(value);
        }

        /// <summary>
        /// Gets the specified line of text.
        /// </summary>
        /// <param name="lineIndex">The index of the line of text to retrieve.</param>
        /// <returns>A string containing the contents of the specified line of text.</returns>
        public String GetLineText(Int32 lineIndex)
        {
            if (TextEditor != null)
                return TextEditor.GetLineText(lineIndex);

            return null;
        }

        /// <summary>
        /// Gets the specified line of text.
        /// </summary>
        /// <param name="lineIndex">The index of the line of text to retrieve.</param>
        /// <param name="stringBuilder">A <see cref="StringBuilder"/> to populate with the contents of the specified line of text.</param>
        public void GetLineText(Int32 lineIndex, StringBuilder stringBuilder)
        {
            if (TextEditor != null)
                TextEditor.GetLineText(lineIndex, stringBuilder);
        }

        /// <summary>
        /// Scrolls the text box so that the specified line is in full view.
        /// </summary>
        /// <param name="lineIndex">The index of the line to scroll into view.</param>
        public void ScrollToLine(Int32 lineIndex)
        {
            if (TextEditor != null)
                TextEditor.ScrollToLine(lineIndex);
        }

        /// <summary>
        /// Gets the index of the first character on the specified line.
        /// </summary>
        /// <param name="lineIndex">The index of the line for which to retrieve a character index.</param>
        /// <returns>The index of the first character on the specified line.</returns>
        public Int32 GetCharacterIndexFromLineIndex(Int32 lineIndex)
        {
            if (TextEditor != null)
                return TextEditor.GetCharacterIndexFromLineIndex(lineIndex);

            return 0;
        }

        /// <summary>
        /// Gets the index of the character that is closest to the specified point.
        /// </summary>
        /// <param name="point">A point in client space to evaluate.</param>
        /// <param name="snapToText">A value indicating that the closest character should be returned (if true), 
        /// or that only characters directly under the specified point should be returned (if false).</param>
        /// <returns>The index of the character at the specified point, or -1 if there is no character at the specified point.</returns>
        public Int32 GetCharacterIndexFromPoint(Point2D point, Boolean snapToText)
        {
            if (TextEditor != null)
                return TextEditor.GetCharacterIndexFromPoint(point, snapToText);

            return -1;
        }

        /// <summary>
        /// Gets the index of the first visible line of text.
        /// </summary>
        /// <returns>The index of the first visible line of text.</returns>
        public Int32 GetFirstVisibleLineIndex()
        {
            if (TextEditor != null)
                return TextEditor.GetFirstVisibleLineIndex();

            return 0;
        }

        /// <summary>
        /// Gets the index of the last visible line of text.
        /// </summary>
        /// <returns>The index of the last visible line of text.</returns>
        public Int32 GetLastVisibleLineIndex()
        {
            if (TextEditor != null)
                return TextEditor.GetLastVisibleLineIndex();

            return 0;
        }

        /// <summary>
        /// Gets the index of the line of text that contains the specified character.
        /// </summary>
        /// <param name="charIndex">The index of the character to evaluate.</param>
        /// <returns>The index of the line of text that contains the specified character.</returns>
        public Int32 GetLineIndexFromCharacterIndex(Int32 charIndex)
        {
            if (TextEditor != null)
                return TextEditor.GetLineIndexFromCharacterIndex(charIndex);

            return 0;
        }

        /// <summary>
        /// Gets the number of characters on the specified line of text.
        /// </summary>
        /// <param name="lineIndex">The index of the line to evaluate.</param>
        /// <returns>The number of characters on the specified line of text.</returns>
        public Int32 GetLineLength(Int32 lineIndex)
        {
            if (TextEditor != null)
                return TextEditor.GetLineLength(lineIndex);

            return 0;
        }

        /// <summary>
        /// Gets a rectangle that represents the leading edge of the specified character.
        /// </summary>
        /// <param name="charIndex">The index of the character for which to retrieve the rectangle.</param>
        /// <returns>A rectangle which represents the bounds of the leading edge of the specified character,
        /// or <see cref="RectangleD.Empty"/> if the bounding rectangle cannot be determined.</returns>
        public RectangleD GetRectFromCharacterIndex(Int32 charIndex)
        {
            if (TextEditor != null)
                return TextEditor.GetRectFromCharacterIndex(charIndex);

            return RectangleD.Empty;
        }

        /// <summary>
        /// Gets a rectangle that represents the leading or trailing edge of the specified character.
        /// </summary>
        /// <param name="charIndex">The index of the character for which to retrieve the rectangle.</param>
        /// <param name="trailingEdge">A value specifying whether to retrieve the trailing edge of the character.</param>
        /// <returns>A rectangle which represents the bounds of the leading or trailing edge of the specified character,
        /// or <see cref="RectangleD.Empty"/> if the bounding rectangle cannot be determined.</returns>
        public RectangleD GetRectFromCharacterIndex(Int32 charIndex, Boolean trailingEdge)
        {
            if (TextEditor != null)
                return TextEditor.GetRectFromCharacterIndex(charIndex, trailingEdge);

            return RectangleD.Empty;
        }

        /// <summary>
        /// Gets or sets the text box's current keyboard mode.
        /// </summary>
        public KeyboardMode KeyboardMode
        {
            get { return GetValue<KeyboardMode>(KeyboardModeProperty); }
            set { SetValue(KeyboardModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a <see cref="CharacterCasing"/> value which specifies the casing which is applies to the text box's text.
        /// </summary>
        public CharacterCasing CharacterCasing
        {
            get { return GetValue<CharacterCasing>(CharacterCasingProperty); }
            set { SetValue(CharacterCasingProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets a value specifying how the text box's text is aligned.
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return GetValue<TextAlignment>(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value specifying how the text box's text wraps when it reaches the edge of its container.
        /// </summary>
        public TextWrapping TextWrapping
        {
            get { return GetValue<TextWrapping>(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        /// <summary>
        /// Gets the total number of characters in the text box's text.
        /// </summary>
        public Int32 TextLength
        {
            get
            {
                if (TextEditor != null)
                    return TextEditor.TextLength;

                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the maximum length of the text which is entered into the text box.
        /// </summary>
        public Int32 MaxLength
        {
            get { return GetValue<Int32>(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        /// <summary>
        /// Gets the total number of lines in the text box's text.
        /// </summary>
        public Int32 LineCount
        {
            get
            {
                if (TextEditor != null)
                    return TextEditor.LineCount;

                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the minimum number of visible lines.
        /// </summary>
        public Int32 MinLines
        {
            get { return GetValue<Int32>(MinLinesProperty); }
            set { SetValue(MinLinesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum number of visible lines.
        /// </summary>
        public Int32 MaxLines
        {
            get { return GetValue<Int32>(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current position of the insertion caret.
        /// </summary>
        public Int32 CaretIndex
        {
            get
            {
                if (TextEditor != null)
                    return TextEditor.CaretIndex;

                return 0;
            }
            set
            {
                if (TextEditor != null)
                    TextEditor.CaretIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the starting point of the selected text.
        /// </summary>
        public Int32 SelectionStart
        {
            get
            {
                if (TextEditor != null)
                    return TextEditor.SelectionStart;

                return 0;
            }
            set
            {
                if (TextEditor != null)
                    TextEditor.SelectionStart = value;
            }
        }

        /// <summary>
        /// Gets or sets the length of the selected text.
        /// </summary>
        public Int32 SelectionLength
        {
            get
            {
                if (TextEditor != null)
                    return TextEditor.SelectionLength;

                return 0;
            }
            set
            {
                if (TextEditor != null)
                    TextEditor.SelectionLength = value;
            }
        }
        
        /// <summary>
        /// Identifies the Text dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(VersionedStringSource), typeof(TextBox),
            new PropertyMetadata<VersionedStringSource>(VersionedStringSource.Invalid, PropertyMetadataOptions.None, HandleTextChanged));

        /// <summary>
        /// Identifies the <see cref="KeyboardMode"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'keyboard-mode'.</remarks>
        public static readonly DependencyProperty KeyboardModeProperty = DependencyProperty.Register("KeyboardMode", typeof(KeyboardMode), typeof(TextBox),
            new PropertyMetadata<KeyboardMode>(KeyboardMode.Text, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CharacterCasing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CharacterCasingProperty = DependencyProperty.Register("CharacterCasing", typeof(CharacterCasing), typeof(TextBox),
            new PropertyMetadata<CharacterCasing>(CharacterCasing.Normal, PropertyMetadataOptions.None));
        
        /// <summary>
        /// Identifies the <see cref="TextAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(TextBox),
            new PropertyMetadata<TextAlignment>(TextAlignment.Left, PropertyMetadataOptions.AffectsMeasure, HandleTextAlignmentChanged));

        /// <summary>
        /// Identifies the <see cref="TextWrapping"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(TextBox),
            new PropertyMetadata<TextWrapping>(TextWrapping.NoWrap, PropertyMetadataOptions.AffectsMeasure, HandleTextWrappingChanged));
        
        /// <summary>
        /// Identifies the <see cref="MinLines"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinLinesProperty = DependencyProperty.Register("MinLines", typeof(Int32), typeof(TextBox),
            new PropertyMetadata<Int32>(1, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="MaxLines"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof(Int32), typeof(TextBox),
            new PropertyMetadata<Int32>(Int32.MaxValue, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="MaxLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(Int32), typeof(TextBox),
            new PropertyMetadata<Int32>(CommonBoxedValues.Int32.Zero, PropertyMetadataOptions.None));

        /// <inheritdoc/>
        internal override void LineUpInternal()
        {
            var font = Font;
            if (!font.IsLoaded)
                return;

            var fontFace = font.Resource.Value.GetFace(SpriteFontStyle.Regular);
            var fontLineHeight = fontFace.LineSpacing;

            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - fontLineHeight);
        }

        /// <inheritdoc/>
        internal override void LineDownInternal()
        {
            var font = Font;
            if (!font.IsLoaded)
                return;

            var fontFace = font.Resource.Value.GetFace(SpriteFontStyle.Regular);
            var fontLineHeight = fontFace.LineSpacing;

            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + fontLineHeight);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            UpdateScrollViewerSize();
            return base.MeasureOverride(availableSize);
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                Focus();
                CaptureMouse();
            }

            if (TextEditor != null && IsMouseWithinEditor())
                TextEditor.HandleMouseDown(device, button, ref data);

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

            if (TextEditor != null && IsMouseWithinEditor())
                TextEditor.HandleMouseUp(device, button, ref data);

            data.Handled = true;
            base.OnMouseUp(device, button, ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseDoubleClick(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (IsMouseWithinEditor() && TextEditor != null)
                TextEditor.HandleMouseDoubleClick(device, button, ref data);

            base.OnMouseDoubleClick(device, button, ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, ref RoutedEventData data)
        {
            if (TextEditor != null)
                TextEditor.HandleMouseMove(device, ref data);

            data.Handled = true;
            base.OnMouseMove(device, x, y, dx, dy, ref data);
        }

        /// <inheritdoc/>
        protected override void OnLostMouseCapture(ref RoutedEventData data)
        {
            if (TextEditor != null)
                TextEditor.HandleLostMouseCapture();

            data.Handled = true;
            base.OnLostMouseCapture(ref data);
        }

        /// <inheritdoc/>
        protected override void OnGotKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, ref RoutedEventData data)
        {
            Ultraviolet.GetInput().ShowSoftwareKeyboard(KeyboardMode);

            if (TextEditor != null)
                TextEditor.HandleGotKeyboardFocus();

            base.OnGotKeyboardFocus(device, oldFocus, newFocus, ref data);
        }

        /// <inheritdoc/>
        protected override void OnLostKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, ref RoutedEventData data)
        {
            Ultraviolet.GetInput().HideSoftwareKeyboard();

            if (TextEditor != null)
                TextEditor.HandleLostKeyboardFocus();

            base.OnLostKeyboardFocus(device, oldFocus, newFocus, ref data);
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            if (TextEditor != null)
                TextEditor.HandleKeyDown(device, key, modifiers, ref data);
            
            base.OnKeyDown(device, key, modifiers, ref data);
        }

        /// <inheritdoc/>
        protected override void OnTextInput(KeyboardDevice device, ref RoutedEventData data)
        {
            if (TextEditor != null)
                TextEditor.HandleTextInput(device, ref data);

            base.OnTextInput(device, ref data);
        }

        /// <summary>
        /// Occurs when the control handles a <see cref="ScrollViewer.ScrollChangedEvent"/> routed event.
        /// </summary>
        private static void HandleScrollChanged(DependencyObject dobj, ref ScrollChangedInfo scrollInfo, ref RoutedEventData data)
        {
            if (!MathUtil.IsApproximatelyZero(scrollInfo.ViewportHeightChange))
            {
                ((TextBox)dobj).UpdateScrollViewerSize();
            }
            data.Handled = true;
        }

        /// <summary>
        /// Occurs when the value of the Text dependency property changes.
        /// </summary>
        private static void HandleTextChanged(DependencyObject dobj, VersionedStringSource oldValue, VersionedStringSource newValue)
        {
            var raiseTextChanged = false;

            var textBox = (TextBox)dobj;
            if (textBox.TextEditor != null)
                raiseTextChanged = !textBox.TextEditor.HandleTextChanged(newValue);

            if (raiseTextChanged)
                textBox.OnTextChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TextAlignment"/> dependency property changes.
        /// </summary>
        private static void HandleTextAlignmentChanged(DependencyObject dobj, TextAlignment oldValue, TextAlignment newValue)
        {
            var textBox = (TextBox)dobj;
            if (textBox.TextEditor != null)
            {
                switch (newValue)
                {
                    case TextAlignment.Right:
                        textBox.TextEditor.HorizontalAlignment = HorizontalAlignment.Right;
                        break;

                    case TextAlignment.Center:
                        textBox.TextEditor.HorizontalAlignment = HorizontalAlignment.Center;
                        break;

                    default:
                        textBox.TextEditor.HorizontalAlignment = HorizontalAlignment.Left;
                        break;
                }
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TextWrapping"/> dependency property changes.
        /// </summary>
        private static void HandleTextWrappingChanged(DependencyObject dobj, TextWrapping oldValue, TextWrapping newValue)
        {
            var textBox = (TextBox)dobj;
            textBox.CoerceValue(HorizontalScrollBarVisibilityProperty);

            if (textBox.TextEditor != null)
                textBox.TextEditor.InvalidateMeasure();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Primitives.TextBoxBase.IsReadOnly"/> dependency property changes.
        /// </summary>
        private static void HandleIsReadOnlyChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var textBox = (TextBox)dobj;            
            if (textBox.TextEditor != null)
                textBox.TextEditor.HandleIsReadOnlyChanged();
        }

        /// <summary>
        /// Coerces the value of the <see cref="Primitives.TextBoxBase.HorizontalScrollBarVisibility"/> property to force the scroll bar
        /// to a disabled state when wrapping is enabled.
        /// </summary>
        private static ScrollBarVisibility CoerceHorizontalScrollBarVisibility(DependencyObject dobj, ScrollBarVisibility value)
        {
            var textBox = (TextBox)dobj;
            if (textBox.TextWrapping == TextWrapping.Wrap)
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
            var mouseTarget = (Control)TextEditorScrollViewer ?? this;
            var mouseBounds = mouseTarget.Bounds;

            return mouseBounds.Contains(Mouse.GetPosition(mouseTarget));
        }

        /// <summary>
        /// Gets a value indicating whether the text box's height is constrained by a <see cref="FrameworkElement.Height"/>,
        /// <see cref="FrameworkElement.MinHeight"/>, or <see cref="FrameworkElement.MaxHeight"/> value.
        /// </summary>
        /// <returns><c>true</c> if the text box's height is constrained; otherwise, <c>false</c>.</returns>
        private Boolean IsHeightConstrained()
        {
            return !Double.IsNaN(Height) || !MathUtil.IsApproximatelyZero(MinHeight) || !Double.IsPositiveInfinity(MaxHeight);
        }

        /// <summary>
        /// Updates the size constraints of the text box's scroll viewer.
        /// </summary>
        private void UpdateScrollViewerSize()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer == null)
                return;

            if (IsHeightConstrained())
            {
                scrollViewer.ClearLocalValue(MinHeightProperty);
                scrollViewer.ClearLocalValue(MaxHeightProperty);
            }
            else
            {
                var heightOfComponents = scrollViewer.ActualHeight - scrollViewer.ViewportHeight;
                var heightOfLine = !Font.IsLoaded ? 0 : Font.Resource.Value.GetFace(FontStyle).LineSpacing;

                if (MinLines > 1)
                {
                    scrollViewer.MinHeight = heightOfComponents + (heightOfLine * MinLines);
                }
                else
                {
                    scrollViewer.ClearLocalValue(MinHeightProperty);
                }

                if (MaxLines < Int32.MaxValue)
                {
                    scrollViewer.MaxHeight = heightOfComponents + (heightOfLine * MaxLines);
                }
                else
                {
                    scrollViewer.ClearLocalValue(MaxHeightProperty);
                }
            }
        }
    }
}
