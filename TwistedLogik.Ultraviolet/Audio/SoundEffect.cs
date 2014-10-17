using System;

namespace TwistedLogik.Ultraviolet.Audio
{
    /// <summary>
    /// Represents a sound effect.
    /// </summary>
    public abstract class SoundEffect : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the SoundEffect class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        protected SoundEffect(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Plays the sound effect.
        /// </summary>
        public abstract void Play();

        /// <summary>
        /// Plays the sound effect with the specified parameters.
        /// </summary>
        /// <param name="volume">A value from 0.0 (silent) to 1.0 (full volume) representing the sound effect's volume.</param>
        /// <param name="pitch">A value from -1.0 (down one octave) to 1.0 (up one octave) indicating the sound effect's pitch adjustment.</param>
        /// <param name="pan">A value from -1.0 (full left) to 1.0 (full right) representing the sound effect's panning position.</param>
        public abstract void Play(Single volume, Single pitch, Single pan);

        /// <summary>
        /// Gets the sound effect's duration.
        /// </summary>
        public abstract TimeSpan Duration
        {
            get;
        }
    }
}
