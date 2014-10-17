using System;

namespace TwistedLogik.Ultraviolet.Audio
{
    /// <summary>
    /// Represents a song.
    /// </summary>
    public abstract class Song : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the Song class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        protected Song(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Gets the song's duration.
        /// </summary>
        public abstract TimeSpan Duration
        {
            get;
        }
    }
}
