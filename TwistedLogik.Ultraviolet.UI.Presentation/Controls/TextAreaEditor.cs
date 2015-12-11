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
    public partial class TextAreaEditor : FrameworkElement
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

            InsertTextAtCaret(text, false);
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
                InsertTextAtCaret(value, false);
                var caretPositionNew = caretPosition;

                var selectionStart = caretPositionOld;
                var selectionLength = caretPositionNew - caretPositionOld;

                Select(selectionStart, selectionLength);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the caret is drawn on top of the text while the caret is in insertion mode.
        /// </summary>
        public Boolean CaretInsertTopmost
        {
            get { return GetValue<Boolean>(CaretInsertTopmostProperty); }
            set { SetValue(CaretInsertTopmostProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the caret is drawn on top of the text while the caret is in insertion mode.
        /// </summary>
        public Boolean CaretOverwriteTopmost
        {
            get { return GetValue<Boolean>(CaretOverwriteTopmostProperty); }
            set { SetValue(CaretOverwriteTopmostProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image used to draw the caret while the caret is in insertion mode.
        /// </summary>
        public SourcedImage CaretInsertImage
        {
            get { return GetValue<SourcedImage>(CaretInsertImageProperty); }
            set { SetValue(CaretInsertImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image used to draw the caret while the caret is in overwrite mode.
        /// </summary>
        public SourcedImage CaretOverwriteImage
        {
            get { return GetValue<SourcedImage>(CaretInsertImageProperty); }
            set { SetValue(CaretInsertImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the caret is drawn while the caret is in insertion mode.
        /// </summary>
        public Color CaretInsertColor
        {
            get { return GetValue<Color>(CaretInsertColorProperty); }
            set { SetValue(CaretInsertColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the caret is drawn while the caret is in overwrite mode.
        /// </summary>
        public Color CaretOverwriteColor
        {
            get { return GetValue<Color>(CaretOverwriteColorProperty); }
            set { SetValue(CaretOverwriteColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the caret's width when it is in insertion mode, specified in device in independent pixels.
        /// </summary>
        /// <remarks>The caret will never be wider than the glyph at which it is positioned, regardless of this value.</remarks>
        public Double CaretWidth
        {
            get { return GetValue<Double>(CaretWidthProperty); }
            set { SetValue(CaretWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the cart's thickness when it is in overwrite mode, specified in device independent pixels.
        /// </summary>
        /// <remarks>The caret will never be taller than the glyph at which it is positioned, regardless of this value.</remarks>
        public Double CaretThickness
        {
            get { return GetValue<Double>(CaretThicknessProperty); }
            set { SetValue(CaretThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image used to draw the selection highlight.
        /// </summary>
        public SourcedImage SelectionImage
        {
            get { return GetValue<SourcedImage>(SelectionImageProperty); }
            set { SetValue(SelectionImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the selection highlight is drawn.
        /// </summary>
        public Color SelectionColor
        {
            get { return GetValue<Color>(SelectionColorProperty); }
            set { SetValue(SelectionColorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="CaretInsertTopmost"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CaretInsertTopmostProperty = DependencyProperty.Register("CaretInsertTopmost", typeof(Boolean), typeof(TextAreaEditor),
            new PropertyMetadata<Boolean>(true, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CaretOverwriteTopmost"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CaretOverwriteTopmostProperty = DependencyProperty.Register("CaretOverwriteTopmost", typeof(Boolean), typeof(TextAreaEditor),
            new PropertyMetadata<Boolean>(true, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CaretInsertImage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CaretInsertImageProperty = DependencyProperty.Register("CaretInsertImage", typeof(SourcedImage), typeof(TextAreaEditor),
            new PropertyMetadata<SourcedImage>(null, PropertyMetadataOptions.None, HandleCaretInsertImageChanged));

        /// <summary>
        /// Identifies the <see cref="CaretOverwriteImage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CaretOverwriteImageProperty = DependencyProperty.Register("CaretOverwriteImage", typeof(SourcedImage), typeof(TextAreaEditor),
            new PropertyMetadata<SourcedImage>(null, PropertyMetadataOptions.None, HandleCaretOverwriteImageChanged));

        /// <summary>
        /// Identifies the <see cref="CaretInsertColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CaretInsertColorProperty = DependencyProperty.Register("CaretInsertColor", typeof(Color), typeof(TextAreaEditor),
            new PropertyMetadata<Color>(Color.White, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CaretOverwriteColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CaretOverwriteColorProperty = DependencyProperty.Register("CaretOverwriteColor", typeof(Color), typeof(TextAreaEditor),
            new PropertyMetadata<Color>(Color.White, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CaretWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CaretWidthProperty = DependencyProperty.Register("CaretWidth", typeof(Double), typeof(TextAreaEditor),
            new PropertyMetadata<Double>(1.0, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CaretThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CaretThicknessProperty = DependencyProperty.Register("CaretThickness", typeof(Double), typeof(TextAreaEditor),
            new PropertyMetadata<Double>(4.0, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="SelectionImage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectionImageProperty = DependencyProperty.Register("SelectionImage", typeof(SourcedImage), typeof(TextAreaEditor),
            new PropertyMetadata<SourcedImage>(null, PropertyMetadataOptions.None, HandleSelectionImageChanged));

        /// <summary>
        /// Identifies the <see cref="SelectionColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectionColorProperty = DependencyProperty.Register("SelectionColor", typeof(Color), typeof(TextAreaEditor),
            new PropertyMetadata<Color>(Color.Blue * 0.4f, PropertyMetadataOptions.None));

        /// <summary>
        /// Called when the value of the <see cref="TextArea.TextProperty"/> dependency property changes.
        /// </summary>
        /// <param name="value">The new value of the dependency property.</param>
        internal void HandleTextChanged(VersionedStringSource value)
        {
            if (value.IsSourcedFromStringBuilder)
            {
                var vsb = (VersionedStringBuilder)value;
                if (vsb.Version == bufferText.Version)
                    return;
            }

            bufferText.Clear();
            bufferText.Append(value);

            if (caretPosition > bufferText.Length)
            {
                caretBlinkTimer = 0;
                caretPosition = bufferText.Length;

                pendingScrollToCaret = true;                
            }
            
            UpdateTextStringSource();
            UpdateTextParserStream();
        }

        /// <summary>
        /// Called when the text area's text is changed by the <see cref="TextArea.SetText(StringBuilder)"/> method.
        /// </summary>
        /// <param name="value">The <see cref="StringBuilder"/> that contains the text area's new text.</param>
        internal void HandleTextChanged(StringBuilder value)
        {
            selectionPosition = null;

            bufferText.Length = 0;
            bufferText.Append(value);

            UpdateTextStringSource();
            UpdateTextParserStream();

            if (caretPosition > value.Length)
            {
                caretPosition = value.Length;
                caretBlinkTimer = 0;

                UpdateSelectionAndCaret();
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
                ScrollToCaret(true, false, false);
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
                        InsertTextAtCaret("\n", false);
                        data.Handled = true;
                    }
                    break;

                case Key.Tab:
                    if (owner != null && owner.AcceptsTab)
                    {
                        InsertTextAtCaret("\t", true);
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
                    MoveCaretInDirection(CaretNavigationDirection.Left, modifiers);
                    data.Handled = true;
                    break;

                case Key.Right:
                    MoveCaretInDirection(CaretNavigationDirection.Right, modifiers);
                    data.Handled = true;
                    break;

                case Key.Up:
                    MoveCaretInDirection(CaretNavigationDirection.Up, modifiers);
                    data.Handled = true;
                    break;

                case Key.Down:
                    MoveCaretInDirection(CaretNavigationDirection.Down, modifiers);
                    data.Handled = true;
                    break;

                case Key.Home:
                    MoveCaretInDirection(CaretNavigationDirection.Home, modifiers);
                    data.Handled = true;
                    break;

                case Key.End:
                    MoveCaretInDirection(CaretNavigationDirection.End, modifiers);
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

            InsertTextAtCaret((StringSegment)bufferInput, true);

            data.Handled = true;
        }

        /// <summary>
        /// Gets or sets the editor's insertion mode.
        /// </summary>
        internal TextBoxInsertionMode InsertionMode
        {
            get { return insertionMode; }
            set
            {
                if (insertionMode != value)
                {
                    insertionMode = value;
                    caretBlinkTimer = 0;
                    UpdateCaret();
                }
            }
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            UpdateTextLayoutStream(availableSize);

            var owner = TemplatedParent as TextArea;
            var ownerFont = (owner == null || !owner.Font.IsLoaded) ? null : owner.Font.Resource.Value;
            var ownerFontLineSpacing = (ownerFont == null) ? 0 : ownerFont.GetFace(SpriteFontStyle.Regular).LineSpacing;
            var ownerFontLineSpacingHalf = (Int32)Math.Ceiling(ownerFontLineSpacing / 2f);

            var widthOfInsertionCaret = Math.Min((Int32)Display.DipsToPixels(CaretWidth), ownerFontLineSpacingHalf);
            var width = Display.PixelsToDips(textLayoutStream.ActualWidth);
            width = Math.Max(width, Display.PixelsToDips(caretBounds.Width));
            width = width + Display.PixelsToDips(widthOfInsertionCaret);

            var height = Display.PixelsToDips(textLayoutStream.ActualHeight);
            height = Math.Max(height, Display.PixelsToDips(caretBounds.Height));

            return new Size2D(width, height);
        }
        
        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            return finalSize;
        }

        /// <inheritdoc/>
        protected override void PositionOverride()
        {
            if (pendingScrollToCaret)
            {
                pendingScrollToCaret = false;
                ScrollToCaret(false, false, false);
            }
            base.PositionOverride();
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            DrawSelection(time, dc);

            var caretTopmost = 
                (caretInsertionMode == TextBoxInsertionMode.Insert && CaretInsertTopmost) ||
                (caretInsertionMode == TextBoxInsertionMode.Overwrite && CaretOverwriteTopmost);

            if (caretTopmost)
            {
                DrawText(time, dc);
                DrawCaret(time, dc);
            }
            else
            {
                DrawCaret(time, dc);
                DrawText(time, dc);
            }

            base.DrawOverride(time, dc);
        }
        
        /// <inheritdoc/>
        protected override void ReloadContentCore(Boolean recursive)
        {
            ReloadCaretInsertImage();
            ReloadCaretOverwriteImage();
            ReloadSelectionImage();

            if (textLayoutStream.Count > 0)
                UpdateTextLayoutStream(MostRecentAvailableSize);

            UpdateSelectionAndCaret();

            base.ReloadContentCore(recursive);
        }

        /// <summary>
        /// Reloads the <see cref="CaretInsertImage"/> resource.
        /// </summary>
        protected void ReloadCaretInsertImage()
        {
            LoadImage(CaretInsertImage);
        }

        /// <summary>
        /// Reloads the <see cref="CaretOverwriteImage"/> resource.
        /// </summary>
        protected void ReloadCaretOverwriteImage()
        {
            LoadImage(CaretOverwriteImage);
        }

        /// <summary>
        /// Reloads the <see cref="SelectionImage"/> resource.
        /// </summary>
        protected void ReloadSelectionImage()
        {
            LoadImage(SelectionImage);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CaretInsertImage"/> dependency property changes.
        /// </summary>
        private static void HandleCaretInsertImageChanged(DependencyObject dobj, SourcedImage oldValue, SourcedImage newValue)
        {
            var textAreaEditor = (TextAreaEditor)dobj;
            textAreaEditor.ReloadCaretInsertImage();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CaretOverwriteImage"/> dependency property changes.
        /// </summary>
        private static void HandleCaretOverwriteImageChanged(DependencyObject dobj, SourcedImage oldValue, SourcedImage newValue)
        {
            var textAreaEditor = (TextAreaEditor)dobj;
            textAreaEditor.ReloadCaretOverwriteImage();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionImage"/> dependency property changes.
        /// </summary>
        private static void HandleSelectionImageChanged(DependencyObject dobj, SourcedImage oldValue, SourcedImage newValue)
        {
            var textAreaEditor = (TextAreaEditor)dobj;
            textAreaEditor.ReloadSelectionImage();
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

            View.Resources.TextRenderer.Parse((StringBuilder)bufferText, textParserStream, TextParserOptions.IgnoreCommandCodes);
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
            DrawImage(dc, SelectionImage, selectionTopDips, SelectionColor, true);

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
                    DrawImage(dc, SelectionImage, lineBoundsDips, SelectionColor, true);
                }

                textLayoutStream.ReleasePointers();
            }

            // Draw the last line
            if (selectionLineCount > 1)
            {
                var selectionBottomDips = Display.PixelsToDips(selectionBottom);
                DrawImage(dc, SelectionImage, selectionBottomDips, SelectionColor, true);
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
                var caretBoundsDips = Display.PixelsToDips(caretRenderBounds);
                var caretImage = (caretInsertionMode == TextBoxInsertionMode.Insert) ? CaretInsertImage : CaretOverwriteImage;
                var caretColor = (caretInsertionMode == TextBoxInsertionMode.Insert) ? CaretInsertColor : CaretOverwriteColor;
				DrawImage(dc, caretImage, caretBoundsDips, caretColor, true);
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

            ScrollToCaret(true, false, false);
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

                ScrollToCaret(false, true, false);
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

                ScrollToCaret(false, false, true);
            }
        }

        /// <summary>
        /// Moves the caret up.
        /// </summary>
        private void MoveCaretUp()
        {
            var x = caretBounds.Left;
            var y = caretBounds.Top - 1;
            
            var movementAllowed = (textLayoutStream.Count > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.Up))
            {
                caretBlinkTimer = 0;
                caretPosition = View.Resources.TextRenderer.GetInsertionPointAtPosition(textLayoutStream, x, y);
                UpdateSelectionAndCaret();

                ScrollToCaret(true, false, false);
            }
        }

        /// <summary>
        /// Moves the caret down.
        /// </summary>
        private void MoveCaretDown()
        {
            var x = caretBounds.Left;
            var y = caretBounds.Bottom;
            
            var movementAllowed = (textLayoutStream.Count > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.Down))
            {
                caretBlinkTimer = 0;
                caretPosition = View.Resources.TextRenderer.GetInsertionPointAtPosition(textLayoutStream, x, y);
                UpdateSelectionAndCaret();

                ScrollToCaret(true, false, false);               
            }
        }

        /// <summary>
        /// Moves the caret to the beginning of the current line.
        /// </summary>
        private void MoveCaretToHome(Boolean moveToBeginningOfText)
        {
            var movementAllowed = (caretPosition > 0 && textLayoutStream.TotalLength > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.Home))
            {
                if (moveToBeginningOfText)
                {
                    caretPosition = 0;
                }
                else
                {
                    var lineInfo = textLayoutStream.GetLineInfo(caretLineIndex);
                    caretPosition = lineInfo.OffsetInGlyphs;
                }

                caretBlinkTimer = 0;
                UpdateSelectionAndCaret();

                ScrollToCaret(true, false, false);
            }
        }

        /// <summary>
        /// Moves the caret to the end of the current line.
        /// </summary>
        private void MoveCaretToEnd(Boolean moveToEndOfText)
        {
            var movementAllowed = (caretPosition < textLayoutStream.TotalLength && textLayoutStream.TotalLength > 0);
            if (!HandleSelectionMovement(movementAllowed, CaretNavigationDirection.End))
            {
                if (moveToEndOfText)
                {
                    caretPosition = textLayoutStream.TotalLength;
                }
                else
                {
                    var lineInfo = textLayoutStream.GetLineInfo(caretLineIndex);
                    caretPosition = lineInfo.OffsetInGlyphs + lineInfo.LengthInGlyphs;
                    if (caretPosition > 0 && bufferText[caretPosition - 1] == '\n')
                        caretPosition--;
                }

                caretBlinkTimer = 0;
                UpdateSelectionAndCaret();

                ScrollToCaret(true, false, false);
            }
        }

        /// <summary>
        /// Moves the caret in the specified direction.
        /// </summary>
        private void MoveCaretInDirection(CaretNavigationDirection direction, ModifierKeys modifiers = ModifierKeys.None)
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
                    MoveCaretToHome((modifiers & ModifierKeys.Control) == ModifierKeys.Control);
                    break;

                case CaretNavigationDirection.End:
                    MoveCaretToEnd((modifiers & ModifierKeys.Control) == ModifierKeys.Control);
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

            UpdateTextStringSource();
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

                UpdateTextStringSource();
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

                UpdateTextStringSource();
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
        private void InsertTextAtPosition(StringSegment str, Int32 position, Boolean overwrite)
        {
            var owner = TemplatedParent as TextArea;
            var acceptsReturn = (owner != null && owner.AcceptsReturn);
            var acceptsTab = (owner != null && owner.AcceptsTab);

            if (caretInsertionMode != TextBoxInsertionMode.Overwrite)
                overwrite = false;

            var selectionStart = SelectionStart;
            var selectionLength = SelectionLength;

            var selectionDeleted = DeleteSelection();
            if (selectionDeleted)
            {
                if (position > selectionStart)
                    position -= selectionLength;

                overwrite = false;
            }            

            var charactersInserted = 0;

            for (int i = 0; i < str.Length; i++)
            {
                var character = str[i];
                if (character == '\r')
                    continue;
                if (character == '\n' && !acceptsReturn)
                    break;
                if (character == '\t' && !acceptsTab)
                    character = ' ';

                if (overwrite && position + i < bufferText.Length)
                    bufferText.Remove(position + i, 1);

                bufferText.Insert(position + i, character);
                charactersInserted++;
            }
            
            caretBlinkTimer = 0;
            caretPosition = (position <= caretPosition) ? caretPosition + charactersInserted : caretPosition;

            pendingScrollToCaret = true;

            UpdateTextStringSource();
            UpdateTextParserStream();
        }

        /// <summary>
        /// Inserts text at the current caret position.
        /// </summary>
        private void InsertTextAtCaret(StringSegment str, Boolean overwrite)
        {
            InsertTextAtPosition(str, caretPosition, overwrite);
        }

        /// <summary>
        /// Updates the <see cref="VersionedStringSource"/> instance which is exposed through the <see cref="TextArea.TextProperty"/> dependency property.
        /// </summary>
        private void UpdateTextStringSource()
        {
            var owner = TemplatedParent as TextArea;
            if (owner == null)
                return;

            owner.SetValue(TextArea.TextProperty, new VersionedStringSource(bufferText));
        }

        /// <summary>
        /// Calculates the position at which to draw the text caret.
        /// </summary>
        private void UpdateCaret()
        {
            var owner = TemplatedParent as TextArea;

            var font = (owner != null && owner.Font.IsLoaded) ? owner.Font.Resource.Value.GetFace(SpriteFontStyle.Regular) : null;
            var fontLineSpacing = (font != null) ? font.LineSpacing : 0;
            var fontLineSpacingHalf = (Int32)Math.Ceiling(fontLineSpacing / 2f);

            var styledCaretWidth = (Int32)Display.DipsToPixels(CaretWidth);
            var styledCaretThickness = (Int32)Display.DipsToPixels(CaretThickness);

            if (InsertionMode == TextBoxInsertionMode.Overwrite && !IsCaretOnLineBreak())
            {
                caretInsertionMode = TextBoxInsertionMode.Overwrite;

                var caretX = 0;
                var caretY = 0;
                var caretWidth = fontLineSpacingHalf;
                var caretHeight = styledCaretThickness;

                if (textLayoutStream.TotalLength > 0)
                {
                    var glyphLineIndex = 0;
                    var glyphLineWidth = 0;
                    var glyphLineHeight = 0;
                    var glyphBounds = View.Resources.TextRenderer.GetGlyphBounds(textLayoutStream,
                        caretPosition, out glyphLineIndex, out glyphLineWidth, out glyphLineHeight, true);

                    caretX = glyphBounds.Left;
                    caretY = glyphBounds.Top;
                    caretWidth = glyphBounds.Width;
                    caretBounds = new Ultraviolet.Rectangle(caretX, caretY, caretWidth, glyphBounds.Height);
                    caretLineIndex = glyphLineIndex;
                }
                else
                {
                    caretBounds = new Ultraviolet.Rectangle(caretX, caretY, caretWidth, fontLineSpacing);
                    caretLineIndex = 0;
                }

                caretRenderBounds = new Ultraviolet.Rectangle(caretBounds.Left, 
                    Math.Max(caretBounds.Top, caretBounds.Bottom - caretHeight), caretWidth, 
                    Math.Min(caretBounds.Height, caretHeight));
            }
            else
            {
                caretInsertionMode = TextBoxInsertionMode.Insert;

                var caretX = 0;
                var caretY = 0;
                var caretWidth = fontLineSpacingHalf;
                var caretHeight = fontLineSpacing;

                if (textLayoutStream.TotalLength > 0)
                {
                    var caretWidthPx = (Int32)Display.DipsToPixels(CaretWidth);

                    var lineIndex = 0;
                    var lineWidth = 0;
                    var lineHeight = 0;

                    var boundsGlyph = default(Ultraviolet.Rectangle?);
                    var boundsInsert = View.Resources.TextRenderer.GetInsertionPointBounds(textLayoutStream, caretPosition,
                        out lineIndex, out lineWidth, out lineHeight, out boundsGlyph);

                    caretX = boundsInsert.X;
                    caretY = boundsInsert.Y;
                    caretWidth = (boundsGlyph.HasValue && boundsGlyph.Value.Width > 0) ? boundsGlyph.Value.Width : fontLineSpacingHalf;
                    caretHeight = boundsInsert.Height;
                    caretBounds = new Ultraviolet.Rectangle(caretX, caretY, caretWidth, caretHeight);
                    caretLineIndex = lineIndex;                    
                }
                else
                {
                    caretBounds = new Ultraviolet.Rectangle(caretX, caretY, caretWidth, caretHeight);
                    caretLineIndex = 0;
                }

                caretRenderBounds = new Ultraviolet.Rectangle(caretBounds.Left, caretBounds.Top,
                    Math.Min(caretBounds.Width, styledCaretWidth), caretBounds.Height);
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

        /// <summary>
        /// Scrolls to ensure that the caret is within the viewport.
        /// </summary>
        private void ScrollToCaret(Boolean showMaximumLineWidth, Boolean jumpLeft, Boolean jumpRight)
        {
            var scrollViewer = Parent as ScrollViewer;
            if (scrollViewer == null)
                return;

            var boundsViewport = new RectangleD(scrollViewer.HorizontalOffset, scrollViewer.VerticalOffset, 
                scrollViewer.ViewportWidth, scrollViewer.ViewportHeight);
            var boundsCaret = Display.PixelsToDips(caretRenderBounds);

            var isHorizontalScrollingNecessary = (boundsCaret.Left < boundsViewport.Left || boundsCaret.Right > boundsViewport.Right);
            var isVerticalScrollingNecessary = (boundsCaret.Top < boundsViewport.Top || boundsCaret.Bottom > boundsViewport.Bottom);

            if (!isHorizontalScrollingNecessary && !isVerticalScrollingNecessary)
                return;
            
            if (isVerticalScrollingNecessary)
            {
                if (boundsCaret.Top < boundsViewport.Top)
                {
                    var verticalOffset = boundsCaret.Top;
                    scrollViewer.ScrollToVerticalOffset(verticalOffset);
                }
                else
                {
                    var verticalOffset = (boundsCaret.Bottom - boundsViewport.Height);
                    scrollViewer.ScrollToVerticalOffset(verticalOffset);
                }
            }

            if (showMaximumLineWidth)
            {
                var horizontalOffset = boundsCaret.Right - boundsViewport.Width;
                scrollViewer.ScrollToHorizontalOffset(horizontalOffset);
            }
            else
            {
                if (isHorizontalScrollingNecessary)
                {
                    if (boundsCaret.Left < boundsViewport.Left)
                    {
                        var horizontalOffset = boundsCaret.Left -
                            (jumpLeft ? (boundsViewport.Width / 3.0) : 0);

                        scrollViewer.ScrollToHorizontalOffset(horizontalOffset);
                    }
                    else
                    {
                        var horizontalOffset = (boundsCaret.Right - boundsViewport.Width) +
                            (jumpRight ? (boundsViewport.Width / 3.0) : 0);

                        scrollViewer.ScrollToHorizontalOffset(horizontalOffset);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the caret is currently positioned on a line break.
        /// </summary>
        private Boolean IsCaretOnLineBreak()
        {
            return caretPosition == bufferText.Length || bufferText[caretPosition] == '\n';
        }

        // Property values.
        private TextBoxInsertionMode insertionMode = TextBoxInsertionMode.Insert;

        // State values.
        private readonly TextParserTokenStream textParserStream = new TextParserTokenStream();
        private readonly TextLayoutCommandStream textLayoutStream = new TextLayoutCommandStream();
        private Boolean pendingScrollToCaret;

        // Caret parameters.
        private Double caretBlinkTimer;
        private Int32 caretPosition;
        private Int32 caretLineIndex;
        private Ultraviolet.Rectangle caretBounds;
        private Ultraviolet.Rectangle caretRenderBounds;
        private TextBoxInsertionMode caretInsertionMode = TextBoxInsertionMode.Insert;

        // Selection parameters.
        private Int32? selectionPosition;
        private Int32 selectionLineStart;
        private Int32 selectionLineCount;
        private Ultraviolet.Rectangle selectionTop;
        private Ultraviolet.Rectangle selectionBottom;
        private Boolean selectionFollowingMouse;

        // The editor's internal text buffer.
        private readonly StringBuilder bufferInput = new StringBuilder();
        private readonly VersionedStringBuilder bufferText = new VersionedStringBuilder();
    }
}
