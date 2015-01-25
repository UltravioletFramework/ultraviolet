using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
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
        /// <param name="id">The element's unique identifier within its view.</param>
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
        public event UIElementEventHandler ViewportSizeChanged;

        /// <summary>
        /// Identifies the <see cref="ViewportSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewportSizeProperty = DependencyProperty.Register("ViewportSize", typeof(Double), typeof(ScrollBarBase),
            new DependencyPropertyMetadata(HandleViewportSizeChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Updates the layout of the scroll bar's components.
        /// </summary>
        protected abstract void UpdateScrollbarComponents();

        /// <inheritdoc/>
        protected override void OnValueChanged()
        {
            UpdateScrollbarComponents();
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
        /// Calculates the scroll thumb's offset.
        /// </summary>
        /// <returns>The scroll thumb's current offset.</returns>
        protected Double CalculateThumbOffset()
        {
            var val = Value;
            var min = Minimum;
            var max = Maximum;

            if (min == max)
                return 0;

            var available = ActualTrackLength - ActualThumbLength;
            var percent   = (val - min) / (max - min);
            var used      = available * percent;

            return used;
        }

        /// <summary>
        /// Updates the length of the scroll bar's thumb.
        /// </summary>
        /// <param name="minimumLength">The minimum length of the thumb.</param>
        /// <returns>The calculated thumb length.</returns>
        protected Double CalculateThumbLength(Double minimumLength)
        {
            var max = Maximum;
            var min = Minimum;
            var vps = ViewportSize;

            if (max - min + vps == 0)
                return 0;

            var lengthMin   = LayoutUtil.GetValidMeasure(minimumLength, 0);
            var lengthThumb = ((vps / (max - min + vps)) * ActualTrackLength);
            if (lengthThumb < lengthMin)
            {
                lengthThumb = lengthMin;
            }

            return lengthThumb;
        }

        /// <summary>
        /// Converts a range value to a pixel offset into the scroll bar's track.
        /// </summary>
        /// <param name="value">The range value to convert.</param>
        /// <returns>The converted pixel value.</returns>
        protected Double ValueToOffset(Double value)
        {
            var available = ActualTrackLength - ActualThumbLength;

            var min = Minimum;
            var max = Maximum;

            if (max == min)
                return 0;

            var percent = (value - min) / (max - min);
            var used    = available * percent;

            return used;
        }

        /// <summary>
        /// Converts a pixel offset into the scroll bar's track to a range value.
        /// </summary>
        /// <param name="pixels">The pixel value to convert.</param>
        /// <returns>The converted range value.</returns>
        protected Double OffsetToValue(Double pixels)
        {
            var available = ActualTrackLength - ActualThumbLength;

            var min = Minimum;
            var max = Maximum;

            if (max == min)
                return 0;

            var percent = pixels / available;
            var value   = (percent * (Maximum - Minimum)) + Minimum;

            return value;
        }

        /// <summary>
        /// Gets the offset from the left edge of the control to the left edge of the scroll bar's track.
        /// </summary>
        protected abstract Double ActualTrackOffsetX
        {
            get;
        }

        /// <summary>
        /// Gets the offset from the top edge of the control to the top edge of the scroll bar's track.
        /// </summary>
        protected abstract Double ActualTrackOffsetY
        {
            get;
        }

        /// <summary>
        /// Gets the width of the scroll bar's track.
        /// </summary>
        protected abstract Double ActualTrackWidth
        {
            get;
        }

        /// <summary>
        /// Gets the height of the scroll bar's track.
        /// </summary>
        protected abstract Double ActualTrackHeight
        {
            get;
        }

        /// <summary>
        /// Gets the length of the scroll bar's track.
        /// </summary>
        protected abstract Double ActualTrackLength
        {
            get;
        }

        /// <summary>
        /// Gets the length of the scroll bar's thumb.
        /// </summary>
        protected abstract Double ActualThumbLength
        {
            get;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ViewportSize"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleViewportSizeChanged(DependencyObject dobj)
        {
            var scrollbar = (ScrollBarBase)dobj;
            scrollbar.UpdateScrollbarComponents();
            scrollbar.OnViewportSizeChanged();
        }
    }
}
