using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="DynamicTexture2D"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="width">The texture's width in pixels.</param>
    /// <param name="height">The texture's height in pixels.</param>
    /// <param name="options">The texture's configuration options.</param>
    /// <param name="state">An arbitrary state object which will be passed to the flush handler.</param>
    /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
    /// <returns>The instance of <see cref="DynamicTexture2D"/> that was created.</returns>
    public delegate DynamicTexture2D DynamicTexture2DFactory(UltravioletContext uv, Int32 width, Int32 height, TextureOptions options, Object state, Action<Texture2D, Object> flushed);

    /// <summary>
    /// Represents a 2D texture which is designed to be dynamically updated from data which resides on the CPU.
    /// </summary>
    public abstract class DynamicTexture2D : Texture2D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicTexture2D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="options">The texture's configuration options.</param>
        /// <param name="state">An arbitrary state object which will be passed to the flush handler.</param>
        /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
        protected DynamicTexture2D(UltravioletContext uv, Int32 width, Int32 height, TextureOptions options, Object state, Action<Texture2D, Object> flushed)
            : base(uv)
        {
            Contract.Require(flushed, nameof(flushed));

            this.state = state;
            this.flushed = flushed;
        }
        
        /// <summary>
        /// Flushes any pending changes and uploads the result to video memory.
        /// </summary>
        public void Flush() => flushed(this, this.state);

        // The delegate to invoke when the texture is flushed.
        private readonly Object state;
        private readonly Action<Texture2D, Object> flushed;
    }
}
