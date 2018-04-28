using System;
using System.IO;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2
{
    /// <summary>
    /// Represents a wrapper around a <see cref="System.IO.Stream"/> which is compatible with SDL2's RWops API.
    /// </summary>
    public sealed unsafe partial class SDL2StreamWrapper : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2StreamWrapper"/> class.
        /// </summary>
        /// <param name="stream">The stream which will be wrapped by this object.</param>
        public SDL2StreamWrapper(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            this.stream = stream;
            this.gchandle = GCHandle.Alloc(this, GCHandleType.Weak);

            this.rwops = (SDL_RWops*)SDL_AllocRW();
            this.rwops->size = Marshal.GetFunctionPointerForDelegate(new RWops_size(RWops_size_callback));
            this.rwops->seek = Marshal.GetFunctionPointerForDelegate(new RWops_seek(RWops_seek_callback));
            this.rwops->read = Marshal.GetFunctionPointerForDelegate(new RWops_read(RWops_read_callback));
            this.rwops->write = Marshal.GetFunctionPointerForDelegate(new RWops_write(RWops_write_callback));
            this.rwops->close = Marshal.GetFunctionPointerForDelegate(new RWops_close(RWops_close_callback));
            this.rwops->data1 = GCHandle.ToIntPtr(gchandle);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Gets a <see cref="IntPtr"/> which represents the wrapper's underlying SDL_RWops structure.
        /// </summary>
        /// <returns>A pointer to the native RWops structure which this object represents.</returns>
        public IntPtr ToIntPtr()
        {
            return (IntPtr)rwops;
        }

        /// <summary>
        /// Implements the size() callback for this stream.
        /// </summary>
        [MonoPInvokeCallback(typeof(RWops_size))]
        private static Int64 RWops_size_callback(SDL_RWops* rwops)
        {
            var handle = GCHandle.FromIntPtr(rwops->data1);
            var target = (SDL2StreamWrapper)handle.Target;
            if (target == null || target.disposed)
                return -1;

            return target.stream.Length;
        }

        /// <summary>
        /// Implements the seek() callback for this stream.
        /// </summary>
        [MonoPInvokeCallback(typeof(RWops_seek))]
        private static Int64 RWops_seek_callback(SDL_RWops* rwops, Int64 pos, Int32 flags)
        {
            var handle = GCHandle.FromIntPtr(rwops->data1);
            var target = (SDL2StreamWrapper)handle.Target;
            if (target == null || target.disposed || !target.stream.CanSeek)
                return -1;

            switch (flags)
            {
                case RW_SEEK_SET:
                    target.stream.Seek(pos, SeekOrigin.Begin);
                    break;
                    
                case RW_SEEK_END:
                    target.stream.Seek(pos, SeekOrigin.End);
                    break;

                default:
                    target.stream.Seek(pos, SeekOrigin.Current);
                    break;
            }

            return target.stream.Position;
        }

        /// <summary>
        /// Implements the read() callback for this stream.
        /// </summary>
        [MonoPInvokeCallback(typeof(RWops_read))]
        private static UInt32 RWops_read_callback(SDL_RWops* rwops, IntPtr ptr, UInt32 size, UInt32 maxnum)
        {
            var handle = GCHandle.FromIntPtr(rwops->data1);
            var target = (SDL2StreamWrapper)handle.Target;
            if (target == null || target.disposed || !target.stream.CanRead)
                return 0;

            var objCount = 0u;
            var objBuffer = new Byte[size];

            var output = (Byte*)ptr.ToPointer();

            for (uint i = 0; i < maxnum; i++)
            {
                if (target.stream.Read(objBuffer, 0, objBuffer.Length) < objBuffer.Length)
                    break;

                Marshal.Copy(objBuffer, 0, (IntPtr)output, objBuffer.Length);
                output += size;

                objCount++;
            }

            return objCount;
        }

        /// <summary>
        /// Implements the write() callback for this stream.
        /// </summary>
        [MonoPInvokeCallback(typeof(RWops_write))]
        private static UInt32 RWops_write_callback(SDL_RWops* rwops, IntPtr ptr, UInt32 size, UInt32 num)
        {
            var handle = GCHandle.FromIntPtr(rwops->data1);
            var target = (SDL2StreamWrapper)handle.Target;
            if (target == null || target.disposed || !target.stream.CanWrite)
                return 0;

            var objCount = 0u;
            var objBuffer = new Byte[size];

            var input = (Byte*)ptr.ToPointer();

            for (uint i = 0; i < num; i++)
            {
                Marshal.Copy((IntPtr)input, objBuffer, 0, objBuffer.Length);
                input += size;

                target.stream.Write(objBuffer, 0, objBuffer.Length);

                objCount++;
            }

            return objCount;
        }

        /// <summary>
        /// Implements the close() callback for this stream.
        /// </summary>
        [MonoPInvokeCallback(typeof(RWops_close))]
        private static Int32 RWops_close_callback(SDL_RWops* rwops)
        {
            var handle = GCHandle.FromIntPtr(rwops->data1);
            var target = (SDL2StreamWrapper)handle.Target;
            if (target == null || target.disposed)
                return -1;

            target.Dispose();
            return 0;
        }

        /// <summary>
        /// Releases memory associated with the object.
        /// </summary>
        private void Dispose(Boolean disposing)
        {
            if (disposed)
                return;
            
            if (rwops != null)
            {
                SDL_FreeRW((IntPtr)rwops);
                rwops = null;
            }

            if (gchandle.IsAllocated)
                gchandle.Free();

            disposed = true;
        }

        // The stream which is wrapped by this object.
        private readonly Stream stream;

        // State values.
        private readonly GCHandle gchandle;
        private SDL_RWops* rwops;
        private Boolean disposed;

        // SDL2 constants.
        private const Int32 RW_SEEK_SET = 0;
        private const Int32 RW_SEEK_CUR = 1;
        private const Int32 RW_SEEK_END = 2;
    }
}
