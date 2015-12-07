using System;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents the text editor in a <see cref="TextArea"/>.
    /// </summary>
    [UvmlKnownType]
    public partial class TextAreaEditor : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextAreaEditor"/> control.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TextAreaEditor(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Selects the specified range of text.
        /// </summary>
        /// <param name="start">The index of the first character to select.</param>
        /// <param name="length">The number of characters to select.</param>
        public void Select(Int32 start, Int32 length)
        {
            Contract.EnsureRange(start >= 0 && start <= textLayoutStream.TotalLength, "start");
            Contract.EnsureRange(length >= 0 && start + length <= textLayoutStream.TotalLength, "length");

            selectionPosition = start;
            caretPosition = start + length;

            if (selectionPosition == caretPosition)
                selectionPosition = null;

            UpdateSelectionAndCaret();
        }

        /// <summary>
        /// Selects the entirety of the editor's text.
        /// </summary>
        public void SelectAll()
        {
            Select(0, textLayoutStream.TotalLength);
        }

        /// <summary>
        /// Selects the word, whitespace, or symbol at the current caret position.
        /// </summary>
        public void SelectCurrentToken()
        {
            if (bufferText.Length == 0)
                return;

            var selectedPos = Math.Min(bufferText.Length - 1, caretPosition);
            var selectedChar = bufferText[selectedPos];

            var substrStart = 0;
            var substrEnd = 0;

            var predicate = default(Func<Char, Boolean>);

            if (Char.IsPunctuation(selectedChar))
            {
                predicate = (c) => false;
            }
            else
            {
                if (Char.IsWhiteSpace(selectedChar))
                {
                    predicate = (c) => Char.IsWhiteSpace(c);
                }
                else
                {
                    predicate = (c) => !Char.IsWhiteSpace(c) && !Char.IsPunctuation(c);
                }
            }

            FindSubstringMatchingPredicate(selectedPos, out substrStart, out substrEnd, predicate);

            var newSelectionStart = substrStart;
            var newSelectionLength = substrEnd - substrStart;
            Select(newSelectionStart, newSelectionLength);
        }

        /// <summary>
        /// Copies the currently selected text onto the clipboard.
        /// </summary>
        public void Copy()
        {
            Ultraviolet.GetPlatform().Clipboard.Text = SelectedText;
        }

        /// <summary>
        /// Cuts the currently selected text onto the clipboard.
        /// </summary>
        public void Cut()
        {
            var text = SelectedText;
            DeleteSelection();

            Ultraviolet.GetPlatform().Clipboard.Text = text;
        }

        /// <summary>
        /// Pastes the contents of the clipboard at the current caret position.
        /// </summary>
        public void Paste()
        {
            var text = Ultraviolet.GetPlatform().Clipboard.Text;

            if (String.IsNullOrEmpty(text))
                return;

            InsertTextAtCaret(text);
        }

        /// <summary>
        /// Gets or sets the starting point of the selected text.
        /// </summary>
        public Int32 SelectionStart
        {
            get { return Math.Min(selectionPosition ?? caretPosition, caretPosition); }
            set
            {
                Contract.EnsureRange(value >= 0, "value");

                var boundedStart = Math.Min(value, textLayoutStream.TotalLength);
                var boundedLength = Math.Min(SelectionLength, textLayoutStream.TotalLength - value);
                Select(boundedStart, boundedLength);
            }
        }

        /// <summary>
        /// Gets or sets the length of the selected text.
        /// </summary>
        public Int32 SelectionLength
        {
            get
            {
                if (selectionPosition == null)
                    return 0;
                
                return Math.Abs(selectionPosition.Value - caretPosition);
            }
            set
            {
                Contract.EnsureRange(value >= 0, "value");

                var boundedStart = SelectionStart;
                var boundedLength = Math.Min(SelectionLength, textLayoutStream.TotalLength - boundedStart);
                Select(boundedStart, boundedLength);
            }
        }

        /// <summary>
        /// Gets or sets the currently selected text.
        /// </summary>
        public String SelectedText
        {
            get
            {
                var length = SelectionLength;
                if (length == 0)
                    return String.Empty;

                return bufferText.ToString(SelectionStart, length);
            }
            set
            {
                DeleteSelection();

                var caretPositionOld = caretPosition;
                InsertTextAtCaret(value);
                var caretPositionNew = caretPosition;

                var selectionStart = caretPositionOld;
                var selectionLength = caretPositionNew - caretPositionOld;

                Select(selectionStart, selectionLength);
            }
        }
        
        /// <summary>
        /// Called when the editor should process a mouse button being pressed.
        /// </summary>
        /// <param name="device">The <see cref="MouseDevice"/> that raised the event.</param>
        /// <param name="button">The <see cref="MouseButton"/> value that corresponds to the button that was pressed.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void HandleMouseDown(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (textLayoutStream.Count == 0)
                return;

            if (button == MouseButton.Left)
            {
                MoveCaretToMouse();

                selectionPosition = caretPosition;
                selectionFollowingMouse = true;

                UpdateSelectionAndCaret();
            }
        }

        /// <summary>
        /// Called when the editor should process a mouse button being released.
        /// </summary>
        /// <param name="device">The <see cref="MouseDevice"/> that raised the event.</param>
        /// <param name="button">The <see cref="MouseButton"/> value that corresponds to the button that was released.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void HandleMouseUp(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (textLayoutStream.Count == 0)
                return;

            if (button == MouseButton.Left)
            {
                FinishSelection();
            }
        }

        /// <summary>
        /// Called when the editor should handle the mouse being double-clicked.
        /// </summary>
        /// <param name="device">The <see cref="MouseDevice"/> that raised the event.</param>
        /// <param name="button">The <see cref="MouseButton"/> value that corresponds to the button that was clicked.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void HandleMouseDoubleClick(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                SelectCurrentToken();
            }
        }

        /// <summary>
        /// Called when the editor should process the mouse being moved.
        /// </summary>
        /// <param name="device">The <see cref="MouseDevice"/> that raised the event.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void HandleMouseMove(MouseDevice device, ref RoutedEventData data)
        {
            if (selectionFollowingMouse)
            {
                MoveCaretToMouse();
            }
        }

        /// <summary>
        /// Called when the editor should process mouse capture being lost.
        /// </summary>
        internal void HandleLostMouseCapture()
        {
            FinishSelection();
        }

        /// <summary>
        /// Called when the editor should handle a <see cref="Keyboard.GotKeyboardFocusEvent"/> routed event.
        /// </summary>
        internal void HandleGotKeyboardFocus()
        {
            caretBlinkTimer = 0;
            UpdateCaret();
        }

        /// <summary>
        /// Called when the editor should handle a <see cref="Keyboard.LostKeyboardFocusEvent"/> routed event.
        /// </summary>
        internal void HandleLostKeyboardFocus()
        {

        }

        /// <summary>
        /// Called when the editor should handle a <see cref="Keyboard.KeyDownEvent"/> routed event.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="modifiers">A <see cref="ModifierKeys"/> value indicating which of the key modifiers are currently active.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        internal void HandleKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            var owner = TemplatedParent as TextArea;
            var ctrl = (modifiers & ModifierKeys.Control) == ModifierKeys.Control;

            switch (key)
            {
                case Key.A:
                    if (ctrl)
                    {
                        SelectAll();
                        data.Handled = true;
                    }
                    break;

                case Key.C:
                    if (ctrl)
                    {
                        Copy();
                        data.Handled = true;
                    }
                    break;

                case Key.X:
                    if (ctrl)
                    {
                        Cut();
                        data.Handled = true;
                    }
                    break;

                case Key.V:
                    if (ctrl)
                    {
                        Paste();
                        data.Handled = true;
                    }
                    break;

                case Key.Return:
                case Key.Return2:
                    if (owner != null && owner.AcceptsReturn)
                    {
                        InsertTextAtCaret("\n");
                        data.Handled = true;
                    }
                    break;

                case Key.Tab:
                    if (owner != null && owner.AcceptsTab)
                    {
                        InsertTextAtCaret("\t");
                        data.Handled = true;
                    }
                    break;

                case Key.Backspace:
                    DeleteBehind();
                    break;

                case Key.Delete:
                    DeleteAhead();
                    break;

                case Key.Left:
                    MoveCaretInDirection(CaretNavigationDirection.Left);
                    data.Handled = true;
                    break;

                case Key.Right:
                    MoveCaretInDirection(CaretNavigationDirection.Right);
                    data.Handled = true;
                    break;

                case Key.Up:
                    MoveCaretInDirection(CaretNavigationDirection.Up);
                    data.Handled = true;
                    break;

                case Key.Down:
                    MoveCaretInDirection(CaretNavigationDirection.Down);
                    data.Handled = true;
                    break;

                case Key.Home:
                    MoveCaretInDirection(CaretNavigationDirection.Home);
                    data.Handled = true;
                    break;

                case Key.End:
                    MoveCaretInDirection(CaretNavigationDirection.End);
                    data.Handled = true;
                    break;
            }
        }
        
        /// <summary>
        /// Called when the editor should read text input from the keyboard device.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void HandleTextInput(KeyboardDevice device, ref RoutedEventData data)
        {
            device.GetTextInput(bufferInput);

            InsertTextAtCaret((StringSegment)bufferInput);

            data.Handled = true;
        }
                
        /// <inheritdoc/>
        protected override void ReloadContentCore(Boolean recursive)
        {
            if (textLayoutStream.Count > 0)
                UpdateTextLayoutStream(MostRecentAvailableSize);

            UpdateCaret();
            UpdateSelection();

            base.ReloadContentCore(recursive);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            UpdateTextLayoutStream(availableSize);

            var width = Display.PixelsToDips(textLayoutStream.ActualWidth);
            width = Math.Max(width, Display.PixelsToDips(caretWidth));

            var height = Display.PixelsToDips(textLayoutStream.ActualHeight);
            height = Math.Max(height, Display.PixelsToDips(caretHeight));

            return new Size2D(width, height);
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            return finalSize;
        }
        
        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            DrawSelection(time, dc);
            DrawText(time, dc);
            DrawCaret(time, dc);

            base.DrawOverride(time, dc);
        }

        /// <summary>
        /// Searches the editor's text buffer for the substring at the current edit position which matches the specified predicate.
        /// </summary>
        private void FindSubstringMatchingPredicate(Int32 position, out Int32 substrStart, out Int32 substrEnd, Func<Char, Boolean> predicate)
        {
            var posClampedLeft = (position - 1);
            var posClampedRight = Math.Min(bufferText.Length, position + 1);

            // Find start of substring
            substrStart = position;
            for (int i = posClampedLeft; i >= 0; i--)
            {
                if (!predicate(bufferText[i]))
                    break;

                substrStart--;
            }

            // Find end of substring
            substrEnd = posClampedRight;
            for (int i = posClampedRight; i < bufferText.Length; i++)
            {
                if (!predicate(bufferText[i]))
                    break;

                substrEnd++;
            }
        }

        /// <summary>
        /// Updates the text parser stream.
        /// </summary>
        private void UpdateTextParserStream()
        {
            textParserStream.Clear();

            if (View == null)
                return;

            View.Resources.TextRenderer.Parse(bufferText, textParserStream, TextParserOptions.IgnoreCommandCodes);
            InvalidateMeasure();
        }

        /// <summary>
        /// Updates the text layout stream.
        /// </summary>
        private void UpdateTextLayoutStream(Size2D availableSize)
        {
            textLayoutStream.Clear();

            if (View == null)
                return;

            var owner = TemplatedParent as TextArea;
            if (owner == null)
                return;

            if (textParserStream.Count == 0)
            {
                UpdateCaret();
                UpdateSelection();
                return;
            }

            var wrap = (owner.TextWrapping == TextWrapping.Wrap);
            var width = wrap ? (Int32?)Display.DipsToPixels(availableSize.Width) : null;

            var settings = new TextLayoutSettings(owner.Font, width, null, TextFlags.Standard);
            View.Resources.TextRenderer.CalculateLayout(textParserStream, textLayoutStream, settings);

            UpdateSelectionAndCaret();
        }

        /// <summary>
        /// Draws the current selection.
        /// </summary>
        private void DrawSelection(UltravioletTime time, DrawingContext dc)
        {
            if (!selectionPosition.HasValue || SelectionLength == 0)
                return;

            // Draw the first line
            var selectionTopDips = Display.PixelsToDips(selectionTop);
            DrawBlank(dc, selectionTopDips, Color.CornflowerBlue);

            // Draw the middle
            if (selectionLineCount > 2)
            {
                textLayoutStream.AcquirePointers();

                var lineInfo = textLayoutStream.GetLineInfo(selectionLineStart);

                for (int i = selectionLineStart + 1; i < selectionLineStart + selectionLineCount - 1; i++)
                {
                    textLayoutStream.GetNextLineInfoRef(ref lineInfo, out lineInfo);

                    var lineBounds = new Ultraviolet.Rectangle(lineInfo.X, lineInfo.Y, lineInfo.Width, lineInfo.Height);
                    var lineBoundsDips = Display.PixelsToDips(lineBounds);
                    DrawBlank(dc, lineBoundsDips, Color.CornflowerBlue);
                }

                textLayoutStream.ReleasePointers();
            }

            // Draw the last line
            if (selectionLineCount > 1)
            {
                var selectionBottomDips = Display.PixelsToDips(selectionBottom);
                DrawBlank(dc, selectionBottomDips, Color.CornflowerBlue);
            }
        }

        /// <summary>
        /// Draws the editor's text.
        /// </summary>
        private void DrawText(UltravioletTime time, DrawingContext dc)
        {
            if (textLayoutStream.Count == 0)
                return;

            var owner = TemplatedParent as TextArea;
            if (owner == null)
                return;
            
            var position = Display.DipsToPixels(UntransformedAbsolutePosition);
            var positionRounded = dc.IsTransformed ? (Vector2)position : (Vector2)(Point2)position;
            View.Resources.TextRenderer.Draw((SpriteBatch)dc, textLayoutStream, positionRounded, owner.Foreground * dc.Opacity);
        }

        /// <summary>
        /// Draws the editor's caret.
        /// </summary>
        private void DrawCaret(UltravioletTime time, DrawingContext dc)
        {
            var owner = TemplatedParent as TextArea;
            if (owner == null || !owner.IsKeyboardFocusWithin || selectionFollowingMouse)
                return;

            caretBlinkTimer = (caretBlinkTimer + time.ElapsedTime.TotalMilliseconds) % 1000.0;

            var isCaretVisible = ((Int32)(caretBlinkTimer / 500.0) % 2 == 0);
            if (isCaretVisible)
            {
                var caretXDips = Display.PixelsToDips(caretX);
                var caretYDips = Display.PixelsToDips(caretY);
                var caretWidthDips = Display.PixelsToDips(caretWidth);
                var caretHeightDips = Display.PixelsToDips(caretHeight);
                var caretBounds = new RectangleD(caretXDips, caretYDips, caretWidthDips, caretHeightDips);
                DrawBlank(dc, caretBounds, owner.Foreground);
            }
        }

        /// <summary>
        /// Moves the caret to the current mouse position.
        /// </summary>
        private void MoveCaretToMouse()
        {
            var mousePosDips = Mouse.GetPosition(this);
            var mousePosPixs = (Point2)Display.DipsToPixels(mousePosDips);

            caretBlinkTimer = 0;
            caretPosition = View.Resources.TextRenderer.GetInsertionPointAtPosition(textLayoutStream, mousePosPixs);

            UpdateSelectionAndCaret();
        }

        /// <summary>
        /// Moves the caret to the left.
        /// </summary>
        private void MoveCaretLeft()
        {
            var movementAllowed = (caretPosition > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.Left))
            {
                caretBlinkTimer = 0;
                caretPosition -= 1;
                UpdateSelectionAndCaret();
            }
        }

        /// <summary>
        /// Moves the caret to the right.
        /// </summary>
        private void MoveCaretRight()
        {
            var movementAllowed = (caretPosition < textLayoutStream.TotalLength);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.Right))
            {
                caretBlinkTimer = 0;
                caretPosition += 1;
                UpdateSelectionAndCaret();
            }
        }

        /// <summary>
        /// Moves the caret up.
        /// </summary>
        private void MoveCaretUp()
        {
            var x = caretX;
            var y = caretY - 1;

            if (y < 0)
                return;

            var movementAllowed = (textLayoutStream.Count > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.Up))
            {
                caretBlinkTimer = 0;
                caretPosition = View.Resources.TextRenderer.GetInsertionPointAtPosition(textLayoutStream, x, y);
                UpdateSelectionAndCaret();
            }
        }

        /// <summary>
        /// Moves the caret down.
        /// </summary>
        private void MoveCaretDown()
        {
            var x = caretX;
            var y = caretY + caretHeight;

            if (y >= textLayoutStream.ActualHeight)
                return;

            var movementAllowed = (textLayoutStream.Count > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.Down))
            {
                caretBlinkTimer = 0;
                caretPosition = View.Resources.TextRenderer.GetInsertionPointAtPosition(textLayoutStream, x, y);
                UpdateSelectionAndCaret();
            }
        }

        /// <summary>
        /// Moves the caret to the beginning of the text.
        /// </summary>
        private void MoveCaretToHome()
        {
            var movementAllowed = (caretPosition > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.Home))
            {
                caretBlinkTimer = 0;
                caretPosition = 0;

                UpdateSelectionAndCaret();
            }
        }

        /// <summary>
        /// Moves the caret to the end of the text.
        /// </summary>
        private void MoveCaretToEnd()
        {
            var movementAllowed = (caretPosition < textLayoutStream.TotalLength);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.End))
            {
                caretBlinkTimer = 0;
                caretPosition = textLayoutStream.TotalLength;

                UpdateSelectionAndCaret();
            }
        }

        /// <summary>
        /// Moves the caret in the specified direction.
        /// </summary>
        /// <param name="direction">A <see cref="CaretNavigationDirection"/> value that specifies how to move the caret.</param>
        private void MoveCaretInDirection(CaretNavigationDirection direction)
        {
            switch (direction)
            {
                case CaretNavigationDirection.Left:
                    MoveCaretLeft();
                    break;

                case CaretNavigationDirection.Right:
                    MoveCaretRight();
                    break;

                case CaretNavigationDirection.Up:
                    MoveCaretUp();
                    break;

                case CaretNavigationDirection.Down:
                    MoveCaretDown();
                    break;

                case CaretNavigationDirection.Home:
                    MoveCaretToHome();
                    break;

                case CaretNavigationDirection.End:
                    MoveCaretToEnd();
                    break;
            }
        }
        
        /// <summary>
        /// Deletes the text in the current selection.
        /// </summary>
        /// <returns><c>true</c> if the selection was deleted; otherwise, <c>false</c>.</returns>
        private Boolean DeleteSelection()
        {
            var length = SelectionLength;
            if (length == 0)
                return false;

            var start = SelectionStart;

            selectionPosition = null;

            if (caretPosition > start)
                caretPosition = start;

            bufferText.Remove(start, length);

            UpdateTextParserStream();

            return true;
        }

        /// <summary>
        /// Deletes the text ahead of the caret.
        /// </summary>
        private Boolean DeleteAhead()
        {
            if (!DeleteSelection() && caretPosition < textLayoutStream.TotalLength)
            {
                bufferText.Remove(caretPosition, 1);
                caretBlinkTimer = 0;

                UpdateTextParserStream();

                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes the text behind the caret.
        /// </summary>
        private Boolean DeleteBehind()
        {
            if (!DeleteSelection() && caretPosition > 0)
            {
                bufferText.Remove(caretPosition - 1, 1);
                caretBlinkTimer = 0;
                caretPosition -= 1;

                UpdateTextParserStream();

                return true;
            }
            return false;
        }

        /// <summary>
        /// Handles movement events which affect the selection.
        /// </summary>
        /// <param name="movementAllowed">A value indicating whether the user's desired movement is allowed.</param>
        /// <param name="movementDirection">A <see cref="CaretNavigationDirection"/> value specifying which direction the selection is moving.</param>
        /// <returns><c>true</c> if the movement was handled; otherwise, <c>false</c>.</returns>
        private Boolean HandleSelectionMovement(Boolean movementAllowed, CaretNavigationDirection movementDirection)
        {
            var selecting = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
            if (selecting)
            {
                if (selectionPosition == null)
                    selectionPosition = caretPosition;

                return !movementAllowed;
            }
            else
            {
                if (selectionPosition == null)
                    return !movementAllowed;

                var newCaretPosition =
                    (movementDirection == CaretNavigationDirection.Left) ? SelectionStart :
                    (movementDirection == CaretNavigationDirection.Right) ? SelectionStart + SelectionLength : (Int32?)null;

                selectionPosition = null;

                if (newCaretPosition.HasValue)
                {
                    caretBlinkTimer = 0;
                    caretPosition = newCaretPosition.Value;

                    UpdateCaret();
                    UpdateSelection();

                    return true;
                }

                return !movementAllowed;
            }
        }

        /// <summary>
        /// Ends updating the selection via mouse motion.
        /// </summary>
        private void FinishSelection()
        {
            if (caretPosition == selectionPosition)
                selectionPosition = null;

            selectionFollowingMouse = false;

            UpdateCaret();
            UpdateSelection();
        }

        /// <summary>
        /// Inserts text at the specified position in the text buffer.
        /// </summary>
        private void InsertTextAtPosition(StringSegment str, Int32 position)
        {
            var selectionStart = SelectionStart;
            var selectionLength = SelectionLength;

            if (DeleteSelection() && position > selectionStart)
                position -= selectionLength;

            for (int i = 0; i < str.Length; i++)
                bufferText.Insert(position + i, str[i]);

            caretBlinkTimer = 0;
            caretPosition = (position <= caretPosition) ? caretPosition + str.Length : caretPosition;

            UpdateTextParserStream();
        }

        /// <summary>
        /// Inserts text at the current caret position.
        /// </summary>
        private void InsertTextAtCaret(StringSegment str)
        {
            InsertTextAtPosition(str, caretPosition);
        }

        /// <summary>
        /// Calculates the position at which to draw the text caret.
        /// </summary>
        private void UpdateCaret()
        {
            if (caretPosition > 0 && caretPosition <= textLayoutStream.TotalLength)
            {
                var caretBounds = View.Resources.TextRenderer.GetInsertionPointBounds(textLayoutStream, caretPosition);
                caretX = caretBounds.X;
                caretY = caretBounds.Y;
                caretWidth = 1;
                caretHeight = caretBounds.Height;
            }
            else
            {
                var owner = TemplatedParent as TextArea;
                caretX = 0;
                caretY = 0;
                caretWidth = 1;
                caretHeight = (owner == null && owner.Font.IsLoaded) ? 0 : owner.Font.Resource.Value.GetFace(SpriteFontStyle.Regular).LineSpacing;
            }
        }
        
        /// <summary>
        /// Calculates the parameters with which to draw the selection.
        /// </summary>
        private void UpdateSelection()
        {
            if (View == null || textLayoutStream.Count == 0 || !selectionPosition.HasValue)
                return;

            if (SelectionLength == 0)
            {
                selectionLineStart = 0;
                selectionLineCount = 0;

                selectionTop = default(Ultraviolet.Rectangle);
                selectionBottom = default(Ultraviolet.Rectangle);
            }
            else
            {
                var selectionStart = SelectionStart;
                var selectionStartLineIndex = 0;
                var selectionStartLineWidth = 0;
                var selectionStartLineHeight = 0;
                var selectionStartGlyphBounds = View.Resources.TextRenderer.GetGlyphBounds(textLayoutStream, selectionStart,
                    out selectionStartLineIndex, out selectionStartLineWidth, out selectionStartLineHeight, true);

                var selectionEnd = SelectionStart + (SelectionLength - 1);
                var selectionEndLineIndex = 0;
                var selectionEndLineWidth = 0;
                var selectionEndLineHeight = 0;
                var selectionEndGlyphBounds = View.Resources.TextRenderer.GetGlyphBounds(textLayoutStream, selectionEnd,
                    out selectionEndLineIndex, out selectionEndLineWidth, out selectionEndLineHeight, true);

                selectionLineStart = selectionStartLineIndex;
                selectionLineCount = 1 + (selectionEndLineIndex - selectionStartLineIndex);

                // Top
                var selectionTopX = selectionStartGlyphBounds.X;
                var selectionTopY = selectionStartGlyphBounds.Y;
                var selectionTopWidth = (selectionStartLineIndex == selectionEndLineIndex) ?
                    selectionEndGlyphBounds.Right - selectionStartGlyphBounds.Left :
                    selectionStartLineWidth - selectionStartGlyphBounds.X;
                var selectionTopHeight = selectionStartLineHeight;
                selectionTop = new Ultraviolet.Rectangle(selectionTopX, selectionTopY, selectionTopWidth, selectionTopHeight);

                // Bottom
                if (selectionLineCount > 1)
                {
                    var selectionBottomX = 0;
                    var selectionBottomY = selectionEndGlyphBounds.Y;
                    var selectionBottomWidth = selectionEndGlyphBounds.Right;
                    var selectionBottomHeight = selectionEndLineHeight;
                    selectionBottom = new Ultraviolet.Rectangle(selectionBottomX, selectionBottomY, selectionBottomWidth, selectionBottomHeight);
                }
            }            
        }

        /// <summary>
        /// Calculates the position at which to draw the text caret, and calculates the parameters with which to draw the selection.
        /// </summary>
        private void UpdateSelectionAndCaret()
        {
            UpdateSelection();
            UpdateCaret();
        }

        // State values.
        private readonly TextParserTokenStream textParserStream = new TextParserTokenStream();
        private readonly TextLayoutCommandStream textLayoutStream = new TextLayoutCommandStream();

        // Caret parameters.
        private Double caretBlinkTimer;
        private Int32 caretPosition;
        private Int32 caretX;
        private Int32 caretY;
        private Int32 caretWidth;
        private Int32 caretHeight;        

        // Selection parameters.
        private Int32? selectionPosition;
        private Int32 selectionLineStart;
        private Int32 selectionLineCount;
        private Ultraviolet.Rectangle selectionTop;
        private Ultraviolet.Rectangle selectionBottom;
        private Boolean selectionFollowingMouse;

        // The editor's internal text buffer.
        private readonly StringBuilder bufferInput = new StringBuilder();
        private readonly StringBuilder bufferText = new StringBuilder();
    }
}
