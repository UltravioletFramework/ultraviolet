
namespace Ultraviolet
{
    /// <summary>
    /// Represents an application component which participates in an Ultraviolet context.
    /// </summary>
    public interface IUltravioletComponent
    {
        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        UltravioletContext Ultraviolet { get; }
    }
}
