using System;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents an element which allows the user to edit a string of text.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.TextBox.xml")]
    [UvmlDefaultProperty("Text")]
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
            Contract.Require(stringBuilder, nameof(stringBuilder));

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
        /// Gets or sets the text box's text source.
        /// </summary>
        /// <value>A <see cref="VersionedStringSource"/> which represents the source of the text box's text.
        /// The default value is <see cref="VersionedStringSource.Invalid"/>.</value>
        /// <remarks>
        /// <para>In most cases, rather than using this property to access the element's text, you should use the <see cref="GetText(StringBuilder)"/>
        /// and <see cref="SetText(StringBuilder)"/> methods in order to avoid allocating onto the managed heap.</para>
        /// <dprop>
        ///     <dpropField><see cref="TextProperty"/></dpropField>
        ///     <dpropStylingName>text</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public VersionedStringSource Text
        {
            get { return GetValue<VersionedStringSource>(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text box's current keyboard mode.
        /// </summary>
        /// <value>A <see cref="Ultraviolet.Input.KeyboardMode"/> value which determines the kind of software keyboard
        /// which is displayed by this text box on platforms which use software keyboard. The default
        /// value is <see cref="Ultraviolet.Input.KeyboardMode.Text"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="KeyboardModeProperty"/></dpropField>
        ///     <dpropStylingName>keyboard-mode</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public KeyboardMode KeyboardMode
        {
            get { return GetValue<KeyboardMode>(KeyboardModeProperty); }
            set { SetValue(KeyboardModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a <see cref="Controls.CharacterCasing"/> value which specifies the casing which is 
        /// applied to the text box's text.
        /// </summary>
        /// <value>A <see cref="Controls.CharacterCasing"/> value which specifies the casing which is 
        /// applied to the text box's text. The default value is <see cref="Controls.CharacterCasing.Normal"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="CharacterCasingProperty"/></dpropField>
        ///     <dpropStylingName>character-casing</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public CharacterCasing CharacterCasing
        {
            get { return GetValue<CharacterCasing>(CharacterCasingProperty); }
            set { SetValue(CharacterCasingProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets a value specifying how the text box's text is aligned.
        /// </summary>
        /// <value>A <see cref="Presentation.TextAlignment"/> value which specifies how the text box's text is aligned.
        /// The default value is <see cref="Presentation.TextAlignment.Left"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="TextAlignmentProperty"/></dpropField>
        ///     <dpropStylingName>text-alignment</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public TextAlignment TextAlignment
        {
            get { return GetValue<TextAlignment>(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value specifying how the text box's text wraps when it reaches the edge of its container.
        /// </summary>
        /// <value>A <see cref="Presentation.TextWrapping"/> value which specifies how the text box's text is wrapped
        /// when it reaches the edge of its container. The default value is <see cref="Presentation.TextWrapping.NoWrap"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="TextWrappingProperty"/></dpropField>
        ///     <dpropStylingName>text-wrapping</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public TextWrapping TextWrapping
        {
            get { return GetValue<TextWrapping>(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        /// <summary>
        /// Gets the total number of characters in the text box's text.
        /// </summary>
        /// <value>An <see cref="Int32"/> value which represents the total number of characters
        /// in the text box's text.</value>
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
        /// <value>An <see cref="Int32"/> value which represents the maximum length of the
        /// text box's text, or 0 if the text has no maximum length. The default value is 0.</value>
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
        /// Gets or sets the minimum number of visible lines.
        /// </summary>
        /// <value>An <see cref="Int32"/> value which represents the minimum number of lines
        /// which will be displayed by the text box. The default value is 1.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="MinLinesProperty"/></dpropField>
        ///     <dpropStylingName>min-lines</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Int32 MinLines
        {
            get { return GetValue<Int32>(MinLinesProperty); }
            set { SetValue(MinLinesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum number of visible lines.
        /// </summary>
        /// <value>An <see cref="Int32"/> which represents the maximum number of lines
        /// which will be displayed by the text box. The default value is <see cref="Int32.MaxValue"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="MaxLinesProperty"/></dpropField>
        ///     <dpropStylingName>max-lines</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Int32 MaxLines
        {
            get { return GetValue<Int32>(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }
        
        /// <summary>
        /// Gets the total number of lines in the text box's text.
        /// </summary>
        /// <value>An <see cref="Int32"/> value which represents the total number of
        /// line in the text box's text.</value>
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
        /// Gets or sets the current position of the insertion caret.
        /// </summary>
        /// <value>A <see cref="Int32"/> value which represents the position of the insertion
        /// caret within the text box's text.</value>
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
        /// <value>A <see cref="Int32"/> value which represents the offset of the text selection
        /// within the text box's text.</value>
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
        /// <value>A <see cref="Int32"/> value which represents the length of the text selection.</value>
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
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Text"/> dependency property.</value>
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
        /// <value>The identifier for the <see cref="CharacterCasing"/> dependency property.</value>
        public static readonly DependencyProperty CharacterCasingProperty = DependencyProperty.Register("CharacterCasing", typeof(CharacterCasing), typeof(TextBox),
            new PropertyMetadata<CharacterCasing>(CharacterCasing.Normal, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="TextAlignment"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="TextAlignment"/> dependency property.</value>
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(TextBox),
            new PropertyMetadata<TextAlignment>(TextAlignment.Left, PropertyMetadataOptions.AffectsMeasure, HandleTextAlignmentChanged));

        /// <summary>
        /// Identifies the <see cref="TextWrapping"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="TextWrapping"/> dependency property.</value>
        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(TextBox),
            new PropertyMetadata<TextWrapping>(TextWrapping.NoWrap, PropertyMetadataOptions.AffectsMeasure, HandleTextWrappingChanged));

        /// <summary>
        /// Identifies the <see cref="MaxLength"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="MaxLength"/> dependency property.</value>
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(Int32), typeof(TextBox),
            new PropertyMetadata<Int32>(CommonBoxedValues.Int32.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="MinLines"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="MinLines"/> dependency property.</value>
        public static readonly DependencyProperty MinLinesProperty = DependencyProperty.Register("MinLines", typeof(Int32), typeof(TextBox),
            new PropertyMetadata<Int32>(1, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="MaxLines"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="MaxLines"/> dependency property.</value>
        public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof(Int32), typeof(TextBox),
            new PropertyMetadata<Int32>(Int32.MaxValue, PropertyMetadataOptions.AffectsMeasure));

        /// <inheritdoc/>
        internal override void LineUpInternal()
        {
            var font = Font;
            if (!font.IsLoaded)
                return;

            var fontFace = font.Resource.Value.GetFace(UltravioletFontStyle.Regular);
            var fontLineHeight = fontFace.LineSpacing;

            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
            {
                UpdateLayout();
                scrollViewer.ScrollToVerticalOffset(scrollViewer.ContentVerticalOffset - fontLineHeight);
            }
        }

        /// <inheritdoc/>
        internal override void LineDownInternal()
        {
            var font = Font;
            if (!font.IsLoaded)
                return;

            var fontFace = font.Resource.Value.GetFace(UltravioletFontStyle.Regular);
            var fontLineHeight = fontFace.LineSpacing;

            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
            {
                UpdateLayout();
                scrollViewer.ScrollToVerticalOffset(scrollViewer.ContentVerticalOffset + fontLineHeight);
            }
        }
        
        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            UpdateScrollViewerSize();
            return base.MeasureOverride(availableSize);
        }

        /// <inheritdoc/>
        protected override void PositionChildrenOverride()
        {
            base.PositionChildrenOverride();

            if (IsKeyboardFocused)
                UpdateTextInputRegion();
        }

        /// <inheritdoc/>
        protected override void OnQueryCursor(MouseDevice device, CursorQueryRoutedEventData data)
        {
            if (IsMouseCaptured && IsMouseWithinEditor())
            {
                data.Cursor = TextEditor?.Cursor.Resource?.Cursor ?? data.Cursor;
                data.Handled = true;
            }
            base.OnQueryCursor(device, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseDown(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left)
                Focus();
            
            if (TextEditor != null && IsMouseWithinEditor())
            {
                CaptureMouse();
                TextEditor.HandleMouseDown(device, button, data);
                data.Handled = true;
            }

            base.OnPreviewMouseDown(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseUp(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left)
                ReleaseMouseCapture();

            if (TextEditor != null && IsMouseWithinEditor())
            {
                TextEditor.HandleMouseUp(device, button, data);
                data.Handled = true;
            }

            base.OnPreviewMouseUp(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseDoubleClick(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (TextEditor != null && IsMouseWithinEditor())
            {
                TextEditor.HandleMouseDoubleClick(device, button, data);
                data.Handled = true;
            }

            base.OnPreviewMouseDoubleClick(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, RoutedEventData data)
        {
            if (TextEditor != null)
            {
                var capture = Mouse.GetCaptured(View);
                if (capture == null || capture == this)
                {
                    TextEditor.HandleMouseMove(device, data);
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

            if (TextEditor != null && IsTouchWithinEditor(id))
            {
                CaptureTouch(id);
                TextEditor.HandleTouchDown(device, id, x, y, pressure, data);
                data.Handled = true;
            }

            UpdateTextInputRegion();
            Ultraviolet.GetInput().ShowSoftwareKeyboard();

            base.OnPreviewTouchDown(device, id, x, y, pressure, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewTouchUp(TouchDevice device, Int64 id, RoutedEventData data)
        {
            if (TextEditor != null && IsTouchWithinEditor(id))
            {
                TextEditor.HandleTouchUp(device, id, data);
                data.Handled = true;
            }

            base.OnPreviewTouchUp(device, id, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewTouchLongPress(TouchDevice device, Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            if (TextEditor != null && IsTouchWithinEditor(id))
            {
                TextEditor.HandleTouchLongPress(device, id, x, y, pressure, data);
                data.Handled = true;
            }

            base.OnPreviewTouchLongPress(device, id, x, y, pressure, data);
        }

        /// <inheritdoc/>
        protected override void OnPreviewTouchMove(TouchDevice device, Int64 id, Double x, Double y, Double dx, Double dy, Single pressure, RoutedEventData data)
        {
            if (TextEditor != null)
            {
                var capture = Touch.GetCaptured(View, id);
                if (capture == this || capture == null)
                {
                    TextEditor.HandleTouchMove(device, id, x, y, dx, dy, pressure, data);
                    data.Handled = true;
                }
            }

            base.OnPreviewTouchMove(device, id, x, y, dx, dy, pressure, data);
        }
        
        /// <inheritdoc/>
        protected override void OnLostMouseCapture(RoutedEventData data)
        {
            if (TextEditor != null)
                TextEditor.HandleLostMouseCapture();

            data.Handled = true;
            base.OnLostMouseCapture(data);
        }

        /// <inheritdoc/>
        protected override void OnGotKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            UpdateTextInputRegion();
            Ultraviolet.GetInput().ShowSoftwareKeyboard(KeyboardMode);

            if (TextEditor != null)
                TextEditor.HandleGotKeyboardFocus();

            base.OnGotKeyboardFocus(device, oldFocus, newFocus, data);
        }

        /// <inheritdoc/>
        protected override void OnLostKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            UpdateTextInputRegion(clear: true);
            Ultraviolet.GetInput().HideSoftwareKeyboard();

            if (TextEditor != null)
                TextEditor.HandleLostKeyboardFocus();

            base.OnLostKeyboardFocus(device, oldFocus, newFocus, data);
        }
        
        /// <inheritdoc/>
        protected override void OnTextInput(KeyboardDevice device, RoutedEventData data)
        {
            if (TextEditor != null)
                TextEditor.HandleTextInput(device, data);

            base.OnTextInput(device, data);
        }

        /// <inheritdoc/>
        protected override void OnTextEditing(KeyboardDevice device, RoutedEventData data)
        {
            if (TextEditor != null)
                TextEditor.HandleTextEditing(device, data);

            base.OnTextEditing(device, data);
        }

        /// <summary>
        /// Occurs when the control handles a <see cref="ScrollViewer.ScrollChangedEvent"/> routed event.
        /// </summary>
        private static void HandleScrollChanged(DependencyObject dobj, ScrollChangedRoutedEventData data)
        {
            if (!MathUtil.IsApproximatelyZero(data.ViewportHeightChange))
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
            var mouseTarget = (UIElement)TextEditor ?? this;
            if (Mouse.GetCaptured(View) != null && !mouseTarget.IsMouseCaptureWithin)
                return false;

            var mouseBounds = mouseTarget.Bounds;
            return mouseBounds.Contains(Mouse.GetPosition(mouseTarget));
        }
        
        /// <summary>
        /// Gets a value indicating whether the specified touch is currently inside of the editor.
        /// </summary>
        private Boolean IsTouchWithinEditor(Int64 id)
        {
            var touchTarget = (UIElement)TextEditor ?? this;
            var touchBounds = touchTarget.Bounds;

            return touchBounds.Contains(Touch.GetPosition(id, touchTarget));
        }

        /// <summary>
        /// Gets a value indicating whether the text box's height is constrained by a <see cref="FrameworkElement.Height"/>,
        /// <see cref="FrameworkElement.MinHeight"/>, or <see cref="FrameworkElement.MaxHeight"/> value.
        /// </summary>
        /// <returns><see langword="true"/> if the text box's height is constrained; otherwise, <see langword="false"/>.</returns>
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
    }
}
