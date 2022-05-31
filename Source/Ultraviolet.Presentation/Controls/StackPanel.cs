using System;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents an element container which stacks its children either directly on top of each
    /// other (if <see cref="Orientation"/> is <see cref="Controls.Orientation.Vertical"/>) or
    /// side-by-side if (see <see cref="Orientation"/> is <see cref="Controls.Orientation.Horizontal"/>).
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.StackPanel.xml")]
    public class StackPanel : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StackPanel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public StackPanel(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the panel's orientation.
        /// </summary>
        /// <value>A <see cref="Controls.Orientation"/> value which determines whether the panel is oriented horizontally
        /// or vertically. The default value is <see cref="Controls.Orientation.Vertical"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="OrientationProperty"/></dpropField>
        ///     <dpropStylingName>orientation</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Orientation Orientation
        {
            get { return GetValue<Orientation>(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Orientation"/> dependency property.</value>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(StackPanel),
            new PropertyMetadata<Orientation>(PresentationBoxedValues.Orientation.Vertical, PropertyMetadataOptions.AffectsMeasure));

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var contentWidth  = 0.0;
            var contentHeight = 0.0;

            if (Orientation == Orientation.Vertical)
            {
                foreach (var child in Children)
                {
                    child.Measure(new Size2D(availableSize.Width, Double.PositiveInfinity));

                    contentWidth  = Math.Max(contentWidth, child.DesiredSize.Width);
                    contentHeight = contentHeight + child.DesiredSize.Height;
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    child.Measure(new Size2D(Double.PositiveInfinity, availableSize.Height));

                    contentWidth  = contentWidth + child.DesiredSize.Width;
                    contentHeight = Math.Max(contentHeight, child.DesiredSize.Height);
                }
            }

            return new Size2D(contentWidth, contentHeight);
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            var positionX = 0.0;
            var positionY = 0.0;

            if (Orientation == Orientation.Vertical)
            {
                foreach (var child in Children)
                {
                    child.Arrange(new RectangleD(positionX, positionY, finalSize.Width, child.DesiredSize.Height));
                    positionY += child.DesiredSize.Height;
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    child.Arrange(new RectangleD(positionX, positionY, child.DesiredSize.Width, finalSize.Height));
                    positionX += child.DesiredSize.Width;
                }
            }

            return finalSize;
        }
    }
}
