using System;
using Ultraviolet.Audio;

namespace Ultraviolet.BASS.Audio
{
    /// <inheritdoc/>
    public sealed class BASSAudioCapabilities : AudioCapabilities
    {
        /// <inheritdoc/>
        public override Boolean SupportsPitchShifting { get; } = false;
    }
}
