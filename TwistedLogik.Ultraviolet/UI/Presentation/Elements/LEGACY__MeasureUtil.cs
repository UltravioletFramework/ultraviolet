using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Contains helper methods for performing UI measurements.
    /// </summary>
    public static class MeasureUtil
    {        
        /// <summary>
        /// Converts a <see cref="Thickness"/> value given in device independent pixels (1/96 of an inch) to device pixels.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="thickness">The bounding rectangle to convert.</param>
        /// <param name="nan">The value to substitute for any of the bounding rectangle's parameters if that parameter is not a number.</param>
        /// <returns>The converted <see cref="Thickness"/> value.</returns>
        public static Thickness ConvertThicknessToPixels(UltravioletContext uv, Thickness thickness, Double nan)
        {
            Contract.Require(uv, "uv");

            var display = uv.GetPlatform().Displays.PrimaryDisplay;

            var left   = ConvertMeasureToPixelsInternal(display, thickness.Left, nan);
            var top    = ConvertMeasureToPixelsInternal(display, thickness.Top, nan);
            var right  = ConvertMeasureToPixelsInternal(display, thickness.Right, nan);
            var bottom = ConvertMeasureToPixelsInternal(display, thickness.Bottom, nan);

            return new Thickness(left, top, right, bottom);
        }

        /// <summary>
        /// Converts a <see cref="Thickness"/> value given in device independent pixels (1/96 of an inch) to device pixels.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="thickness">The bounding rectangle to convert.</param>
        /// <param name="posInf">The value to substitute for any of the bounding rectangle's parameters if that parameter is positive infinity.</param>
        /// <param name="negInf">The value to substitute for any of the bounding rectangle's parameters if that parameter is negative infinity.</param>
        /// <param name="nan">The value to substitute for any of the bounding rectangle's parameters if that parameter is not a number.</param>
        /// <returns>The converted <see cref="Thickness"/> value.</returns>
        public static Thickness ConvertThicknessToPixels(UltravioletContext uv, Thickness thickness, Double posInf, Double negInf, Double nan)
        {
            Contract.Require(uv, "uv");

            var display = uv.GetPlatform().Displays.PrimaryDisplay;

            var left   = ConvertMeasureToPixelsInternal(display, thickness.Left, posInf, negInf, nan);
            var top    = ConvertMeasureToPixelsInternal(display, thickness.Top, posInf, negInf, nan);
            var right  = ConvertMeasureToPixelsInternal(display, thickness.Right, posInf, negInf, nan);
            var bottom = ConvertMeasureToPixelsInternal(display, thickness.Bottom, posInf, negInf, nan);

            return new Thickness(left, top, right, bottom);
        }

        /// <summary>
        /// Converts a measure given in device independent pixels (1/96 of an inch) to device pixels.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="measure">The measure to convert.</param>
        /// <param name="nan">The value to substitute for <paramref name="measure"/> if <paramref name="measure"/> is not a number.</param>
        /// <returns>The converted measure value.</returns>
        public static Double ConvertMeasureToPixels(UltravioletContext uv, Double measure, Double nan)
        {
            Contract.Require(uv, "uv");

            var display = uv.GetPlatform().Displays.PrimaryDisplay;

            if (Double.IsPositiveInfinity(measure))
                return Int32.MaxValue;

            if (Double.IsNegativeInfinity(measure))
                return Int32.MinValue;

            if (Double.IsNaN(measure))
                return display.DipsToPixels(nan);

            return display.DipsToPixels(measure);
        }

        /// <summary>
        /// Converts a measure given in device independent pixels (1/96 of an inch) to device pixels.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="measure">The measure to convert.</param>
        /// <param name="posInf">The value to substitute for <paramref name="measure"/> if <paramref name="measure"/> is positive infinity.</param>
        /// <param name="negInf">The value to substitute for <paramref name="measure"/> if <paramref name="measure"/> is negative infinity.</param>
        /// <param name="nan">The value to substitute for <paramref name="measure"/> if <paramref name="measure"/> is not a number.</param>
        /// <returns>The converted measure value.</returns>
        public static Double ConvertMeasureToPixels(UltravioletContext uv, Double measure, Double posInf, Double negInf, Double nan)
        {
            Contract.Require(uv, "uv");

            var display = uv.GetPlatform().Displays.PrimaryDisplay;

            if (Double.IsPositiveInfinity(measure))
                return display.DipsToPixels(posInf);

            if (Double.IsNegativeInfinity(measure))
                return display.DipsToPixels(negInf);

            if (Double.IsNaN(measure))
                return display.DipsToPixels(nan);

            return display.DipsToPixels(measure);
        }

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
        
        /// <summary>
        /// Converts a measure given in device independent pixels (1/96 of an inch) to device pixels.
        /// </summary>
        /// <param name="measure">The measure to convert.</param>
        /// <param name="nan">The value to substitute for <paramref name="measure"/> if <paramref name="measure"/> is not a number.</param>
        /// <returns>The converted measure value.</returns>
        private static Double ConvertMeasureToPixelsInternal(IUltravioletDisplay display, Double measure, Double nan)
        {
            if (Double.IsPositiveInfinity(measure))
                return Int32.MaxValue;

            if (Double.IsNegativeInfinity(measure))
                return Int32.MinValue;

            if (Double.IsNaN(measure))
                return display.DipsToPixels(nan);

            return display.DipsToPixels(measure);
        }

        /// <summary>
        /// Converts a measure given in device independent pixels (1/96 of an inch) to device pixels.
        /// </summary>
        /// <param name="measure">The measure to convert.</param>
        /// <param name="posInf">The value to substitute for <paramref name="measure"/> if <paramref name="measure"/> is positive infinity.</param>
        /// <param name="negInf">The value to substitute for <paramref name="measure"/> if <paramref name="measure"/> is negative infinity.</param>
        /// <param name="nan">The value to substitute for <paramref name="measure"/> if <paramref name="measure"/> is not a number.</param>
        /// <returns>The converted measure value.</returns>
        private static Double ConvertMeasureToPixelsInternal(IUltravioletDisplay display, Double measure, Double posInf, Double negInf, Double nan)
        {
            if (Double.IsPositiveInfinity(measure))
                return display.DipsToPixels(posInf);

            if (Double.IsNegativeInfinity(measure))
                return display.DipsToPixels(negInf);

            if (Double.IsNaN(measure))
                return display.DipsToPixels(nan);

            return display.DipsToPixels(measure);
        }
    }
}
