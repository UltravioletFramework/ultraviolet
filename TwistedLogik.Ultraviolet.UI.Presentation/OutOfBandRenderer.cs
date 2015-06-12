using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

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
            this.renderTargetPool = new ExpandingPool<OutOfBandRenderTarget>(4, 32, () => 
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
        /// Gets a value indicating whether the specified element's texture is ready to be used.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element's texture is ready; otherwise, <c>false</c>.</returns>
        public Boolean IsTextureReady(UIElement element)
        {
            OutOfBandRenderTarget rtarget;
            if (registeredElements.TryGetValue(element, out rtarget))
            {
                return rtarget.IsReady;
            }
            return false;
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
            rt.Resize(renderTargetSize, renderTargetSize);
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
                    kvp.Value.IsReady = false;

                foreach (var kvp in registeredElements)
                {
                    var element = kvp.Key;
                    var rtarget = kvp.Value;

                    graphics.SetRenderTarget(rtarget.RenderTarget);
                    graphics.Clear(Color.Transparent);

                    drawingContext.Reset(element.View.Display);
                    drawingContext.SpriteBatch = spriteBatch;

                    var centerX = rtarget.RenderTarget.Width / 2f;
                    var centerY = rtarget.RenderTarget.Height / 2f;

                    var display = element.View.Display;

                    var pxRenderOriginX = (Single)display.DipsToPixels(element.RenderSize.Width * element.RenderTransformOrigin.X);
                    var pxRenderOriginY = (Single)display.DipsToPixels(element.RenderSize.Height * element.RenderTransformOrigin.Y);
                    var pxPositionX     = (Single)display.DipsToPixels(element.AbsolutePosition.X);
                    var pxPositionY     = (Single)display.DipsToPixels(element.AbsolutePosition.Y);

                    var translate = new Vector2(centerX - (pxPositionX + pxRenderOriginX), centerY - (pxPositionY + pxRenderOriginY));
                    var transform = Matrix.CreateTranslation(translate.X, translate.Y, 0);

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, transform);

                    drawingContext.ClipTranslationX = translate.X;
                    drawingContext.ClipTranslationY = translate.Y;

                    element.Draw(time, drawingContext);

                    spriteBatch.End();

                    rtarget.IsReady = true;
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
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);
 
                return registeredElements.Count > 0; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether the out-of-band renderer is in the process of drawing its render targets.
        /// </summary>
        public Boolean IsDrawingRenderTargets
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return isDrawingRenderTargets; 
            }
        }

        /// <summary>
        /// Gets or sets the size of the renderer's render targets.
        /// </summary>
        public Int32 RenderTargetSize
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return renderTargetSize;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                var max     = Ultraviolet.GetGraphics().Capabilities.MaximumTextureSize;
                var clamped = Math.Min(max, value);

                if (clamped != renderTargetSize)
                {
                    renderTargetSize = clamped;

                    foreach (var kvp in registeredElements)
                    {
                        kvp.Value.Resize(renderTargetSize, renderTargetSize);
                    }
                }
            }
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

        // The pool of available render buffers.
        private readonly IPool<OutOfBandRenderTarget> renderTargetPool;

        // The collection of registered elements and their render buffers.
        private readonly SortedDictionary<UIElement, OutOfBandRenderTarget> registeredElements = 
            new SortedDictionary<UIElement, OutOfBandRenderTarget>(new UIElementComparer());
        
        // The drawing context used to render elements.
        private readonly DrawingContext drawingContext;
        private readonly SpriteBatch spriteBatch;

        // Property values.
        private Int32 renderTargetSize = 1;
        private Boolean isDrawingRenderTargets;
    }
}
