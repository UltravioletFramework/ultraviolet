using System;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a control which renders a border around its content.
    /// </summary>
    [UvmlKnownType]
    public class Border : Decorator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Border(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets a the thickness of the control's border.
        /// </summary>
        /// <value>A <see cref="Thickness"/> value that describes the thickness of the
        /// control's border on each of its four sides.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="BorderThicknessProperty"/></dpropField>
        ///     <dpropStylingName>border-thickness</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Thickness BorderThickness
        {
            get { return GetValue<Thickness>(BorderThicknessProperty); }
            set { SetValue<Thickness>(BorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the control's border.
        /// </summary>
        /// <value>A <see cref="Color"/> value that describes the color of the
        /// control's border.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="BorderColorProperty"/></dpropField>
        ///     <dpropStylingName>border-color</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color BorderColor
        {
            get { return GetValue<Color>(BorderColorProperty); }
            set { SetValue<Color>(BorderColorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="BorderThickness"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="BorderThickness"/> dependency property.</value>
        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(Border),
            new PropertyMetadata<Thickness>(PresentationBoxedValues.Thickness.One, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="BorderColor"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="BorderColor"/> dependency property.</value>
        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register("BorderColor", typeof(Color), typeof(Border),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.Black));

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var totalPadding = BorderThickness + Padding;
            var totalPaddingWidth = totalPadding.Left + totalPadding.Right;
            var totalPaddingHeight = totalPadding.Top + totalPadding.Bottom;

            var child = Child;
            if (child == null)
            {
                return new Size2D(totalPaddingWidth, totalPaddingHeight);
            }

            var childAvailableSize = availableSize - totalPadding;
            child.Measure(childAvailableSize);

            return new Size2D(
                child.DesiredSize.Width + totalPaddingWidth, 
                child.DesiredSize.Height + totalPaddingHeight);
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            var totalPadding = BorderThickness + Padding;
            var totalPaddingWidth = totalPadding.Left + totalPadding.Right;
            var totalPaddingHeight = totalPadding.Top + totalPadding.Bottom;

            var child = Child;
            if (child != null)
            {
                var childArrangeRect = new RectangleD(
                    totalPadding.Left, 
                    totalPadding.Right,
                    Math.Max(0, finalSize.Width - totalPaddingWidth), 
                    Math.Max(0, finalSize.Height - totalPaddingHeight));

                child.Arrange(childArrangeRect, options);
            }

            return finalSize;
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            var borderColor = BorderColor;
            var borderThickness = BorderThickness;

            var borderArea = new RectangleD(0, 0, UntransformedRelativeBounds.Width, UntransformedRelativeBounds.Height);

            var leftSize = Math.Min(borderThickness.Left, borderArea.Width);
            if (leftSize > 0)
            {
                var leftArea = new RectangleD(borderArea.Left, borderArea.Top, leftSize, borderArea.Height);
                DrawBlank(dc, leftArea, borderColor);
            }

            var topSize = Math.Min(borderThickness.Top, borderArea.Height);
            if (topSize > 0)
            {
                var topArea = new RectangleD(borderArea.Left, borderArea.Top, borderArea.Width, topSize);
                DrawBlank(dc, topArea, borderColor);
            }

            var rightSize = Math.Min(borderThickness.Right, borderArea.Width);
            if (rightSize > 0)
            {
                var rightPos = Math.Max(borderArea.Left, borderArea.Right - rightSize);
                var rightArea = new RectangleD(rightPos, borderArea.Top, rightSize, borderArea.Height);
                DrawBlank(dc, rightArea, borderColor);
            }

            var bottomSize = Math.Min(borderThickness.Bottom, borderArea.Height);
            if (bottomSize > 0)
            {
                var bottomPos = Math.Max(borderArea.Top, borderArea.Bottom - bottomSize);
                var bottomArea = new RectangleD(borderArea.Left, bottomPos, borderArea.Width, bottomSize);
                DrawBlank(dc, bottomArea, borderColor);
            }

            base.DrawOverride(time, dc);
        }
    }
}
