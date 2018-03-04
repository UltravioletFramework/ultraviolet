using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="Texture2D"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="pixels">A pointer to the raw pixel data with which to populate the texture.</param>
    /// <param name="width">The texture's width in pixels.</param>
    /// <param name="height">The texture's height in pixels.</param>
    /// <param name="bytesPerPixel">The number of bytes which represent each pixel in the raw data.</param>
    /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
    public delegate Texture2D Texture2DFromRawDataFactory(UltravioletContext uv, IntPtr pixels, Int32 width, Int32 height, Int32 bytesPerPixel);

    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="Texture2D"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="width">The texture's width in pixels.</param>
    /// <param name="height">The texture's height in pixels.</param>
    /// <param name="immutable">A value indicating whether the texture should be created with immutable storage.</param>
    /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
    public delegate Texture2D Texture2DFactory(UltravioletContext uv, Int32 width, Int32 height, Boolean immutable);

    /// <summary>
    /// Represents a two-dimensional texture.
    /// </summary>
    public abstract class Texture2D : Texture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public Texture2D(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="pixels">A pointer to the raw pixel data with which to populate the texture.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="bytesPerPixel">The number of bytes which represent each pixel in the raw data.</param>
        /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
        public static Texture2D Create(IntPtr pixels, Int32 width, Int32 height, Int32 bytesPerPixel)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Texture2DFromRawDataFactory>()(uv, pixels, width, height, bytesPerPixel);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
        public static Texture2D Create(Int32 width, Int32 height)
        {
            return Create(width, height, true);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="immutable">A value indicating whether to use immutable storage.</param>
        /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
        public static Texture2D Create(Int32 width, Int32 height, Boolean immutable)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Texture2DFactory>()(uv, width, height, immutable);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture2D"/> class which is designed to be
        /// dynamically updated from data on the CPU.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
        /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
        public static Texture2D CreateDynamic(Int32 width, Int32 height, Action<DynamicTexture2D> flushed)
        {
            return CreateDynamic(width, height, true, flushed);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture2D"/> class which is designed to be
        /// dynamically updated from data on the CPU.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="immutable">A value indicating whether to use immutable storage.</param>
        /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
        /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
        public static Texture2D CreateDynamic(Int32 width, Int32 height, Boolean immutable, Action<DynamicTexture2D> flushed)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.Require(flushed, nameof(flushed));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<DynamicTexture2DFactory>()(uv, width, height, immutable, flushed);
        }

        /// <summary>
        /// Resizes the texture.
        /// </summary>
        /// <param name="width">The texture's new width in pixels.</param>
        /// <param name="height">The texture's new height in pixels.</param>
        public abstract void Resize(Int32 width, Int32 height);

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array being set as the texture's data.</typeparam>
        /// <param name="data">An array containing the data to set.</param>
        public abstract void SetData<T>(T[] data) where T : struct;

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array being set as the texture's data.</typeparam>
        /// <param name="data">An array containing the data to set.</param>
        /// <param name="startIndex">The index of the first element to set.</param>
        /// <param name="elementCount">The number of elements to set.</param>
        public abstract void SetData<T>(T[] data, Int32 startIndex, Int32 elementCount) where T : struct;

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array being set as the texture's data.</typeparam>
        /// <param name="level">The mipmap level to set.</param>
        /// <param name="rect">A bounding box that defines the position and location (in pixels) of the data.</param>
        /// <param name="data">An array containing the data to set.</param>
        /// <param name="startIndex">The index of the first element to set.</param>
        /// <param name="elementCount">The number of elements to set.</param>
        public abstract void SetData<T>(Int32 level, Rectangle? rect, T[] data, Int32 startIndex, Int32 elementCount) where T : struct;

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
    }
}
