using System;
using Ultraviolet.Presentation.Controls.Primitives;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a text block that is optimized for displaying numeric values.
    /// </summary>
    [UvmlKnownType]
    [UvmlDefaultProperty("Value")]
    public class NumericTextBlock : TextBlockBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericTextBlock"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public NumericTextBlock(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the label's value.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the label's value.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ValueProperty"/></dpropField>
        ///     <dpropStylingName>value</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double Value
        {
            get { return GetValue<Double>(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the format string used to format the label's value.
        /// </summary>
        /// <value>A format string that specifies how to format the text box's value. The default
        /// value is <see langword="null"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="FormatProperty"/></dpropField>
        ///     <dpropStylingName>format</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public String Format
        {
            get { return GetValue<String>(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'value'.</remarks>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Double), typeof(NumericTextBlock),
            new PropertyMetadata<Double>(null, PropertyMetadataOptions.AffectsMeasure));
        
        /// <summary>
        /// Identifies the <see cref="Format"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'format'.</remarks>
        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof(String), typeof(NumericTextBlock),
            new PropertyMetadata<String>(null, PropertyMetadataOptions.AffectsMeasure));

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            var font = Font;
            if (font.IsLoaded)
            {
                View.Resources.StringFormatter.Reset();
                View.Resources.StringFormatter.AddArgument(Value);
                View.Resources.StringFormatter.Format(Format ?? "{0}", View.Resources.StringBuffer);

                var face = font.Resource.Value.GetFace(FontStyle);
                var positionRaw = Display.DipsToPixels(UntransformedAbsolutePosition);
                var positionX = dc.IsTransformed ? positionRaw.X : Math.Floor(positionRaw.X);
                var positionY = dc.IsTransformed ? positionRaw.Y : Math.Floor(positionRaw.Y);
                var position = new Vector2((Single)positionX, (Single)positionY);

                dc.RawDrawString(face, View.Resources.StringBuffer, position, Foreground);
            }
            base.DrawOverride(time, dc);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var font = Font;
            if (!font.IsLoaded)
                return Size2D.Zero;

            View.Resources.StringFormatter.Reset();
            View.Resources.StringFormatter.AddArgument(Value);
            View.Resources.StringFormatter.Format(Format ?? "{0}", View.Resources.StringBuffer);

            var face = font.Resource.Value.GetFace(FontStyle);
            return face.MeasureString(View.Resources.StringBuffer);
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            return finalSize;
        }
    }
}
