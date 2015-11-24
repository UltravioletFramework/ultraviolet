using System;
using System.Text;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents the text editor in a <see cref="TextArea"/>.
    /// </summary>
    [UvmlKnownType]
    public class TextAreaContentPresenter : ContentPresenter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextAreaContentPresenter"/> control.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TextAreaContentPresenter(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Called when the <see cref="TextArea"/> that owns this content presenter updates its internal text buffer.
        /// </summary>
        /// <param name="textBuffer">The buffer which contains the text to display.</param>
        internal void HandleTextBufferUpdate(StringBuilder textBuffer)
        {
            UpdateTextParserStream(textBuffer);
        }
        
        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            UpdateTextLayoutStream(availableSize);

            var width = Display.PixelsToDips(textLayoutStream.ActualWidth);
            var height = Display.PixelsToDips(textLayoutStream.ActualHeight);

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
            if (textLayoutStream.Count > 0)
            {
                var color = (ContainingTextArea == null) ? Color.White : ContainingTextArea.Foreground;
                var position = Display.DipsToPixels(UntransformedAbsolutePosition + ContentOffset);
                var positionRounded = dc.IsTransformed ? (Vector2)position : (Vector2)(Point2)position;
                View.Resources.TextRenderer.Draw((SpriteBatch)dc, textLayoutStream, positionRounded, color * dc.Opacity);
            }
            base.DrawOverride(time, dc);
        }

        /// <summary>
        /// Gets the <see cref="TextArea"/> which contains this content presenter.
        /// </summary>
        protected TextArea ContainingTextArea
        {
            get { return TemplatedParent as TextArea; }
        }

        /// <summary>
        /// Updates the text parser stream.
        /// </summary>
        private void UpdateTextParserStream(StringBuilder textBuffer)
        {
            textParserStream.Clear();

            if (View == null || ContainingTextArea == null)
                return;

            View.Resources.TextRenderer.Parse(textBuffer, textParserStream, TextParserOptions.IgnoreCommandCodes);

            InvalidateMeasure();
        }

        /// <summary>
        /// Updates the text layout stream.
        /// </summary>
        private void UpdateTextLayoutStream(Size2D availableSize)
        {
            textLayoutStream.Clear();

            if (View == null || ContainingTextArea == null)
                return;

            var wrap = (ContainingTextArea.TextWrapping == TextWrapping.Wrap);
            var width = wrap ? (Int32?)Display.DipsToPixels(availableSize.Width) : null;

            var settings = new TextLayoutSettings(ContainingTextArea.Font, width, null, TextFlags.Standard);
            View.Resources.TextRenderer.CalculateLayout(textParserStream, textLayoutStream, settings);
        }

        // State values.
        private readonly TextParserTokenStream textParserStream = new TextParserTokenStream();
        private readonly TextLayoutCommandStream textLayoutStream = new TextLayoutCommandStream();
    }
}
