using System;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    [Flags]
    public enum FMOD_INITFLAGS : uint
    {
        FMOD_INIT_NORMAL                        = 0x00000000, /* Initialize normally */
        FMOD_INIT_STREAM_FROM_UPDATE            = 0x00000001, /* No stream thread is created internally.  Streams are driven from System::update.  Mainly used with non-realtime outputs. */
        FMOD_INIT_MIX_FROM_UPDATE               = 0x00000002, /* No mixer thread is created internally. Mixing is driven from System::update. Only applies to polling based output modes such as FMOD_OUTPUTTYPE_NOSOUND, FMOD_OUTPUTTYPE_WAVWRITER, FMOD_OUTPUTTYPE_DSOUND, FMOD_OUTPUTTYPE_WINMM,FMOD_OUTPUTTYPE_XAUDIO. */
        FMOD_INIT_3D_RIGHTHANDED                = 0x00000004, /* 3D calculations will be performed in right-handed coordinates. */
        FMOD_INIT_CHANNEL_LOWPASS               = 0x00000100, /* Enables usage of Channel::setLowPassGain,  Channel::set3DOcclusion, or automatic usage by the Geometry API.  All voices will add a software lowpass filter effect into the DSP chain which is idle unless one of the previous functions/features are used. */
        FMOD_INIT_CHANNEL_DISTANCEFILTER        = 0x00000200, /* All FMOD_3D based voices will add a software lowpass and highpass filter effect into the DSP chain which will act as a distance-automated bandpass filter. Use System::setAdvancedSettings to adjust the center frequency. */
        FMOD_INIT_PROFILE_ENABLE                = 0x00010000, /* Enable TCP/IP based host which allows FMOD Designer or FMOD Profiler to connect to it, and view memory, CPU and the DSP network graph in real-time. */
        FMOD_INIT_VOL0_BECOMES_VIRTUAL          = 0x00020000, /* Any sounds that are 0 volume will go virtual and not be processed except for having their positions updated virtually.  Use System::setAdvancedSettings to adjust what volume besides zero to switch to virtual at. */
        FMOD_INIT_GEOMETRY_USECLOSEST           = 0x00040000, /* With the geometry engine, only process the closest polygon rather than accumulating all polygons the sound to listener line intersects. */
        FMOD_INIT_PREFER_DOLBY_DOWNMIX          = 0x00080000, /* When using FMOD_SPEAKERMODE_5POINT1 with a stereo output device, use the Dolby Pro Logic II downmix algorithm instead of the SRS Circle Surround algorithm. */
        FMOD_INIT_THREAD_UNSAFE                 = 0x00100000, /* Disables thread safety for API calls. Only use this if FMOD low level is being called from a single thread, and if Studio API is not being used! */
        FMOD_INIT_PROFILE_METER_ALL             = 0x00200000, /* Slower, but adds level metering for every single DSP unit in the graph.  Use DSP::setMeteringEnabled to turn meters off individually. */
        FMOD_INIT_DISABLE_SRS_HIGHPASSFILTER    = 0x00400000, /* Using FMOD_SPEAKERMODE_5POINT1 with a stereo output device will enable the SRS Circle Surround downmixer. By default the SRS downmixer applies a high pass filter with a cutoff frequency of 80Hz. Use this flag to diable the high pass fitler, or use PREFER_DOLBY_DOWNMIX to use the Dolby Pro Logic II downmix algorithm instead. */
    }
#pragma warning restore 1591
}
