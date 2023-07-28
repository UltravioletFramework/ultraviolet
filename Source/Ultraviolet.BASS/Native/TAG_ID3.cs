using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct TAG_ID3
    {
        public fixed byte id[3];
        public fixed byte title[30];
        public fixed byte artist[30];
        public fixed byte album[30];
        public fixed byte year[4];
        public fixed byte comment[30];
        public MARSHALLED_TAG_ID3 ToMarshalledStruct() { return new MARSHALLED_TAG_ID3(this); }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MARSHALLED_TAG_ID3
    {
        public MARSHALLED_TAG_ID3(TAG_ID3 id3)
        {
            String MarshalString(byte* ptr, int len)
            {
                var str = Encoding.ASCII.GetString(ptr, len);
                var nul = str.IndexOf('\0');
                return nul < 0 ? str : str.Substring(0, nul);
            }
            this.id = MarshalString(id3.id, 3);
            this.title = MarshalString(id3.title, 30);
            this.artist = MarshalString(id3.artist, 30);
            this.album = MarshalString(id3.album, 30);
            this.year = MarshalString(id3.year, 4);
            this.comment = MarshalString(id3.comment, 30);
        }
        public String id;
        public String title;
        public String artist;
        public String album;
        public String year;
        public String comment;
    }
#pragma warning restore 1591
}
