using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="Surface3D"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="width">The surface's width in pixels.</param>
    /// <param name="height">The surface's height in pixels.</param>
    /// <param name="depth">The number of layers in the surface.</param>
    /// <param name="bytesPerPixel">The number of bytes used to represent a pixel on the surface.</param>
    /// <returns>The instance of <see cref="Surface3D"/> that was created.</returns>
    public delegate Surface3D Surface3DFactory(UltravioletContext uv, Int32 width, Int32 height, Int32 depth, Int32 bytesPerPixel);

    /// <summary>
    /// Represents a three-dimensional image which is held in CPU memory.
    /// </summary>
    public abstract class Surface3D : Surface
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Surface3D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public Surface3D(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="Surface3D"/> class.
        /// </summary>
        /// <param name="width">The surface's width in pixels.</param>
        /// <param name="height">The surface's height in pixels.</param>
        /// <param name="depth">The number of layers in the surface.</param>
        /// <param name="bytesPerPixel">The number of bytes used to represent a pixel on the surface.</param>
        /// <returns>The instance of <see cref="Surface3D"/> which was created.</returns>
        public static Surface3D Create(Int32 width, Int32 height, Int32 depth, Int32 bytesPerPixel = 4)
        {
            Contract.Ensure(width > 0, nameof(width));
            Contract.Ensure(height > 0, nameof(height));
            Contract.Ensure(depth > 0, nameof(depth));
            Contract.Ensure(bytesPerPixel == 3 || bytesPerPixel == 4, nameof(bytesPerPixel));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Surface3DFactory>()(uv, width, height, depth, bytesPerPixel);
        }

        /// <summary>
        /// Gets the <see cref="Surface2D"/> which represents the specified layer of 
        /// this <see cref="Surface3D"/> instance.
        /// </summary>
        /// <param name="layer">The index of the layer to retrieve.</param>
        /// <returns>The <see cref="Surface2D"/> which represents the specified
        /// layer of this <see cref="Surface3D"/> instance.</returns>
        public abstract Surface2D GetLayer(Int32 layer);

        /// <summary>
        /// Sets the <see cref="Surface2D"/> which represents the specified layer 
        /// of this <see cref="Surface3D"/> instance.
        /// </summary>
        /// <param name="layer">The index of the layer to set.</param>
        /// <param name="surface">The <see cref="Surface2D"/> which represents the
        /// specified layer of this <see cref="Surface3D"/> instance.</param>
        /// <param name="transferOwnership">A value indicating whether ownership of the layer
        /// is transferred to this object. If so, the layer surface will be disposed when this object is disposed.</param>
        public abstract void SetLayer(Int32 layer, Surface2D surface, Boolean transferOwnership = false);

        /// <summary>
        /// Sets the collection of <see cref="Surface2D"/> instances which make up the 
        /// layers of this <see cref="Surface3D"/> instance.
        /// </summary>
        /// <param name="surfaces">The collection that contains the <see cref="Surface2D"/>
        /// instances which make up the layers of this <see cref="Surface3D"/> instance.</param>
        /// <param name="transferOwnership">A value indicating whether ownership of the layer
        /// is transferred to this object. If so, the layer surface will be disposed when this object is disposed.</param>
        public abstract void SetLayers(IEnumerable<Surface2D> surfaces, Boolean transferOwnership = false);

        /// <summary>
        /// Sets the collection of <see cref="Surface2D"/> instances which make up the
        /// layers of this <see cref="Surface3D"/> instance.
        /// </summary>
        /// <param name="surfaces">The collection that contains the <see cref="Surface2D"/>
        /// instances which make up the layers of this <see cref="Surface3D"/> instance.</param>
        /// <param name="offset">The offset at which to begin retrieving layers from <paramref name="surfaces"/>.</param>
        /// <param name="transferOwnership">A value indicating whether ownership of the layers
        /// is transferred to this object. If so, the layer surfaces will be disposed when this object is disposed.</param>
        public abstract void SetLayers(IEnumerable<Surface2D> surfaces, Int32 offset, Boolean transferOwnership = false);

        /// <summary>
        /// Prepares the layers of this surface to be exported as texture data.
        /// </summary>
        /// <param name="premultiply">A value indicating whether to premultiply the layers' alpha.</param>
        /// <param name="flip">A value indicating whether to flip the layer data upside-down.</param>
        /// <param name="opaque">A value indicating whether the texture is opaque and color keying should be disabled.</param>
        public abstract void PrepareForTextureExport(Boolean premultiply, Boolean flip, Boolean opaque);

        /// <summary>
        /// Creates a texture from the surface.
        /// </summary>
        /// <returns>The <see cref="Texture3D"/> that was created from the surface.</returns>
        public abstract Texture3D CreateTexture();

        /// <summary>
        /// Creates a texture from the surface.
        /// </summary>
        /// <param name="premultiply">A value indicating whether to premultiply the surface's alpha.</param>
        /// <param name="flip">A value indicating whether to flip the surface data upside-down.</param>
        /// <param name="opaque">A value indicating whether the texture is opaque and color keying should be disabled.</param>
        /// <returns>The <see cref="Texture3D"/> that was created from the surface.</returns>
        public abstract Texture3D CreateTexture(Boolean premultiply, Boolean flip, Boolean opaque);

        /// <summary>
        /// Gets the surface's width in pixels.
        /// </summary>
        public abstract Int32 Width { get; }

        /// <summary>
        /// Gets the surface's height in pixels.
        /// </summary>
        public abstract Int32 Height { get; }

        /// <summary>
        /// Gets the number of layers in this surface.
        /// </summary>
        public abstract Int32 Depth { get; }

        /// <summary>
        /// Gets the number of bytes used to represent a single pixel on this surface.
        /// </summary>
        public abstract Int32 BytesPerPixel { get; }

        /// <summary>
        /// Gets a value indicating whether all of this surface's layers have been specified.
        /// </summary>
        public abstract Boolean IsComplete { get; }

        /// <summary>
        /// Gets a value indicating whether this surface is ready to be exported as a texture.
        /// </summary>
        public abstract Boolean IsReadyForTextureExport { get; }
    }
}
