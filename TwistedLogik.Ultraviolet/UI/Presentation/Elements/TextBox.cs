using System;
using System.ComponentModel;
using System.Text;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;

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
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public TextBox(UltravioletContext uv, String id)
            : base(uv, id)
        {

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
        /// Gets the position of the text box's cursor within its text.
        /// </summary>
        public Int32 TextPosition
        {
            get { return textPosition; }
        }

        /// <summary>
        /// Gets the length of the text box's current text selection.
        /// </summary>
        public Int32 TextSelectionLength
        {
            get { return textSelectionLength; }
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
        /// Occurs when the value of the <see cref="Text"/> property changes.
        /// </summary>
        public event UIElementEventHandler TextChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler SelectionColorChanged;

        /// <summary>
        /// Identifies the Text dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(TextBox),
            new DependencyPropertyMetadata(HandleTextChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the SelectionColor dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectionColorProperty = DependencyProperty.Register("SelectionColor", typeof(Color), typeof(TextBox),
            new DependencyPropertyMetadata(HandleSelectionColorChanged, () => Color.Blue * 0.4f, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected internal override void OnKeyPressed(KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat)
        {
            switch (key)
            {
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
        protected internal override void OnMouseButtonPressed(MouseDevice device, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                View.Focus(this);
            }
            base.OnMouseButtonPressed(device, button);
        }

        /// <inheritdoc/>
        protected internal override Boolean CanGainFocus
        {
            get { return true; }
        }
        
        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch)
        {
            DrawBackgroundImage(spriteBatch);
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

            if (IsTextSelected)
            {
                var selectionStartOffset = CalculateOffset(SelectionStart);
                var selectionEndOffset   = CalculateOffset(SelectionEnd);
                var selectionWidth       = selectionEndOffset - selectionStartOffset;
                var selectionArea        = new RectangleF(
                    textArea.X + textScrollOffset + selectionStartOffset, 
                    textArea.Y, selectionWidth, Font.Regular.LineSpacing);

                spriteBatch.Draw(UIElementResources.BlankTexture, selectionArea, SelectionColor);
            }

            if (!String.IsNullOrEmpty(Text))
            {
                spriteBatch.DrawString(Font.Regular, Text, 
                    new Vector2(textArea.X + textScrollOffset, textArea.Y), FontColor);
            }

            if (IsCaretDisplayed && IsCaretVisible && !IsTextSelected)
            {
                var caretOffset   = CalculateCaretOffset();
                var caretPosition = new RectangleF(textArea.X + textScrollOffset + caretOffset, textArea.Y, 1, Font.Regular.LineSpacing);
                spriteBatch.Draw(UIElementResources.BlankTexture, caretPosition, Color.Blue);
            }

            spriteBatch.Flush();
            Ultraviolet.GetGraphics().SetScissorRectangle(scissorCurrent);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Text"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleTextChanged(DependencyObject dobj)
        {
            var textbox = (TextBox)dobj;
            textbox.OnTextChanged();
            textbox.textPosition = Math.Min(textbox.textPosition, (textbox.Text == null) ? 0 : textbox.Text.Length);
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
        /// Processes a text insertion event.
        /// </summary>
        /// <param name="text">The text to insert.</param>
        private void ProcessInsertText(String text)
        {
            DeleteSelection();

            if (Text == null)
            {
                Text         = text;
                textPosition = Text.Length;
            }
            else
            {
                Text         = Text.Insert(textPosition, text);
                textPosition = textPosition + text.Length;
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
            var width  = GetRelativeTextArea().Width;
            var offset = CalculateCaretOffset();
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
            var width  = GetRelativeTextArea().Width;
            var offset = CalculateCaretOffset();
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
        /// Deletes the currently selected text.
        /// </summary>
        private void DeleteSelection()
        {
            if (!IsTextSelected)
                return;

            Text = Text.Remove(SelectionStart, SelectionEnd - SelectionStart);

            textPosition        = SelectionStart;
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

            return new Rectangle(textX, textY, textAreaWidth, textAreaHeight);
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
        /// Calculates the offset of the specified character index.
        /// </summary>
        /// <param name="ix">The index of the character for which to calculate an offset.</param>
        /// <returns>The offset of the specified character in pixels.</returns>
        private Int32 CalculateOffset(Int32 ix)
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
            return CalculateOffset(textPosition);
        }

        /// <summary>
        /// Gets the index of the start of the selection.
        /// </summary>
        private Int32 SelectionStart
        {
            get { return Math.Min(textPosition + textSelectionLength, textPosition); }
        }

        /// <summary>
        /// Gets the index of the end of the selection.
        /// </summary>
        private Int32 SelectionEnd
        {
            get { return Math.Max(textPosition + textSelectionLength, textPosition); }
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
        private Int32 textPosition = 0;
        private Int32 textSelectionLength = 0;
        private Int32 textScrollOffset = 0;
        private Double caretBlinkTimer;
    }
}
