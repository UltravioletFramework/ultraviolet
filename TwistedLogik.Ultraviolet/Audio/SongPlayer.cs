using System;

namespace Ultraviolet.Audio
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="SongPlayer"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="SongPlayer"/> that was created.</returns>
    public delegate SongPlayer SongPlayerFactory(UltravioletContext uv);

    /// <summary>
    /// Represents the method that is called when a <see cref="SongPlayer"/> raises an event.
    /// </summary>
    /// <param name="songPlayer">The <see cref="SongPlayer"/> that raised the event.</param>
    public delegate void SongPlayerEventHandler(SongPlayer songPlayer);

    /// <summary>
    /// Represents an object which plays <see cref="Song"/> resources.
    /// </summary>
    public abstract class SongPlayer : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SongPlayer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        protected SongPlayer(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="SongPlayer"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="SongPlayer"/> that was created.</returns>
        public static SongPlayer Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<SongPlayerFactory>()(uv);
        }

        /// <summary>
        /// Updates the player's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last update.</param>
        public abstract void Update(UltravioletTime time);

        /// <summary>
        /// Plays the specified <see cref="Song"/>.
        /// </summary>
        /// <param name="song">The <see cref="Song"/> to play.</param>
        /// <param name="loop">A value indicating whether to loop the song.</param>
        /// <returns><see langword="true"/> if the song began playing successfully; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean Play(Song song, Boolean loop = false);

        /// <summary>
        /// Plays the specified <see cref="Song"/>.
        /// </summary>
        /// <param name="song">The <see cref="Song"/> to play.</param>
        /// <param name="loopStart">The time at which the song beings looping.</param>
        /// <param name="loopLength">The length of the portion of the song which loops, or <see langword="null"/>
        /// to loop until the end of the song.</param>
        /// <returns><see langword="true"/> if the song began playing successfully; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean Play(Song song, TimeSpan loopStart, TimeSpan? loopLength = null);

        /// <summary>
        /// Plays the specified <see cref="Song"/>.
        /// </summary>
        /// <param name="song">The <see cref="Song"/> to play.</param>
        /// <param name="volume">A value from 0.0 (silent) to 1.0 (full volume) representing the song's volume.</param>
        /// <param name="pitch">A value from -1.0 (down one octave) to 1.0 (up one octave) indicating the song's pitch adjustment.</param>
        /// <param name="pan">A value from -1.0 (full left) to 1.0 (full right) representing the song's panning position.</param>
        /// <param name="loop">A value indicating whether to loop the song.</param>
        /// <returns><see langword="true"/> if the song began playing successfully; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean Play(Song song, Single volume, Single pitch, Single pan, Boolean loop = false);

        /// <summary>
        /// Plays the specified <see cref="Song"/>.
        /// </summary>
        /// <param name="song">The <see cref="Song"/> to play.</param>
        /// <param name="volume">A value from 0.0 (silent) to 1.0 (full volume) representing the song's volume.</param>
        /// <param name="pitch">A value from -1.0 (down one octave) to 1.0 (up one octave) indicating the song's pitch adjustment.</param>
        /// <param name="pan">A value from -1.0 (full left) to 1.0 (full right) representing the song's panning position.</param>
        /// <param name="loopStart">The time at which the song beings looping.</param>
        /// <param name="loopLength">The length of the portion of the song which loops, or <see langword="null"/>
        /// to loop until the end of the song.</param>
        /// <returns><see langword="true"/> if the song began playing successfully; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean Play(Song song, Single volume, Single pitch, Single pan, TimeSpan loopStart, TimeSpan? loopLength = null);

        /// <summary>
        /// Stops the song that is currently playing.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Pauses the song that is currently playing.
        /// </summary>
        public abstract void Pause();

        /// <summary>
        /// Resumes the song after it has been paused.
        /// </summary>
        public abstract void Resume();

        /// <summary>
        /// Slides the song's volume to the specified value over the specified period of time.
        /// </summary>
        /// <param name="volume">The value to which to slide the song's volume.</param>
        /// <param name="time">The amount of time over which to perform the slide.</param>
        public abstract void SlideVolume(Single volume, TimeSpan time);

        /// <summary>
        /// Slides the song's pitch to the specified value over the specified period of time.
        /// </summary>
        /// <param name="pitch">The value to which to slide the song's pitch.</param>
        /// <param name="time">The amount of time over which to perform the slide.</param>
        public abstract void SlidePitch(Single pitch, TimeSpan time);

        /// <summary>
        /// Slides the song's pan to the specified value over the specified period of time.
        /// </summary>
        /// <param name="pan">The value to which to slide the song's pan.</param>
        /// <param name="time">The amount of time over which to perform the slide.</param>
        public abstract void SlidePan(Single pan, TimeSpan time);

        /// <summary>
        /// Gets the song player's current playback state.
        /// </summary>
        public abstract PlaybackState State
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the song player is playing a song.
        /// </summary>
        public abstract Boolean IsPlaying
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the song player is looping.
        /// </summary>
        public abstract Boolean IsLooping
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the song player's position within the currently-playing song.
        /// </summary>
        public abstract TimeSpan Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the duration of the currently-playing song.
        /// </summary>
        public abstract TimeSpan Duration
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value from 0.0 (silent) to 1.0 (full volume) representing the volume of the currently-playing song.
        /// </summary>
        public abstract Single Volume
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value from -1.0 (down one octave) to 1.0 (up one octave) indicating the pitch adjustment of the currently-playing song.
        /// </summary>
        public abstract Single Pitch
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value from -1.0 (full left) to 1.0 (full right) representing the panning position of the currently-playing song.
        /// </summary>
        public abstract Single Pan
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when playback starts.
        /// </summary>
        public event SongPlayerEventHandler SongStarted;

        /// <summary>
        /// Occurs when playback ends.
        /// </summary>
        public event SongPlayerEventHandler SongEnded;

        /// <summary>
        /// Occurs when the song player's playback state changes.
        /// </summary>
        public event SongPlayerEventHandler StateChanged;

        /// <summary>
        /// Raises the <see cref="SongStarted"/> event.
        /// </summary>
        protected void OnSongStarted() =>
            SongStarted?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="SongEnded"/> event.
        /// </summary>
        protected void OnSongEnded() =>
            SongEnded?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="StateChanged"/> event.
        /// </summary>
        protected void OnStateChanged() =>
            StateChanged?.Invoke(this);
    }
}
