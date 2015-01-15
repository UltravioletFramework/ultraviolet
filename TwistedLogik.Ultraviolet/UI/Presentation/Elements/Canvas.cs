using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents an element container which positions its children according to their distance from the container's
    /// left, top, right, and bottom edges.
    /// </summary>
    [UIElement("Canvas")]
    public class Canvas : Container
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
            SetDefaultValue<Color>(UIElement.BackgroundColorProperty, Color.Transparent);

            LoadComponentRoot(ComponentTemplate);
        }

        protected override Size2 MeasureCore(Size2 availableSize)
        {
            foreach (var child in Children)
            {
                child.Measure(new Size2(Int32.MaxValue, Int32.MaxValue));
            }
            return base.MeasureCore(availableSize);
        }

        protected override void ArrangeCore(Rectangle finalRect)
        {
            foreach (var child in Children)
            {
                var left   = Canvas.GetLeft(child);
                var right  = Canvas.GetRight(child);
                var top    = Canvas.GetTop(child);
                var bottom = Canvas.GetBottom(child);

                var width = child.DesiredSize.Width;
                var height = child.DesiredSize.Height;

                if (!Double.IsNaN(left) && !Double.IsNaN(right))
                    width = finalRect.Width - ((int)left + (int)right);

                if (!Double.IsNaN(top) && !Double.IsNaN(bottom))
                    height = finalRect.Height - ((int)top + (int)bottom);

                var x = Double.IsNaN(left) ? 0 : (int)left;
                var y = Double.IsNaN(top) ? 0 : (int)top;

                child.Arrange(new Rectangle(x, y, width, height));
            }
            base.ArrangeCore(finalRect);
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
            new DependencyPropertyMetadata(null, () => Double.NaN, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Gets or sets a value indicating the distance between the top edge of the canvas and the top edge of the element.
        /// </summary>
        [Styled("top")]
        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(Double), typeof(Canvas),
            new DependencyPropertyMetadata(null, () => Double.NaN, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Gets or sets a value indicating the distance between the right edge of the canvas and the right edge of the element.
        /// </summary>
        [Styled("right")]
        public static readonly DependencyProperty RightProperty = DependencyProperty.Register("Right", typeof(Double), typeof(Canvas),
            new DependencyPropertyMetadata(null, () => Double.NaN, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Gets or sets a value indicating the distance between the bottom edge of the canvas and the bottom edge of the element.
        /// </summary>
        [Styled("bottom")]
        public static readonly DependencyProperty BottomProperty = DependencyProperty.Register("Bottom", typeof(Double), typeof(Canvas),
            new DependencyPropertyMetadata(null, () => Double.NaN, DependencyPropertyOptions.AffectsMeasure));

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            DrawBackgroundImage(spriteBatch, opacity);

            base.OnDrawing(time, spriteBatch, opacity);
        }
    }
}
