using System;
using System.Runtime.InteropServices;
using Ultraviolet.Audio;
using Ultraviolet.Core;
using Ultraviolet.FMOD.Native;
using static Ultraviolet.FMOD.Native.FMOD_MODE;
using static Ultraviolet.FMOD.Native.FMOD_RESULT;
using static Ultraviolet.FMOD.Native.FMODNative;

namespace Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Represents the FMOD implementation of the <see cref="SoundEffect"/> class.
    /// </summary>
    public sealed unsafe class FMODSoundEffect : SoundEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FMODSoundEffect"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="file">The path to the file from which to load the sound effect.</param>
        public FMODSoundEffect(UltravioletContext uv, String file)
            : base(uv)
        {
            Contract.RequireNotEmpty(file, nameof(file));

            var result = default(FMOD_RESULT);
            var system = ((FMODUltravioletAudio)uv.GetAudio()).System;

            fixed (FMOD_SOUND** psound = &sound)
            {
                var exinfo = new FMOD_CREATESOUNDEXINFO();
                exinfo.cbsize = Marshal.SizeOf(exinfo);

                result = FMOD_System_CreateStream(system, file, FMOD_DEFAULT, &exinfo, psound);
                if (result != FMOD_OK)
                    throw new FMODException(result);
            }
            
            var durationInMilliseconds = 0u;

            result = FMOD_Sound_GetLength(sound, &durationInMilliseconds, FMOD_TIMEUNIT.FMOD_TIMEUNIT_MS);
            if (result != FMOD_OK)
                throw new FMODException(result);

            this.duration = TimeSpan.FromMilliseconds(durationInMilliseconds);
        }

        /// <inheritdoc/>
        public override void Play()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var result = default(FMOD_RESULT);

            var system = ((FMODUltravioletAudio)Ultraviolet.GetAudio()).System;
            var channel = default(FMOD_CHANNEL*);
            var channelgroup = ChannelGroup;

            result = FMOD_System_PlaySound(system, sound, channelgroup, false, &channel);
            if (result != FMOD_OK)
                throw new FMODException(result);
        }

        /// <inheritdoc/>
        public override void Play(Single volume, Single pitch, Single pan)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var result = default(FMOD_RESULT);

            var system = ((FMODUltravioletAudio)Ultraviolet.GetAudio()).System;
            var channel = default(FMOD_CHANNEL*);
            var channelgroup = ChannelGroup;

            result = FMOD_System_PlaySound(system, sound, channelgroup, true, &channel);
            if (result != FMOD_OK)
                throw new FMODException(result);

            result = FMOD_Channel_SetVolume(channel, MathUtil.Clamp(volume, 0f, 1f));
            if (result != FMOD_OK)
                throw new FMODException(result);

            result = FMOD_Channel_SetPitch(channel, 1f + MathUtil.Clamp(volume, -1f, 1f));
            if (result != FMOD_OK)
                throw new FMODException(result);

            result = FMOD_Channel_SetPan(channel, MathUtil.Clamp(volume, -1f, 1f));
            if (result != FMOD_OK)
                throw new FMODException(result);

            result = FMOD_Channel_SetPaused(channel, false);
            if (result != FMOD_OK)
                throw new FMODException(result);
        }

        /// <inheritdoc/>
        public override TimeSpan Duration => duration;
        
        /// <summary>
        /// Gets the FMOD sound pointer for this object.
        /// </summary>
        internal FMOD_SOUND* Sound => sound;

        /// <summary>
        /// Gets the FMOD channel group for this object.
        /// </summary>
        internal FMOD_CHANNELGROUP* ChannelGroup => ((FMODUltravioletAudio)Ultraviolet.GetAudio()).ChannelGroupSoundEffects;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            var result = FMOD_Sound_Release(sound);
            if (result != FMOD_OK)
                throw new FMODException(result);

            base.Dispose(disposing);
        }

        // FMOD state variables.
        private readonly FMOD_SOUND* sound;
        private readonly TimeSpan duration;
    }
}
