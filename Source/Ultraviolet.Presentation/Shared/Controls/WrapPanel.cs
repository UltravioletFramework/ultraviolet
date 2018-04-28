using System;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents an element container which stacks its children either directly on top of each
    /// other (if <see cref="Orientation"/> is <see cref="Controls.Orientation.Vertical"/>) or
    /// side-by-side if (see <see cref="Orientation"/> is <see cref="Controls.Orientation.Horizontal"/>,
    /// wrapping the content if necessary to fit within the available space.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.WrapPanel.xml")]
    public class WrapPanel : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrapPanel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public WrapPanel(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the panel's orientation.
        /// </summary>
        /// <value>An <see cref="Orientation"/> value specifying whether the panel is arranged horizontally
        /// or vertically. The default value is <see cref="Controls.Orientation.Horizontal"/>.</value>
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
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(WrapPanel),
            new PropertyMetadata<Orientation>(PresentationBoxedValues.Orientation.Horizontal, PropertyMetadataOptions.AffectsMeasure));

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            foreach (var child in Children)
                child.Measure(availableSize);

            var contentSize = (Orientation == Orientation.Vertical) ? 
                MeasureVertically(availableSize) :
                MeasureHorizontally(availableSize);

            return contentSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            if (Orientation == Orientation.Vertical)
            {
                finalSize = ArrangeVertically(finalSize, options);
            }
            else
            {
                finalSize = ArrangeHorizontally(finalSize, options);
            }

            return finalSize;
        }

        /// <summary>
        /// Gets a value indicating whether the specified elements are in the same layout row of this wrap panel.
        /// </summary>
        /// <param name="e1">The first element to compare.</param>
        /// <param name="e2">The second element to compare.</param>
        /// <returns><see langword="true"/> if the specified elements are in the same layout row; otherwise, <see langword="false"/>.</returns>
        private static Boolean AreInSameRow(UIElement e1, UIElement e2)
        {
            return e1.MostRecentFinalRect.Y == e2.MostRecentFinalRect.Y;
        }

        /// <summary>
        /// Gets a value indicating whether the specified elements are in the same layout column of this wrap panel.
        /// </summary>
        /// <param name="e1">The first element to compare.</param>
        /// <param name="e2">The second element to compare.</param>
        /// <returns><see langword="true"/> if the specified elements are in the same layout column; otherwise, <see langword="false"/>.</returns>
        private static Boolean AreInSameColumn(UIElement e1, UIElement e2)
        {
            return e1.MostRecentFinalRect.X == e2.MostRecentFinalRect.X;
        }

        /// <summary>
        /// Measures the panel when it is oriented vertically.
        /// </summary>
        /// <param name="availableSize">The size of the area which the element's parent has 
        /// specified is available for the element's layout.</param>
        /// <returns>The panel's measured size.</returns>
        private Size2D MeasureVertically(Size2D availableSize)
        {
            var contentWidth  = 0.0;
            var contentHeight = 0.0;

            var index     = 0;            
            var colCount  = 0;
            var colWidth  = 0.0;
            var colHeight = 0.0;

            while (CalculateColumnProperties(availableSize, index, out colCount, out colWidth, out colHeight))
            {
                contentWidth  = contentWidth + colWidth;
                contentHeight = Math.Max(contentHeight, colHeight);

                index += colCount;
            }

            return new Size2D(contentWidth, contentHeight);
        }

        /// <summary>
        /// Measures the panel when it is oriented horizontally.
        /// </summary>
        /// <param name="availableSize">The size of the area which the element's parent has 
        /// specified is available for the element's layout.</param>
        /// <returns>The panel's measured size.</returns>
        private Size2D MeasureHorizontally(Size2D availableSize)
        {
            var contentWidth  = 0.0;
            var contentHeight = 0.0;

            var index     = 0;
            var rowCount  = 0;
            var rowWidth  = 0.0;
            var rowHeight = 0.0;

            while (CalculateRowProperties(availableSize, index, out rowCount, out rowWidth, out rowHeight))
            {
                contentWidth  = Math.Max(contentWidth, rowWidth);
                contentHeight = contentHeight + rowHeight;

                index += rowCount;
            }

            return new Size2D(contentWidth, contentHeight);
        }

        /// <summary>
        /// Arranges the panel when it is oriented vertically.
        /// </summary>
        /// <param name="finalSize">The element's final size after arrangement.</param>
        /// <param name="options">A set of <see cref="ArrangeOptions"/> values specifying the options for this arrangement.</param>
        /// <returns>The panel's size after arrangement.</returns>
        private Size2D ArrangeVertically(Size2D finalSize, ArrangeOptions options)
        {
            var index     = 0;
            var positionX = 0.0;
            var positionY = 0.0;

            var colCount  = 0;
            var colWidth  = 0.0;
            var colHeight = 0.0;

            while (CalculateColumnProperties(finalSize, index, out colCount, out colWidth, out colHeight))
            {
                for (int i = 0; i < colCount; i++)
                {
                    var child = Children[index + i];
                    var childRect = new RectangleD(positionX, positionY, colWidth, child.DesiredSize.Height);

                    child.Arrange(childRect);

                    positionY += childRect.Height;
                }

                positionX = positionX + colWidth;
                positionY = 0;

                index += colCount;
            }

            return finalSize;
        }

        /// <summary>
        /// Arranges the panel when it is oriented horizontally.
        /// </summary>
        /// <param name="finalSize">The element's final relative to its parent element.</param>
        /// <param name="options">A set of <see cref="ArrangeOptions"/> values specifying the options for this arrangement.</param>
        /// <returns>The panel's size after arrangement.</returns>
        private Size2D ArrangeHorizontally(Size2D finalSize, ArrangeOptions options)
        {
            var index     = 0;
            var positionX = 0.0;
            var positionY = 0.0;

            var rowCount  = 0;
            var rowWidth  = 0.0;
            var rowHeight = 0.0;

            while (CalculateRowProperties(finalSize, index, out rowCount, out rowWidth, out rowHeight))
            {
                for (int i = 0; i < rowCount; i++)
                {
                    var child = Children[index + i];
                    var childRect = new RectangleD(positionX, positionY, child.DesiredSize.Width, rowHeight);

                    child.Arrange(childRect);

                    positionX += childRect.Width;
                }

                positionX = 0;
                positionY = positionY + rowHeight;

                index += rowCount;
            }

            return finalSize;
        }

        /// <summary>
        /// Calculates the properties of the specified layout row.
        /// </summary>
        /// <param name="availableSize">The amount of space that is available for laying out the panel.</param>
        /// <param name="index">The index of the first element in the layout row.</param>
        /// <param name="rowCount">The number of elements in the layout row.</param>
        /// <param name="rowWidth">The width of the layout row in device independent pixels.</param>
        /// <param name="rowHeight">The height of the layout row in device independent pixels.</param>
        /// <returns><see langword="true"/> if the specified index is the beginning of a valid layout row; otherwise, <see langword="false"/>.</returns>
        private Boolean CalculateRowProperties(Size2D availableSize, Int32 index, out Int32 rowCount, out Double rowWidth, out Double rowHeight)
        {
            rowCount  = 0;
            rowWidth  = 0;
            rowHeight = 0.0;

            if (index >= Children.Count)
                return false;

            var position = 0.0;
            var height   = 0.0;
            var count    = 0;

            for (int i = index; i < Children.Count; i++)
            {
                var child = Children[i];
                if (position + child.DesiredSize.Width > availableSize.Width)
                    break;

                count  = count + 1;
                height = Math.Max(height, child.DesiredSize.Height);

                position += child.DesiredSize.Width;
            }

            rowCount  = count;
            rowWidth  = position;
            rowHeight = height;

            return rowCount > 0;
        }

        /// <summary>
        /// Calculates the properties of the specified layout column.
        /// </summary>
        /// <param name="availableSize">The amount of space that is available for laying out the panel.</param>
        /// <param name="index">The index of the first element in the layout column.</param>
        /// <param name="columnCount">The number of elements in the layout column.</param>
        /// <param name="columnWidth">The width of the layout column in pixels.</param>
        /// <param name="columnHeight">The height of the layout column in pixels.</param>
        /// <returns><see langword="true"/> if the specified index is the beginning of a valid layout column; otherwise, <see langword="false"/>.</returns>
        private Boolean CalculateColumnProperties(Size2D availableSize, Int32 index, out Int32 columnCount, out Double columnWidth, out Double columnHeight)
        {
            columnCount  = 0;
            columnWidth  = 0.0;
            columnHeight = 0.0;

            if (index >= Children.Count)
                return false;

            var position = 0.0;
            var width    = 0.0;
            var count    = 0;

            for (int i = index; i < Children.Count; i++)
            {
                var child = Children[i];
                if (position + child.DesiredSize.Height > availableSize.Height)
                    break;

                count = count + 1;
                width = Math.Max(width, child.DesiredSize.Width);

                position += child.DesiredSize.Height;
            }

            columnCount  = count;
            columnWidth  = width;
            columnHeight = position;

            return columnCount > 0;
        }
    }
}
