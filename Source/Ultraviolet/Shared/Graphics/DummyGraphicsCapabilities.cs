using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a dummy implementation of <see cref="GraphicsCapabilities"/>.
    /// </summary>
    public sealed class DummyGraphicsCapabilities : GraphicsCapabilities
    {
        /// <inheritdoc/>
        public override Boolean FlippedTextures { get; }

        /// <inheritdoc/>
        public override Boolean Supports3DTextures { get; }

        /// <inheritdoc/>
        public override Boolean SupportsDepthStencilTextures { get; }

        /// <inheritdoc/>
        public override Boolean SupportsInstancedRendering { get; }

        /// <inheritdoc/>
        public override Boolean SupportsNonZeroBaseInstance { get; }

        /// <inheritdoc/>
        public override Boolean SupportsPreservingRenderTargetContentInHardware { get; }

        /// <inheritdoc/>
        public override Boolean SupportsIndependentSamplerState { get; }

        /// <inheritdoc/>
        public override Boolean SupportsIntegerVertexAttributes { get; }

        /// <inheritdoc/>
        public override Boolean SupportsDoublePrecisionVertexAttributes { get; }

        /// <inheritdoc/>
        public override Boolean SrgbEncodingEnabled { get; }

        /// <inheritdoc/>
        public override Int32 MaximumTextureSize { get; } = 0;

        /// <inheritdoc/>
        public override Int32 MaximumViewportHeight { get; } = 0;

        /// <inheritdoc/>
        public override Int32 MaximumViewportWidth { get; } = 0;
    }
}
