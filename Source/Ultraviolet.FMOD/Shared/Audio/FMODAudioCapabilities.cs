using System;
using Ultraviolet.Audio;

namespace Ultraviolet.FMOD.Audio
{
    /// <inheritdoc/>
    public sealed class FMODAudioCapabilities : AudioCapabilities
    {
        /// <inheritdoc/>
        public override Boolean SupportsPitchShifting { get; } = true;
    }
}
