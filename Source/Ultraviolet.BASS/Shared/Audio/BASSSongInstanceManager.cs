using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Ultraviolet.BASS.Native;
using Ultraviolet.Platform;
using static Ultraviolet.BASS.Native.BASSNative;

namespace Ultraviolet.BASS.Audio
{
    partial class BASSSong
    {
        /// <summary>
        /// Manages stream instances for the <see cref="BASSSong"/> class.
        /// </summary>
        private class BASSSongInstanceManager : IDisposable
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="BASSSongInstanceManager"/> class.
            /// </summary>
            /// <param name="file">The name of the file that contains the song data.</param>
            public BASSSongInstanceManager(String file)
            {
                this.file = file;
                this.fnClose = StreamClose;
                this.fnLength = StreamLength;
                this.fnRead = StreamRead;
                this.fnSeek = StreamSeek;
            }

            /// <summary>
            /// Releases resources associated with the object.
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// Creates a BASS stream that represents the song.
            /// </summary>
            /// <param name="flags">The flags to apply to the stream that is created.</param>
            /// <returns>The handle to the BASS stream that was created.</returns>
            public UInt32 CreateInstance(UInt32 flags)
            {
                var fileSystemService = FileSystemService.Create();

                var instance = fileSystemService.OpenRead(file);
                var instanceID = this.nextInstanceID++;
                instances.Add(instanceID, instance);

                var stream = 0u;
                try
                {
                    var procs = new BASS_FILEPROCS(fnClose, fnLength, fnRead, fnSeek);

                    unsafe
                    {
                        stream = BASS_StreamCreateFileUser(1, flags, &procs, new IntPtr((int)instanceID));
                        if (!BASSUtil.IsValidHandle(stream))
                            throw new BASSException();
                    }
                }
                catch
                {
                    instance.Dispose();
                    instances.Remove(instanceID);
                    throw;
                }

                return stream;
            }

            /// <summary>
            /// Releases resources associated with the object.
            /// </summary>
            /// <param name="disposing"><see langword="true"/> if the object is being disposed; <see langword="false"/> if the object is being finalized.</param>
            protected virtual void Dispose(Boolean disposing)
            {
                if (disposing)
                {
                    foreach (var kvp in instances)
                    {
                        kvp.Value.Dispose();
                    }
                }
                instances.Clear();
            }

            /// <summary>
            /// Closes an instance stream.
            /// </summary>
            private void StreamClose(IntPtr user)
            {
                var instanceID = (UInt32)user.ToInt32();
                var instance = instances[instanceID];

                instances.Remove(instanceID);

                instance.Dispose();
            }

            /// <summary>
            /// Retrieves the length of an instance stream.
            /// </summary>
            private UInt64 StreamLength(IntPtr user)
            {
                var instanceID = (UInt32)user.ToInt32();
                var instance = instances[instanceID];

                return (UInt64)instance.Length;
            }

            /// <summary>
            /// Reads from an instance stream.
            /// </summary>
            private UInt32 StreamRead(IntPtr buffer, UInt32 length, IntPtr user)
            {
                var instanceID = (UInt32)user.ToInt32();
                var instance = instances[instanceID];

                var totalRead = 0u;
                var remaining = length;

                unsafe
                {
                    var output = (Byte*)buffer;

                    while (remaining > 0)
                    {
                        var requested = (Int32)Math.Min(remaining, this.copyBuffer.Length);
                        var read = instance.Read(this.copyBuffer, 0, requested);

                        if (read > 0)
                        {
                            Marshal.Copy(this.copyBuffer, 0, (IntPtr)output, read);
                            output += read;

                            totalRead += (UInt32)read;
                            remaining -= (UInt32)read;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return totalRead;
            }

            /// <summary>
            /// Seeks an instance stream.
            /// </summary>
            private Boolean StreamSeek(UInt64 offset, IntPtr user)
            {
                var instanceID = (UInt32)user.ToInt32();
                var instance = instances[instanceID];

                instance.Seek((Int64)offset, SeekOrigin.Begin);

                return true;
            }

            // A buffer used for transferring data from an instance stream to BASS' internal buffer.
            private readonly Byte[] copyBuffer = new Byte[1024];

            // File proc delegates used by BASS to access the instance streams.
            private readonly FileCloseProc fnClose;
            private readonly FileLenProc fnLength;
            private readonly FileReadProc fnRead;
            private readonly FileSeekProc fnSeek;

            // State values.
            private readonly String file;
            private UInt32 nextInstanceID;
            private readonly Dictionary<UInt32, Stream> instances =
                new Dictionary<UInt32, Stream>();
        }
    }
}