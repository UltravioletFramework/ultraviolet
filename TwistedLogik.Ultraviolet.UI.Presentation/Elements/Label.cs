using System;
using System.ComponentModel;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a text label on a user interface.
    /// </summary>
    [UvmlKnownType("Label")]
    [DefaultProperty("Text")]
    public class Label : LabelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public Label(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Gets or sets the label's text.
        /// </summary>
        public String Text
        {
            get { return GetValue<String>(TextProperty); }
            set { SetValue<String>(TextProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Text"/> property changes.
        /// </summary>
        public event UIElementEventHandler TextChanged;

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(Label),
            new DependencyPropertyMetadata(HandleTextChanged, null, DependencyPropertyOptions.AffectsMeasure));

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

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            if (textLayoutResult.Count > 0)
            {
                var position = (Vector2)Display.DipsToPixels(AbsolutePosition);
                View.Resources.TextRenderer.Draw(dc.SpriteBatch, textLayoutResult, position, FontColor * dc.Opacity);
            }
            base.DrawOverride(time, dc);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            UpdateTextLayoutResult(availableSize);

            var sizePixels = new Size2D(textLayoutResult.ActualWidth, textLayoutResult.ActualHeight);
            var sizeDips   = Display.PixelsToDips(sizePixels);

            return sizeDips;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            UpdateTextLayoutResult(finalSize);

            return base.ArrangeOverride(finalSize, options);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Text"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleTextChanged(DependencyObject dobj)
        {
            var label = (Label)dobj;
            label.UpdateTextParserResult();
            label.OnTextChanged();
        }

        /// <summary>
        /// Updates the cache that contains the result of parsing the label's text.
        /// </summary>
        private void UpdateTextParserResult()
        {
            textParserResult.Clear();

            var text = Text;
            if (!String.IsNullOrEmpty(text))
            {
                View.Resources.TextRenderer.Parse(text, textParserResult);
            }
        }

        /// <summary>
        /// Updates the cache that contains the result of laying out the label's text.
        /// </summary>
        /// <param name="availableSize">The size of the space that is available for laying out text.</param>
        private void UpdateTextLayoutResult(Size2D availableSize)
        {
            textLayoutResult.Clear();

            if (textParserResult.Count > 0)
            {
                var unconstrainedWidth  = Double.IsNaN(Width)  && HorizontalAlignment != HorizontalAlignment.Stretch;
                var unconstrainedHeight = Double.IsNaN(Height) && VerticalAlignment != VerticalAlignment.Stretch;

                var constraintX = unconstrainedWidth  ? Int32.MaxValue : (Int32)Display.DipsToPixels(availableSize.Width);
                var constraintY = unconstrainedHeight ? Int32.MaxValue : (Int32)Display.DipsToPixels(availableSize.Height);

                var flags    = LayoutUtil.ConvertAlignmentsToTextFlags(HorizontalContentAlignment, VerticalContentAlignment);
                var settings = new TextLayoutSettings(Font, constraintX, constraintY, flags, FontStyle);

                View.Resources.TextRenderer.CalculateLayout(textParserResult, textLayoutResult, settings);
            }
        }

        // State values.
        private readonly TextParserResult textParserResult = new TextParserResult();
        private readonly TextLayoutResult textLayoutResult = new TextLayoutResult();
    }
}
