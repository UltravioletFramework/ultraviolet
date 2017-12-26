using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.SDL2
{
    public sealed unsafe partial class SDL2StreamWrapper
    {
        /// <summary>
        /// Represents the method which is called to retrieve the size of an SDL_RWops stream.
        /// </summary>
        private delegate Int64 RWops_size(SDL_RWops* rwops);

        /// <summary>
        /// Represents the method which is called to seek to a position within an SDL_RWops stream.
        /// </summary>
        private delegate Int64 RWops_seek(SDL_RWops* rwops, Int64 pos, Int32 flags);

        /// <summary>
        /// Represents the method which is called to seek to read data from an SDL_RWops stream.
        /// </summary>
        private delegate UInt32 RWops_read(SDL_RWops* rwops, IntPtr ptr, UInt32 size, UInt32 maxnum);

        /// <summary>
        /// Represents the method which is called to seek to write data to an SDL_RWops stream.
        /// </summary>
        private delegate UInt32 RWops_write(SDL_RWops* rwops, IntPtr ptr, UInt32 size, UInt32 num);

        /// <summary>
        /// Represents the method which is called to seek to close an SDL_RWops stream.
        /// </summary>
        private delegate Int32 RWops_close(SDL_RWops* rwops);

        /// <summary>
        /// Note that this is NOT a complete specification of the SDL_RWops structure, which is a big
        /// complicated union full of platform-specific data. We're only declaring what we need for our
        /// stream wrapper, and trusting that if we set <see cref="type"/> to SDL_RWOPS_UNKNOWN
        /// that SDL2 isn't going to try to access the memory that we're neglecting to allocate.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct SDL_RWops
        {
            public IntPtr size;
            public IntPtr seek;
            public IntPtr read;
            public IntPtr write;
            public IntPtr close;
            public UInt32 type;
            public IntPtr data1;
            public IntPtr data2;
        }
    }
}
