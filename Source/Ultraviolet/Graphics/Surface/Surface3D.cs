using System;
using System.Collections.Generic;
using System.IO;
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
    /// <param name="options">The surface's configuration options.</param>
    /// <returns>The instance of <see cref="Surface3D"/> that was created.</returns>
    public delegate Surface3D Surface3DFactory(UltravioletContext uv, Int32 width, Int32 height, Int32 depth, Int32 bytesPerPixel, SurfaceOptions options);

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
        /// <param name="options">The surface's configuration options.</param>
        /// <returns>The instance of <see cref="Surface3D"/> which was created.</returns>
        public static Surface3D Create(Int32 width, Int32 height, Int32 depth, SurfaceOptions options = SurfaceOptions.Default)
        {
            return Create(width, height, depth, 4, options);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Surface3D"/> class.
        /// </summary>
        /// <param name="width">The surface's width in pixels.</param>
        /// <param name="height">The surface's height in pixels.</param>
        /// <param name="depth">The number of layers in the surface.</param>
        /// <param name="bytesPerPixel">The number of bytes used to represent a pixel on the surface.</param>
        /// <param name="options">The surface's configuration options.</param>
        /// <returns>The instance of <see cref="Surface3D"/> which was created.</returns>
        public static Surface3D Create(Int32 width, Int32 height, Int32 depth, Int32 bytesPerPixel, SurfaceOptions options = SurfaceOptions.Default)
        {
            Contract.Ensure(width > 0, nameof(width));
            Contract.Ensure(height > 0, nameof(height));
            Contract.Ensure(depth > 0, nameof(depth));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Surface3DFactory>()(uv, width, height, depth, bytesPerPixel, options);
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
        /// Creates a deep copy of this surface.
        /// </summary>
        /// <returns>A new instance of the <see cref="Surface3D"/> class which is a deep copy of this instance.</returns>
        public abstract Surface3D CreateSurface();

        /// <summary>
        /// Saves the surface as a JPEG image to the specified stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which to save the image data.</param>
        public abstract void SaveAsJpeg(Stream stream);

        /// <summary>
        /// Saves the surface as a PNG image to the specified stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which to save the image data.</param>
        public abstract void SaveAsPng(Stream stream);

        /// <summary>
        /// Gets or sets a value indicating whether the surface's data is SRGB encoded. The default value of this property
        /// is determined by the value of the <see cref="UltravioletContextProperties.SrgbDefaultForSurface3D"/> property.
        /// </summary>
        /// <remarks>Ultraviolet does not require that all of a 3D surface's layers match this property until the surface
        /// is converted to a texture. At that point, any mismatch will cause an exception to be thrown.</remarks>
        public abstract Boolean SrgbEncoded { get; set; }

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
    }
}
