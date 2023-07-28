using System;
using System.Collections.Generic;
using System.IO;

namespace Ultraviolet.Core.Native
{
    /// <summary>
    /// Contains methods for loading native libraries and function pointers.
    /// </summary>
    /// <remarks>This code is based on a prototype by Eric Mellinoe (https://github.com/mellinoe/nativelibraryloader/tree/master/NativeLibraryLoader).</remarks>
    public abstract class LibraryLoader
    {
        /// <summary>
        /// Creates a new instance of the <see cref="LibraryLoader"/> class which is
        /// appropriate for the current platform.
        /// </summary>
        /// <returns>The <see cref="LibraryLoader"/> instance which was created.</returns>
        public static LibraryLoader GetPlatformDefaultLoader()
        {
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Windows:
                    return new Win32LibraryLoader();

                case UltravioletPlatform.macOS:
                    return new UnixLibraryLoaderLibdl();

                case UltravioletPlatform.Linux:
                    return FindLibraryLoaderForLinux();

                default:
                    throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// Loads a native library by name and return an operating system handle which represents it.
        /// </summary>
        /// <param name="name">The name of the library to load.</param>
        /// <returns>An <see cref="IntPtr"/> which represents the native library's handle.</returns>
        public IntPtr LoadNativeLibrary(String name)
        {
            return LoadNativeLibrary(name, PathResolver.Default);
        }

        /// <summary>
        /// Loads a native library by name and return an operating system handle which represents it.
        /// </summary>
        /// <param name="names">An ordered collection of library names. Each name is tried in 
        /// turn until the library is successfully loaded.</param>
        /// <returns>An <see cref="IntPtr"/> which represents the native library's handle.</returns>
        public IntPtr LoadNativeLibrary(IEnumerable<String> names)
        {
            return LoadNativeLibrary(names, PathResolver.Default);
        }

        /// <summary>
        /// Loads a native library by name and return an operating system handle which represents it.
        /// </summary>
        /// <param name="name">The name of the library to load.</param>
        /// <param name="pathResolver">A <see cref="PathResolver"/> instance which specifies
        /// the algorithm for resolving library paths from names.</param>
        /// <returns>An <see cref="IntPtr"/> which represents the native library's handle.</returns>
        public IntPtr LoadNativeLibrary(String name, PathResolver pathResolver)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            var ret = LoadWithResolver(name, pathResolver);
            if (ret == IntPtr.Zero)
                throw new FileNotFoundException(CoreStrings.CouldNotLoadLibraryFromName.Format(name));

            return ret;
        }

        /// <summary>
        /// Loads a native library by name and return an operating system handle which represents it.
        /// </summary>
        /// <param name="names">An ordered collection of library names. Each name is tried in 
        /// turn until the library is successfully loaded.</param>
        /// <param name="pathResolver">A <see cref="PathResolver"/> instance which specifies
        /// the algorithm for resolving library paths from names.</param>
        /// <returns>An <see cref="IntPtr"/> which represents the native library's handle.</returns>
        public IntPtr LoadNativeLibrary(IEnumerable<String> names, PathResolver pathResolver)
        {
            Contract.Require(names, nameof(names));

            var ret = IntPtr.Zero;
            foreach (var name in names)
            {
                ret = LoadWithResolver(name, pathResolver);
                if (ret != IntPtr.Zero)
                    break;
            }

            if (ret == IntPtr.Zero)
                throw new FileNotFoundException(CoreStrings.CouldNotLoadLibraryFromNames.Format(String.Join(", ", names)));

            return ret;
        }

        /// <summary>
        /// Retrieves a pointer to the specified function within a native library.
        /// </summary>
        /// <param name="handle">The handle of the shared library from which to retrieve the function pointer.</param>
        /// <param name="functionName">The name of the function for which to retrieve a function pointer.</param>
        /// <returns>An <see cref="IntPtr"/> which represents the function, or <see cref="IntPtr.Zero"/> if 
        /// the function could not be retrieved.</returns>
        public IntPtr LoadFunctionPointer(IntPtr handle, String functionName)
        {
            Contract.RequireNotEmpty(functionName, nameof(functionName));

            return CoreLoadNativeFunctionPointer(handle, functionName);
        }

        /// <summary>
        /// Frees the native library represented by the specified operating system handle.
        /// </summary>
        /// <param name="handle">The handle of the shared library to free.</param>
        public void FreeNativeLibrary(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
                throw new ArgumentNullException(nameof(handle));

            CoreFreeNativeLibrary(handle);
        }
        
        /// <summary>
        /// When overridden in a derived class, loads a native library and returns an operating system handle for it.
        /// </summary>
        /// <param name="libraryName">The name of the library to load.</param>
        /// <returns>The operating system handle for the shared library, or <see cref="IntPtr.Zero"/> if the library cannot be loaded.</returns>
        protected abstract IntPtr CoreLoadNativeLibrary(String libraryName);

        /// <summary>
        /// When overridden in a derived class, loads a native function pointer from the specified library.
        /// </summary>
        /// <param name="handle">The handle of the native library from which to load the function pointer.</param>
        /// <param name="functionName">The name of the native function for which to load a function pointer.</param>
        /// <returns>The loaded function pointer, or <see cref="IntPtr.Zero"/> if the function pointer cannot be loaded.</returns>
        protected abstract IntPtr CoreLoadNativeFunctionPointer(IntPtr handle, String functionName);

        /// <summary>
        /// When overriden in a derived class, frees the native library represented by the specified operating system handle.
        /// </summary>
        /// <param name="handle">The handle of the shared library to free.</param>
        protected abstract void CoreFreeNativeLibrary(IntPtr handle);

        /// <summary>
        /// Determines which library loader instance to use for the current Linux platform.
        /// </summary>
        private static LibraryLoader FindLibraryLoaderForLinux()
        {
            return
                TestLibraryLoaderForLinux(new UnixLibraryLoaderLibdl2()) ??
                TestLibraryLoaderForLinux(new UnixLibraryLoaderLibdl()) ??
                TestLibraryLoaderForLinux(new UnixLibraryLoaderLibc());
        }

        /// <summary>
        /// Tests a library loader to determine whether it is valid for the current Linux platform.
        /// </summary>
        private static LibraryLoader TestLibraryLoaderForLinux(LibraryLoader loader)
        {
            try
            {
                loader.CoreLoadNativeLibrary(null);
            }
            catch (Exception e)
            {
                if (e is DllNotFoundException || e is EntryPointNotFoundException)
                    return null;
                else throw;
            }
            return loader;
        }

        /// <summary>
        /// Attempts to load a native library with the specified path resolver.
        /// </summary>
        private IntPtr LoadWithResolver(String name, PathResolver pathResolver)
        {
            var targets = pathResolver.EnumeratePossibleLibraryLoadTargets(name);
            foreach (var target in targets)
            {
                if (!Path.IsPathRooted(target) || File.Exists(target))
                {
                    var ret = CoreLoadNativeLibrary(target);
                    if (ret != IntPtr.Zero)
                        return ret;
                }
            }
            return IntPtr.Zero;
        }
    }
}
