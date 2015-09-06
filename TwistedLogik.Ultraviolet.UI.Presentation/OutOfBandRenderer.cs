using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Media;

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
            this.renderTargetPool = new PresentationFoundationPool<OutOfBandRenderTarget>(uv, 4, 32, () => new OutOfBandRenderTarget(uv));
            this.weakReferencePool = new ExpandingPool<WeakReference>(4, 32, () => new WeakReference(null));

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

            return element.OutOfBandRenderTarget != null;
        }

        /// <summary>
        /// Gets a value indicating whether the specified element's texture is ready to be used.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element's texture is ready; otherwise, <c>false</c>.</returns>
        public Boolean IsTextureReady(UIElement element)
        {
            Contract.Require(element, "element");
            Contract.EnsureNotDisposed(this, Disposed);

            var oobrt = element.OutOfBandRenderTarget;
            return oobrt != null && oobrt.IsReady;
        }

        /// <summary>
        /// Updates the state of the out-of-band renderer.
        /// </summary>
        public void Update()
        {
            renderTargetPool.Update();
        }

        /// <summary>
        /// Registers an element with the out-of-band renderer.
        /// </summary>
        /// <param name="element">The element to register.</param>
        /// <param name="additional">The number of additional render targets to reserve.</param>
        public void Register(UIElement element, Int32 additional)
        {
            Contract.Require(element, "element");
            Contract.EnsureRange(additional >= 0, "additional");
            Contract.EnsureNotDisposed(this, Disposed);

            if (IsRenderedOutOfBand(element))
                return;

            var weakRef = weakReferencePool.Retrieve();
            weakRef.Target = element;
            
            var target = renderTargetPool.Retrieve(element);
            var bounds = default(RectangleD);
            target.ResizeForElement(element, out bounds);

            var currentTarget = target;
            currentTarget.Next = null;

            for (int i = 0; i < additional; i++)
            {
                var additionalTarget = renderTargetPool.Retrieve(element);
                additionalTarget.Next = null;
                additionalTarget.Resize(target.Width, target.Height);

                currentTarget.Next = additionalTarget;
            }

            registeredElements.Add(weakRef, target);
            element.OutOfBandRenderTarget = target;
        }

        /// <summary>
        /// Unregisters an element from the out-of-band renderer.
        /// </summary>
        /// <param name="element">The element to unregister.</param>
        public void Unregister(UIElement element)
        {
            Contract.Require(element, "element");
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var registeredElement in registeredElements)
            {
                var weakRefTarget = (UIElement)registeredElement.Key.Target;
                if (weakRefTarget != element)
                    continue;

                for (OutOfBandRenderTarget current = element.OutOfBandRenderTarget, next = null; current != null; current = next)
                {
                    next = current.Next;
                    current.Resize(1, 1);
                    current.Next = null;
                    renderTargetPool.Release(element, current);
                }

                registeredElements.Remove(registeredElement.Key);
                element.OutOfBandRenderTarget = null;

                registeredElement.Key.Target = null;
                weakReferencePool.Release(registeredElement.Key);

                break;
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
                    var element = (UIElement)kvp.Key.Target;
                    if (element == null || element.View == null)
                        continue;
                    
                    element.View.EnsureIsLoaded();

                    var bounds = default(RectangleD);
                    var effect = element.Effect;

                    var rtarget = kvp.Value;
                    rtarget.ResizeForElement(element, out bounds);

                    for (var current = rtarget.Next; current != null; current = current.Next)
                        current.Resize(rtarget.Width, rtarget.Height);

                    graphics.SetRenderTarget(rtarget.RenderTarget);
                    graphics.Clear(Color.Transparent);

                    var popup = element as Popup;
                    if (popup != null)
                    {
                        if (!popup.IsOpen)
                            continue;

                        element = popup.Root;
                    }

                    if (!element.AbsoluteVisualBounds.IsEmpty && !IsVisuallyDisconnectedFromRoot(element))
                    {
                        drawingContext.Reset(element.View.Display);

                        var visualParent = VisualTreeHelper.GetParent(element) as UIElement;
                        var visualTransformOfParent = (visualParent == null) ? popup.PopupTransformToAncestor : visualParent.GetVisualTransformMatrix();
                        var visualTransformOfElement = element.GetVisualTransformMatrix(ref visualTransformOfParent);

                        rtarget.VisualTransform = visualTransformOfElement;
                        rtarget.VisualBounds = bounds;

                        element.DrawToRenderTarget(time, drawingContext, rtarget.RenderTarget,
                            (element is PopupRoot) ? visualTransformOfElement : visualTransformOfParent);

                        if (rtarget.Next != null)
                        {
                            if (effect != null)
                            {
                                effect.DrawRenderTargets(drawingContext, element, rtarget);
                            }
                        }

                        rtarget.IsReady = true;
                    }
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
        /// Gets the render target that represents the specified element.
        /// </summary>
        /// <param name="element">The element for which to retrieve a render target.</param>
        /// <returns>The render target associated with the specified out-of-band element, or <c>null if the element is
        /// not registered for out-of-band rendering.</c></returns>
        public OutOfBandRenderTarget GetElementRenderTarget(UIElement element)
        {
            Contract.Require(element, "element");
            Contract.EnsureNotDisposed(this, Disposed);

            return element.OutOfBandRenderTarget;
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
        
        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(spriteBatch);
                SafeDispose.Dispose(renderTargetPool);

                registeredElements.Clear();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets a value indicating whether the specified element has become disconnected from the view's layout root through the visual tree.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element is visually disconnected; otherwise, <c>false</c>.</returns>
        private static Boolean IsVisuallyDisconnectedFromRoot(UIElement element)
        {
            var current = element;
            
            while (current != null)
            {
                if (current == element.View.LayoutRoot)
                    return false;

                if (current is PopupRoot)
                {
                    var popup = current.Parent as Popup;
                    if (popup == null || !popup.IsOpen)
                        return true;

                    current = popup;
                }
                else
                {
                    current = VisualTreeHelper.GetParent(current) as UIElement;
                }
            }

            return true;
        }

        // Object pools.
        private readonly PresentationFoundationPool<OutOfBandRenderTarget> renderTargetPool;
        private readonly ExpandingPool<WeakReference> weakReferencePool;

        // The collection of registered elements and their render buffers.
        private readonly SortedDictionary<WeakReference, OutOfBandRenderTarget> registeredElements =
            new SortedDictionary<WeakReference, OutOfBandRenderTarget>(new UIElementComparer());

        // The drawing context used to render elements.
        private readonly DrawingContext drawingContext;
        private readonly SpriteBatch spriteBatch;

        // Property values.
        private bool isDrawingRenderTargets;
    }
}