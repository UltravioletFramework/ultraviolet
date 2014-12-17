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

        /// <summary>
        /// Gets the display device's supported display modes.
        /// </summary>
        /// <returns>A collection containing the display device's supported display modes.</returns>
        public IEnumerable<DisplayMode> GetSupportedDisplayModes()
        {
            return displayModes;
        }

        /// <summary>
        /// Gets the display's bounds.
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                SDL_Rect bounds;
                SDL.GetDisplayBounds(displayIndex, &bounds);

                return new Rectangle(bounds.x, bounds.y, bounds.w, bounds.h);
            }
        }

        /// <summary>
        /// Gets the display's rotation on devices which can be rotated.
        /// </summary>
        public ScreenRotation Rotation
        {
            get
            {
                return screenRotationService.ScreenRotation;
            }
        }

        /// <summary>
        /// Gets the scaling factor for device independent pixels.
        /// </summary>
        public Single DensityScale
        {
            get 
            { 
                return screenDensityService.DensityScale; 
            }
        }

        /// <summary>
        /// Gets the display's density in dots per inch along the horizontal axis.
        /// </summary>
        public Single DpiX
        {
            get
            {
                return screenDensityService.DensityX;
            }
        }

        /// <summary>
        /// Gets the display's density in dots per inch along the vertical axis.
        /// </summary>
        public Single DpiY
        {
            get
            {
                return screenDensityService.DensityY;
            }
        }

        /// <summary>
        /// Gets the display's density bucket.
        /// </summary>
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
