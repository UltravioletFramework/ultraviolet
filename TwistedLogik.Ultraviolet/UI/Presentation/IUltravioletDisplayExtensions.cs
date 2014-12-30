using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains extension methods for the <see cref="IUltravioletDisplay"/> interface.
    /// </summary>
    public static class IUltravioletDisplayExtensions
    {
        /// <summary>
        /// Converts a <see cref="Thickness"/> given in inches to a <see cref="Thickness"/> given in display pixels.
        /// </summary>
        /// <param name="this">The display with which to perform the conversion.</param>
        /// <param name="thicknessInInches">The value in inches to convert.</param>
        /// <returns>The converted value in display units.</returns>
        public static Thickness InchesToPixels(this IUltravioletDisplay @this, Thickness thicknessInInches)
        {
            Contract.Require(@this, "this");

            var left   = @this.InchesToPixels(thicknessInInches.Left);
            var top    = @this.InchesToPixels(thicknessInInches.Top);
            var right  = @this.InchesToPixels(thicknessInInches.Right);
            var bottom = @this.InchesToPixels(thicknessInInches.Bottom);
            return new Thickness(left, top, right, bottom);
        }

        /// <summary>
        /// Converts a <see cref="Thickness"/> given in display independent pixels (1/96 of an inch) 
        /// to a <see cref="Thickness"/> given in display pixels.
        /// </summary>
        /// <param name="this">The display with which to perform the conversion.</param>
        /// <param name="thicknessInDips">The value in display independent units to convert.</param>
        /// <returns>The converted value in display units.</returns>
        public static Thickness DipsToPixels(this IUltravioletDisplay @this, Thickness thicknessInDips)
        {
            Contract.Require(@this, "this");

            var left   = @this.DipsToPixels(thicknessInDips.Left);
            var top    = @this.DipsToPixels(thicknessInDips.Top);
            var right  = @this.DipsToPixels(thicknessInDips.Right);
            var bottom = @this.DipsToPixels(thicknessInDips.Bottom);
            return new Thickness(left, top, right, bottom);
        }

        /// <summary>
        /// Converts a <see cref="Thickness"/> given in inches to a <see cref="Thickness"/> given in 
        /// display independent pixels (1/96 of an inch).
        /// </summary>
        /// <param name="this">The display with which to perform the conversion.</param>
        /// <param name="thicknessInInches">The value in inches to convert.</param>
        /// <returns>The converted value in display independent units.</returns>
        public static Thickness InchesToDips(this IUltravioletDisplay @this, Thickness thicknessInInches)
        {
            Contract.Require(@this, "this");

            var left   = @this.InchesToDips(thicknessInInches.Left);
            var top    = @this.InchesToDips(thicknessInInches.Top);
            var right  = @this.InchesToDips(thicknessInInches.Right);
            var bottom = @this.InchesToDips(thicknessInInches.Bottom);
            return new Thickness(left, top, right, bottom);
        }

        /// <summary>
        /// Converts a <see cref="Thickness"/> given in display pixels to a <see cref="Thickness"/> given 
        /// in display independent pixels (1/96 of an inch).
        /// </summary>
        /// <param name="this">The display with which to perform the conversion.</param>
        /// <param name="thicknessInPixels">The value in display units to convert.</param>
        /// <returns>The converted value in display independent units.</returns>
        public static Thickness PixelsToDips(this IUltravioletDisplay @this, Thickness thicknessInPixels)
        {
            Contract.Require(@this, "this");

            var left   = @this.PixelsToDips(thicknessInPixels.Left);
            var top    = @this.PixelsToDips(thicknessInPixels.Top);
            var right  = @this.PixelsToDips(thicknessInPixels.Right);
            var bottom = @this.PixelsToDips(thicknessInPixels.Bottom);
            return new Thickness(left, top, right, bottom);
        }
    }
}
