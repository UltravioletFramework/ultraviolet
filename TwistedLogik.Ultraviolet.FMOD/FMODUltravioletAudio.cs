using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.FMOD
{
    /// <summary>
    /// Represents an implementation of the Ultraviolet audio subsystem based on the FMOD sound library.
    /// </summary>
    public sealed class FMODUltravioletAudio : UltravioletResource, IUltravioletAudio
    {
        /// <summary>
        /// Initializes a new instance of the FMODUltravioletAudio class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public FMODUltravioletAudio(UltravioletContext uv)
            : base(uv)
        {
            LibraryLoader.Load("fmod");

            FMODNative.RESULT result;

            result = FMODNative.Factory.System_Create(out system);
            FMODUtil.CheckResult(result);

            uint version;
            result = system.getVersion(out version);
            FMODUtil.CheckResult(result);

            if (version < FMODNative.VERSION.number)
                throw new InvalidOperationException(FMODStrings.FMODVersionMismatch);

            result = system.init(32, FMODNative.INITFLAGS.NORMAL, IntPtr.Zero);
            FMODUtil.CheckResult(result);

            result = system.createChannelGroup("Samples", out samplesGroup);
            FMODUtil.CheckResult(result);

            result = system.createChannelGroup("Streams", out streamsGroup);
            FMODUtil.CheckResult(result);
        }

        /// <summary>
        /// Updates the subsystem's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            system.update();

            OnUpdating(time);
        }

        /// <summary>
        /// Suspends all audio output.
        /// </summary>
        public void Suspend()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Resumes audio output after a call to <see cref="Suspend"/>.
        /// </summary>
        public void Resume()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the FMOD System object.
        /// </summary>
        public FMODNative.System System
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return system; 
            }
        }

        /// <summary>
        /// Gets the channel group used to play samples.
        /// </summary>
        public FMODNative.ChannelGroup SamplesGroup
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return samplesGroup;
            }
        }

        /// <summary>
        /// Gets the channel group used to play streams.
        /// </summary>
        public FMODNative.ChannelGroup StreamsGroup
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return streamsGroup;
            }
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

                    UpdateSamplesMuted();
                    UpdateStreamsMuted();
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

                    UpdateStreamsMuted();
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

                    UpdateSamplesMuted();
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
            if (Disposed)
                return;

            if (system != null)
            {
                system.close();
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
            var result = streamsGroup.setVolume(audioMasterVolume * songsMasterVolume);
            FMODUtil.CheckResult(result);
        }

        /// <summary>
        /// Updates the global volume of samples to match the subsystem's current settings.
        /// </summary>
        private void UpdateSampleVolume()
        {
            var result = samplesGroup.setVolume(audioMasterVolume * soundEffectsMasterVolume);
            FMODUtil.CheckResult(result);
        }

        /// <summary>
        /// Updates the mute state of the stream channel group to match the subsystem's current settings.
        /// </summary>
        private void UpdateStreamsMuted()
        {
            streamsGroup.setMute(audioMuted || songsMuted);
        }

        /// <summary>
        /// Updates the mute state of the sample channel group to match the subsystem's current settings.
        /// </summary>
        private void UpdateSamplesMuted()
        {
            samplesGroup.setMute(audioMuted || soundEffectsMuted);
        }

        // Property values.
        private Single audioMasterVolume = 1.0f;
        private Single songsMasterVolume = 1.0f;
        private Single soundEffectsMasterVolume = 1.0f;
        private Boolean audioMuted;
        private Boolean songsMuted;
        private Boolean soundEffectsMuted;

        // State values.
        private readonly FMODNative.System system;
        private readonly FMODNative.ChannelGroup samplesGroup;
        private readonly FMODNative.ChannelGroup streamsGroup;
    }
}
