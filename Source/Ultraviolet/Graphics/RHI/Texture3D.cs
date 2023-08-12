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
    /// <param name="format">The format each each pixel in the raw data.</param>
    /// <param name="options">The texture's configuration options.</param>
    /// <returns>The instance of <see cref="Texture3D"/> that was created.</returns>
    public delegate Texture3D Texture3DFromRawDataFactory(UltravioletContext uv, IList<IntPtr> layers, Int32 width, Int32 height, TextureFormat format, TextureOptions options);

    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="Texture3D"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="width">The texture's width in pixels.</param>
    /// <param name="height">The texture's height in pixels.</param>
    /// <param name="depth">The texture's depth in layers.</param>
    /// <param name="options">The texture's configuration options.</param>
    /// <returns>The instance of <see cref="Texture3D"/> that was created.</returns>
    public delegate Texture3D Texture3DFactory(UltravioletContext uv, Int32 width, Int32 height, Int32 depth, TextureOptions options);

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
        /// Creates a texture from the surface.
        /// </summary>
        /// <param name="surface">The surface</param>
        /// <param name="unprocessed">A value indicating whether the surface data should be passed
        /// through to the texture without any further processing, regardless of the platform's
        /// requirements. For example, an unprocessed texture will not be flipped vertically on
        /// the OpenGL implementation.</param>
        /// <returns>The <see cref="Texture3D"/> that was created from the surface.</returns>
        public static Texture3D CreateTextureFromSurface(Surface3D surface, Boolean unprocessed)
        {
            Contract.EnsureNotDisposed(surface, surface?.Disposed ?? true);
            Contract.Ensure(surface.IsComplete, UltravioletStrings.SurfaceIsNotComplete);

            var uv = UltravioletContext.DemandCurrent();

            for (int i = 0; i < surface.Depth; i++)
            {
                if (surface.GetLayer(i).SrgbEncoded != surface.SrgbEncoded)
                    throw new InvalidOperationException(UltravioletStrings.SurfaceLayerEncodingMismatch);
            }

            var copysurfs = new Surface2D[surface.Depth];
            var surfsdata = new IntPtr[surface.Depth];
            try
            {
                var flipdir = unprocessed ? SurfaceFlipDirection.None :
                    (uv.GetGraphics().Capabilities.FlippedTextures ? SurfaceFlipDirection.Vertical : SurfaceFlipDirection.None);

                for (int i = 0; i < copysurfs.Length; i++)
                {
                    copysurfs[i] = surface.GetLayer(i).CreateSurface();
                    copysurfs[i].FlipAndProcessAlpha(flipdir, false, Color.Magenta);
                    surfsdata[i] = (IntPtr)(copysurfs[i]).Pixels;
                }

                var options = TextureOptions.ImmutableStorage | (surface.SrgbEncoded ? TextureOptions.SrgbColor : TextureOptions.LinearColor);
                return Texture3D.CreateTexture(surfsdata, surface.Width, surface.Height, TextureUtils.GetTextureFormatFromSurfaceFormat(SurfaceSourceDataFormat.RGBA, surface.BytesPerPixel), options);
            }
            finally
            {
                foreach (var copysurf in copysurfs)
                    copysurf?.Dispose();
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture3D"/> class.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="depth">The texture's depth in layers.</param>
        /// <param name="options">The texture's configuration options.</param>
        /// <returns>The instance of <see cref="Texture3D"/> that was created.</returns>
        public static Texture3D CreateTexture(Int32 width, Int32 height, Int32 depth, TextureOptions options = TextureOptions.Default)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(depth > 0, nameof(depth));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Texture3DFactory>()(uv, width, height, depth, options);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture3D"/> class.
        /// </summary>
        /// <param name="layers">A pointer to the raw pixel data for each of the texture's layers.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="format">The format of each pixel in the raw data.</param>
        /// <param name="options">The texture's configuration options.</param>
        /// <returns>The instance of <see cref="Texture3D"/> that was created.</returns>
        public static Texture3D CreateTexture(IList<IntPtr> layers, Int32 width, Int32 height, TextureFormat format, TextureOptions options = TextureOptions.Default)
        {
            Contract.Require(layers, nameof(layers));
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(format == TextureFormat.RGB || format == TextureFormat.BGR || format == TextureFormat.RGBA || format == TextureFormat.BGRA, nameof(format));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Texture3DFromRawDataFactory>()(uv, layers, width, height, format, options);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture3D"/> class which is designed to be
        /// dynamically updated from data on the CPU.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="depth">The texture's depth in pixels.</param>
        /// <param name="state">An arbitrary state object which will be passed to the flush handler.</param>
        /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
        /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
        public static Texture3D CreateDynamicTexture(Int32 width, Int32 height, Int32 depth, Object state, Action<Texture3D, Object> flushed)
        {
            return CreateDynamicTexture(width, height, depth, TextureOptions.ImmutableStorage, state, flushed);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Texture3D"/> class which is designed to be
        /// dynamically updated from data on the CPU.
        /// </summary>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="depth">The texture's depth in pixels.</param>
        /// <param name="options">The texture's configuration options.</param>
        /// <param name="state">An arbitrary state object which will be passed to the flush handler.</param>
        /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
        /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
        public static Texture3D CreateDynamicTexture(Int32 width, Int32 height, Int32 depth, TextureOptions options, Object state, Action<Texture3D, Object> flushed)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.Require(flushed, nameof(flushed));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<DynamicTexture3DFactory>()(uv, width, height, depth, options, state, flushed);
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
        /// <param name="left">The x-coordinate of the left face of the 3D bounding cube.</param>
        /// <param name="top">The y-coordinate of the top face of the 3D bounding cube.</param>
        /// <param name="right">The x-coordinate of the right face of the 3D bounding cube.</param>
        /// <param name="bottom">The y-coordinate of the bottom face of the 3D bounding cube.</param>
        /// <param name="front">The z-coordinate of the front face of the 3D bounding cube.</param>
        /// <param name="back">The z-coordinate of the back face of the 3D bounding cube.</param>
        /// <param name="data">A pointer to the data to set.</param>
        /// <param name="offsetInBytes">The offset from the start of <paramref name="data"/>, in bytes, at which to begin copying data.</param>
        /// <param name="sizeInBytes">The number of bytes to copy.</param>
        public abstract void SetRawData(Int32 level, Int32 left, Int32 top, Int32 right, Int32 bottom, Int32 front, Int32 back, IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes);

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

        /// <summary>
        /// Gets the texture's depth in layers.
        /// </summary>
        public abstract Int32 Depth
        {
            get;
        }        
    }
}
