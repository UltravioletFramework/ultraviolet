namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    public enum FMOD_SPEAKERMODE : uint
    {
        FMOD_SPEAKERMODE_DEFAULT,          /* Default speaker mode for the chosen output mode which will resolve after System::init. */
        FMOD_SPEAKERMODE_RAW,              /* Assume there is no special mapping from a given channel to a speaker, channels map 1:1 in order. Use System::setSoftwareFormat to specify the speaker count. */
        FMOD_SPEAKERMODE_MONO,             /*  1 speaker setup (monaural). */
        FMOD_SPEAKERMODE_STEREO,           /*  2 speaker setup (stereo) front left, front right. */
        FMOD_SPEAKERMODE_QUAD,             /*  4 speaker setup (4.0)    front left, front right, surround left, surround right. */
        FMOD_SPEAKERMODE_SURROUND,         /*  5 speaker setup (5.0)    front left, front right, center, surround left, surround right. */
        FMOD_SPEAKERMODE_5POINT1,          /*  6 speaker setup (5.1)    front left, front right, center, low frequency, surround left, surround right. */
        FMOD_SPEAKERMODE_7POINT1,          /*  8 speaker setup (7.1)    front left, front right, center, low frequency, surround left, surround right, back left, back right. */
        FMOD_SPEAKERMODE_7POINT1POINT4,    /* 12 speaker setup (7.1.4)  front left, front right, center, low frequency, surround left, surround right, back left, back right, top front left, top front right, top back left, top back right. */

        FMOD_SPEAKERMODE_MAX,              /* Maximum number of speaker modes supported. */
    }
#pragma warning restore 1591
}
