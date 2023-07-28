using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the <see cref="DynamicTexture3D"/> class.
    /// </summary>
    public class OpenGLDynamicTexture3D : DynamicTexture3D, IOpenGLDynamicTexture, IOpenGLResource, IBindableResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLDynamicTexture2D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="depth">The texture's depth in pixels.</param>
        /// <param name="options">The texture's configuration options.</param>
        /// <param name="state">An arbitrary state object which will be passed to the flush handler.</param>
        /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
        public OpenGLDynamicTexture3D(UltravioletContext uv, Int32 width, Int32 height, Int32 depth, TextureOptions options, Object state, Action<Texture3D, Object> flushed)
            : base(uv, width, height, depth, options, state, flushed)
        {
            this.texture = new OpenGLTexture3D(uv, width, height, depth, options);
        }

        /// <inheritdoc/>
        public override Int32 CompareTo(Texture other) =>
            texture.CompareTo(other);

        /// <inheritdoc/>
        public override void Resize(Int32 width, Int32 height, Int32 depth) =>
            texture.Resize(width, height, depth);

        /// <inheritdoc/>
        public override void SetData<T>(T[] data) =>
            texture.SetData(data);

        /// <inheritdoc/>
        public override void SetData<T>(T[] data, Int32 startIndex, Int32 elementCount) =>
            texture.SetData(data, startIndex, elementCount);

        /// <inheritdoc/>
        public override void SetData<T>(Int32 level, Int32 left, Int32 top, Int32 right, Int32 bottom, Int32 front, Int32 back, T[] data, Int32 startIndex, Int32 elementCount) =>
            texture.SetData(level, left, top, right, bottom, front, back, data, startIndex, elementCount);

        /// <inheritdoc/>
        public override void SetRawData(IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes) =>
            texture.SetRawData(data, offsetInBytes, sizeInBytes);

        /// <inheritdoc/>
        public override void SetRawData(Int32 level, Int32 left, Int32 top, Int32 right, Int32 bottom, Int32 front, Int32 back, IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes) =>
            texture.SetRawData(level, left, top, right, bottom, front, back, data, offsetInBytes, sizeInBytes);

        /// <inheritdoc/>
        public void BindRead() =>
            texture.BindRead();

        /// <inheritdoc/>
        public void BindWrite() =>
            texture.BindWrite();

        /// <inheritdoc/>
        public void UnbindRead() =>
            texture.UnbindRead();

        /// <inheritdoc/>
        public void UnbindWrite() =>
            texture.UnbindWrite();

        /// <inheritdoc/>
        public UInt32 OpenGLName => texture.OpenGLName;

        /// <inheritdoc/>
        public override Boolean SrgbEncoded => texture.SrgbEncoded;

        /// <inheritdoc/>
        public override Int32 Width => texture.Width;

        /// <inheritdoc/>
        public override Int32 Height => texture.Height;

        /// <inheritdoc/>
        public override Int32 Depth => texture.Depth;

        /// <inheritdoc/>
        public override Boolean BoundForReading => texture.BoundForReading;

        /// <inheritdoc/>
        public override Boolean BoundForWriting => texture.BoundForWriting;

        /// <inheritdoc/>
        public override Boolean ImmutableStorage => texture.ImmutableStorage;

        /// <inheritdoc/>
        public override Boolean WillNotBeSampled => texture.WillNotBeSampled;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                SafeDispose.Dispose(texture);
            }

            base.Dispose(disposing);
        }

        // State values.
        private readonly OpenGLTexture3D texture;
    }
}
