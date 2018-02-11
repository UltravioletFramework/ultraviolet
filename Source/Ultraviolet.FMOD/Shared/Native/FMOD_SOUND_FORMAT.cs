namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    public enum FMOD_SOUND_FORMAT
    {
        FMOD_SOUND_FORMAT_NONE,             /* Unitialized / unknown. */
        FMOD_SOUND_FORMAT_PCM8,             /* 8bit integer PCM data. */
        FMOD_SOUND_FORMAT_PCM16,            /* 16bit integer PCM data. */
        FMOD_SOUND_FORMAT_PCM24,            /* 24bit integer PCM data. */
        FMOD_SOUND_FORMAT_PCM32,            /* 32bit integer PCM data. */
        FMOD_SOUND_FORMAT_PCMFLOAT,         /* 32bit floating point PCM data. */
        FMOD_SOUND_FORMAT_BITSTREAM,        /* Sound data is in its native compressed format. */

        FMOD_SOUND_FORMAT_MAX,              /* Maximum number of sound formats supported. */
    }
#pragma warning restore 1591
}
