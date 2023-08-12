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
    /// <param name="format">The format of each pixel in the raw data.</param>
    /// <param name="options">The texture's configuration options.</param>
    /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
    public delegate Texture2D Texture2DFromRawDataFactory(UltravioletContext uv, IntPtr pixels, Int32 width, Int32 height, TextureFormat format, TextureOptions options);

    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="Texture2D"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="width">The texture's width in pixels.</param>
    /// <param name="height">The texture's height in pixels.</param>
    /// <param name="options">The texture's configuration options.</param>
    /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
    public delegate Texture2D Texture2DFactory(UltravioletContext uv, Int32 width, Int32 height, TextureOptions options);

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
        /// Creates a texture from the surface.
        /// </summary>
        /// <param name="surface">The surface</param>
        /// <param name="unprocessed">A value indicating whether the surface data should be passed
        /// through to the texture without any further processing, regardless of the platform's
        /// requirements. For example, an unprocessed texture will not be flipped vertically on
        /// the OpenGL implementation.</param>
        /// <returns>The <see cref="Texture2D"/> that was created from the surface.</returns>
        public static Texture2D CreateTextureFromSurface(Surface2D surface, Boolean unprocessed = false)
        {
            Contract.EnsureNotDisposed(surface, surface?.Disposed ?? true);

            var uv = UltravioletContext.DemandCurrent();

            if (unprocessed)
            {
                var options = TextureOptions.ImmutableStorage | (surface.SrgbEncoded ? TextureOptions.SrgbColor : TextureOptions.LinearColor);
                var format = TextureUtils.GetTextureFormatFromSurfaceFormat(SurfaceSourceDataFormat.RGBA, surface.BytesPerPixel);
                return Texture2D.CreateTexture((IntPtr)surface.Pixels, surface.Width, surface.Height, format, options);
            }
            else
            {
                surface.CreateSurface();
                using (var copysurf = surface.CreateSurface())
                {
                    copysurf.Flip(uv.GetGraphics().Capabilities.FlippedTextures ?
                        SurfaceFlipDirection.Vertical : SurfaceFlipDirection.None); // todo uvj: is this needed?

                    var options = TextureOptions.ImmutableStorage | (surface.SrgbEncoded ? TextureOptions.SrgbColor : TextureOptions.LinearColor);
                    var format = TextureUtils.GetTextureFormatFromSurfaceFormat(SurfaceSourceDataFormat.RGBA, copysurf.BytesPerPixel);
                    return Texture2D.CreateTexture((IntPtr)copysurf.Pixels, copysurf.Width, copysurf.Height, format, options);
                }
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="pixels">A pointer to the raw pixel data with which to populate the texture.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="format">The format of each pixel in the raw data.</param>
        /// <param name="options">The texture's configuration options.</param>
        /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
        public static Texture2D CreateTexture(IntPtr pixels, Int32 width, Int32 height, TextureFormat format, TextureOptions options = TextureOptions.Default)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Texture2DFromRawDataFactory>()(uv, pixels, width, height, format, options);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="options">The texture's configuration options.</param>
        /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
        public static Texture2D CreateTexture(Int32 width, Int32 height, TextureOptions options = TextureOptions.Default)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Texture2DFactory>()(uv, width, height, options);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture2D"/> class which is designed to be
        /// dynamically updated from data on the CPU.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="state">An arbitrary state object which will be passed to the flush handler.</param>
        /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
        /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
        public static Texture2D CreateDynamicTexture(Int32 width, Int32 height, Object state, Action<Texture2D, Object> flushed)
        {
            return CreateDynamicTexture(width, height, TextureOptions.Default, state, flushed);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture2D"/> class which is designed to be
        /// dynamically updated from data on the CPU.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="options">The texture's configuration options.</param>
        /// <param name="state">An arbitrary state object which will be passed to the flush handler.</param>
        /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
        /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
        public static Texture2D CreateDynamicTexture(Int32 width, Int32 height, TextureOptions options, Object state, Action<Texture2D, Object> flushed)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.Require(flushed, nameof(flushed));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<DynamicTexture2DFactory>()(uv, width, height, options, state, flushed);
        }
        
        /// <summary>
        /// Creates a new instance of the <see cref="RenderBuffer2D"/> that represents a color buffer.
        /// </summary>
        /// <param name="width">The render buffer's width in pixels.</param>
        /// <param name="height">The render buffer's height in pixels.</param>
        /// <param name="options">The render buffer's configuration options.</param>
        /// <returns>The instance of <see cref="RenderBuffer2D"/> that was created.</returns>
        public static RenderBuffer2D CreateRenderBuffer(Int32 width, Int32 height, RenderBufferOptions options = RenderBufferOptions.Default)
        {
            return CreateRenderBuffer(RenderBufferFormat.Color, width, height, options);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RenderBuffer2D"/> class.
        /// </summary>
        /// <param name="format">A <see cref="RenderBufferFormat"/> value specifying the render buffer's data format.</param>
        /// <param name="width">The render buffer's width in pixels.</param>
        /// <param name="height">The render buffer's height in pixels.</param>
        /// <param name="options">The render buffer's configuration options.</param>
        /// <returns>The instance of <see cref="RenderBuffer2D"/> that was created.</returns>
        public static RenderBuffer2D CreateRenderBuffer(RenderBufferFormat format, Int32 width, Int32 height, RenderBufferOptions options = RenderBufferOptions.Default)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<RenderBuffer2DFactory>()(uv, format, width, height, options);
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
        /// Sets the texture's data.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the buffer being set as the texture's data.</typeparam>
        /// <param name="data">A pointer to the data to set.</param>
        /// <param name="startIndex">The index of the first element to set.</param>
        /// <param name="elementCount">The number of elements to set.</param>
        public abstract void SetData<T>(IntPtr data, Int32 startIndex, Int32 elementCount) where T : struct;

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the buffer being set as the texture's data.</typeparam>
        /// <param name="level">The mipmap level to set.</param>
        /// <param name="rect">A bounding box that defines the position and location (in pixels) of the data.</param>
        /// <param name="data">A pointer to the data to set.</param>
        /// <param name="startIndex">The index of the first element to set.</param>
        /// <param name="elementCount">The number of elements to set.</param>
        public abstract void SetData<T>(Int32 level, Rectangle? rect, IntPtr data, Int32 startIndex, Int32 elementCount) where T : struct;

        /// <summary>
        /// Sets the texture's data from the data at the specified pointer.
        /// </summary>
        /// <param name="data">A pointer to the data to set.</param>
        /// <param name="offsetInBytes">The offset from the start of <paramref name="data"/>, in bytes, at which to begin copying data.</param>
        /// <param name="sizeInBytes">The number of bytes to copy.</param>
        public abstract void SetRawData(IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes);

        /// <summary>
        /// Sets the texture's data from the data at the specified pointer.
        /// </summary>
        /// <param name="level">The mipmap level to set.</param>
        /// <param name="rect">A bounding box that defines the position and location (in pixels) of the data.</param>
        /// <param name="data">A pointer to the data to set.</param>
        /// <param name="offsetInBytes">The offset from the start of <paramref name="data"/>, in bytes, at which to begin copying data.</param>
        /// <param name="sizeInBytes">The number of bytes to copy.</param>
        public abstract void SetRawData(Int32 level, Rectangle? rect, IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes);

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <param name="surface">The <see cref="Surface2D"/> which contains the data to set.</param>
        public abstract void SetData(Surface2D surface);

        /// <summary>
        /// Gets a value indicating whether this is an SRGB encoded texture.
        /// </summary>
        public abstract Boolean SrgbEncoded { get; }

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
