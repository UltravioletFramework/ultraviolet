using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the DynamicIndexBuffer class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="itype">The index element type.</param>
    /// <param name="icount">The index element count.</param>
    /// <returns>The instance of DynamicIndexBuffer that was created.</returns>
    public delegate DynamicIndexBuffer DynamicIndexBufferFactory(UltravioletContext uv, IndexBufferElementType itype, Int32 icount);

    /// <summary>
    /// Represents a index buffer that is optimized for dynamic updates.
    /// </summary>
    public abstract class DynamicIndexBuffer : IndexBuffer
    {
        /// <summary>
        /// Initializes a new instance of the IndexBuffer class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="itype">The index element type.</param>
        /// <param name="icount">The index element count.</param>
        public DynamicIndexBuffer(UltravioletContext uv, IndexBufferElementType itype, Int32 icount)
            : base(uv, itype, icount)
        {

        }

        /// <summary>
        /// Creates a new instance of the DynamicIndexBuffer class.
        /// </summary>
        /// <param name="itype">The index element type.</param>
        /// <param name="icount">The index element count.</param>
        /// <returns>The instance of DynamicIndexBuffer that was created.</returns>
        public static new DynamicIndexBuffer Create(IndexBufferElementType itype, Int32 icount)
        {
            Contract.EnsureRange(icount > 0, "icount");

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<DynamicIndexBufferFactory>()(uv, itype, icount);
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
        /// Raises the ContentLost event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnContentLost(EventArgs e)
        {
            var temp = ContentLost;
            if (temp != null)
            {
                temp(this, e);
            }
        }
    }
}
