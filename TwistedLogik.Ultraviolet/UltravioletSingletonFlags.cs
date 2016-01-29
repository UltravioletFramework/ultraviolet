namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a collection of flags which modify the behavior of an instance of
    /// the <see cref="UltravioletSingleton{T}"/> class.
    /// </summary>
    public enum UltravioletSingletonFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// The singleton is disabled when the context is in service mode.
        /// </summary>
        DisabledInServiceMode = 0x0001,
    }
}
