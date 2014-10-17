using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Audio;
using TwistedLogik.Ultraviolet.BASS.Native;

namespace TwistedLogik.Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Represents the BASS implementation of the Song class.
    /// </summary>
    public sealed class BASSSong : Song
    {
        /// <summary>
        /// Initializes a new instance of the BASSSong class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="file">The path to the file from which to stream the song.</param>
        public BASSSong(UltravioletContext uv, String file)
            : base(uv)
        {
            Contract.RequireNotEmpty(file, "file");

            var stream = BASSNative.StreamCreateFile(file, BASSNative.BASS_STREAM_DECODE);
            if (!BASSUtil.IsValidHandle(stream))
                throw new BASSException();

            var duration = BASSUtil.GetDurationInSeconds(stream);

            if (!BASSNative.StreamFree(stream))
                throw new BASSException();

            this.file = file;
            this.duration = TimeSpan.FromSeconds(duration);
        }

        /// <summary>
        /// Gets the song's total playback length.
        /// </summary>
        public override TimeSpan Duration
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return duration;
            }
        }

        /// <summary>
        /// Creates a BASS stream that represents the song.
        /// </summary>
        /// <param name="flags">The flags to apply to the stream that is created.</param>
        /// <returns>The handle to the BASS stream that was created.</returns>
        internal UInt32 CreateStream(UInt32 flags)
        {
            var stream = BASSNative.StreamCreateFile(file, flags);
            if (!BASSUtil.IsValidHandle(stream))
                throw new BASSException();

            return stream;
        }

        // The file from which to stream the song.
        private readonly String file;
        private readonly TimeSpan duration;
    }
}
