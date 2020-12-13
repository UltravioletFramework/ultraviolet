using System;
using System.Runtime.InteropServices;
using Ultraviolet.Audio;
using Ultraviolet.BASS.Messages;
using Ultraviolet.BASS.Native;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;
using static Ultraviolet.BASS.Native.BASSNative;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Represents the BASS implementation of the <see cref="SongPlayer"/> class.
    /// </summary>
    public sealed class BASSSongPlayer : SongPlayer,
        IMessageSubscriber<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BASSSongPlayer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public BASSSongPlayer(UltravioletContext uv)
            : base(uv)
        {
            gcHandle = GCHandle.Alloc(this, GCHandleType.Weak);

            uv.Messages.Subscribe(this, BASSUltravioletMessages.BASSDeviceChanged);
        }

        /// <inheritdoc/>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            if (type == BASSUltravioletMessages.BASSDeviceChanged)
            {
                if (BASSUtil.IsValidHandle(stream))
                {
                    var deviceID = ((BASSDeviceChangedMessageData)data).DeviceID;
                    if (!BASS_ChannelSetDevice(stream, deviceID))
                        throw new BASSException();
                }
                return;
            }
        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {

        }

        /// <inheritdoc/>
        public override Boolean Play(Song song, Boolean loop = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(song, nameof(song));

            return PlayInternal(song, 1f, 0f, 0f,
                loop ? TimeSpan.Zero : (TimeSpan?)null, null);
        }

        /// <inheritdoc/>
        public override Boolean Play(Song song, TimeSpan loopStart, TimeSpan? loopLength)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(song, nameof(song));

            return PlayInternal(song, 1f, 0f, 0f, loopStart, loopLength ?? Duration - loopStart);
        }

        /// <inheritdoc/>
        public override Boolean Play(Song song, Single volume, Single pitch, Single pan, Boolean loop = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(song, nameof(song));

            return PlayInternal(song, volume, pitch, pan,
                loop ? TimeSpan.Zero : (TimeSpan?)null, null);
        }

        /// <inheritdoc/>
        public override Boolean Play(Song song, Single volume, Single pitch, Single pan, TimeSpan loopStart, TimeSpan? loopLength)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(song, nameof(song));

            return PlayInternal(song, volume, pitch, pan, loopStart, loopLength ?? Duration - loopStart);
        }

        /// <inheritdoc/>
        public override void Stop()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (StopInternal())
            {
                OnStateChanged();
                OnSongEnded();
            }
        }

        /// <inheritdoc/>
        public override void Pause()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (State == PlaybackState.Playing)
            {
                if (!BASS_ChannelPause(stream))
                    throw new BASSException();

                OnStateChanged();
            }
        }

        /// <inheritdoc/>
        public override void Resume()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (State == PlaybackState.Paused)
            {
                if (!BASS_ChannelPlay(stream, false))
                    throw new BASSException();

                OnStateChanged();
            }
        }

        /// <inheritdoc/>
        public override void SlideVolume(Single volume, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            EnsureChannelIsValid();

            BASSUtil.SlideVolume(stream, volume, time);
        }

        /// <inheritdoc/>
        public override void SlidePitch(Single pitch, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            EnsureChannelIsValid();
        }

        /// <inheritdoc/>
        public override void SlidePan(Single pan, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            EnsureChannelIsValid();

            BASSUtil.SlidePan(stream, pan, time);
        }

        /// <inheritdoc/>
        public override PlaybackState State
        {
            get
            {
                if (BASSUtil.IsValidHandle(stream))
                {
                    switch (BASS_ChannelIsActive(stream))
                    {
                        case BASS_ACTIVE_PLAYING:
                        case BASS_ACTIVE_STALLED:
                            return PlaybackState.Playing;

                        case BASS_ACTIVE_STOPPED:
                            return PlaybackState.Stopped;

                        case BASS_ACTIVE_PAUSED:
                            return PlaybackState.Paused;
                    }
                }
                return PlaybackState.Stopped;
            }
        }

        /// <inheritdoc/>
        public override Boolean IsPlaying
        {
            get => State == PlaybackState.Playing;
        }

        /// <inheritdoc/>
        public override Boolean IsLooping
        {
            get => IsChannelValid() ? (syncLoop != 0 || BASSUtil.GetIsLooping(stream)) : false;
            set
            {
                EnsureChannelIsValid();

                if (syncLoop != 0)
                {
                    if (!BASS_ChannelRemoveSync(stream, syncLoop))
                        throw new BASSException();

                    syncLoop = 0;
                    syncLoopDelegate = null;
                }
                else
                {
                    BASSUtil.SetIsLooping(stream, value);
                }
            }
        }

        /// <inheritdoc/>
        public override TimeSpan Position
        {
            get => IsChannelValid() ? BASSUtil.GetPositionAsTimeSpan(stream) : TimeSpan.Zero;
            set 
            {
                EnsureChannelIsValid();

                if (value.TotalSeconds < 0 || value > Duration)
                    throw new ArgumentOutOfRangeException(nameof(value));

                BASSUtil.SetPositionInSeconds(stream, value.TotalSeconds);
            }
        }

        /// <inheritdoc/>
        public override TimeSpan Duration
        {
            get => IsChannelValid() ? BASSUtil.GetDurationAsTimeSpan(stream) : TimeSpan.Zero;
        }

        /// <inheritdoc/>
        public override Single Volume
        {
            get => IsChannelValid() ? BASSUtil.GetVolume(stream) : 1f;
            set 
            {
                EnsureChannelIsValid();
                BASSUtil.SetVolume(stream, MathUtil.Clamp(value, -1f, 1f));
            }
        }

        /// <inheritdoc/>
        public override Single Pitch
        {
            get => 0f;
            set
            {
                EnsureChannelIsValid();
            }
        }

        /// <inheritdoc/>
        public override Single Pan
        {
            get => IsChannelValid() ? BASSUtil.GetPan(stream) : 0f;
            set
            {
                EnsureChannelIsValid();
                BASSUtil.SetPan(stream, MathUtil.Clamp(value, -1f, 1f));
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Ultraviolet != null && !Ultraviolet.Disposed)
            {
                StopInternal();
                Ultraviolet.Messages.Unsubscribe(this);
            }

            stream = 0;

            if (gcHandle.IsAllocated)
                gcHandle.Free();
            
            base.Dispose(disposing);
        }

        /// <summary>
        /// Performs custom looping when a loop range is specified.
        /// </summary>
        [MonoPInvokeCallback(typeof(SyncProc))]
        private static void SyncLoopThunk(UInt32 handle, UInt32 channel, UInt32 data, IntPtr user)
        {
            if (!BASS_ChannelSetPosition(channel, (UInt32)user, 0))
                throw new BASSException();
        }

        /// <summary>
        /// Raises a callback when a song ends.
        /// </summary>
        [MonoPInvokeCallback(typeof(SyncProc))]
        private static void SyncEndThunk(UInt32 handle, UInt32 channel, UInt32 data, IntPtr user)
        {
            var gcHandle = GCHandle.FromIntPtr(user);
            ((BASSSongPlayer)gcHandle.Target)?.SyncEnd(handle, channel, data, IntPtr.Zero);
        }

        /// <summary>
        /// Plays the specified song.
        /// </summary>
        private Boolean PlayInternal(Song song, Single volume, Single pitch, Single pan, TimeSpan? loopStart, TimeSpan? loopLength)
        {
            Ultraviolet.ValidateResource(song);

            Stop();

            stream = ((BASSSong)song).CreateStream(0);
            if (!BASSUtil.IsValidHandle(stream))
                throw new BASSException();

            var autoloop = loopStart.HasValue && !loopLength.HasValue;
            var syncloop = loopStart.HasValue && !autoloop;

            BASSUtil.SetIsLooping(stream, autoloop);
            BASSUtil.SetVolume(stream, MathUtil.Clamp(volume, 0f, 1f));
            BASSUtil.SetPan(stream, MathUtil.Clamp(pan, -1f, 1f));

            if (loopStart > TimeSpan.Zero && loopLength <= TimeSpan.Zero)
                throw new ArgumentException(nameof(loopLength));

            if (syncloop)
            {
                var loopStartInBytes = BASS_ChannelSeconds2Bytes(stream, loopStart.Value.TotalSeconds);
                var loopEndInBytes = BASS_ChannelSeconds2Bytes(stream, (loopStart + loopLength).Value.TotalSeconds);
                syncLoopDelegate = SyncLoopThunk;
                syncLoop = BASS_ChannelSetSync(stream, BASS_SYNC_POS, loopEndInBytes, syncLoopDelegate, new IntPtr((Int32)loopStartInBytes));
                if (syncLoop == 0)
                    throw new BASSException();
            }

            syncEndDelegate = SyncEndThunk;
            syncEnd = BASS_ChannelSetSync(stream, BASS_SYNC_END, 0, syncEndDelegate, GCHandle.ToIntPtr(gcHandle));
            if (syncEnd == 0)
                throw new BASSException();

            if (!BASS_ChannelPlay(stream, true))
                throw new BASSException();

            OnStateChanged();
            OnSongStarted();

            return true;
        }

        /// <summary>
        /// Releases the memory associated with the underlying stream object.
        /// </summary>
        private Boolean StopInternal()
        {
            if (stream == 0)
                return false;

            if (!BASS_StreamFree(stream))
                throw new BASSException();

            stream = 0;

            syncLoopDelegate = null;
            syncLoop = 0;

            syncEndDelegate = null;
            syncEnd = 0;

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the channel is in a valid state.
        /// </summary>
        /// <returns>true if the channel is in a valid state; otherwise, false.</returns>
        private Boolean IsChannelValid()
        {
            return State != PlaybackState.Stopped;
        }

        /// <summary>
        /// Throws an <see cref="System.InvalidOperationException"/> if the channel is not in a valid state.
        /// </summary>
        private void EnsureChannelIsValid()
        {
            if (State == PlaybackState.Stopped)
                throw new InvalidOperationException(BASSStrings.NotCurrentlyValid);
        }

        /// <summary>
        /// Raises a callback when a song ends.
        /// </summary>
        private void SyncEnd(UInt32 handle, UInt32 channel, UInt32 data, IntPtr user)
        {
            if (!IsLooping)
            {
                if (StopInternal())
                {
                    OnStateChanged();
                    OnSongEnded();
                }
            }
        }

        // State values.
        private GCHandle gcHandle;
        private UInt32 stream;
        private UInt32 syncLoop;
        private UInt32 syncEnd;
        private SyncProc syncLoopDelegate;
        private SyncProc syncEndDelegate;
    }
}
