using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents the base class for scroll bars.
    /// </summary>
    public abstract class ScrollBarBase : RangeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBarBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ScrollBarBase(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Gets or sets the amount of scrollable content that is currently visible.
        /// </summary>
        public Double ViewportSize
        {
            get { return GetValue<Double>(ViewportSizeProperty); }
            set { SetValue<Double>(ViewportSizeProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ViewportSize"/> property changes.
        /// </summary>
        public event UpfEventHandler ViewportSizeChanged;

        /// <summary>
        /// Identifies the <see cref="ViewportSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewportSizeProperty = DependencyProperty.Register("ViewportSize", typeof(Double), typeof(ScrollBarBase),
            new DependencyPropertyMetadata(HandleViewportSizeChanged, null, DependencyPropertyOptions.AffectsMeasure));

        /// <inheritdoc/>
        protected override void OnMinimumChanged()
        {
            InvalidateMeasure();
            base.OnMinimumChanged();
        }

        /// <inheritdoc/>
        protected override void OnMaximumChanged()
        {
            InvalidateMeasure();
            base.OnMaximumChanged();
        }

        /// <inheritdoc/>
        protected override void OnValueChanged()
        {
            if (Track != null)
            {
                Track.InvalidateArrange();
            }
            base.OnValueChanged();
        }

        /// <summary>
        /// Raises the <see cref="ViewportSize"/> event.
        /// </summary>
        protected virtual void OnViewportSizeChanged()
        {
            var temp = ViewportSizeChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ViewportSize"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleViewportSizeChanged(DependencyObject dobj)
        {
            var scrollbar = (ScrollBarBase)dobj;
            scrollbar.OnViewportSizeChanged();
        }

        // Component references.
        private readonly Track Track = null;
    }
}
