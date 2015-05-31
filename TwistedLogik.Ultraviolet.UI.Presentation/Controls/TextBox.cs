using System;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a element used for entering a single line of text.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.TextBox.xml")]
    [DefaultProperty("Text")]
    public partial class TextBox : Control
    {
        /// <summary>
        /// Initializes the <see cref="TextBox"/> class.
        /// </summary>
        static TextBox()
        {
            FontProperty.AddOwner(typeof(TextBox), new PropertyMetadata<SourcedResource<SpriteFont>>(HandleFontChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TextBox(UltravioletContext uv, String name)
            : base(uv, name)
        {
            SetDefaultValue<Boolean>(FocusableProperty, true);
        }

        /// <summary>
        /// Moves the caret to the beginning of the text.
        /// </summary>
        public void MoveHome()
        {
            textCaretPosition = 0;
            textScrollOffset  = 0;
            caretBlinkTimer   = 0;
        }

        /// <summary>
        /// Moves the caret to the end of the text.
        /// </summary>
        public void MoveEnd()
        {
            if (Text != null)
            {
                textCaretPosition = Text.Length;
                ScrollForwardToCaret();
            }
            caretBlinkTimer = 0;
        }

        /// <summary>
        /// Gets or sets the text displayed by the text box.
        /// </summary>
        public String Text
        {
            get { return GetValue<String>(TextProperty); }
            set { SetValue<String>(TextProperty, value); }
        }

        /// <summary>
        /// Gets the currently selected text.
        /// </summary>
        public String SelectedText
        {
            get
            {
                if (!IsTextSelected || Text == null)
                    return null;

                return Text.Substring(SelectionStart, SelectionEnd - SelectionStart);
            }
        }

        /// <summary>
        /// Gets or sets a regular expression which is used to limit the values
        /// which can be entered into the text box.
        /// </summary>
        public String Pattern
        {
            get { return GetValue<String>(PatternProperty); }
            set { SetValue<String>(PatternProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets the maximum length of the text box's text.
        /// </summary>
        public Int32 MaxLength
        {
            get { return GetValue<Int32>(MaxLengthProperty); }
            set { SetValue<Int32>(MaxLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the thickness of the caret while the text box's <see cref="InsertionMode"/> property
        /// is set to <see cref="TextBoxInsertionMode.Overwrite"/>, specified in device independent pixels.
        /// </summary>
        public Double CaretThickness
        {
            get { return GetValue<Double>(CaretThicknessProperty); }
            set { SetValue<Double>(CaretThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image used to draw the caret.
        /// </summary>
        public SourcedImage CaretImage
        {
            get { return GetValue<SourcedImage>(CaretImageProperty); }
            set { SetValue<SourcedImage>(CaretImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the caret is rendered.
        /// </summary>
        public Color CaretColor
        {
            get { return GetValue<Color>(CaretColorProperty); }
            set { SetValue<Color>(CaretColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image used to draw the selection highlight.
        /// </summary>
        public SourcedImage SelectionImage
        {
            get { return GetValue<SourcedImage>(SelectionImageProperty); }
            set { SetValue<SourcedImage>(SelectionImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color used to highlight the text box's selected text.
        /// </summary>
        public Color SelectionColor
        {
            get { return GetValue<Color>(SelectionColorProperty); }
            set { SetValue<Color>(SelectionColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text box's current insertion mode.
        /// </summary>
        public TextBoxInsertionMode InsertionMode
        {
            get { return GetValue<TextBoxInsertionMode>(InsertionModeProperty); }
            set { SetValue<TextBoxInsertionMode>(InsertionModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether text editing is disabled for this control.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return GetValue<Boolean>(IsReadOnlyProperty); }
            set { SetValue<Boolean>(IsReadOnlyProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'text'.</remarks>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(TextBox),
            new PropertyMetadata<String>(HandleTextChanged));

        /// <summary>
        /// Identifies the <see cref="Pattern"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'pattern'.</remarks>
        public static readonly DependencyProperty PatternProperty = DependencyProperty.Register("Pattern", typeof(String), typeof(TextBox),
            new PropertyMetadata<String>(HandlePatternChanged));

        /// <summary>
        /// Identifies the <see cref="MaxLength"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'max-length'.</remarks>
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(Int32), typeof(TextBox),
            new PropertyMetadata<Int32>());

        /// <summary>
        /// Identifies the <see cref="CaretThickness"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'caret-thickness'.</remarks>
        public static readonly DependencyProperty CaretThicknessProperty = DependencyProperty.Register("CaretThickness", typeof(Double), typeof(TextBox),
            new PropertyMetadata<Double>(4.0));

        /// <summary>
        /// Identifies the <see cref="CaretImage"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'caret-image'.</remarks>
        public static readonly DependencyProperty CaretImageProperty = DependencyProperty.Register("CaretImage", typeof(SourcedImage), typeof(TextBox),
            new PropertyMetadata<SourcedImage>(HandleCaretImageChanged));

        /// <summary>
        /// Identifies the <see cref="CaretColor"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'caret-color'.</remarks>
        public static readonly DependencyProperty CaretColorProperty = DependencyProperty.Register("CaretColor", typeof(Color), typeof(TextBox),
            new PropertyMetadata<Color>(Color.Blue * 0.4f));

        /// <summary>
        /// Identifies the <see cref="SelectionImage"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'selection-image'.</remarks>
        public static readonly DependencyProperty SelectionImageProperty = DependencyProperty.Register("SelectionImage", typeof(SourcedImage), typeof(TextBox),
            new PropertyMetadata<SourcedImage>(HandleSelectionImageChanged));

        /// <summary>
        /// Identifies the <see cref="SelectionColor"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'selection-color'.</remarks>
        public static readonly DependencyProperty SelectionColorProperty = DependencyProperty.Register("SelectionColor", typeof(Color), typeof(TextBox),
            new PropertyMetadata<Color>(Color.Blue * 0.4f));

        /// <summary>
        /// Identifies the <see cref="InsertionMode"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'insertion-mode'.</remarks>
        public static readonly DependencyProperty InsertionModeProperty = DependencyProperty.Register("InsertionMode", typeof(TextBoxInsertionMode), typeof(TextBox),
            new PropertyMetadata<TextBoxInsertionMode>(TextBoxInsertionMode.Insert));

        /// <summary>
        /// Identifies the <see cref="IsReadOnly"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'read-only'.</remarks>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(Boolean), typeof(TextBox),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));

        /// <summary>
        /// Invalidates the clipping region used for the text box's text.
        /// </summary>
        internal void InvalidateTextClip()
        {
            this.textClip = CalculateTextClip();
        }

        /// <inheritdoc/>
        protected override void OnLostKeyboardFocus(ref RoutedEventData data)
        {
            textSelectionLength = 0;

            base.OnLostKeyboardFocus(ref data);
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            switch (key)
            {
                case Key.Insert:
                    InsertionMode = (InsertionMode == TextBoxInsertionMode.Insert) ? 
                        TextBoxInsertionMode.Overwrite :
                        TextBoxInsertionMode.Insert;
                    data.Handled = true;
                    break;

                case Key.A:
                    if ((modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        SelectAll();
                        data.Handled = true;
                    }
                    break;

                case Key.C:
                    if ((modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        Copy();
                        data.Handled = true;
                    }
                    break;

                case Key.X:
                    if ((modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        Cut();
                        data.Handled = true;
                    }
                    break;

                case Key.V:
                    if ((modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        Paste();
                        data.Handled = true;
                    }
                    break;

                case Key.Left:
                    MoveBackward((modifiers & ModifierKeys.Shift) == ModifierKeys.Shift);
                    data.Handled = true;
                    break;

                case Key.Right:
                    MoveForward((modifiers & ModifierKeys.Shift) == ModifierKeys.Shift);
                    data.Handled = true;
                    break;

                case Key.Home:
                    MoveHome();
                    data.Handled = true;
                    break;

                case Key.End:
                    MoveEnd();
                    data.Handled = true;
                    break;

                case Key.Backspace:
                    ProcessBackspace();
                    data.Handled = true;
                    break;

                case Key.Delete:
                    ProcessDelete();
                    data.Handled = true;
                    break;
            }

            base.OnKeyDown(device, key, modifiers, ref data);
        }

        /// <inheritdoc/>
        protected override void OnTextInput(KeyboardDevice device, ref RoutedEventData data)
        {
            device.GetTextInput(textBuffer);
            ProcessInsertText(textBuffer.ToString());
            data.Handled = true;

            base.OnTextInput(device, ref data);
        }

        /// <inheritdoc/>
        protected override void OnLostMouseCapture(ref RoutedEventData data)
        {
            mouseSelectionInProgress = false;

            base.OnLostMouseCapture(ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, ref RoutedEventData data)
        {
            if (mouseSelectionInProgress && !String.IsNullOrEmpty(Text))
            {
                // Cursor is inside box
                var textArea = AbsoluteTextBounds;
                if (textArea.Left <= x && textArea.Right > x)
                {
                    var index = CalculateIndexFromCursor(device);
                    SelectToIndex(index);
                    ScrollToSelectionHead();
                }
                else
                {
                    var delta = (Int32)dx;

                    // Cursor is left of box, moving left
                    if (x < textArea.Left && delta < 0)
                    {
                        MoveSelectionLeft(Math.Abs(delta));
                        ScrollToSelectionHead();
                    }

                    // Cursor is right of box, moving right
                    if (x >= textArea.Right && delta > 0)
                    {
                        MoveSelectionRight(Math.Abs(delta));
                        ScrollToSelectionHead();
                    }
                }
                data.Handled = true;
            }
            base.OnMouseMove(device, x, y, dx, dy, ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                mouseSelectionInProgress = true;

                Focus();
                CaptureMouse();

                textCaretPosition   = CalculateIndexFromCursor(device);
                textSelectionLength = 0;

                ScrollForwardToCaret();
            }
            data.Handled = true;
            base.OnMouseDown(device, button, ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                mouseSelectionInProgress = false;
                ReleaseMouseCapture();
            }
            data.Handled = true;
            base.OnMouseUp(device, button, ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseDoubleClick(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                SelectAll();
            }
            data.Handled = true;
            base.OnMouseDoubleClick(device, button, ref data);
        }

        /// <inheritdoc/>
        protected override void ReloadContentCore(Boolean recursive)
        {
            ReloadCaretImage();
            ReloadSelectionImage();

            base.ReloadContentCore(recursive);
        }
        
        /// <summary>
        /// Draws the element's text.
        /// </summary>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        protected virtual void DrawText(DrawingContext dc)
        {
            if (!Font.IsLoaded || (String.IsNullOrEmpty(Text) && View.ElementWithFocus != this))
                return;

            var textArea = AbsoluteTextBounds;
            if (textArea.Width <= 0 || textArea.Height <= 0)
                return;

            if (textClip != null)
                dc.PushClipRectangle(textClip.Value);

            DrawTextSelection(dc);

            if (!String.IsNullOrEmpty(Text))
            {
                var textCenteringOffset = (Display.DipsToPixels(textArea.Height) - Font.Resource.Value.Regular.LineSpacing) / 2.0;
                var textPos             = Display.DipsToPixels(textArea.Location + new Point2D(textScrollOffset, textCenteringOffset));
                dc.SpriteBatch.DrawString(Font.Resource.Value.Regular, Text, (Vector2)textPos, Foreground * dc.Opacity);
            }

            DrawTextCaret(dc);

            if (textClip != null)
                dc.PopClipRectangle();
        }

        /// <summary>
        /// Draws the text selection box.
        /// </summary>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        protected virtual void DrawTextSelection(DrawingContext dc)
        {
            if (!IsTextSelected || !Font.IsLoaded)
                return;

            var selectionStartOffset = CalculateOffsetFromIndex(SelectionStart);
            var selectionEndOffset   = CalculateOffsetFromIndex(SelectionEnd);
            var selectionWidth       = selectionEndOffset - selectionStartOffset;
            var selectionHeight      = Font.Resource.Value.Regular.LineSpacing;
            var selectionPosition    = RelativeTextBounds + new Point2D(textScrollOffset + selectionStartOffset, 0);
            var selectionArea        = new RectangleD(selectionPosition.X, selectionPosition.Y, selectionWidth, selectionHeight);

            DrawImage(dc, SelectionImage, selectionArea, SelectionColor);
        }

        /// <summary>
        /// Draws the text caret.
        /// </summary>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        protected virtual void DrawTextCaret(DrawingContext dc)
        {
            if (!IsCaretDisplayed || !IsCaretVisible || IsTextSelected)
                return;

            var caretOffset = CalculateCaretOffset();

            var textBounds = RelativeTextBounds;
            var textX      = textBounds.X;
            var textY      = textBounds.Y;
            var textWidth  = textBounds.Width;
            var textHeight = (Font.IsLoaded) ? Font.Resource.Value.Regular.LineSpacing : textBounds.Height;

            if (InsertionMode == TextBoxInsertionMode.Insert)
            {
                var caretPosition = new Point2D(textX + textScrollOffset + caretOffset, textY);
                var caretArea     = new RectangleD(caretPosition.X, caretPosition.Y, CaretWidth, textHeight);

                DrawImage(dc, CaretImage, caretArea, CaretColor);
            }
            else
            {
                var textLength = (Text == null) ? 0 : Text.Length;
                var caretChar1 = (textCaretPosition >= textLength) ? ' ' : Text[textCaretPosition];
                var caretChar2 = (textCaretPosition + 1 >= textLength) ? ' ' : Text[textCaretPosition + 1];
                var caretWidth = Font.Resource.Value.Regular.MeasureGlyph(caretChar1, caretChar2).Width;

                var caretThickness = (Int32)Display.DipsToPixels(CaretThickness);
                var caretPosition  = new Point2D(textX + textScrollOffset + caretOffset, textY + textHeight - caretThickness);
                var caretArea      = new RectangleD(caretPosition.X, caretPosition.Y, caretWidth, caretThickness);

                DrawImage(dc, CaretImage, caretArea, CaretColor);
            }
        }

        /// <summary>
        /// Reloads the <see cref="CaretImage"/> resource.
        /// </summary>
        protected void ReloadCaretImage()
        {
            LoadImage(CaretImage);
        }

        /// <summary>
        /// Reloads the <see cref="SelectionImage"/> resource.
        /// </summary>
        protected void ReloadSelectionImage()
        {
            LoadImage(SelectionImage);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Text"/> dependency property changes.
        /// </summary>
        private static void HandleTextChanged(DependencyObject dobj, String oldValue, String newValue)
        {
            var textbox = (TextBox)dobj;

            textbox.textCaretPosition   = Math.Min(textbox.textCaretPosition, (textbox.Text == null) ? 0 : textbox.Text.Length);
            textbox.textSelectionLength = 0;

            textbox.textClip = textbox.CalculateTextClip();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Pattern"/> dependency property changes.
        /// </summary>
        private static void HandlePatternChanged(DependencyObject dobj, String oldValue, String newValue)
        {
            var textbox = (TextBox)dobj;
            var pattern = textbox.Pattern;
            textbox.patternRegex = String.IsNullOrEmpty(pattern) ? null : new Regex("^" + pattern + "$", RegexOptions.Singleline);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CaretImage"/> dependency property changes.
        /// </summary>
        private static void HandleCaretImageChanged(DependencyObject dobj, SourcedImage oldValue, SourcedImage newValue)
        {
            var textbox = (TextBox)dobj;
            textbox.ReloadCaretImage();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionImage"/> dependency property changes.
        /// </summary>
        private static void HandleSelectionImageChanged(DependencyObject dobj, SourcedImage oldValue, SourcedImage newValue)
        {
            var textbox = (TextBox)dobj;
            textbox.ReloadSelectionImage();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Control.Font"/> dependency property changes.
        /// </summary>
        private static void HandleFontChanged(DependencyObject dobj, SourcedResource<SpriteFont> oldValue, SourcedResource<SpriteFont> newValue)
        {
            var textbox = (TextBox)dobj;
            textbox.ReloadFont();
        }

        /// <summary>
        /// Handles the content presenter's <see cref="UIElement.Updating"/> event.
        /// </summary>
        private void PresenterUpdate(UIElement element, UltravioletTime time)
        {
            caretBlinkTimer = (caretBlinkTimer + time.ElapsedTime.TotalMilliseconds) % 1000.0; 
        }

        /// <summary>
        /// Handles the content presenter's <see cref="UIElement.Drawing"/> event.
        /// </summary>
        private void PresenterDraw(UIElement element, UltravioletTime time, DrawingContext dc)
        {
            DrawText(dc);
        }

        /// <summary>
        /// Processes a text insertion event.
        /// </summary>
        /// <param name="text">The text to insert.</param>
        private void ProcessInsertText(String text)
        {
            DeleteSelection();

            var startingCaretPosition = textCaretPosition;

            var charactersUsed      = (Text == null) ? 0 : Text.Length;
            var charactersAvailable = ((MaxLength == 0) ? Int32.MaxValue : MaxLength) - charactersUsed;

            if (charactersAvailable == 0)
                return;

            if (text != null && text.Length > charactersAvailable)
                text = text.Substring(0, charactersAvailable);

            var textLength = (text == null) ? 0 : text.Length;

            if (Text == null)
            {
                if (patternRegex != null && !patternRegex.IsMatch(text))
                    return;

                Text = text;
                textCaretPosition = textLength;
            }
            else
            {
                var textTemp = Text;

                if (InsertionMode == TextBoxInsertionMode.Overwrite && startingCaretPosition < Text.Length)
                    textTemp = textTemp.Remove(startingCaretPosition, 1);

                textTemp = textTemp.Insert(startingCaretPosition, text);

                if (patternRegex != null && !patternRegex.IsMatch(textTemp))
                    return;

                Text = textTemp;
                textCaretPosition = Math.Min(Text.Length, startingCaretPosition + textLength);
            }

            ScrollForwardToCaret();

            caretBlinkTimer = 0;
        }

        /// <summary>
        /// Processes a backspace event.
        /// </summary>
        private void ProcessBackspace()
        {
            if (IsTextSelected)
            {
                DeleteSelection();
            }
            else
            {
                if (textCaretPosition > 0)
                {
                    var position = textCaretPosition;
                    Text = Text.Remove(position - 1, 1);
                    textCaretPosition = position - 1;
                }
            }
            ScrollBackwardToCaret();
            caretBlinkTimer = 0;
        }

        /// <summary>
        /// Processes a delete event.
        /// </summary>
        private void ProcessDelete()
        {
            if (IsTextSelected)
            {
                DeleteSelection();
            }
            else
            {
                if (Text != null && textCaretPosition < Text.Length)
                {
                    Text = Text.Remove(textCaretPosition, 1);
                }
            }
            ScrollBackwardToCaret();
            caretBlinkTimer = 0;
        }

        /// <summary>
        /// Moves the cursor forward.
        /// </summary>
        /// <param name="select">A value indicating whether to update the selection during movement.</param>
        private void MoveForward(Boolean select)
        {
            if (IsTextSelected && !select)
            {
                textCaretPosition = SelectionEnd;
                textSelectionLength = 0;
            }
            else
            {
                if (Text != null && textCaretPosition < Text.Length)
                {
                    textCaretPosition++;
                    if (select)
                    {
                        textSelectionLength--;
                    }
                }
            }

            ScrollForwardToCaret();
            
            caretBlinkTimer = 0;
        }

        /// <summary>
        /// Moves the cursor backward.
        /// </summary>
        /// <param name="select">A value indicating whether to update the selection during movement.</param>
        private void MoveBackward(Boolean select)
        {
            if (IsTextSelected && !select)
            {
                textCaretPosition = SelectionStart;
                textSelectionLength = 0;
            }
            else
            {
                if (textCaretPosition > 0)
                {
                    textCaretPosition--;
                    if (select)
                    {
                        textSelectionLength++;
                    }
                }
            }

            ScrollBackwardToCaret();

            caretBlinkTimer = 0;
        }

        /// <summary>
        /// Scrolls the text forward to the position of the caret.
        /// </summary>
        private void ScrollForwardToCaret()
        {
            ScrollForwardToIndex(textCaretPosition);
        }

        /// <summary>
        /// Scrolls the text forwards to the specified character index.
        /// </summary>
        /// <param name="ix">The index of the character to which to scroll the text box.</param>
        private void ScrollForwardToIndex(Int32 ix)
        {
            var width  = RelativeTextBounds.Width;
            var offset = CalculateOffsetFromIndex(ix);
            if (offset + textScrollOffset > width)
            {
                ScrollToOffset((offset - width) + CaretWidth);
            }
        }

        /// <summary>
        /// Scrolls the text backwards to the position of the caret.
        /// </summary>
        private void ScrollBackwardToCaret()
        {
            ScrollBackwardToIndex(textCaretPosition);
        }

        /// <summary>
        /// Scrolls the text backwards to the specified character index.
        /// </summary>
        /// <param name="ix">The index of the character to which to scroll the text box.</param>
        private void ScrollBackwardToIndex(Int32 ix)
        {
            var width  = RelativeTextBounds.Width;
            var offset = CalculateOffsetFromIndex(ix);
            if (offset + textScrollOffset < 0)
            {
                ScrollToOffset((offset - (width / 3)) + CaretWidth);
            }
        }

        /// <summary>
        /// Scrolls the text to the specified offset.
        /// </summary>
        /// <param name="offset">The offset to which to scroll the text.</param>
        private void ScrollToOffset(Double offset)
        {
            textScrollOffset = -Math.Max(0, offset);
        }

        /// <summary>
        /// Scrolls the text box to the position of the selection head.
        /// </summary>
        private void ScrollToSelectionHead()
        {
            var offset   = CalculateOffsetFromIndex(SelectionHead);
            var position = GetRelativeOffsetPosition(offset);
            switch (position)
            {
                case OffsetPosition.Left:
                    ScrollToOffset(offset);
                    break;

                case OffsetPosition.Right:
                    var length = (Text == null) ? 0 : Text.Length;
                    var next   = CalculateOffsetFromIndex(Math.Min(length, SelectionHead + 1));
                    ScrollToOffset(next - RelativeTextBounds.Width);
                    break;
            }
        }

        /// <summary>
        /// Moves the current selection left by the specified number of characters.
        /// </summary>
        /// <param name="count">The number of characters to move the selection.</param>
        private void MoveSelectionLeft(Int32 count)
        {
            if (Text == null)
                return;

            var minLength = -textCaretPosition;
            textSelectionLength = Math.Max(minLength, textSelectionLength - count);
        }

        /// <summary>
        /// Moves the current selection right by the specified number of characters.
        /// </summary>
        /// <param name="count">The number of characters to move the selection.</param>
        private void MoveSelectionRight(Int32 count)
        {
            if (Text == null)
                return;

            var maxLength = Text.Length - textCaretPosition;
            textSelectionLength = Math.Min(maxLength, textSelectionLength + count);
        }

        /// <summary>
        /// Selects the entire text.
        /// </summary>
        private void SelectAll()
        {
            if (Text == null)
                return;

            textCaretPosition   = 0;
            textSelectionLength = Text.Length;
        }

        /// <summary>
        /// Moves the selection so that it is between the current caret position
        /// and the specified character index.
        /// </summary>
        /// <param name="ix">The index which bounds the selection.</param>
        private void SelectToIndex(Int32 ix)
        {
            textSelectionLength = ix - textCaretPosition;
        }

        /// <summary>
        /// Deletes the currently selected text.
        /// </summary>
        private void DeleteSelection()
        {
            if (!IsTextSelected)
                return;

            var updatedText          = Text.Remove(SelectionStart, SelectionEnd - SelectionStart);
            var updatedCaretPosition = SelectionStart;

            Text = updatedText;

            textCaretPosition   = updatedCaretPosition;
            textSelectionLength = 0;

            ScrollBackwardToCaret();
        }

        /// <summary>
        /// Copies the current selection.
        /// </summary>
        private void Copy()
        {
            if (!IsTextSelected)
                return;

            Ultraviolet.GetPlatform().Clipboard.Text = SelectedText;
        }

        /// <summary>
        /// Cuts the current selection.
        /// </summary>
        private void Cut()
        {
            Copy();
            DeleteSelection();
        }

        /// <summary>
        /// Pastes the contents of the clipboard into the textbox.
        /// </summary>
        private void Paste()
        {
            var text = Ultraviolet.GetPlatform().Clipboard.Text;
            if (text != null)
            {
                DeleteSelection();
                ProcessInsertText(text);
            }
        }

        /// <summary>
        /// Gets the position of the specified text offset relative to the currently visible text area.
        /// </summary>
        /// <param name="offset">The text offset to evaluate.</param>
        /// <returns>A <see cref="OffsetPosition"/> value indicating whether the offset is to the left of, to the right of,
        /// or inside of the currently visible text area.</returns>
        private OffsetPosition GetRelativeOffsetPosition(Double offset)
        {
            var relativeArea   = RelativeTextBounds;
            var relativeOffset = offset + textScrollOffset;

            if (relativeOffset < 0)
                return OffsetPosition.Left;

            if (relativeOffset >= relativeArea.Width)
                return OffsetPosition.Right;

            return OffsetPosition.Visible;
        }

        /// <summary>
        /// Calculates the offset of the character beneath the mouse cursor.
        /// </summary>
        /// <param name="mouse">The mouse device.</param>
        /// <returns>The offset of the character beneath the mouse cursor.</returns>
        private Double CalculateOffsetFromCursor(MouseDevice mouse)
        {
            var cursorpos  = Display.PixelsToDips((Point2D)mouse.Position) - AbsolutePosition;
            var textOffset = (cursorpos.X - RelativeTextBounds.X) - textScrollOffset;

            return textOffset;
        }

        /// <summary>
        /// Calculates the index of the character beneath the mouse cursor.
        /// </summary>
        /// <param name="mouse">The mouse device.</param>
        /// <returns>The index of the character beneath the mouse cursor.</returns>
        private Int32 CalculateIndexFromCursor(MouseDevice mouse)
        {
            var textOffset = CalculateOffsetFromCursor(mouse);
            var textIndex  = CalculateIndexFromOffset(textOffset);

            return textIndex;
        }

        /// <summary>
        /// Calculates the index of the character at the specified offset within the text.
        /// </summary>
        /// <param name="offset">The offset in pixels for which to calculate an index.</param>
        /// <returns>The index of the character at the specified offset within the text.</returns>
        private Int32 CalculateIndexFromOffset(Double offset)
        {
            if (String.IsNullOrEmpty(Text) || !Font.IsLoaded)
                return 0;

            if (offset <= 0)
                return 0;

            var font     = Font.Resource.Value.Regular;
            var length   = 0;
            var pxoffset = (Int32)Display.DipsToPixels(offset);

            for (int i = 0; i < Text.Length; i++)
            {
                length += font.MeasureGlyph(Text, i).Width;

                if (pxoffset < length)
                    return i;
            }

            return Text.Length;
        }

        /// <summary>
        /// Calculates the offset of the specified character index.
        /// </summary>
        /// <param name="ix">The index of the character for which to calculate an offset.</param>
        /// <returns>The offset of the specified character in pixels.</returns>
        private Double CalculateOffsetFromIndex(Int32 ix)
        {
            if (String.IsNullOrEmpty(Text) || !Font.IsLoaded || ix <= 0 || ix > Text.Length)
                return 0;

            var font      = Font.Resource.Value.Regular;
            var text      = Text;
            var endOfLine = ix == text.Length;
            var segment   = new StringSegment(text, 0, ix);
            var kerning   = endOfLine ? 0 : font.Kerning.Get(text[ix - 1], endOfLine ? ' ' : text[ix]) / 2;
            var measure   = font.MeasureString(segment);

            return Display.PixelsToDips(measure.Width + kerning);
        }

        /// <summary>
        /// Calculates the current offset of the caret.
        /// </summary>
        /// <returns>The offset of the caret in pixels.</returns>
        private Double CalculateCaretOffset()
        {
            return CalculateOffsetFromIndex(textCaretPosition);
        }

        /// <summary>
        /// Calculates the clipping rectangle to apply to the control's text.
        /// </summary>
        /// <returns>The clipping rectangle to apply to the control's text, or <c>null</c> if no clipping should take place.</returns>
        private RectangleD? CalculateTextClip()
        {
            if (!Font.IsLoaded || (String.IsNullOrEmpty(Text) && View.ElementWithFocus != this))
                return null;

            return AbsoluteTextBounds;
        }

        /// <summary>
        /// Gets the index of the start of the selection.
        /// </summary>
        private Int32 SelectionStart
        {
            get { return IsTextSelected ? Math.Min(textCaretPosition + textSelectionLength, textCaretPosition) : textCaretPosition; }
        }

        /// <summary>
        /// Gets the index of the end of the selection.
        /// </summary>
        private Int32 SelectionEnd
        {
            get { return IsTextSelected ? Math.Max(textCaretPosition + textSelectionLength, textCaretPosition) : textCaretPosition; }
        }

        /// <summary>
        /// Gets the "head" of the current selection, which is the end of the selection
        /// which is not the current text position.
        /// </summary>
        private Int32 SelectionHead
        {
            get { return IsTextSelected ? textCaretPosition + textSelectionLength : textCaretPosition; }
        }

        /// <summary>
        /// Gets the length of the text box's current text selection.
        /// </summary>
        private Int32 SelectionLength
        {
            get { return textSelectionLength; }
        }

        /// <summary>
        /// Gets the width of the text box's caret in pixels.
        /// </summary>
        private Double CaretWidth
        {
            get { return 1.0; }
        }

        /// <summary>
        /// Gets a value indicating whether the caret is currently being displayed.
        /// </summary>
        private Boolean IsCaretDisplayed
        {
            get { return View != null && View.ElementWithFocus == this; }
        }

        /// <summary>
        /// Gets a value indicating whether the caret is currently visible.
        /// </summary>
        private Boolean IsCaretVisible
        {
            get { return (Int32)(caretBlinkTimer / 500.0) % 2 == 0; }
        }

        /// <summary>
        /// Gets a value indicating whether any text is currently selected.
        /// </summary>
        private Boolean IsTextSelected
        {
            get { return textSelectionLength != 0; }
        }

        /// <summary>
        /// Gets the bounds of the text box's text region in absolute screen coordinates.
        /// </summary>
        private RectangleD AbsoluteTextBounds
        {
            get
            {
                if (PART_ContentPresenter == null)
                    return RectangleD.Empty;

                return PART_ContentPresenter.AbsoluteBounds;
            }
        }

        /// <summary>
        /// Gets the bounds of the text box's text region in control-relative coordinates.
        /// </summary>
        private RectangleD RelativeTextBounds
        {
            get
            {
                if (PART_ContentPresenter == null)
                    return RectangleD.Empty;

                var offset = PART_ContentPresenter.AbsolutePosition - AbsolutePosition;
                return PART_ContentPresenter.Bounds + offset;
            }
        }

        // State values.
        private readonly StringBuilder textBuffer = new StringBuilder();
        private Boolean mouseSelectionInProgress;
        private Int32 textCaretPosition = 0;
        private Int32 textSelectionLength = 0;
        private Double textScrollOffset = 0;
        private Double caretBlinkTimer;
        private Regex patternRegex;
        private RectangleD? textClip;

        // Component references.
        private readonly ContentPresenter PART_ContentPresenter = null;
    }
}
