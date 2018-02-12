using System;
using static Ultraviolet.FMOD.Native.FMOD_TIMEUNIT;
using Ultraviolet.Audio;
using Ultraviolet.Core;
using Ultraviolet.FMOD.Native;
using static Ultraviolet.FMOD.Native.FMOD_MODE;
using static Ultraviolet.FMOD.Native.FMOD_RESULT;
using static Ultraviolet.FMOD.Native.FMODNative;

namespace Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Represents the FMOD implementation of the <see cref="SongPlayer"/> class.
    /// </summary>
    public sealed unsafe class FMODSongPlayer : SongPlayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FMODSongPlayer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public FMODSongPlayer(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (State != PlaybackState.Stopped)
            {
                UpdateSlidingVolume(time);
                UpdateSlidingPitch(time);
                UpdateSlidingPan(time);
            }
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
                var result = FMOD_Channel_SetPaused(channel, true);
                if (result != FMOD_OK)
                    throw new FMODException(result);

                OnStateChanged();
            }
        }

        /// <inheritdoc/>
        public override void Resume()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (State == PlaybackState.Paused)
            {
                var result = FMOD_Channel_SetPaused(channel, false);
                if (result != FMOD_OK)
                    throw new FMODException(result);

                OnStateChanged();
            }
        }

        /// <inheritdoc/>
        public override void SlideVolume(Single volume, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (State == PlaybackState.Stopped)
                throw new InvalidOperationException("TODO");

            this.isSlidingVolume = true;
            this.slideStartVolume = this.Volume;
            this.slideEndVolume = volume;
            this.slideClockVolume = 0.0;
            this.slideDurationVolume = time.TotalMilliseconds;
        }

        /// <inheritdoc/>
        public override void SlidePitch(Single pitch, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (State == PlaybackState.Stopped)
                throw new InvalidOperationException("TODO");

            this.isSlidingPitch = true;
            this.slideStartPitch = this.Pitch;
            this.slideEndPitch = pitch;
            this.slideClockPitch = 0.0;
            this.slideDurationPitch = time.TotalMilliseconds;
        }

        /// <inheritdoc/>
        public override void SlidePan(Single pan, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (State == PlaybackState.Stopped)
                throw new InvalidOperationException("TODO");

            this.isSlidingPan = true;
            this.slideStartPan = this.Pan;
            this.slideEndPan = pan;
            this.slideClockPan = 0.0;
            this.slideDurationPan = time.TotalMilliseconds;
        }

        /// <inheritdoc/>
        public override PlaybackState State
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (channel != null)
                {
                    var result = default(FMOD_RESULT);
                    var isplaying = false;
                    var ispaused = false;

                    result = FMOD_Channel_GetPaused(channel, &ispaused);
                    if (result != FMOD_OK)
                        throw new FMODException(result);

                    if (ispaused)
                        return PlaybackState.Paused;

                    result = FMOD_Channel_IsPlaying(channel, &isplaying);
                    if (result != FMOD_OK)
                        throw new FMODException(result);

                    return isplaying ? PlaybackState.Playing : PlaybackState.Stopped;
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

                if (channel == null)
                    return false;

                var mode = default(FMOD_MODE);
                var result = FMOD_Channel_GetMode(channel, &mode);
                if (result != FMOD_OK)
                    throw new FMODException(result);

                return 
                    (mode & FMOD_LOOP_NORMAL) != 0 ||
                    (mode & FMOD_LOOP_BIDI) != 0;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (State == PlaybackState.Stopped)
                    throw new InvalidOperationException("TODO");

                var result = FMOD_Channel_SetMode(channel, value ? FMOD_LOOP_NORMAL : FMOD_LOOP_OFF);
                if (result != FMOD_OK)
                    throw new FMODException(result);
            }
        }

        /// <inheritdoc/>
        public override TimeSpan Position
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (channel == null)
                    return TimeSpan.Zero;

                var position = 0u;
                var result = FMOD_Channel_GetPosition(channel, &position, FMOD_TIMEUNIT_MS);
                if (result != FMOD_OK)
                    throw new FMODException(result);

                return TimeSpan.FromMilliseconds(position);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (State == PlaybackState.Stopped)
                    throw new InvalidOperationException("TODO");

                var result = FMOD_Channel_SetPosition(channel, (UInt32)value.TotalMilliseconds, FMOD_TIMEUNIT_MS);
                if (result != FMOD_OK)
                    throw new FMODException(result);
            }
        }

        /// <inheritdoc/>
        public override TimeSpan Duration
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (channel == null)
                    return TimeSpan.Zero;

                return duration;
            }
        }

        /// <inheritdoc/>
        public override Single Volume
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return (channel == null) ? 1f : volume;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (State == PlaybackState.Stopped)
                    throw new InvalidOperationException("TODO");

                var clamped = MathUtil.Clamp(value, 0f, 1f);
                var result = FMOD_Channel_SetVolume(channel, clamped);
                if (result != FMOD_OK)
                    throw new FMODException(result);

                this.volume = clamped;
                this.isSlidingVolume = false;
            }
        }

        /// <inheritdoc/>
        public override Single Pitch
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return (channel == null) ? 0f : pitch;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (State == PlaybackState.Stopped)
                    throw new InvalidOperationException("TODO");

                var clamped = MathUtil.Clamp(value, -1f, 1f);
                var result = FMOD_Channel_SetPitch(channel, 1f + clamped);
                if (result != FMOD_OK)
                    throw new FMODException(result);

                this.pitch = clamped;
                this.isSlidingPitch = false;
            }
        }

        /// <inheritdoc/>
        public override Single Pan
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return (channel == null) ? 0f : pan;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (State == PlaybackState.Stopped)
                    throw new InvalidOperationException("TODO");

                var clamped = MathUtil.Clamp(pan, -1f, 1f);
                var result = FMOD_Channel_SetPan(channel, clamped);
                if (result != FMOD_OK)
                    throw new FMODException(result);

                this.pan = clamped;
                this.isSlidingPan = false;
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Ultraviolet != null && !Ultraviolet.Disposed)
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

            var result = default(FMOD_RESULT);

            var system = ((FMODUltravioletAudio)Ultraviolet.GetAudio()).System;
            var sound = ((FMODSong)song).Sound;

            var channel = default(FMOD_CHANNEL*);
            var channelgroup = ((FMODUltravioletAudio)Ultraviolet.GetAudio()).ChannelGroupSongs;
            
            if (loopStart > TimeSpan.Zero && loopLength <= TimeSpan.Zero)
                throw new ArgumentException(nameof(loopLength));

            var looping = loopStart.HasValue || loopLength.HasValue;
            var loopStartMs = loopStart.HasValue ? (UInt32)loopStart.Value.TotalMilliseconds : 0;
            var loopEnd = (loopStart ?? TimeSpan.Zero) + loopLength;
            var loopEndMs = loopEnd.HasValue ? (UInt32)(loopEnd.Value.TotalMilliseconds - 1) : (UInt32)(song.Duration.TotalMilliseconds - 1);

            result = FMOD_System_PlaySound(system, sound, channelgroup, true, &channel);
            if (result != FMOD_OK)
                throw new FMODException(result);

            result = FMOD_Channel_SetVolume(channel, MathUtil.Clamp(volume, 0f, 1f));
            if (result != FMOD_OK)
                throw new FMODException(result);

            result = FMOD_Channel_SetPitch(channel, 1f + MathUtil.Clamp(pitch, -1f, 1f));
            if (result != FMOD_OK)
                throw new FMODException(result);

            result = FMOD_Channel_SetPan(channel, MathUtil.Clamp(pan, -1f, 1f));
            if (result != FMOD_OK)
                throw new FMODException(result);

            result = FMOD_Channel_SetMode(channel, looping ? FMOD_LOOP_NORMAL : FMOD_LOOP_OFF);
            if (result != FMOD_OK)
                throw new FMODException(result);

            result = FMOD_Channel_SetLoopPoints(channel, loopStartMs, FMOD_TIMEUNIT_MS, loopEndMs, FMOD_TIMEUNIT_MS);
            if (result != FMOD_OK)
                throw new FMODException(result);

            result = FMOD_Channel_SetPaused(channel, false);
            if (result != FMOD_OK)
                throw new FMODException(result);
            
            this.duration = song.Duration;
            this.volume = volume;
            this.pitch = pitch;
            this.pan = pan;

            OnStateChanged();
            OnSongStarted();

            return true;
        }

        /// <summary>
        /// Stops the playing song.
        /// </summary>
        private Boolean StopInternal()
        {
            if (channel == null)
                return false;

            var result = FMOD_Channel_Stop(channel);
            if (result != FMOD_OK)
                throw new FMODException(result);

            channel = null;

            isSlidingVolume = false;
            isSlidingPitch = false;
            isSlidingPan = false;

            return true;
        }

        /// <summary>
        /// Updates the player's volume, if its volume is sliding.
        /// </summary>
        private void UpdateSlidingVolume(UltravioletTime time)
        {
            if (!isSlidingVolume)
                return;

            slideClockVolume += time.ElapsedTime.TotalMilliseconds;

            var factor = (Single)MathUtil.Clamp(slideClockVolume / slideDurationVolume, 0.0, 1.0);
            var volume = MathUtil.Clamp(Tweening.Lerp(slideStartVolume, slideEndVolume, factor), 0f, 1f);

            var result = FMOD_Channel_SetVolume(channel, volume);
            if (result != FMOD_OK)
                throw new FMODException(result);

            this.volume = volume;

            if (factor == 1f)
                isSlidingVolume = false;
        }

        /// <summary>
        /// Updates the player's pitch, if its pitch is sliding.
        /// </summary>
        private void UpdateSlidingPitch(UltravioletTime time)
        {
            if (!isSlidingPitch)
                return;

            slideClockPitch += time.ElapsedTime.TotalMilliseconds;

            var factor = (Single)MathUtil.Clamp(slideClockPitch / slideDurationPitch, 0.0, 1.0);
            var pitch = MathUtil.Clamp(Tweening.Lerp(slideStartPitch, slideEndPitch, factor), -1f, 1f);

            var result = FMOD_Channel_SetPitch(channel, 1f + pitch);
            if (result != FMOD_OK)
                throw new FMODException(result);

            this.pitch = pitch;

            if (factor == 1f)
                isSlidingPitch = false;
        }

        /// <summary>
        /// Updates the player's pan, if its pan is sliding.
        /// </summary>
        private void UpdateSlidingPan(UltravioletTime time)
        {
            if (!isSlidingPan)
                return;

            slideClockPan += time.ElapsedTime.TotalMilliseconds;

            var factor = (Single)MathUtil.Clamp(slideClockPan / slideDurationPitch, 0.0, 1.0);
            var pan = MathUtil.Clamp(Tweening.Lerp(slideStartPan, slideEndPan, factor), -1f, 1f);

            var result = FMOD_Channel_SetPan(channel, pan);
            if (result != FMOD_OK)
                throw new FMODException(result);

            this.pan = pan;

            if (factor == 1f)
                isSlidingPan = false;
        }

        // State values.
        private FMOD_CHANNEL* channel;
        private Single volume;
        private Single pitch;
        private Single pan;
        private TimeSpan duration;

        // Sliding parameters.
        private Boolean isSlidingVolume;
        private Boolean isSlidingPitch;
        private Boolean isSlidingPan;
        private Single slideStartVolume;
        private Single slideStartPitch;
        private Single slideStartPan;
        private Single slideEndVolume;
        private Single slideEndPitch;
        private Single slideEndPan;
        private Double slideClockVolume;
        private Double slideClockPitch;
        private Double slideClockPan;
        private Double slideDurationVolume;
        private Double slideDurationPitch;
        private Double slideDurationPan;
    }
}
