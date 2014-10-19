
namespace TwistedLogik.Ultraviolet.BASS.Native
{
    internal enum BASSAttrib : uint
    {
        ATTRIB_FREQ             = 1,
        ATTRIB_VOL              = 2,
        ATTRIB_PAN              = 3,
        ATTRIB_EAXMIX           = 4,
        ATTRIB_NOBUFFER         = 5,
        ATTRIB_CPU              = 7,
        ATTRIB_SRC              = 8,
        ATTRIB_MUSIC_AMPLIFY    = 0x100,
        ATTRIB_MUSIC_PANSEP     = 0x101,
        ATTRIB_MUSIC_PSCALER    = 0x102,
        ATTRIB_MUSIC_BPM        = 0x103,
        ATTRIB_MUSIC_SPEED      = 0x104,
        ATTRIB_MUSIC_VOL_GLOBAL = 0x105,
        ATTRIB_MUSIC_VOL_CHAN   = 0x200,
        ATTRIB_MUSIC_VOL_INST   = 0x300,

        // BASS_FX
        ATTRIB_TEMPO            = 0x10000,
        ATTRIB_TEMPO_PITCH      = 0x10001,
        ATTRIB_TEMP_FREQ        = 0x10002,
    }
}
