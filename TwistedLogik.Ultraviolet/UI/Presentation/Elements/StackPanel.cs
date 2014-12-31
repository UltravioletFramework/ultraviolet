using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents an element container which positions its children according to their distance from the container's
    /// left, top, right, and bottom edges.
    /// </summary>
    [UIElement("StackPanel")]
    public class StackPanel : Container
    {
        /// <summary>
        /// Initializes the <see cref="StackPanel"/> type.
        /// </summary>
        static StackPanel()
        {
            ComponentTemplate = LoadComponentTemplateFromManifestResourceStream(typeof(StackPanel).Assembly,
                "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.StackPanel.xml");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StackPanel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        /// <param name="viewModelType">The type of view model to which the element will be bound.</param>
        /// <param name="bindingContext">The binding context to apply to the element which is instantiated.</param>
        public StackPanel(UltravioletContext uv, String id, Type viewModelType, String bindingContext = null)
            : base(uv, id)
        {
            SetDefaultValue<Color>(UIElement.BackgroundColorProperty, Color.Transparent);

            if (ComponentTemplate != null)
                LoadComponentRoot(ComponentTemplate, viewModelType, bindingContext);
        }

        /// <inheritdoc/>
        public sealed override void CalculateContentSize(ref Int32? width, ref Int32? height)
        {
            if (width == null)
                width = contentSize.Width;
            if (height == null)
                height = contentSize.Height;

            base.CalculateContentSize(ref width, ref height);
        }

        /// <inheritdoc/>
        public sealed override void PerformContentLayout()
        {
            contentSize = Size2.Zero;

            var position = 0;
            foreach (var child in Children)
            {
                UpdateChildLayout(child, ref position, ref contentSize);
            }
            UpdateScissorRectangle();
        }

        /// <inheritdoc/>
        public sealed override void PerformPartialLayout(UIElement content)
        {
            Contract.Require(content, "content");

            RequestLayout();
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
        /// Gets or sets the stack panel's orientation.
        /// </summary>
        public Orientation Orientation
        {
            get { return GetValue<Orientation>(OrientationProperty); }
            set { SetValue<Orientation>(OrientationProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation"/> property changes.
        /// </summary>
        public event UIElementEventHandler OrientationChanged;

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(StackPanel),
            new DependencyPropertyMetadata(HandleOrientationChanged, () => Orientation.Vertical, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch)
        {
            DrawBackgroundImage(spriteBatch);
            base.OnDrawing(time, spriteBatch);
        }

        /// <summary>
        /// Raises the <see cref="OrientationChanged"/> event.
        /// </summary>
        protected virtual void OnOrientationChanged()
        {
            var temp = OrientationChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleOrientationChanged(DependencyObject dobj)
        {
            var element = (StackPanel)dobj;
            element.OnOrientationChanged();
            element.PerformLayout();

            var parent = element.Parent;
            if (parent != null)
                parent.PerformPartialLayout(element);
        }

        /// <summary>
        /// Immediately recalculates the layout of the specified child element.
        /// </summary>
        /// <param name="child">The child element for which to calculate a layout.</param>
        /// <param name="partial">A value indicating whether this is a partial layout.</param>
        private void UpdateChildLayout(UIElement child, ref Int32 position, ref Size2 contentSize)
        {
            if (Orientation == Orientation.Horizontal)
            {
                UpdateChildLayoutHorizontal(child, ref position, ref contentSize);
            }
            else
            {
                UpdateChildLayoutVertical(child, ref position, ref contentSize);
            }
        }

        /// <summary>
        /// Immediately recalculates the layout of the specified child element when
        /// the stack panel is in a vertical orientation.
        /// </summary>
        /// <param name="child">The child element for which to calculate a layout.</param>
        /// <param name="partial">A value indicating whether this is a partial layout.</param>
        private void UpdateChildLayoutVertical(UIElement child, ref Int32 position, ref Size2 contentSize)
        {
            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;

            var widthpx  = Double.IsNaN(child.Width) ? (Int32?)null : (Int32)display.DipsToPixels(child.Width);
            var heightpx = Double.IsNaN(child.Height) ? (Int32?)null : (Int32)display.DipsToPixels(child.Height);
            child.CalculateContentSize(ref widthpx, ref heightpx);

            var margin = display.DipsToPixels(child.Margin);

            var relativeX               = 0;
            var relativeY               = position + (Int32)margin.Top;
            var relativeWidth           = widthpx ?? 0;
            var relativeHeight          = heightpx ?? 0;

            switch (child.HorizontalAlignment)
            {
                case HorizontalAlignment.Center:
                case HorizontalAlignment.Stretch:
                    relativeX = ((ActualWidth - (widthpx ?? 0)) / 2) + ((Int32)margin.Left - (Int32)margin.Right);
                    break;

                case HorizontalAlignment.Left:
                    relativeX = (Int32)margin.Left;            
                    break;

                case HorizontalAlignment.Right:
                    relativeX = ActualWidth - ((widthpx ?? 0) + (Int32)margin.Right);
                    break;
            }

            child.ContainerRelativeArea = new Rectangle(relativeX, relativeY, relativeWidth, relativeHeight);
            UpdateContentSize(child, margin, ref contentSize);

            position = relativeY + child.ContainerRelativeArea.Height + (Int32)margin.Bottom;
        }

        /// <summary>
        /// Immediately recalculates the layout of the specified child element when
        /// the stack panel is in a horizontal orientation.
        /// </summary>
        /// <param name="child">The child element for which to calculate a layout.</param>
        /// <param name="partial">A value indicating whether this is a partial layout.</param>
        private void UpdateChildLayoutHorizontal(UIElement child, ref Int32 position, ref Size2 contentSize)
        {
            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;

            var widthpx  = Double.IsNaN(child.Width) ? (Int32?)null : (Int32)display.DipsToPixels(child.Width);
            var heightpx = Double.IsNaN(child.Height) ? (Int32?)null : (Int32)display.DipsToPixels(child.Height);
            child.CalculateContentSize(ref widthpx, ref heightpx);

            var margin = display.DipsToPixels(child.Margin);

            var relativeX               = position + (Int32)margin.Left;
            var relativeY               = 0;
            var relativeWidth           = widthpx ?? 0;
            var relativeHeight          = heightpx ?? 0;

            switch (child.VerticalAlignment)
            {
                case VerticalAlignment.Center:
                case VerticalAlignment.Stretch:
                    relativeY = ((ActualHeight - (heightpx ?? 0)) / 2) + ((Int32)margin.Top - (Int32)margin.Bottom);
                    break;

                case VerticalAlignment.Top:
                    relativeY = (Int32)margin.Top;
                    break;

                case VerticalAlignment.Bottom:
                    relativeY = ActualHeight - ((heightpx ?? 0) + (Int32)margin.Bottom);
                    break;
            }

            child.ContainerRelativeArea = new Rectangle(relativeX, relativeY, relativeWidth, relativeHeight);
            UpdateContentSize(child, margin, ref contentSize);

            position = relativeX + child.ContainerRelativeArea.Width + (Int32)margin.Right;
        }

        /// <summary>
        /// Updates the element's content size to include the size of the specified element.
        /// </summary>
        /// <param name="element">The element to add to the stack panel's content size.</param>
        /// <param name="margin">The element margin converted to device pixels.</param>
        /// <param name="contentSize">The stack panel's current content size.</param>
        private void UpdateContentSize(UIElement element, Thickness margin, ref Size2 contentSize)
        {
            var elementArea = element.ContainerRelativeArea;

            var marginBoundsRight  = elementArea.X + elementArea.Width + (Int32)margin.Right;
            var marginBoundsBottom = elementArea.Y + elementArea.Height + (Int32)margin.Bottom;

            var contentWidth  = contentSize.Width;
            var contentHeight = contentSize.Height;

            if (contentWidth < marginBoundsRight)
                contentWidth = marginBoundsRight;

            if (contentHeight < marginBoundsBottom)
                contentHeight = marginBoundsBottom;

            contentSize = new Size2(contentWidth, contentHeight);
        }

        // State values.
        private Size2 contentSize;
    }
}
