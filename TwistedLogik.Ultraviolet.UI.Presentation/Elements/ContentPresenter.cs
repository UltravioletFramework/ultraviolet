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

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            if (Control != null)
            {
                Control.OnContentPresenterDraw(time, dc);
            }
            base.DrawOverride(time, dc);
        }

        /// <inheritdoc/>
        protected override void UpdateOverride(UltravioletTime time)
        {
            if (Control != null)
            {
                Control.OnContentPresenterUpdate(time);
            }
            base.UpdateOverride(time);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (Control == null)
                return Size2D.Zero;

            var contentWidth  = UnconstrainedWidth ? Double.PositiveInfinity : availableSize.Width;
            var contentHeight = UnconstrainedHeight ? Double.PositiveInfinity : availableSize.Height;

            return Control.OnContentPresenterMeasure(new Size2D(contentWidth, contentHeight));
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            if (Control == null)
                return Size2D.Zero;

            return Control.OnContentPresenterArrange(finalSize, options);
        }

        /// <inheritdoc/>
        protected override void PositionOverride(Point2D position)
        {
            if (Control != null)
            {
                Control.OnContentPresenterPosition(AbsolutePosition);
            }
            base.PositionOverride(position);
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
