using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Audio;

namespace TwistedLogik.Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Represents the FMOD implementation of the Song class.
    /// </summary>
    public sealed class FMODSong : Song
    {
        /// <summary>
        /// Initializes a new instance of the FMODSong class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="file">The path to the file from which to stream the song.</param>
        public FMODSong(UltravioletContext uv, String file)
            : base(uv)
        {
            Contract.RequireNotEmpty(file, "file");

            system = ((FMODUltravioletAudio)uv.GetAudio()).System;

            var mode   = FMODNative.MODE.CREATESTREAM | FMODNative.MODE._2D | FMODNative.MODE.LOOP_OFF;
            var result = system.createSound(file, mode, out sound);
            FMODUtil.CheckResult(result);
        }

        /// <summary>
        /// Gets the native FMOD System object.
        /// </summary>
        public FMODNative.System System
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return system;
            }
        }

        /// <summary>
        /// Gets the song's native FMOD Sound object.
        /// </summary>
        public FMODNative.Sound Sound
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return sound; 
            }
        }

        /// <summary>
        /// Gets the song's total playback length.
        /// </summary>
        public override TimeSpan Duration
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                var ms = 0u;
                var result = sound.getLength(out ms, FMODNative.TIMEUNIT.MS);
                FMODUtil.CheckResult(result);

                return TimeSpan.FromMilliseconds(ms);
            }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            sound.release();

            base.Dispose(disposing);
        }

        // State values.
        private readonly FMODNative.System system;
        private readonly FMODNative.Sound sound;
    }
}
