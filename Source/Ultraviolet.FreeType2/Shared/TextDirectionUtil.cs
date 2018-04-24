using Ultraviolet.FreeType2.Native;
using Ultraviolet.Graphics.Graphics2D.Text;

namespace Ultraviolet.FreeType2
{
#pragma warning disable 1591
    internal static class TextDirectionUtil
    {
        /// <summary>
        /// Converts a <see cref="TextDirection"/> value to an <see cref="hb_direction_t"/> value.
        /// </summary>
        /// <param name="direction">The direction to convert.</param>
        /// <returns>The converted direction.</returns>
        public static hb_direction_t UltravioletToHarfBuzz(TextDirection direction)
        {
            switch (direction)
            {
                case TextDirection.LeftToRight:
                    return hb_direction_t.HB_DIRECTION_LTR;
                case TextDirection.RightToLeft:
                    return hb_direction_t.HB_DIRECTION_RTL; 
               case TextDirection.TopToBottom:
                    return hb_direction_t.HB_DIRECTION_TTB;
                case TextDirection.BottomToTop:
                    return hb_direction_t.HB_DIRECTION_BTT;
                default:
                    return hb_direction_t.HB_DIRECTION_INVALID;
            }
        }

        /// <summary>
        /// Converts an <see cref="hb_direction_t"/> value to a <see cref="TextDirection"/> value.
        /// </summary>
        /// <param name="direction">The direction to convert.</param>
        /// <returns>The converted direction.</returns>
        public static TextDirection HarfBuzzToUltraviolet(hb_direction_t direction)
        {
            switch (direction)
            {
                case hb_direction_t.HB_DIRECTION_LTR:
                    return TextDirection.LeftToRight;
                case hb_direction_t.HB_DIRECTION_RTL:
                    return TextDirection.RightToLeft;
                case hb_direction_t.HB_DIRECTION_TTB:
                    return TextDirection.TopToBottom;
                case hb_direction_t.HB_DIRECTION_BTT:
                    return TextDirection.BottomToTop;
                default:
                    return TextDirection.Invalid;
            }
        }
    }
#pragma warning restore 1591
}
