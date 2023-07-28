namespace Ultraviolet.BASS
{
    /// <summary>
    /// Contains the implementation's Ultraviolet engine events.
    /// </summary>
    public static class BASSUltravioletMessages
    {
        /// <summary>
        /// An event indicating that the current BASS device has changed.
        /// </summary>
        public static readonly UltravioletMessageID BASSDeviceChanged = UltravioletMessageID.Acquire(nameof(BASSDeviceChanged));
    }
}
