using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains methods for rendering UI elements out-of-band, that is, prior to rendering the rest of the
    /// visual tree. This is necessary in order to properly render arbitrarily transformed elements.
    /// </summary>
    internal sealed partial class OutOfBandRenderer : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfBandRenderer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public OutOfBandRenderer(UltravioletContext uv)
            : base(uv)
        {
            this.renderTargetPool = new ExpandingPool<OutOfBandRenderTarget>(4, () => 
            {
                return new OutOfBandRenderTarget(uv);
            });

            this.spriteBatch = SpriteBatch.Create();

            this.drawingContext = new DrawingContext();
            this.drawingContext.SpriteBatch = spriteBatch;
        }

        /// <summary>
        /// Gets a value indicating whether the specified element is rendered out-of-band.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element is rendered out-of-band; otherwise, <c>false</c>.</returns>
        public Boolean IsRenderedOutOfBand(UIElement element)
        {
            Contract.Require(element, "element");
            Contract.EnsureNotDisposed(this, Disposed);

            return registeredElements.ContainsKey(element);
        }

        /// <summary>
        /// Registers an element with the out-of-band renderer.
        /// </summary>
        /// <param name="element">The element to register.</param>
        public void Register(UIElement element)
        {
            Contract.Require(element, "element");
            Contract.EnsureNotDisposed(this, Disposed);

            if (IsRenderedOutOfBand(element))
                return;

            var rt = renderTargetPool.Retrieve();
            MatchRenderTargetSizeToElement(rt, element);
            registeredElements.Add(element, rt);
        }

        /// <summary>
        /// Unregisters an element from the out-of-band renderer.
        /// </summary>
        /// <param name="element">The element to unregister.</param>
        public void Unregister(UIElement element)
        {
            Contract.Require(element, "element");
            Contract.EnsureNotDisposed(this, Disposed);

            OutOfBandRenderTarget rt;
            if (registeredElements.TryGetValue(element, out rt))
            {
                rt.Resize(1, 1);
                renderTargetPool.Release(rt);
                registeredElements.Remove(element);
            }
        }

        /// <summary>
        /// Draws out-of-band elements to their render buffers.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        public void DrawRenderTargets(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var graphics = Ultraviolet.GetGraphics();

            try
            {
                isDrawingRenderTargets = true;

                foreach (var kvp in registeredElements)
                {
                    var element = kvp.Key;
                    var rtarget = kvp.Value;

                    MatchRenderTargetSizeToElement(rtarget, element);

                    graphics.SetRenderTarget(rtarget.RenderTarget);
                    graphics.Clear(Color.Transparent);

                    drawingContext.Reset();
                    drawingContext.SpriteBatch = spriteBatch;

                    var translate = (Vector2)element.View.Display.DipsToPixels(element.AbsolutePosition);
                    var transform = Matrix.CreateTranslation(-translate.X, -translate.Y, 0);

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, transform);

                    element.Draw(time, drawingContext);

                    spriteBatch.End();
                }
            }
            finally
            {
                isDrawingRenderTargets = false;
            }

            graphics.SetRenderTarget(null);
            graphics.Clear(Color.Transparent);
        }

        /// <summary>
        /// Gets the texture that represents the specified element.
        /// </summary>
        /// <param name="element">The element for which to retrieve a texture.</param>
        /// <returns>The texture associated with the specified out-of-band element, or <c>null</c> if the element is
        /// not registered for out-of-band rendering.</returns>
        public Texture2D GetElementTexture(UIElement element)
        {
            Contract.Require(element, "element");
            Contract.EnsureNotDisposed(this, Disposed);

            OutOfBandRenderTarget buffer;
            if (registeredElements.TryGetValue(element, out buffer))
            {
                return buffer.ColorBuffer;
            }
            return null;
        }

        /// <summary>
        /// Gets a value indicating whether the out-of-band renderer is currently in use (that is,
        /// whether it currently has any registered elements).
        /// </summary>
        public Boolean IsCurrentlyInUse
        {
            get { return registeredElements.Count > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the out-of-band renderer is in the process of drawing its render targets.
        /// </summary>
        public Boolean IsDrawingRenderTargets
        {
            get { return isDrawingRenderTargets; }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(spriteBatch);

                foreach (var kvp in registeredElements)
                {
                    kvp.Value.Dispose();
                }
                registeredElements.Clear();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Resizes the specified render target to match the specified element.
        /// </summary>
        private static void MatchRenderTargetSizeToElement(OutOfBandRenderTarget rt, UIElement element)
        {
            var display = element.View.Display;
            var dims    = display.DipsToPixels(element.RenderSize);

            var width  = Math.Max(1, (Int32)dims.Width);
            var height = Math.Max(1, (Int32)dims.Height);

            if (rt.Width != width || rt.Height != height)
            {
                rt.Resize(width, height);
            }
        }

        // The pool of available render buffers.
        private readonly IPool<OutOfBandRenderTarget> renderTargetPool;

        // The collection of registered elements and their render buffers.
        private readonly Dictionary<UIElement, OutOfBandRenderTarget> registeredElements = 
            new Dictionary<UIElement, OutOfBandRenderTarget>();
        
        // The drawing context used to render elements.
        private readonly DrawingContext drawingContext;
        private readonly SpriteBatch spriteBatch;

        // Property values.
        private Boolean isDrawingRenderTargets;
    }
}
