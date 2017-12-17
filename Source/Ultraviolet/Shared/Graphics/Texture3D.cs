using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="Texture3D"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="layers">A pointer to the raw pixel data for each of the texture's layers.</param>
    /// <param name="width">The texture's width in pixels.</param>
    /// <param name="height">The texture's height in pixels.</param>
    /// <param name="bytesPerPixel">The number of bytes which represent each pixel in the raw data.</param>
    /// <returns>The instance of <see cref="Texture3D"/> that was created.</returns>
    public delegate Texture3D Texture3DFromRawDataFactory(UltravioletContext uv, IList<IntPtr> layers, Int32 width, Int32 height, Int32 bytesPerPixel);

    /// <summary>
    /// Represents a three-dimensional texture.
    /// </summary>
    public abstract class Texture3D : Texture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Texture3D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public Texture3D(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture3D"/> class.
        /// </summary>
        /// <param name="layers">A pointer to the raw pixel data for each of the texture's layers.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="bytesPerPixel">The number of bytes which represent each pixel in the raw data.</param>
        /// <returns>The instance of <see cref="Texture3D"/> that was created.</returns>
        public static Texture3D Create(IList<IntPtr> layers, Int32 width, Int32 height, Int32 bytesPerPixel)
        {
            Contract.Require(layers, nameof(layers));
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(bytesPerPixel == 3 || bytesPerPixel == 4, nameof(bytesPerPixel));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Texture3DFromRawDataFactory>()(uv, layers, width, height, bytesPerPixel);
        }
        
        /// <summary>
        /// Gets the texture's width in pixels.
        /// </summary>
        public abstract Int32 Width
        {
            get;
        }

        /// <summary>
        /// Gets the texture's height in pixels.
        /// </summary>
        public abstract Int32 Height
        {
            get;
        }

        /// <summary>
        /// Gets the texture's depth in layers.
        /// </summary>
        public abstract Int32 Depth
        {
            get;
        }        
    }
}
