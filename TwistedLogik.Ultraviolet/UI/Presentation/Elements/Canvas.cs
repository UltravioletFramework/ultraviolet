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

        /// <inheritdoc/>
        public override void PerformContentLayout()
        {
            foreach (var child in Children)
            {
                if (!ElementParticipatesInLayout(child))
                    continue;

                UpdateChildLayout(child, false);
            }
            UpdateScissorRectangle();
        }

        /// <inheritdoc/>
        public override void PerformPartialLayout(UIElement content)
        {
            Contract.Require(content, "content");

            UpdateChildLayout(content, true);
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

        /// <summary>
        /// Immediately recalculates the layout of the specified child element.
        /// </summary>
        /// <param name="child">The child element for which to calculate a layout.</param>
        /// <param name="partial">A value indicating whether this is a partial layout.</param>
        private void UpdateChildLayout(UIElement child, Boolean partial)
        {
            UpdateParentRelativeArea(child);

            child.UpdateAbsoluteScreenPosition(
                ContentElement.AbsoluteScreenX + child.ParentRelativeX,
                ContentElement.AbsoluteScreenY + child.ParentRelativeY, true);

            if (partial)
                UpdateScissorRectangle();
        }

        /// <summary>
        /// Immediately recalculates the value of the <see cref="UIElement.ParentRelativeArea"/> property
        /// for the specified child element.
        /// </summary>
        /// <param name="child">The child element for which to calculate a layout.</param>
        private void UpdateParentRelativeArea(UIElement child)
        {            
            if (View == null)
                return;

            if (UpdateComponentRootLayout(child))
                return;

            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;

            var left   = GetLeft(child);
            var top    = GetTop(child);
            var right  = GetRight(child);
            var bottom = GetBottom(child);
            var width  = Double.NaN;
            var height = Double.NaN;
            var margin = child.Margin;

            var pxWidth  = (Int32?)null;
            var pxHeight = (Int32?)null;

            // If we have neither left nor right, assume left: 0
            if (Double.IsNaN(left) && Double.IsNaN(right))
                left = 0;

            // If we have neither top nor bottom, assume top: 0
            if (Double.IsNaN(top) && Double.IsNaN(bottom))
                top = 0;

            // Apply margins.
            if (!Double.IsNaN(margin.Left))
                left += margin.Left;

            if (!Double.IsNaN(margin.Top))
                top  += margin.Top;

            if (!Double.IsNaN(right) && !Double.IsNaN(margin.Right))
                right += margin.Right;

            if (!Double.IsNaN(bottom) && !Double.IsNaN(margin.Bottom))
                bottom += margin.Bottom;

            // If we have both left and right, calculate width
            if (!Double.IsNaN(left) && !Double.IsNaN(right))
                width = display.PixelsToDips(ActualWidth) - (left + right);

            // If we have both top and bottom, calculate height
            if (!Double.IsNaN(top) && !Double.IsNaN(bottom))
                height = display.PixelsToDips(ActualHeight) - (top + bottom);

            // Convert from dips to pixels and calculate our final size.
            pxWidth  = Double.IsNaN(width)  ? (Int32?)null : (Int32)Math.Ceiling(display.DipsToPixels(width));
            pxHeight = Double.IsNaN(height) ? (Int32?)null : (Int32)Math.Ceiling(display.DipsToPixels(height));
            child.CalculateActualSize(ref pxWidth, ref pxHeight);

            // Calculate the element's layout area.
            var x = 0;
            var y = 0;
            if (!Double.IsNaN(left))
            {
                var pxLeft = (Int32)display.DipsToPixels(left);
                x = pxLeft;
            }
            else
            {
                var pxRight = (Int32)display.DipsToPixels(right);
                x = ActualWidth - (pxRight + (pxWidth ?? 0));
            }
            if (!Double.IsNaN(top))
            {
                var pxTop = (Int32)display.DipsToPixels(top);
                y = pxTop;
            }
            else
            {
                var pxBottom = (Int32)display.DipsToPixels(bottom);
                y = ActualHeight - (pxBottom + (pxHeight ?? 0));
            }
            
            // Apply the layout area to the element.
            child.ParentRelativeArea = new Rectangle(x, y, pxWidth ?? 0, pxHeight ?? 0);
        }

        /// <summary>
        /// Updates the layout of the container's component root.
        /// </summary>
        /// <param name="child">The element to attempt to update.</param>
        /// <returns><c>true</c> if the specified element is the container's component root; otherwise, <c>false</c>.</returns>
        private Boolean UpdateComponentRootLayout(UIElement child)
        {
            if (ComponentRoot == child)
            {
                child.ParentRelativeArea = new Rectangle(0, 0, ActualWidth, ActualHeight);
                return true;
            }
            return false;
        }
    }
}
