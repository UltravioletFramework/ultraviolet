using System;

namespace Ultraviolet.Audio
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="SoundEffectPlayer"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="SoundEffectPlayer"/> that was created.</returns>
    public delegate SoundEffectPlayer SoundEffectPlayerFactory(UltravioletContext uv);

    /// <summary>
    /// Represents an object which plays <see cref="SoundEffect"/> resources.
    /// </summary>
    public abstract class SoundEffectPlayer : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEffectPlayer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        protected SoundEffectPlayer(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="SoundEffectPlayer"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="SoundEffectPlayer"/> that was created.</returns>
        public static SoundEffectPlayer Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<SoundEffectPlayerFactory>()(uv);
        }

        /// <summary>
        /// Updates the player's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last update.</param>
        public abstract void Update(UltravioletTime time);

        /// <summary>
        /// Plays a <see cref="SoundEffect"/>.
        /// </summary>
        /// <param name="soundEffect">The <see cref="SoundEffect"/> to play.</param>
        /// <param name="loop">A value indicating whether to loop the sound effect.</param>
        /// <returns><see langword="true"/> if the sound effect began playing successfully; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean Play(SoundEffect soundEffect, Boolean loop = false);

        /// <summary>
        /// Plays a <see cref="SoundEffect"/>.
        /// </summary>
        /// <param name="soundEffect">The <see cref="SoundEffect"/> to play.</param>
        /// <param name="volume">A value from 0.0 (silent) to 1.0 (full volume) representing the sound effect's volume.</param>
        /// <param name="pitch">A value from -1.0 (down one octave) to 1.0 (up one octave) indicating the sound effect's pitch adjustment.</param>
        /// <param name="pan">A value from -1.0 (full left) to 1.0 (full right) representing the sound effect's panning position.</param>
        /// <param name="loop">A value indicating whether to loop the sound effect.</param>
        /// <returns><see langword="true"/> if the sound effect began playing successfully; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean Play(SoundEffect soundEffect, Single volume, Single pitch, Single pan, Boolean loop = false);

        /// <summary>
        /// Stops the sound effect that is currently playing.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Pauses the sound effect that is currently playing.
        /// </summary>
        public abstract void Pause();

        /// <summary>
        /// Resumes the sound effect after it has been paused.
        /// </summary>
        public abstract void Resume();

        /// <summary>
        /// Slides the sound effect's volume to the specified value over the specified period of time.
        /// </summary>
        /// <param name="volume">The value to which to slide the sound effect's volume.</param>
        /// <param name="time">The amount of time over which to perform the slide.</param>
        public abstract void SlideVolume(Single volume, TimeSpan time);

        /// <summary>
        /// Slides the sound effect's pitch to the specified value over the specified period of time.
        /// </summary>
        /// <param name="pitch">The value to which to slide the sound effect's pitch.</param>
        /// <param name="time">The amount of time over which to perform the slide.</param>
        public abstract void SlidePitch(Single pitch, TimeSpan time);

        /// <summary>
        /// Slides the sound effect's pan to the specified value over the specified period of time.
        /// </summary>
        /// <param name="pan">The value to which to slide the sound effect's pan.</param>
        /// <param name="time">The amount of time over which to perform the slide.</param>
        public abstract void SlidePan(Single pan, TimeSpan time);

        /// <summary>
        /// Gets the sound effect player's current playback state.
        /// </summary>
        public abstract PlaybackState State
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the sound effect player is playing a sound effect.
        /// </summary>
        public abstract Boolean IsPlaying
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the sound effect player is looping.
        /// </summary>
        public abstract Boolean IsLooping
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sound effect player's position within the currently-playing sound effect.
        /// </summary>
        /// <remarks>If no sound effect is currently playing, <see cref="System.TimeSpan.Zero"/> is returned.</remarks>
        public abstract TimeSpan Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the duration of the currently-playing sound effect.
        /// </summary>
        /// <remarks>If no sound effect is currently playing, <see cref="System.TimeSpan.Zero"/> is returned.</remarks>
        public abstract TimeSpan Duration
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value from 0.0 (silent) to 1.0 (full volume) representing the 
        /// volume of the currently-playing sound effect.
        /// </summary>
        public abstract Single Volume
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value from -1.0 (down one octave) to 1.0 (up one octave) indicating the 
        /// pitch adjustment of the currently-playing sound effect.
        /// </summary>
        public abstract Single Pitch
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value from -1.0 (full left) to 1.0 (full right) representing the 
        /// panning position of the currently-playing sound effect.
        /// </summary>
        public abstract Single Pan
        {
            get;
            set;
        }
    }
}
