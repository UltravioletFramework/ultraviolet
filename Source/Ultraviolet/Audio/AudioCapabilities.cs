using System;

namespace Ultraviolet.Audio
{
    /// <summary>
    /// Encapsulates the capabilities of an audio device.
    /// </summary>
    public abstract class AudioCapabilities
    {
        /// <summary>
        /// Gets a value indicating whether the audio device supports shifting the pitch of songs and effects.
        /// </summary>
        public abstract Boolean SupportsPitchShifting { get; }
    }
}
