using System;
using TwistedLogik.Ultraviolet.Platform;
using MonoMac.AppKit;
using MonoMac.Foundation;

namespace TwistedLogik.Ultraviolet.OSX
{
	public sealed class OSXScreenDensityService : ScreenDensityService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OSXScreenDensityService"/> class.
		/// </summary>
		/// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve density information.</param>
		public OSXScreenDensityService(IUltravioletDisplay display)
			: base(display)
		{
			var screen  = NSScreen.Screens[display.Index];
			var density = ((NSValue)screen.DeviceDescription["NSDeviceResolution"]).SizeFValue;

			this.densityScale = screen.BackingScaleFactor;
			this.densityX     = density.Width;
			this.densityY     = density.Height;
		}

		/// <inheritdoc/>
		public override Single DensityScale
		{
			get { return densityScale; }
		}

		/// <inheritdoc/>
		public override Single DensityX
		{
			get { return densityX; }
		}

		/// <inheritdoc/>
		public override Single DensityY
		{
			get { return densityY; }
		}

		/// <inheritdoc/>
		public override ScreenDensityBucket DensityBucket
		{
			get
			{
				if (densityScale == 1.0f)
					return ScreenDensityBucket.Desktop;

				if (densityScale == 2.0f)
					return ScreenDensityBucket.Retina;

				if (densityScale == 3.0f)
					return ScreenDensityBucket.RetinaHD;

				return ScreenDensityBucket.ExtraExtraExtraHigh;
			}
		}

		// State values.
		private readonly Single densityScale;
		private readonly Single densityX;
		private readonly Single densityY;
	}
}

