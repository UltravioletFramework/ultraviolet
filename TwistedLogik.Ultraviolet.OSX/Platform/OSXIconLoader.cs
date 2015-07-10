using System;
using System.Reflection;
using TwistedLogik.Ultraviolet.Desktop.Graphics;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Platform;
using MonoMac.AppKit;

namespace TwistedLogik.Ultraviolet.OSX
{
	public sealed class OSXIconLoader : IconLoader
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

			var icon = NSWorkspace.SharedWorkspace.IconForFile(assemblyLocation);
			if (icon == null)
			{
				throw new InvalidOperationException();
			}

			try
			{
				using (var source = new OSXSurfaceSource(icon))
				{
					return Surface2D.Create(source);
				}
			}
			finally
			{
				icon.Dispose();
			}
		}
	}
}

