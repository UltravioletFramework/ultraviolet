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
            return Enumerable.Empty<IUltravioletAudioDevice>();
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
        public Single AudioMasterVolume
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return 1f;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
            }
        }

        /// <inheritdoc/>
        public Single SongsMasterVolume
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return 1f;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
            }
        }

        /// <inheritdoc/>
        public Single SoundEffectsMasterVolume
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return 1f;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
            }
        }

        /// <inheritdoc/>
        public Boolean AudioMuted
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return false;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
            }
        }

        /// <inheritdoc/>
        public Boolean SongsMuted
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return false;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
            }
        }

        /// <inheritdoc/>
        public Boolean SoundEffectsMuted
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return false;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
            }
        }

        /// <inheritdoc/>
        public event UltravioletSubsystemUpdateEventHandler Updating;
    }
}
