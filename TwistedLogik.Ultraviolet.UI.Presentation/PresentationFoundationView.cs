using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Elements;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the top-level container for UI elements.
    /// </summary>
    public sealed class PresentationFoundationView : UIView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationFoundationView"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="viewModelType">The view's associated model type.</param>
        public PresentationFoundationView(UltravioletContext uv, Type viewModelType)
            : base(uv, viewModelType)
        {
            this.elementRegistry = new UIElementRegistry();
            this.resources       = new PresentationFoundationViewResources(this);
            this.drawingContext  = new DrawingContext(this);

            this.layoutRoot = new Grid(uv, null);
            this.layoutRoot.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.layoutRoot.VerticalAlignment = VerticalAlignment.Stretch;
            this.layoutRoot.View = this;
            this.layoutRoot.CacheLayoutParameters();
            this.layoutRoot.InvalidateMeasure();

            HookKeyboardEvents();
            HookMouseEvents();
        }

        /// <summary>
        /// Loads an instance of <see cref="PresentationFoundationView"/> from an XML document.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiPanelDefinition">The <see cref="UIPanelDefinition"/> that defines the view's containing panel.</param>
        /// <returns>The <see cref="PresentationFoundationView"/> that was loaded from the specified XML document.</returns>
        public static PresentationFoundationView Load(UltravioletContext uv, UIPanelDefinition uiPanelDefinition)
        {
            Contract.Require(uv, "uv");
            Contract.Require(uiPanelDefinition, "uiPanelDefinition");

            if (uiPanelDefinition.ViewElement == null)
                return null;

            var view = UvmlLoader.Load(uv, uiPanelDefinition.ViewElement);

            var uvss    = String.Join(Environment.NewLine, uiPanelDefinition.Stylesheets);
            var uvssdoc = UvssDocument.Parse(uvss);

            view.SetStylesheet(uvssdoc);

            return view;
        }

        /// <inheritdoc/>
        public override void Cleanup()
        {
            layoutRoot.Cleanup();
        }

        /// <inheritdoc/>
        public override void Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            Contract.Require(time, "time");
            Contract.Require(spriteBatch, "spriteBatch");

            if (Window == null)
                return;

            drawingContext.Reset();
            drawingContext.SpriteBatch = spriteBatch;

            layoutRoot.Draw(time, drawingContext);

            drawingContext.SpriteBatch = null;
        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            Contract.Require(time, "time");

            if (Window == null)
                return;

            HandleUserInput();

            layoutRoot.Update(time);
        }

        /// <inheritdoc/>
        public override void NavigateUp()
        {
            if (elementWithFocus == null)
                return;

            var target = default(UIElement);
            do
            {
                target = (target == null) ? elementWithFocus.GetNavUpElement() : target.GetNavUpElement();
                if (target != null && !target.Focusable)
                {
                    target = target.GetFirstFocusableDescendant(false);
                }
            }
            while (target != null && !LayoutUtil.IsValidForNav(target));
            
            if (target == null)
                return;

            FocusElement(target);
        }

        /// <inheritdoc/>
        public override void NavigateDown()
        {
            if (elementWithFocus == null)
                return;

            var target = default(UIElement);
            do
            {
                target = (target == null) ? elementWithFocus.GetNavDownElement() : target.GetNavDownElement();
                if (target != null && !target.Focusable)
                {
                    target = target.GetFirstFocusableDescendant(false);
                }
            }
            while (target != null && !LayoutUtil.IsValidForNav(target));

            if (target == null)
                return;

            FocusElement(target);
        }

        /// <inheritdoc/>
        public override void NavigateLeft()
        {
            if (elementWithFocus == null)
                return;

            var target = default(UIElement);
            do
            {
                target = (target == null) ? elementWithFocus.GetNavLeftElement() : target.GetNavLeftElement();
                if (target != null && !target.Focusable)
                {
                    target = target.GetFirstFocusableDescendant(false);
                }
            }
            while (target != null && !LayoutUtil.IsValidForNav(target));

            if (target == null)
                return;

            FocusElement(target);
        }

        /// <inheritdoc/>
        public override void NavigateRight()
        {
            if (elementWithFocus == null)
                return;

            var target = default(UIElement);
            do
            {
                target = (target == null) ? elementWithFocus.GetNavRightElement() : target.GetNavRightElement();
                if (target != null && !target.Focusable)
                {
                    target = target.GetFirstFocusableDescendant(false);
                }
            }
            while (target != null && !LayoutUtil.IsValidForNav(target));

            if (target == null)
                return;

            FocusElement(target);
        }

        /// <inheritdoc/>
        public override void NavigatePreviousTabStop()
        {
            var target = (elementWithFocus == null) ? GetLastFocusableElement(true) : elementWithFocus.GetPreviousTabStop() ?? GetLastFocusableElement(true);
            if (target == null)
                return;

            FocusElement(target);
        }

        /// <inheritdoc/>
        public override void NavigateNextTabStop()
        {
            var target = (elementWithFocus == null) ? GetFirstFocusableElement(true) : elementWithFocus.GetNextTabStop() ?? GetFirstFocusableElement(true);
            if (target == null)
                return;

            FocusElement(target);
        }

        /// <summary>
        /// Invalidates the styling state of the view's layout root.
        /// </summary>
        public void InvalidateStyle()
        {
            layoutRoot.InvalidateStyle();
        }

        /// <summary>
        /// Invalidates the measurement state of the view's layout root.
        /// </summary>
        public void InvalidateMeasure()
        {
            layoutRoot.InvalidateMeasure();
        }

        /// <summary>
        /// Invalidates the arrangement state of the view's layout root.
        /// </summary>
        public void InvalidateArrange()
        {
            layoutRoot.InvalidateArrange();
        }

        /// <summary>
        /// Grants input focus within this view to the specified element.
        /// </summary>
        /// <param name="element">The element to which to grant input focus.</param>
        public void FocusElement(UIElement element)
        {
            Contract.Require(element, "element");

            if (elementWithFocus == element || !element.Focusable)
                return;

            if (elementWithFocus != null)
            {
                var handledLostFocus = false;
                Keyboard.RaiseLostKeyboardFocus(elementWithFocus, ref handledLostFocus);
            }

            elementWithFocus = element;

            var handledGotFocus = false;
            Keyboard.RaiseGotKeyboardFocus(elementWithFocus, ref handledGotFocus);
        }

        /// <summary>
        /// Removes input focus within this view from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove input focus.</param>
        public void BlurElement(UIElement element)
        {
            Contract.Require(element, "element");

            if (elementWithFocus != element)
                return;

            var handledLostFocus = false;
            Keyboard.RaiseLostKeyboardFocus(elementWithFocus, ref handledLostFocus);
            
            elementWithFocus = null;
        }

        /// <summary>
        /// Assigns mouse capture to the specified element.
        /// </summary>
        /// <param name="element">The element to which to assign mouse capture.</param>
        public void CaptureMouse(UIElement element)
        {
            Contract.Require(element, "element");

            if (elementWithMouseCapture == element)
                return;

            if (elementWithMouseCapture != null)
            {
                var lostMouseCaptureHandled = false;
                Mouse.RaiseLostMouseCapture(elementWithMouseCapture, ref lostMouseCaptureHandled);
            }

            elementWithMouseCapture = element;

            var gotMouseCaptureHandled = false;
            Mouse.RaiseGotMouseCapture(elementWithMouseCapture, ref gotMouseCaptureHandled);
        }

        /// <summary>
        /// Releases the mouse from the element that is currently capturing it.
        /// </summary>
        /// <param name="element">The element that is attempting to release mouse capture.</param>
        public void ReleaseMouse(UIElement element)
        {
            Contract.Require(element, "element");

            if (elementWithMouseCapture != element)
                return;

            var lostMouseCaptureHandled = false;
            Mouse.RaiseLostMouseCapture(elementWithMouseCapture, ref lostMouseCaptureHandled);

            elementWithMouseCapture = null;
        }

        /// <summary>
        /// Gets the element within the view which has the specified identifying name.
        /// </summary>
        /// <param name="name">The identifying name of the element to retrieve.</param>
        /// <returns>The element with the specified identifying name, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementByName(String name)
        {
            Contract.RequireNotEmpty(name, "id");

            return elementRegistry.GetElementByName(name);
        }

        /// <summary>
        /// Performs a hit test against the view at the specified point in screen space.
        /// </summary>
        /// <param name="x">The x-coordinate in screen space to evaluate.</param>
        /// <param name="y">The y-coordinate in screen space to evaluate.</param>
        /// <returns>The topmost <see cref="UIElement"/> in the view which contains the specified point, or <c>null</c>.</returns>
        public UIElement HitTestScreenPixel(Int32 x, Int32 y)
        {
            var dipsX = Display.PixelsToDips(x - Area.X);
            var dipsY = Display.PixelsToDips(y - Area.Y);

            return LayoutRoot.HitTest(new Point2D(dipsX, dipsY)) as UIElement;
        }

        /// <summary>
        /// Performs a hit test against the view at the specified point in screen space.
        /// </summary>
        /// <param name="point">The point in screen space to evaluate.</param>
        /// <returns>The topmost <see cref="UIElement"/> in the view which contains the specified point, or <c>null</c>.</returns>
        public UIElement HitTestScreenPixel(Point2 point)
        {
            var dipsPoint = Display.PixelsToDips(point - Area.Location);

            return LayoutRoot.HitTest(dipsPoint) as UIElement;
        }

        /// <summary>
        /// Performs a hit test against the view at the specified point in view-relative screen space.
        /// </summary>
        /// <param name="x">The x-coordinate in view-relative screen space to evaluate.</param>
        /// <param name="y">The y-coordinate in view-relative screen space to evaluate.</param>
        /// <returns>The topmost <see cref="UIElement"/> in the view which contains the specified point, or <c>null</c>.</returns>
        public UIElement HitTestPixel(Int32 x, Int32 y)
        {
            var dipsX = Display.PixelsToDips(x);
            var dipsY = Display.PixelsToDips(y);

            return LayoutRoot.HitTest(new Point2D(dipsX, dipsY)) as UIElement;
        }

        /// <summary>
        /// Performs a hit test against the view at the specified point in view-relative screen space.
        /// </summary>
        /// <param name="point">The point in view-relative screen space to evaluate.</param>
        /// <returns>The topmost <see cref="UIElement"/> in the view which contains the specified point, or <c>null</c>.</returns>
        public UIElement HitTestPixel(Point2 point)
        {
            var dipsPoint = Display.PixelsToDips(point - Area.Location);

            return LayoutRoot.HitTest(dipsPoint) as UIElement;
        }

        /// <summary>
        /// Performs a hit test against the view at the specified point in device-independent view space.
        /// </summary>
        /// <param name="x">The x-coordinate in device-independent view space to evaluate.</param>
        /// <param name="y">The y-coordinate in device-independent view space to evaluate.</param>
        /// <returns>The topmost <see cref="UIElement"/> in the view which contains the specified point, or <c>null</c>.</returns>
        public UIElement HitTest(Double x, Double y)
        {
            return LayoutRoot.HitTest(new Point2D(x, y)) as UIElement;
        }

        /// <summary>
        /// Performs a hit test against the view at the specified point in device-independent view space.
        /// </summary>
        /// <param name="point">The point in device-independent view space to evaluate.</param>
        /// <returns>The topmost <see cref="UIElement"/> in the view which contains the specified point, or <c>null</c>.</returns>
        public UIElement HitTest(Point2D point)
        {
            return LayoutRoot.HitTest(point) as UIElement;
        }

        /// <summary>
        /// Gets the first focusable element in the view.
        /// </summary>
        /// <param name="tabStop">A value indicating whether the element must also be a tab stop.</param>
        /// <returns>The first focusable element in the view, or <c>null</c> if no such element exists.</returns>
        public UIElement GetFirstFocusableElement(Boolean tabStop)
        {
            return GetFirstFocusableElementInternal(LayoutRoot, tabStop);
        }

        /// <summary>
        /// Gets the last focusable element in the view.
        /// </summary>
        /// <param name="tabStop">A value indicating whether the element must also be a tab stop.</param>
        /// <returns>The last focusable element in the view, or <c>null</c> if no such element exists.</returns>
        public UIElement GetLastFocusableElement(Boolean tabStop)
        {
            return GetLastFocusableElementInternal(LayoutRoot, tabStop);
        }

        /// <summary>
        /// Sets the view's stylesheet.
        /// </summary>
        /// <param name="stylesheet">The view's stylesheet.</param>
        public void SetStylesheet(UvssDocument stylesheet)
        {
            this.stylesheet = stylesheet;

            if (stylesheet != null)
            {
                LoadViewResources(stylesheet);
                layoutRoot.Style(stylesheet);
            }
            else
            {
                LoadViewResources(null);
                layoutRoot.ClearStyledValues(true);
            }
        }

        /// <summary>
        /// Loads the specified resource from the global content manager.
        /// </summary>
        /// <param name="resource">The resource to load.</param>
        /// <param name="asset">The asset identifier that specifies which resource to load.</param>
        public void LoadGlobalResource<T>(FrameworkResource<T> resource, AssetID asset) where T : class
        {
            if (resource == null || GlobalContent == null)
                return;

            resource.Load(GlobalContent, asset);
        }

        /// <summary>
        /// Loads the specified resource from the local content manager.
        /// </summary>
        /// <param name="resource">The resource to load.</param>
        /// <param name="asset">The asset identifier that specifies which resource to load.</param>
        public void LoadLocalResource<T>(FrameworkResource<T> resource, AssetID asset) where T : class
        {
            if (resource == null || LocalContent == null)
                return;

            resource.Load(LocalContent, asset);
        }

        /// <summary>
        /// Loads the specified sourced image.
        /// </summary>
        /// <param name="image">The identifier of the image to load.</param>
        public void LoadImage(SourcedImage image)
        {
            if (image.Resource == null)
                return;

            switch (image.Source)
            {
                case AssetSource.Global:
                    if (GlobalContent != null)
                    {
                        image.Resource.Load(GlobalContent);
                    }
                    break;

                case AssetSource.Local:
                    if (LocalContent != null)
                    {
                        image.Resource.Load(LocalContent);
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Loads the specified sourced resource.
        /// </summary>
        /// <param name="resource">The identifier of the resource to load.</param>
        public void LoadResource<T>(SourcedResource<T> resource) where T : class
        {
            if (resource.Resource == null)
                return;

            switch (resource.Source)
            {
                case AssetSource.Global:
                    if (GlobalContent != null)
                    {
                        resource.Load(GlobalContent);
                    }
                    break;

                case AssetSource.Local:
                    if (LocalContent != null)
                    {
                        resource.Load(LocalContent);
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Searches the view's associated stylesheet for a storyboard with the specified name.
        /// </summary>
        /// <param name="name">The name of the storyboard to retrieve.</param>
        /// <returns>The <see cref="Storyboard"/> with the specified name, or <c>null</c> if the specified storyboard does not exist.</returns>
        public Storyboard FindStoryboard(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            if (Stylesheet != null)
            {
                Stylesheet.InstantiateStoryboardByName(LayoutRoot.Ultraviolet, name);
            }

            return null;
        }

        /// <summary>
        /// Gets the stylesheet that is currently applied to this view.
        /// </summary>
        public UvssDocument Stylesheet
        {
            get { return stylesheet; }
        }

        /// <summary>
        /// Gets the root element of the view's layout.
        /// </summary>
        public UIElement LayoutRoot
        {
            get { return layoutRoot; }
        }

        /// <summary>
        /// Gets the element that is currently under the mouse cursor.
        /// </summary>
        public UIElement ElementUnderMouse
        {
            get { return elementUnderMouse; }
        }

        /// <summary>
        /// Gets the element that currently has focus.
        /// </summary>
        public UIElement ElementWithFocus
        {
            get { return elementWithFocus; }
        }

        /// <summary>
        /// Gets the element that currently has mouse capture.
        /// </summary>
        public UIElement ElementWithMouseCapture
        {
            get { return elementWithMouseCapture; }
        }

        /// <summary>
        /// Gets the view's global resource collection.
        /// </summary>
        public PresentationFoundationViewResources Resources
        {
            get { return resources; }
        }
        
        /// <summary>
        /// Gets the view's element registry.
        /// </summary>
        internal UIElementRegistry ElementRegistry
        {
            get { return elementRegistry; }
        } 

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && !Ultraviolet.Disposed)
            {
                UnhookKeyboardEvents();
                UnhookMouseEvents();
            }
            base.Dispose(disposing);
        }

        /// <inheritdoc/>
        protected override void OnContentManagersChanged()
        {
            resources.Reload();
            layoutRoot.ReloadContent(true);

            base.OnContentManagersChanged();
        }

        /// <inheritdoc/>
        protected override void OnViewModelChanged()
        {            
            layoutRoot.CacheLayoutParameters();

            if (ViewModel != null)
                elementRegistry.PopulateFieldsFromRegisteredElements(ViewModel);

            base.OnViewModelChanged();
        }

        /// <inheritdoc/>
        protected override void OnViewSizeChanged()
        {
            var dipsArea = Display.PixelsToDips(Area);

            layoutRoot.Measure(dipsArea.Size);
            layoutRoot.Arrange(dipsArea);

            base.OnViewSizeChanged();
        }

        /// <summary>
        /// Recurses through the logical tree to find the first element which is focusable (and potentially, a tab stop).
        /// </summary>
        /// <param name="parent">The parent element which is being examined.</param>
        /// <param name="tabStop">A value indicating whether a matching element must be a tab stop.</param>
        /// <returns>The first element within this branch of the logical tree which meets the specified criteria.</returns>
        private UIElement GetFirstFocusableElementInternal(UIElement parent, Boolean tabStop)
        {
            if (parent.Focusable && (!tabStop || parent.IsTabStop))
                return parent;

            for (int i = 0; i < parent.VisualChildrenCount; i++)
            {
                var child = parent.GetVisualChild(i);
                var match = GetFirstFocusableElementInternal(child, tabStop);
                if (match != null)
                {
                    return match;
                }
            }

            return null;
        }

        /// <summary>
        /// Recurses through the logical tree to find the last element which is focusable (and potentially, a tab stop).
        /// </summary>
        /// <param name="parent">The parent element which is being examined.</param>
        /// <param name="tabStop">A value indicating whether a matching element must be a tab stop.</param>
        /// <returns>The last element within this branch of the logical tree which meets the specified criteria.</returns>
        private UIElement GetLastFocusableElementInternal(UIElement parent, Boolean tabStop)
        {
            for (int i = parent.VisualChildrenCount - 1; i >= 0; i--)
            {
                var child = parent.GetVisualChild(i);
                var match = GetLastFocusableElementInternal(child, tabStop);
                if (match != null)
                {
                    return match;
                }
            }

            if (parent.Focusable && (!tabStop || parent.IsTabStop))
                return parent;

            return null;
        }

        /// <summary>
        /// Loads the view's global resources from the specified stylesheet.
        /// </summary>
        /// <param name="stylesheet">The stylesheet from which to load global resources.</param>
        private void LoadViewResources(UvssDocument stylesheet)
        {
            resources.ClearStyledValues();

            if (stylesheet != null)
            {
                resources.ApplyStyles(stylesheet);
            }
        }

        /// <summary>
        /// Hooks into Ultraviolet's keyboard input events.
        /// </summary>
        private void HookKeyboardEvents()
        {
            var input = Ultraviolet.GetInput();
            if (input.IsKeyboardSupported())
            {
                var keyboard          = input.GetKeyboard();
                keyboard.KeyPressed  += keyboard_KeyPressed;
                keyboard.KeyReleased += keyboard_KeyReleased;
                keyboard.TextInput   += keyboard_TextInput;
            }
        }

        /// <summary>
        /// Hooks into Ultraviolet's mouse input events.
        /// </summary>
        private void HookMouseEvents()
        {
            var input = Ultraviolet.GetInput();
            if (input.IsMouseSupported())
            {
                var mouse             = input.GetMouse();
                mouse.Moved          += mouse_Moved;
                mouse.ButtonPressed  += mouse_ButtonPressed;
                mouse.ButtonReleased += mouse_ButtonReleased;
                mouse.Click          += mouse_Click;
                mouse.DoubleClick    += mouse_DoubleClick;
                mouse.WheelScrolled  += mouse_WheelScrolled;
            }
        }

        /// <summary>
        /// Unhooks from Ultraviolet's keyboard input events.
        /// </summary>
        private void UnhookKeyboardEvents()
        {
            var input = Ultraviolet.GetInput();
            if (input.IsKeyboardSupported())
            {
                var keyboard          = input.GetKeyboard();
                keyboard.KeyPressed  -= keyboard_KeyPressed;
                keyboard.KeyReleased -= keyboard_KeyReleased;
                keyboard.TextInput   -= keyboard_TextInput;
            }
        }

        /// <summary>
        /// Unhooks from Ultraviolet's mouse input events.
        /// </summary>
        private void UnhookMouseEvents()
        {
            var input = Ultraviolet.GetInput();
            if (input.IsMouseSupported())
            {
                var mouse             = input.GetMouse();
                mouse.Moved          -= mouse_Moved;
                mouse.ButtonPressed  -= mouse_ButtonPressed;
                mouse.ButtonReleased -= mouse_ButtonReleased;
                mouse.Click          -= mouse_Click;
                mouse.DoubleClick    -= mouse_DoubleClick;
                mouse.WheelScrolled  -= mouse_WheelScrolled;
            }
        }

        /// <summary>
        /// Handles user input by raising input messages on the elements in the view.
        /// </summary>
        private void HandleUserInput()
        {
            if (Ultraviolet.GetInput().IsKeyboardSupported())
            {
                HandleKeyboardInput();
            }
            if (Ultraviolet.GetInput().IsMouseSupported())
            {
                HandleMouseInput();
            }
        }

        /// <summary>
        /// Handles keyboard input.
        /// </summary>
        private void HandleKeyboardInput()
        {
            // TODO
        }

        /// <summary>
        /// Handles mouse input.
        /// </summary>
        private void HandleMouseInput()
        {
            UpdateElementUnderMouse();
        }

        /// <summary>
        /// Determines which element is currently under the mouse cursor.
        /// </summary>
        private void UpdateElementUnderMouse()
        {
            var mouse = Ultraviolet.GetInput().GetMouse();

            // Determine which element is currently under the mouse cursor.
            if (elementWithMouseCapture == null)
            {
                var mousePos  = mouse.Position;
                var mouseView = mouse.Window == Window ? this : null;

                elementUnderMousePrev = elementUnderMouse;
                elementUnderMouse     = (mouseView == null) ? null : mouseView.HitTestScreenPixel((Point2)mousePos);
            }

            if (elementUnderMouse != null && !elementUnderMouse.IsHitTestVisible)
                elementUnderMousePrev = elementUnderMouse;

            if (elementWithMouseCapture != null && !elementWithMouseCapture.IsHitTestVisible)
                ReleaseMouse(elementWithMouseCapture);

            // Handle mouse motion events
            if (elementUnderMouse != elementUnderMousePrev)
            {
                if (elementUnderMousePrev != null)
                {
                    var mouseLeaveHandled = false;
                    Mouse.RaiseMouseLeave(elementUnderMousePrev, mouse, ref mouseLeaveHandled);
                }

                if (elementUnderMouse != null)
                {
                    var mouseEnterHandled = false;
                    Mouse.RaiseMouseEnter(elementUnderMouse, mouse, ref mouseEnterHandled);
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="KeyboardDevice.KeyPressed"/> event.
        /// </summary>
        private void keyboard_KeyPressed(IUltravioletWindow window, KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat)
        {
            if (!Focused)
                return;

            if (elementWithFocus != null)
            {
                var keyDownHandled = false;
                Keyboard.RaisePreviewKeyDown(elementWithFocus, device, key, ctrl, alt, shift, repeat, ref keyDownHandled);
                Keyboard.RaiseKeyDown(elementWithFocus, device, key, ctrl, alt, shift, repeat, ref keyDownHandled);
            }
        }

        /// <summary>
        /// Handles the <see cref="KeyboardDevice.KeyReleased"/> event.
        /// </summary>
        private void keyboard_KeyReleased(IUltravioletWindow window, KeyboardDevice device, Key key)
        {
            if (!Focused)
                return;

            if (elementWithFocus != null)
            {
                var keyUpHandled = false;
                Keyboard.RaisePreviewKeyUp(elementWithFocus, device, key, ref keyUpHandled);
                Keyboard.RaiseKeyUp(elementWithFocus, device, key, ref keyUpHandled);
            }
        }

        /// <summary>
        /// Handles the <see cref="KeyboardDevice.TextInput"/> event.
        /// </summary>
        private void keyboard_TextInput(IUltravioletWindow window, KeyboardDevice device)
        {
            if (!Focused)
                return;

            if (elementWithFocus != null)
            {
                var textInputHandled = false;
                Keyboard.RaisePreviewTextInput(elementWithFocus, device, ref textInputHandled);
                Keyboard.RaiseTextInput(elementWithFocus, device, ref textInputHandled);
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.Moved"/> event.
        /// </summary>
        private void mouse_Moved(IUltravioletWindow window, MouseDevice device, Int32 x, Int32 y, Int32 dx, Int32 dy)
        {
            if (window != Window)
                return;

            var recipient = elementWithMouseCapture ?? elementUnderMouse;
            if (recipient != null)
            {
                var dipsX      = Display.PixelsToDips(x);
                var dipsY      = Display.PixelsToDips(y);
                var dipsDeltaX = Display.PixelsToDips(dx);
                var dipsDeltaY = Display.PixelsToDips(dy);

                var mouseMoveHandled = false;
                Mouse.RaisePreviewMouseMove(recipient, device, dipsX, dipsY, dipsDeltaX, dipsDeltaY, ref mouseMoveHandled);
                Mouse.RaiseMouseMove(recipient, device, dipsX, dipsY, dipsDeltaX, dipsDeltaY, ref mouseMoveHandled);
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.ButtonPressed"/> event.
        /// </summary>
        private void mouse_ButtonPressed(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (window != Window)
                return;

            var recipient = elementWithMouseCapture;
            if (recipient == null)
            {
                UpdateElementUnderMouse();
                recipient = elementUnderMouse;
            }

            if (recipient != elementWithFocus)
            {
                if (elementWithFocus != null)
                {
                    BlurElement(elementWithFocus);
                }
                if (recipient.Focusable)
                {
                    FocusElement(recipient);
                }
            }

            if (recipient != null)
            {
                var mouseDownHandled = false;
                Mouse.RaisePreviewMouseDown(recipient, device, button, ref mouseDownHandled);
                Mouse.RaiseMouseDown(recipient, device, button, ref mouseDownHandled);
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.ButtonReleased"/> event.
        /// </summary>
        private void mouse_ButtonReleased(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (window != Window)
                return;

            var recipient = elementWithMouseCapture ?? elementUnderMouse;
            if (recipient != null)
            {
                var mouseUpHandled = false;
                Mouse.RaisePreviewMouseUp(recipient, device, button, ref mouseUpHandled);
                Mouse.RaiseMouseUp(recipient, device, button, ref mouseUpHandled);
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.Click"/> event.
        /// </summary>
        private void mouse_Click(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (window != Window)
                return;

            var recipient = elementWithMouseCapture ?? elementUnderMouse;
            if (recipient != null)
            {
                var mouseClickHandled = false;
                Mouse.RaisePreviewMouseClick(recipient, device, button, ref mouseClickHandled);
                Mouse.RaiseMouseClick(recipient, device, button, ref mouseClickHandled);
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.DoubleClick"/> event.
        /// </summary>
        private void mouse_DoubleClick(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (window != Window)
                return;

            var recipient = elementWithMouseCapture ?? elementUnderMouse;
            if (recipient != null)
            {
                var mouseDoubleClickHandled = false;
                Mouse.RaisePreviewMouseDoubleClick(recipient, device, button, ref mouseDoubleClickHandled);
                Mouse.RaiseMouseDoubleClick(recipient, device, button, ref mouseDoubleClickHandled);
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.WheelScrolled"/> event.
        /// </summary>
        private void mouse_WheelScrolled(IUltravioletWindow window, MouseDevice device, Int32 x, Int32 y)
        {
            if (window != Window)
                return;

            var recipient = elementWithMouseCapture ?? elementUnderMouse;
            if (recipient != null)
            {
                var dipsX = Display.PixelsToDips(x);
                var dipsY = Display.PixelsToDips(y);

                var mouseWheelHandled = false;
                Mouse.RaisePreviewMouseWheel(recipient, device, dipsX, dipsY, ref mouseWheelHandled);
                Mouse.RaiseMouseWheel(recipient, device, dipsX, dipsY, ref mouseWheelHandled);
            }
        }

        // Property values.
        private readonly UIElementRegistry elementRegistry;
        private readonly PresentationFoundationViewResources resources;
        private UvssDocument stylesheet;
        private Grid layoutRoot;

        // State values.
        private readonly DrawingContext drawingContext;
        private UIElement elementUnderMousePrev;
        private UIElement elementUnderMouse;
        private UIElement elementWithMouseCapture;
        private UIElement elementWithFocus;
    }
}
