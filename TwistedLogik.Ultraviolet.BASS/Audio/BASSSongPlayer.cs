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

        /// <summary>
        /// Updates the player's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last update.</param>
        public override void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);
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
                if (!BASSNative.ChannelStop(stream))
                    throw new BASSException();

                stream = 0;
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
                if (!BASSNative.ChannelPause(stream))
                    throw new BASSException();
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
                if (!BASSNative.ChannelPlay(stream, false))
                    throw new BASSException();
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

            BASSUtil.SlideVolume(stream, volume, time);
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

            BASSUtil.SlidePitch(stream, pitch, time);
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

            BASSUtil.SlidePan(stream, pan, time);
        }

        /// <summary>
        /// Gets the player's current playback state.
        /// </summary>
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

                return BASSUtil.GetIsLooping(stream);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                BASSUtil.SetIsLooping(stream, value);
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

                var duration = BASSUtil.GetDurationInSeconds(stream);
                return TimeSpan.FromSeconds(duration);
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

                return BASSUtil.GetVolume(stream);
            }
            set 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                BASSUtil.SetVolume(stream, MathUtil.Clamp(value, -1f, 1f));
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

                return BASSUtil.GetPitch(stream);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                BASSUtil.SetPitch(stream, MathUtil.Clamp(value, -1f, 1f));
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

                return BASSUtil.GetPan(stream);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                BASSUtil.SetPan(stream, MathUtil.Clamp(value, -1f, 1f));
            }
        }

        /// <summary>
        /// Plays the specified song.
        /// </summary>
        private Boolean PlayInternal(Song song, Single volume, Single pitch, Single pan, Boolean loop)
        {
            Ultraviolet.ValidateResource(song);

            Stop();

            stream = ((BASSSong)song).CreateStream(BASSNative.BASS_STREAM_DECODE);
            stream = BASSFXNative.TempoCreate(stream, BASSNative.BASS_FX_FREESOURCE | BASSNative.BASS_STREAM_AUTOFREE);
            if (!BASSUtil.IsValidHandle(stream))
                throw new BASSException();

            BASSUtil.SetIsLooping(stream, loop);
            BASSUtil.SetVolume(stream, MathUtil.Clamp(volume, 0f, 1f));
            BASSUtil.SetPitch(stream, MathUtil.Clamp(pitch, -1f, 1f));
            BASSUtil.SetPan(stream, MathUtil.Clamp(pan, -1f, 1f));

            if (!BASSNative.ChannelPlay(stream, true))
                throw new BASSException();

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
        /// Gets a value indicating whether the channel is in a valid state.
        /// </summary>
        /// <returns>true if the channel is in a valid state; otherwise, false.</returns>
        private Boolean IsChannelValid()
        {
            return State != PlaybackState.Stopped;
        }

        // State values.
        private UInt32 stream;
    }
}
