using System;
using System.IO;
using System.Reflection;
using TwistedLogik.Ultraviolet.Desktop.Graphics;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Platform;
using MonoMac.AppKit;
using MonoMac.Foundation;

namespace TwistedLogik.Ultraviolet.OSX
{
	public sealed class OSXIconLoader : IconLoader
	{
		/// <inheritdoc/>
		public override Surface2D LoadIcon()
		{
			var bundle = NSBundle.MainBundle;
			if (bundle == null)
				return null;

			var icon = default(NSImage);
			try
			{
				try
				{
					if (String.Equals(Path.GetExtension(bundle.BundlePath), ".app", StringComparison.OrdinalIgnoreCase))
					{
						icon = NSWorkspace.SharedWorkspace.IconForFile(bundle.BundlePath);
						if (icon == null)
						{
							throw new InvalidOperationException();
						}
					} 
					else
					{
						using (var stream = typeof(UltravioletContext).Assembly.GetManifestResourceStream("TwistedLogik.Ultraviolet.uv.ico"))
						{
							icon = NSImage.FromStream(stream);
						}
					}
				} 
				catch (FileNotFoundException)
				{
					return null;
				}

				using (var source = new OSXSurfaceSource(icon))
				{
					return Surface2D.Create(source);
				}
			} 
			finally
			{
				if (icon != null)
				{
					icon.Dispose();
				}
			}
		}
	}
}

