namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents Ultraviolet's default compositor, which renders
    /// directly to the back buffer.
    /// </summary>
    public sealed class DefaultCompositor : Compositor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCompositor"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        private DefaultCompositor(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="DefaultCompositor"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="DefaultCompositor"/> that was created.</returns>
        public static DefaultCompositor Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return new DefaultCompositor(uv);
        }

        /// <inheritdoc/>
        public override RenderTarget2D GetRenderTarget() => null;

        /// <inheritdoc/>
        public override void BeginFrame()
        {
            var window = Ultraviolet.GetPlatform().Windows.GetCurrent();
            var graphics = Ultraviolet.GetGraphics();
            graphics.SetRenderTarget(null);
            graphics.SetViewport(new Viewport(0, 0, window.ClientSize.Width, window.ClientSize.Height));
            graphics.Clear(Color.CornflowerBlue, 1.0f, 0);
        }        
    }
}
