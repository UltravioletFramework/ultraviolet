
namespace Ultraviolet.SDL2
{
    /// <summary>
    /// Contains the implementation's Ultraviolet engine events.
    /// </summary>
    public static class SDL2UltravioletMessages
    {
        /// <summary>
        /// An event indicating that an SDL event was raised.
        /// </summary>
        public static readonly UltravioletMessageID SDLEvent = UltravioletMessageID.Acquire("SDLEvent");
    }
}
