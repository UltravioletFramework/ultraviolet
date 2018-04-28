using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FMOD_TAG
    {
        public FMOD_TAGTYPE type;          /* [r] The type of this tag. */
        public FMOD_TAGDATATYPE datatype;  /* [r] The type of data that this tag contains */
        public IntPtr name;                /* [r] The name of this tag i.e. "TITLE", "ARTIST" etc. */
        public IntPtr data;                /* [r] Pointer to the tag data - its format is determined by the datatype member */
        public UInt32 datalen;             /* [r] Length of the data contained in this tag */
        private Int32 _updated;            /* [r] True if this tag has been updated since last being accessed with Sound::getTag */
        public Boolean updated { get { return _updated != 0; } set { _updated = value ? 1 : 0; } }
    }
#pragma warning restore 1591
}
