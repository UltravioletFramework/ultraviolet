using System;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Input;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the method that is called when a <see cref="TextEditor"/> is validating a character for entry.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="text">The editor's current text.</param>
    /// <param name="offset">The offset at which the character is being inserted into the text.</param>
    /// <param name="character">The character being inserted into the text.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfTextEntryValidationHandler(DependencyObject element, 
        StringSegment text, Int32 offset, Char character, TextEntryValidationRoutedEventData data);

    /// <summary>
    /// Represents the component of a <see cref="TextBox"/> which is responsible for performing text editing.
    /// </summary>
    [UvmlKnownType]
    public partial class TextEditor : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextEditor"/> control.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TextEditor(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Controls.Primitives.TextEditor.TextEntryValidation"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddTextEntryValidationHandler(DependencyObject element, UpfTextEntryValidationHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            UIElementHelper.AddHandler(element, TextEntryValidationEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Controls.Primitives.TextEditor.TextEntryValidation"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveTextEntryValidationHandler(DependencyObject element, UpfTextEntryValidationHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            UIElementHelper.RemoveHandler(element, TextEntryValidationEvent, handler);
        }

        /// <summary>
        /// Gets the selected text.
        /// </summary>
        /// <returns>A string containing the selected text.</returns>
        public String GetSelectedText()
        {
            var length = SelectionLength;
            if (length == 0)
                return String.Empty;

            return bufferText.ToString(SelectionStart, length);
        }

        /// <summary>
        /// Gets the selected text.
        /// </summary>
        /// <param name="stringBuilder">A <see cref="StringBuilder"/> to populate with the contents of the selection.</param>
        public void GetSelectedText(StringBuilder stringBuilder)
        {
            Contract.Require(stringBuilder, nameof(stringBuilder));

            stringBuilder.Length = 0;

            var length = SelectionLength;
            if (length == 0)
                return;

            var start = SelectionStart;
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(bufferText[start + i]);
            }
        }

        /// <summary>
        /// Sets the selected text.
        /// </summary>
        /// <param name="value">A <see cref="String"/> containing text to set.</param>
        public void SetSelectedText(String value)
        {
            SetSelectedText(new StringSegment(value));
        }

        /// <summary>
        /// Sets the selected text.
        /// </summary>
        /// <param name="value">A <see cref="StringBuilder"/> containing the text to set.</param>
        public void SetSelectedText(StringBuilder value)
        {
            SetSelectedText(new StringSegment(value));
        }

        /// <summary>
        /// Sets the selected text.
        /// </summary>
        /// <param name="value">A <see cref="StringSegment"/> containing the text to set.</param>
        public void SetSelectedText(StringSegment value)
        {
            BeginTrackingSelectionChanges();

            DeleteSelection(value.Length == 0);

            var caretPositionOld = caretPosition;
            InsertTextAtCaret(value, false);
            var caretPositionNew = caretPosition;

            var selectionStart = caretPositionOld;
            var selectionLength = caretPositionNew - caretPositionOld;

            Select(selectionStart, selectionLength);

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Gets the specified line of text.
        /// </summary>
        /// <param name="lineIndex">The index of the line of text to retrieve.</param>
        /// <returns>A string containing the contents of the specified line of text.</returns>
        public String GetLineText(Int32 lineIndex)
        {
            Contract.EnsureRange(lineIndex >= 0 && lineIndex < textLayoutStream.LineCount, nameof(lineIndex));

            var lineInfo = textLayoutStream.GetLineInfo(lineIndex);

            return bufferText.ToString(lineInfo.OffsetInSource, lineInfo.LengthInSource);
        }

        /// <summary>
        /// Gets the specified line of text.
        /// </summary>
        /// <param name="lineIndex">The index of the line of text to retrieve.</param>
        /// <param name="stringBuilder">A <see cref="StringBuilder"/> to populate with the contents of the specified line of text.</param>
        public void GetLineText(Int32 lineIndex, StringBuilder stringBuilder)
        {
            Contract.EnsureRange(lineIndex >= 0 && lineIndex < textLayoutStream.LineCount, nameof(lineIndex));
            Contract.Require(stringBuilder, nameof(stringBuilder));

            var lineInfo = textLayoutStream.GetLineInfo(lineIndex);

            stringBuilder.Length = 0;

            for (int i = 0; i < lineInfo.LengthInSource; i++)
                stringBuilder.Append(bufferText[lineInfo.OffsetInSource + i]);
        }

        /// <summary>
        /// Finds the extent of the word at the specified caret position.
        /// </summary>
        /// <param name="position">The caret position at which to begin searching for words.</param>
        /// <param name="wordStart">The start index of the word that was found.</param>
        /// <param name="wordEnd">The end index of the word that was found.</param>
        public void FindWord(Int32 position, out Int32 wordStart, out Int32 wordEnd)
        {
            wordStart = wordEnd = 0;
            
            if (bufferText.Length == 0)
                return;

            if (IsMasked)
            {
                wordStart = 0;
                wordEnd = bufferText.Length;
            }
            else
            {
                // Find the beginning of the word
                var wordStartNeeded = true;
                for (int i = Math.Min(position, bufferText.Length) - 1; i > 0; i--)
                {
                    var charPrev = bufferText[i - 1];
                    var charNext = bufferText[i];
                    if (IsEndOfWordBoundary(charPrev, charNext))
                    {
                        wordStart = i;
                        wordStartNeeded = false;
                        break;
                    }
                }

                if (wordStartNeeded)
                    wordStart = 0;

                // Find the end of the word
                var wordEndNeeded = true;
                for (int i = Math.Max(position, 0) + 1; i < bufferText.Length; i++)
                {
                    var charPrev = bufferText[i - 1];
                    var charNext = bufferText[i];
                    if (IsEndOfWordBoundary(charPrev, charNext))
                    {
                        wordEnd = i;
                        wordEndNeeded = false;
                        break;
                    }
                }

                if (wordEndNeeded)
                    wordEnd = bufferText.Length;
            }
        }

        /// <summary>
        /// Selects the specified range of text.
        /// </summary>
        /// <param name="start">The index of the first character to select.</param>
        /// <param name="length">The number of characters to select.</param>
        public void Select(Int32 start, Int32 length)
        {
            Contract.EnsureRange(start >= 0 && start <= textLayoutStream.TotalSourceLength, nameof(start));
            Contract.EnsureRange(length >= 0 && start + length <= textLayoutStream.TotalSourceLength, nameof(length));

            BeginTrackingSelectionChanges();
            
            selectionPosition = AdjustCaretToAvoidInvalidPositions(start, false);
            caretPosition = AdjustCaretToAvoidInvalidPositions(start + length, true);

            if (selectionPosition == caretPosition)
                selectionPosition = null;

            UpdateSelectionAndCaret();
            ScrollToCaret(true, false, false);

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Selects the entirety of the editor's text.
        /// </summary>
        public void SelectAll()
        {
            Select(0, textLayoutStream.TotalSourceLength);
        }
        
        /// <summary>
        /// Selects the word, whitespace, or symbol at the current caret position.
        /// </summary>
        public void SelectCurrentToken()
        {
            if (bufferText.Length == 0)
                return;

            if (MaskCharacter.HasValue)
            {
                SelectAll();
                return;
            }

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
        /// Appends the specified string to the end of the text box's content.
        /// </summary>
        /// <param name="textData">The text to append to the end of the text box's content.</param>
        public void AppendText(String textData)
        {
            if (textData == null)
                return;

            BeginTrackingSelectionChanges();

            InsertIntoBuffer(textData, bufferText.Length);
            UpdateTextStringSource();
            UpdateTextParserStream();

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Copies the currently selected text onto the clipboard.
        /// </summary>
        public void Copy()
        {
            Ultraviolet.GetPlatform().Clipboard.Text = GetSelectedText();
        }

        /// <summary>
        /// Cuts the currently selected text onto the clipboard.
        /// </summary>
        public void Cut()
        {
            BeginTrackingSelectionChanges();

            Ultraviolet.GetPlatform().Clipboard.Text = GetSelectedText();
            DeleteSelection(true);

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Pastes the contents of the clipboard at the current caret position.
        /// </summary>
        public void Paste()
        {
            BeginTrackingSelectionChanges();

            var text = Ultraviolet.GetPlatform().Clipboard.Text;
            if (!String.IsNullOrEmpty(text))
                InsertTextAtCaret(text, false);

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Inserts a new line.
        /// </summary>
        public void InsertNewLine()
        {
            if (IsReadOnly)
                return;

            InsertTextAtCaret(Environment.NewLine, false);
        }

        /// <summary>
        /// Inserts a tab character.
        /// </summary>
        public void InsertTab()
        {
            if (IsReadOnly)
                return;

            InsertTextAtCaret("\t", true);
        }

        /// <summary>
        /// Deletes the character ahead of the caret.
        /// </summary>
        public void Delete()
        {
            if (IsReadOnly || !IsEnabled)
                return;

            DeleteAhead();
        }

        /// <summary>
        /// Deletes everything from the current caret position to the beginning of the next word.
        /// </summary>
        public void DeleteNextWord()
        {
            if (IsReadOnly || !IsEnabled)
                return;

            var wordStart = 0;
            var wordEnd = 0;
            FindWord(caretPosition, out wordStart, out wordEnd);

            var wordLength = (wordEnd - caretPosition);
            if (wordLength > 0)
                DeleteSpan(caretPosition, wordLength, true);
        }

        /// <summary>
        /// Deletes everything from the current caret position to the end of the previous word.
        /// </summary>
        public void DeletePreviousWord()
        {
            if (IsReadOnly || !IsEnabled)
                return;

            var wordStart = 0;
            var wordEnd = 0;
            FindWord(caretPosition, out wordStart, out wordEnd);

            var wordLength = (caretPosition - wordStart);
            if (wordLength > 0)
            {
                DeleteSpan(wordStart, wordLength, true);

                UpdateSelectionAndCaret();
                ScrollToCaret(false, true, false);
            }
        }

        /// <summary>
        /// Deletes the character behind the caret.
        /// </summary>
        public void Backspace()
        {
            if (IsReadOnly)
                return;

            DeleteBehind();

            UpdateSelectionAndCaret();
            ScrollToCaret(false, true, false);
        }

        /// <summary>
        /// Clears the text editor's text.
        /// </summary>
        public void Clear()
        {
            BeginTrackingSelectionChanges();

            caretPosition = 0;
            caretBlinkTimer = 0;
            selectionPosition = null;
            UpdateSelectionAndCaret();

            ClearBuffer(true);

            UpdateTextStringSource();
            UpdateTextParserStream();

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Scrolls the text editor so that the specified line is in full view.
        /// </summary>
        /// <param name="lineIndex">The index of the line to scroll into view.</param>
        public void ScrollToLine(Int32 lineIndex)
        {
            Contract.EnsureRange(lineIndex >= 0 && lineIndex < textLayoutStream.LineCount, nameof(lineIndex));

            var scrollViewer = Parent as ScrollViewer;
            if (scrollViewer == null)
                return;

            var lineInfo = textLayoutStream.GetLineInfo(lineIndex);

            var boundsViewport = new RectangleD(scrollViewer.ContentHorizontalOffset, scrollViewer.ContentVerticalOffset,
                scrollViewer.ViewportWidth, scrollViewer.ViewportHeight);
            var boundsLine = Display.PixelsToDips(new RectangleD(lineInfo.X, lineInfo.Y, lineInfo.Width, lineInfo.Height));

            if (boundsViewport.Contains(boundsLine))
                return;

            scrollViewer.ScrollToHorizontalOffset(lineInfo.X);
            scrollViewer.ScrollToVerticalOffset(lineInfo.Y);
        }

        /// <summary>
        /// Moves the caret to the specified cursor position.
        /// </summary>
        public void MoveCaretToCursor(Point2D position)
        {
            BeginTrackingSelectionChanges();

            var positionPixs = (Point2)Display.DipsToPixels(position) - new Point2(textOffsetX, textOffsetY);

            caretBlinkTimer = 0;
            caretPosition = View.Resources.TextRenderer.GetInsertionPointAtPosition(textLayoutStream, positionPixs);

            UpdateSelectionAndCaret();
            ScrollToCaret(true, false, false);

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Moves the caret to the left.
        /// </summary>
        /// <param name="moveSelection">A value indicating whether or not the selection is moving.</param>
        public void MoveCaretLeft(Boolean moveSelection)
        {
            BeginTrackingSelectionChanges();

            var movementAllowed = (caretPosition > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.Left, moveSelection))
            {
                caretBlinkTimer = 0;
                caretPosition = AdjustCaretToAvoidInvalidPositions(caretPosition - 1, false);

                UpdateSelectionAndCaret();
            }
            ScrollToCaret(false, true, false);

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Moves the caret to the right.
        /// </summary>
        /// <param name="moveSelection">A value indicating whether or not the selection is moving.</param>
        public void MoveCaretRight(Boolean moveSelection)
        {
            BeginTrackingSelectionChanges();

            var movementAllowed = (caretPosition < textLayoutStream.TotalSourceLength);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.Right, moveSelection))
            {
                caretBlinkTimer = 0;
                caretPosition = AdjustCaretToAvoidInvalidPositions(caretPosition + 1, true);

                UpdateSelectionAndCaret();
            }
            ScrollToCaret(false, false, true);

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Moves the caret up.
        /// </summary>
        /// <param name="moveSelection">A value indicating whether or not the selection is moving.</param>
        public void MoveCaretUp(Boolean moveSelection)
        {
            BeginTrackingSelectionChanges();

            var x = caretBounds.Left;
            var y = caretBounds.Top - 1;

            var movementAllowed = (textLayoutStream.Count > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.Up, moveSelection))
            {
                caretBlinkTimer = 0;
                caretPosition = View.Resources.TextRenderer.GetInsertionPointAtPosition(textLayoutStream, x, y);

                UpdateSelectionAndCaret();
                ScrollToCaret(true, false, false);
            }

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Moves the caret down.
        /// </summary>
        /// <param name="moveSelection">A value indicating whether or not the selection is moving.</param>
        public void MoveCaretDown(Boolean moveSelection)
        {
            BeginTrackingSelectionChanges();

            var x = caretBounds.Left;
            var y = caretBounds.Bottom;

            var movementAllowed = (textLayoutStream.Count > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.Down, moveSelection))
            {
                caretBlinkTimer = 0;
                caretPosition = View.Resources.TextRenderer.GetInsertionPointAtPosition(textLayoutStream, x, y);

                UpdateSelectionAndCaret();
                ScrollToCaret(true, false, false);
            }

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Moves the caret up one page.
        /// </summary>
        /// <param name="moveSelection">A value indicating whether or not the selection is moving.</param>
        public void MoveCaretPageUp(Boolean moveSelection)
        {
            var scrollViewer = Parent as ScrollViewer;
            if (scrollViewer == null)
                return;

            BeginTrackingSelectionChanges();

            var x = caretBounds.Left;
            var y = caretBounds.Top - (Int32)Display.DipsToPixels(scrollViewer.ViewportHeight);

            if (caretLineIndex > 0 && y < 0)
                y = 0;

            var movementAllowed = (textLayoutStream.Count > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.PageUp, moveSelection))
            {
                caretBlinkTimer = 0;
                caretPosition = View.Resources.TextRenderer.GetInsertionPointAtPosition(textLayoutStream, x, y);

                UpdateSelectionAndCaret();
                ScrollToCaret(true, false, false);
            }

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Moves the caret down one page.
        /// </summary>
        /// <param name="moveSelection">A value indicating whether or not the selection is moving.</param>
        public void MoveCaretPageDown(Boolean moveSelection)
        {
            var scrollViewer = Parent as ScrollViewer;
            if (scrollViewer == null)
                return;

            BeginTrackingSelectionChanges();

            var x = caretBounds.Left;
            var y = caretBounds.Top + (Int32)Display.DipsToPixels(scrollViewer.ViewportHeight);

            if (caretLineIndex < textLayoutStream.LineCount - 1 && y >= textLayoutStream.ActualHeight)
                y = textLayoutStream.ActualHeight - 1;

            var movementAllowed = (textLayoutStream.Count > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.PageDown, moveSelection))
            {
                caretBlinkTimer = 0;
                caretPosition = View.Resources.TextRenderer.GetInsertionPointAtPosition(textLayoutStream, x, y);

                UpdateSelectionAndCaret();
                ScrollToCaret(true, false, false);
            }

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Moves the caret to the beginning of the current line.
        /// </summary>
        /// <param name="moveSelection">A value indicating whether or not the selection is moving.</param>
        /// <param name="moveToBeginningOfText">A value indicating whether the caret should be moved to the beginning of the editor's text.</param>
        public void MoveCaretToHome(Boolean moveSelection, Boolean moveToBeginningOfText)
        {
            BeginTrackingSelectionChanges();

            var movementAllowed = (caretPosition > 0 && textLayoutStream.TotalSourceLength > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.Home, moveSelection))
            {
                caretBlinkTimer = 0;
                caretPosition = moveToBeginningOfText ? 0 : textLayoutStream.GetLineInfo(caretLineIndex).OffsetInSource;

                UpdateSelectionAndCaret();
                ScrollToCaret(true, false, false);
            }

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Moves the caret to the end of the current line.
        /// </summary>
        /// <param name="moveSelection">A value indicating whether or not the selection is moving.</param>
        /// <param name="moveToEndOfText">A value indicating whether the caret should be moved to the end of the editor's text.</param>
        public void MoveCaretToEnd(Boolean moveSelection, Boolean moveToEndOfText)
        {
            BeginTrackingSelectionChanges();

            var movementAllowed = (caretPosition < textLayoutStream.TotalSourceLength && textLayoutStream.TotalSourceLength > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.End, moveSelection))
            {
                caretBlinkTimer = 0;
                if (moveToEndOfText)
                {
                    caretPosition = textLayoutStream.TotalSourceLength;
                }
                else
                {
                    var lineInfo = textLayoutStream.GetLineInfo(caretLineIndex);
                    caretPosition = lineInfo.OffsetInSource + (lineInfo.LengthInSource - lineInfo.TerminatingLineBreakLength);

                    if (IsLineBreak(caretPosition - 1, false))
                        caretPosition = AdjustCaretToAvoidInvalidPositions(caretPosition - 1, false);
                }

                UpdateSelectionAndCaret();
                ScrollToCaret(true, false, false);
            }

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Moves the caret to the start of the next word.
        /// </summary>
        /// <param name="moveSelection">A value indicating whether or not the selection is moving.</param>
        public void MoveCaretToNextWord(Boolean moveSelection)
        {
            BeginTrackingSelectionChanges();

            var movementAllowed = (caretPosition < textLayoutStream.TotalSourceLength);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.NextWord, moveSelection))
            {
                var wordStart = 0;
                var wordEnd = 0;
                FindWord(caretPosition, out wordStart, out wordEnd);

                caretBlinkTimer = 0;
                caretPosition = wordEnd;

                UpdateSelectionAndCaret();
                ScrollToCaret(false, false, true);
            }
            
            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Moves the caret to the end of the previous word.
        /// </summary>
        /// <param name="moveSelection">A value indicating whether or not the selection is moving.</param>
        public void MoveCaretToPreviousWord(Boolean moveSelection)
        {
            BeginTrackingSelectionChanges();

            var movementAllowed = (caretPosition > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.PreviousWord, moveSelection))
            {
                var wordStart = 0;
                var wordEnd = 0;
                FindWord(caretPosition, out wordStart, out wordEnd);

                caretBlinkTimer = 0;
                caretPosition = wordStart;

                UpdateSelectionAndCaret();
                ScrollToCaret(false, false, true);
            }

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Toggles the editor's insertion mode.
        /// </summary>
        public void ToggleInsertionMode()
        {
            caretBlinkTimer = 0;
            caretInsertionMode = (caretInsertionMode == CaretMode.Insert) ?
                CaretMode.Overwrite : CaretMode.Insert;

            UpdateCaret();
        }

        /// <summary>
        /// Gets the index of the first character on the specified line.
        /// </summary>
        /// <param name="lineIndex">The index of the line for which to retrieve a character index.</param>
        /// <returns>The index of the first character on the specified line.</returns>
        public Int32 GetCharacterIndexFromLineIndex(Int32 lineIndex)
        {
            Contract.EnsureRange(lineIndex >= 0 && lineIndex < textLayoutStream.LineCount, nameof(lineIndex));

            if (View == null)
                return 0;

            var lineInfo = textLayoutStream.GetLineInfo(lineIndex);
            return lineInfo.OffsetInSource;
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
            if (View == null)
                return -1;

            var pointX = point.X;
            var pointY = point.Y;

            if (snapToText)
            {
                if (pointX < 0)
                    pointX = 0;

                if (pointX >= textLayoutStream.ActualWidth)
                    pointX = textLayoutStream.ActualWidth - 1;

                if (pointY < 0)
                    pointY = 0;

                if (pointY >= textLayoutStream.ActualHeight)
                    pointY = textLayoutStream.ActualHeight - 1;
            }

            pointX = Display.DipsToPixels(pointX);
            pointY = Display.DipsToPixels(pointY);

            var lineAtPosition = default(Int32?);
            var character = View.Resources.TextRenderer.GetGlyphAtPosition(textLayoutStream,
                (Int32)pointX, (Int32)pointY, snapToText, out lineAtPosition);

            return character ?? -1;
        }

        /// <summary>
        /// Gets the index of the first visible line of text.
        /// </summary>
        /// <returns>The index of the first visible line of text.</returns>
        public Int32 GetFirstVisibleLineIndex()
        {
            if (View == null)
                return 0;

            var position = Point2D.Zero;

            var scrollViewer = Parent as ScrollViewer;
            if (scrollViewer != null)
                position = new Point2D(0, scrollViewer.ContentVerticalOffset);

            var textFontFace = TextFontFace;
            if (textFontFace == null)
                return 0;

            return (Int32)Display.DipsToPixels(position.Y) / textFontFace.LineSpacing;
        }

        /// <summary>
        /// Gets the index of the last visible line of text.
        /// </summary>
        /// <returns>The index of the last visible line of text.</returns>
        public Int32 GetLastVisibleLineIndex()
        {
            if (View == null)
                return 0;

            var position = Point2D.Zero;

            var scrollViewer = Parent as ScrollViewer;
            if (scrollViewer != null)
                position = new Point2D(0, scrollViewer.ContentVerticalOffset + scrollViewer.ViewportHeight);

            var textFontFace = TextFontFace;
            if (textFontFace == null)
                return 0;

            return (Int32)Display.DipsToPixels(position.Y) / textFontFace.LineSpacing;
        }

        /// <summary>
        /// Gets the index of the line of text that contains the specified character.
        /// </summary>
        /// <param name="charIndex">The index of the character to evaluate.</param>
        /// <returns>The index of the line of text that contains the specified character.</returns>
        public Int32 GetLineIndexFromCharacterIndex(Int32 charIndex)
        {
            Contract.EnsureRange(charIndex >= 0 && charIndex < textLayoutStream.TotalSourceLength, nameof(charIndex));

            var lineInfo = textLayoutStream.GetLineInfo(0);
            var charCount = 0;

            if (charIndex < lineInfo.LengthInSource)
                return 0;

            charCount += lineInfo.LengthInSource;

            for (int i = 1; i < textLayoutStream.LineCount; i++)
            {
                textLayoutStream.GetNextLineInfoRef(ref lineInfo, out lineInfo);

                if (charIndex < charCount + lineInfo.LengthInSource)
                    return i;

                charCount += lineInfo.LengthInSource;
            }

            return textLayoutStream.LineCount - 1;
        }

        /// <summary>
        /// Gets the number of characters on the specified line of text.
        /// </summary>
        /// <param name="lineIndex">The index of the line to evaluate.</param>
        /// <returns>The number of characters on the specified line of text.</returns>
        public Int32 GetLineLength(Int32 lineIndex)
        {
            Contract.EnsureRange(lineIndex >= 0 && lineIndex < textLayoutStream.LineCount, nameof(lineIndex));

            var lineInfo = textLayoutStream.GetLineInfo(lineIndex);

            return lineInfo.LengthInSource;
        }

        /// <summary>
        /// Gets a rectangle that represents the leading edge of the specified character.
        /// </summary>
        /// <param name="charIndex">The index of the character for which to retrieve the rectangle.</param>
        /// <returns>A rectangle which represents the bounds of the leading edge of the specified character,
        /// or <see cref="RectangleD.Empty"/> if the bounding rectangle cannot be determined.</returns>
        public RectangleD GetRectFromCharacterIndex(Int32 charIndex)
        {
            return GetRectFromCharacterIndex(charIndex, false);
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
            Contract.EnsureRange(charIndex >= 0 && charIndex < textLayoutStream.TotalSourceLength, nameof(charIndex));

            if (View == null)
                return RectangleD.Empty;

            var bounds = View.Resources.TextRenderer.GetInsertionPointBounds(
                textLayoutStream, charIndex + (trailingEdge ? 1 : 0));

            return Display.PixelsToDips(bounds);
        }

        /// <summary>
        /// Gets the total number of characters in the text editor's text.
        /// </summary>
        /// <value>An <see cref="Int32"/> which represents the total number of characters
        /// in the text editor's text.</value>
        public Int32 TextLength
        {
            get { return bufferText.Length; }
        }

        /// <summary>
        /// Gets the total number of lines in the text editor's text.
        /// </summary>
        /// <value>A <see cref="Int32"/> which represents the total number of lines
        /// of text which are being displayed in the text editor.</value>
        public Int32 LineCount
        {
            get { return textLayoutStream.LineCount; }
        }

        /// <summary>
        /// Gets or sets the current position of the insertion caret.
        /// </summary>
        /// <value>A <see cref="Int32"/> which represents the offset of the insertion caret
        /// within the text editor's text.</value>
        public Int32 CaretIndex
        {
            get { return caretPosition; }
            set
            {
                Contract.EnsureRange(value >= 0 && value <= bufferText.Length, nameof(value));

                selectionPosition = null;

                caretPosition = AdjustCaretToAvoidInvalidPositions(value);
                caretBlinkTimer = 0;

                UpdateSelectionAndCaret();
            }
        }

        /// <summary>
        /// Gets or sets the starting point of the selected text.
        /// </summary>
        /// <value>A <see cref="Int32"/> which represents the offset of the beginning of the selected text.</value>
        public Int32 SelectionStart
        {
            get { return Math.Min(selectionPosition ?? caretPosition, caretPosition); }
            set
            {
                Contract.EnsureRange(value >= 0, nameof(value));

                var boundedStart = Math.Min(value, textLayoutStream.TotalSourceLength);
                var boundedLength = Math.Min(SelectionLength, textLayoutStream.TotalSourceLength - value);
                Select(boundedStart, boundedLength);
            }
        }

        /// <summary>
        /// Gets or sets the length of the selected text.
        /// </summary>
        /// <value>A <see cref="Int32"/> which represents the length in characters of the selected text.</value>
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
                Contract.EnsureRange(value >= 0, nameof(value));

                var boundedStart = SelectionStart;
                var boundedLength = Math.Min(SelectionLength, textLayoutStream.TotalSourceLength - boundedStart);
                Select(boundedStart, boundedLength);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the caret is drawn on top of the text while the caret is in insertion mode.
        /// </summary>
        /// <value><see langword="true"/> if the caret is drawn above the text while the caret is in insertion mode;
        /// otherwise, <see langword="false"/>. The default value is <see langword="true"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="CaretInsertTopmostProperty"/></dpropField>
        ///		<dpropStylingName>caret-insert-topmost</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean CaretInsertTopmost
        {
            get { return GetValue<Boolean>(CaretInsertTopmostProperty); }
            set { SetValue(CaretInsertTopmostProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the caret is drawn on top of the text while the caret is in overwrite mode.
        /// </summary>
        /// <value><see langword="true"/> if the caret is drawn above the text while the caret is in overwrite mode;
        /// otherwise, <see langword="false"/>. The default value is <see langword="true"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="CaretOverwriteTopmostProperty"/></dpropField>
        ///		<dpropStylingName>caret-overwrite-topmost</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean CaretOverwriteTopmost
        {
            get { return GetValue<Boolean>(CaretOverwriteTopmostProperty); }
            set { SetValue(CaretOverwriteTopmostProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image used to draw the caret while the caret is in insertion mode.
        /// </summary>
        /// <value>A <see cref="SourcedImage"/> which represents the image that is used to draw the 
        /// caret while the caret is in insertion mode. The default value is an invalid image.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="CaretInsertImageProperty"/></dpropField>
        ///		<dpropStylingName>caret-insert-image</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public SourcedImage CaretInsertImage
        {
            get { return GetValue<SourcedImage>(CaretInsertImageProperty); }
            set { SetValue(CaretInsertImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image used to draw the caret while the caret is in overwrite mode.
        /// </summary>
        /// <value>A <see cref="SourcedImage"/> which represents the image that is used to draw the
        /// caret while the caret is in overwrite mode. The default value is an invalid image.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="CaretOverwriteImageProperty"/></dpropField>
        ///		<dpropStylingName>caret-overwrite-image</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public SourcedImage CaretOverwriteImage
        {
            get { return GetValue<SourcedImage>(CaretInsertImageProperty); }
            set { SetValue(CaretInsertImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the caret is drawn while the caret is in insertion mode.
        /// </summary>
        /// <value>A <see cref="Color"/> value which is used to draw the caret while it is in insertion mode.
        /// The default value is <see cref="Color.White"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="CaretInsertColorProperty"/></dpropField>
        ///		<dpropStylingName>caret-insert-color</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color CaretInsertColor
        {
            get { return GetValue<Color>(CaretInsertColorProperty); }
            set { SetValue(CaretInsertColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the caret is drawn while the caret is in overwrite mode.
        /// </summary>
        /// <value>A <see cref="Color"/> value which is used to draw the caret while it is in overwrite mode.
        /// The default value is <see cref="Color.White"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="CaretOverwriteColorProperty"/></dpropField>
        ///		<dpropStylingName>caret-overwrite-color</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color CaretOverwriteColor
        {
            get { return GetValue<Color>(CaretOverwriteColorProperty); }
            set { SetValue(CaretOverwriteColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the caret's width when it is in insertion mode, specified in device in independent pixels.
        /// </summary>
        /// <value>A <see cref="Double"/> which represents the width of the caret in device-independent pixels while
        /// the caret is in insertion mode. The default value is 1.0.</value>
        /// <remarks>
        /// <para>The caret will never be wider than the glyph at which it is positioned, regardless of this value.</para>
        /// <dprop>
        ///		<dpropField><see cref="CaretWidthProperty"/></dpropField>
        ///		<dpropStylingName>caret-width</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double CaretWidth
        {
            get { return GetValue<Double>(CaretWidthProperty); }
            set { SetValue(CaretWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the cart's thickness when it is in overwrite mode, specified in device independent pixels.
        /// </summary>
        /// <value>A <see cref="Double"/> which represents the thickness of the caret in device-independent pixels while
        /// the caret is in overwrite mode. The default value is 4.0.</value>
        /// <remarks>
        /// <para>The caret will never be taller than the glyph at which it is positioned, regardless of this value.</para>
        /// <dprop>
        ///		<dpropField><see cref="CaretThicknessProperty"/></dpropField>
        ///		<dpropStylingName>caret-thickness</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double CaretThickness
        {
            get { return GetValue<Double>(CaretThicknessProperty); }
            set { SetValue(CaretThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image used to draw the selection highlight.
        /// </summary>
        /// <value>A <see cref="SourcedImage"/> which represents the image that is used to draw the text selection highlight.
        /// The default value is an invalid image.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="SelectionImageProperty"/></dpropField>
        ///		<dpropStylingName>selection-image</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public SourcedImage SelectionImage
        {
            get { return GetValue<SourcedImage>(SelectionImageProperty); }
            set { SetValue(SelectionImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the selection highlight is drawn.
        /// </summary>
        /// <value>A <see cref="Color"/> value which is used to draw the text selection highlight while
        /// the text box has focus. The default value is <see cref="Color.Blue"/> at 40% opacity.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="SelectionColorProperty"/></dpropField>
        ///		<dpropStylingName>selection-color</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color SelectionColor
        {
            get { return GetValue<Color>(SelectionColorProperty); }
            set { SetValue(SelectionColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the selection highlight is drawn when the control does not have focus.
        /// </summary>
        /// <value>A <see cref="Color"/> value which is used to draw the text selection highlight while
        /// the text box does not have focus. The default value is <see cref="Color.Silver"/> at 40% opacity.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="InactiveSelectionColorProperty"/></dpropField>
        ///		<dpropStylingName>inactive-selection-color</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color InactiveSelectionColor
        {
            get { return GetValue<Color>(InactiveSelectionColorProperty); }
            set { SetValue(InactiveSelectionColorProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether this editor is read-only.
        /// </summary>
        public Boolean IsReadOnly
        {
            get
            {
                var owner = TemplatedParent as Control;
                return (owner != null && owner.GetValue<Boolean>(TextBoxBase.IsReadOnlyProperty));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this editor's caret is visible when it is read-only.
        /// </summary>
        public Boolean IsReadOnlyCaretVisible
        {
            get
            {
                var owner = TemplatedParent as Control;
                return (owner != null && owner.GetValue<Boolean>(TextBoxBase.IsReadOnlyCaretVisibleProperty));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this editor masks its text.
        /// </summary>
        public Boolean IsMasked
        {
            get { return MaskCharacter.HasValue; }
        }

        /// <summary>
        /// Gets a value indicating whether this editor accepts new lines.
        /// </summary>
        public Boolean AcceptsReturn
        {
            get
            {
                var owner = TemplatedParent as Control;
                return (owner != null && owner.GetValue<Boolean>(TextBoxBase.AcceptsReturnProperty));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this editor accepts tabs.
        /// </summary>
        public Boolean AcceptsTab
        {
            get
            {
                var owner = TemplatedParent as Control;
                return (owner != null && owner.GetValue<Boolean>(TextBoxBase.AcceptsTabProperty));
            }
        }

        /// <summary>
        /// Identifies the <see cref="CaretInsertTopmost"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="CaretInsertTopmost"/> dependency property.</value>
        public static readonly DependencyProperty CaretInsertTopmostProperty = DependencyProperty.Register("CaretInsertTopmost", typeof(Boolean), typeof(TextEditor),
            new PropertyMetadata<Boolean>(true, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CaretOverwriteTopmost"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="CaretOverwriteTopmost"/> dependency property.</value>
        public static readonly DependencyProperty CaretOverwriteTopmostProperty = DependencyProperty.Register("CaretOverwriteTopmost", typeof(Boolean), typeof(TextEditor),
            new PropertyMetadata<Boolean>(true, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CaretInsertImage"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="CaretInsertImage"/> dependency property.</value>
        public static readonly DependencyProperty CaretInsertImageProperty = DependencyProperty.Register("CaretInsertImage", typeof(SourcedImage), typeof(TextEditor),
            new PropertyMetadata<SourcedImage>(null, PropertyMetadataOptions.None, HandleCaretInsertImageChanged));

        /// <summary>
        /// Identifies the <see cref="CaretOverwriteImage"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="CaretOverwriteImage"/> dependency property.</value>
        public static readonly DependencyProperty CaretOverwriteImageProperty = DependencyProperty.Register("CaretOverwriteImage", typeof(SourcedImage), typeof(TextEditor),
            new PropertyMetadata<SourcedImage>(null, PropertyMetadataOptions.None, HandleCaretOverwriteImageChanged));

        /// <summary>
        /// Identifies the <see cref="CaretInsertColor"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="CaretInsertColor"/> dependency property.</value>
        public static readonly DependencyProperty CaretInsertColorProperty = DependencyProperty.Register("CaretInsertColor", typeof(Color), typeof(TextEditor),
            new PropertyMetadata<Color>(Color.White, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CaretOverwriteColor"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="CaretOverwriteColor"/> dependency property.</value>
        public static readonly DependencyProperty CaretOverwriteColorProperty = DependencyProperty.Register("CaretOverwriteColor", typeof(Color), typeof(TextEditor),
            new PropertyMetadata<Color>(Color.White, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CaretWidth"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="CaretWidth"/> dependency property.</value>
        public static readonly DependencyProperty CaretWidthProperty = DependencyProperty.Register("CaretWidth", typeof(Double), typeof(TextEditor),
            new PropertyMetadata<Double>(1.0, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CaretThickness"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="CaretThickness"/> dependency property.</value>
        public static readonly DependencyProperty CaretThicknessProperty = DependencyProperty.Register("CaretThickness", typeof(Double), typeof(TextEditor),
            new PropertyMetadata<Double>(4.0, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="SelectionImage"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="SelectionImage"/> dependency property.</value>
        public static readonly DependencyProperty SelectionImageProperty = DependencyProperty.Register("SelectionImage", typeof(SourcedImage), typeof(TextEditor),
            new PropertyMetadata<SourcedImage>(null, PropertyMetadataOptions.None, HandleSelectionImageChanged));

        /// <summary>
        /// Identifies the <see cref="SelectionColor"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="SelectionColor"/> dependency property.</value>
        public static readonly DependencyProperty SelectionColorProperty = DependencyProperty.Register("SelectionColor", typeof(Color), typeof(TextEditor),
            new PropertyMetadata<Color>(Color.Blue * 0.4f, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="InactiveSelectionColor"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="InactiveSelectionColor"/> dependency property.</value>
        public static readonly DependencyProperty InactiveSelectionColorProperty = DependencyProperty.Register("InactiveSelectionColor", typeof(Color), typeof(TextEditor),
            new PropertyMetadata<Color>(Color.Silver * 0.4f, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Controls.Primitives.TextEditor.TextEntryValidation"/> attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Controls.Primitives.TextEditor.TextEntryValidation"/> attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the text editor is validating text that has been entered.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///		<revtField><see cref="TextEntryValidationEvent"/></revtField>
        ///		<revtStylingName>text-entry-validation</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfTextEntryValidationHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent TextEntryValidationEvent = EventManager.RegisterRoutedEvent("TextEntryValidation",
            RoutingStrategy.Bubble, typeof(UpfTextEntryValidationHandler), typeof(TextEditor));

        /// <summary>
        /// Gets a value indicating whether this control lays out text right-to-left.
        /// </summary>
        /// <returns><see langword="true"/> if the editor is right-to-left; otherwise, <see langword="false"/>.</returns>
        internal Boolean IsRightToLeft()
        {
            var parent = TemplatedParent as FrameworkElement;
            return parent?.FlowDirection == FlowDirection.RightToLeft;
        }

        /// <summary>
        /// Called when the value of the <see cref="TextBox.TextProperty"/> dependency property changes.
        /// </summary>
        /// <param name="value">The new value of the dependency property.</param>
        /// <returns><see langword="true"/> if the editor replaced the text with its own source; otherwise, <see langword="false"/>.</returns>
        internal Boolean HandleTextChanged(VersionedStringSource value)
        {
            if (value.IsSourcedFromStringBuilder && (VersionedStringBuilder)value == bufferText)
                return false;

            var valueHasNotChanged = value.Length == bufferText.Length;
            if (valueHasNotChanged)
            {
                for (int i = 0; i < bufferText.Length; i++)
                {
                    if (bufferText[i] != value[i])
                    {
                        valueHasNotChanged = false;
                        break;
                    }
                }
            }

            if (valueHasNotChanged)
                return false;

            if (value.IsSourcedFromStringBuilder)
            {
                var vsb = (VersionedStringBuilder)value;
                if (vsb.Version == bufferText.Version)
                    return false;
            }

            BeginTrackingSelectionChanges();
            
            ClearBuffer(value.Length == 0);
            InsertIntoBuffer(value, bufferText.Length);

            if (caretPosition > bufferText.Length)
            {
                caretBlinkTimer = 0;
                caretPosition = bufferText.Length;

                pendingScrollToCaret = true;
                pendingScrollState = PendingScrollState.None;
            }

            UpdateTextStringSource();
            UpdateTextParserStream();

            EndTrackingSelectionChanges();

            ScrollToCaret(true, false, false);

            return true;
        }

        /// <summary>
        /// Replaces the editor's text with a string of the specified masking characters.
        /// </summary>
        /// <param name="mask">The masking character.</param>
        internal void ReplaceTextWithMask(Char mask)
        {
            var length = bufferText.Length;

            bufferText.Length = 0;

            for (int i = 0; i < length; i++)
                bufferText.Append(mask);

            UpdateTextStringSource();
            UpdateTextParserStream();
        }

        /// <summary>
        /// Called when the text editor's text is changed by the <see cref="TextBox.SetText(StringBuilder)"/> method.
        /// </summary>
        /// <param name="value">The <see cref="StringBuilder"/> that contains the text editor's new text.</param>
        internal void HandleTextChanged(StringBuilder value)
        {
            BeginTrackingSelectionChanges();

            selectionPosition = null;

            ClearBuffer(value.Length == 0);
            InsertIntoBuffer(value, bufferText.Length);

            if (caretPosition > value.Length)
            {
                caretPosition = value.Length;
                caretBlinkTimer = 0;

                UpdateSelectionAndCaret();
            }

            UpdateTextStringSource();
            UpdateTextParserStream();

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Called when the editor should process a mouse button being pressed.
        /// </summary>
        /// <param name="device">The <see cref="MouseDevice"/> that raised the event.</param>
        /// <param name="button">The <see cref="MouseButton"/> value that corresponds to the button that was pressed.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void HandleMouseDown(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (textLayoutStream.Count == 0)
                return;

            if (button == MouseButton.Left)
            {
                BeginSelectionWithCursor(0, Mouse.GetPosition(this));
            }
        }

        /// <summary>
        /// Called when the editor should process a mouse button being released.
        /// </summary>
        /// <param name="device">The <see cref="MouseDevice"/> that raised the event.</param>
        /// <param name="button">The <see cref="MouseButton"/> value that corresponds to the button that was released.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void HandleMouseUp(MouseDevice device, MouseButton button, RoutedEventData data)
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
        internal void HandleMouseDoubleClick(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                SelectCurrentToken();
                ScrollToCaret(true, false, false);
            }
        }

        /// <summary>
        /// Called when the editor should process the mouse being moved.
        /// </summary>
        /// <param name="device">The <see cref="MouseDevice"/> that raised the event.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void HandleMouseMove(MouseDevice device, RoutedEventData data)
        {
            if (selectionFollowingCursor && selectionFollowingCursorID == 0)
            {
                MoveCaretToCursor(Mouse.GetPosition(this));
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
        /// Called when the editor should process a touch beginning over the editor.
        /// </summary>
        /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
        /// <param name="id">The unique identifier of the touch.</param>
        /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="pressure">The normalized pressure of the touch.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void HandleTouchDown(TouchDevice device, 
            Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            if (Ultraviolet.GetInput().IsMouseCursorAvailable || textLayoutStream.Count == 0)
                return;

            if (device.IsFirstTouchInGesture(id))
            {
                BeginSelectionWithCursor(id, Touch.GetPosition(id, this));
            }
        }

        /// <summary>
        /// Called when the editor should process a touch ending over the editor.
        /// </summary>
        /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
        /// <param name="id">The unique identifier of the touch that was released.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void HandleTouchUp(TouchDevice device,
            Int64 id, RoutedEventData data)
        {
            if (Ultraviolet.GetInput().IsMouseCursorAvailable || textLayoutStream.Count == 0)
                return;

            if (device.IsFirstTouchInGesture(id))
            {
                FinishSelection();
            }
        }

        /// <summary>
        /// Called when the editor should press a long press on the editor.
        /// </summary>
        /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
        /// <param name="id">The unique identifier of the touch.</param>
        /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="pressure">The normalized pressure of the touch.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void HandleTouchLongPress(TouchDevice device,
            Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            if (Ultraviolet.GetInput().IsMouseCursorAvailable)
                return;

            if (device.IsFirstTouchInGesture(id))
            {
                SelectCurrentToken();
                ScrollToCaret(true, false, false);
            }
        }

        /// <summary>
        /// Called when the editor should process a touch being moved.
        /// </summary>
        /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
        /// <param name="id">The unique identifier of the touch.</param>
        /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="dx">The difference between the x-coordinate of the touch's 
        /// current position and the x-coordinate of the touch's previous position.</param>
        /// <param name="dy">The difference between the y-coordinate of the touch's 
        /// current position and the y-coordinate of the touch's previous position.</param>
        /// <param name="pressure">The normalized pressure of the touch.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void HandleTouchMove(TouchDevice device, 
            Int64 id, Double x, Double y, Double dx, Double dy, Single pressure, RoutedEventData data)
        {
            if (Ultraviolet.GetInput().IsMouseCursorAvailable)
                return;

            if (selectionFollowingCursor && selectionFollowingCursorID == id)
            {
                MoveCaretToCursor(Touch.GetPosition(id, this));
            }
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
        /// Called when the editor should read text input from the keyboard device.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void HandleTextInput(KeyboardDevice device, RoutedEventData data)
        {
            var owner = TemplatedParent as Control;

            var isReadOnly = (owner != null && owner.GetValue<Boolean>(TextBox.IsReadOnlyProperty));
            if (isReadOnly)
                return;

            device.GetTextInput(bufferInput);
            InsertTextAtCaret((StringSegment)bufferInput, true);

            data.Handled = true;
        }

        /// <summary>
        /// Called when the editor should handle a text editing event.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void HandleTextEditing(KeyboardDevice device, RoutedEventData data)
        {

        }

        /// <summary>
        /// Called when the value of the <see cref="Primitives.TextBoxBase.IsReadOnly"/> property changes.
        /// </summary>
        internal void HandleIsReadOnlyChanged()
        {
            UpdateSelectionAndCaret();
        }

        /// <inheritdoc/>
        protected override void OnViewChanged(PresentationFoundationView oldView, PresentationFoundationView newView)
        {
            UpdateTextParserStream();

            base.OnViewChanged(oldView, newView);
        }
        
        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            UpdateTextLayoutStream(availableSize);

            var caretSize = GetDefaultSizeOfInsertionCaret();

            var textWidth = textLayoutStream.Settings.Width ?? textLayoutStream.ActualWidth;
            var textHeight = textLayoutStream.Settings.Height ?? textLayoutStream.ActualHeight;

            var desiredWidth = Display.PixelsToDips(textWidth + caretSize.Width);
            var desiredHeight = Math.Max(Display.PixelsToDips(textHeight), Display.PixelsToDips(caretSize.Height));

            return new Size2D(desiredWidth, desiredHeight);
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            if (IsRightToLeft() && textLayoutStream != null)
            {
                textOffsetX = (Int32)Display.DipsToPixels(finalSize.Width) - ((textLayoutStream.Settings.Width ?? 0) + 1);
            }

            UpdateSelectionAndCaret();

            return finalSize;
        }

        /// <inheritdoc/>
        protected override void UpdateOverride(UltravioletTime time)
        {
            if (textLayoutStream != null && textLayoutStream.Count > 0 && textLayoutStream.Settings.Font != TextFont)
            {
                InvalidateMeasure();
            }
            base.UpdateOverride(time);
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            var minClip = (Int32?)null;
            var maxClip = (Int32?)null;

            var scrollViewer = Parent as ScrollViewer;
            if (scrollViewer != null)
            {
                minClip = (Int32)Math.Floor(Display.DipsToPixels(scrollViewer.ContentVerticalOffset));
                maxClip = (Int32)Math.Ceiling(Display.DipsToPixels(scrollViewer.ContentVerticalOffset) + Display.DipsToPixels(scrollViewer.ViewportHeight));
            }

            DrawSelection(time, minClip, maxClip, dc);

            var caretTopmost =
                (actualInsertionMode == CaretMode.Insert && CaretInsertTopmost) ||
                (actualInsertionMode == CaretMode.Overwrite && CaretOverwriteTopmost);

            if (caretTopmost)
            {
                DrawText(time, minClip, maxClip, dc);
                DrawCaret(time, minClip, maxClip, dc);
            }
            else
            {
                DrawCaret(time, minClip, maxClip, dc);
                DrawText(time, minClip, maxClip, dc);
            }

            base.DrawOverride(time, dc);
        }

        /// <inheritdoc/>
        protected override void ReloadContentOverride(Boolean recursive)
        {
            ReloadCaretInsertImage();
            ReloadCaretOverwriteImage();
            ReloadSelectionImage();

            if (textLayoutStream.Count > 0)
                UpdateTextLayoutStream(MostRecentAvailableSize);

            UpdateSelectionAndCaret();

            base.ReloadContentOverride(recursive);
        }

        /// <inheritdoc/>
        protected override void PrepareOverride()
        {
            LayoutUpdated -= OnLayoutUpdated;
            LayoutUpdated += OnLayoutUpdated;

            base.PrepareOverride();
        }

        /// <inheritdoc/>
        protected override void CleanupOverride()
        {
            LayoutUpdated -= OnLayoutUpdated;

            base.CleanupOverride();
        }

        /// <summary>
        /// Gets a value specifying whether the specified character is valid to enter at the specified position in the text.
        /// </summary>
        /// <param name="offset">The offset at which the character is being entered.</param>
        /// <param name="character">The character which is being entered.</param>
        /// <returns><see langword="true"/> if the character is valid for entry; otherwise, <see langword="false"/>.</returns>
        protected virtual Boolean IsValidCharacterForEntry(Int32 offset, Char character)
        {
            if (TemplatedParent == null)
                return true;

            var evtDelegate = EventManager.GetInvocationDelegate<UpfTextEntryValidationHandler>(TextEntryValidationEvent);
            var evtData = TextEntryValidationRoutedEventData.Retrieve(TemplatedParent, autorelease: false);

            var text = new StringSegment((StringBuilder)bufferText);
            evtDelegate(TemplatedParent, text, offset, character, evtData);
            var valid = evtData.Valid;
            evtData.Release();

            return valid;
        }

        /// <summary>
        /// Called when a character is inserted into the text box.
        /// </summary>
        /// <param name="offset">The insertion position at which the character was inserted.</param>
        /// <param name="charRequested">The character that was proposed for insertion, prior to masking.</param>
        /// <param name="charInserted">The character that was actually inserted into the text.</param>
        /// <param name="raiseChangeEvents">A value indicating whether events related to the text changing should be raised.</param>
        protected virtual void OnCharacterInserted(Int32 offset, Char charRequested, Char charInserted, Boolean raiseChangeEvents)
        {

        }

        /// <summary>
        /// Called when a character is removed from the text box.
        /// </summary>
        /// <param name="offset">The index of the first character that was removed.</param>
        /// <param name="length">The number of characters that were removed.</param>
        /// <param name="raiseChangeEvents">A value indicating whether events related to the text changing should be raised.</param>
        protected virtual void OnCharacterDeleted(Int32 offset, Int32 length, Boolean raiseChangeEvents)
        {

        }

        /// <summary>
        /// Reloads the <see cref="CaretInsertImage"/> resource.
        /// </summary>
        protected void ReloadCaretInsertImage()
        {
            LoadResource(CaretInsertImage);
        }

        /// <summary>
        /// Reloads the <see cref="CaretOverwriteImage"/> resource.
        /// </summary>
        protected void ReloadCaretOverwriteImage()
        {
            LoadResource(CaretOverwriteImage);
        }

        /// <summary>
        /// Reloads the <see cref="SelectionImage"/> resource.
        /// </summary>
        protected void ReloadSelectionImage()
        {
            LoadResource(SelectionImage);
        }

        /// <summary>
        /// Gets the font with which the editor draws its text.
        /// </summary>
        protected UltravioletFont TextFont
        {
            get
            {
                var owner = TemplatedParent as Control;
                return (owner != null && owner.Font.IsLoaded) ? owner.Font.Resource : null;
            }
        }

        /// <summary>
        /// Gets the font face with which the editor draws its text.
        /// </summary>
        protected UltravioletFontFace TextFontFace
        {
            get
            {
                var owner = TemplatedParent as Control;
                if (owner == null || !owner.Font.IsLoaded)
                    return null;

                return owner.Font.Resource.Value.GetFace(owner.FontStyle);
            }
        }

        /// <summary>
        /// The text editor's masking character, or <see langword="null"/> if masking is not enabled.
        /// </summary>
        protected virtual Char? MaskCharacter
        {
            get { return null; }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CaretInsertImage"/> dependency property changes.
        /// </summary>
        private static void HandleCaretInsertImageChanged(DependencyObject dobj, SourcedImage oldValue, SourcedImage newValue)
        {
            var textEditor = (TextEditor)dobj;
            textEditor.ReloadCaretInsertImage();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CaretOverwriteImage"/> dependency property changes.
        /// </summary>
        private static void HandleCaretOverwriteImageChanged(DependencyObject dobj, SourcedImage oldValue, SourcedImage newValue)
        {
            var textEditor = (TextEditor)dobj;
            textEditor.ReloadCaretOverwriteImage();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionImage"/> dependency property changes.
        /// </summary>
        private static void HandleSelectionImageChanged(DependencyObject dobj, SourcedImage oldValue, SourcedImage newValue)
        {
            var textEditor = (TextEditor)dobj;
            textEditor.ReloadSelectionImage();
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is a "non-word" character such 
        /// as white space or a hyphen.
        /// </summary>
        private static Boolean IsNonWordCharacter(Char c)
        {
            return Char.IsWhiteSpace(c) || c == '-';
        }

        /// <summary>
        /// Gets a value indicating whether the specified character combination represents the end of a word.
        /// </summary>
        private static Boolean IsEndOfWordBoundary(Char charPrev, Char charNext)
        {
            return IsNonWordCharacter(charPrev) && !IsNonWordCharacter(charNext);
        }

        /// <summary>
        /// Adjusts the specified caret position so that it does not lie at an invalid position such as the
        /// middle of a CRLF sequence or in the middle of a surrogate pair.
        /// </summary>
        private Int32 AdjustCaretToAvoidInvalidPositions(Int32 position, Boolean moveForward = false)
        {
            if (position == 0 || position == bufferText.Length)
                return position;

            var isCRLF = bufferText[position] == '\n' && bufferText[position - 1] == '\r';
            if (isCRLF)
                return position + (moveForward ? 1 : -1);

            var isSurrogatePair = Char.IsSurrogatePair(bufferText[position - 1], bufferText[position]);
            if (isSurrogatePair)
                return position + (moveForward ? 1 : -1);

            return position;
        }

        /// <summary>
        /// Gets the default size of the insertion caret.
        /// </summary>
        /// <returns>The default size of the insertion caret.</returns>
        private Size2 GetDefaultSizeOfInsertionCaret()
        {
            var textFontFace = TextFontFace;
            if (textFontFace == null)
                return Size2.Zero;

            var fontLineSpacing = textFontFace.LineSpacing - textFontFace.Descender;
            var fontLineSpacingHalf = fontLineSpacing / 2;

            var caretWidth = Math.Min((Int32)Display.DipsToPixels(CaretWidth), fontLineSpacingHalf);
            var caretHeight = fontLineSpacing;

            return new Size2(caretWidth, caretHeight);
        }

        /// <summary>
        /// Gets the default size of the overwrite caret.
        /// </summary>
        /// <returns>The default size of the overwrite caret.</returns>
        private Size2 GetDefaultSizeOfOverwriteCaret()
        {
            var textFontFace = TextFontFace;
            if (textFontFace == null)
                return Size2.Zero;

            var fontLineSpacing = textFontFace.LineSpacing;
            var fontLineSpacingHalf = fontLineSpacing / 2;

            var caretWidth = fontLineSpacingHalf;
            var caretHeight = fontLineSpacing;

            return new Size2(caretWidth, caretHeight);
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

            View.Resources.TextRenderer.Parser.Parse((StringBuilder)bufferText, textParserStream, TextParserOptions.IgnoreCommandCodes);
            InvalidateMeasure();

            pendingTextLayout = true;
        }

        /// <summary>
        /// Updates the text layout stream.
        /// </summary>
        private void UpdateTextLayoutStream(Size2D availableSize)
        {
            pendingTextLayout = false;

            textLayoutStream.Clear();

            if (View == null)
                return;

            if (textParserStream.Count == 0)
            {
                UpdateSelectionAndCaret();
                return;
            }

            var owner = TemplatedParent as Control;
            if (owner == null || !owner.Font.IsLoaded)
                return;

            var textFlags = TextFlags.AlignTop;
            var textWrapping = GetActualTextWrapping();
            var textAlignment = GetActualTextAlignment();

            var layoutWidth = (textWrapping == TextWrapping.Wrap) ? (Int32?)Display.DipsToPixels(availableSize.Width) : null;
            if (layoutWidth.HasValue)
            {
                layoutWidth--; // Leave space for the cursor
            }
            var layoutHeight = (Int32?)null;

            switch (textAlignment)
            {
                case TextAlignment.Left:
                    textFlags |= TextFlags.AlignLeft;
                    break;

                case TextAlignment.Center:
                    textFlags |= TextFlags.AlignCenter;
                    break;

                case TextAlignment.Right:
                    textFlags |= TextFlags.AlignRight;
                    break;
            }

            var textRenderingMode = (owner == null) ? TextOptions.GetTextRenderingMode(this) : TextOptions.GetTextRenderingMode(owner);
            var textLanguage = (owner == null) ? TextOptions.GetTextLanguage(this) : TextOptions.GetTextLanguage(owner);
            var textScript = (owner == null) ? TextOptions.GetTextScript(this) : TextOptions.GetTextScript(owner);
            var textDirection = IsRightToLeft() ? TextDirection.RightToLeft : TextDirection.LeftToRight;

            var options = (textRenderingMode == TextRenderingMode.Shaped) ? TextLayoutOptions.Shape : TextLayoutOptions.None;
            var settings = new TextLayoutSettings(owner.Font, layoutWidth, layoutHeight, textFlags, options, textDirection, textScript, UltravioletFontStyle.Regular, null, textLanguage);
            View.Resources.TextRenderer.CalculateLayout(textParserStream, textLayoutStream, settings);

            textOffsetX = 0;
            textOffsetY = 0;

            UpdateSelectionAndCaret();
        }

        /// <summary>
        /// Draws the current selection.
        /// </summary>
        private void DrawSelection(UltravioletTime time, Int32? minClip, Int32? maxClip, DrawingContext dc)
        {
            if (!selectionPosition.HasValue || SelectionLength == 0)
                return;

            // Don't draw the selection unless we have keyboard focus (unless IsInactiveSelectionHighlightEnabled is true).
            var owner = TemplatedParent as Control;
            if (owner == null)
                return;

            var isActive = owner.IsKeyboardFocusWithin;
            var isEnabledIfInactive = owner.GetValue<Boolean>(TextBox.IsInactiveSelectionHighlightEnabledProperty);
            if (!isActive && !isEnabledIfInactive)
                return;

            var selectionColor = isActive ? SelectionColor : InactiveSelectionColor;

            // Draw the first line
            var selectionTopOffset = global::Ultraviolet.Rectangle.Offset(selectionTop, textOffsetX, textOffsetY);
            if ((!minClip.HasValue || selectionTopOffset.Bottom > minClip.Value) && (!maxClip.HasValue || selectionTopOffset.Top <= maxClip.Value))
            {
                var selectionTopDips = Display.PixelsToDips(selectionTopOffset);
                DrawImage(dc, SelectionImage, selectionTopDips, selectionColor, true);
            }

            // Draw the middle
            if (selectionLineCount > 2)
            {
                textLayoutStream.AcquirePointers();

                var lineInfo = textLayoutStream.GetLineInfo(selectionLineStart);

                for (int i = selectionLineStart + 1; i < selectionLineStart + selectionLineCount - 1; i++)
                {
                    textLayoutStream.GetNextLineInfoRef(ref lineInfo, out lineInfo);

                    var lineBounds = new Ultraviolet.Rectangle(lineInfo.X + textOffsetX, lineInfo.Y + textOffsetY, lineInfo.Width, lineInfo.Height);

                    if (minClip.HasValue && lineBounds.Bottom <= minClip.Value)
                        continue;

                    if (maxClip.HasValue && lineBounds.Top > maxClip.Value)
                        break;

                    var lineBoundsDips = Display.PixelsToDips(lineBounds);
                    DrawImage(dc, SelectionImage, lineBoundsDips, selectionColor, true);
                }

                textLayoutStream.ReleasePointers();
            }

            // Draw the last line
            if (selectionLineCount > 1)
            {
                var selectionBottomOffset = global::Ultraviolet.Rectangle.Offset(selectionBottom, textOffsetX, textOffsetY);
                if ((!minClip.HasValue || selectionBottomOffset.Bottom > minClip.Value) && (!maxClip.HasValue || selectionBottomOffset.Top <= maxClip.Value))
                {
                    var selectionBottomDips = Display.PixelsToDips(selectionBottomOffset);
                    DrawImage(dc, SelectionImage, selectionBottomDips, selectionColor, true);
                }
            }
        }

        /// <summary>
        /// Draws the editor's text.
        /// </summary>
        private void DrawText(UltravioletTime time, Int32? minClip, Int32? maxClip, DrawingContext dc)
        {
            if (textLayoutStream.Count == 0)
                return;

            var owner = TemplatedParent as Control;
            var foreground = (owner == null) ? Color.White : owner.Foreground;

            var positionRaw = Display.DipsToPixels(UntransformedAbsolutePosition);
            var positionX = dc.IsTransformed ? (textOffsetX + positionRaw.X) : Math.Floor(textOffsetX + positionRaw.X);
            var positionY = dc.IsTransformed ? (textOffsetY + positionRaw.Y) : Math.Floor(textOffsetY + positionRaw.Y);
            var position = new Vector2((Single)positionX, (Single)positionY);
            View.Resources.TextRenderer.Draw((SpriteBatch)dc, textLayoutStream, position, minClip, maxClip, foreground * dc.Opacity);
        }

        /// <summary>
        /// Draws the editor's caret.
        /// </summary>
        private void DrawCaret(UltravioletTime time, Int32? minClip, Int32? maxClip, DrawingContext dc)
        {
            var owner = TemplatedParent as Control;
            if (owner == null)
                return;

            var isReadOnly = owner.GetValue<Boolean>(TextBox.IsReadOnlyProperty);
            var isReadOnlyCaretVisible = owner.GetValue<Boolean>(TextBox.IsReadOnlyCaretVisibleProperty);
            if (isReadOnly && !isReadOnlyCaretVisible)
                return;

            if (selectionFollowingCursor || !owner.IsKeyboardFocusWithin)
                return;

            caretBlinkTimer = (caretBlinkTimer + time.ElapsedTime.TotalMilliseconds) % 1000.0;

            var isCaretVisible = ((Int32)(caretBlinkTimer / 500.0) % 2 == 0);
            if (isCaretVisible)
            {
                var caretBoundsOffset = global::Ultraviolet.Rectangle.Offset(caretRenderBounds, textOffsetX, textOffsetY);
                if ((minClip.HasValue && caretBoundsOffset.Bottom < minClip.Value) || (maxClip.HasValue && caretBoundsOffset.Top > maxClip.Value))
                    return;

                var caretBoundsDips = Display.PixelsToDips(caretBoundsOffset);
                var caretImage = (actualInsertionMode == CaretMode.Insert) ? CaretInsertImage : CaretOverwriteImage;
                var caretColor = (actualInsertionMode == CaretMode.Insert) ? CaretInsertColor : CaretOverwriteColor;
                DrawImage(dc, caretImage, caretBoundsDips, caretColor, true);
            }
        }

        /// <summary>
        /// Handles the <see cref="UIElement.LayoutUpdated"/> event.
        /// </summary>
        private void OnLayoutUpdated(Object sender, EventArgs e)
        {
            if (pendingScrollToCaret)
            {
                pendingScrollToCaret = false;

                var showMaximumLineWidth = (pendingScrollState & PendingScrollState.ShowMaximumLineWidth) == PendingScrollState.ShowMaximumLineWidth;
                var jumpLeft = (pendingScrollState & PendingScrollState.JumpLeft) == PendingScrollState.JumpLeft;
                var jumpRight = (pendingScrollState & PendingScrollState.JumpRight) == PendingScrollState.JumpRight;
                ScrollToCaret(showMaximumLineWidth, jumpLeft, jumpRight);
            }
        }

        /// <summary>
        /// Deletes the text in the current selection.
        /// </summary>
        /// <param name="raiseChangeEvents">A value indicating whether the editor should raise events relating to the text being changed.</param>
        /// <returns><see langword="true"/> if the selection was deleted; otherwise, <see langword="false"/>.</returns>
        private Boolean DeleteSelection(Boolean raiseChangeEvents)
        {
            var length = SelectionLength;
            if (length == 0)
                return false;

            BeginTrackingSelectionChanges();

            var start = SelectionStart;
            selectionPosition = null;

            if (caretPosition > start)
            {
                caretBlinkTimer = 0;
                caretPosition = start;
            }
            
            bufferText.Remove(start, length);
            OnCharacterDeleted(start, length, raiseChangeEvents);

            UpdateTextStringSource();
            UpdateTextParserStream();

            EndTrackingSelectionChanges();

            return true;
        }

        /// <summary>
        /// Deletes the specified span of text.
        /// </summary>
        /// <param name="start">The first character to delete.</param>
        /// <param name="length">The number of characters to delete.</param>
        /// <param name="raiseChangeEvents">A value indicating whether the editor should raise events relating to the text being changed.</param>
        /// <returns><see langword="true"/> if the span was deleted; otherwise, <see langword="false"/>.</returns>
        private Boolean DeleteSpan(Int32 start, Int32 length, Boolean raiseChangeEvents)
        {
            BeginTrackingSelectionChanges();

            if (SelectionLength > 0)
                Select(caretPosition, 0);

            if (caretPosition > start)
            {
                caretBlinkTimer = 0;
                caretPosition = start;
            }

            var clampedLength = Math.Min(length, bufferText.Length - start);
            bufferText.Remove(start, clampedLength);
            OnCharacterDeleted(start, length, raiseChangeEvents);

            UpdateTextStringSource();
            UpdateTextParserStream();

            EndTrackingSelectionChanges();

            return true;
        }

        /// <summary>
        /// Deletes the text ahead of the caret.
        /// </summary>
        private Boolean DeleteAhead()
        {
            var result = false;

            BeginTrackingSelectionChanges();

            if (!DeleteSelection(true) && caretPosition < textLayoutStream.TotalSourceLength)
            {
                var isCRLF = IsStartOfCRLFOrSurrogatePair(caretPosition);

                var length = isCRLF ? 2 : 1;
                for (int i = 0; i < length; i++)
                {
                    bufferText.Remove(caretPosition, 1);
                    OnCharacterDeleted(caretPosition, 1, (i + 1) == length);
                }
                
                caretBlinkTimer = 0;

                UpdateTextStringSource();
                UpdateTextParserStream();

                result = true;
            }

            EndTrackingSelectionChanges();

            return result;
        }

        /// <summary>
        /// Deletes the text behind the caret.
        /// </summary>
        private Boolean DeleteBehind()
        {
            var result = false;

            BeginTrackingSelectionChanges();

            if (!DeleteSelection(true) && caretPosition > 0)
            {
                var isCRLF = IsEndOfCRLFOrSurrogatePair(caretPosition);

                var length = isCRLF ? 2 : 1;
                for (int i = 0; i < length; i++)
                {
                    caretPosition--;
                    caretBlinkTimer = 0;

                    bufferText.Remove(caretPosition, 1);
                    OnCharacterDeleted(caretPosition, 1, (i + 1) == length);
                }

                UpdateTextStringSource();
                UpdateTextParserStream();

                result = true;
            }

            EndTrackingSelectionChanges();

            return result;
        }

        /// <summary>
        /// Handles movement events which affect the selection.
        /// </summary>
        /// <param name="movementAllowed">A value indicating whether the user's desired movement is allowed.</param>
        /// <param name="movementDirection">A <see cref="CaretNavigationDirection"/> value specifying which direction the selection is moving.</param>
        /// <param name="moveSelection">A value indicating whether the selection is moving (if <see langword="true"/>) or 
        /// if the insertion position is moving (if <see langword="false"/>).</param>
        /// <returns><see langword="true"/> if the movement was handled; otherwise, <see langword="false"/>.</returns>
        private Boolean HandleSelectionMovement(Boolean movementAllowed, CaretNavigationDirection movementDirection, Boolean moveSelection)
        {
            if (moveSelection)
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

            selectionFollowingCursor = false;

            UpdateCaret();
            UpdateSelection();
        }

        /// <summary>
        /// Clears the internal text buffer.
        /// </summary>
        /// <param name="raiseChangeEvents">A value indicating whether events related to the text changing should be raised.</param>
        private void ClearBuffer(Boolean raiseChangeEvents)
        {
            var lengthDeleted = bufferText.Length;

            bufferText.Clear();

            if (lengthDeleted > 0)
            {
                OnCharacterDeleted(0, lengthDeleted, raiseChangeEvents);
            }
        }

        /// <summary>
        /// Appends the specified string to the end of the internal text buffer.
        /// </summary>
        private void AppendToBuffer(String str)
        {
            InsertIntoBuffer(new StringSegment(str), bufferText.Length);
        }

        /// <summary>
        /// Appends the specified string to the end of the internal text buffer.
        /// </summary>
        private void AppendToBuffer(StringBuilder str)
        {
            InsertIntoBuffer(new StringSegment(str), bufferText.Length);
        }

        /// <summary>
        /// Appends the specified string to the end of the internal text buffer.
        /// </summary>
        private void AppendToBuffer(VersionedStringSource str)
        {
            if (str.IsValid)
            {
                if (str.IsSourcedFromString)
                {
                    InsertIntoBuffer(new StringSegment((String)str), bufferText.Length);
                }
                else
                {
                    InsertIntoBuffer(new StringSegment((StringBuilder)(VersionedStringBuilder)str), bufferText.Length);
                }
            }
        }
        
        /// <summary>
        /// Inserts the specified string into the internal text buffer at the specified position.
        /// </summary>
        private void InsertIntoBuffer(String str, Int32 position)
        {
            InsertIntoBuffer(new StringSegment(str), position);
        }

        /// <summary>
        /// Inserts the specified string into the internal text buffer at the specified position.
        /// </summary>
        private void InsertIntoBuffer(StringBuilder str, Int32 position)
        {
            InsertIntoBuffer(new StringSegment(str), position);
        }

        /// <summary>
        /// Inserts the specified string into the internal text buffer at the specified position.
        /// </summary>
        private void InsertIntoBuffer(VersionedStringSource str, Int32 position)
        {
            if (str.IsValid)
            {
                if (str.IsSourcedFromString)
                {
                    InsertIntoBuffer(new StringSegment((String)str), position);
                }
                else
                {
                    InsertIntoBuffer(new StringSegment((StringBuilder)(VersionedStringBuilder)str), position);
                }
            }
        }

        /// <summary>
        /// Inserts the specified string segment into the text buffer at the specified position, performing masking if necessary.
        /// </summary>
        private void InsertIntoBuffer(StringSegment str, Int32 position)
        {
            var maskCharacter = MaskCharacter;
            for (int i = 0; i < str.Length; i++)
            {
                var charRequested = str[i];
                var charInserted = maskCharacter ?? charRequested;
                var finalCharacter = (i + 1 == str.Length);

                bufferText.Insert(position + i, charInserted);
                OnCharacterInserted(position + i, charRequested, charInserted, finalCharacter);
            }
        }
        
        /// <summary>
        /// Inserts text at the specified position in the text buffer.
        /// </summary>
        private void InsertTextAtPosition(StringSegment str, Int32 position, Boolean overwrite)
        {
            if (str.Length == 0)
                return;

            BeginTrackingSelectionChanges();

            var owner = TemplatedParent as Control;
            var acceptsReturn = (owner != null && owner.GetValue<Boolean>(TextBox.AcceptsReturnProperty));
            var acceptsTab = (owner != null && owner.GetValue<Boolean>(TextBox.AcceptsTabProperty));

            if (actualInsertionMode != CaretMode.Overwrite)
                overwrite = false;

            var selectionStart = SelectionStart;
            var selectionLength = SelectionLength;

            var selectionDeleted = DeleteSelection(false);
            if (selectionDeleted)
            {
                if (position > selectionStart)
                    position -= selectionLength;

                overwrite = false;
            }

            var mask = MaskCharacter;
            var characterCasing = (owner == null) ? CharacterCasing.Normal : owner.GetValue<CharacterCasing>(TextBox.CharacterCasingProperty);
            var characterCount = 0;

            var maxLength = (owner == null) ? 0 : owner.GetValue<Int32>(TextBox.MaxLengthProperty);

            for (int i = 0; i < str.Length; i++)
            {
                var character = str[i];
                if (character == '\r' && !acceptsReturn)
                    break;
                if (character == '\n' && !acceptsReturn)
                    break;
                if (character == '\t' && !acceptsTab)
                    character = ' ';

                switch (characterCasing)
                {
                    case CharacterCasing.Lower:
                        character = Char.ToLower(character);
                        break;

                    case CharacterCasing.Upper:
                        character = Char.ToUpper(character);
                        break;
                }

                if (!IsValidCharacterForEntry(position + characterCount, character))
                    continue;

                var charRequested = character;
                var charInserted = mask ?? character;
                var raiseChangeEvents = (i + 1 == str.Length);

                if (overwrite && position + characterCount < bufferText.Length)
                {
                    bufferText.Remove(position + characterCount, 1);
                    OnCharacterDeleted(position + characterCount, 1, false);
                }
                else
                {
                    if (maxLength > 0 && bufferText.Length >= maxLength)
                        break;
                }

                if (maxLength > 0 && bufferText.Length + 1 >= maxLength)
                    raiseChangeEvents = true;

                bufferText.Insert(position + characterCount, charInserted);
                OnCharacterInserted(position + characterCount, charRequested, charInserted, raiseChangeEvents);

                characterCount++;
            }
            
            caretBlinkTimer = 0;
            caretPosition = (position <= caretPosition) ? caretPosition + characterCount : caretPosition;
            caretPosition = AdjustCaretToAvoidInvalidPositions(caretPosition, true);

            pendingScrollToCaret = true;
            pendingScrollState = PendingScrollState.None;

            UpdateTextStringSource();
            UpdateTextParserStream();

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Inserts text at the current caret position.
        /// </summary>
        private void InsertTextAtCaret(StringSegment str, Boolean overwrite)
        {
            InsertTextAtPosition(str, caretPosition, overwrite);
        }

        /// <summary>
        /// Updates the <see cref="VersionedStringSource"/> instance which is exposed through the <see cref="TextBox.TextProperty"/> dependency property.
        /// </summary>
        private void UpdateTextStringSource()
        {
            var owner = TemplatedParent as TextBox;
            if (owner != null)
                owner.SetValue(TextBox.TextProperty, new VersionedStringSource(bufferText));
        }

        /// <summary>
        /// Calculates the position at which to draw the text caret.
        /// </summary>
        private void UpdateCaret()
        {
            if (View == null || pendingTextLayout)
                return;

            var owner = TemplatedParent as Control;

            var isReadOnly = (owner != null && owner.GetValue<Boolean>(TextBox.IsReadOnlyProperty));

            var fontFace = TextFontFace;
            var fontLineSpacing = (fontFace != null) ? fontFace.LineSpacing : 0;
            var fontLineSpacingHalf = (Int32)Math.Ceiling(fontLineSpacing / 2f);

            var styledCaretWidth = (Int32)Display.DipsToPixels(CaretWidth);
            var styledCaretThickness = (Int32)Display.DipsToPixels(CaretThickness);

            var caretAlignment = TextAlignment.Left;
            var caretRenderX = 0;
            var caretRenderY = 0;
            var caretRenderWidth = 0;
            var caretRenderHeight = 0;
            
            if (caretInsertionMode == CaretMode.Overwrite && !IsLineBreak(caretPosition) && !isReadOnly)
            {
                actualInsertionMode = CaretMode.Overwrite;

                var caretDefaultSize = GetDefaultSizeOfOverwriteCaret();
                var caretX = 0;
                var caretY = 0;
                var caretWidth = caretDefaultSize.Width;
                var caretHeight = caretDefaultSize.Height;
                
                if (textLayoutStream.TotalSourceLength > 0)
                {
                    var glyphLineInfo = default(LineInfo);
                    var glyphBounds = View.Resources.TextRenderer.GetGlyphBounds(textLayoutStream, caretPosition, out glyphLineInfo, true);

                    caretX = glyphBounds.Left;
                    caretY = glyphBounds.Top;

                    caretWidth = glyphBounds.Width;
                    caretBounds = new Ultraviolet.Rectangle(caretX, caretY, caretWidth, glyphBounds.Height);
                    caretLineIndex = glyphLineInfo.LineIndex;
                }
                else
                {
                    caretAlignment = GetActualTextAlignment();
                    caretBounds = new Ultraviolet.Rectangle(caretX, caretY, caretWidth, fontLineSpacing);
                    caretLineIndex = 0;
                }
                
                caretRenderWidth = caretWidth;
                caretRenderHeight = Math.Min(caretBounds.Height, styledCaretThickness);
                caretRenderY = caretBounds.Bottom - caretRenderHeight;
            }
            else
            {
                actualInsertionMode = CaretMode.Insert;

                var caretDefaultSize = GetDefaultSizeOfInsertionCaret();
                var caretX = 0;
                var caretY = 0;
                var caretWidth = caretDefaultSize.Width;
                var caretHeight = caretDefaultSize.Height;
                
                if (textLayoutStream.TotalSourceLength > 0)
                {
                    var caretPositionInGlyphs = View.Resources.TextRenderer.GetGlyphAtCharacterIndex(textLayoutStream, caretPosition);

                    var insertLineInfo = default(LineInfo);
                    var insertGlyphBounds = default(Ultraviolet.Rectangle?);
                    var insertBounds = View.Resources.TextRenderer.GetInsertionPointBounds(textLayoutStream,
                        caretPositionInGlyphs, out insertLineInfo, out insertGlyphBounds);

                    caretX = insertBounds.X;
                    caretY = insertBounds.Y;

                    caretWidth = (insertGlyphBounds.HasValue && insertGlyphBounds.Value.Width > 0) ? insertGlyphBounds.Value.Width : fontLineSpacingHalf;
                    caretHeight = insertBounds.Height;
                    caretBounds = new Ultraviolet.Rectangle(caretX, caretY, caretWidth, caretHeight);
                    caretLineIndex = insertLineInfo.LineIndex;                           
                }
                else
                {
                    caretAlignment = GetActualTextAlignment();
                    caretBounds = new Ultraviolet.Rectangle(caretX, caretY, caretWidth, caretHeight);
                    caretLineIndex = 0;
                }

                caretRenderWidth = Math.Min(caretBounds.Width, styledCaretWidth);
                caretRenderHeight = caretBounds.Height;
                caretRenderY = caretBounds.Top;
            }

            switch (caretAlignment)
            {
                case TextAlignment.Left:
                    caretRenderX = caretBounds.Left;
                    break;

                case TextAlignment.Center:
                    caretRenderX = caretBounds.Center.X - (caretRenderWidth / 2);
                    break;

                case TextAlignment.Right:
                    caretRenderX = (caretBounds.Right - caretRenderWidth);
                    break;
            }

            caretRenderBounds = new Ultraviolet.Rectangle(caretRenderX, caretRenderY, caretRenderWidth, caretRenderHeight);
        }

        /// <summary>
        /// Calculates the parameters with which to draw the selection.
        /// </summary>
        private void UpdateSelection()
        {
            if (View == null || textLayoutStream.Count == 0 || !selectionPosition.HasValue || pendingTextLayout)
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
                var selectionStartInGlyphs = View.Resources.TextRenderer.GetGlyphAtCharacterIndex(textLayoutStream, selectionStart);
                var selectionStartLineInfo = default(LineInfo);
                var selectionStartGlyphBounds = View.Resources.TextRenderer.GetGlyphBounds(textLayoutStream,
                    selectionStartInGlyphs, out selectionStartLineInfo, true);

                var selectionEnd = SelectionStart + (SelectionLength - 1);
                var selectionEndInGlyphs = View.Resources.TextRenderer.GetGlyphAtCharacterIndex(textLayoutStream, selectionEnd);
                var selectionEndLineInfo = default(LineInfo);
                var selectionEndGlyphBounds = View.Resources.TextRenderer.GetGlyphBounds(textLayoutStream,
                    selectionEndInGlyphs, out selectionEndLineInfo, true);

                selectionLineStart = selectionStartLineInfo.LineIndex;
                selectionLineCount = 1 + (selectionEndLineInfo.LineIndex - selectionStartLineInfo.LineIndex);

                if (IsRightToLeft())
                {
                    // Top
                    var selectionTopWidth = (selectionLineCount == 1) ? selectionStartGlyphBounds.Right - selectionEndGlyphBounds.Left :
                        selectionStartGlyphBounds.Right - selectionStartLineInfo.X;
                    var selectionTopHeight = selectionStartLineInfo.Height;
                    var selectionTopX = selectionStartGlyphBounds.Right - selectionTopWidth;
                    var selectionTopY = selectionStartGlyphBounds.Top;
                    selectionTop = new Ultraviolet.Rectangle(selectionTopX, selectionTopY, selectionTopWidth, selectionTopHeight);

                    // Bottom
                    if (selectionLineCount > 1)
                    {
                        var selectionBottomX = selectionEndGlyphBounds.Left;
                        var selectionBottomY = selectionEndGlyphBounds.Top;
                        var selectionBottomWidth = (selectionEndLineInfo.X + selectionEndLineInfo.Width) - selectionBottomX;
                        var selectionBottomHeight = selectionEndLineInfo.Height;
                        selectionBottom = new Ultraviolet.Rectangle(selectionBottomX, selectionBottomY, selectionBottomWidth, selectionBottomHeight);
                    }
                }
                else
                {
                    // Top
                    var selectionTopWidth = (selectionLineCount == 1) ? selectionEndGlyphBounds.Right - selectionStartGlyphBounds.Left :
                        selectionStartLineInfo.Width - (selectionStartGlyphBounds.Left - selectionStartLineInfo.X);
                    var selectionTopHeight = selectionStartLineInfo.Height;
                    var selectionTopX = selectionStartGlyphBounds.Left;
                    var selectionTopY = selectionStartGlyphBounds.Top;
                    selectionTop = new Ultraviolet.Rectangle(selectionTopX, selectionTopY, selectionTopWidth, selectionTopHeight);

                    // Bottom
                    if (selectionLineCount > 1)
                    {
                        var selectionBottomX = selectionEndLineInfo.X;
                        var selectionBottomY = selectionEndGlyphBounds.Top;
                        var selectionBottomWidth = selectionEndGlyphBounds.Right - selectionBottomX;
                        var selectionBottomHeight = selectionEndLineInfo.Height;
                        selectionBottom = new Ultraviolet.Rectangle(selectionBottomX, selectionBottomY, selectionBottomWidth, selectionBottomHeight);
                    }
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

        /// <summary>
        /// Scrolls to ensure that the caret is within the viewport.
        /// </summary>
        private void ScrollToCaret(Boolean showMaximumLineWidth, Boolean jumpLeft, Boolean jumpRight)
        {
            var scrollViewer = Parent as ScrollViewer;
            if (scrollViewer == null)
                return;

            if (pendingTextLayout)
            {
                pendingScrollToCaret = true;
                pendingScrollState = PendingScrollStateFromValues(showMaximumLineWidth, jumpLeft, jumpRight);
                return;
            }

            var boundsViewport = new RectangleD(scrollViewer.ContentHorizontalOffset, scrollViewer.ContentVerticalOffset, scrollViewer.ViewportWidth, scrollViewer.ViewportHeight); 
            var boundsCaretPixs = new Ultraviolet.Rectangle(caretBounds.X, caretBounds.Y, caretRenderBounds.Width, caretBounds.Height);
            var boundsCaretDips = Display.PixelsToDips(boundsCaretPixs);

            var isHorizontalScrollingNecessary = (boundsCaretDips.Left <= boundsViewport.Left || boundsCaretDips.Right >= boundsViewport.Right);
            var isVerticalScrollingNecessary = (boundsCaretDips.Top <= boundsViewport.Top || boundsCaretDips.Bottom >= boundsViewport.Bottom);

            if (!isHorizontalScrollingNecessary && !isVerticalScrollingNecessary)
                return;
            
            if (isVerticalScrollingNecessary)
            {
                if (boundsCaretDips.Top <= boundsViewport.Top)
                {
                    var verticalOffset = boundsCaretDips.Top;
                    scrollViewer.ScrollToVerticalOffset(verticalOffset);
                }
                else
                {
                    var verticalOffset = boundsCaretDips.Bottom - boundsViewport.Height;
                    scrollViewer.ScrollToVerticalOffset(verticalOffset);
                }
            }

            if (showMaximumLineWidth)
            {
                var owner = TemplatedParent as Control;
                var alignment = GetActualTextAlignment();
                var horizontalOffset = (alignment == TextAlignment.Right) ? boundsCaretDips.Left : boundsCaretDips.Right - boundsViewport.Width;
                scrollViewer.ScrollToHorizontalOffset(horizontalOffset);
            }
            else
            {
                if (isHorizontalScrollingNecessary)
                {
                    if (boundsCaretDips.Left <= boundsViewport.Left)
                    {
                        var horizontalOffset = boundsCaretDips.Left -
                            (jumpLeft ? (boundsViewport.Width / 3.0) : 0);

                        scrollViewer.ScrollToHorizontalOffset(horizontalOffset);
                    }
                    else
                    {
                        var horizontalOffset = (boundsCaretDips.Right - boundsViewport.Width) +
                            (jumpRight ? (boundsViewport.Width / 3.0) : 0);

                        scrollViewer.ScrollToHorizontalOffset(horizontalOffset);
                    }
                }
            }
        }

        /// <summary>
        /// Starts a new selection that tracks the specified cursor.
        /// </summary>
        private void BeginSelectionWithCursor(Int64 id, Point2D position)
        {
            BeginTrackingSelectionChanges();

            MoveCaretToCursor(position);

            selectionPosition = caretPosition;
            selectionFollowingCursor = true;
            selectionFollowingCursorID = id;
            UpdateSelectionAndCaret();

            EndTrackingSelectionChanges();
        }

        /// <summary>
        /// Begins tracking changes to the selection state.
        /// </summary>
        private void BeginTrackingSelectionChanges()
        {
            if (++selectionTrackingCounter == 1)
            {
                storedCaretPosition = caretPosition;
                storedSelectionPosition = selectionPosition ?? caretPosition;
            }
        }

        /// <summary>
        /// Finishes tracking changes to the selection state and raises a <see cref="Primitives.TextBoxBase.SelectionChangedEvent"/> 
        /// routed event if necessary,
        /// </summary>
        private void EndTrackingSelectionChanges()
        {
            if (--selectionTrackingCounter < 0)
                throw new InvalidOperationException();

            if (selectionTrackingCounter == 0)
            {
                if (caretPosition != storedCaretPosition || (selectionPosition ?? caretPosition) != storedSelectionPosition)
                    RaiseSelectionChanged();

                storedCaretPosition = 0;
                storedSelectionPosition = 0;
            }
        }

        /// <summary>
        /// Raises the <see cref="Primitives.TextBoxBase.SelectionChangedEvent"/> routed event.
        /// </summary>
        private void RaiseSelectionChanged()
        {
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(TextBoxBase.SelectionChangedEvent);
            var evtData = RoutedEventData.Retrieve(this);
            evtDelegate(this, evtData);
        }

        /// <summary>
        /// Gets a value indicating whether the caret is currently positioned on a line break.
        /// </summary>
        private Boolean IsLineBreak(Int32 position, Boolean includeEndOfText = true)
        {
            if (position == bufferText.Length)
                return includeEndOfText;

            var character = bufferText[position];
            return character == '\r' || character == '\n';
        }

        /// <summary>
        /// Gets a value indicating whether the specified position in the text buffer is the 
        /// start of a carriage return - line feed (CRLF) sequence, or a surrogate pair.
        /// </summary>
        private Boolean IsStartOfCRLFOrSurrogatePair(Int32 position)
        {
            if (position + 2 > bufferText.Length)
                return false;

            var isSurrogate = Char.IsSurrogatePair(bufferText[position], bufferText[position + 1]);
            if (isSurrogate)
                return true;

            var isCRLF = bufferText[position] == '\r' && bufferText[position + 1] == '\n';
            if (isCRLF)
                return true;

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the specified position in the text buffer is the 
        /// end of a carriage return - line feed (CRLF) sequence, or a surrogate pair.
        /// </summary>
        private Boolean IsEndOfCRLFOrSurrogatePair(Int32 position)
        {
            if (position - 2 < 0)
                return false;

            var isSurrogate = Char.IsSurrogatePair(bufferText[position - 2], bufferText[position - 1]);
            if (isSurrogate)
                return true;

            var isCRLF = bufferText[position - 1] == '\n' && bufferText[position - 2] == '\r';
            if (isCRLF)
                return true;

            return false;
        }

        /// <summary>
        /// Gets the editor's actual text wrapping mode.
        /// </summary>
        private TextWrapping GetActualTextWrapping()
        {
            var dobj = (TemplatedParent ?? this) as FrameworkElement;
            if (dobj == null)
                return TextWrapping.Wrap;

            return dobj.GetValue<TextWrapping>(TextBox.TextWrappingProperty);
        }

        /// <summary>
        /// Gets the text editor's actual text alignment, which depends on both the value of
        /// the <see cref="TextAlignment"/> property and the <see cref="FlowDirection"/> property.
        /// </summary>
        private TextAlignment GetActualTextAlignment()
        {
            var dobj = (TemplatedParent ?? this) as FrameworkElement;
            if (dobj == null)
                return TextAlignment.Left;

            var align = dobj.GetValue<TextAlignment>(TextBox.TextAlignmentProperty);
            if (dobj.FlowDirection == FlowDirection.RightToLeft)
            {
                switch (align)
                {
                    case TextAlignment.Left:
                        align = TextAlignment.Right;
                        break;

                    case TextAlignment.Right:
                        align = TextAlignment.Left;
                        break;
                }
            }
            return align;
        }

        // State values.
        private readonly TextParserTokenStream textParserStream = new TextParserTokenStream();
        private readonly TextLayoutCommandStream textLayoutStream = new TextLayoutCommandStream();
        private Int32 textOffsetX;
        private Int32 textOffsetY;
        private Boolean pendingTextLayout = true;
        private Boolean pendingScrollToCaret;
        private PendingScrollState pendingScrollState;

        // Caret parameters.
        private Double caretBlinkTimer;
        private Int32 caretPosition;
        private Int32 caretLineIndex;
        private Ultraviolet.Rectangle caretBounds;
        private Ultraviolet.Rectangle caretRenderBounds;
        private CaretMode caretInsertionMode = CaretMode.Insert;
        private CaretMode actualInsertionMode = CaretMode.Insert;

        // Selection parameters.
        private Int32? selectionPosition;
        private Int32 selectionLineStart;
        private Int32 selectionLineCount;
        private Int64 selectionFollowingCursorID;
        private Boolean selectionFollowingCursor;
        private Ultraviolet.Rectangle selectionTop;
        private Ultraviolet.Rectangle selectionBottom;

        // Cached values for selection change tracking.
        private Int32 selectionTrackingCounter;
        private Int32 storedCaretPosition;
        private Int32 storedSelectionPosition;

        // The editor's internal text buffer.
        private readonly StringBuilder bufferInput = new StringBuilder();
        private readonly VersionedStringBuilder bufferText = new VersionedStringBuilder();
    }
}
