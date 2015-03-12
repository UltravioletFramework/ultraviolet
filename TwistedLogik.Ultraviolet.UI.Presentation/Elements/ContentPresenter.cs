using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents an element which is used to indicate the position of child content within a component template.
    /// </summary>
    [UIElement("ContentPresenter")]
    public sealed class ContentPresenter : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentPresenter"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public ContentPresenter(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Gets or sets a value indicating whether the content presenter's content is unconstrained in its width.
        /// If this property is <c>true</c>, a value of <see cref="Double.PositiveInfinity"/> is passed to the presenter's
        /// content during the measure phase of layout.
        /// </summary>
        public Boolean UnconstrainedWidth
        {
            get { return GetValue<Boolean>(UnconstrainedWidthProperty); }
            set { SetValue<Boolean>(UnconstrainedWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the content presenter's content is unconstrained in its height.
        /// If this property is <c>true</c>, a value of <see cref="Double.PositiveInfinity"/> is passed to the presenter's
        /// content during the measure phase of layout.
        /// </summary>
        public Boolean UnconstrainedHeight
        {
            get { return GetValue<Boolean>(UnconstrainedHeightProperty); }
            set { SetValue<Boolean>(UnconstrainedHeightProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="UnconstrainedWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UnconstrainedWidthProperty = DependencyProperty.Register("UnconstrainedWidth", typeof(Boolean), typeof(ContentPresenter),
            new DependencyPropertyMetadata(HandleUnconstrainedWidthChanged, null, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="UnconstrainedHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UnconstrainedHeightProperty = DependencyProperty.Register("UnconstrainedHeight", typeof(Boolean), typeof(ContentPresenter),
            new DependencyPropertyMetadata(HandleUnconstrainedHeightChanged, null, DependencyPropertyOptions.AffectsMeasure));

        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var owner = Control as ContentControl;
            if (owner == null)
                return Size2D.Zero;

            var content = owner.Content as UIElement;
            if (content == null)
                return Size2D.Zero;

            content.Measure(availableSize);
            return content.DesiredSize;
        }

        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            var owner = Control as ContentControl;
            if (owner == null)
                return Size2D.Zero;

            var content = owner.Content as UIElement;
            if (content == null)
                return Size2D.Zero;

            content.Arrange(new RectangleD(0, 0, finalSize.Width, finalSize.Height), options);
            return content.RenderSize;
        }

        protected internal override UIElement GetLogicalChild(int childIndex)
        {
            var owner = Control as ContentControl;
            if (owner == null || owner.TreatContentAsLogicalChild)
                throw new ArgumentOutOfRangeException("childIndex");

            var content = owner.Content as UIElement;
            if (content == null || childIndex != 0)
                throw new ArgumentOutOfRangeException("childIndex");

            return content;
        }

        protected internal override UIElement GetVisualChild(int childIndex)
        {
            var owner = Control as ContentControl;
            if (owner == null)
                throw new ArgumentOutOfRangeException("childIndex");

            var content = owner.Content as UIElement;
            if (content == null || childIndex != 0)
                throw new ArgumentOutOfRangeException("childIndex");

            return content;
        }

        protected internal override Int32 LogicalChildrenCount
        {
            get 
            {
                var owner = Control as ContentControl;
                if (owner == null)
                    return 0;

                return owner.TreatContentAsLogicalChild ? 0 : ((owner.Content is UIElement) ? 1 : 0);
            }
        }

        protected internal override Int32 VisualChildrenCount
        {
            get 
            {
                var owner = Control as ContentControl;
                if (owner == null)
                    return 0;

                return (owner.Content is UIElement) ? 1 : 0;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="UnconstrainedWidth"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleUnconstrainedWidthChanged(DependencyObject dobj)
        {
            var presenter = (ContentPresenter)dobj;
            presenter.InvalidateMeasure();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="UnconstrainedHeight"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleUnconstrainedHeightChanged(DependencyObject dobj)
        {
            var presenter = (ContentPresenter)dobj;
            presenter.InvalidateMeasure();
        }
    }
}
