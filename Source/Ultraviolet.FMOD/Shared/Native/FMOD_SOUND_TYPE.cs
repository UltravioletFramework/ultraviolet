namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    public enum FMOD_SOUND_TYPE
    {
        FMOD_SOUND_TYPE_UNKNOWN,         /* 3rd party / unknown plugin format. */
        FMOD_SOUND_TYPE_AIFF,            /* AIFF. */
        FMOD_SOUND_TYPE_ASF,             /* Microsoft Advanced Systems Format (ie WMA/ASF/WMV). */
        FMOD_SOUND_TYPE_DLS,             /* Sound font / downloadable sound bank. */
        FMOD_SOUND_TYPE_FLAC,            /* FLAC lossless codec. */
        FMOD_SOUND_TYPE_FSB,             /* FMOD Sample Bank. */
        FMOD_SOUND_TYPE_IT,              /* Impulse Tracker. */
        FMOD_SOUND_TYPE_MIDI,            /* MIDI. */
        FMOD_SOUND_TYPE_MOD,             /* Protracker / Fasttracker MOD. */
        FMOD_SOUND_TYPE_MPEG,            /* MP2/MP3 MPEG. */
        FMOD_SOUND_TYPE_OGGVORBIS,       /* Ogg vorbis. */
        FMOD_SOUND_TYPE_PLAYLIST,        /* Information only from ASX/PLS/M3U/WAX playlists */
        FMOD_SOUND_TYPE_RAW,             /* Raw PCM data. */
        FMOD_SOUND_TYPE_S3M,             /* ScreamTracker 3. */
        FMOD_SOUND_TYPE_USER,            /* User created sound. */
        FMOD_SOUND_TYPE_WAV,             /* Microsoft WAV. */
        FMOD_SOUND_TYPE_XM,              /* FastTracker 2 XM. */
        FMOD_SOUND_TYPE_XMA,             /* Xbox360 XMA */
        FMOD_SOUND_TYPE_AUDIOQUEUE,      /* iPhone hardware decoder, supports AAC, ALAC and MP3. */
        FMOD_SOUND_TYPE_AT9,             /* PS4 / PSVita ATRAC 9 format */
        FMOD_SOUND_TYPE_VORBIS,          /* Vorbis */
        FMOD_SOUND_TYPE_MEDIA_FOUNDATION,/* Windows Store Application built in system codecs */
        FMOD_SOUND_TYPE_MEDIACODEC,      /* Android MediaCodec */
        FMOD_SOUND_TYPE_FADPCM,          /* FMOD Adaptive Differential Pulse Code Modulation */
        FMOD_SOUND_TYPE_MAX,             /* Maximum number of sound types supported. */
    }
#pragma warning restore 1591
}
