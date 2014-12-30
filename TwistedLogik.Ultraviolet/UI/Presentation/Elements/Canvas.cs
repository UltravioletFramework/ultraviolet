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
        /// <param name="viewModelType">The type of view model to which the element will be bound.</param>
        /// <param name="bindingContext">The binding context to apply to the element which is instantiated.</param>
        public Canvas(UltravioletContext uv, String id, Type viewModelType, String bindingContext = null)
            : base(uv, id)
        {
            SetDefaultValue<Color>(UIElement.BackgroundColorProperty, Color.Transparent);

            if (ComponentTemplate != null)
                LoadComponentRoot(ComponentTemplate, viewModelType, bindingContext);
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
            new DependencyPropertyMetadata(OnLayoutPropertyChanged, () => Double.NaN, DependencyPropertyOptions.None));

        /// <summary>
        /// Gets or sets a value indicating the distance between the top edge of the canvas and the top edge of the element.
        /// </summary>
        [Styled("top")]
        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(Double), typeof(Canvas),
            new DependencyPropertyMetadata(OnLayoutPropertyChanged, () => Double.NaN, DependencyPropertyOptions.None));

        /// <summary>
        /// Gets or sets a value indicating the distance between the right edge of the canvas and the right edge of the element.
        /// </summary>
        [Styled("right")]
        public static readonly DependencyProperty RightProperty = DependencyProperty.Register("Right", typeof(Double), typeof(Canvas),
            new DependencyPropertyMetadata(OnLayoutPropertyChanged, () => Double.NaN, DependencyPropertyOptions.None));

        /// <summary>
        /// Gets or sets a value indicating the distance between the bottom edge of the canvas and the bottom edge of the element.
        /// </summary>
        [Styled("bottom")]
        public static readonly DependencyProperty BottomProperty = DependencyProperty.Register("Bottom", typeof(Double), typeof(Canvas),
            new DependencyPropertyMetadata(OnLayoutPropertyChanged, () => Double.NaN, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected override Rectangle CalculateLayoutArea(UIElement child)
        {
            if (View == null)
                return Rectangle.Empty;

            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;

            var left   = GetLeft(child);
            var top    = GetTop(child);
            var right  = GetRight(child);
            var bottom = GetBottom(child);
            var width  = child.Width;
            var height = child.Height;

            var widthpx  = (Int32?)null;
            var heightpx = (Int32?)null;

            // If we have neither left nor right, assume left: 0
            if (Double.IsNaN(left) && Double.IsNaN(right))
                left = 0;

            // If we have neither top nor bottom, assume top: 0
            if (Double.IsNaN(top) && Double.IsNaN(bottom))
                top = 0;

            // If we have both left and right, calculate width
            if (!Double.IsNaN(left) && !Double.IsNaN(right))
                width = ActualWidth - (left + right);

            // If we have both top and bottom, calculate height
            if (!Double.IsNaN(top) && !Double.IsNaN(bottom))
                height = ActualHeight - (top + bottom);

            // Honor dimensional constraints
            if (width < MinWidth)
                width = MinWidth;
            if (width > MaxWidth)
                width = MaxWidth;

            if (height < MinHeight)
                height = MinHeight;
            if (height > MaxHeight)
                height = MaxHeight;

            // Convert from dips to pixels
            widthpx  = Double.IsNaN(width)  ? (Int32?)null : (Int32)Math.Ceiling(display.DipsToPixels(width));
            heightpx = Double.IsNaN(height) ? (Int32?)null : (Int32)Math.Ceiling(display.DipsToPixels(height));
            
            // If we're missing a dimension, calculate the recommended dimension.
            if (widthpx == null || heightpx == null)
                child.CalculateContentSize(ref widthpx, ref heightpx);

            // If we have no width, assume 0
            if (widthpx == null)
                widthpx = 0;

            // If we have no height, assume 0
            if (heightpx == null)
                heightpx = 0;

            // Make sure we don't have negative dimensions.
            widthpx  = Math.Max(0, widthpx.GetValueOrDefault());
            heightpx = Math.Max(0, heightpx.GetValueOrDefault());

            // Calculate the element's layout area.
            var x = 0;
            var y = 0;
            if (!Double.IsNaN(left))
            {
                x = (Int32)display.DipsToPixels(left);
            }
            else
            {
                x = ActualWidth - ((Int32)display.DipsToPixels(right) + widthpx.GetValueOrDefault());
            }
            if (!Double.IsNaN(top))
            {
                y = (Int32)display.DipsToPixels(top);
            }
            else
            {
                y = ActualHeight - ((Int32)display.DipsToPixels(bottom) + heightpx.GetValueOrDefault());
            }
            return new Rectangle(x, y, widthpx.GetValueOrDefault(), heightpx.GetValueOrDefault());
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
            if (element.Parent != null)
            {
                element.Parent.PerformPartialLayout(element);
            }
        }
    }
}
