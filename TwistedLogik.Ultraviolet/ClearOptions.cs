using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents the set of buffers which will be cleared by the <see cref="IUltravioletGraphics.Clear(ClearOptions, Color, Double, Int32)"/> method.
    /// </summary>
    [Flags]
    public enum ClearOptions
    {
        /// <summary>
        /// Clears the render target.
        /// </summary>
        Target = 1,

        /// <summary>
        /// Clears the depth buffer.
        /// </summary>
        DepthBuffer = 2,

        /// <summary>
        /// Clears the stencil buffer.
        /// </summary>
        Stencil = 4,
    }
}
