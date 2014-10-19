
namespace TwistedLogik.Ultraviolet.BASS.Native
{
    internal enum BASSSync : uint
    {
        SYNC_POS			= 0,
        SYNC_END			= 2,
        SYNC_META			= 4,
        SYNC_SLIDE			= 5,
        SYNC_STALL			= 6,
        SYNC_DOWNLOAD		= 7,
        SYNC_FREE			= 8,
        SYNC_SETPOS		    = 11,
        SYNC_MUSICPOS		= 10,
        SYNC_MUSICINST		= 1,
        SYNC_MUSICFX		= 3,
        SYNC_OGG_CHANGE	    = 12,
        SYNC_MIXTIME		= 0x40000000,
        SYNC_ONETIME		= 0x80000000,
    }
}
