using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Audio;

namespace TwistedLogik.Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Represents the FMOD implementation of the SoundEffect class.
    /// </summary>
    public sealed class FMODSoundEffect : SoundEffect
    {
        /// <summary>
        /// Initializes a new instance of the FMODSoundEffect class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="filename">The filename of the sample to load.</param>
        public FMODSoundEffect(UltravioletContext uv, String filename)
            : base(uv)
        {
            Contract.RequireNotEmpty(filename, "filename");

            system = ((FMODUltravioletAudio)uv.GetAudio()).System;

            var mode   = FMODNative.MODE.CREATESAMPLE | FMODNative.MODE._2D | FMODNative.MODE.LOOP_OFF;
            var result = system.createSound(filename, mode, out sound);
            FMODUtil.CheckResult(result);
        }

        /// <summary>
        /// Plays the sound effect in a fire-and-forget fashion.
        /// </summary>
        /// <remarks>If you need to control the sound effect's properties while it is playing, play it
        /// using an instance of the <see cref="SoundEffectPlayer"/> class instead.</remarks>
        public override void Play()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            FMODNative.Channel channel;
            FMODNative.RESULT result;

            var group = ((FMODUltravioletAudio)Ultraviolet.GetAudio()).SamplesGroup;
            result = system.playSound(sound, group, false, out channel);
            FMODUtil.CheckResult(result);
        }

        /// <summary>
        /// Plays the sound effect in a fire-and-forget fashion with the specified parameters.
        /// </summary>
        /// <remarks>If you need to control the sound effect's properties while it is playing, play it
        /// using an instance of the <see cref="SoundEffectPlayer"/> class instead.</remarks>
        /// <param name="volume">A value from 0.0 (silent) to 1.0 (full volume) representing the sound effect's volume.</param>
        /// <param name="pitch">A value from -1.0 (down one octave) to 1.0 (up one octave) indicating the sound effect's pitch adjustment.</param>
        /// <param name="pan">A value from -1.0 (full left) to 1.0 (full right) representing the sound effect's panning position.</param>
        public override void Play(Single volume, Single pitch, Single pan)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            FMODNative.Channel channel;
            FMODNative.RESULT result;

            var group = ((FMODUltravioletAudio)Ultraviolet.GetAudio()).SamplesGroup;
            result = system.playSound(sound, group, true, out channel);
            FMODUtil.CheckResult(result);

            var frequency = 0f;
            result = channel.getFrequency(out frequency);
            FMODUtil.CheckResult(result);

            result = channel.setVolume(volume);
            FMODUtil.CheckResult(result);

            FMODUtil.SetPitch(channel, frequency, pitch);

            result = channel.setPan(pan);
            FMODUtil.CheckResult(result);

            result = channel.setPaused(false);
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
        /// Gets the sound effect's duration.
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
