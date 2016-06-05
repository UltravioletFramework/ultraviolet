using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Audio;
using TwistedLogik.Ultraviolet.BASS.Native;

namespace TwistedLogik.Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Represents the BASS implementation of the SongPlayer class.
    /// </summary>
    public sealed class BASSSongPlayer : SongPlayer
    {
        /// <summary>
        /// Initializes a new instance of the BASSSongPlayer class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public BASSSongPlayer(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);
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
        public override Boolean Play(Song song, TimeSpan loopStart, TimeSpan loopLength)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(song, nameof(song));

            return PlayInternal(song, 1f, 0f, 0f, loopStart, loopLength);
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
        public override Boolean Play(Song song, Single volume, Single pitch, Single pan, TimeSpan loopStart, TimeSpan loopLength)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(song, nameof(song));

            return PlayInternal(song, volume, pitch, pan, loopStart, loopLength);
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
                if (!BASSNative.ChannelPause(stream))
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
                if (!BASSNative.ChannelPlay(stream, false))
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

            BASSUtil.SlidePitch(stream, pitch, time);
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
                Contract.EnsureNotDisposed(this, Disposed);

                if (BASSUtil.IsValidHandle(stream))
                {
                    switch (BASSNative.ChannelIsActive(stream))
                    {
                        case BASSNative.BASS_ACTIVE_PLAYING:
                        case BASSNative.BASS_ACTIVE_STALLED:
                            return PlaybackState.Playing;

                        case BASSNative.BASS_ACTIVE_STOPPED:
                            return PlaybackState.Stopped;

                        case BASSNative.BASS_ACTIVE_PAUSED:
                            return PlaybackState.Stopped;
                    }
                }
                return PlaybackState.Stopped;
            }
        }

        /// <inheritdoc/>
        public override Boolean IsPlaying
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return State == PlaybackState.Playing; 
            }
        }

        /// <inheritdoc/>
        public override Boolean IsLooping
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return false;

                return syncLoop != 0 || BASSUtil.GetIsLooping(stream);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                if (syncLoop != 0)
                {
                    if (!BASSNative.ChannelRemoveSync(stream, syncLoop))
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
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return TimeSpan.Zero;

                var position = BASSUtil.GetPositionInSeconds(stream);
                return TimeSpan.FromSeconds(position);
            }
            set 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                if (value.TotalSeconds < 0 || value > Duration)
                    throw new ArgumentOutOfRangeException("value");

                BASSUtil.SetPositionInSeconds(stream, value.TotalSeconds);
            }
        }

        /// <inheritdoc/>
        public override TimeSpan Duration
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return TimeSpan.Zero;

                var duration = BASSUtil.GetDurationInSeconds(stream);
                return TimeSpan.FromSeconds(duration);
            }
        }

        /// <inheritdoc/>
        public override Single Volume
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return 1f;

                return BASSUtil.GetVolume(stream);
            }
            set 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                BASSUtil.SetVolume(stream, MathUtil.Clamp(value, -1f, 1f));
            }
        }

        /// <inheritdoc/>
        public override Single Pitch
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return 0f;

                return BASSUtil.GetPitch(stream);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                BASSUtil.SetPitch(stream, MathUtil.Clamp(value, -1f, 1f));
            }
        }

        /// <inheritdoc/>
        public override Single Pan
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return 0f;

                return BASSUtil.GetPan(stream);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                BASSUtil.SetPan(stream, MathUtil.Clamp(value, -1f, 1f));
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
                StopInternal();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Plays the specified song.
        /// </summary>
        private Boolean PlayInternal(Song song, Single volume, Single pitch, Single pan, TimeSpan? loopStart, TimeSpan? loopLength)
        {
            Ultraviolet.ValidateResource(song);

            Stop();

            stream = ((BASSSong)song).CreateStream(BASSNative.BASS_STREAM_DECODE);
            stream = BASSFXNative.TempoCreate(stream, BASSNative.BASS_FX_FREESOURCE | BASSNative.BASS_STREAM_AUTOFREE);
            if (!BASSUtil.IsValidHandle(stream))
                throw new BASSException();

            var autoloop = loopStart.HasValue && !loopLength.HasValue;
            var syncloop = loopStart.HasValue && !autoloop;

            BASSUtil.SetIsLooping(stream, autoloop);
            BASSUtil.SetVolume(stream, MathUtil.Clamp(volume, 0f, 1f));
            BASSUtil.SetPitch(stream, MathUtil.Clamp(pitch, -1f, 1f));
            BASSUtil.SetPan(stream, MathUtil.Clamp(pan, -1f, 1f));

            if (loopStart > TimeSpan.Zero && loopLength <= TimeSpan.Zero)
                throw new ArgumentException(nameof(loopLength));

            if (syncloop)
            {
                var loopStartInBytes = BASSNative.ChannelSeconds2Bytes(stream, loopStart.Value.TotalSeconds);
                var loopEndInBytes = BASSNative.ChannelSeconds2Bytes(stream, (loopStart + loopLength).Value.TotalSeconds);
                syncLoopDelegate = SyncLoop;
                syncLoop = BASSNative.ChannelSetSync(stream, BASSSync.SYNC_POS, loopEndInBytes, syncLoopDelegate, new IntPtr((Int32)loopStartInBytes));
                if (syncLoop == 0)
                    throw new BASSException();
            }

            syncEndDelegate = SyncEnd;
            syncEnd = BASSNative.ChannelSetSync(stream, BASSSync.SYNC_END, 0, syncEndDelegate, IntPtr.Zero);
            if (syncEnd == 0)
                throw new BASSException();

            if (!BASSNative.ChannelPlay(stream, true))
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

            if (!BASSNative.StreamFree(stream))
                throw new BASSException();

            stream = 0;

            syncLoopDelegate = null;
            syncLoop = 0;

            syncEndDelegate = null;
            syncEnd = 0;

            return true;
        }

        /// <summary>
        /// Throws an <see cref="System.InvalidOperationException"/> if the channel is not in a valid state.
        /// </summary>
        private void EnsureChannelIsValid()
        {
            if (State == PlaybackState.Stopped)
            {
                throw new InvalidOperationException(BASSStrings.NotCurrentlyValid);
            }
        }

        /// <summary>
        /// Performs custom looping when a loop range is specified.
        /// </summary>
        private void SyncLoop(UInt32 handle, UInt32 channel, UInt32 data, IntPtr user)
        {
            if (!BASSNative.ChannelSetPosition(channel, (UInt32)user, 0))
                throw new BASSException();
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

        /// <summary>
        /// Gets a value indicating whether the channel is in a valid state.
        /// </summary>
        /// <returns>true if the channel is in a valid state; otherwise, false.</returns>
        private Boolean IsChannelValid()
        {
            return State != PlaybackState.Stopped;
        }

        // State values.
        private UInt32 stream;
        private UInt32 syncLoop;
        private UInt32 syncEnd;
        private SyncProc syncLoopDelegate;
        private SyncProc syncEndDelegate;
    }
}
