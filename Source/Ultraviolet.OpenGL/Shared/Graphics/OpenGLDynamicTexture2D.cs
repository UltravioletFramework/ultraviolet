using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the <see cref="DynamicTexture2D"/> class.
    /// </summary>
    public class OpenGLDynamicTexture2D : DynamicTexture2D, IOpenGLDynamicTexture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLDynamicTexture2D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="immutable">A value indicating whether to use immutable storage.</param>
        /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
        public OpenGLDynamicTexture2D(UltravioletContext uv, Int32 width, Int32 height, Boolean immutable, Action<Texture2D> flushed)
            : base(uv, width, height, immutable, flushed)
        {
            this.texture = new OpenGLTexture2D(uv, width, height, immutable);
        }

        /// <inheritdoc/>
        public override Int32 CompareTo(Texture other) => 
            texture.CompareTo(other);

        /// <inheritdoc/>
        public override void Resize(Int32 width, Int32 height) => 
            texture.Resize(width, height);

        /// <inheritdoc/>
        public override void SetData<T>(T[] data) => 
            texture.SetData(data);

        /// <inheritdoc/>
        public override void SetData<T>(T[] data, Int32 startIndex, Int32 elementCount) => 
            texture.SetData(data, startIndex, elementCount);

        /// <inheritdoc/>
        public override void SetData<T>(Int32 level, Rectangle? rect, T[] data, Int32 startIndex, Int32 elementCount) => 
            texture.SetData(level, rect, data, startIndex, elementCount);

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
        public override Int32 Width => texture.Width;

        /// <inheritdoc/>
        public override Int32 Height => texture.Height;

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
        private readonly OpenGLTexture2D texture;
    }
}
