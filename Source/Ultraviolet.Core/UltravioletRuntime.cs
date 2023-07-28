namespace Ultraviolet.Core
{
    /// <summary>
    /// Represents the .NET runtime implementations which are supported by the Ultraviolet Framework.
    /// </summary>
    public enum UltravioletRuntime
    {
        /// <summary>
        /// The Microsoft CLR.
        /// </summary>
        CLR,

        /// <summary>
        /// The .NET Core runtime.
        /// </summary>
        CoreCLR,

        /// <summary>
        /// The Mono runtime.
        /// </summary>
        Mono,
    }
}
