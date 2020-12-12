using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Audio;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a dummy implementation of <see cref="IUltravioletAudio"/>.
    /// </summary>
    public sealed class DummyUltravioletAudio : UltravioletResource, IUltravioletAudio
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DummyUltravioletAudio"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public DummyUltravioletAudio(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <inheritdoc/>
        public IEnumerable<IUltravioletAudioDevice> EnumerateAudioDevices()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return Enumerable.Empty<IUltravioletAudioDevice>();
        }

        /// <inheritdoc/>
        public IUltravioletAudioDevice FindAudioDeviceByName(String name)
        {
            return null;
        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            Updating?.Invoke(this, time);
        }

        /// <inheritdoc/>
        public void Suspend()
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void Resume()
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public IUltravioletAudioDevice PlaybackDevice
        {
            get { return null; }
            set { }
        }

        /// <inheritdoc/>
        public AudioCapabilities Capabilities { get; } = new DummyAudioCapabilities();

        /// <inheritdoc/>
        public Single AudioMasterVolume
        {
            get { return 1f; }
            set { }
        }

        /// <inheritdoc/>
        public Single SongsMasterVolume
        {
            get { return 1f; }
            set { }
        }

        /// <inheritdoc/>
        public Single SoundEffectsMasterVolume
        {
            get { return 1f; }
            set { }
        }

        /// <inheritdoc/>
        public Boolean AudioMuted
        {
            get { return false; }
            set { }
        }

        /// <inheritdoc/>
        public Boolean SongsMuted
        {
            get { return false; }
            set { }
        }

        /// <inheritdoc/>
        public Boolean SoundEffectsMuted
        {
            get { return false; }
            set { }
        }

        /// <inheritdoc/>
        public event UltravioletSubsystemUpdateEventHandler Updating;
    }
}
