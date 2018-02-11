using System;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    [Flags]
    public enum FMOD_TIMEUNIT : uint
    {
        FMOD_TIMEUNIT_MS            = 0x00000001,  /* Milliseconds. */
        FMOD_TIMEUNIT_PCM           = 0x00000002,  /* PCM samples, related to milliseconds * samplerate / 1000. */
        FMOD_TIMEUNIT_PCMBYTES      = 0x00000004,  /* Bytes, related to PCM samples * channels * datawidth (ie 16bit = 2 bytes). */
        FMOD_TIMEUNIT_RAWBYTES      = 0x00000008,  /* Raw file bytes of (compressed) sound data (does not include headers).  Only used by Sound::getLength and Channel::getPosition. */
        FMOD_TIMEUNIT_PCMFRACTION   = 0x00000010,  /* Fractions of 1 PCM sample.  Unsigned int range 0 to = 0xFFFFFFFF.  Used for sub-sample granularity for DSP purposes. */
        FMOD_TIMEUNIT_MODORDER      = 0x00000100,  /* MOD/S3M/XM/IT.  Order in a sequenced module format.  Use Sound::getFormat to determine the PCM format being decoded to. */
        FMOD_TIMEUNIT_MODROW        = 0x00000200,  /* MOD/S3M/XM/IT.  Current row in a sequenced module format.  Cannot use with Channel::setPosition.  Sound::getLength will return the number of rows in the currently playing or seeked to pattern. */
        FMOD_TIMEUNIT_MODPATTERN    = 0x00000400,  /* MOD/S3M/XM/IT.  Current pattern in a sequenced module format.  Cannot use with Channel::setPosition.  Sound::getLength will return the number of patterns in the song and Channel::getPosition will return the currently playing pattern. */
    }
#pragma warning restore 1591
}
