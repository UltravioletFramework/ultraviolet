using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="DynamicVertexBuffer"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="vdecl">The vertex declaration for the buffer.</param>
    /// <param name="vcount">The number of vertices in the buffer.</param>
    /// <returns>The instance of <see cref="DynamicVertexBuffer"/> that was created.</returns>
    public delegate DynamicVertexBuffer DynamicVertexBufferFactory(UltravioletContext uv, VertexDeclaration vdecl, Int32 vcount);

    /// <summary>
    /// Represents a vertex buffer that is optimized for dynamic updates.
    /// </summary>
    public abstract class DynamicVertexBuffer : VertexBuffer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicVertexBuffer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="vdecl">The vertex declaration for the buffer.</param>
        /// <param name="vcount">The number of vertices in the buffer.</param>
        public DynamicVertexBuffer(UltravioletContext uv, VertexDeclaration vdecl, Int32 vcount)
            : base(uv, vdecl, vcount)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="DynamicVertexBuffer"/> class.
        /// </summary>
        /// <param name="vdecl">The vertex declaration for the buffer.</param>
        /// <param name="vcount">The number of vertices in the buffer.</param>
        /// <returns>The instance of <see cref="DynamicVertexBuffer"/> that was created.</returns>
        public static new DynamicVertexBuffer Create(VertexDeclaration vdecl, Int32 vcount)
        {
            Contract.Require(vdecl, nameof(vdecl));
            Contract.EnsureRange(vcount > 0, nameof(vcount));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<DynamicVertexBufferFactory>()(uv, vdecl, vcount);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DynamicVertexBuffer"/> class.
        /// </summary>
        /// <typeparam name="T">The buffer's vertex type.</typeparam>
        /// <param name="vcount">The number of vertices in the buffer.</param>
        /// <returns>The instance of <see cref="DynamicVertexBuffer"/> that was created.</returns>
        public static new DynamicVertexBuffer Create<T>(Int32 vcount) where T : struct, IVertexType
        {
            Contract.EnsureRange(vcount > 0, nameof(vcount));

            var vdecl = new T().VertexDeclaration;

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<DynamicVertexBufferFactory>()(uv, vdecl, vcount);
        }

        /// <summary>
        /// Gets a value indicating whether the buffer's content has been lost.
        /// </summary>
        public abstract Boolean IsContentLost
        {
            get;
        }

        /// <summary>
        /// Occurs when the buffer's content is lost.
        /// </summary>
        public event EventHandler ContentLost;

        /// <summary>
        /// Raises the <see cref="ContentLost"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnContentLost(EventArgs e) =>
            ContentLost?.Invoke(this, e);
    }
}
