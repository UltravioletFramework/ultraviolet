namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Specifies whether a render target's data is preserved or discarded
    /// after it is bound to the graphics device.
    /// </summary>
    public enum RenderTargetUsage
    {
        /// <summary>
        /// The render target's data is always discarded when the render target is bound.
        /// </summary>
        DiscardContents,

        /// <summary>
        /// The render target's data is preserved on platforms which support doing so in hardware,
        /// and discarded on platforms which do not provide such support.
        /// </summary>
        PlatformContents,

        /// <summary>
        /// The render target's data is always preserved when the render target is bound.
        /// Some platforms may not support this in hardware, leading to performance issues;
        /// check the value of <see cref="GraphicsCapabilities.SupportsPreservingRenderTargetContentInHardware"/>
        /// to determine whether this is the case.
        /// </summary>
        PreserveContents,
    }
}
