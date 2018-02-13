using System;
using System.Text;
using System.Collections.Generic;
using Ultraviolet.Audio;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;
using Ultraviolet.FMOD.Audio;
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

            UpdateAudioDevices();
            PlaybackDevice = GetDefaultDevice();
            
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

        /// <inheritdoc/>
        public IEnumerable<IUltravioletAudioDevice> EnumerateAudioDevices()
        {
            return new IUltravioletAudioDevice[0];
        }

        /// <inheritdoc/>
        public IUltravioletAudioDevice FindAudioDeviceByName(String name)
        {
            Contract.Require(name, nameof(name));
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var device in knownAudioDevices)
            {
                if (device.IsValid && String.Equals(name, device.Name, StringComparison.Ordinal))
                    return device;
            }

            return null;
        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var result = default(FMOD_RESULT);

            result = FMOD_System_Update(system);
            if (result != FMOD_OK)
                throw new FMODException(result);

            UpdateAudioDevices();

            Updating?.Invoke(this, time);
        }

        /// <inheritdoc/>
        public void Suspend()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var result = default(FMOD_RESULT);

            result = FMOD_System_MixerSuspend(system);
            if (result != FMOD_OK)
                throw new FMODException(result);

            suspended = true;
        }

        /// <inheritdoc/>
        public void Resume()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var result = default(FMOD_RESULT);

            result = FMOD_System_MixerResume(system);
            if (result != FMOD_OK)
                throw new FMODException(result);

            suspended = false;
        }

        /// <inheritdoc/>
        public IUltravioletAudioDevice PlaybackDevice
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return playbackDevice;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                var val = value ?? GetDefaultDevice();
                if (val == null)
                {
                    playbackDevice = null;
                }
                else
                {
                    if (value is FMODUltravioletAudioDevice device)
                    {
                        Ultraviolet.ValidateResource(device);

                        var result = default(FMOD_RESULT);
                        var olddriver = 0;

                        result = FMOD_System_GetDriver(system, &olddriver);
                        if (result != FMOD_OK)
                            throw new FMODException(result);

                        result = FMOD_System_SetDriver(system, device.ID);
                        if (result == FMOD_ERR_OUTPUT_INIT)
                        {
                            result = FMOD_System_SetDriver(system, olddriver);
                            if (result != FMOD_OK)
                                throw new FMODException(result);

                            return;
                        }

                        if (result != FMOD_OK)
                            throw new FMODException(result);

                        playbackDevice = device;
                    }
                    else throw new ArgumentException(nameof(value));
                }
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <summary>
        /// Updates the list of audio devices.
        /// </summary>
        private void UpdateAudioDevices()
        {
            var result = default(FMOD_RESULT);

            var numdrivers = 0;
            result = FMOD_System_GetNumDrivers(system, &numdrivers);
            if (result != FMOD_OK)
                throw new FMODException(result);
            
            if (numdrivers != knownAudioDevices.Count)
            {
                foreach (var device in knownAudioDevices)
                    device.IsValid = false;

                knownAudioDevices.Clear();

                var namebuf = new StringBuilder(256);
                var namelen = namebuf.Capacity;

                for (int i = 0; i < numdrivers; i++)
                {
                    result = FMOD_System_GetDriverInfo(system, i, namebuf, namelen, null, null, null, null);
                    if (result != FMOD_OK)
                        throw new FMODException(result);

                    var device = new FMODUltravioletAudioDevice(Ultraviolet, i, namebuf.ToString());
                    device.IsValid = true;
                    device.IsDefault = (i == 0);

                    knownAudioDevices.Add(device);
                }
            }

            if (playbackDevice != null && !playbackDevice.IsValid)
                this.PlaybackDevice = FindAudioDeviceByName(PlaybackDevice.Name) ?? GetDefaultDevice();
        }

        /// <summary>
        /// Gets the default audio device.
        /// </summary>
        private FMODUltravioletAudioDevice GetDefaultDevice()
        {
            return knownAudioDevices.Count == 0 ? null : knownAudioDevices[0];
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

        // Audio device cache.
        private FMODUltravioletAudioDevice playbackDevice;
        private List<FMODUltravioletAudioDevice> knownAudioDevices =
            new List<FMODUltravioletAudioDevice>();
    }
}
