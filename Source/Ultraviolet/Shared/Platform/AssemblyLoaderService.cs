using System;
using System.Reflection;
using Ultraviolet.Core;

namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="AssemblyLoaderService"/> class.
    /// </summary>
    /// <returns>The instance of <see cref="AssemblyLoaderService"/> that was created.</returns>
    public delegate AssemblyLoaderService AssemblyLoaderServiceFactory();

    /// <summary>
    /// Represents a platform-specific service which loads assemblies.
    /// </summary>
    public abstract class AssemblyLoaderService
    {    
        /// <summary>
        /// Represents a default, platform-agnostic implementation of the <see cref="AssemblyLoaderService"/> class.
        /// </summary>
        private sealed class PlatformAgnosticAssemblyLoaderService : AssemblyLoaderService
        {
            /// <inheritdoc/>
            public override Assembly Load(String name)
            {
                switch (UltravioletPlatformInfo.CurrentPlatform)
                {
                    case UltravioletPlatform.Android:
                    case UltravioletPlatform.iOS:
                        return Assembly.Load(name);

                    default:
                        return Assembly.LoadFrom(name);
                }
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="FileSystemService"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="FileSystemService"/> that was created.</returns>
        public static AssemblyLoaderService Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            var factory = uv.TryGetFactoryMethod<AssemblyLoaderServiceFactory>();
            if (factory != null)
                return factory();

            return new PlatformAgnosticAssemblyLoaderService();
        }

        /// <summary>
        /// Loads the assembly with the specified name.
        /// </summary>
        /// <param name="name">The name of the assembly to load.</param>
        /// <returns>The loaded assembly.</returns>
        public abstract Assembly Load(String name);
    }
}
