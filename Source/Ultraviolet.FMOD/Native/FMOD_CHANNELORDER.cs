namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    public enum FMOD_CHANNELORDER
    {
        FMOD_CHANNELORDER_DEFAULT,              /* Left, Right, Center, LFE, Surround Left, Surround Right, Back Left, Back Right (see FMOD_SPEAKER enumeration)   */
        FMOD_CHANNELORDER_WAVEFORMAT,           /* Left, Right, Center, LFE, Back Left, Back Right, Surround Left, Surround Right (as per Microsoft .wav WAVEFORMAT structure master order) */
        FMOD_CHANNELORDER_PROTOOLS,             /* Left, Center, Right, Surround Left, Surround Right, LFE */
        FMOD_CHANNELORDER_ALLMONO,              /* Mono, Mono, Mono, Mono, Mono, Mono, ... (each channel all the way up to FMOD_MAX_CHANNEL_WIDTH channels are treated as if they were mono) */
        FMOD_CHANNELORDER_ALLSTEREO,            /* Left, Right, Left, Right, Left, Right, ... (each pair of channels is treated as stereo all the way up to FMOD_MAX_CHANNEL_WIDTH channels) */
        FMOD_CHANNELORDER_ALSA,                 /* Left, Right, Surround Left, Surround Right, Center, LFE (as per Linux ALSA channel order) */

        FMOD_CHANNELORDER_MAX,                  /* Maximum number of channel orderings supported. */
    }
#pragma warning restore 1591
}
