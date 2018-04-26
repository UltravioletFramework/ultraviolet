using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Platform;
using Ultraviolet.TestFramework.Graphics;

namespace Ultraviolet.Tests.Graphics
{
    /// <summary>
    /// Represents a test compositor which renders the scene to a buffer of predetermined size,
    /// then scales the buffer up to fill the back buffer.
    /// </summary>
    public sealed class CustomCompositor : Compositor, ITestFrameworkCompositor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCompositor"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="window">The window with which this compositor is associated.</param>
        public CustomCompositor(UltravioletContext uv, IUltravioletWindow window)
            : base(uv, window)
        {
            rtScene = RenderTarget2D.Create(BufferWidth, BufferHeight);

            rtSceneColor = Texture2D.CreateRenderBuffer(RenderBufferFormat.Color, BufferWidth, BufferHeight);
            rtScene.Attach(rtSceneColor);

            rtSceneDepthStencil = Texture2D.CreateRenderBuffer(RenderBufferFormat.Depth24Stencil8, BufferWidth, BufferHeight);
            rtScene.Attach(rtSceneDepthStencil);

            rtInterface = RenderTarget2D.Create(BufferWidth, BufferHeight);

            rtInterfaceColor = Texture2D.CreateRenderBuffer(RenderBufferFormat.Color, BufferWidth, BufferHeight);
            rtInterface.Attach(rtInterfaceColor);

            rtInterfaceDepthStencil = Texture2D.CreateRenderBuffer(RenderBufferFormat.Depth24Stencil8, BufferWidth, BufferHeight);
            rtInterface.Attach(rtInterfaceDepthStencil);

            rtComposition = RenderTarget2D.Create(BufferWidth, BufferHeight, RenderTargetUsage.PreserveContents);

            rtCompositionColor = Texture2D.CreateRenderBuffer(RenderBufferFormat.Color, BufferWidth, BufferHeight);
            rtComposition.Attach(rtCompositionColor);

            rtCompositionDepthStencil = Texture2D.CreateRenderBuffer(RenderBufferFormat.Depth24Stencil8, BufferWidth, BufferHeight);
            rtComposition.Attach(rtCompositionDepthStencil);

            spriteBatch = SpriteBatch.Create();
        }

        /// <inheritdoc/>
        public override Point2 PointToWindow(Point2 pt)
        {
            var xFactor = Window.DrawableSize.Width / (Double)BufferWidth;
            var yFactor = Window.DrawableSize.Height / (Double)BufferHeight;

            return new Point2((Int32)(pt.X * xFactor), (Int32)(pt.Y * yFactor));
        }

        /// <inheritdoc/>
        public override Point2 WindowToPoint(Point2 pt)
        {
            var xFactor = BufferWidth / (Double)Window.DrawableSize.Width;
            var yFactor = BufferHeight / (Double)Window.DrawableSize.Height;

            return new Point2((Int32)(pt.X * xFactor), (Int32)(pt.Y * yFactor));
        }

        /// <inheritdoc/>
        public override RenderTarget2D GetRenderTarget()
        {
            switch (CurrentContext)
            {
                case CompositionContext.Scene:
                    return rtScene;

                case CompositionContext.Interface:
                case CompositionContext.Overlay:
                    return rtInterface;

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <inheritdoc/>
        public override void BeginFrame()
        {
            var gfx = Ultraviolet.GetGraphics();

            gfx.SetRenderTarget(rtComposition);
            gfx.Clear(Color.Transparent, 1.0f, 0);

            gfx.SetRenderTarget(rtScene);
            gfx.Clear(Color.Transparent, 1.0f, 0);

            base.BeginFrame();
        }

        /// <inheritdoc/>
        public override void BeginContext(CompositionContext context, Boolean force)
        {
            if (CurrentContext == context && !force)
                return;

            var gfx = Ultraviolet.GetGraphics();

            switch (context)
            {
                case CompositionContext.Scene:
                    {
                        Compose();

                        gfx.SetRenderTarget(rtScene);
                        gfx.Clear(Color.Transparent, 1.0f, 0);
                    }
                    break;

                case CompositionContext.Interface:
                    {
                        gfx.SetRenderTarget(rtInterface);
                        gfx.Clear(Color.Transparent, 1.0f, 0);
                    }
                    break;

                case CompositionContext.Overlay:
                    {
                        gfx.SetRenderTarget(rtInterface);
                    }
                    break;

                default:
                    throw new ArgumentException(nameof(context));
            }

            base.BeginContext(context);
        }

        /// <inheritdoc/>
        public override void Compose()
        {
            var gfx = Ultraviolet.GetGraphics();
            gfx.SetRenderTarget(rtComposition);

            var area = new RectangleF(0, 0,
                rtComposition.Width, rtComposition.Height);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            spriteBatch.Draw(rtSceneColor, area, Color.Red);
            spriteBatch.Draw(rtInterfaceColor, area, Color.Lime);
            spriteBatch.End();

            gfx.UnbindTexture(rtSceneColor);
            gfx.UnbindTexture(rtInterfaceColor);

            base.Compose();
        }

        /// <inheritdoc/>
        public override void Present()
        {
            var gfx = Ultraviolet.GetGraphics();
            gfx.SetRenderTarget(TestFrameworkRenderTarget);
            gfx.Clear(Color.Black, 1.0f, 0);

            var area = new RectangleF(0, 0,
                Window.DrawableSize.Width, Window.DrawableSize.Height);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            spriteBatch.Draw(rtCompositionColor, area, Color.White);
            spriteBatch.End();

            gfx.UnbindTexture(rtCompositionColor);

            base.Present();
        }

        /// <inheritdoc/>
        public override Size2 Size
        {
            get { return new Size2(BufferWidth, BufferHeight); }
        }

        /// <inheritdoc/>
        public RenderTarget2D TestFrameworkRenderTarget
        {
            get;
            set;
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(rtSceneDepthStencil);
                SafeDispose.Dispose(rtSceneColor);
                SafeDispose.Dispose(rtScene);

                SafeDispose.Dispose(rtInterfaceDepthStencil);
                SafeDispose.Dispose(rtInterfaceColor);
                SafeDispose.Dispose(rtInterface);

                SafeDispose.Dispose(rtCompositionColor);
                SafeDispose.Dispose(rtComposition);

                SafeDispose.Dispose(spriteBatch);
            }
            base.Dispose(disposing);
        }

        // State values.
        private const Int32 BufferWidth = 480 / 4;
        private const Int32 BufferHeight = 360 / 4;

        private RenderTarget2D rtScene;
        private RenderBuffer2D rtSceneColor;
        private RenderBuffer2D rtSceneDepthStencil;

        private RenderTarget2D rtInterface;
        private RenderBuffer2D rtInterfaceColor;
        private RenderBuffer2D rtInterfaceDepthStencil;

        private RenderTarget2D rtComposition;
        private RenderBuffer2D rtCompositionColor;
        private RenderBuffer2D rtCompositionDepthStencil;

        private SpriteBatch spriteBatch;
    }
}
