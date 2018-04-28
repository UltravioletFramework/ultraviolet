using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Ultraviolet.Core.Native
{
    /// <summary>
    /// Represents a native shared library opened by the operating system.
    /// This type can be used to load native function pointers by name.
    /// </summary>
    public class NativeLibrary : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NativeLibrary"/> class.
        /// </summary>
        /// <param name="name">The name of the library to load.</param>
        public NativeLibrary(String name)
            : this(name, platformDefaultLoader, PathResolver.Default)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeLibrary"/> class.
        /// </summary>
        /// <param name="names">An ordered collection of library names. Each name is tried in 
        /// turn until the library is successfully loaded.</param>
        public NativeLibrary(IEnumerable<String> names)
            : this(names, platformDefaultLoader, PathResolver.Default)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeLibrary"/> class.
        /// </summary>
        /// <param name="name">The name of the library to load.</param>
        /// <param name="loader">The <see cref="LibraryLoader"/> which is used to load the library
        /// and retrieve function pointers.</param>
        public NativeLibrary(String name, LibraryLoader loader)
            : this(name, loader, PathResolver.Default)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeLibrary"/> class.
        /// </summary>
        /// <param name="names">An ordered collection of library names. Each name is tried in 
        /// turn until the library is successfully loaded.</param>
        /// <param name="loader">The <see cref="LibraryLoader"/> which is used to load the library
        /// and retrieve function pointers.</param>
        public NativeLibrary(IEnumerable<String> names, LibraryLoader loader)
            : this(names, loader, PathResolver.Default)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeLibrary"/> class.
        /// </summary>
        /// <param name="name">The name of the library to load.</param>
        /// <param name="loader">The <see cref="LibraryLoader"/> which is used to load the library
        /// and retrieve function pointers.</param>
        /// <param name="pathResolver">A <see cref="PathResolver"/> instance which determines the algorithm
        /// for resolving library paths from library names.</param>
        public NativeLibrary(String name, LibraryLoader loader, PathResolver pathResolver)
        {
            this.loader = loader;
            this.Handle = loader.LoadNativeLibrary(name, pathResolver);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeLibrary"/> class.
        /// </summary>
        /// <param name="names">An ordered collection of library names. Each name is tried in 
        /// turn until the library is successfully loaded.</param>
        /// <param name="loader">The <see cref="LibraryLoader"/> which is used to load the library
        /// and retrieve function pointers.</param>
        /// <param name="pathResolver">A <see cref="PathResolver"/> instance which determines the algorithm
        /// for resolving library paths from library names.</param>
        public NativeLibrary(IEnumerable<String> names, LibraryLoader loader, PathResolver pathResolver)
        {
            this.loader = loader;
            this.Handle = loader.LoadNativeLibrary(names, pathResolver);
        }

        /// <summary>
        /// Loads a function whose signature matches the given delegate's type signature.
        /// </summary>
        /// <typeparam name="T">The type of delegate to return.</typeparam>
        /// <param name="name">The name of the native function to load.</param>
        /// <returns>A delegate wrapping the native function with the specified name.</returns>
        public T LoadFunction<T>(String name)
        {
            var ptr = loader.LoadFunctionPointer(Handle, name);
            if (ptr == IntPtr.Zero)
                throw new InvalidOperationException(CoreStrings.CouldNotLoadFunctionFromName.Format(name));

            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        /// <summary>
        /// Frees the native library. Function pointers retrieved from this library will be void.
        /// </summary>
        public void Dispose()
        {
            loader.FreeNativeLibrary(Handle);
        }

        /// <summary>
        /// Gets the operating system handle which represents this library.
        /// </summary>
        public IntPtr Handle { get; }

        // The loader instance. used to load this library.
        private static readonly LibraryLoader platformDefaultLoader = LibraryLoader.GetPlatformDefaultLoader();
        private readonly LibraryLoader loader;
    }
}
