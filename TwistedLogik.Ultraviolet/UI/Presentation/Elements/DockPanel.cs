using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a panel which allows its children to dock to one of its four edges.
    /// </summary>
    [UIElement("DockPanel", "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.DockPanel.xml")]
    public class DockPanel : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DockPanel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public DockPanel(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Gets the <see cref="Dock"/> value that specifies how an element is positioned within its parent <see cref="DockPanel"/>.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The <see cref="Dock"/> value that specifies how an element is positioned within its parent <see cref="DockPanel"/>.</returns>
        public static Dock GetDock(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Dock>(DockProperty);
        }

        /// <summary>
        /// Sets the <see cref="Dock"/> value that specifies how an element is positioned within its parent <see cref="DockPanel"/>.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <param name="value">The <see cref="Dock"/> value that specifies how an element is positioned within its parent <see cref="DockPanel"/>.</param>
        public static void SetDock(UIElement element, Dock value)
        {
            Contract.Require(element, "element");

            element.SetValue<Dock>(DockProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the panel's last child stretches to fit the available space.
        /// </summary>
        public Boolean LastChildFill
        {
            get { return GetValue<Boolean>(LastChildFillProperty); }
            set { SetValue<Boolean>(LastChildFillProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="LastChildFill"/> property changes.
        /// </summary>
        public event UIElementEventHandler LastChildFillChanged;

        /// <summary>
        /// Identifies the Dock attached property.
        /// </summary>
        [Styled("dock")]
        public static readonly DependencyProperty DockProperty = DependencyProperty.Register("Dock", typeof(Dock), typeof(DockPanel),
            new DependencyPropertyMetadata(null, () => Dock.Left, DependencyPropertyOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="LastChildFill"/> dependency property.
        /// </summary>
        [Styled("last-child-fill")]
        public static readonly DependencyProperty LastChildFillProperty = DependencyProperty.Register("LastChildFill", typeof(Boolean), typeof(DockPanel),
            new DependencyPropertyMetadata(HandleLastChildFillChanged, () => false, DependencyPropertyOptions.AffectsArrange));

        /// <inheritdoc/>
        protected override Size2D MeasureContent(Size2D availableSize)
        {
            var sizeLeft   = 0.0;
            var sizeTop    = 0.0;
            var sizeRight  = 0.0;
            var sizeBottom = 0.0;

            foreach (var child in Children)
            {
                var availableSizeForChild = new Size2D(
                    availableSize.Width - (sizeLeft + sizeRight),
                    availableSize.Height - (sizeTop + sizeBottom));
                child.Measure(availableSizeForChild);
            }

            return new Size2D(sizeLeft + sizeRight, sizeTop + sizeBottom);
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeContent(Size2D finalSize, ArrangeOptions options)
        {
            var sizeLeft   = 0.0;
            var sizeTop    = 0.0;
            var sizeRight  = 0.0;
            var sizeBottom = 0.0;

            var childWidth  = 0.0;
            var childHeight = 0.0;

            var lastChildFill = LastChildFill;

            for (int i = 0; i < Children.Count; i++)
            {
                var child       = Children[i];
                var isLastChild = (i == Children.Count - 1);

                var remainingWidth  = finalSize.Width - (sizeLeft + sizeRight);
                var remainingHeight = finalSize.Height - (sizeTop + sizeBottom);

                var childRect = RectangleD.Empty;

                switch (GetDock(child))
                {
                    case Dock.Left:
                        childWidth  = isLastChild && lastChildFill ? remainingWidth : Math.Min(child.DesiredSize.Width, remainingWidth);
                        childHeight = remainingHeight;
                        childRect   = new RectangleD(sizeLeft, sizeTop, childWidth, childHeight);
                        sizeLeft    = sizeLeft + childRect.Width;
                        break;

                    case Dock.Top:
                        childWidth  = remainingWidth;
                        childHeight = isLastChild && lastChildFill ? remainingHeight : Math.Min(child.DesiredSize.Height, remainingHeight);
                        childRect   = new RectangleD(sizeLeft, sizeTop, childWidth, childHeight);
                        sizeTop     = sizeTop + childRect.Height;
                        break;

                    case Dock.Right:
                        childWidth  = isLastChild && lastChildFill ? remainingWidth : Math.Min(child.DesiredSize.Width, remainingWidth);
                        childHeight = remainingHeight;
                        childRect   = new RectangleD(finalSize.Width - (sizeRight + childWidth), sizeTop, childWidth, childHeight);
                        sizeRight   = sizeRight + childRect.Width;
                        break;

                    case Dock.Bottom:
                        childWidth  = remainingWidth;
                        childHeight = isLastChild && lastChildFill ? remainingHeight : Math.Min(child.DesiredSize.Height, remainingHeight);
                        childRect   = new RectangleD(sizeLeft, finalSize.Height - (sizeBottom + childHeight), childWidth, childHeight);
                        sizeBottom  = sizeBottom + childRect.Height;
                        break;
                }

                child.Arrange(childRect);
            }

            return finalSize;
        }

        /// <inheritdoc/>
        protected override void PositionContent(Point2D position)
        {
            foreach (var child in Children)
                child.Position(position);

            base.PositionContent(position);
        }

        /// <summary>
        /// Raises the <see cref="LastChildFillChanged"/> event.
        /// </summary>
        protected virtual void OnLastChildFillChanged()
        {
            var temp = LastChildFillChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="LastChildFill"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleLastChildFillChanged(DependencyObject dobj)
        {
            var panel = (DockPanel)dobj;
            panel.OnLastChildFillChanged();
        }
    }
}
