namespace Ultraviolet.Presentation.Media
{
    /// <summary>
    /// Specifies the rendering modes which can be used to render text.
    /// </summary>
    public enum TextRenderingMode
    {
        /// <summary>
        /// The text is rendered normally.
        /// </summary>
        Auto,

        /// <summary>
        /// The text is rendered after being passed through a shaping buffer.
        /// </summary>
        Shaped,
    }
}
