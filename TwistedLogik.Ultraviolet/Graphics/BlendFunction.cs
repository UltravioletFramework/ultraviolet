
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the arithmetic functions that may be used when blending alpha values.
    /// </summary>
    public enum BlendFunction
    {
        /// <summary>
        /// The source value is added to the destination value.
        /// </summary>
        Add,

        /// <summary>
        /// The maximum of the source and destination values.
        /// </summary>
        Max,

        /// <summary>
        /// The minimum of the source and destination values is used.
        /// </summary>
        Min,

        /// <summary>
        /// The source value is subtracted from the destination value.
        /// </summary>
        ReverseSubtract,

        /// <summary>
        /// The destination value is subtracted from the source value.
        /// </summary>
        Subtract,
    }
}
