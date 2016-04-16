using System;
using System.IO;
using MonoMac;
using MonoMac.AppKit;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OSX
{
	/// <summary>
	/// Represents an implementation of the <see cref="SurfaceSource"/> class for Mac OS X.
	/// </summary>
	public sealed unsafe class OSXSurfaceSource : SurfaceSource
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OSXSurfaceSource"/> class from the specified stream.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> that contains the surface data.</param>
		public OSXSurfaceSource(Stream stream)
		{
			Contract.Require(stream, "stream");

			var data = new Byte[stream.Length];
			stream.Read(data, 0, data.Length);

			using (var mstream = new MemoryStream(data))
			{
				this.image = NSImage.FromStream(mstream);
				this.imageRep = new NSBitmapImageRep(image.CGImage);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OSXSurfaceSource"/> class from the specified image.
		/// </summary>
		/// <param name="stream">The <see cref="NSImage"/> that contains the surface data.</param>
		public OSXSurfaceSource(NSImage image)
		{
			Contract.Require(image, "image");

			this.image = image;
			this.imageRep = new NSBitmapImageRep(image.CGImage);
		}

		/// <inheritdoc/>
		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <inheritdoc/>
		public override Color this[int x, int y]
		{
			get
			{
				var pixel = ((byte*)imageRep.BitmapData) + (imageRep.BytesPerRow * y) + (x * sizeof(UInt32));
				return Color.FromArgb(*(uint*)pixel);
			}
		}

		/// <inheritdoc/>
		public override IntPtr Data
		{
			get
			{
				return this.imageRep.BitmapData;
			}
		}

		/// <inheritdoc/>
		public override Int32 Stride
		{
			get
			{
				return this.imageRep.BytesPerRow;
			}
		}

		/// <inheritdoc/>
		public override Int32 Width
		{
			get
			{
				return this.imageRep.PixelsWide;
			}
		}

		/// <inheritdoc/>
		public override Int32 Height
		{
			get
			{
				return this.imageRep.PixelsHigh;
			}
		}

		/// <inheritdoc/>
		public override SurfaceSourceDataFormat DataFormat
		{
			get
			{
				return SurfaceSourceDataFormat.BGRA;
			}
		}

		/// <summary>
		/// Releases resources associated with the object.
		/// </summary>
		/// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
		private void Dispose(Boolean disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				this.imageRep.Dispose();
				this.image.Dispose();
			}

			disposed = true;
		}

		// State values.
		private readonly NSImage image;
		private readonly NSBitmapImageRep imageRep;
		private Boolean disposed;
	}
}

