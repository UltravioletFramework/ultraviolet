using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Layout.Stylesheets;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents an element container which positions its children according to their distance from the container's
    /// left, top, right, and bottom edges.
    /// </summary>
    [UIElement("Canvas")]
    public class Canvas : UIContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its layout.</param>
        public Canvas(UltravioletContext uv, String id)
            : base(uv, id)
        {
            var dpBackgroundColor = DependencyProperty.FindByName("BackgroundColor", typeof(UIElement));
            SetDefaultValue<Color>(dpBackgroundColor, Color.Transparent);
        }

        /// <summary>
        /// Gets the distance between the left edge of the canvas and the left edge of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The distance between the left edge of the canvas and the left edge of the specified element.</returns>
        public static Int32? GetLeft(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Int32?>(LeftProperty);
        }

        /// <summary>
        /// Gets the distance between the top edge of the canvas and the top edge of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The distance between the top edge of the canvas and the top edge of the specified element.</returns>
        public static Int32? GetTop(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Int32?>(TopProperty);
        }

        /// <summary>
        /// Gets the distance between the right edge of the canvas and the right edge of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The distance between the right edge of the canvas and the right edge of the specified element.</returns>
        public static Int32? GetRight(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Int32?>(RightProperty);
        }

        /// <summary>
        /// Gets the distance between the bottom edge of the canvas and the bottom edge of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The distance between the bottom edge of the canvas and the bottom edge of the specified element.</returns>
        public static Int32? GetBottom(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Int32?>(BottomProperty);
        }

        /// <summary>
        /// Gets the width of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The width of the specified element.</returns>
        public static Int32? GetWidth(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Int32?>(WidthProperty);
        }

        /// <summary>
        /// Gets the height of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The height of the specified element.</returns>
        public static Int32? GetHeight(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Int32?>(HeightProperty);
        }

        /// <summary>
        /// Sets the distance between the left edge of the canvas and the left edge of the specified element.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The distance between the left edge of the canvas and the left edge of the specified element.</param>
        public static void SetLeft(UIElement element, Int32? value)
        {
            Contract.Require(element, "element");

            element.SetValue<Int32?>(LeftProperty, value);
        }

        /// <summary>
        /// Sets the distance between the top edge of the canvas and the top edge of the specified element.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The distance between the top edge of the canvas and the top edge of the specified element.</param>
        public static void SetTop(UIElement element, Int32? value)
        {
            Contract.Require(element, "element");

            element.SetValue<Int32?>(TopProperty, value);
        }

        /// <summary>
        /// Sets the distance between the right edge of the canvas and the right edge of the specified element.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The distance between the right edge of the canvas and the right edge of the specified element.</param>
        public static void SetRight(UIElement element, Int32? value)
        {
            Contract.Require(element, "element");

            element.SetValue<Int32?>(RightProperty, value);
        }

        /// <summary>
        /// Sets the distance between the bottom edge of the canvas and the bottom edge of the specified element.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The distance between the bottom edge of the canvas and the bottom edge of the specified element.</param>
        public static void SetBottom(UIElement element, Int32? value)
        {
            Contract.Require(element, "element");

            element.SetValue<Int32?>(BottomProperty, value);
        }

        /// <summary>
        /// Sets the width of the specified element.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The width of the specified element.</param>
        public static void SetWidth(UIElement element, Int32? value)
        {
            Contract.Require(element, "element");

            element.SetValue<Int32?>(WidthProperty, value);
        }

        /// <summary>
        /// Sets the height of the specified element.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The height of the specified element.</param>
        public static void SetHeight(UIElement element, Int32? value)
        {
            Contract.Require(element, "element");

            element.SetValue<Int32?>(HeightProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating the distance between the left edge of the canvas and the left edge of the element.
        /// </summary>
        [Styled("left")]
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(Int32?), typeof(Canvas),
            new DependencyPropertyMetadata(OnLayoutPropertyChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Gets or sets a value indicating the distance between the top edge of the canvas and the top edge of the element.
        /// </summary>
        [Styled("top")]
        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(Int32?), typeof(Canvas),
            new DependencyPropertyMetadata(OnLayoutPropertyChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Gets or sets a value indicating the distance between the right edge of the canvas and the right edge of the element.
        /// </summary>
        [Styled("right")]
        public static readonly DependencyProperty RightProperty = DependencyProperty.Register("Right", typeof(Int32?), typeof(Canvas),
            new DependencyPropertyMetadata(OnLayoutPropertyChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Gets or sets a value indicating the distance between the bottom edge of the canvas and the bottom edge of the element.
        /// </summary>
        [Styled("bottom")]
        public static readonly DependencyProperty BottomProperty = DependencyProperty.Register("Bottom", typeof(Int32?), typeof(Canvas),
            new DependencyPropertyMetadata(OnLayoutPropertyChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Gets or sets a value indicating the width of the specified element.
        /// </summary>
        [Styled("width")]
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(Int32?), typeof(Canvas),
            new DependencyPropertyMetadata(OnLayoutPropertyChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Gets or sets a value indicating the height of the specified element.
        /// </summary>
        [Styled("height")]
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(Int32?), typeof(Canvas),
            new DependencyPropertyMetadata(OnLayoutPropertyChanged, null, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected override Rectangle CalculateLayoutArea(UIElement child)
        {
            if (View == null)
                return Rectangle.Empty;

            var left   = GetLeft(child);
            var top    = GetTop(child);
            var right  = GetRight(child);
            var bottom = GetBottom(child);
            var width  = GetWidth(child);
            var height = GetHeight(child);

            // If we have neither left nor right, assume left: 0
            if (left == null && right == null)
                left = 0;

            // If we have neither top nor bottom, assume top: 0
            if (top == null && bottom == null)
                top = 0;

            // If we have both left and right, calculate width
            if (left != null && right != null)
                width = CalculatedWidth - (left.GetValueOrDefault() + right.GetValueOrDefault());

            // If we have both top and bottom, calculate height
            if (top != null && bottom != null)
                height = CalculatedHeight - (top.GetValueOrDefault() + bottom.GetValueOrDefault());

            // If we have no width, assume 0
            if (width == null)
                width = 0;

            // If we have no height, assume 0
            if (height == null)
                height = 0;

            // Make sure we don't have negative dimensions.
            width  = Math.Max(0, width.GetValueOrDefault());
            height = Math.Max(0, height.GetValueOrDefault());

            // Calculate the element's layout area.
            var x = 0;
            var y = 0;
            if (left != null)
            {
                x = left.GetValueOrDefault();
            }
            else
            {
                x = CalculatedWidth - (right.GetValueOrDefault() + width.GetValueOrDefault());
            }
            if (top != null)
            {
                y = top.GetValueOrDefault();
            }
            else
            {
                y = CalculatedHeight - (bottom.GetValueOrDefault() + height.GetValueOrDefault());
            }
            return new Rectangle(x, y, width.GetValueOrDefault(), height.GetValueOrDefault());
        }

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch)
        {
            DrawBackgroundImage(spriteBatch);
            base.OnDrawing(time, spriteBatch);
        }

        /// <summary>
        /// Called when the value of a layout-required dependency property is changed on an object.
        /// </summary>
        /// <param name="dependencyObject">The dependency object that was changed.</param>
        private static void OnLayoutPropertyChanged(DependencyObject dependencyObject)
        {
            var element = (UIElement)dependencyObject;
            if (element.Container != null)
            {
                element.Container.PerformLayout(element);
            }
        }
    }
}
