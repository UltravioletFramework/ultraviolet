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
    /// <param name="immutable">A value indicating whether to use immutable storage.</param>
    /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
    /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
    public delegate DynamicTexture2D DynamicTexture2DFactory(UltravioletContext uv, Int32 width, Int32 height, Boolean immutable, Action<DynamicTexture2D> flushed);

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
        /// <param name="immutable">A value indicating whether to use immutable storage.</param>
        /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
        protected DynamicTexture2D(UltravioletContext uv, Int32 width, Int32 height, Boolean immutable, Action<Texture2D> flushed)
            : base(uv)
        {
            Contract.Require(flushed, nameof(flushed));

            this.flushed = flushed;
        }
        
        /// <summary>
        /// Flushes any pending changes and uploads the result to video memory.
        /// </summary>
        public void Flush() => flushed(this);

        // The delegate to invoke when the texture is flushed.
        private readonly Action<Texture2D> flushed;
    }
}
