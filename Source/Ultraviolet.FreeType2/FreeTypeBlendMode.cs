namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents the blending modes which can be used to blit glyphs onto a surface.
    /// </summary>
    internal enum FreeTypeBlendMode
    {
        /// <summary>
        /// The glyph data should be treated as opaque.
        /// </summary>
        Opaque,

        /// <summary>
        /// The glyph data should be alpha blended onto the surface.
        /// </summary>
        Blend,
    }
}
