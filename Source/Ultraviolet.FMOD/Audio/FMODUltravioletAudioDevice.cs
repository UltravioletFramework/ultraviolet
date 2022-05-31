using System;
using Ultraviolet.Audio;
using Ultraviolet.Core;

namespace Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Represents the FMOD implementation of the <see cref="IUltravioletAudioDevice"/> interface.
    /// </summary>
    public sealed class FMODUltravioletAudioDevice : UltravioletResource, IUltravioletAudioDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FMODUltravioletAudioDevice"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The device's FMOD identifier.</param>
        /// <param name="name">The device's name.</param>
        public FMODUltravioletAudioDevice(UltravioletContext uv, Int32 id, String name)
            : base(uv)
        {
            Contract.Require(name, nameof(name));

            this.ID = id;
            this.Name = name;
        }

        /// <summary>
        /// Gets the device's FMOD identifier.
        /// </summary>
        public Int32 ID { get; }

        /// <inheritdoc/>
        public String Name { get; }

        /// <inheritdoc/>
        public Boolean IsDefault { get; internal set; }

        /// <inheritdoc/>
        public Boolean IsValid { get; internal set; }
    }
}
