using System;
using System.ComponentModel;
using System.Text;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a UI element used for text entry.
    /// </summary>
    [UIElement("TextBox")]
    [DefaultProperty("Text")]
    public class TextBox : UIElement
    {
        /// <summary>
        /// Represents the position of a text offset relative to the text area.
        /// </summary>
        private enum OffsetPosition
        {
            /// <summary>
            /// The offset is to the left of the text area.
            /// </summary>
            Left,

            /// <summary>
            /// The offset is currently visible.
            /// </summary>
            Visible,

            /// <summary>
            /// The offset is to the right of the text area.
            /// </summary>
            Right,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public TextBox(UltravioletContext uv, String id)
            : base(uv, id)
        {
            SetDefaultValue<Boolean>(FocusableProperty, true);
        }

        /// <inheritdoc/>
        public override void ReloadContent()
        {
            ReloadCaretImage();
            ReloadSelectionImage();

            base.ReloadContent();
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
        /// Gets or sets the maximum length of the text box's text.
        /// </summary>
        public Int32 MaxLength
        {
            get { return GetValue<Int32>(MaxLengthProperty); }
            set { SetValue<Int32>(MaxLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image used to draw the caret.
        /// </summary>
        public SourcedRef<Image> CaretImage
        {
            get { return GetValue<SourcedRef<Image>>(CaretImageProperty); }
            set { SetValue<SourcedRef<Image>>(CaretImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the thickness of the caret while the text box's <see cref="InsertionMode"/> property
        /// is set to <see cref="TextBoxInsertionMode.Overwrite"/>, specified in device independent pixels (1/96 of an inch).
        /// </summary>
        public Double CaretThickness
        {
            get { return GetValue<Double>(CaretThicknessProperty); }
            set { SetValue<Double>(CaretThicknessProperty, value); }
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
        public SourcedRef<Image> SelectionImage
        {
            get { return GetValue<SourcedRef<Image>>(SelectionImageProperty); }
            set { SetValue<SourcedRef<Image>>(SelectionImageProperty, value); }
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
        /// Occurs when the value of the <see cref="Text"/> property changes.
        /// </summary>
        public event UIElementEventHandler TextChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="MaxLength"/> property changes.
        /// </summary>
        public event UIElementEventHandler MaxLengthChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="CaretImage"/> property changes.
        /// </summary>
        public event UIElementEventHandler CaretImageChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="CaretThickness"/> property changes.
        /// </summary>
        public event UIElementEventHandler CaretThicknessChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="CaretColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler CaretColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionImage"/> property changes.
        /// </summary>
        public event UIElementEventHandler SelectionImageChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler SelectionColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="InsertionMode"/> property changes.
        /// </summary>
        public event UIElementEventHandler InsertionModeChanged;

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(TextBox),
            new DependencyPropertyMetadata(HandleTextChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="MaxLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(Int32), typeof(TextBox),
            new DependencyPropertyMetadata(HandleMaxLengthChanged, () => 0, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="CaretImage"/> dependency property.
        /// </summary>
        [Styled("caret-image")]
        public static readonly DependencyProperty CaretImageProperty = DependencyProperty.Register("CaretImage", typeof(SourcedRef<Image>), typeof(TextBox),
            new DependencyPropertyMetadata(HandleCaretImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="CaretColor"/> dependency property.
        /// </summary>
        [Styled("caret-color")]
        public static readonly DependencyProperty CaretColorProperty = DependencyProperty.Register("CaretColor", typeof(Color), typeof(TextBox),
            new DependencyPropertyMetadata(HandleCaretColorChanged, () => Color.DarkBlue * 0.8f, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="CaretThickness"/> dependency property.
        /// </summary>
        [Styled("caret-thickness")]
        public static readonly DependencyProperty CaretThicknessProperty = DependencyProperty.Register("CaretThickness", typeof(Double), typeof(TextBox),
            new DependencyPropertyMetadata(HandleCaretThicknessChanged, () => 4.0, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="SelectionImage"/> dependency property.
        /// </summary>
        [Styled("selection-image")]
        public static readonly DependencyProperty SelectionImageProperty = DependencyProperty.Register("SelectionImage", typeof(SourcedRef<Image>), typeof(TextBox),
            new DependencyPropertyMetadata(HandleSelectionImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="SelectionColor"/> dependency property.
        /// </summary>
        [Styled("selection-color")]
        public static readonly DependencyProperty SelectionColorProperty = DependencyProperty.Register("SelectionColor", typeof(Color), typeof(TextBox),
            new DependencyPropertyMetadata(HandleSelectionColorChanged, () => Color.Blue * 0.4f, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="InsertionMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InsertionModeProperty = DependencyProperty.Register("InsertionMode", typeof(TextBoxInsertionMode), typeof(TextBox),
            new DependencyPropertyMetadata(HandleInsertionModeChanged, () => TextBoxInsertionMode.Insert, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected internal override void OnKeyPressed(KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat)
        {
            switch (key)
            {
                case Key.Insert:
                    InsertionMode = (InsertionMode == TextBoxInsertionMode.Insert) ? 
                        TextBoxInsertionMode.Overwrite :
                        TextBoxInsertionMode.Insert;
                    break;

                case Key.A:
                    if (ctrl)
                    {
                        SelectAll();
                    }
                    break;

                case Key.C:
                    if (ctrl)
                    {
                        Copy();
                    }
                    break;

                case Key.X:
                    if (ctrl)
                    {
                        Cut();
                    }
                    break;

                case Key.V:
                    if (ctrl)
                    {
                        Paste();
                    }
                    break;

                case Key.Left:
                    MoveBackward(shift);
                    break;

                case Key.Right:
                    MoveForward(shift);
                    break;

                case Key.Home:
                    MoveHome();
                    break;

                case Key.End:
                    MoveEnd();
                    break;

                case Key.Backspace:
                    ProcessBackspace();
                    break;

                case Key.Delete:
                    ProcessDelete();
                    break;
            }
            base.OnKeyPressed(device, key, ctrl, alt, shift, repeat);
        }

        /// <inheritdoc/>
        protected internal override void OnTextInput(KeyboardDevice device)
        {
            device.GetTextInput(textBuffer);
            ProcessInsertText(textBuffer.ToString());

            base.OnTextInput(device);
        }

        /// <inheritdoc/>
        protected internal override void OnLostMouseCapture()
        {
            mouseSelectionInProgress = false;

            base.OnLostMouseCapture();
        }

        /// <inheritdoc/>
        protected internal override void OnMouseMotion(MouseDevice device, Int32 x, Int32 y, Int32 dx, Int32 dy)
        {
            if (mouseSelectionInProgress && !String.IsNullOrEmpty(Text))
            {
                // Cursor is inside box
                var textArea = GetAbsoluteTextArea();
                if (textArea.Left <= x && textArea.Right > x)
                {
                    var index = CalculateIndexFromCursor(device);
                    SelectToIndex(index);
                    ScrollToSelectionHead();
                }
                else
                {
                    // Cursor is left of box, moving left
                    if (x < textArea.Left && dx < 0)
                    {
                        MoveSelectionLeft(Math.Abs(dx));
                        ScrollToSelectionHead();
                    }

                    // Cursor is right of box, moving right
                    if (x >= textArea.Right && dx > 0)
                    {
                        MoveSelectionRight(Math.Abs(dx));
                        ScrollToSelectionHead();
                    }
                }
            }
            base.OnMouseMotion(device, x, y, dx, dy);
        }

        /// <inheritdoc/>
        protected internal override void OnMouseButtonPressed(MouseDevice device, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                mouseSelectionInProgress = true;

                View.CaptureMouse(this);

                textPosition        = CalculateIndexFromCursor(device);
                textSelectionLength = 0;

                ScrollForwardToCaret();
            }
            base.OnMouseButtonPressed(device, button);
        }

        /// <inheritdoc/>
        protected internal override void OnMouseButtonReleased(MouseDevice device, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                mouseSelectionInProgress = false;
                View.ReleaseMouse(this);
            }
            base.OnMouseButtonReleased(device, button);
        }
        
        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch)
        {
            DrawBackgroundImage(spriteBatch);
            DrawFocusedImage(spriteBatch);
            DrawText(spriteBatch);

            base.OnDrawing(time, spriteBatch);
        }

        /// <inheritdoc/>
        protected override void OnUpdating(UltravioletTime time)
        {
            caretBlinkTimer = (caretBlinkTimer + time.ElapsedTime.TotalMilliseconds) % 1000.0;

            base.OnUpdating(time);
        }

        /// <summary>
        /// Raises the <see cref="TextChanged"/> event.
        /// </summary>
        protected virtual void OnTextChanged()
        {
            var temp = TextChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MaxLengthChanged"/> event.
        /// </summary>
        protected virtual void OnMaxLengthChanged()
        {
            var temp = MaxLengthChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="CaretImageChanged"/> event.
        /// </summary>
        protected virtual void OnCaretImageChanged()
        {
            var temp = CaretImageChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="CaretThicknessChanged"/> event.
        /// </summary>
        protected virtual void OnCaretThicknessChanged()
        {
            var temp = CaretThicknessChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="CaretColorChanged"/> event.
        /// </summary>
        protected virtual void OnCaretColorChanged()
        {
            var temp = CaretColorChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="SelectionImageChanged"/> event.
        /// </summary>
        protected virtual void OnSelectionImageChanged()
        {
            var temp = SelectionImageChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="SelectionColorChanged"/> event.
        /// </summary>
        protected virtual void OnSelectionColorChanged()
        {
            var temp = SelectionColorChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="InsertionModeChanged"/> event.
        /// </summary>
        protected virtual void OnInsertionModeChanged()
        {
            var temp = InsertionModeChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Draws the element's text.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw the element's text.</param>
        protected virtual void DrawText(SpriteBatch spriteBatch)
        {
            if (Font == null || (String.IsNullOrEmpty(Text) && !View.HasFocus(this)))
                return;

            var textArea = GetAbsoluteTextArea();

            var scissorCurrent = Ultraviolet.GetGraphics().GetScissorRectangle();
            var scissorText    = textArea;

            spriteBatch.Flush();
            Ultraviolet.GetGraphics().SetScissorRectangle(scissorText);

            DrawTextSelection(spriteBatch, textArea);

            if (!String.IsNullOrEmpty(Text))
            {
                spriteBatch.DrawString(Font.Regular, Text, 
                    new Vector2(textArea.X + textScrollOffset, textArea.Y), FontColor);
            }

            DrawTextCaret(spriteBatch, textArea);

            spriteBatch.Flush();
            Ultraviolet.GetGraphics().SetScissorRectangle(scissorCurrent);
        }

        /// <summary>
        /// Draws the text selection box.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw the text selection box.</param>
        /// <param name="textArea">A <see cref="Rectangle"/> describing the absolute screen area in which the element draws its text.</param>
        protected virtual void DrawTextSelection(SpriteBatch spriteBatch, Rectangle textArea)
        {
            if (IsTextSelected && SelectionImage.Value != null && SelectionImage.Value.IsLoaded && Font != null)
            {
                var selectionStartOffset = CalculateOffsetFromIndex(SelectionStart);
                var selectionEndOffset   = CalculateOffsetFromIndex(SelectionEnd);
                var selectionWidth       = selectionEndOffset - selectionStartOffset;
                var selectionHeight      = Font.Regular.LineSpacing;
                var selectionPosition    = new Vector2(textArea.X + textScrollOffset + selectionStartOffset, textArea.Y);

                spriteBatch.DrawImage(SelectionImage.Value, selectionPosition, selectionWidth, selectionHeight, SelectionColor);
            }
        }

        /// <summary>
        /// Draws the text caret.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw the text caret.</param>
        /// <param name="textArea">A <see cref="Rectangle"/> describing the absolute screen area in which the element draws its text.</param>
        protected virtual void DrawTextCaret(SpriteBatch spriteBatch, Rectangle textArea)
        {
            if (IsCaretDisplayed && IsCaretVisible && !IsTextSelected)
            {
                if (CaretImage.Value == null || !CaretImage.Value.IsLoaded)
                    return;

                var caretOffset = CalculateCaretOffset();

                if (InsertionMode == TextBoxInsertionMode.Insert)
                {
                    var caretPosition = new Vector2(textArea.X + textScrollOffset + caretOffset, textArea.Y);
                    spriteBatch.DrawImage(CaretImage.Value, caretPosition, 1, Font.Regular.LineSpacing, CaretColor);
                }
                else
                {
                    var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;

                    var textLength = (Text == null) ? 0 : Text.Length;
                    var caretChar1 = (textPosition >= textLength) ? ' ' : Text[textPosition];
                    var caretChar2 = (textPosition + 1 >= textLength) ? ' ' : Text[textPosition + 1];
                    var caretWidth = Font.Regular.MeasureGlyph(caretChar1, caretChar2).Width;

                    var caretThickness = (Int32)display.DipsToPixels(CaretThickness);
                    var caretPosition  = new Vector2(textArea.X + textScrollOffset + caretOffset, textArea.Bottom - caretThickness);

                    spriteBatch.DrawImage(CaretImage.Value, caretPosition, caretWidth, caretThickness, CaretColor);
                }
            }
        }

        /// <summary>
        /// Reloads the caret image.
        /// </summary>
        protected void ReloadCaretImage()
        {
            LoadContent(CaretImage);
        }

        /// <summary>
        /// Reloads the selection highlight image.
        /// </summary>
        protected void ReloadSelectionImage()
        {
            LoadContent(SelectionImage);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Text"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleTextChanged(DependencyObject dobj)
        {
            var textbox = (TextBox)dobj;
            textbox.OnTextChanged();

            textbox.textPosition        = Math.Min(textbox.textPosition, (textbox.Text == null) ? 0 : textbox.Text.Length);
            textbox.textSelectionLength = 0;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MaxLength"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleMaxLengthChanged(DependencyObject dobj)
        {
            var textbox = (TextBox)dobj;
            textbox.OnMaxLengthChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CaretImage"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleCaretImageChanged(DependencyObject dobj)
        {
            var textbox = (TextBox)dobj;
            textbox.ReloadCaretImage();
            textbox.OnCaretImageChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CaretColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleCaretColorChanged(DependencyObject dobj)
        {
            var textbox = (TextBox)dobj;
            textbox.OnCaretColorChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CaretThickness"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleCaretThicknessChanged(DependencyObject dobj)
        {
            var textbox = (TextBox)dobj;
            textbox.OnCaretThicknessChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionImage"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleSelectionImageChanged(DependencyObject dobj)
        {
            var textbox = (TextBox)dobj;
            textbox.ReloadSelectionImage();
            textbox.OnSelectionImageChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleSelectionColorChanged(DependencyObject dobj)
        {
            var textbox = (TextBox)dobj;
            textbox.OnSelectionColorChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="InsertionMode"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleInsertionModeChanged(DependencyObject dobj)
        {
            var textbox = (TextBox)dobj;
            textbox.OnInsertionModeChanged();
        }

        /// <summary>
        /// Processes a text insertion event.
        /// </summary>
        /// <param name="text">The text to insert.</param>
        private void ProcessInsertText(String text)
        {
            DeleteSelection();

            var charactersUsed      = (Text == null) ? 0 : Text.Length;
            var charactersAvailable = ((MaxLength == 0) ? Int32.MaxValue : MaxLength) - charactersUsed;

            if (charactersAvailable == 0)
                return;

            if (text != null && text.Length > charactersAvailable)
                text = text.Substring(0, charactersAvailable);

            var textLength = (text == null) ? 0 : text.Length;

            if (Text == null)
            {
                Text         = text;
                textPosition = textLength;
            }
            else
            {
                if (InsertionMode == TextBoxInsertionMode.Overwrite && textPosition < Text.Length)
                    Text = Text.Remove(textPosition, 1);

                var position = textPosition + textLength;
                Text         = Text.Insert(textPosition, text);
                textPosition = position;
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
                if (textPosition > 0)
                {
                    textPosition--;
                    Text = Text.Remove(textPosition, 1);
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
                if (Text != null && textPosition < Text.Length)
                {
                    Text = Text.Remove(textPosition, 1);
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
                textPosition = SelectionEnd;
                textSelectionLength = 0;
            }
            else
            {
                if (Text != null && textPosition < Text.Length)
                {
                    textPosition++;
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
                textPosition = SelectionStart;
                textSelectionLength = 0;
            }
            else
            {
                if (textPosition > 0)
                {
                    textPosition--;
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
        /// Moves the caret to the beginning of the text.
        /// </summary>
        private void MoveHome()
        {
            textPosition     = 0;
            textScrollOffset = 0;
            caretBlinkTimer  = 0;
        }

        /// <summary>
        /// Moves the caret to the end of the text.
        /// </summary>
        private void MoveEnd()
        {
            if (Text != null)
            {
                textPosition = Text.Length;
                ScrollForwardToCaret();
            }
            caretBlinkTimer = 0;
        }

        /// <summary>
        /// Scrolls the text forward to the position of the caret.
        /// </summary>
        private void ScrollForwardToCaret()
        {
            ScrollForwardToIndex(textPosition);
        }

        /// <summary>
        /// Scrolls the text forwards to the specified character index.
        /// </summary>
        /// <param name="ix">The index of the character to which to scroll the text box.</param>
        private void ScrollForwardToIndex(Int32 ix)
        {
            var width  = GetRelativeTextArea().Width;
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
            ScrollBackwardToIndex(textPosition);
        }

        /// <summary>
        /// Scrolls the text backwards to the specified character index.
        /// </summary>
        /// <param name="ix">The index of the character to which to scroll the text box.</param>
        private void ScrollBackwardToIndex(Int32 ix)
        {
            var width  = GetRelativeTextArea().Width;
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
        private void ScrollToOffset(Int32 offset)
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
                    ScrollToOffset(next - GetRelativeTextArea().Width);
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

            var minLength = -textPosition;
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

            var maxLength = Text.Length - textPosition;
            textSelectionLength = Math.Min(maxLength, textSelectionLength + count);
        }

        /// <summary>
        /// Selects the entire text.
        /// </summary>
        private void SelectAll()
        {
            if (Text == null)
                return;

            textPosition        = 0;
            textSelectionLength = Text.Length;
        }

        /// <summary>
        /// Moves the selection so that it is between the current caret position
        /// and the specified character index.
        /// </summary>
        /// <param name="ix">The index which bounds the selection.</param>
        private void SelectToIndex(Int32 ix)
        {
            textSelectionLength = ix - textPosition;
        }

        /// <summary>
        /// Deletes the currently selected text.
        /// </summary>
        private void DeleteSelection()
        {
            if (!IsTextSelected)
                return;

            var updatedText = Text.Remove(SelectionStart, SelectionEnd - SelectionStart);

            textPosition        = SelectionStart;
            textSelectionLength = 0;

            Text = updatedText;

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
        /// Gets the element's text area in element space.
        /// </summary>
        /// <returns>The element's text area in element space.</returns>
        private Rectangle GetRelativeTextArea()
        {
            if (Font == null)
                return Rectangle.Empty;

            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var padding = display.DipsToPixels(Padding);

            var textAreaWidth  = ActualWidth - ((Int32)padding.Left + (Int32)padding.Right);
            var textAreaHeight = ActualHeight - ((Int32)padding.Top + (Int32)padding.Bottom);
            var textX          = (Int32)padding.Left;
            var textY          = (Int32)padding.Top + ((textAreaHeight - Font.Regular.LineSpacing) / 2);

            return new Rectangle(textX, textY, textAreaWidth, Font.Regular.LineSpacing);
        }

        /// <summary>
        /// Gets the element's text area in screen space.
        /// </summary>
        /// <returns>The element's text area in screen space.</returns>
        private Rectangle GetAbsoluteTextArea()
        {
            var relative = GetRelativeTextArea();
            return new Rectangle(AbsoluteScreenX + relative.X, AbsoluteScreenY + relative.Y, relative.Width, relative.Height);
        }

        /// <summary>
        /// Gets the position of the specified text offset relative to the currently visible text area.
        /// </summary>
        /// <param name="offset">The text offset to evaluate.</param>
        /// <returns>A <see cref="OffsetPosition"/> value indicating whether the offset is to the left of, to the right of,
        /// or inside of the currently visible text area.</returns>
        private OffsetPosition GetRelativeOffsetPosition(Int32 offset)
        {
            var relativeArea   = GetRelativeTextArea();
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
        private Int32 CalculateOffsetFromCursor(MouseDevice mouse)
        {
            var cursorPosition = ScreenPositionToElementPosition(mouse.Position);

            var textArea   = GetRelativeTextArea();
            var textOffset = (cursorPosition.X - textArea.X) - textScrollOffset;

            return (Int32)textOffset;
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
        private Int32 CalculateIndexFromOffset(Int32 offset)
        {
            if (String.IsNullOrEmpty(Text) || Font == null)
                return 0;

            if (offset <= 0)
                return 0;

            var length = 0;
            for (int i = 0; i < Text.Length; i++)
            {
                length += Font.Regular.MeasureGlyph(Text, i).Width;

                if (offset < length)
                    return i;
            }

            return Text.Length;
        }

        /// <summary>
        /// Calculates the offset of the specified character index.
        /// </summary>
        /// <param name="ix">The index of the character for which to calculate an offset.</param>
        /// <returns>The offset of the specified character in pixels.</returns>
        private Int32 CalculateOffsetFromIndex(Int32 ix)
        {
            if (String.IsNullOrEmpty(Text) || Font == null || ix <= 0 || ix > Text.Length)
                return 0;

            var text      = Text;
            var endOfLine = ix == text.Length;
            var segment   = new StringSegment(text, 0, ix);
            var kerning   = Font.Regular.Kerning.Get(text[ix - 1], endOfLine ? ' ' : text[ix]);
            var measure   = Font.Regular.MeasureString(segment);
            return measure.Width + kerning;
        }

        /// <summary>
        /// Calculates the current offset of the caret.
        /// </summary>
        /// <returns>The offset of the caret in pixels.</returns>
        private Int32 CalculateCaretOffset()
        {
            return CalculateOffsetFromIndex(textPosition);
        }

        /// <summary>
        /// Gets the index of the start of the selection.
        /// </summary>
        private Int32 SelectionStart
        {
            get { return IsTextSelected ? Math.Min(textPosition + textSelectionLength, textPosition) : textPosition; }
        }

        /// <summary>
        /// Gets the index of the end of the selection.
        /// </summary>
        private Int32 SelectionEnd
        {
            get { return IsTextSelected ? Math.Max(textPosition + textSelectionLength, textPosition) : textPosition; }
        }

        /// <summary>
        /// Gets the "head" of the current selection, which is the end of the selection
        /// which is not the current text position.
        /// </summary>
        private Int32 SelectionHead
        {
            get { return IsTextSelected ? textPosition + textSelectionLength : textPosition; }
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
        private Int32 CaretWidth
        {
            get { return 1; }
        }

        /// <summary>
        /// Gets a value indicating whether the caret is currently being displayed.
        /// </summary>
        private Boolean IsCaretDisplayed
        {
            get { return View != null && View.HasFocus(this); }
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

        // State values.
        private readonly StringBuilder textBuffer = new StringBuilder();
        private Boolean mouseSelectionInProgress;
        private Int32 textPosition = 0;
        private Int32 textSelectionLength = 0;
        private Int32 textScrollOffset = 0;
        private Double caretBlinkTimer;
    }
}
