using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;

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
        /// <param name="window">The window with which this compositor is associated.</param>
        public Compositor(UltravioletContext uv, IUltravioletWindow window)
            : base(uv)
        {
            Contract.Require(window, nameof(window));

            this.window = window;
        }

        /// <summary>
        /// Gets the current target to which the application should render.
        /// </summary>
        /// <returns>The target to which the application should render, or <see langword="null"/> if
        /// the application should render directly to the back buffer.</returns>
        public abstract RenderTarget2D GetRenderTarget();

        /// <summary>
        /// Converts a point in compositor space to an equivalent point in window space.
        /// </summary>
        /// <param name="pt">The point in screen space to convert.</param>
        /// <returns>The converted point in window space.</returns>
        public virtual Point2 PointToWindow(Point2 pt)
        {
            return pt;
        }
        
        /// <summary>
        /// Converts a point in compositor space to an equivalent point in window space.
        /// </summary>
        /// <param name="x">The x-coordinate of the point in screen space to convert.</param>
        /// <param name="y">The y-coordinate of the point in screen space to convert.</param>
        /// <returns>The converted point in window space.</returns>
        public Point2 PointToWindow(Int32 x, Int32 y)
        {
            return PointToWindow(new Point2(x, y));
        }

        /// <summary>
        /// Converts a point in window space to an equivalent point in compositor space.
        /// </summary>
        /// <param name="pt">The point in window space to convert.</param>
        /// <returns>The converted point in compositor space.</returns>
        public virtual Point2 WindowToPoint(Point2 pt)
        {
            return pt;
        }

        /// <summary>
        /// Converts a point in window space to an equivalent point in compositor space.
        /// </summary>
        /// <param name="x">The x-coordinate of the point in window space to convert.</param>
        /// <param name="y">The y-coordinate of the point in window space to convert.</param>
        /// <returns>The converted point in compositor space.</returns>
        public Point2 WindowToPoint(Int32 x, Int32 y)
        {
            return WindowToPoint(new Point2(x, y));
        }

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
            this.currentContext = context;
        }

        /// <summary>
        /// Presents the composited scene to the graphics device.
        /// </summary>
        public virtual void Present()
        {

        }
        
        /// <summary>
        /// Gets the window with which this compositor is associated.
        /// </summary>
        public IUltravioletWindow Window
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return window;
            }
        }

        /// <summary>
        /// Gets the current composition context.
        /// </summary>
        public CompositionContext CurrentContext
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return currentContext;
            }
        }

        /// <summary>
        /// Gets the current size of the composition buffer.
        /// </summary>
        public abstract Size2 Size
        {
            get;
        }

        /// <summary>
        /// Gets the current width of the composition buffer.
        /// </summary>
        public Int32 Width
        {
            get { return Size.Width; }
        }

        /// <summary>
        /// Gets the current height of the composition buffer.
        /// </summary>
        public Int32 Height
        {
            get { return Size.Height; }
        }

        // Property values.
        private readonly IUltravioletWindow window;
        private CompositionContext currentContext;        
    }
}
