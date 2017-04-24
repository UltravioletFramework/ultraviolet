
namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the ways in which a sprite batch can sort its sprites.
    /// </summary>
    public enum SpriteSortMode
    {
        /// <summary>
        /// Sprites are not drawn until the batch ends, and then are drawn in the order that they were added to the batch.
        /// </summary>
        Deferred,

        /// <summary>
        /// Sprites are rendered immediately, without any batching.
        /// </summary>
        Immediate,

        /// <summary>
        /// Sprites are sorted according to their texture.
        /// </summary>
        Texture,

        /// <summary>
        /// Sprites are sorted according to their layer depth, from back to front.
        /// </summary>
        BackToFront,

        /// <summary>
        /// Sprites are sorted according to the layer depth, from front to back.
        /// </summary>
        FrontToBack,
    }
}
