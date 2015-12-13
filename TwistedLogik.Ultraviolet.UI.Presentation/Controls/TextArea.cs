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
            EventManager.RegisterClassHandler(typeof(TextArea), ScrollViewer.ScrollChangedEvent, new UpfScrollChangedEventHandler(HandleScrollChanged));
            EventManager.RegisterClassHandler(typeof(TextArea), SelectionChangedEvent, new UpfRoutedEventHandler(HandleSelectionChanged));
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
        /// Clears the text area's content.
        /// </summary>
        public void Clear()
        {
            if (PART_Editor != null)
                PART_Editor.Clear();
        }

        /// <summary>
        /// Scrolls the text area so that the specified line is in full view.
        /// </summary>
        /// <param name="lineIndex">The index of the line to scroll into view.</param>
        public void ScrollToLine(Int32 lineIndex)
        {
            if (PART_Editor != null)
                PART_Editor.ScrollToLine(lineIndex);
        }

        /// <summary>
        /// Gets the index of the first character on the specified line.
        /// </summary>
        /// <param name="lineIndex">The index of the line for which to retrieve a character index.</param>
        /// <returns>The index of the first character on the specified line.</returns>
        public Int32 GetCharacterIndexFromLineIndex(Int32 lineIndex)
        {
            if (PART_Editor != null)
                return PART_Editor.GetCharacterIndexFromLineIndex(lineIndex);

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
            if (PART_Editor != null)
                return PART_Editor.GetCharacterIndexFromPoint(point, snapToText);

            return -1;
        }

        /// <summary>
        /// Gets the index of the first visible line of text.
        /// </summary>
        /// <returns>The index of the first visible line of text.</returns>
        public Int32 GetFirstVisibleLineIndex()
        {
            if (PART_Editor != null)
                return PART_Editor.GetFirstVisibleLineIndex();

            return 0;
        }

        /// <summary>
        /// Gets the index of the last visible line of text.
        /// </summary>
        /// <returns>The index of the last visible line of text.</returns>
        public Int32 GetLastVisibleLineIndex()
        {
            if (PART_Editor != null)
                return PART_Editor.GetLastVisibleLineIndex();

            return 0;
        }

        /// <summary>
        /// Gets the index of the line of text that contains the specified character.
        /// </summary>
        /// <param name="charIndex">The index of the character to evaluate.</param>
        /// <returns>The index of the line of text that contains the specified character.</returns>
        public Int32 GetLineIndexFromCharacterIndex(Int32 charIndex)
        {
            if (PART_Editor != null)
                return PART_Editor.GetLineIndexFromCharacterIndex(charIndex);

            return 0;
        }

        /// <summary>
        /// Gets the number of characters on the specified line of text.
        /// </summary>
        /// <param name="lineIndex">The index of the line to evaluate.</param>
        /// <returns>The number of characters on the specified line of text.</returns>
        public Int32 GetLineLength(Int32 lineIndex)
        {
            if (PART_Editor != null)
                return PART_Editor.GetLineLength(lineIndex);

            return 0;
        }

        /// <summary>
        /// Gets the specified line of text.
        /// </summary>
        /// <param name="lineIndex">The index of the line of text to retrieve.</param>
        /// <returns>A string containing the contents of the specified line of text.</returns>
        public String GetLineText(Int32 lineIndex)
        {
            if (PART_Editor != null)
                return PART_Editor.GetLineText(lineIndex);

            return null;
        }

        /// <summary>
        /// Gets a rectangle that represents the leading edge of the specified character.
        /// </summary>
        /// <param name="charIndex">The index of the character for which to retrieve the rectangle.</param>
        /// <returns>A rectangle which represents the bounds of the leading edge of the specified character,
        /// or <see cref="RectangleD.Empty"/> if the bounding rectangle cannot be determined.</returns>
        public RectangleD GetRectFromCharacterIndex(Int32 charIndex)
        {
            if (PART_Editor != null)
                return PART_Editor.GetRectFromCharacterIndex(charIndex);

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
            if (PART_Editor != null)
                return PART_Editor.GetRectFromCharacterIndex(charIndex, trailingEdge);

            return RectangleD.Empty;
        }

        /// <summary>
        /// Gets or sets a <see cref="CharacterCasing"/> value which specifies the casing which is applies to the text area's text.
        /// </summary>
        public CharacterCasing CharacterCasing
        {
            get { return GetValue<CharacterCasing>(CharacterCasingProperty); }
            set { SetValue(CharacterCasingProperty, value); }
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
        /// Gets or sets a value specifying how the text area's text is aligned.
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return GetValue<TextAlignment>(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
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
        /// Gets or sets a value indicating whether this is a read-only text area. A read-only text area cannot be changed by the
        /// user, but may still be changed programmatically.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return GetValue<Boolean>(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the caret is visible when this text box is read-only.
        /// </summary>
        public Boolean IsReadOnlyCaretVisible
        {
            get { return GetValue<Boolean>(IsReadOnlyCaretVisibleProperty); }
            set { SetValue(IsReadOnlyCaretVisibleProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the text area has focus and selected text.
        /// </summary>
        public Boolean IsSelectionActive
        {
            get { return GetValue<Boolean>(IsSelectionActiveProperty); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the selection highlight is displayed when the text area does not have focus.
        /// </summary>
        public Boolean IsInactiveSelectionHighlightEnabled
        {
            get { return GetValue<Boolean>(IsInactiveSelectionHighlightEnabledProperty); }
            set { SetValue(IsInactiveSelectionHighlightEnabledProperty, value); }
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
        /// Gets the total number of characters in the text area's text.
        /// </summary>
        public Int32 TextLength
        {
            get
            {
                if (PART_Editor != null)
                    return PART_Editor.TextLength;

                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the maximum length of the text which is entered into the text area.
        /// </summary>
        public Int32 MaxLength
        {
            get { return GetValue<Int32>(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        /// <summary>
        /// Gets the total number of lines in the text area's text.
        /// </summary>
        public Int32 LineCount
        {
            get
            {
                if (PART_Editor != null)
                    return PART_Editor.LineCount;

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
                if (PART_Editor != null)
                    return PART_Editor.CaretIndex;

                return 0;
            }
            set
            {
                if (PART_Editor != null)
                    PART_Editor.CaretIndex = value;
            }
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
        /// Gets the horizontal offset of the text area's scroll viewer.
        /// </summary>
        public Double HorizontalOffset
        {
            get
            {
                if (PART_ScrollViewer != null)
                    return PART_ScrollViewer.HorizontalOffset;

                return 0;
            }
        }

        /// <summary>
        /// Gets the vertical offset of the text area's scroll viewer.
        /// </summary>
        public Double VerticalOffset
        {
            get
            {
                if (PART_ScrollViewer != null)
                    return PART_ScrollViewer.VerticalOffset;

                return 0;
            }
        }

        /// <summary>
        /// Gets the width of the content which is being displayed by the text area's scroll viewer.
        /// </summary>
        public Double ExtentWidth
        {
            get
            {
                if (PART_ScrollViewer != null)
                    return PART_ScrollViewer.ExtentWidth;

                return 0;
            }
        }

        /// <summary>
        /// Gets the height of the content which is being displayed by the text area's scroll viewer.
        /// </summary>
        public Double ExtentHeight
        {
            get
            {
                if (PART_ScrollViewer != null)
                    return PART_ScrollViewer.ExtentHeight;

                return 0;
            }
        }

        /// <summary>
        /// Gets the width of the text area's scrollable viewport.
        /// </summary>
        public Double ViewportWidth
        {
            get
            {
                if (PART_ScrollViewer != null)
                    return PART_ScrollViewer.ViewportWidth;

                return 0;
            }
        }

        /// <summary>
        /// Gets the height of the text area's scrollable viewport.
        /// </summary>
        public Double ViewportHeight
        {
            get
            {
                if (PART_ScrollViewer != null)
                    return PART_ScrollViewer.ViewportHeight;

                return 0;
            }
        }

        /// <summary>
        /// Occurs when the selected text is changed.
        /// </summary>
        public event UpfRoutedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }

        /// <summary>
        /// Identifies the Text dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(VersionedStringSource), typeof(TextArea),
            new PropertyMetadata<VersionedStringSource>(VersionedStringSource.Invalid, PropertyMetadataOptions.None, HandleTextChanged));

        /// <summary>
        /// Identifies the <see cref="CharacterCasing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CharacterCasingProperty = DependencyProperty.Register("CharacterCasing", typeof(CharacterCasing), typeof(TextArea),
            new PropertyMetadata<CharacterCasing>(CharacterCasing.Normal, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="InsertionMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InsertionModeProperty = DependencyProperty.Register("InsertionMode", typeof(TextBoxInsertionMode), typeof(TextArea),
            new PropertyMetadata<TextBoxInsertionMode>(TextBoxInsertionMode.Insert, PropertyMetadataOptions.None, HandleInsertionModeChanged));

        /// <summary>
        /// Identifies the <see cref="TextAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(TextArea),
            new PropertyMetadata<TextAlignment>(TextAlignment.Left, PropertyMetadataOptions.AffectsMeasure, HandleTextAlignmentChanged));

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
        /// Identifies the <see cref="IsReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(Boolean), typeof(TextArea),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsReadOnlyChanged));

        /// <summary>
        /// Identifies the <see cref="IsReadOnlyCaretVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyCaretVisibleProperty = DependencyProperty.Register("IsReadOnlyCaretVisibleProperty", typeof(Boolean), typeof(TextArea),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// The private access key for the <see cref="IsSelectionActive"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsSelectionActivePropertyKey = DependencyProperty.RegisterReadOnly("IsSelectionActive", typeof(Boolean), typeof(TextArea),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="IsSelectionActive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectionActiveProperty = IsSelectionActivePropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="IsInactiveSelectionHighlightEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty = DependencyProperty.Register("IsInactiveSelectionHighlightEnabled", typeof(Boolean), typeof(TextArea),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="AcceptsReturn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AcceptsReturnProperty = KeyboardNavigation.AcceptsReturnProperty.AddOwner(typeof(TextArea));

        /// <summary>
        /// Identifies the <see cref="AcceptsTab"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AcceptsTabProperty = DependencyProperty.Register("AcceptsTab", typeof(Boolean), typeof(TextArea),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));

        /// <summary>
        /// Identifies the <see cref="MinLines"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinLinesProperty = DependencyProperty.Register("MinLines", typeof(Int32), typeof(TextArea),
            new PropertyMetadata<Int32>(1, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="MaxLines"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof(Int32), typeof(TextArea),
            new PropertyMetadata<Int32>(Int32.MaxValue, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="MaxLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(Int32), typeof(TextArea),
            new PropertyMetadata<Int32>(CommonBoxedValues.Int32.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="SelectionChanged"/> routed event.
        /// </summary>
        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", 
            RoutingStrategy.Bubble, typeof(UpfRoutedEventHandler), typeof(TextArea));

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

            UpdateIsSelectionActive();

            base.OnGotKeyboardFocus(device, oldFocus, newFocus, ref data);
        }

        /// <inheritdoc/>
        protected override void OnLostKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, ref RoutedEventData data)
        {
            if (PART_Editor != null)
                PART_Editor.HandleLostKeyboardFocus();

            UpdateIsSelectionActive();

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
                        if (!IsReadOnly)
                        {
                            InsertionMode = (InsertionMode == TextBoxInsertionMode.Insert) ?
                                TextBoxInsertionMode.Overwrite :
                                TextBoxInsertionMode.Insert;
                        }
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
        /// Occurs when the control handles a <see cref="ScrollViewer.ScrollChangedEvent"/> routed event.
        /// </summary>
        private static void HandleScrollChanged(DependencyObject dobj, ref ScrollChangedInfo scrollInfo, ref RoutedEventData data)
        {
            if (!MathUtil.IsApproximatelyZero(scrollInfo.ViewportHeightChange))
            {
                ((TextArea)dobj).UpdateScrollViewerSize();
            }
            data.Handled = true;
        }

        /// <summary>
        /// Occurs when the control handles a <see cref="SelectionChangedEvent"/> routed event.
        /// </summary>
        private static void HandleSelectionChanged(DependencyObject dobj, ref RoutedEventData data)
        {
            var textArea = (TextArea)dobj;
            textArea.UpdateIsSelectionActive();

            if (textArea.PART_Editor != null && data.OriginalSource == textArea.PART_Editor)
            {
                var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(SelectionChangedEvent);
                var evtData = new RoutedEventData(textArea);
                evtDelegate(textArea, ref evtData);

                data.Handled = true;
            }
        }

        /// <summary>
        /// Occurs when the value of the Text dependency property changes.
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
        /// Occurs when the value of the <see cref="TextAlignment"/> dependency property changes.
        /// </summary>
        private static void HandleTextAlignmentChanged(DependencyObject dobj, TextAlignment oldValue, TextAlignment newValue)
        {
            var textArea = (TextArea)dobj;
            if (textArea.PART_Editor != null)
            {
                switch (newValue)
                {
                    case TextAlignment.Right:
                        textArea.PART_Editor.HorizontalAlignment = HorizontalAlignment.Right;
                        break;

                    case TextAlignment.Center:
                        textArea.PART_Editor.HorizontalAlignment = HorizontalAlignment.Center;
                        break;

                    default:
                        textArea.PART_Editor.HorizontalAlignment = HorizontalAlignment.Left;
                        break;
                }
            }
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
        /// Occurs when the value of the <see cref="IsReadOnly"/> dependency property changes.
        /// </summary>
        private static void HandleIsReadOnlyChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var textArea = (TextArea)dobj;            
            if (textArea.PART_Editor != null)
                textArea.PART_Editor.HandleIsReadOnlyChanged();
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

        /// <summary>
        /// Gets a value indicating whether the text area's height is constrained by a <see cref="FrameworkElement.Height"/>,
        /// <see cref="FrameworkElement.MinHeight"/>, or <see cref="FrameworkElement.MaxHeight"/> value.
        /// </summary>
        /// <returns><c>true</c> if the text area's height is constrained; otherwise, <c>false</c>.</returns>
        private Boolean IsHeightConstrained()
        {
            return !Double.IsNaN(Height) || !MathUtil.IsApproximatelyZero(MinHeight) || !Double.IsPositiveInfinity(MaxHeight);
        }

        /// <summary>
        /// Updates the size constraints of the text area's scroll viewer.
        /// </summary>
        private void UpdateScrollViewerSize()
        {
            if (PART_ScrollViewer == null)
                return;

            if (IsHeightConstrained())
            {
                PART_ScrollViewer.ClearLocalValue(MinHeightProperty);
                PART_ScrollViewer.ClearLocalValue(MaxHeightProperty);
            }
            else
            {
                var heightOfComponents = PART_ScrollViewer.ActualHeight - PART_ScrollViewer.ViewportHeight;
                var heightOfLine = !Font.IsLoaded ? 0 : Font.Resource.Value.GetFace(FontStyle).LineSpacing;

                if (MinLines > 1)
                {
                    PART_ScrollViewer.MinHeight = heightOfComponents + (heightOfLine * MinLines);
                }
                else
                {
                    PART_ScrollViewer.ClearLocalValue(MinHeightProperty);
                }

                if (MaxLines < Int32.MaxValue)
                {
                    PART_ScrollViewer.MaxHeight = heightOfComponents + (heightOfLine * MaxLines);
                }
                else
                {
                    PART_ScrollViewer.ClearLocalValue(MaxHeightProperty);
                }
            }
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
                SetValue(IsSelectionActivePropertyKey, isSelectionActive);
            }
        }

        // Component references.
        private readonly ScrollViewer PART_ScrollViewer = null;
        private readonly TextAreaEditor PART_Editor = null;        
    }
}
