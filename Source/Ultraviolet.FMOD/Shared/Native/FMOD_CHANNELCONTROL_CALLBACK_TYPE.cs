using System;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    [Flags]
    public enum FMOD_CHANNELCONTROL_CALLBACK_TYPE : uint
    {
        FMOD_CHANNELCONTROL_CALLBACK_END,                  /* Called when a sound ends. */
        FMOD_CHANNELCONTROL_CALLBACK_VIRTUALVOICE,         /* Called when a voice is swapped out or swapped in. */
        FMOD_CHANNELCONTROL_CALLBACK_SYNCPOINT,            /* Called when a syncpoint is encountered.  Can be from wav file markers. */
        FMOD_CHANNELCONTROL_CALLBACK_OCCLUSION,            /* Called when the channel has its geometry occlusion value calculated.  Can be used to clamp or change the value. */

        FMOD_CHANNELCONTROL_CALLBACK_MAX,                  /* Maximum number of callback types supported. */
    }
#pragma warning restore 1591
}
