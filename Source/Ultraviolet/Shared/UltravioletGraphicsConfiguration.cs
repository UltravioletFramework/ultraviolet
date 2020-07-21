using System;
using Ultraviolet.Graphics;

namespace Ultraviolet
{
    /// <summary>
    /// Represents the Ultraviolet Framework's graphics configuration settings.
    /// </summary>
    public abstract class UltravioletGraphicsConfiguration
    {
        /// <summary>
        /// Gets the name of the graphics API that this configuration represents.
        /// </summary>
        public abstract String GraphicsApiName { get; }

        /// <summary>
        /// Gets or sets the <see cref="RenderTargetUsage"/> value which is used by the back buffer.
        /// </summary>
        public RenderTargetUsage BackBufferRenderTargetUsage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SRGB encoding should be enabled for
        /// textures and render buffers if it is supported. After device initialization, check the value
        /// of the <see cref="GraphicsCapabilities.SrgbEncodingEnabled"/> property to determine whether SRGB 
        /// encoding was actually enabled -- not all devices support it.
        /// </summary>
        public Boolean SrgbBuffersEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value which determines whether instances of the <see cref="Surface2D"/>
        /// class are treated as SRGB encoded by default.
        /// </summary>
        public Boolean SrgbDefaultForSurface2D { get; set; }

        /// <summary>
        /// Gets or sets a value which determines whether instances of the <see cref="Surface3D"/>
        /// class are treated as SRGB encoded by default.
        /// </summary>
        public Boolean SrgbDefaultForSurface3D { get; set; }

        /// <summary>
        /// Gets or sets a value which determines whether instances of the <see cref="Texture2D"/>
        /// class are treated as SRGB encoded by default.
        /// </summary>
        public Boolean SrgbDefaultForTexture2D { get; set; }

        /// <summary>
        /// Gets or sets a value which determines whether instances of the <see cref="Texture3D"/>
        /// class are treated as SRGB encoded by default.
        /// </summary>
        public Boolean SrgbDefaultForTexture3D { get; set; }

        /// <summary>
        /// Gets or sets a value which determines whether instances of the <see cref="RenderBuffer2D"/>
        /// class are treated as SRGB encoded by default.
        /// </summary>
        public Boolean SrgbDefaultForRenderBuffer2D { get; set; }
    }
}
