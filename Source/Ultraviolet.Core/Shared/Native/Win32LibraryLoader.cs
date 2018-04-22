using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ultraviolet.Core.Native
{
    /// <summary>
    /// Contains methods for loading shared libraries on the Windows platform.
    /// </summary>
    internal sealed class Win32LibraryLoader : LibraryLoader
    {
        /// <summary>
        /// Contains native methods used by the <see cref="Win32LibraryLoader"/> class.
        /// </summary>
        private static class Native
        {
            [DllImport("kernel32", SetLastError = true)]
            public static extern IntPtr LoadLibrary(String fileName);

            [DllImport("kernel32", SetLastError = true)]
            public static extern IntPtr GetProcAddress(IntPtr module, String procName);

            [DllImport("kernel32", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern Boolean FreeLibrary(IntPtr module);
        }

        /// <inheritdoc/>
        protected override IntPtr CoreLoadNativeLibrary(String libraryName)
        {
            var libraryDir = Path.GetDirectoryName(libraryName);

            var oldCurrentDir = Directory.GetCurrentDirectory();
            var newCurrentDir = String.IsNullOrEmpty(libraryDir) ? oldCurrentDir : Path.GetFullPath(libraryDir);

            if (Directory.Exists(newCurrentDir))
                Directory.SetCurrentDirectory(newCurrentDir);

            try
            {
                return Native.LoadLibrary(libraryName);
            }
            finally
            {
                Directory.SetCurrentDirectory(oldCurrentDir);
            }
        }

        /// <inheritdoc/>
        protected override IntPtr CoreLoadNativeFunctionPointer(IntPtr handle, String functionName) => 
            Native.GetProcAddress(handle, functionName);

        /// <inheritdoc/>
        protected override void CoreFreeNativeLibrary(IntPtr handle) => 
            Native.FreeLibrary(handle);
    }
}
