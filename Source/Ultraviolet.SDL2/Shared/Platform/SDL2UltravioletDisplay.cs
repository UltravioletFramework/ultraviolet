using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Messages;
using Ultraviolet.Platform;
using Ultraviolet.SDL2.Native;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2.Platform
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="IUltravioletDisplay"/> interface.
    /// </summary>
    public sealed unsafe class SDL2UltravioletDisplay : IUltravioletDisplay
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletDisplay class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="displayIndex">The SDL2 display index that this object represents.</param>
        public SDL2UltravioletDisplay(UltravioletContext uv, Int32 displayIndex)
        {
            Contract.Require(uv, nameof(uv));

            this.uv = uv;

            this.displayIndex = displayIndex;
            this.displayModes = Enumerable.Range(0, SDL_GetNumDisplayModes(displayIndex))
                .Select(modeIndex => CreateDisplayModeFromSDL(displayIndex, modeIndex))
                .ToList();

            this.name = SDL_GetDisplayName(displayIndex);

            SDL_DisplayMode sdlDesktopDisplayMode;
            if (SDL_GetDesktopDisplayMode(displayIndex, &sdlDesktopDisplayMode) < 0)
                throw new SDL2Exception();

            this.desktopDisplayMode = CreateDisplayModeFromSDL(sdlDesktopDisplayMode);

            this.screenRotationService = ScreenRotationService.Create(this);
            this.screenDensityService = ScreenDensityService.Create(this);
        }

        /// <inheritdoc/>
        public IEnumerable<DisplayMode> GetSupportedDisplayModes()
        {
            return displayModes;
        }

        /// <inheritdoc/>
        public Double InchesToPixels(Double inches)
        {
            return DipsToPixels(96.0 * inches);
        }

        /// <inheritdoc/>
        public Double PixelsToInches(Double pixels)
        {
            return PixelsToDips(pixels) / 96.0;
        }

        /// <inheritdoc/>
        public Double DipsToPixels(Double dips)
        {
            if (Double.IsPositiveInfinity(dips))
            {
                return Int32.MaxValue;
            }
            if (Double.IsNegativeInfinity(dips))
            {
                return Int32.MinValue;
            }
            return dips * DensityScale;
        }

        /// <inheritdoc/>
        public Double PixelsToDips(Double pixels)
        {
            return pixels / DensityScale;
        }

        /// <inheritdoc/>
        public Double InchesToDips(Double inches)
        {
            return 96.0 * inches;
        }

        /// <inheritdoc/>
        public Double DipsToInches(Double dips)
        {
            return dips / 96.0;
        }

        /// <inheritdoc/>
        public Point2D InchesToPixels(Point2D inches)
        {
            var x = InchesToPixels(inches.X);
            var y = InchesToPixels(inches.Y);

            return new Point2D(x, y);
        }

        /// <inheritdoc/>
        public Point2D PixelsToInches(Point2D pixels)
        {
            var x = PixelsToInches(pixels.X);
            var y = PixelsToInches(pixels.Y);

            return new Point2D(x, y);
        }

        /// <inheritdoc/>
        public Point2D DipsToPixels(Point2D dips)
        {
            var x = DipsToPixels(dips.X);
            var y = DipsToPixels(dips.Y);

            return new Point2D(x, y);
        }

        /// <inheritdoc/>
        public Point2D PixelsToDips(Point2D pixels)
        {
            var x = PixelsToDips(pixels.X);
            var y = PixelsToDips(pixels.Y);

            return new Point2D(x, y);
        }

        /// <inheritdoc/>
        public Point2D InchesToDips(Point2D inches)
        {
            var x = InchesToDips(inches.X);
            var y = InchesToDips(inches.Y);

            return new Point2D(x, y);
        }

        /// <inheritdoc/>
        public Point2D DipsToInches(Point2D dips)
        {
            var x = DipsToInches(dips.X);
            var y = DipsToInches(dips.Y);

            return new Point2D(x, y);
        }

        /// <inheritdoc/>
        public Size2D InchesToPixels(Size2D inches)
        {
            var width = InchesToPixels(inches.Width);
            var height = InchesToPixels(inches.Height);

            return new Size2D(width, height);
        }

        /// <inheritdoc/>
        public Size2D PixelsToInches(Size2D pixels)
        {
            var width = PixelsToInches(pixels.Width);
            var height = PixelsToInches(pixels.Height);

            return new Size2D(width, height);
        }

        /// <inheritdoc/>
        public Size2D DipsToPixels(Size2D dips)
        {
            var width = DipsToPixels(dips.Width);
            var height = DipsToPixels(dips.Height);

            return new Size2D(width, height);
        }

        /// <inheritdoc/>
        public Size2D PixelsToDips(Size2D pixels)
        {
            var width = PixelsToDips(pixels.Width);
            var height = PixelsToDips(pixels.Height);

            return new Size2D(width, height);
        }

        /// <inheritdoc/>
        public Size2D InchesToDips(Size2D inches)
        {
            var width = InchesToDips(inches.Width);
            var height = InchesToDips(inches.Height);

            return new Size2D(width, height);
        }

        /// <inheritdoc/>
        public Size2D DipsToInches(Size2D dips)
        {
            var width = DipsToInches(dips.Width);
            var height = DipsToInches(dips.Height);

            return new Size2D(width, height);
        }

        /// <inheritdoc/>
        public RectangleD InchesToPixels(RectangleD inches)
        {
            var x = InchesToPixels(inches.X);
            var y = InchesToPixels(inches.Y);
            var width = InchesToPixels(inches.Width);
            var height = InchesToPixels(inches.Height);

            return new RectangleD(x, y, width, height);
        }

        /// <inheritdoc/>
        public RectangleD PixelsToInches(RectangleD pixels)
        {
            var x = PixelsToInches(pixels.X);
            var y = PixelsToInches(pixels.Y);
            var width = PixelsToInches(pixels.Width);
            var height = PixelsToInches(pixels.Height);

            return new RectangleD(x, y, width, height);
        }

        /// <inheritdoc/>
        public RectangleD DipsToPixels(RectangleD dips)
        {
            var x = DipsToPixels(dips.Left);
            var y = DipsToPixels(dips.Top);
            var width = DipsToPixels(dips.Width);
            var height = DipsToPixels(dips.Height);

            return new RectangleD(x, y, width, height);
        }

        /// <inheritdoc/>
        public RectangleD PixelsToDips(RectangleD pixels)
        {
            var x = PixelsToDips(pixels.X);
            var y = PixelsToDips(pixels.Y);
            var width = PixelsToDips(pixels.Width);
            var height = PixelsToDips(pixels.Height);

            return new RectangleD(x, y, width, height);
        }

        /// <inheritdoc/>
        public RectangleD InchesToDips(RectangleD inches)
        {
            var x = InchesToDips(inches.X);
            var y = InchesToDips(inches.Y);
            var width = InchesToDips(inches.Width);
            var height = InchesToDips(inches.Height);

            return new RectangleD(x, y, width, height);
        }

        /// <inheritdoc/>
        public RectangleD DipsToInches(RectangleD dips)
        {
            var x = DipsToInches(dips.X);
            var y = DipsToInches(dips.Y);
            var width = DipsToInches(dips.Width);
            var height = DipsToInches(dips.Height);

            return new RectangleD(x, y, width, height);
        }

        /// <inheritdoc/>
        public Vector2 InchesToPixels(Vector2 inches)
        {
            var x = (Single)InchesToPixels(inches.X);
            var y = (Single)InchesToPixels(inches.Y);

            return new Vector2(x, y);
        }

        /// <inheritdoc/>
        public Vector2 PixelsToInches(Vector2 pixels)
        {
            var x = (Single)PixelsToInches(pixels.X);
            var y = (Single)PixelsToInches(pixels.Y);

            return new Vector2(x, y);
        }

        /// <inheritdoc/>
        public Vector2 DipsToPixels(Vector2 dips)
        {
            var x = (Single)DipsToPixels(dips.X);
            var y = (Single)DipsToPixels(dips.Y);

            return new Vector2(x, y);
        }

        /// <inheritdoc/>
        public Vector2 PixelsToDips(Vector2 pixels)
        {
            var x = (Single)PixelsToDips(pixels.X);
            var y = (Single)PixelsToDips(pixels.Y);

            return new Vector2(x, y);
        }

        /// <inheritdoc/>
        public Vector2 InchesToDips(Vector2 inches)
        {
            var x = (Single)InchesToDips(inches.X);
            var y = (Single)InchesToDips(inches.Y);

            return new Vector2(x, y);
        }

        /// <inheritdoc/>
        public Vector2 DipsToInches(Vector2 dips)
        {
            var x = (Single)DipsToInches(dips.X);
            var y = (Single)DipsToInches(dips.Y);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Updates the display's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {

        }

        /// <inheritdoc/>
        public Int32 Index
        {
            get { return displayIndex; }
        }

        /// <inheritdoc/>
        public String Name
        {
            get
            {
                return name;
            }
        }

        /// <inheritdoc/>
        public Rectangle Bounds
        {
            get
            {
                SDL_Rect bounds;
                SDL_GetDisplayBounds(displayIndex, &bounds);

                return new Rectangle(bounds.x, bounds.y, bounds.w, bounds.h);
            }
        }

        /// <inheritdoc/>
        public ScreenRotation Rotation
        {
            get
            {
                return screenRotationService.ScreenRotation;
            }
        }

        /// <inheritdoc/>
        public Single DeviceScale
        {
            get
            {
                return screenDensityService.DeviceScale;
            }
        }

        /// <inheritdoc/>
        public Single DensityScale
        {
            get
            {
                return screenDensityService.DensityScale;
            }
        }

        /// <inheritdoc/>
        public Single DpiX
        {
            get
            {
                return screenDensityService.DensityX;
            }
        }

        /// <inheritdoc/>
        public Single DpiY
        {
            get
            {
                return screenDensityService.DensityY;
            }
        }

        /// <inheritdoc/>
        public ScreenDensityBucket DensityBucket
        {
            get
            {
                return screenDensityService.DensityBucket;
            }
        }

        /// <inheritdoc/>
        public DisplayMode DesktopDisplayMode
        {
            get
            {
                return desktopDisplayMode;
            }
        }

        /// <summary>
        /// Instructs the display to re-query its density information.
        /// </summary>
        internal void RefreshDensityInformation()
        {
            var oldDensityBucket = screenDensityService.DensityBucket;
            var oldDensityScale = screenDensityService.DensityScale;
            var oldDensityX = screenDensityService.DensityX;
            var oldDensityY = screenDensityService.DensityY;
            var oldDeviceScale = screenDensityService.DeviceScale;

            if (screenDensityService.Refresh())
            {
                var messageData = uv.Messages.CreateMessageData<DisplayDensityChangedMessageData>();
                messageData.Display = this;
                uv.Messages.Publish(UltravioletMessages.DisplayDensityChanged, messageData);

                GC.Collect(2, GCCollectionMode.Forced);
            }
        }

        /// <summary>
        /// Creates an Ultraviolet DisplayMode object from the specified SDL2 display mode.
        /// </summary>
        private DisplayMode CreateDisplayModeFromSDL(SDL_DisplayMode mode)
        {
            Int32 bpp;
            UInt32 Rmask, Gmask, Bmask, Amask;
            SDL_PixelFormatEnumToMasks((uint)mode.format, &bpp, &Rmask, &Gmask, &Bmask, &Amask);

            return new DisplayMode(mode.w, mode.h, bpp, mode.refresh_rate, Index);
        }

        /// <summary>
        /// Creates an Ultraviolet DisplayMode object from the specified SDL2 display mode.
        /// </summary>
        private DisplayMode CreateDisplayModeFromSDL(Int32 displayIndex, Int32 modeIndex)
        {
            SDL_DisplayMode mode;
            SDL_GetDisplayMode(displayIndex, modeIndex, &mode);

            return CreateDisplayModeFromSDL(mode);
        }

        // SDL2 display info.
        private readonly UltravioletContext uv;
        private readonly Int32 displayIndex;
        private readonly String name;
        private readonly List<DisplayMode> displayModes;
        private readonly DisplayMode desktopDisplayMode;

        // Services.
        private readonly ScreenRotationService screenRotationService;
        private readonly ScreenDensityService screenDensityService;
    }
}
