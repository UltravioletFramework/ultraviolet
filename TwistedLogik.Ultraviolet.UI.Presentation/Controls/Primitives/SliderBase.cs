using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
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
        /// <param name="name">The element's identifying name within its namescope.</param>
        public SliderBase(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

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
            if (PART_Track != null)
            {
                PART_Track.InvalidateArrange();
            }
            base.OnValueChanged();
        }

        /// <summary>
        /// Converts a range value to a pixel offset into the scroll bar's slider.
        /// </summary>
        /// <param name="value">The range value to convert.</param>
        /// <param name="thumbSize">The size of the slider.</param>
        /// <param name="sliderSize">The size of the thumb.</param>
        /// <returns>The converted pixel value.</returns>
        protected Double ValueToOffset(Double value, Double sliderSize, Double thumbSize)
        {
            var available = sliderSize - thumbSize;

            var min = Minimum;
            var max = Maximum;

            if (max == min)
                return 0;

            var percent = (value - min) / (max - min);
            var used    = available * percent;

            return used;
        }

        /// <summary>
        /// Converts a pixel offset into the scroll bar's slider to a range value.
        /// </summary>
        /// <param name="pixels">The pixel value to convert.</param>
        /// <param name="thumbSize">The size of the slider.</param>
        /// <param name="sliderSize">The size of the thumb.</param>
        /// <returns>The converted range value.</returns>
        protected Double OffsetToValue(Double pixels, Double sliderSize, Double thumbSize)
        {
            var available = sliderSize - thumbSize;

            var min = Minimum;
            var max = Maximum;

            if (max == min)
                return 0;

            var percent = pixels / available;
            var value   = (percent * (Maximum - Minimum)) + Minimum;

            return value;
        }
        
        /// <summary>
        /// Calculates the offset of the slider's thumb.
        /// </summary>
        /// <param name="sliderSize">The size of the slider.</param>
        /// <param name="thumbSize">The size of the thumb.</param>
        /// <returns>The offset of the slider's thumb.</returns>
        protected Double CalculateThumbOffset(Double sliderSize, Double thumbSize)
        {
            var val = Value;
            var min = Minimum;
            var max = Maximum;

            if (min == max)
            {
                return 0;
            }

            var available = sliderSize - thumbSize;
            var percent   = (val - min) / (max - min);
            var used      = available * percent;

            return Math.Floor(used);
        }

        // Component references.
        private readonly Track PART_Track = null;
    }
}
