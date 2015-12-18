using System;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a dummy implementation of <see cref="GraphicsCapabilities"/>.
    /// </summary>
    public sealed class DummyGraphicsCapabilities : GraphicsCapabilities
    {
        /// <inheritdoc/>
        public override Boolean SupportsDepthStencilTextures
        {
            get { return false; }
        }

        /// <inheritdoc/>
        public override Int32 MaximumTextureSize
        {
            get { return 0; }
        }

        /// <inheritdoc/>
        public override Int32 MaximumViewportHeight
        {
            get { return 0; }
        }

        /// <inheritdoc/>
        public override Int32 MaximumViewportWidth
        {
            get { return 0; }
        }
    }
}
