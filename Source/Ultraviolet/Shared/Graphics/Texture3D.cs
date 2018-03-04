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
    /// Represents a factory method which constructs instances of the <see cref="Texture3D"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="width">The texture's width in pixels.</param>
    /// <param name="height">The texture's height in pixels.</param>
    /// <param name="depth">The texture's depth in layers.</param>
    /// <param name="immutable">A value indicating whether the texture should be created with immutable storage.</param>
    /// <returns>The instance of <see cref="Texture3D"/> that was created.</returns>
    public delegate Texture3D Texture3DFactory(UltravioletContext uv, Int32 width, Int32 height, Int32 depth, Boolean immutable);

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
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="depth">The texture's depth in layers.</param>
        /// <returns>The instance of <see cref="Texture3D"/> that was created.</returns>
        public static Texture3D Create(Int32 width, Int32 height, Int32 depth)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(depth > 0, nameof(depth));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Texture3DFactory>()(uv, width, height, depth, true);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture3D"/> class.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="depth">The texture's depth in layers.</param>
        /// <param name="immutable">A value indicating whether to use immutable storage.</param>
        /// <returns>The instance of <see cref="Texture3D"/> that was created.</returns>
        public static Texture3D Create(Int32 width, Int32 height, Int32 depth, Boolean immutable)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(depth > 0, nameof(depth));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Texture3DFactory>()(uv, width, height, depth, immutable);
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
        /// Creates a new instance of the <see cref="Texture3D"/> class which is designed to be
        /// dynamically updated from data on the CPU.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="depth">The texture's depth in pixels.</param>
        /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
        /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
        public static Texture3D CreateDynamic(Int32 width, Int32 height, Int32 depth, Action<DynamicTexture3D> flushed)
        {
            return CreateDynamic(width, height, depth, true, flushed);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture3D"/> class which is designed to be
        /// dynamically updated from data on the CPU.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="depth">The texture's depth in pixels.</param>
        /// <param name="immutable">A value indicating whether to use immutable storage.</param>
        /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
        /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
        public static Texture3D CreateDynamic(Int32 width, Int32 height, Int32 depth, Boolean immutable, Action<DynamicTexture3D> flushed)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.Require(flushed, nameof(flushed));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<DynamicTexture3DFactory>()(uv, width, height, depth, flushed);
        }

        /// <summary>
        /// Resizes the texture.
        /// </summary>
        /// <param name="width">The texture's new width in pixels.</param>
        /// <param name="height">The texture's new height in pixels.</param>
        /// <param name="depth">The texture's new depth in layers.</param>
        public abstract void Resize(Int32 width, Int32 height, Int32 depth);

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <typeparam name="T">The type of elements in the data array.</typeparam>
        /// <param name="data">An array containing the data to set.</param>
        public abstract void SetData<T>(T[] data) where T : struct;

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <typeparam name="T">The type of elements in the data array.</typeparam>
        /// <param name="data">An array containing the data to set.</param>
        /// <param name="startIndex">The index of the first element to set.</param>
        /// <param name="elementCount">The number of elements to set.</param>
        public abstract void SetData<T>(T[] data, Int32 startIndex, Int32 elementCount) where T : struct;

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <typeparam name="T">The type of elements in the data array.</typeparam>
        /// <param name="level">The mipmap level for which to set data.</param>
        /// <param name="left">The x-coordinate of the left face of the 3D bounding cube.</param>
        /// <param name="top">The y-coordinate of the top face of the 3D bounding cube.</param>
        /// <param name="right">The x-coordinate of the right face of the 3D bounding cube.</param>
        /// <param name="bottom">The y-coordinate of the bottom face of the 3D bounding cube.</param>
        /// <param name="front">The z-coordinate of the front face of the 3D bounding cube.</param>
        /// <param name="back">The z-coordinate of the back face of the 3D bounding cube.</param>
        /// <param name="data">An array containing the data to set.</param>
        /// <param name="startIndex">The index of the first element to set.</param>
        /// <param name="elementCount">The number of elements to set.</param>
        public abstract void SetData<T>(Int32 level, Int32 left, Int32 top, Int32 right, Int32 bottom, Int32 front, Int32 back, T[] data, Int32 startIndex, Int32 elementCount) where T : struct;

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
