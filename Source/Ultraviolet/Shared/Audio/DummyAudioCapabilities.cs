using System;

namespace Ultraviolet.Audio
{
    /// <summary>
    /// Represents a dummy implementation of <see cref="AudioCapabilities"/>.
    /// </summary>
    public sealed class DummyAudioCapabilities : AudioCapabilities
    {
        /// <inheritdoc/>
        public override Boolean SupportsPitchShifting { get; } = false;
    }
}
