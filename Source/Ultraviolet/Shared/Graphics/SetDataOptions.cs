
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a hint to the underlying driver as to whether buffer data will be overwritten by a <c>SetData()</c> operation.
    /// </summary>
    public enum SetDataOptions
    {
        /// <summary>
        /// No hint provided.
        /// </summary>
        None,

        /// <summary>
        /// The SetData() operation will discard the contents of the buffer.
        /// </summary>
        Discard,

        /// <summary>
        /// The SetData() operation will not overwrite existing data.
        /// </summary>
        NoOverwrite,
    }
}
