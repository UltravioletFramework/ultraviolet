
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the stencil buffer operations.
    /// </summary>
    public enum StencilOperation
    {
        /// <summary>
        /// Decrements the stencil buffer value, wrapping to the maximum value if the new value is less than 0.
        /// </summary>
        Decrement,

        /// <summary>
        /// Decrements the stencil buffer value, clamping to 0.
        /// </summary>
        DecrementSaturation,

        /// <summary>
        /// Increments the stencil buffer value, wrapping to the minimum value if the new value exceeds the maximum value.
        /// </summary>
        Increment,

        /// <summary>
        /// Increments the stencil buffer value, clamping to the maximum value.
        /// </summary>
        IncrementSaturation,

        /// <summary>
        /// Inverts the bits of the stencil buffer value.
        /// </summary>
        Invert,

        /// <summary>
        /// Preserves the stencil buffer value.
        /// </summary>
        Keep,

        /// <summary>
        /// Replaces the stencil buffer value with the reference value.
        /// </summary>
        Replace,

        /// <summary>
        /// Sets the stencil buffer value to zero.
        /// </summary>
        Zero,
    }
}
