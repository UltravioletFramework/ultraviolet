using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents the base class for scroll bar tracks.
    /// </summary>
    public abstract class ScrollTrackBase : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollTrackBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public ScrollTrackBase(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Gets or sets the size of the track's associated viewport.
        /// </summary>
        public Double ViewportSize
        {
            get
            {
                var scrollbar = Control as ScrollBarBase;
                return (scrollbar == null) ? 0 : scrollbar.ViewportSize;
            }
            set
            {
                var scrollbar = Control as ScrollBarBase;
                if (scrollbar != null)
                {
                    scrollbar.ViewportSize = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the track's minimum value.
        /// </summary>
        public Double Minimum
        {
            get
            {
                var scrollbar = Control as RangeBase;
                return (scrollbar == null) ? 0 : scrollbar.Minimum;
            }
            set
            {
                var scrollbar = Control as RangeBase;
                if (scrollbar != null)
                {
                    scrollbar.Minimum = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the track's maximum value.
        /// </summary>
        public Double Maximum
        {
            get
            {
                var scrollbar = Control as RangeBase;
                return (scrollbar == null) ? 0 : scrollbar.Maximum;
            }
            set
            {
                var scrollbar = Control as RangeBase;
                if (scrollbar != null)
                {
                    scrollbar.Maximum = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the track's current value.
        /// </summary>
        public Double Value
        {
            get
            {
                var scrollbar = Control as RangeBase;
                return (scrollbar == null) ? 0 : scrollbar.Value;
            }
            set
            {
                var scrollbar = Control as RangeBase;
                if (scrollbar != null)
                {
                    scrollbar.Value = value;
                }
            }
        }

        /// <summary>
        /// Converts a range value to a pixel offset into the scroll bar's track.
        /// </summary>
        /// <param name="value">The range value to convert.</param>
        /// <param name="thumbSize">The size of the track.</param>
        /// <param name="trackSize">The size of the thumb.</param>
        /// <returns>The converted pixel value.</returns>
        protected Double ValueToOffset(Double value, Double trackSize, Double thumbSize)
        {
            var available = trackSize - thumbSize;

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
        /// <param name="thumbSize">The size of the track.</param>
        /// <param name="trackSize">The size of the thumb.</param>
        /// <returns>The converted range value.</returns>
        protected Double OffsetToValue(Double pixels, Double trackSize, Double thumbSize)
        {
            var available = trackSize - thumbSize;

            var min = Minimum;
            var max = Maximum;

            if (max == min)
                return 0;

            var percent = pixels / available;
            var value   = (percent * (Maximum - Minimum)) + Minimum;

            return value;
        }

        /// <summary>
        /// Calculates the size of the track's thumb.
        /// </summary>
        /// <param name="trackSize">The size of the track.</param>
        /// <param name="thumbSizeMin">The minimum possible size of the thumb.</param>
        /// <returns>The size of the track's thumb.</returns>
        protected Double CalculateThumbWidth(Double trackSize, Double thumbSizeMin)
        {
            if (Double.IsPositiveInfinity(trackSize))
                return 0;

            var max = Maximum;
            var min = Minimum;
            var vps = ViewportSize;

            if (max - min + vps == 0)
            {
                return 0;
            }
            else
            {
                var thumbSize = ((vps / (max - min + vps)) * trackSize);
                if (thumbSize < thumbSizeMin)
                {
                    thumbSize = thumbSizeMin;
                }
                return Math.Ceiling(thumbSize);
            }
        }

        /// <summary>
        /// Calculates the offset of the track's thumb.
        /// </summary>
        /// <param name="trackSize">The size of the track.</param>
        /// <param name="thumbSize">The size of the thumb.</param>
        /// <returns>The offset of the track's thumb.</returns>
        protected Double CalculateThumbOffset(Double trackSize, Double thumbSize)
        {
            var val = Value;
            var min = Minimum;
            var max = Maximum;

            if (min == max)
            {
                return 0;
            }

            var available = trackSize - thumbSize;
            var percent   = (val - min) / (max - min);
            var used      = available * percent;

            return Math.Floor(used);
        }

        /// <summary>
        /// Handles a <see cref="ButtonBase.Click"/> that decreases the track's value.
        /// </summary>
        protected void HandleDecreaseLarge(UIElement element)
        {
            var scrollbar = Control as RangeBase;
            if (scrollbar != null)
            {
                scrollbar.DecreaseLarge();
            }
        }

        /// <summary>
        /// Handles a <see cref="ButtonBase.Click"/> that increases the track's value.
        /// </summary>
        protected void HandleIncreaseLarge(UIElement small)
        {
            var scrollbar = Control as RangeBase;
            if (scrollbar != null)
            {
                scrollbar.IncreaseLarge();
            }
        }
    }
}
