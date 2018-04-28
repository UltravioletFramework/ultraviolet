using System;
using Ultraviolet.Presentation.Documents;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the root visual for elements inside of a <see cref="Popup"/> control.
    /// </summary>
    internal class PopupRoot : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupRoot"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="resized">The action to perform when the popup content is resized.</param>
        public PopupRoot(UltravioletContext uv, Action resized)
            : base(uv, null)
        {
            this.resized = resized;

            this.nonLogicalAdornerDecorator = new NonLogicalAdornerDecorator(uv, null);
            this.nonLogicalAdornerDecorator.ChangeLogicalParent(this);
        }

        /// <summary>
        /// Gets or sets the popup root's child element.
        /// </summary>
        /// <value>The <see cref="UIElement"/> that represents the popup's content. The default
        /// value is <see langword="null"/></value>
        /// <remarks>
        ///	<dprop>
        ///		<dpropField><see cref="ChildProperty"/></dpropField>
        ///		<dpropStylingName>child</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public UIElement Child
        {
            get { return GetValue<UIElement>(ChildProperty); }
            set { SetValue(ChildProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the popup is currently open.
        /// </summary>
        /// <value><see langword="true"/> if the popup is open; otherwise, <see langword="false"/>. The
        /// default value is <see langword="false"/>.</value>
        public Boolean IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; } 
        }

        /// <summary>
        /// Identifies the <see cref="Child"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Child"/> dependency property.</value>
        public static readonly DependencyProperty ChildProperty = DependencyProperty.Register("Child", typeof(UIElement), typeof(PopupRoot),
            new PropertyMetadata<UIElement>(null, PropertyMetadataOptions.None, HandleChildChanged));

        /// <inheritdoc/>
        protected internal override Int32 VisualChildrenCount
        {
            get { return 1; }
        }

        /// <inheritdoc/>
        protected internal override Int32 LogicalChildrenCount
        {
            get { return base.LogicalChildrenCount; }
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(Int32 childIndex)
        {
            if (childIndex != 0)
                throw new ArgumentOutOfRangeException("childIndex");

            return nonLogicalAdornerDecorator;
        }

        /// <inheritdoc/>
        protected internal override UIElement GetLogicalChild(Int32 childIndex)
        {
            return base.GetLogicalChild(childIndex);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            nonLogicalAdornerDecorator.Measure(availableSize);
            return nonLogicalAdornerDecorator.DesiredSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            nonLogicalAdornerDecorator.Arrange(new RectangleD(Point2D.Zero, finalSize), options);
            return nonLogicalAdornerDecorator.RenderSize;
        }

        /// <inheritdoc/>
        protected override Visual HitTestCore(Point2D point)
        {
            return nonLogicalAdornerDecorator.HitTest(point);
        }

        /// <inheritdoc/>
        protected override void OnChildDesiredSizeChanged(UIElement child)
        {
            var popup = Parent as Popup;
            if (popup != null && popup.IsOpen)
            {
                resized?.Invoke();
            }
            base.OnChildDesiredSizeChanged(child);
        }
        
        /// <inheritdoc/>
        protected override RectangleD CalculateTransformedVisualBounds()
        {
            var popup = Parent as Popup;
            if (popup == null)
                return RectangleD.Empty;

            var visualBounds = VisualBounds;

            var popupTransform = popup.PopupTransformToViewWithOrigin;
            RectangleD.TransformAxisAligned(ref visualBounds, ref popupTransform, out visualBounds);

            return visualBounds;
        }

        /// <summary>
        /// Recursively invalidates the measurement state of the
        /// popup root and all of its descendants.
        /// </summary>
        internal void InvalidateMeasureRecursively()
        {
            InvalidateMeasureRecursively(this);
        }

        /// <summary>
        /// Hooks the popup root's children into the visual tree of its parent popup.
        /// </summary>
        internal void HookIntoVisualTree()
        {
            nonLogicalAdornerDecorator.ChangeVisualParent(this);
        }

        /// <summary>
        /// Unhooks the popup root's children from the visual tree of its parent popup.
        /// </summary>
        internal void UnhookFromVisualTree()
        {
            nonLogicalAdornerDecorator.ChangeVisualParent(null);
        }

        /// <inheritdoc/>
        internal override Boolean IsVisuallyConnectedToViewRoot
        {
            get { return IsOpen; }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Child"/> dependency property changes.
        /// </summary>
        private static void HandleChildChanged(DependencyObject dobj, UIElement oldValue, UIElement newValue)
        {
            var popupRoot = (PopupRoot)dobj;
            popupRoot.nonLogicalAdornerDecorator.Child = newValue;
        }

        /// <summary>
        /// Recursively invalidates the measurement state of the
        /// specified element and all of its descendants.
        /// </summary>
        private static void InvalidateMeasureRecursively(UIElement element)
        {
            element.InvalidateMeasure();

            VisualTreeHelper.ForEachChild(element, null, (child, state) =>
            {
                InvalidateMeasureRecursively((UIElement)child);
            });
        }

        // State values.
        private readonly Action resized;
        private Boolean isOpen;

        // Popup components.
        private readonly NonLogicalAdornerDecorator nonLogicalAdornerDecorator;
    }
}
