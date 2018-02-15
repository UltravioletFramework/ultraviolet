using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Ultraviolet.FMOD.Native;
using Ultraviolet.Platform;
using static Ultraviolet.FMOD.Native.FMOD_RESULT;

namespace Ultraviolet.FMOD
{
#pragma warning disable 1591
    /// <summary>
    /// Contains methods which implement the FMOD file system when using a file source.
    /// </summary>
    public static unsafe class FMODFileSystem
    {
        public static FMOD_RESULT UserOpen(String name, UInt32* filesize, void** handle, void* userdata)
        {
            var fss = FileSystemService.Create();
            if (fss.FileExists(name))
            {
                var iostream = default(Stream);
                try
                {
                    iostream = fss.OpenRead(name);
                }
                catch (FileNotFoundException) { return FMOD_ERR_FILE_NOTFOUND; }

                var fmodstream = new FMODFileStream(iostream);
                var handlenum = Interlocked.Increment(ref nexthandle);
                var handlemem = Marshal.AllocHGlobal(sizeof(Int64)); ;
                *(Int64*)handlemem = handlenum;

                *filesize = (UInt32)iostream.Length;
                *handle = (void*)handlemem;

                lockSlim.EnterWriteLock();
                try
                {
                    streams[handlenum] = fmodstream;
                }
                finally { lockSlim.ExitWriteLock(); }

                return FMOD_OK;
            }
            else return FMOD_ERR_FILE_NOTFOUND;
        }

        public static FMOD_RESULT UserClose(void* handle, void* userdata)
        {
            var stream = default(FMODFileStream);
            var handlenum = *(Int64*)handle;
            var handlemem = (IntPtr)handle;
            Marshal.FreeHGlobal(handlemem);

            lockSlim.EnterWriteLock();
            try
            {
                stream = streams[handlenum];
                streams.Remove(handlenum);
            }
            finally { lockSlim.ExitWriteLock(); }

            stream.Dispose();

            return FMOD_OK;
        }

        public static FMOD_RESULT UserRead(void* handle, void* buffer, UInt32 sizebytes, UInt32* bytesread, void* userdata)
        {
            var stream = default(FMODFileStream);
            var handlenum = *(Int64*)handle;

            lockSlim.EnterReadLock();
            try
            {
                stream = streams[handlenum];
            }
            finally { lockSlim.ExitReadLock(); }
            
            return stream.Read(buffer, sizebytes, bytesread, userdata);
        }

        public static FMOD_RESULT UserSeek(void* handle, UInt32 pos, void* userdata)
        {
            var stream = default(FMODFileStream);
            var handlenum = *(Int64*)handle;

            lockSlim.EnterReadLock();
            try
            {
                stream = streams[handlenum];
            }
            finally { lockSlim.ExitReadLock(); }

            return stream.Seek(pos, userdata);
        }

        private static readonly ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();
        private static readonly Dictionary<Int64, FMODFileStream> streams = new Dictionary<Int64, FMODFileStream>();
        private static Int64 nexthandle;
    }
#pragma warning restore 1591
}
