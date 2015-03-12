using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents an element container which stacks its children either directly on top of each
    /// other (if <see cref="StackPanel.Orientation"/> is <see cref="F:Orientation.Vertical"/>) or
    /// side-by-side if (see <see cref="StackPanel.Orientation"/> is <see cref="F:Orientation.Horizontal"/>).
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.StackPanel.xml")]
    public class StackPanel : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StackPanel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public StackPanel(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Gets or sets the panel's orientation.
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
            new DependencyPropertyMetadata(HandleOrientationChanged, () => Orientation.Vertical, DependencyPropertyOptions.AffectsMeasure));

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var contentWidth  = 0.0;
            var contentHeight = 0.0;

            if (Orientation == Orientation.Vertical)
            {
                foreach (var child in Children)
                {
                    child.Measure(new Size2D(availableSize.Width, Double.PositiveInfinity));

                    contentWidth  = Math.Max(contentWidth, child.DesiredSize.Width);
                    contentHeight = contentHeight + child.DesiredSize.Height;
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    child.Measure(new Size2D(Double.PositiveInfinity, availableSize.Height));

                    contentWidth  = contentWidth + child.DesiredSize.Width;
                    contentHeight = Math.Max(contentHeight, child.DesiredSize.Height);
                }
            }

            contentSize = new Size2D(
                Math.Min(availableSize.Width, contentWidth), 
                Math.Min(availableSize.Height, contentHeight));

            return contentSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            var positionX = 0.0;
            var positionY = 0.0;

            if (Orientation == Orientation.Vertical)
            {
                foreach (var child in Children)
                {
                    child.Arrange(new RectangleD(positionX, positionY, finalSize.Width, child.DesiredSize.Height));
                    positionY += child.DesiredSize.Height;
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    child.Arrange(new RectangleD(positionX, positionY, child.DesiredSize.Width, finalSize.Height));
                    positionX += child.DesiredSize.Width;
                }
            }

            return finalSize;
        }

        /// <inheritdoc/>
        protected override UIElement GetNextNavUp(UIElement current)
        {  
            if (Orientation == Orientation.Vertical)
            {
                var ixCurrent = Children.IndexOf(current);
                if (ixCurrent > 0)
                {
                    return Children[ixCurrent - 1];
                }
            }
            return null;
        }

        /// <inheritdoc/>
        protected override UIElement GetNextNavDown(UIElement current)
        {
            if (Orientation == Orientation.Vertical)
            {
                var ixCurrent = Children.IndexOf(current);
                if (ixCurrent + 1 < Children.Count)
                {
                    return Children[ixCurrent + 1];
                }
            }
            return null;
        }

        /// <inheritdoc/>
        protected override UIElement GetNextNavLeft(UIElement current)
        {
            if (Orientation == Orientation.Horizontal)
            {
                var ixCurrent = Children.IndexOf(current);
                if (ixCurrent > 0)
                {
                    return Children[ixCurrent - 1];
                }
            }
            return null;
        }

        /// <inheritdoc/>
        protected override UIElement GetNextNavRight(UIElement current)
        {
            if (Orientation == Orientation.Horizontal)
            {
                var ixCurrent = Children.IndexOf(current);
                if (ixCurrent + 1 < Children.Count)
                {
                    return Children[ixCurrent + 1];
                }
            }
            return null;
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
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleOrientationChanged(DependencyObject dobj)
        {
            var stackPanel = (StackPanel)dobj;
            stackPanel.OnOrientationChanged();
        }

        // State values.
        private Size2D contentSize;
    }
}
