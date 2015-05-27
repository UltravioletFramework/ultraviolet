using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents an element container which positions its children according to their distance from the container's
    /// left, top, right, and bottom edges.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.Canvas.xml")]
    public class Canvas : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Canvas(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets the distance between the left edge of the canvas and the left edge of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The distance between the left edge of the canvas and the left edge of the specified element.</returns>
        public static Double GetLeft(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Double>(LeftProperty);
        }

        /// <summary>
        /// Gets the distance between the top edge of the canvas and the top edge of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The distance between the top edge of the canvas and the top edge of the specified element.</returns>
        public static Double GetTop(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Double>(TopProperty);
        }

        /// <summary>
        /// Gets the distance between the right edge of the canvas and the right edge of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The distance between the right edge of the canvas and the right edge of the specified element.</returns>
        public static Double GetRight(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Double>(RightProperty);
        }

        /// <summary>
        /// Gets the distance between the bottom edge of the canvas and the bottom edge of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The distance between the bottom edge of the canvas and the bottom edge of the specified element.</returns>
        public static Double GetBottom(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Double>(BottomProperty);
        }

        /// <summary>
        /// Sets the distance between the left edge of the canvas and the left edge of the specified element.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The distance between the left edge of the canvas and the left edge of the specified element.</param>
        public static void SetLeft(DependencyObject element, Double value)
        {
            Contract.Require(element, "element");

            element.SetValue<Double>(LeftProperty, value);
        }

        /// <summary>
        /// Sets the distance between the top edge of the canvas and the top edge of the specified element.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The distance between the top edge of the canvas and the top edge of the specified element.</param>
        public static void SetTop(DependencyObject element, Double value)
        {
            Contract.Require(element, "element");

            element.SetValue<Double>(TopProperty, value);
        }

        /// <summary>
        /// Sets the distance between the right edge of the canvas and the right edge of the specified element.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The distance between the right edge of the canvas and the right edge of the specified element.</param>
        public static void SetRight(DependencyObject element, Double value)
        {
            Contract.Require(element, "element");

            element.SetValue<Double>(RightProperty, value);
        }

        /// <summary>
        /// Sets the distance between the bottom edge of the canvas and the bottom edge of the specified element.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The distance between the bottom edge of the canvas and the bottom edge of the specified element.</param>
        public static void SetBottom(DependencyObject element, Double value)
        {
            Contract.Require(element, "element");

            element.SetValue<Double>(BottomProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating the distance between the left edge of the canvas and the left edge of the element.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'left'.</remarks>
        public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached("Left", typeof(Double), typeof(Canvas),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.NaN, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets or sets a value indicating the distance between the top edge of the canvas and the top edge of the element.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'top'.</remarks>
        public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached("Top", typeof(Double), typeof(Canvas),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.NaN, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets or sets a value indicating the distance between the right edge of the canvas and the right edge of the element.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'right'.</remarks>
        public static readonly DependencyProperty RightProperty = DependencyProperty.RegisterAttached("Right", typeof(Double), typeof(Canvas),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.NaN, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets or sets a value indicating the distance between the bottom edge of the canvas and the bottom edge of the element.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'bottom'.</remarks>
        public static readonly DependencyProperty BottomProperty = DependencyProperty.RegisterAttached("Bottom", typeof(Double), typeof(Canvas),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.NaN, PropertyMetadataOptions.AffectsMeasure));

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var contentWidth  = 0.0;
            var contentHeight = 0.0;

            foreach (var child in Children)
            {
                var left   = GetLeft(child);
                var top    = GetTop(child);
                var right  = GetRight(child);
                var bottom = GetBottom(child);

                var constraintWidth  = availableSize.Width;
                var constraintHeight = availableSize.Height;

                /* NOTE: For the purposes of determining desired size, we assume that
                 * children which stretch - i.e., which have both left & right or
                 * top & bottom values - are content-sized along the relevant axis.
                 * Otherwise, the canvas will expand to fill all available space. */

                if (Double.IsPositiveInfinity(availableSize.Width) && !Double.IsNaN(left) && !Double.IsNaN(right))
                    constraintWidth = Double.PositiveInfinity;

                if (Double.IsPositiveInfinity(availableSize.Height) && !Double.IsNaN(top) && !Double.IsNaN(bottom))
                    constraintHeight = Double.PositiveInfinity;

                if (!Double.IsPositiveInfinity(constraintWidth))
                {
                    if (!Double.IsNaN(left))
                        constraintWidth -= left;
                    if (!Double.IsNaN(right))
                        constraintWidth -= right;
                }

                if (!Double.IsPositiveInfinity(constraintHeight))
                {
                    if (!Double.IsNaN(top))
                        constraintHeight -= top;
                    if (!Double.IsNaN(bottom))
                        constraintHeight -= bottom;
                }

                child.Measure(new Size2D(constraintWidth, constraintHeight));

                var childWidth  = child.DesiredSize.Width;
                var childHeight = child.DesiredSize.Height;

                if (!Double.IsNaN(left))
                    childWidth += left;

                if (!Double.IsNaN(right))
                    childWidth += right;

                if (!Double.IsNaN(top))
                    childHeight += top;

                if (!Double.IsNaN(bottom))
                    childHeight += bottom;

                contentWidth = Math.Max(contentWidth, childWidth);
                contentHeight = Math.Max(contentHeight, childHeight);
            }

            var contentSize = new Size2D(contentWidth, contentHeight);
            return contentSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            foreach (var child in Children)
            {
                var left   = GetLeft(child);
                var top    = GetTop(child);
                var right  = GetRight(child);
                var bottom = GetBottom(child);

                if (Double.IsNaN(left) && Double.IsNaN(right))
                    left = 0;

                if (Double.IsNaN(top) && Double.IsNaN(bottom))
                    top = 0;

                var validLeft   = LayoutUtil.GetValidMeasure(left, 0);
                var validTop    = LayoutUtil.GetValidMeasure(top, 0);
                var validRight  = LayoutUtil.GetValidMeasure(right, 0);
                var validBottom = LayoutUtil.GetValidMeasure(bottom, 0);

                var childWidth  = Math.Min(child.DesiredSize.Width, finalSize.Width - (validLeft + validRight));
                var childHeight = Math.Min(child.DesiredSize.Height, finalSize.Height - (validTop + validBottom));
                
                if (!Double.IsNaN(left) && !Double.IsNaN(right))
                    childWidth = Math.Max(0, finalSize.Width - (left + right));

                if (!Double.IsNaN(top) && !Double.IsNaN(bottom))
                    childHeight = Math.Max(0, finalSize.Height - (top + bottom));
                
                var childX = 0.0;
                var childY = 0.0;

                if (!Double.IsNaN(left))
                    childX = left;

                if (!Double.IsNaN(top))
                    childY = top;

                if (!Double.IsNaN(right))
                    childX = finalSize.Width - (right + childWidth);

                if (!Double.IsNaN(bottom))
                    childY = finalSize.Height - (bottom + childHeight);

                child.Arrange(new RectangleD(childX, childY, childWidth, childHeight), ArrangeOptions.Fill);
            }

            return finalSize;
        }
    }
}
