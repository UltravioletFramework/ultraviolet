using System;
using System.Reflection;
using Ultraviolet.Graphics;
using Ultraviolet.Platform;
using Ultraviolet.Shims.Desktop.Graphics;

namespace Ultraviolet.Shims.Desktop.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="IconLoader"/> class for desktop platforms.
    /// </summary>
    public sealed class DesktopIconLoader : IconLoader
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

            var icon = System.Drawing.Icon.ExtractAssociatedIcon(assemblyLocation);
            if (icon == null)
            {
                throw new InvalidOperationException();
            }

            try
            {
                using (var iconbmp = icon.ToBitmap())
                {
                    using (var source = new DesktopSurfaceSource(iconbmp))
                    {
                        return Surface2D.Create(source, SurfaceOptions.SrgbColor);
                    }
                }
            }
            finally
            {
                icon.Dispose();
            }
        }
    }
}
