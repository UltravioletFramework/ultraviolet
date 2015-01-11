using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Contains helper methods for performing UI measurements.
    /// </summary>
    public static class MeasureUtil
    {
        /// <summary>
        /// Positions an item within its layout area based on its horizontal alignment.
        /// </summary>
        /// <param name="alignment">A <see cref="HorizontalAlignment"/> value specifying how the item should be laid out.</param>
        /// <param name="itemWidth">The width of the item being laid out, in pixels.</param>
        /// <param name="availableWidth">The width of the space which is available for layout, in pixels.</param>
        /// <returns>The specified item's horizontal offset within its layout area.</returns>
        public static Int32 AlignHorizontally(HorizontalAlignment alignment, Int32 itemWidth, Int32 availableWidth)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Center:
                    return (availableWidth - itemWidth) / 2;

                case HorizontalAlignment.Stretch:
                case HorizontalAlignment.Left:
                    return 0;

                case HorizontalAlignment.Right:
                    return availableWidth - itemWidth;

                default:
                    throw new NotSupportedException("alignment");
            }
        }

        /// <summary>
        /// Positions an item within its layout area based on its vertical alignment.
        /// </summary>
        /// <param name="alignment">A <see cref="HorizontalAlignment"/> value specifying how the item should be laid out.</param>
        /// <param name="itemHeight">The height of the item being laid out, in pixels.</param>
        /// <param name="availableHeight">The height of the space which is available for layout, in pixels.</param>
        /// <returns>The specified item's vertical offset within its layout area.</returns>
        public static Int32 AlignVertically(VerticalAlignment alignment, Int32 itemHeight, Int32 availableHeight)
        {
            switch (alignment)
            {
                case VerticalAlignment.Center:
                    return (availableHeight - itemHeight) / 2;

                case VerticalAlignment.Stretch:
                case VerticalAlignment.Top:
                    return 0;

                case VerticalAlignment.Bottom:
                    return availableHeight - itemHeight;

                default:
                    throw new NotSupportedException("alignment");
            }
        }

    }
}
