using System;
using Ultraviolet.BASS.Native;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;
using System.Collections.Generic;
using Ultraviolet.Audio;
using Ultraviolet.BASS.Audio;

namespace Ultraviolet.BASS
{
    /// <summary>
    /// Represents the BASS implementation of the Ultraviolet audio subsystem.
    /// </summary>
    public sealed unsafe class BASSUltravioletAudio : UltravioletResource,
        IUltravioletAudio,
        IMessageSubscriber<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes a new instance of the BASSUltravioletAudio class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        [Preserve]
        public BASSUltravioletAudio(UltravioletContext uv)
            : base(uv)
        {
            if (uv.Platform == UltravioletPlatform.Windows || uv.Platform == UltravioletPlatform.macOS)
            {
                if (!BASSNative.SetConfig(BASSConfig.CONFIG_DEV_DEFAULT, 1))
                    throw new BASSException();
            }

            var device = -1;
            var freq = 44100u;
            if (!BASSNative.Init(device, freq, 0, IntPtr.Zero, IntPtr.Zero))
                throw new BASSException();

            UpdateAudioDevices();

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
            Contract.EnsureNotDisposed(this, Disposed);

            return knownAudioDevices;
        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            UpdateAudioDevices();

            OnUpdating(time);
        }

        /// <inheritdoc/>
        public void Suspend()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (!BASSNative.Pause())
                throw new BASSException();

            suspended = true;
        }

        /// <inheritdoc/>
        public void Resume()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (!BASSNative.Start())
                throw new BASSException();

            suspended = false;
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

                    UpdateSampleVolume();
                    UpdateStreamVolume();
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

                    UpdateStreamVolume();
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

                    UpdateSampleVolume();
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

                    UpdateStreamVolume();
                    UpdateSampleVolume();
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

                    UpdateStreamVolume();
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

                    UpdateSampleVolume();
                }
            }
        }

        /// <inheritdoc/>
        public event UltravioletSubsystemUpdateEventHandler Updating;

        /// <inheritdoc/>
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
        private void OnUpdating(UltravioletTime time) =>
            Updating?.Invoke(this, time);

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

        /// <summary>
        /// Updates the list of audio devices.
        /// </summary>
        private void UpdateAudioDevices()
        {
            BASS_DEVICEINFO info;
            for (int i = 1; BASSNative.GetDeviceInfo((uint)i, &info); i++)
            {
                var isEnabled = (info.flags & BASSNative.BASS_DEVICE_ENABLED) == BASSNative.BASS_DEVICE_ENABLED;
                var isDefault = (info.flags & BASSNative.BASS_DEVICE_DEFAULT) == BASSNative.BASS_DEVICE_DEFAULT;

                var ix = (i - 1);
                if (ix >= knownAudioDevices.Count)
                {
                    var marshalledInfo = info.ToMarshalledStruct();
                    knownAudioDevices.Add(new BASSUltravioletAudioDevice(marshalledInfo.name));
                }

                var device = knownAudioDevices[ix];
                device.IsValid = isEnabled;
                device.IsDefault = isDefault;
            }
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

        // Audio device cache.
        private List<BASSUltravioletAudioDevice> knownAudioDevices = 
            new List<BASSUltravioletAudioDevice>();
    }
}
