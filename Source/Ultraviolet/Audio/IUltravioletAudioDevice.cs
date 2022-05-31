using System;

namespace Ultraviolet.Audio
{
    /// <summary>r
    /// Represents one of the system's attached audio devices.
    /// </summary>
    public interface IUltravioletAudioDevice
    {
        /// <summary>
        /// Gets the device's name.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets a value indicating whether this is the system's default device.
        /// </summary>
        Boolean IsDefault { get; }
        
        /// <summary>
        /// Gets a value indicating whether this device is currently valid.
        /// </summary>
        Boolean IsValid { get; }
    }
}
