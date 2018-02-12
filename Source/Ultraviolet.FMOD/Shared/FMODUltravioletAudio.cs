using System;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;
using Ultraviolet.FMOD.Native;
using static Ultraviolet.FMOD.Native.FMOD_INITFLAGS;
using static Ultraviolet.FMOD.Native.FMOD_RESULT;
using static Ultraviolet.FMOD.Native.FMODNative;

namespace Ultraviolet.FMOD
{
    /// <summary>
    /// Represents the FMOD implementation of the Ultraviolet audio subsystem.
    /// </summary>
    public sealed unsafe class FMODUltravioletAudio : UltravioletResource,
        IUltravioletAudio,
        IMessageSubscriber<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes a new instance of the FMODUltravioletAudio class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        [Preserve]
        public FMODUltravioletAudio(UltravioletContext uv)
            : base(uv)
        {
            var result = default(FMOD_RESULT);

            fixed (FMOD_SYSTEM** psystem = &system)
            {
                result = FMOD_System_Create(psystem);
                if (result != FMOD_OK)
                    throw new FMODException(result);
            }

            var version = 0u;
            result = FMOD_System_GetVersion(system, &version);
            if (result != FMOD_OK)
                throw new FMODException(result);

            if (version < FMOD_VERSION)
                throw new Exception(FMODStrings.FMODVersionMismatch.Format(FMOD_VERSION, version));

            var extradriverdata = default(void*);
            result = FMOD_System_Init(system, 256, FMOD_INIT_NORMAL, extradriverdata);
            if (result != FMOD_OK)
                throw new FMODException(result);

            fixed (FMOD_CHANNELGROUP** pcgroupSongs = &cgroupSongs)
            {
                result = FMOD_System_CreateChannelGroup(system, "Songs", pcgroupSongs);
                if (result != FMOD_OK)
                    throw new FMODException(result);
            }

            fixed (FMOD_CHANNELGROUP** pcgroupSoundEffects = &cgroupSoundEffects)
            {
                result = FMOD_System_CreateChannelGroup(system, "Sound Effects", pcgroupSoundEffects);
                if (result != FMOD_OK)
                    throw new FMODException(result);
            }
            
            uv.Messages.Subscribe(this, UltravioletMessages.ApplicationSuspending);
            uv.Messages.Subscribe(this, UltravioletMessages.ApplicationResumed);
        }

        /// <inheritdoc/>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            if (type == UltravioletMessages.ApplicationSuspending)
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

            var result = default(FMOD_RESULT);

            result = FMOD_System_Update(system);
            if (result != FMOD_OK)
                throw new FMODException(result);

            Updating?.Invoke(this, time);
        }

        /// <summary>
        /// Suspends all audio output.
        /// </summary>
        public void Suspend()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var result = default(FMOD_RESULT);

            result = FMOD_System_MixerSuspend(system);
            if (result != FMOD_OK)
                throw new FMODException(result);

            suspended = true;
        }

        /// <summary>
        /// Resumes audio output after a call to <see cref="Suspend"/>.
        /// </summary>
        public void Resume()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var result = default(FMOD_RESULT);

            result = FMOD_System_MixerResume(system);
            if (result != FMOD_OK)
                throw new FMODException(result);

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

                    UpdateSoundVolume();
                    UpdateSongsVolume();
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

                    UpdateSongsVolume();
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

                    UpdateSoundVolume();
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

                    UpdateSongsVolume();
                    UpdateSoundVolume();
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

                    UpdateSongsVolume();
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

                    UpdateSoundVolume();
                }
            }
        }

        /// <summary>
        /// Occurs when the subsystem is updating its state.
        /// </summary>
        public event UltravioletSubsystemUpdateEventHandler Updating;

        /// <summary>
        /// Gets a pointer to the FMOD system object.
        /// </summary>
        internal FMOD_SYSTEM* System => system;

        /// <summary>
        /// Gets a pointer to the FMOD channel group for songs.
        /// </summary>
        internal FMOD_CHANNELGROUP* ChannelGroupSongs => cgroupSongs;

        /// <summary>
        /// Gets a pointer to the FMOD channel group for sound effects.
        /// </summary>
        internal FMOD_CHANNELGROUP* ChannelGroupSoundEffects => cgroupSoundEffects;

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            var result = default(FMOD_RESULT);

            result = FMOD_ChannelGroup_Release(cgroupSongs);
            if (result != FMOD_OK)
                throw new FMODException(result);

            result = FMOD_ChannelGroup_Release(cgroupSoundEffects);
            if (result != FMOD_OK)
                throw new FMODException(result);

            result = FMOD_System_Release(system);
            if (result != FMOD_OK)
                throw new FMODException(result);

            if (disposing && !Ultraviolet.Disposed)
            {
                Ultraviolet.Messages.Unsubscribe(this);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Updates the global volume of the Songs channel group to match the subsystem's current settings.
        /// </summary>
        private void UpdateSongsVolume()
        {
            var volumeSongs = (audioMuted || songsMuted) ? 0f : audioMasterVolume * songsMasterVolume;

            var result = FMOD_ChannelGroup_SetVolume(cgroupSongs, volumeSongs);
            if (result != FMOD_OK)
                throw new FMODException(result);
        }

        /// <summary>
        /// Updates the global volume of the Sound Effects channel group to match the subsystem's current settings.
        /// </summary>
        private void UpdateSoundVolume()
        {
            var volumeSoundEffects = (audioMuted || soundEffectsMuted) ? 0f : audioMasterVolume * soundEffectsMasterVolume;

            var result = FMOD_ChannelGroup_SetVolume(cgroupSoundEffects, volumeSoundEffects);
            if (result != FMOD_OK)
                throw new FMODException(result);
        }

        // FMOD state variables.
        private readonly FMOD_SYSTEM* system;
        private readonly FMOD_CHANNELGROUP* cgroupSongs;
        private readonly FMOD_CHANNELGROUP* cgroupSoundEffects;

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
