using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents the base class for sliders.
    /// </summary>
    public abstract class SliderBase : RangeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SliderBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public SliderBase(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Updates the layout of the slider's components.
        /// </summary>
        protected abstract void PositionSliderComponents();

        /// <inheritdoc/>
        protected override void PositionOverride(Point2D position)
        {
            base.PositionOverride(position);

            PositionSliderComponents();
        }

        /// <inheritdoc/>
        protected override void OnValueChanged()
        {
            PositionSliderComponents();
            base.OnValueChanged();
        }

        /// <summary>
        /// Calculates the scroll thumb's offset.
        /// </summary>
        /// <param name="thumbLength">The length of the scroll thumb.</param>
        /// <returns>The scroll thumb's current offset.</returns>
        protected Double CalculateThumbOffset(Double thumbLength)
        {
            var val = Value;
            var min = Minimum;
            var max = Maximum;

            if (min == max)
                return 0;

            var available = ActualTrackLength - thumbLength;
            var percent   = (val - min) / (max - min);
            var used      = available * percent;

            return used;
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
    }
}
