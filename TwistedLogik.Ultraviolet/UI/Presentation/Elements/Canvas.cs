using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents an element container which positions its children according to their distance from the container's
    /// left, top, right, and bottom edges.
    /// </summary>
    [UIElement("Canvas")]
    public class Canvas : Panel
    {
        /// <summary>
        /// Initializes the <see cref="Canvas"/> type.
        /// </summary>
        static Canvas()
        {
            ComponentTemplate = LoadComponentTemplateFromManifestResourceStream(typeof(Canvas).Assembly,
                "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.Canvas.xml");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public Canvas(UltravioletContext uv, String id)
            : base(uv, id)
        {
            LoadComponentRoot(ComponentTemplate);
        }

        /// <summary>
        /// Gets the distance between the left edge of the canvas and the left edge of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The distance between the left edge of the canvas and the left edge of the specified element.</returns>
        public static Double GetLeft(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Double>(LeftProperty);
        }

        /// <summary>
        /// Gets the distance between the top edge of the canvas and the top edge of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The distance between the top edge of the canvas and the top edge of the specified element.</returns>
        public static Double GetTop(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Double>(TopProperty);
        }

        /// <summary>
        /// Gets the distance between the right edge of the canvas and the right edge of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The distance between the right edge of the canvas and the right edge of the specified element.</returns>
        public static Double GetRight(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Double>(RightProperty);
        }

        /// <summary>
        /// Gets the distance between the bottom edge of the canvas and the bottom edge of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The distance between the bottom edge of the canvas and the bottom edge of the specified element.</returns>
        public static Double GetBottom(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Double>(BottomProperty);
        }

        /// <summary>
        /// Sets the distance between the left edge of the canvas and the left edge of the specified element.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The distance between the left edge of the canvas and the left edge of the specified element.</param>
        public static void SetLeft(UIElement element, Double value)
        {
            Contract.Require(element, "element");

            element.SetValue<Double>(LeftProperty, value);
        }

        /// <summary>
        /// Sets the distance between the top edge of the canvas and the top edge of the specified element.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The distance between the top edge of the canvas and the top edge of the specified element.</param>
        public static void SetTop(UIElement element, Double value)
        {
            Contract.Require(element, "element");

            element.SetValue<Double>(TopProperty, value);
        }

        /// <summary>
        /// Sets the distance between the right edge of the canvas and the right edge of the specified element.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The distance between the right edge of the canvas and the right edge of the specified element.</param>
        public static void SetRight(UIElement element, Double value)
        {
            Contract.Require(element, "element");

            element.SetValue<Double>(RightProperty, value);
        }

        /// <summary>
        /// Sets the distance between the bottom edge of the canvas and the bottom edge of the specified element.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The distance between the bottom edge of the canvas and the bottom edge of the specified element.</param>
        public static void SetBottom(UIElement element, Double value)
        {
            Contract.Require(element, "element");

            element.SetValue<Double>(BottomProperty, value);
        }

        /// <summary>
        /// Gets or sets the template used to create the control's component tree.
        /// </summary>
        public static XDocument ComponentTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating the distance between the left edge of the canvas and the left edge of the element.
        /// </summary>
        [Styled("left")]
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(Double), typeof(Canvas),
            new DependencyPropertyMetadata(null, () => Double.NaN, DependencyPropertyOptions.AffectsArrange));

        /// <summary>
        /// Gets or sets a value indicating the distance between the top edge of the canvas and the top edge of the element.
        /// </summary>
        [Styled("top")]
        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(Double), typeof(Canvas),
            new DependencyPropertyMetadata(null, () => Double.NaN, DependencyPropertyOptions.AffectsArrange));

        /// <summary>
        /// Gets or sets a value indicating the distance between the right edge of the canvas and the right edge of the element.
        /// </summary>
        [Styled("right")]
        public static readonly DependencyProperty RightProperty = DependencyProperty.Register("Right", typeof(Double), typeof(Canvas),
            new DependencyPropertyMetadata(null, () => Double.NaN, DependencyPropertyOptions.AffectsArrange));

        /// <summary>
        /// Gets or sets a value indicating the distance between the bottom edge of the canvas and the bottom edge of the element.
        /// </summary>
        [Styled("bottom")]
        public static readonly DependencyProperty BottomProperty = DependencyProperty.Register("Bottom", typeof(Double), typeof(Canvas),
            new DependencyPropertyMetadata(null, () => Double.NaN, DependencyPropertyOptions.AffectsArrange));

        /// <inheritdoc/>
        protected override Size2D MeasureContent(Size2D availableSize)
        {
            var contentWidth  = 0.0;
            var contentHeight = 0.0;

            foreach (var child in Children)
            {
                child.Measure(availableSize);

                if (Double.IsNaN(child.DesiredSize.Width) || Double.IsNaN(child.DesiredSize.Height))
                    continue;

                var cwidth = child.DesiredSize.Width;
                var cheight = child.DesiredSize.Height;

                var left   = GetLeft(child);
                var top    = GetTop(child);
                var right  = GetRight(child);
                var bottom = GetBottom(child);

                if (!Double.IsNaN(left))
                    cwidth += left;

                if (!Double.IsNaN(right))
                    cwidth += right;

                if (!Double.IsNaN(top))
                    cheight += top;

                if (!Double.IsNaN(bottom))
                    cheight += bottom;

                contentWidth = Math.Max(contentWidth, cwidth);
                contentHeight = Math.Max(contentHeight, cheight);
            }

            var contentSize = new Size2D(contentWidth, contentHeight);
            return contentSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeContent(Size2D finalSize, ArrangeOptions options)
        {
            foreach (var child in Children)
            {
                var left   = GetLeft(child);
                var top    = GetTop(child);
                var right  = GetRight(child);
                var bottom = GetBottom(child);

                var childWidth  = child.DesiredSize.Width;
                var childHeight = child.DesiredSize.Height;

                if (Double.IsNaN(left) && Double.IsNaN(right))
                    left = 0;

                if (Double.IsNaN(top) && Double.IsNaN(bottom))
                    top = 0;

                if (!Double.IsNaN(left) && !Double.IsNaN(right))
                {
                    childWidth = RenderContentRegion.Width - (left + right);
                }

                if (!Double.IsNaN(top) && !Double.IsNaN(bottom))
                {
                    childHeight = RenderContentRegion.Height - (top + bottom);
                }
                
                var childX = 0.0;
                var childY = 0.0;

                if (!Double.IsNaN(left))
                    childX = left;

                if (!Double.IsNaN(top))
                    childY = top;

                if (!Double.IsNaN(right))
                    childX = RenderContentRegion.Width - (right + childWidth);

                if (!Double.IsNaN(bottom))
                    childY = RenderContentRegion.Height - (bottom + childHeight);

                child.Arrange(new RectangleD(childX, childY, childWidth, childHeight), ArrangeOptions.Fill);
            }

            return finalSize;
        }
    }
}
