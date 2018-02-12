using System;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    [Flags]
    public enum FMOD_CHANNELCONTROL_TYPE : uint
    {
        FMOD_CHANNELCONTROL_CHANNEL,
        FMOD_CHANNELCONTROL_CHANNELGROUP,
    }
#pragma warning restore 1591
}
