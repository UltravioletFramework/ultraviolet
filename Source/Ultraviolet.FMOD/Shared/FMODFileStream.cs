using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Ultraviolet.Core;
using Ultraviolet.FMOD.Native;
using static Ultraviolet.FMOD.Native.FMOD_RESULT;

namespace Ultraviolet.FMOD
{
    /// <summary>
    /// Represents a file which is being loaded by native FMOD code.
    /// </summary>
    public sealed unsafe class FMODFileStream : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FMODFileStream"/> class.
        /// </summary>
        /// <param name="stream">The file stream from which to read data.</param>
        public FMODFileStream(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            this.stream = stream;
        }

        /// <summary>
        /// Releases resources associated with this object.
        /// </summary>
        public void Dispose()
        {
            stream.Dispose();
        }

        /// <summary>
        /// Reads data from the stream into the specified native buffer.
        /// </summary>
        public FMOD_RESULT Read(void* buffer, UInt32 sizebytes, UInt32* bytesread, void* userdata)
        {
            var temp = readbuffer.Value;
            var read = stream.Read(temp, 0, temp.Length);
            Marshal.Copy(temp, 0, (IntPtr)buffer, read);

            *bytesread = (UInt32)read;

            return FMOD_OK;
        }

        /// <summary>
        /// Seeks to the specified position within the stream.
        /// </summary>
        public FMOD_RESULT Seek(UInt32 pos, void* userdata)
        {
            stream.Seek((Int32)pos, SeekOrigin.Begin);

            return FMOD_OK;
        }
        
        // State values.
        private readonly Stream stream;
        private readonly ThreadLocal<Byte[]> readbuffer = new ThreadLocal<Byte[]>(() => new Byte[2048]);
    }
}
