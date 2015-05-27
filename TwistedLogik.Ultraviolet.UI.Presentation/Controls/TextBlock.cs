using System;
using System.ComponentModel;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a lightweight control for displaying text.
    /// </summary>
    [UvmlKnownType]
    [DefaultProperty("Text")]
    public class TextBlock : TextBlockBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBlock"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TextBlock(UltravioletContext uv, String name)
            : base(uv, name)
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
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'text'.</remarks>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(TextBlock),
            new PropertyMetadata<String>(null, PropertyMetadataOptions.AffectsMeasure, HandleTextChanged));

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            UpdateTextParserResult();
            base.OnInitialized();
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            if (textLayoutResult.Count > 0)
            {
                var position = (Vector2)Display.DipsToPixels(AbsolutePosition);
                View.Resources.TextRenderer.Draw(dc.SpriteBatch, textLayoutResult, position, Foreground * dc.Opacity);
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
        private static void HandleTextChanged(DependencyObject dobj, String oldValue, String newValue)
        {
            var label = (TextBlock)dobj;
            label.UpdateTextParserResult();
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

            if (textParserResult.Count > 0 && Font.IsLoaded)
            {
                var unconstrainedWidth  = Double.IsPositiveInfinity(availableSize.Width)  && HorizontalAlignment != HorizontalAlignment.Stretch;
                var unconstrainedHeight = Double.IsPositiveInfinity(availableSize.Height) && VerticalAlignment != VerticalAlignment.Stretch;

                var constraintX = unconstrainedWidth  ? null : (Int32?)Display.DipsToPixels(availableSize.Width);
                var constraintY = unconstrainedHeight ? null : (Int32?)Display.DipsToPixels(availableSize.Height);

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
