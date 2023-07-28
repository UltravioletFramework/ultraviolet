using System;
using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation
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
            this.spriteBatch = SpriteBatch.Create();

            this.drawingContext = new DrawingContext();
            this.drawingContext.SpriteBatch = spriteBatch;
        }

        /// <summary>
        /// Gets a value indicating whether the renderer is currently drawing the out-of-band render targets for
        /// the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the render targets for the specified element are currently being drawn; otherwise, <see langword="false"/>.</returns>
        public Boolean IsDrawingRenderTargetFor(UIElement element)
        {
            Contract.Require(element, nameof(element));

            return element == currentElementDrawingRenderTarget;
        }

        /// <summary>
        /// Gets a value indicating whether the specified element is rendered out-of-band.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the specified element is rendered out-of-band; otherwise, <see langword="false"/>.</returns>
        public Boolean IsRenderedOutOfBand(UIElement element)
        {
            Contract.Require(element, nameof(element));
            Contract.EnsureNotDisposed(this, Disposed);

            return element.OutOfBandRenderTarget != null;
        }

        /// <summary>
        /// Gets a value indicating whether the specified element's texture is ready to be used.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the specified element's texture is ready; otherwise, <see langword="false"/>.</returns>
        public Boolean IsTextureReady(UIElement element)
        {
            Contract.Require(element, nameof(element));
            Contract.EnsureNotDisposed(this, Disposed);

            var rtarget = element.OutOfBandRenderTarget;
            return rtarget != null && rtarget.Value.IsReady;
        }

        /// <summary>
        /// Ensures that the out-of-band renderer's internal pools have been created.
        /// </summary>
        public void InitializePools()
        {
            if (renderTargetPool != null)
                return;

            renderTargetPool = new UpfPool<OutOfBandRenderTarget>(Ultraviolet, 8, 32, () => new OutOfBandRenderTarget(Ultraviolet));
        }

        /// <summary>
        /// Updates the state of the out-of-band renderer.
        /// </summary>
        public void Update()
        {
            if (renderTargetPool == null)
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.BeginUpdate();

            renderTargetPool.Update();

            upf.PerformanceStats.EndUpdate();
        }

        /// <summary>
        /// Registers an element with the out-of-band renderer.
        /// </summary>
        /// <param name="element">The element to register.</param>
        /// <param name="additional">The number of additional render targets to reserve.</param>
        public void Register(UIElement element, Int32 additional)
        {
            Contract.Require(element, nameof(element));
            Contract.EnsureRange(additional >= 0, nameof(additional));
            Contract.EnsureNotDisposed(this, Disposed);

            if (IsRenderedOutOfBand(element))
                return;

            InitializePools();

            var weakRef = WeakReferencePool.Instance.Retrieve();
            weakRef.Target = element;

            var target = renderTargetPool.Retrieve(element);
            var targetObject = target.Value;

            var bounds = default(RectangleD);
            targetObject.ResizeForElement(element, out bounds);

            var currentTarget = targetObject;
            currentTarget.NextInternal = null;

            for (int i = 0; i < additional; i++)
            {
                var additionalTarget = renderTargetPool.Retrieve(element);
                var additionalTargetObject = additionalTarget.Value;

                additionalTargetObject.NextInternal = null;
                additionalTargetObject.Resize(targetObject.Width, targetObject.Height);

                currentTarget.NextInternal = additionalTarget;
                currentTarget = additionalTarget.Value;
            }

            registeredElements.Add(weakRef);
            element.OutOfBandRenderTarget = target;
        }

        /// <summary>
        /// Unregisters an element from the out-of-band renderer.
        /// </summary>
        /// <param name="element">The element to unregister.</param>
        public void Unregister(UIElement element)
        {
            Contract.Require(element, nameof(element));
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var registeredElement in registeredElements)
            {
                var weakRefTarget = (UIElement)registeredElement.Target;
                if (weakRefTarget != element)
                    continue;

                for (UpfPool<OutOfBandRenderTarget>.PooledObject current = element.OutOfBandRenderTarget, next = null; current != null; current = next)
                {
                    next = current.Value.NextInternal;
                    current.Value.Resize(1, 1);
                    current.Value.NextInternal = null;
                    renderTargetPool.Release(current);
                }

                registeredElements.Remove(registeredElement);
                element.OutOfBandRenderTarget = null;

                registeredElement.Target = null;
                WeakReferencePool.Instance.Release(registeredElement);

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

            if (registeredElements.Count == 0)
                return;

            var graphics = Ultraviolet.GetGraphics();

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.BeginDraw();

            try
            {
                isDrawingRenderTargets = true;

                foreach (var registeredElement in registeredElements)
                {
                    var element = (UIElement)registeredElement.Target;
                    if (element != null && element.OutOfBandRenderTarget != null)
                    {
                        element.OutOfBandRenderTarget.Value.IsReady = false;
                    }

                    if (element.View != null && !element.View.LayoutRoot.IsLoaded)
                        viewsNeedingLoading.Add(element.View);
                }

                foreach (var view in viewsNeedingLoading)
                {
                    view.EnsureIsLoaded();
                }
                viewsNeedingLoading.Clear();

                registeredElements.Sort(uiElementComparer);
                foreach (var registeredElement in registeredElements)
                {
                    var element = (UIElement)registeredElement.Target;
                    if (element == null)
                    {
                        deadReferences.Add(registeredElement);
                        continue;
                    }

                    if (element.View == null)
                        continue;

                    var bounds = default(RectangleD);
                    var effect = element.Effect;

                    var rtarget = element.OutOfBandRenderTarget.Value;
                    if (rtarget.ResizeForElement(element, out bounds))
                    {
                        for (var current = rtarget.Next; current != null; current = current.Next)
                            current.Resize(rtarget.Width, rtarget.Height);
                    }

                    var popup = element as Popup;
                    if (popup != null)
                    {
                        if (!popup.IsOpen)
                            continue;

                        element = popup.Root;
                    }

                    if (!element.TransformedVisualBounds.IsEmpty && !IsVisuallyDisconnectedFromRoot(element))
                    {
                        graphics.SetRenderTarget(rtarget.RenderTarget, Color.Transparent);
                        drawingContext.Reset(element.View.Display);

                        var visualParent = VisualTreeHelper.GetParent(element) as UIElement;
                        var visualTransformOfParent = (visualParent == null) ? popup.PopupTransformToView : visualParent.GetVisualTransformMatrix();
                        var visualTransformOfElement = element.GetVisualTransformMatrix(ref visualTransformOfParent);

                        rtarget.VisualTransform = visualTransformOfElement;
                        rtarget.VisualBounds = bounds;

                        currentElementDrawingRenderTarget = element;

                        element.DrawToRenderTarget(time, drawingContext, rtarget.RenderTarget,
                            (popup != null) ? popup.PopupTransformToViewInDevicePixels : visualTransformOfParent);

                        if (rtarget.Next != null)
                        {
                            if (effect != null)
                            {
                                effect.DrawRenderTargets(drawingContext, element, rtarget);
                            }
                        }

                        currentElementDrawingRenderTarget = null;

                        var rt2d = rtarget.RenderTarget;
                        rtarget.RenderTarget.Invalidate(false, rt2d.HasDepthBuffer, rt2d.HasStencilBuffer, rt2d.HasDepthStencilBuffer, 0, 0);
                        rtarget.IsReady = true;
                    }
                }

                foreach (var deadReference in deadReferences)
                    registeredElements.Remove(deadReference);
            }
            finally
            {
                isDrawingRenderTargets = false;
                currentElementDrawingRenderTarget = null;
            }
            deadReferences.Clear();

            graphics.SetRenderTarget(null, Color.Transparent);

            upf.PerformanceStats.EndDraw();
        }

        /// <summary>
        /// Gets the render target that represents the specified element.
        /// </summary>
        /// <param name="element">The element for which to retrieve a render target.</param>
        /// <returns>The render target associated with the specified out-of-band element, or <c>null if the element is
        /// not registered for out-of-band rendering.</c></returns>
        public OutOfBandRenderTarget GetElementRenderTarget(UIElement element)
        {
            Contract.Require(element, nameof(element));
            Contract.EnsureNotDisposed(this, Disposed);

            var pooledObject = element.OutOfBandRenderTarget;
            return (pooledObject == null) ? null : pooledObject.Value;
        }

        /// <summary>
        /// Gets a value indicating whether the out-of-band renderer is currently in use (that is,
        /// whether it currently has any registered elements).
        /// </summary>
        public Boolean IsCurrentlyInUse => registeredElements.Count > 0;

        /// <summary>
        /// Gets a value indicating whether the out-of-band renderer is in the process of drawing its render targets.
        /// </summary>
        public Boolean IsDrawingRenderTargets => isDrawingRenderTargets;

        /// <summary>
        /// Gets the number of active out-of-band render targets which have been allocated from the internal pool.
        /// </summary>
        public Int32 ActiveRenderTargets => (renderTargetPool == null) ? 0 : renderTargetPool.Active;

        /// <summary>
        /// Gets the number of available out-of-band render targets in the internal pool.
        /// </summary>
        public Int32 AvailableRenderTargets => (renderTargetPool == null) ? 0 : renderTargetPool.Available;

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
        /// <returns><see langword="true"/> if the specified element is visually disconnected; otherwise, <see langword="false"/>.</returns>
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
        private UpfPool<OutOfBandRenderTarget> renderTargetPool;

        // The collection of registered elements and their render buffers.
        private readonly List<PresentationFoundationView> viewsNeedingLoading = new List<PresentationFoundationView>();
        private readonly List<WeakReference> deadReferences = new List<WeakReference>();
        private readonly List<WeakReference> registeredElements = new List<WeakReference>();
        private readonly UIElementComparer uiElementComparer = new UIElementComparer();

        // The drawing context used to render elements.
        private readonly DrawingContext drawingContext;
        private readonly SpriteBatch spriteBatch;

        // Property values.
        private bool isDrawingRenderTargets;

        // State values.
        private UIElement currentElementDrawingRenderTarget;
    }
}