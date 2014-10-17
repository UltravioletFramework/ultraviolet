using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Audio;

namespace TwistedLogik.Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Represents the FMOD implementation of the SongPlayer class.
    /// </summary>
    public sealed class FMODSongPlayer : SongPlayer
    {
        /// <summary>
        /// Initializes a new instance of the FMODSongPlayer class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public FMODSongPlayer(UltravioletContext uv)
            : base(uv)
        {
            this.volumeSlider = new SlidingValue(() => volume, UpdateVolume);
            this.pitchSlider  = new SlidingValue(() => pitch,  UpdatePitch);
            this.panSlider    = new SlidingValue(() => pan,    UpdatePan);
        }

        /// <summary>
        /// Updates the player's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last update.</param>
        public override void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            this.volumeSlider.Update(time);
            this.pitchSlider.Update(time);
            this.panSlider.Update(time);
        }

        /// <summary>
        /// Plays the specified song.
        /// </summary>
        /// <param name="song">The song to play.</param>
        /// <param name="loop">A value indicating whether to loop the song.</param>
        /// <returns>true if the song began playing successfully; otherwise, false.</returns>
        public override Boolean Play(Song song, Boolean loop = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(song, "song");

            return PlayInternal(song, 1f, 0f, 0f, loop);
        }

        /// <summary>
        /// Plays the specified song.
        /// </summary>
        /// <param name="song">The song to play.</param>
        /// <param name="volume">A value from 0.0 (silent) to 1.0 (full volume) representing the song's volume.</param>
        /// <param name="pitch">A value from -1.0 (down one octave) to 1.0 (up one octave) indicating the song's pitch adjustment.</param>
        /// <param name="pan">A value from -1.0 (full left) to 1.0 (full right) representing the song's panning position.</param>
        /// <param name="loop">A value indicating whether to loop the song.</param>
        /// <returns>true if the song began playing successfully; otherwise, false.</returns>
        public override Boolean Play(Song song, Single volume, Single pitch, Single pan, Boolean loop = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(song, "song");

            return PlayInternal(song, volume, pitch, pan, loop);
        }

        /// <summary>
        /// Stops the song that is currently playing.
        /// </summary>
        public override void Stop()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (State != PlaybackState.Stopped)
            {
                var result = channel.stop();
                FMODUtil.CheckResult(result);

                channel = null;
            }
        }

        /// <summary>
        /// Pauses the song that is currently playing.
        /// </summary>
        public override void Pause()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (State == PlaybackState.Playing)
            {
                var result = channel.setPaused(true);
                FMODUtil.CheckResult(result);
            }
        }

        /// <summary>
        /// Resumes the song after it has been paused.
        /// </summary>
        public override void Resume()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (State == PlaybackState.Paused)
            {
                var result = channel.setPaused(false);
                FMODUtil.CheckResult(result);
            }
        }

        /// <summary>
        /// Slides the song's volume to the specified value over the specified period of time.
        /// </summary>
        /// <param name="volume">The value to which to slide the song's volume.</param>
        /// <param name="time">The amount of time over which to perform the slide.</param>
        public override void SlideVolume(Single volume, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            EnsureChannelIsValid();

            volumeSlider.Slide(volume, time.TotalMilliseconds);
        }

        /// <summary>
        /// Slides the song's pitch to the specified value over the specified period of time.
        /// </summary>
        /// <param name="pitch">The value to which to slide the song's pitch.</param>
        /// <param name="time">The amount of time over which to perform the slide.</param>
        public override void SlidePitch(Single pitch, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            EnsureChannelIsValid();

            pitchSlider.Slide(volume, time.TotalMilliseconds);
        }

        /// <summary>
        /// Slides the song's pan to the specified value over the specified period of time.
        /// </summary>
        /// <param name="pan">The value to which to slide the song's pan.</param>
        /// <param name="time">The amount of time over which to perform the slide.</param>
        public override void SlidePan(Single pan, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            EnsureChannelIsValid();

            panSlider.Slide(volume, time.TotalMilliseconds);
        }

        /// <summary>
        /// Gets the player's current playback state.
        /// </summary>
        public override PlaybackState State
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (channel != null)
                {
                    var result = default(FMODNative.RESULT);

                    var isplaying = false;
                    result = channel.isPlaying(out isplaying);
                    FMODUtil.CheckResult(result);

                    if (isplaying)
                        return PlaybackState.Playing;

                    var paused = false;
                    result = channel.getPaused(out paused);

                    if (paused)
                        return PlaybackState.Paused;
                }

                return PlaybackState.Stopped;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the song is playing.
        /// </summary>
        public override Boolean IsPlaying
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return State == PlaybackState.Playing;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the song is looping.
        /// </summary>
        public override Boolean IsLooping
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return false;

                var mode = default(FMODNative.MODE);
                var result = channel.getMode(out mode);
                FMODUtil.CheckResult(result);

                return (mode & FMODNative.MODE.LOOP_NORMAL) == FMODNative.MODE.LOOP_NORMAL;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                FMODUtil.SetLooping(channel, value);
            }
        }

        /// <summary>
        /// Gets or sets the song's current playback position.
        /// </summary>
        public override TimeSpan Position
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return TimeSpan.Zero;

                var ms = 0u;
                var result = channel.getPosition(out ms, FMODNative.TIMEUNIT.MS);
                FMODUtil.CheckResult(result);

                return TimeSpan.FromMilliseconds(ms);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                if (value.TotalSeconds < 0 || value > Duration)
                    throw new ArgumentOutOfRangeException("value");

                var position = (uint)value.TotalMilliseconds;
                var result = channel.setPosition(position, FMODNative.TIMEUNIT.MS);
                FMODUtil.CheckResult(result);
            }
        }

        /// <summary>
        /// Gets the song's duration.
        /// </summary>
        public override TimeSpan Duration
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return TimeSpan.Zero;

                var result = default(FMODNative.RESULT);

                var sound = default(FMODNative.Sound);
                result = channel.getCurrentSound(out sound);
                FMODUtil.CheckResult(result);

                var ms = 0u;
                result = sound.getLength(out ms, FMODNative.TIMEUNIT.MS);
                FMODUtil.CheckResult(result);

                return TimeSpan.FromMilliseconds(ms);
            }
        }

        /// <summary>
        /// Gets or sets a value from 0.0 (silent) to 1.0 (full volume) representing the song's volume.
        /// </summary>
        public override Single Volume
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return 1f;

                return volume;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                volumeSlider.Stop();
                UpdateVolume(value);
            }
        }

        /// <summary>
        /// Gets or sets a value from -1.0 (down one octave) to 1.0 (up one octave) indicating the song's pitch adjustment.
        /// </summary>
        public override Single Pitch
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return 0f;

                return pitch;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                pitchSlider.Stop();
                UpdatePitch(value);
            }
        }

        /// <summary>
        /// Gets or sets a value from -1.0 (full left) to 1.0 (full right) representing the song's panning position.
        /// </summary>
        public override Single Pan
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return 0f;

                return pan;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                panSlider.Stop();
                UpdatePan(value);
            }
        }

        /// <summary>
        /// Plays the specified song.
        /// </summary>
        private Boolean PlayInternal(Song song, Single volume, Single pitch, Single pan, Boolean loop)
        {
            Ultraviolet.ValidateResource(song);

            Stop();

            var fmodSong = (FMODSong)song;
            var system = fmodSong.System;
            var sound = fmodSong.Sound;
            var group = ((FMODUltravioletAudio)Ultraviolet.GetAudio()).StreamsGroup;

            var result = default(FMODNative.RESULT);

            result = system.playSound(sound, group, true, out channel);
            FMODUtil.CheckResult(result);

            result = channel.getFrequency(out frequency);
            FMODUtil.CheckResult(result);

            FMODUtil.SetLooping(channel, loop);

            this.volumeSlider.Stop();
            this.UpdateVolume(volume);

            this.pitchSlider.Stop();
            this.UpdatePitch(pitch);

            this.panSlider.Stop();
            this.UpdatePan(pan);

            result = channel.setPaused(false);
            FMODUtil.CheckResult(result);

            return true;
        }

        /// <summary>
        /// Updates the player's volume value.
        /// </summary>
        /// <param name="value">The updated value.</param>
        private void UpdateVolume(Single value)
        {
            volume = MathUtil.Clamp(value, -1f, 1f);
            FMODUtil.SetVolume(channel, volume);
        }

        /// <summary>
        /// Updates the player's pitch value.
        /// </summary>
        /// <param name="value">The updated value.</param>
        private void UpdatePitch(Single value)
        {
            pitch = MathUtil.Clamp(value, -1f, 1f);
            FMODUtil.SetPitch(channel, frequency, pitch);
        }

        /// <summary>
        /// Updates the player's pan value.
        /// </summary>
        /// <param name="value">The updated value.</param>
        private void UpdatePan(Single value)
        {
            pan = MathUtil.Clamp(value, -1f, 1f);
            FMODUtil.SetPan(channel, pan);
        }

        /// <summary>
        /// Throws an <see cref="System.InvalidOperationException"/> if the channel is not in a valid state.
        /// </summary>
        private void EnsureChannelIsValid()
        {
            if (State == PlaybackState.Stopped)
            {
                throw new InvalidOperationException(FMODStrings.NotCurrentlyValid);
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

        // Property values.
        private Single volume = 1f;
        private Single pitch = 1f;
        private Single pan = 0f;
        private readonly SlidingValue volumeSlider;
        private readonly SlidingValue pitchSlider;
        private readonly SlidingValue panSlider;

        // State values.
        private FMODNative.Channel channel;
        private Single frequency;
    }
}
