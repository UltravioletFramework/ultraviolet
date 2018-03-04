using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="DynamicTexture3D"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="width">The texture's width in pixels.</param>
    /// <param name="height">The texture's height in pixels.</param>
    /// <param name="depth">The texture's depth in pixels.</param>
    /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
    /// <returns>The instance of <see cref="Texture2D"/> that was created.</returns>
    public delegate DynamicTexture3D DynamicTexture3DFactory(UltravioletContext uv, Int32 width, Int32 height, Int32 depth, Action<DynamicTexture3D> flushed);

    /// <summary>
    /// Represents a 3D texture which is designed to be dynamically updated from data which resides on the CPU.
    /// </summary>
    public abstract class DynamicTexture3D : Texture3D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicTexture3D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="depth">The texture's depth in pixels.</param>
        /// <param name="immutable">A value indicating whether to use immutable storage.</param>
        /// <param name="flushed">The handler to invoke when the texture is flushed.</param>
        protected DynamicTexture3D(UltravioletContext uv, Int32 width, Int32 height, Int32 depth, Boolean immutable, Action<Texture3D> flushed)
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
        private readonly Action<Texture3D> flushed;
    }
}
