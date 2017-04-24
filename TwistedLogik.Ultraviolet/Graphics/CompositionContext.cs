namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the various composition contexts exposed by Ultraviolet which can be used by
    /// an instance of the <see cref="Compositor"/> class to break a frame down into individual components.
    /// </summary>
    public enum CompositionContext
    {
        /// <summary>
        /// The context containing the basic 2D/3D scene.
        /// </summary>
        Scene,

        /// <summary>
        /// The context containing the user interface elements.
        /// </summary>
        Interface,

        /// <summary>
        /// The context containing graphics which are drawn on top of the interface.
        /// </summary>
        Overlay,
    }
}
