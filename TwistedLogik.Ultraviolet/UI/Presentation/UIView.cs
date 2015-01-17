using System;
using System.Xml.Linq;
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
    public sealed class UIView : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIView"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="viewModelType">The view's associated model type.</param>
        public UIView(UltravioletContext uv, Type viewModelType)
            : base(uv)
        {
            this.viewModelType = viewModelType;

            this.layoutRoot = new Grid(uv, null);
            this.layoutRoot.CacheLayoutParameters();
            this.layoutRoot.InvalidateMeasure();

            HookKeyboardEvents();
            HookMouseEvents();
        }

        /// <summary>
        /// Loads an instance of <see cref="UIView"/> from an XML document.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="xml">The <see cref="XDocument"/> from which to load the view.</param>
        /// <returns>The <see cref="UIView"/> that was loaded from the specified XML document.</returns>
        public static UIView Load(UltravioletContext uv, XDocument xml)
        {
            Contract.Require(uv, "uv");
            Contract.Require(xml, "xml");

            return UvmlLoader.Load(uv, xml.Root);
        }

        /// <summary>
        /// Loads an instance of the <see cref="UIView"/> from an XML node.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="xml">The <see cref="XElement"/> from which to load the view.</param>
        /// <returns>The <see cref="UIView"/> that was loaded from the specified XML element.</returns>
        public static UIView Load(UltravioletContext uv, XElement xml)
        {
            Contract.Require(uv, "uv");
            Contract.Require(xml, "xml");

            return UvmlLoader.Load(uv, xml);
        }

        /// <summary>
        /// Cleans up the view and its elements.
        /// </summary>
        /// <remarks>The <see cref="Cleanup()"/> method releases any internal resources which the view
        /// and its elements may be holding, allowing them to be reused by the Presentation Framework.
        /// The view remains usable after a call to this method, but certain state values (such as
        /// animations, dependency property values, etc.) may be reset.</remarks>
        public void Cleanup()
        {
            // TODO
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
        /// Draws the view and all of its contained elements.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The sprite batch with which to draw the view.</param>
        public void Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            if (window == null)
                return;

            layoutRoot.Draw(time, spriteBatch, 1.0f);
        }

        /// <summary>
        /// Updates the view's state and the state of its contained elements.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            if (window == null)
                return;

            HandleUserInput();

            layoutRoot.Update(time);
        }

        /// <summary>
        /// Gives input focus to the view.
        /// </summary>
        public void FocusView()
        {
            focused = true;
        }

        /// <summary>
        /// Removes input focus from the view.
        /// </summary>
        public void BlurView()
        {
            focused = false;
        }

        /// <summary>
        /// Grants input focus within this view to the specified element.
        /// </summary>
        /// <param name="element">The element to which to grant input focus.</param>
        public void Focus(UIElement element)
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
        public void Blur(UIElement element)
        {
            Contract.Require(element, "element");

            if (elementWithFocus != element)
                return;

            elementWithFocus.OnBlurred();
            elementWithFocus = null;
        }

        /// <summary>
        /// Gets a value indicating whether the specified element has input focus.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element has input focus; otherwise, <c>false</c>.</returns>
        public Boolean HasFocus(UIElement element)
        {
            return element == elementWithFocus;
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
        /// Gets a value indicating whether the specified element has mouse capture.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element has mouse capture; otherwise, <c>false</c>.</returns>
        public Boolean HasMouseCapture(UIElement element)
        {
            return element == elementWithMouseCapture;
        }

        /// <summary>
        /// Converts a position in screen space to a position in view space.
        /// </summary>
        /// <param name="x">The x-coordinate of the screen space position to convert.</param>
        /// <param name="y">The y-coordinate of the screen space position to convert.</param>
        /// <returns>The converted view space position.</returns>
        public Vector2 ScreenPositionToViewPosition(Int32 x, Int32 y)
        {
            return new Vector2(x - Area.X, y - Area.Y);
        }

        /// <summary>
        /// Converts a position in screen space to a position in view space.
        /// </summary>
        /// <param name="position">The screen space position to convert.</param>
        /// <returns>The converted view space position.</returns>
        public Vector2 ScreenPositionToViewPosition(Vector2 position)
        {
            return ScreenPositionToViewPosition((Int32)position.X, (Int32)position.Y);
        }

        /// <summary>
        /// Converts a position in view space to a position in screen space.
        /// </summary>
        /// <param name="x">The x-coordinate of the view space position to convert.</param>
        /// <param name="y">The y-coordinate of the view space position to convert.</param>
        /// <returns>The converted screen space position.</returns>
        public Vector2 ViewPositionToScreenPosition(Int32 x, Int32 y)
        {
            return new Vector2(x + Area.X, y + Area.Y);
        }

        /// <summary>
        /// Converts a position in view space to a position in screen space.
        /// </summary>
        /// <param name="position">The view space position to convert.</param>
        /// <returns>The converted screen space position.</returns>
        public Vector2 ViewPositionToScreenPosition(Vector2 position)
        {
            return ViewPositionToScreenPosition((Int32)position.X, (Int32)position.Y);
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
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="IsHitTestVisible"/> property.</param>
        /// <returns>The element at the specified pixel coordinates, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementAtScreenPixel(Int32 x, Int32 y, Boolean isHitTest)
        {
            return LayoutRoot.GetElementAtPixel(x - area.X, y - area.Y, isHitTest);
        }

        /// <summary>
        /// Gets the element at the specified pixel coordinates relative to screen space.
        /// </summary>
        /// <param name="pt">The screen-relative coordinates of the pixel to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="IsHitTestVisible"/> property.</param>
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
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="IsHitTestVisible"/> property.</param>
        /// <returns>The element at the specified pixel coordinates, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementAtPixel(Int32 x, Int32 y, Boolean isHitTest)
        {
            return LayoutRoot.GetElementAtPixel(x, y, isHitTest);
        }

        /// <summary>
        /// Gets the element at the specified pixel coordinates relative to this view's bounds.
        /// </summary>
        /// <param name="pt">The view-relative coordinates of the pixel to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="IsHitTestVisible"/> property.</param>
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
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="IsHitTestVisible"/> property.</param>
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
        /// value of the <see cref="IsHitTestVisible"/> property.</param>
        /// <returns>The element at the specified coordinates, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementAtPoint(Point2D pt, Boolean isHitTest)
        {
            return LayoutRoot.GetElementAtPoint(pt, isHitTest);
        }

        /// <summary>
        /// Sets the content managers used to load UI assets.
        /// </summary>
        /// <param name="global">The content manager used to load globally-sourced assets.</param>
        /// <param name="local">The content manager used to load locally-sourced assets.</param>
        public void SetContentManagers(ContentManager global, ContentManager local)
        {
            this.globalContent = global;
            this.localContent  = local;

            layoutRoot.ReloadContent(true);
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
                // TODO LayoutRoot.ApplyStyles(stylesheet);
            }
            else
            {
                // TODO LayoutRoot.ClearStyledValuesRecursive();
            }
        }

        /// <summary>
        /// Sets the view's associated view model.
        /// </summary>
        /// <param name="viewModel">The view's associated view model.</param>
        public void SetViewModel(Object viewModel)
        {
            if (viewModel != null && viewModel.GetType() != viewModelType)
                throw new ArgumentException(UltravioletStrings.IncompatibleViewModel.Format(viewModelType));

            this.viewModel = viewModel;

            layoutRoot.CacheLayoutParameters();
        }

        /// <summary>
        /// Sets the view's area on the screen.
        /// </summary>
        /// <param name="window">The window that contains the view.</param>
        /// <param name="area">The area on the screen that is occupied by the view.</param>
        public void SetViewArea(IUltravioletWindow window, Rectangle area)
        {
            Contract.Require(window, "window");

            this.window = window;

            var newSize = false;

            if (this.area.Width != area.Width || this.area.Height != area.Height)
                newSize = true;

            this.area = area;

            if (newSize)
            {
                var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;

                var dipsX      = display.PixelsToDips(area.X);
                var dipsY      = display.PixelsToDips(area.Y);
                var dipsWidth  = display.PixelsToDips(area.Width);
                var dipsHeight = display.PixelsToDips(area.Height);

                layoutRoot.Measure(new Size2D(dipsWidth, dipsHeight));
                layoutRoot.Arrange(new RectangleD(0, 0, dipsWidth, dipsHeight));
                layoutRoot.Position(new Point2D(dipsX, dipsY));
            }
        }

        /// <summary>
        /// Loads the specified asset from the global content manager.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <param name="asset">The identifier of the asset to load.</param>
        /// <returns>The asset that was loaded.</returns>
        public TOutput LoadGlobalContent<TOutput>(AssetID asset)
        {
            if (!asset.IsValid)
                return default(TOutput);

            return (globalContent == null) ? default(TOutput) : globalContent.Load<TOutput>(asset);
        }

        /// <summary>
        /// Loads the specified asset from the local content manager.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <param name="asset">The identifier of the asset to load.</param>
        /// <returns>The asset that was loaded.</returns>
        public TOutput LoadLocalContent<TOutput>(AssetID asset)
        {
            if (!asset.IsValid)
                return default(TOutput);

            return (localContent == null) ? default(TOutput) : localContent.Load<TOutput>(asset);
        }

        /// <summary>
        /// Loads the specified sourced asset.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <param name="asset">The identifier of the asset to load.</param>
        /// <returns>The asset that was loaded.</returns>
        public TOutput LoadContent<TOutput>(SourcedVal<AssetID> asset)
        {
            if (!asset.Value.IsValid)
                return default(TOutput);

            switch (asset.Source)
            {
                case AssetSource.Global:
                    return (globalContent == null) ? default(TOutput) : globalContent.Load<TOutput>(asset.Value);
                
                case AssetSource.Local:
                    return (localContent == null) ? default(TOutput) : localContent.Load<TOutput>(asset.Value);

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Loads the specified image from the global content manager.
        /// </summary>
        /// <param name="image">The identifier of the image to load.</param>
        public void LoadGlobalContent<T>(T image) where T : Image
        {
            if (image == null || globalContent == null)
                return;

            image.Load(globalContent);
        }

        /// <summary>
        /// Loads the specified image from the local content manager.
        /// </summary>
        /// <param name="image">The identifier of the image to load.</param>
        public void LoadLocalContent<T>(T image) where T : Image
        {
            if (image == null || localContent == null)
                return;

            image.Load(localContent);
        }

        /// <summary>
        /// Loads the specified sourced image.
        /// </summary>
        /// <param name="image">The identifier of the image to load.</param>
        public void LoadContent<T>(SourcedRef<T> image) where T : Image
        {
            if (image.Value == null)
                return;

            switch (image.Source)
            {
                case AssetSource.Global:
                    if (globalContent != null)
                    {
                        image.Value.Load(globalContent);
                    }
                    break;

                case AssetSource.Local:
                    if (localContent != null)
                    {
                        image.Value.Load(localContent);
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
                // TODO return Stylesheet.InstantiateStoryboardByName(LayoutRoot.Ultraviolet, name);
            }

            return null;
        }

        /// <summary>
        /// Gets the content manager used to load globally-sourced assets.
        /// </summary>
        public ContentManager GlobalContent
        {
            get { return globalContent; }
        }

        /// <summary>
        /// Gets the content manager used to load locally-sourced assets.
        /// </summary>
        public ContentManager LocalContent
        {
            get { return localContent; }
        }

        /// <summary>
        /// Gets the stylesheet that is currently applied to this view.
        /// </summary>
        public UvssDocument Stylesheet
        {
            get { return stylesheet; }
        }

        /// <summary>
        /// Gets the view's view model.
        /// </summary>
        public Object ViewModel
        {
            get { return viewModel; }
        }

        /// <summary>
        /// Gets the type of view model expected by this view.
        /// </summary>
        public Type ViewModelType
        {
            get { return viewModelType; }
        }

        /// <summary>
        /// Gets the actual type of the view's current view model.
        /// </summary>
        public Type ViewModelActualType
        {
            get { return viewModel == null ? null : viewModel.GetType(); }
        }

        /// <summary>
        /// Gets the area on the screen that the UI view occupies.
        /// </summary>
        public Rectangle Area
        {
            get { return area; }
        }

        /// <summary>
        /// Gets the x-coordinate of the view's top left corner.
        /// </summary>
        public Int32 X
        {
            get { return area.X; }
        }

        /// <summary>
        /// Gets the y-coordinate of the view's top left corner.
        /// </summary>
        public Int32 Y
        {
            get { return area.Y; }
        }

        /// <summary>
        /// Gets the view's width on the screen.
        /// </summary>
        public Int32 Width
        {
            get { return area.Width; }
        }

        /// <summary>
        /// Gets the view's height on the screen.
        /// </summary>
        public Int32 Height
        {
            get { return area.Height; }
        }

        /// <summary>
        /// Gets a value indicating whether the view has input focus.
        /// </summary>
        public Boolean Focused
        {
            get { return focused; }
        }

        /// <summary>
        /// Gets the root element of the view's layout.
        /// </summary>
        public Grid LayoutRoot
        {
            get { return layoutRoot; }
        }

        /// <summary>
        /// Gets the window that contains the view.
        /// </summary>
        public IUltravioletWindow Window
        {
            get { return window; }
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
            if (!focused)
                return;

            if (elementWithFocus != null)
                elementWithFocus.OnKeyPressed(device, key, ctrl, alt, shift, repeat);
        }

        /// <summary>
        /// Handles the <see cref="KeyboardDevice.KeyReleased"/> event.
        /// </summary>
        private void keyboard_KeyReleased(IUltravioletWindow window, KeyboardDevice device, Key key)
        {
            if (!focused)
                return;

            if (elementWithFocus != null)
                elementWithFocus.OnKeyReleased(device, key);
        }

        /// <summary>
        /// Handles the <see cref="KeyboardDevice.TextInput"/> event.
        /// </summary>
        private void keyboard_TextInput(IUltravioletWindow window, KeyboardDevice device)
        {
            if (!focused)
                return;

            if (elementWithFocus != null)
                elementWithFocus.OnTextInput(device);
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
                recipient.OnMouseMotion(device, x, y, dx, dy);
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
                    Blur(elementWithFocus);
                }
                if (recipient.Focusable)
                {
                    Focus(recipient);
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
        private readonly UIElementRegistry elementRegistry = new UIElementRegistry();
        private ContentManager globalContent;
        private ContentManager localContent;
        private UvssDocument stylesheet;
        private Object viewModel;
        private readonly Type viewModelType;
        private Rectangle area;
        private Boolean focused;
        private Grid layoutRoot;
        private IUltravioletWindow window;

        // State values.
        private UIElement elementUnderMousePrev;
        private UIElement elementUnderMouse;
        private UIElement elementWithMouseCapture;
        private UIElement elementWithFocus;
    }
}
