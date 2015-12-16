using System;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the set of options which can be used to modify the behavior of a <see cref="RenderBuffer2D"/>.
    /// </summary>
    [Flags]
    public enum RenderBufferOptions
    {
        /// <summary>
        /// The render buffer has no special options.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// The render buffer should use immutable storage, if it is available.
        /// </summary>
        ImmutableStorage = 0x01,

        /// <summary>
        /// Specifies that the render buffer will not be sampled. Render buffers which have been 
        /// created with this flag cannot be bound as a texture or have their data
        /// populated via a call to the SetData() method.
        /// </summary>
        WillNotBeSampled = 0x02,
    }
}
