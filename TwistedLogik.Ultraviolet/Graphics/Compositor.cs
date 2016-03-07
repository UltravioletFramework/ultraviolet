namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a window compositor, which is responsible for assembling the various components of a
    /// rendered scene into a final image for presentation to the user.
    /// </summary>
    public abstract class Compositor : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Compositor"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public Compositor(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Gets the current target to which the application should render.
        /// </summary>
        /// <returns>The target to which the application should render, or <see langword="null"/> if
        /// the application should render directly to the back buffer.</returns>
        public abstract RenderTarget2D GetRenderTarget();

        /// <summary>
        /// Prepares the compositor to begin rendering a frame.
        /// </summary>
        public virtual void BeginFrame()
        {

        }

        /// <summary>
        /// Prepares the compositor for the specified composition context.
        /// </summary>
        /// <param name="context">The <see cref="CompositionContext"/> to which the compositor should transition.</param>
        public virtual void BeginContext(CompositionContext context)
        {
            CurrentContext = context;
        }

        /// <summary>
        /// Presents the composited scene to the graphics device.
        /// </summary>
        public virtual void Present()
        {

        }

        /// <summary>
        /// Gets the current composition context.
        /// </summary>
        public virtual CompositionContext CurrentContext
        {
            get;
            private set;
        }
    }
}
