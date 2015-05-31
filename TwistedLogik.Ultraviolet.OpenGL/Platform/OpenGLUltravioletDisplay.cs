using System;
using System.Collections.Generic;
using System.Linq;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.OpenGL.Platform
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the IUltravioletDisplay interface.
    /// </summary>
    public sealed unsafe class OpenGLUltravioletDisplay : IUltravioletDisplay
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletDisplay class.
        /// </summary>
        /// <param name="displayIndex">The SDL2 display index that this object represents.</param>
        public OpenGLUltravioletDisplay(Int32 displayIndex)
        {
            this.displayIndex = displayIndex;
            this.displayModes = Enumerable.Range(0, SDL.GetNumDisplayModes(displayIndex))
                .Select(modeIndex => CreateDisplayModeFromSDL(displayIndex, modeIndex))
                .ToList();

            this.screenRotationService = ScreenRotationService.Create(this);
            this.screenDensityService  = ScreenDensityService.Create(this);
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
            return pixels / (Double)DensityScale;
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
            var width  = InchesToPixels(inches.Width);
            var height = InchesToPixels(inches.Height);

            return new Size2D(width, height);
        }

        /// <inheritdoc/>
        public Size2D PixelsToInches(Size2D pixels)
        {
            var width  = PixelsToInches(pixels.Width);
            var height = PixelsToInches(pixels.Height);

            return new Size2D(width, height);
        }

        /// <inheritdoc/>
        public Size2D DipsToPixels(Size2D dips)
        {
            var width  = DipsToPixels(dips.Width);
            var height = DipsToPixels(dips.Height);

            return new Size2D(width, height);
        }

        /// <inheritdoc/>
        public Size2D PixelsToDips(Size2D pixels)
        {
            var width  = PixelsToDips(pixels.Width);
            var height = PixelsToDips(pixels.Height);

            return new Size2D(width, height);
        }

        /// <inheritdoc/>
        public Size2D InchesToDips(Size2D inches)
        {
            var width  = InchesToDips(inches.Width);
            var height = InchesToDips(inches.Height);

            return new Size2D(width, height);
        }

        /// <inheritdoc/>
        public Size2D DipsToInches(Size2D dips)
        {
            var width  = DipsToInches(dips.Width);
            var height = DipsToInches(dips.Height);

            return new Size2D(width, height);
        }

        /// <inheritdoc/>
        public RectangleD InchesToPixels(RectangleD inches)
        {
            var x      = InchesToPixels(inches.X);
            var y      = InchesToPixels(inches.Y);
            var width  = InchesToPixels(inches.Width);
            var height = InchesToPixels(inches.Height);

            return new RectangleD(x, y, width, height);
        }

        /// <inheritdoc/>
        public RectangleD PixelsToInches(RectangleD pixels)
        {
            var x      = PixelsToInches(pixels.X);
            var y      = PixelsToInches(pixels.Y);
            var width  = PixelsToInches(pixels.Width);
            var height = PixelsToInches(pixels.Height);

            return new RectangleD(x, y, width, height);
        }

        /// <inheritdoc/>
        public RectangleD DipsToPixels(RectangleD dips)
        {
            var x      = DipsToPixels(dips.Left);
            var y      = DipsToPixels(dips.Top);
            var width  = DipsToPixels(dips.Width);
            var height = DipsToPixels(dips.Height);

            return new RectangleD(x, y, width, height);
        }

        /// <inheritdoc/>
        public RectangleD PixelsToDips(RectangleD pixels)
        {
            var x      = PixelsToDips(pixels.X);
            var y      = PixelsToDips(pixels.Y);
            var width  = PixelsToDips(pixels.Width);
            var height = PixelsToDips(pixels.Height);

            return new RectangleD(x, y, width, height);
        }

        /// <inheritdoc/>
        public RectangleD InchesToDips(RectangleD inches)
        {
            var x      = InchesToDips(inches.X);
            var y      = InchesToDips(inches.Y);
            var width  = InchesToDips(inches.Width);
            var height = InchesToDips(inches.Height);

            return new RectangleD(x, y, width, height);
        }

        /// <inheritdoc/>
        public RectangleD DipsToInches(RectangleD dips)
        {
            var x      = DipsToInches(dips.X);
            var y      = DipsToInches(dips.Y);
            var width  = DipsToInches(dips.Width);
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

        /// <inheritdoc/>
        public Rectangle Bounds
        {
            get
            {
                SDL_Rect bounds;
                SDL.GetDisplayBounds(displayIndex, &bounds);

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

        /// <summary>
        /// Creates an Ultraviolet DisplayMode object from the specified SDL2 display mode.
        /// </summary>
        /// <param name="displayIndex">The display index.</param>
        /// <param name="modeIndex">The mode index.</param>
        /// <returns>The Ultraviolet DisplayMode object that was created.</returns>
        private static DisplayMode CreateDisplayModeFromSDL(Int32 displayIndex, Int32 modeIndex)
        {
            SDL_DisplayMode mode;
            SDL.GetDisplayMode(displayIndex, modeIndex, &mode);

            Int32 bpp;
            UInt32 Rmask, Gmask, Bmask, Amask;
            SDL.PixelFormatEnumToMasks((uint)mode.format, &bpp, &Rmask, &Gmask, &Bmask, &Amask);

            return new DisplayMode(mode.w, mode.h, bpp, mode.refresh_rate);
        }

        // SDL2 display info.
        private readonly Int32 displayIndex;
        private readonly List<DisplayMode> displayModes;

        // Services.
        private readonly ScreenRotationService screenRotationService;
        private readonly ScreenDensityService screenDensityService;
    }
}
