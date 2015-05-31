using System;

namespace TwistedLogik.Ultraviolet
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
            new Color(unchecked(0xffa65353)), 
            new Color(unchecked(0xffd99100)), 
            new Color(unchecked(0xff269963)), 
            new Color(unchecked(0xff606cbf)), 
            new Color(unchecked(0xffff80b3)), 
            new Color(unchecked(0xffff2200)), 
            new Color(unchecked(0xff7f7920)), 
            new Color(unchecked(0xff00f2a2)), 
            new Color(unchecked(0xff3636d9)), 
            new Color(unchecked(0xffa6295b)), 
            new Color(unchecked(0xff991400)), 
            new Color(unchecked(0xffc2cc33)), 
            new Color(unchecked(0xff80ffe5)), 
            new Color(unchecked(0xffee00ff)), 
            new Color(unchecked(0xffd9003a)), 
            new Color(unchecked(0xffffb380)), 
            new Color(unchecked(0xff338000)), 
            new Color(unchecked(0xff30b6bf)), 
            new Color(unchecked(0xffad59b3)), 
            new Color(unchecked(0xff7f4400)), 
            new Color(unchecked(0xff3df23d)), 
            new Color(unchecked(0xff39ace6)), 
            new Color(unchecked(0xffff00aa))
            #endregion
        };
    }
}
