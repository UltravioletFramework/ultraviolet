using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ultraviolet.Core.Native
{
    /// <summary>
    /// Contains methods for loading shared libraries on Unix-based platforms.
    /// </summary>
    internal sealed class UnixLibraryLoaderLibdl2 : UnixLibraryLoader
    {
        /// <summary>
        /// Contains native methods used by the <see cref="UnixLibraryLoaderLibdl2"/> class.
        /// </summary>
        private static class Native
        {
            public const Int32 RTLD_NOW = 0x002;

            [DllImport("libdl.so.2")]
            public static extern IntPtr dlopen(String fileName, Int32 flags);

            [DllImport("libdl.so.2")]
            public static extern IntPtr dlsym(IntPtr handle, String name);

            [DllImport("libdl.so.2")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern Boolean dlclose(IntPtr handle);
        }

        /// <inheritdoc/>
        protected override IntPtr CoreLoadNativeLibrary(String libraryName) =>
            Native.dlopen(libraryName, Native.RTLD_NOW);

        /// <inheritdoc/>
        protected override IntPtr CoreLoadNativeFunctionPointer(IntPtr handle, String functionName) => 
            Native.dlsym(handle, functionName);

        /// <inheritdoc/>
        protected override void CoreFreeNativeLibrary(IntPtr handle) => 
            Native.dlclose(handle);
    }
}
