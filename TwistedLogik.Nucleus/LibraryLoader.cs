using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace TwistedLogik.Nucleus
{
    /// <summary>
    /// Contains methods for dynamically loading native libraries based on the current operating system and the process' bitness.
    /// </summary>
    /// <remarks>
    /// <para>The <see cref="LibraryLoader"/> class is used to dynamically load native libraries prior to invoking their functions through
    /// Platform Invocation Services (P/Invoke). This gives us greater control over where the libraries are loaded from, allowing
    /// us to ensure that the appropriate version of the library is loaded for the current operating system and process bitness.</para>
    /// <para>When running under Mono, this functionality can instead be provided by Mono's DllMaps functionality. See
    /// the Mono documentation at http://www.mono-project.com/docs/advanced/pinvoke/dllmap/ for more information.</para>
    /// </remarks>
    [SecurityCritical]
    public static class LibraryLoader
    {
        /// <summary>
        /// Contains native methods used by <see cref="LibraryLoader"/>.
        /// </summary>
        private static class NativeMethods
        {
            [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);
        }

        /// <summary>
        /// Represents a method which can load native libraries.
        /// </summary>
        /// <param name="name">The name of the native library to load.</param>
        private delegate void LibraryLoaderFunction(String name);

        /// <summary>
        /// Initializes the <see cref="LibraryLoader"/> type.
        /// </summary>
        static LibraryLoader()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    loader = Load_Win32NT;
                    break;
                case PlatformID.Unix:
                    loader = Load_Unix;
                    break;
                default:
                    loader = Load_NotSupported;
                    break;
            }
        }

        /// <summary>
        /// Loads the specified native library.
        /// </summary>
        /// <param name="name">The name of the native library to load.</param>
        public static void Load(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            loader(name);
        }

        /// <summary>
        /// Loads native libraries for the Win32NT operating system.
        /// </summary>
        /// <param name="name">The name of the native library to load.</param>
        private static void Load_Win32NT(String name)
        {
            var file = Path.ChangeExtension(name, ".dll");
            var path = Path.GetFullPath(Path.Combine(Environment.Is64BitProcess ? "x64" : "x86", "win32nt", file));
            var handle = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, 0);
            if (handle == IntPtr.Zero)
            {
                throw new DllNotFoundException(String.Format("{0} ({1})", name, path));
            }
        }

        /// <summary>
        /// Loads native libraries for the Unix operating system.
        /// </summary>
        /// <param name="name">The name of the native library to load.</param>
        private static void Load_Unix(String name)
		{
			// Do nothing; let Mono's library mapper handle it.
		}

        /// <summary>
        /// Throws an exception indicating that the current operating system is not supported.
        /// </summary>
        private static void Load_NotSupported(String name)
        {
            throw new NotSupportedException();
        }

        // The platform-specific function used to load libraries.
        private static readonly LibraryLoaderFunction loader;
    }
}
