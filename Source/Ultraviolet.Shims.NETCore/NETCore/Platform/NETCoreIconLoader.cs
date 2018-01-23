using System;
using System.Reflection;
using Ultraviolet.Graphics;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.NETCore.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="IconLoader"/> class for the .NET Standard 2.0 platform.
    /// </summary>
    public sealed class NETCoreIconLoader : IconLoader
    {
        /// <inheritdoc/>
        public override Surface2D LoadIcon()
        {
            var assembly = Assembly.GetEntryAssembly();
            var assemblyLocation = (assembly == null) ? typeof(UltravioletContext).Assembly.Location : assembly.Location;

            /* HACK: Trying to load an icon from a network path throws an exception, which is a problem
             * given the way the test servers are currently configured. So just skip loading it. */
            var uri = new Uri(assemblyLocation);
            if (uri.IsUnc)
                return null;
            
            var iconStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.icon.ico");
            if (iconStream == null)
            {
                assembly = typeof(NETCoreIconLoader).Assembly;
                iconStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.icon.ico");
            }

            if (iconStream != null)
            {
                using (var source = SurfaceSource.Create(iconStream))
                {
                    return Surface2D.Create(source);
                }
            }

            return null;
        }
    }
}
