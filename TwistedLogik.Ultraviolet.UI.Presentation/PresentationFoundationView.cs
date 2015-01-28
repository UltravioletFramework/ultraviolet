using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Elements;
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
                elementWithFocus.OnBlurred();

            elementWithFocus = element;
            elementWithFocus.OnFocused();
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

            elementWithFocus.OnBlurred();
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
                elementWithMouseCapture.OnLostMouseCapture();

            elementWithMouseCapture = element;
            elementWithMouseCapture.OnGainedMouseCapture();
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

            elementWithMouseCapture.OnLostMouseCapture();
            elementWithMouseCapture = null;
        }

        /// <summary>
        /// Gets the element within the view which has the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the element to retrieve.</param>
        /// <returns>The element with the specified identifier, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementByID(String id)
        {
            Contract.RequireNotEmpty(id, "id");

            return elementRegistry.GetElementByID(id);
        }

        /// <summary>
        /// Gets the element at the specified pixel coordinates relative to screen space.
        /// </summary>
        /// <param name="x">The screen-relative x-coordinate of the pixel to evaluate.</param>
        /// <param name="y">The screen-relative y-coordinate of the pixel to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="UIElement.IsHitTestVisible"/> property.</param>
        /// <returns>The element at the specified pixel coordinates, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementAtScreenPixel(Int32 x, Int32 y, Boolean isHitTest)
        {
            return LayoutRoot.GetElementAtPixel(x - Area.X, y - Area.Y, isHitTest);
        }

        /// <summary>
        /// Gets the element at the specified pixel coordinates relative to screen space.
        /// </summary>
        /// <param name="pt">The screen-relative coordinates of the pixel to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="UIElement.IsHitTestVisible"/> property.</param>
        /// <returns>The element at the specified pixel coordinates, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementAtScreenPixel(Point2 pt, Boolean isHitTest)
        {
            return GetElementAtScreenPixel(pt.X, pt.Y, isHitTest);
        }

        /// <summary>
        /// Gets the element at the specified pixel coordinates relative to this view's bounds.
        /// </summary>
        /// <param name="x">The view-relative x-coordinate of the pixel to evaluate.</param>
        /// <param name="y">The view-relative y-coordinate of the pixel to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="UIElement.IsHitTestVisible"/> property.</param>
        /// <returns>The element at the specified pixel coordinates, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementAtPixel(Int32 x, Int32 y, Boolean isHitTest)
        {
            return LayoutRoot.GetElementAtPixel(x, y, isHitTest);
        }

        /// <summary>
        /// Gets the element at the specified pixel coordinates relative to this view's bounds.
        /// </summary>
        /// <param name="pt">The view-relative coordinates of the pixel to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="UIElement.IsHitTestVisible"/> property.</param>
        /// <returns>The element at the specified pixel coordinates, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementAtPixel(Point2 pt, Boolean isHitTest)
        {
            return LayoutRoot.GetElementAtPixel(pt, isHitTest);
        }

        /// <summary>
        /// Gets the element at the specified device-independent coordinates relative to this view's bounds.
        /// </summary>
        /// <param name="x">The view-relative x-coordinate of the point to evaluate.</param>
        /// <param name="y">The view-relative y-coordinate of the point to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="UIElement.IsHitTestVisible"/> property.</param>
        /// <returns>The element at the specified coordinates, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementAtPoint(Double x, Double y, Boolean isHitTest)
        {
            return LayoutRoot.GetElementAtPoint(x, y, isHitTest);
        }

        /// <summary>
        /// Gets the element at the specified device-independent coordinates relative to this view's bounds.
        /// </summary>
        /// <param name="pt">The view-relative coordinates of the point to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the
        /// value of the <see cref="UIElement.IsHitTestVisible"/> property.</param>
        /// <returns>The element at the specified coordinates, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementAtPoint(Point2D pt, Boolean isHitTest)
        {
            return LayoutRoot.GetElementAtPoint(pt, isHitTest);
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

            base.OnViewModelChanged();
        }

        /// <inheritdoc/>
        protected override void OnViewSizeChanged()
        {
            var dipsArea = Display.PixelsToDips(Area);

            layoutRoot.Measure(dipsArea.Size);
            layoutRoot.Arrange(dipsArea);
            layoutRoot.Position(dipsArea.Location);

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

            for (int i = 0; i < parent.LogicalChildren; i++)
            {
                var child = parent.GetLogicalChild(i);
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
            for (int i = parent.LogicalChildren - 1; i >= 0; i--)
            {
                var child = parent.GetLogicalChild(i);
                var match = GetFirstFocusableElementInternal(child, tabStop);
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
                elementUnderMouse     = (mouseView == null) ? null : mouseView.GetElementAtScreenPixel((Point2)mousePos, true);
            }

            if (elementUnderMouse != null && !elementUnderMouse.IsHitTestVisible)
                elementUnderMousePrev = elementUnderMouse;

            if (elementWithMouseCapture != null && !elementWithMouseCapture.IsHitTestVisible)
                ReleaseMouse(elementWithMouseCapture);

            // Handle mouse motion events
            if (elementUnderMouse != elementUnderMousePrev)
            {
                if (elementUnderMousePrev != null)
                    elementUnderMousePrev.OnMouseLeave(mouse);

                if (elementUnderMouse != null)
                {
                    elementUnderMouse.OnMouseEnter(mouse);
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
                elementWithFocus.OnKeyPressed(device, key, ctrl, alt, shift, repeat);
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
                elementWithFocus.OnKeyReleased(device, key);
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
                elementWithFocus.OnTextInput(device);
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

                recipient.OnMouseMotion(device, dipsX, dipsY, dipsDeltaX, dipsDeltaY);
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
                recipient.OnMouseButtonPressed(device, button);
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
                recipient.OnMouseButtonReleased(device, button);
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
