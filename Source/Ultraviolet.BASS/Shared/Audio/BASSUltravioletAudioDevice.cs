using System;
using Ultraviolet.Audio;
using Ultraviolet.Core;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Represents the BASS implementation of the <see cref="IUltravioletAudioDevice"/> interface.
    /// </summary>
    public sealed class BASSUltravioletAudioDevice : IUltravioletAudioDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BASSUltravioletAudioDevice"/> class.
        /// </summary>
        /// <param name="name">The device's name.</param>
        public BASSUltravioletAudioDevice(String name)
        {
            Contract.Require(name, nameof(name));

            this.Name = name;
        }

        /// <inheritdoc/>
        public String Name { get; }

        /// <inheritdoc/>
        public Boolean IsDefault { get; internal set; }

        /// <inheritdoc/>
        public Boolean IsValid { get; internal set; }
    }
}
