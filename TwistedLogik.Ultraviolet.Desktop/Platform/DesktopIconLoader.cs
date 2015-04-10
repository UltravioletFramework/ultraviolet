using System;
using System.Reflection;
using TwistedLogik.Ultraviolet.Desktop.Graphics;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Platform;
using System.IO;

namespace TwistedLogik.Ultraviolet.Desktop.Platform
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
            var driveInfo = new DriveInfo(assemblyLocation);
            if (driveInfo.DriveType == DriveType.Network)
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
                        return Surface2D.Create(source);
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
