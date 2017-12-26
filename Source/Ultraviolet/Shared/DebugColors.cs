using System;

namespace Ultraviolet
{
    /// <summary>
    /// Contains a sequence of visually distinct colors useful for rendering debug graphics.
    /// </summary>
    internal static class DebugColors
    {
        /// <summary>
        /// Gets a color from the debug sequence.
        /// </summary>
        /// <param name="ix">The index of the color to retrieve.</param>
        /// <returns>The specified debug color.</returns>
        public static Color Get(Int32 ix)
        {
            return colors[ix % colors.Length];
        }

        // State values.
        private static readonly Color[] colors = new[] 
        {
            #region Colors
            Color.FromArgb(0xffa65353),
            Color.FromArgb(0xffd99100),
            Color.FromArgb(0xff269963),
            Color.FromArgb(0xff606cbf),
            Color.FromArgb(0xffff80b3),
            Color.FromArgb(0xffff2200),
            Color.FromArgb(0xff7f7920),
            Color.FromArgb(0xff00f2a2), 
            Color.FromArgb(0xff3636d9), 
            Color.FromArgb(0xffa6295b), 
            Color.FromArgb(0xff991400), 
            Color.FromArgb(0xffc2cc33), 
            Color.FromArgb(0xff80ffe5), 
            Color.FromArgb(0xffee00ff), 
            Color.FromArgb(0xffd9003a), 
            Color.FromArgb(0xffffb380), 
            Color.FromArgb(0xff338000), 
            Color.FromArgb(0xff30b6bf), 
            Color.FromArgb(0xffad59b3), 
            Color.FromArgb(0xff7f4400), 
            Color.FromArgb(0xff3df23d), 
            Color.FromArgb(0xff39ace6), 
            Color.FromArgb(0xffff00aa)
            #endregion
        };
    }
}
