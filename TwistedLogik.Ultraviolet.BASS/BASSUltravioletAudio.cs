using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Messages;
using TwistedLogik.Ultraviolet.BASS.Native;

namespace TwistedLogik.Ultraviolet.BASS
{
    /// <summary>
    /// Represents the BASS implementation of the Ultraviolet audio subsystem.
    /// </summary>
    public sealed class BASSUltravioletAudio : UltravioletResource,
        IUltravioletAudio,
        IMessageSubscriber<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes a new instance of the BASSUltravioletAudio class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public BASSUltravioletAudio(UltravioletContext uv)
            : base(uv)
        {
            var device = -1;
            var freq = 44100u;
            if (!BASSNative.Init(device, freq, 0, IntPtr.Zero, IntPtr.Zero))
                throw new BASSException();

            uv.Messages.Subscribe(this, UltravioletMessages.ApplicationSuspended);
            uv.Messages.Subscribe(this, UltravioletMessages.ApplicationResumed);
        }

        /// <inheritdoc/>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            if (type == UltravioletMessages.ApplicationSuspended)
            {
                if (!suspended)
                {
                    awaitingResume = true;
                    Suspend();
                }
                return;
            }

            if (type == UltravioletMessages.ApplicationResumed)
            {
                if (awaitingResume)
                {
                    awaitingResume = false;
                    Resume();
                }
                return;
            }
        }

        /// <summary>
        /// Updates the subsystem's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            OnUpdating(time);
        }

        /// <summary>
        /// Suspends all audio output.
        /// </summary>
        public void Suspend()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (!BASSNative.Pause())
                throw new BASSException();

            suspended = true;
        }

        /// <summary>
        /// Resumes audio output after a call to <see cref="Suspend"/>.
        /// </summary>
        public void Resume()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (!BASSNative.Start())
                throw new BASSException();

            suspended = false;
        }

        /// <summary>
        /// Gets or sets the master volume for all audio output.
        /// </summary>
        public Single AudioMasterVolume 
        { 
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return audioMasterVolume;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                var audioMasterVolumeClamped = MathUtil.Clamp(value, 0f, 1f);
                if (audioMasterVolumeClamped != audioMasterVolume)
                {
                    audioMasterVolume = audioMasterVolumeClamped;

                    UpdateSampleVolume();
                    UpdateStreamVolume();
                }
            }
        }

        /// <summary>
        /// Gets or sets the master volume for songs.
        /// </summary>
        public Single SongsMasterVolume
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return songsMasterVolume;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                var songsMasterVolumeClamped = MathUtil.Clamp(value, 0f, 1f);
                if (songsMasterVolumeClamped != songsMasterVolume)
                {
                    songsMasterVolume = songsMasterVolumeClamped;

                    UpdateStreamVolume();
                }
            }
        }

        /// <summary>
        /// Gets or sets the master volume for sound effects.
        /// </summary>
        public Single SoundEffectsMasterVolume
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return soundEffectsMasterVolume;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                var soundEffectsMasterVolumeClamped = MathUtil.Clamp(value, 0f, 1f);
                if (soundEffectsMasterVolumeClamped != soundEffectsMasterVolume)
                {
                    soundEffectsMasterVolume = soundEffectsMasterVolumeClamped;

                    UpdateSampleVolume();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether all audio output is globally muted.
        /// </summary>
        public Boolean AudioMuted 
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return audioMuted;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (audioMuted != value)
                {
                    audioMuted = value;

                    UpdateStreamVolume();
                    UpdateSampleVolume();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether songs are globally muted.
        /// </summary>
        public Boolean SongsMuted
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return songsMuted;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (songsMuted != value)
                {
                    songsMuted = value;

                    UpdateStreamVolume();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether sound effects are globally muted.
        /// </summary>
        public Boolean SoundEffectsMuted
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return soundEffectsMuted;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (soundEffectsMuted != value)
                {
                    soundEffectsMuted = value;

                    UpdateSampleVolume();
                }
            }
        }

        /// <summary>
        /// Occurs when the subsystem is updating its state.
        /// </summary>
        public event UltravioletSubsystemUpdateEventHandler Updating;

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (!BASSNative.Free())
                throw new BASSException();

            if (disposing && !Ultraviolet.Disposed)
            {
                Ultraviolet.Messages.Unsubscribe(this);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Raises the Updating event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        private void OnUpdating(UltravioletTime time)
        {
            var temp = Updating;
            if (temp != null)
            {
                temp(this, time);
            }
        }

        /// <summary>
        /// Updates the global volume of streams to match the subsystem's current settings.
        /// </summary>
        private void UpdateStreamVolume()
        {
            var volumeStream = (audioMuted || songsMuted) ? 0 : (uint)(10000 * audioMasterVolume * songsMasterVolume);
            if (!BASSNative.SetConfig(BASSConfig.CONFIG_GVOL_STREAM, volumeStream))
                throw new BASSException();
        }

        /// <summary>
        /// Updates the global volume of samples to match the subsystem's current settings.
        /// </summary>
        private void UpdateSampleVolume()
        {
            var volumeSample = (audioMuted || soundEffectsMuted) ? 0 : (uint)(10000 * audioMasterVolume * soundEffectsMasterVolume);
            if (!BASSNative.SetConfig(BASSConfig.CONFIG_GVOL_SAMPLE, volumeSample))
                throw new BASSException();
        }

        // Property values.
        private Single audioMasterVolume = 1.0f;
        private Single songsMasterVolume = 1.0f;
        private Single soundEffectsMasterVolume = 1.0f;
        private Boolean audioMuted;
        private Boolean songsMuted;
        private Boolean soundEffectsMuted;

        // State values.
        private Boolean suspended;
        private Boolean awaitingResume;
    }
}
