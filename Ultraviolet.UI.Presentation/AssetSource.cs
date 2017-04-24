
namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a flag indicating which of a UI's content managers an asset should be loaded from.
    /// </summary>
    public enum AssetSource
    {
        /// <summary>
        /// The asset is globally-sourced.
        /// </summary>
        Global,

        /// <summary>
        /// The asset is locally-sourced.
        /// </summary>
        Local,
    }
}
