using System;
using System.Text;
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
    public class TextAreaEditor : Control
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
        /// Called when the editor should read text input from the keyboard device.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void ProcessTextInput(KeyboardDevice device, ref RoutedEventData data)
        {
            device.GetTextInput(bufferInput);

            bufferText.Insert(caretPosition, bufferInput);
            caretBlinkTimer = 0;
            caretPosition += bufferInput.Length;

            UpdateTextParserStream();

            data.Handled = true;
        }

        /// <summary>
        /// Called when the <see cref="TextArea"/> gains keyboard focus.
        /// </summary>
        internal void ProcessGotKeyboardFocus()
        {
            caretBlinkTimer = 0;
            UpdateCaretBounds();
        }

        /// <summary>
        /// Called when the <see cref="TextArea"/> loses keyboard focus.
        /// </summary>
        internal void ProcessLostKeyboardFocus()
        {

        }

        /// <summary>
        /// Called when the user presses the <see cref="Key.Return"/> key.
        /// </summary>
        internal void ProcessReturn()
        {
            bufferText.Insert(caretPosition, '\n');
            caretBlinkTimer = 0;
            caretPosition += 1;

            UpdateTextParserStream();
        }

        /// <summary>
        /// Called when the user presses the <see cref="Key.Tab"/> key.
        /// </summary>
        internal void ProcessTab()
        {
            bufferText.Insert(caretPosition, '\t');
            caretBlinkTimer = 0;
            caretPosition += 1;

            UpdateTextParserStream();
        }

        /// <summary>
        /// Called when the user presses the <see cref="Key.Backspace"/> key.
        /// </summary>
        internal void ProcessBackspace()
        {
            if (caretPosition == 0)
                return;

            bufferText.Remove(caretPosition - 1, 1);
            caretBlinkTimer = 0;
            caretPosition -= 1;

            UpdateTextParserStream();
        }

        /// <summary>
        /// Called when the user presses the <see cref="Key.Delete"/> key.
        /// </summary>
        internal void ProcessDelete()
        {
            if (caretPosition == textLayoutStream.TotalLength)
                return;

            bufferText.Remove(caretPosition, 1);
            caretBlinkTimer = 0;

            UpdateTextParserStream();
        }

        /// <summary>
        /// Called when the user presses the <see cref="Key.Left"/> key.
        /// </summary>
        internal void ProcessLeft()
        {
            if (caretPosition == 0)
                return;

            caretBlinkTimer = 0;
            caretPosition -= 1;

            UpdateCaretBounds();
        }

        /// <summary>
        /// Called when the user presses the <see cref="Key.Right"/> key.
        /// </summary>
        internal void ProcessRight()
        {
            if (caretPosition == textLayoutStream.TotalLength)
                return;

            caretBlinkTimer = 0;
            caretPosition += 1;

            UpdateCaretBounds();
        }

        /// <summary>
        /// Called when the user presses the <see cref="Key.Up"/> key.
        /// </summary>
        internal void ProcessUp()
        {
            if (textLayoutStream.Count == 0)
                return;

            var x = caretX;
            var y = caretY - 1;

            if (y < 0)
                return;

            caretBlinkTimer = 0;
            caretPosition = View.Resources.TextRenderer.GetInsertionPointAtPosition(textLayoutStream, x, y);

            UpdateCaretBounds();
        }

        /// <summary>
        /// Called when the user presses the <see cref="Key.Down"/> key.
        /// </summary>
        internal void ProcessDown()
        {
            if (textLayoutStream.Count == 0)
                return;

            var x = caretX;
            var y = caretY + caretHeight;

            if (y >= textLayoutStream.ActualHeight)
                return;

            caretBlinkTimer = 0;
            caretPosition = View.Resources.TextRenderer.GetInsertionPointAtPosition(textLayoutStream, x, y);

            UpdateCaretBounds();
        }

        /// <summary>
        /// Called when the user presses the <see cref="Key.Home"/> key.
        /// </summary>
        internal void ProcessHome()
        {
            if (caretPosition == 0)
                return;

            caretBlinkTimer = 0;
            caretPosition = 0;

            UpdateCaretBounds();
        }

        /// <summary>
        /// Called when the user presses the <see cref="Key.Key"/> key.
        /// </summary>
        internal void ProcessEnd()
        {
            if (caretPosition == textLayoutStream.TotalLength)
                return;

            caretBlinkTimer = 0;
            caretPosition = textLayoutStream.TotalLength;

            UpdateCaretBounds();
        }

        /// <summary>
        /// Called when the editor should process a mouse click.
        /// </summary>
        /// <param name="device">The <see cref="MouseDevice"/> that raised the event.</param>
        /// <param name="button">The <see cref="MouseButton"/> value that corresponds to the button that was clicked.</param>
        /// <param name="data">The routed event metadata for this event.</param>
        internal void ProcessMouseDown(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (textLayoutStream.Count == 0)
                return;

            var mousePosDips = Mouse.GetPosition(this);
            var mousePosPixs = (Point2)Display.DipsToPixels(mousePosDips);

            caretBlinkTimer = 0;
            caretPosition = View.Resources.TextRenderer.GetInsertionPointAtPosition(textLayoutStream, mousePosPixs);

            UpdateCaretBounds();
        }

        /// <inheritdoc/>
        protected override void ReloadContentCore(Boolean recursive)
        {
            if (textLayoutStream.Count > 0)
                UpdateTextLayoutStream(MostRecentAvailableSize);

            UpdateCaretBounds();

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
            DrawText(time, dc);
            DrawCaret(time, dc);

            base.DrawOverride(time, dc);
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
                UpdateCaretBounds();
                return;
            }

            var wrap = (owner.TextWrapping == TextWrapping.Wrap);
            var width = wrap ? (Int32?)Display.DipsToPixels(availableSize.Width) : null;

            var settings = new TextLayoutSettings(owner.Font, width, null, TextFlags.Standard);
            View.Resources.TextRenderer.CalculateLayout(textParserStream, textLayoutStream, settings);

            UpdateCaretBounds();
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
            if (owner == null || !owner.IsKeyboardFocusWithin)
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
        /// Calculates the position at which to draw the text caret.
        /// </summary>
        private void UpdateCaretBounds()
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

        // The editor's internal text buffer.
        private readonly StringBuilder bufferInput = new StringBuilder();
        private readonly StringBuilder bufferText = new StringBuilder();
    }
}
