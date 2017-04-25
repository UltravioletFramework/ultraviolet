
namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the type of value stored by an instance of the <see cref="GridLength"/> structure.
    /// </summary>
    public enum GridUnitType
    {
        /// <summary>
        /// Size is calculated based on the size of the content.
        /// </summary>
        Auto,

        /// <summary>
        /// Size is expressed as a value in pixels.
        /// </summary>
        Pixel,

        /// <summary>
        /// Size is expressed as a weighted proportion of available space.
        /// </summary>
        Star,
    }
}
