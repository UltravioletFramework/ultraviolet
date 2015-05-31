using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
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
            this.combinedStyleSheet = new UvssDocument(null, null);

            this.namescope      = new Namescope();
            this.resources      = new PresentationFoundationViewResources(this);
            this.drawingContext = new DrawingContext(this);

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

            var uvss    = String.Join(Environment.NewLine, uiPanelDefinition.StyleSheets);
            var uvssdoc = UvssDocument.Parse(uvss);

            view.SetStyleSheet(uvssdoc);

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

            popupQueue.Clear();

            if (Window == null)
                return;

            drawingContext.Reset();
            drawingContext.SpriteBatch = spriteBatch;

            var fe = layoutRoot as FrameworkElement;
            if (fe != null)
                fe.EnsureIsLoaded(true);

            layoutRoot.Draw(time, drawingContext);
            popupQueue.Draw(time, drawingContext);

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
        /// Performs the specified action on all objects within the view's visual tree
        /// which match the specified UVSS selector.
        /// </summary>
        /// <param name="selector">The UVSS selector that specifies which objects should be targeted by the action.</param>
        /// <param name="state">A state value which is passed to the specified action.</param>
        /// <param name="action">The action to perform on the selected objects.</param>
        public void Select(UvssSelector selector, Object state, Action<UIElement, Object> action)
        {
            Contract.Require(selector, "selector");
            Contract.Require(action, "action");

            SelectInternal(layoutRoot, selector, state, action);
        }

        /// <summary>
        /// Performs the specified action on all objects within the view's visual tree
        /// which match the specified UVSS selector.
        /// </summary>
        /// <param name="root">The root element at which to begin evaluation, or <c>null</c> to begin at the layout root.</param>
        /// <param name="selector">The UVSS selector that specifies which objects should be targeted by the action.</param>
        /// <param name="state">A state value which is passed to the specified action.</param>
        /// <param name="action">The action to perform on the selected objects.</param>
        public void Select(UIElement root, UvssSelector selector, Object state, Action<UIElement, Object> action)
        {
            Contract.Require(selector, "selector");
            Contract.Require(action, "action");

            SelectInternal(root ?? layoutRoot, selector, state, action);
        }

        /// <summary>
        /// Invalidates the styling state of the view's layout root.
        /// </summary>
        public void InvalidateStyle()
        {
            layoutRoot.InvalidateStyle(true);
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
        public void FocusElement(IInputElement element)
        {
            Contract.Require(element, "element");

            if (elementWithFocus == element || !element.Focusable)
                return;

            if (elementWithFocus != null)
            {
                BlurElement(elementWithFocus);
            }

            elementWithFocus = element;
            
            SetIsKeyboardFocusWithin(elementWithFocus, true);

            var dobj = elementWithFocus as DependencyObject;
            if (dobj != null)
            {
                var gotFocusData = new RoutedEventData(dobj);
                Keyboard.RaiseGotKeyboardFocus(dobj, ref gotFocusData);
            }
        }

        /// <summary>
        /// Removes input focus within this view from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove input focus.</param>
        public void BlurElement(IInputElement element)
        {
            Contract.Require(element, "element");

            if (elementWithFocus != element)
                return;

            var elementWithFocusOld = elementWithFocus;
            elementWithFocus = null;

            SetIsKeyboardFocusWithin(elementWithFocusOld, false);

            var dobj = elementWithFocusOld as DependencyObject;
            if (dobj != null)
            {
                var lostFocusData = new RoutedEventData(dobj);
                Keyboard.RaiseLostKeyboardFocus(dobj, ref lostFocusData);
            }
        }

        /// <summary>
        /// Releases the mouse from the element that is currently capturing it.
        /// </summary>
        public void ReleaseMouse()
        {
            if (elementWithMouseCapture == null)
                return;

            var elementHadMouseCapture = elementWithMouseCapture;
            elementWithMouseCapture    = null;
            mouseCaptureMode           = CaptureMode.None;

            var uiElement = elementHadMouseCapture as UIElement;
            if (uiElement != null)
            {
                uiElement.IsMouseCaptured = false;
                UpdateIsMouseCaptureWithin(uiElement, false);
                UpdateIsMouseOver(uiElement);
            }

            var dobj = elementHadMouseCapture as DependencyObject;
            if (dobj != null)
            {
                var lostMouseCaptureData = new RoutedEventData(dobj);
                Mouse.RaiseLostMouseCapture(dobj, ref lostMouseCaptureData);
            }
        }

        /// <summary>
        /// Assigns mouse capture to the specified element.
        /// </summary>
        /// <param name="element">The element to which to assign mouse capture.</param>
        /// <param name="mode">The mouse capture mode to apply.</param>
        /// <returns><c>true</c> if the mouse was successfully captured; otherwise, <c>false</c>.</returns>
        public Boolean CaptureMouse(IInputElement element, CaptureMode mode)
        {
            Contract.Require(element, "element");

            if (element == null || mode == CaptureMode.None)
            {
                element = null;
                mode    = CaptureMode.None;
            }

            if (elementWithMouseCapture == element)
                return true;

            if (element != null && !IsElementValidForInput(element))
                return false;

            if (elementWithMouseCapture != null)
            {
                ReleaseMouse();
            }

            elementWithMouseCapture = element;
            mouseCaptureMode        = mode;

            var uiElement = elementWithMouseCapture as UIElement;
            if (uiElement != null)
            {
                uiElement.IsMouseCaptured = true;
                UpdateIsMouseCaptureWithin(uiElement, true);
                UpdateIsMouseOver(uiElement);
            }

            var dobj = elementWithMouseCapture as DependencyObject;
            if (dobj != null)
            {
                var gotMouseCaptureData = new RoutedEventData(dobj);
                Mouse.RaiseGotMouseCapture(dobj, ref gotMouseCaptureData);
            }

            return true;
        }

        /// <summary>
        /// Gets the element within the view which has the specified identifying name.
        /// </summary>
        /// <param name="name">The identifying name of the element to retrieve.</param>
        /// <returns>The element with the specified identifying name, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementByName(String name)
        {
            Contract.RequireNotEmpty(name, "id");

            return namescope.GetElementByName(name);
        }

        /// <summary>
        /// Performs a hit test against the view at the specified point in screen space.
        /// </summary>
        /// <param name="x">The x-coordinate in screen space to evaluate.</param>
        /// <param name="y">The y-coordinate in screen space to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> in the view which contains the specified point, or <c>null</c>.</returns>
        public Visual HitTestScreenPixel(Int32 x, Int32 y)
        {
            var dipsX = Display.PixelsToDips(x - Area.X);
            var dipsY = Display.PixelsToDips(y - Area.Y);

            var popup = default(Popup);

            return HitTestInternal(new Point2D(dipsX, dipsY), out popup);
        }

        /// <summary>
        /// Performs a hit test against the view at the specified point in screen space.
        /// </summary>
        /// <param name="point">The point in screen space to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> in the view which contains the specified point, or <c>null</c>.</returns>
        public Visual HitTestScreenPixel(Point2 point)
        {
            var dipsPoint = Display.PixelsToDips(point - Area.Location);

            var popup = default(Popup);

            return HitTestInternal(dipsPoint, out popup);
        }

        /// <summary>
        /// Performs a hit test against the view at the specified point in view-relative screen space.
        /// </summary>
        /// <param name="x">The x-coordinate in view-relative screen space to evaluate.</param>
        /// <param name="y">The y-coordinate in view-relative screen space to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> in the view which contains the specified point, or <c>null</c>.</returns>
        public Visual HitTestPixel(Int32 x, Int32 y)
        {
            var dipsX = Display.PixelsToDips(x);
            var dipsY = Display.PixelsToDips(y);

            var popup = default(Popup);

            return HitTestInternal(new Point2D(dipsX, dipsY), out popup);
        }

        /// <summary>
        /// Performs a hit test against the view at the specified point in view-relative screen space.
        /// </summary>
        /// <param name="point">The point in view-relative screen space to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> in the view which contains the specified point, or <c>null</c>.</returns>
        public Visual HitTestPixel(Point2 point)
        {
            var dipsPoint = Display.PixelsToDips(point - Area.Location);

            var popup = default(Popup);

            return HitTestInternal(dipsPoint, out popup);
        }

        /// <summary>
        /// Performs a hit test against the view at the specified point in device-independent view space.
        /// </summary>
        /// <param name="x">The x-coordinate in device-independent view space to evaluate.</param>
        /// <param name="y">The y-coordinate in device-independent view space to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> in the view which contains the specified point, or <c>null</c>.</returns>
        public Visual HitTest(Double x, Double y)
        {
            var popup = default(Popup);

            return HitTestInternal(new Point2D(x, y), out popup);
        }

        /// <summary>
        /// Performs a hit test against the view at the specified point in device-independent view space.
        /// </summary>
        /// <param name="point">The point in device-independent view space to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> in the view which contains the specified point, or <c>null</c>.</returns>
        public Visual HitTest(Point2D point)
        {
            var popup = default(Popup);

            return HitTestInternal(point, out popup);
        }

        /// <summary>
        /// Sets the view's style sheet.
        /// </summary>
        /// <param name="styleSheet">The view's style sheet.</param>
        public void SetStyleSheet(UvssDocument styleSheet)
        {
            HookOnGlobalStyleSheetChanged();

            this.localStyleSheet = styleSheet;
            UpdateCombinedStyleSheet();

            LoadViewResources(combinedStyleSheet);

            layoutRoot.InvalidateStyle(true);
            layoutRoot.Style(combinedStyleSheet);
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
        /// Searches the view's associated style sheet for a storyboard with the specified name.
        /// </summary>
        /// <param name="name">The name of the storyboard to retrieve.</param>
        /// <returns>The <see cref="Storyboard"/> with the specified name, or <c>null</c> if the specified storyboard does not exist.</returns>
        public Storyboard FindStoryboard(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            if (StyleSheet != null)
            {
                return StyleSheet.InstantiateStoryboardByName(LayoutRoot.Ultraviolet, name);
            }

            return null;
        }

        /// <summary>
        /// Gets the style sheet that is currently applied to this view.
        /// </summary>
        public UvssDocument StyleSheet
        {
            get { return combinedStyleSheet; }
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
        public IInputElement ElementUnderMouse
        {
            get { return elementUnderMouse; }
        }

        /// <summary>
        /// Gets the element that currently has focus.
        /// </summary>
        public IInputElement ElementWithFocus
        {
            get { return elementWithFocus; }
        }

        /// <summary>
        /// Gets the element that currently has mouse capture.
        /// </summary>
        public IInputElement ElementWithMouseCapture
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
        /// Gets the namescope for the view layout.
        /// </summary>
        internal Namescope Namescope
        {
            get { return namescope; }
        }

        /// <summary>
        /// Gets the view's popup queue.
        /// </summary>
        internal PopupQueue Popups
        {
            get { return popupQueue; }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && !Ultraviolet.Disposed)
            {
                UnhookGlobalStyleSheetChanged();
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
                namescope.PopulateFieldsFromRegisteredElements(ViewModel);

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
        /// Performs a hit test against the layout root and any active popup windows.
        /// </summary>
        /// <param name="point">The point in device-independent screen space to test.</param>
        /// <param name="popup">The popup that contains the resulting visual.</param>
        /// <returns>The topmost <see cref="Visual"/> that contains the specified point, 
        /// or <c>null</c> if none of the items in the layout contain the point.</returns>
        private Visual HitTestInternal(Point2D point, out Popup popup)
        {
            var popupMatch = popupQueue.HitTest(point, out popup);
            if (popupMatch != null)
            {
                return popupMatch;
            }

            popup = null;
            return LayoutRoot.HitTest(point);
        }

        /// <summary>
        /// Recursively performs the specified action on all objects within the view's visual tree
        /// which match the specified UVSS selector.
        /// </summary>
        private void SelectInternal(UIElement element, UvssSelector selector, Object state, Action<UIElement, Object> action)
        {
            if (selector.MatchesElement(element))
                action(element, state);

            var children = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < children; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as UIElement;
                if (child == null)
                    continue;

                Select(child, selector, state, action);
            }
        }

        /// <summary>
        /// Updates the view's combined style sheet, which includes both UPF's global styles and the view's local styles.
        /// </summary>
        private void UpdateCombinedStyleSheet()
        {
            this.combinedStyleSheet.Clear();

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            if (upf.GlobalStyleSheet != null)
            {
                this.combinedStyleSheet.Append(upf.GlobalStyleSheet);
            }

            if (this.localStyleSheet != null)
            {
                this.combinedStyleSheet.Append(this.localStyleSheet);
            }
        }

        /// <summary>
        /// Loads the view's global resources from the specified style sheet.
        /// </summary>
        /// <param name="styleSheet">The style sheet from which to load global resources.</param>
        private void LoadViewResources(UvssDocument styleSheet)
        {
            resources.ClearStyledValues();

            if (styleSheet != null)
            {
                resources.ApplyStyles(styleSheet);
            }
        }

        /// <summary>
        /// Hooks into the <see cref="PresentationFoundation.GlobalStyleSheetChanged"/> event.
        /// </summary>
        private void HookOnGlobalStyleSheetChanged()
        {
            if (hookedGlobalStyleSheetChanged)
                return;

            hookedGlobalStyleSheetChanged = true;

            Ultraviolet.GetUI().GetPresentationFoundation().GlobalStyleSheetChanged += PresentationFoundationView_GlobalStyleSheetChanged;
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
        /// Unhooks from the <see cref="PresentationFoundation.GlobalStyleSheetChanged"/> event.
        /// </summary>
        private void UnhookGlobalStyleSheetChanged()
        {
            if (!hookedGlobalStyleSheetChanged)
                return;

            Ultraviolet.GetUI().GetPresentationFoundation().GlobalStyleSheetChanged -= PresentationFoundationView_GlobalStyleSheetChanged;

            hookedGlobalStyleSheetChanged = false;
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
        /// Updates the value of the <see cref="UIElement.IsMouseOver"/> property for ancestors
        /// of the specified element.
        /// </summary>
        /// <param name="root">The element to update.</param>
        /// <param name="unset">A value indicating whether to forcibly unset the property value.</param>
        private void UpdateIsMouseOver(UIElement root, Boolean unset = false)
        {
            if (root == null)
                return;

            var mouse = Ultraviolet.GetInput().GetMouse();
            if (mouse == null)
                return;

            isMouseOverSet.Clear();

            var elementAtMouse = (DependencyObject)HitTestScreenPixel((Point2)mouse.Position);
            while (elementAtMouse != null)
            {
                isMouseOverSet.Add(elementAtMouse);
                elementAtMouse = VisualTreeHelper.GetParent(elementAtMouse);
            }

            var current = root as DependencyObject;
            while (current != null)
            {
                var uiElement = current as UIElement;
                if (uiElement != null)
                {
                    var oldValue = uiElement.IsMouseOver;
                    var newValue = isMouseOverSet.Contains(uiElement);
                    if (oldValue != newValue)
                    {
                        uiElement.IsMouseOver = newValue;
                        if (newValue)
                        {
                            var dobj = uiElement as DependencyObject;
                            if (dobj != null)
                            {
                                var mouseEnterData = new RoutedEventData(dobj);
                                Mouse.RaiseMouseEnter(dobj, mouse, ref mouseEnterData);
                            }
                        }
                        else
                        {
                            var dobj = uiElement as DependencyObject;
                            if (dobj != null)
                            {
                                var mouseLeaveData = new RoutedEventData(dobj);
                                Mouse.RaiseMouseLeave(dobj, mouse, ref mouseLeaveData);
                            }
                        }
                    }
                }
                current = VisualTreeHelper.GetParent(current);
            }

            isMouseOverSet.Clear();
        }

        /// <summary>
        /// Updates the value of the <see cref="UIElement.IsMouseCaptureWithin"/> property for ancestors
        /// of the specified element.
        /// </summary>
        /// <param name="root">The element to update.</param>
        /// <param name="value">The value to set on the specified tree.</param>
        private void UpdateIsMouseCaptureWithin(UIElement root, Boolean value)
        {
            if (root == null)
                return;

            var current = root as DependencyObject;
            while (current != null)
            {
                var uiElement = current as UIElement;
                if (uiElement != null)
                {
                    uiElement.IsMouseCaptureWithin = value;
                }
                current = VisualTreeHelper.GetParent(current);
            }
        }

        /// <summary>
        /// Determines which element is currently under the mouse cursor.
        /// </summary>
        private void UpdateElementUnderMouse()
        {
            var mouse = Ultraviolet.GetInput().GetMouse();

            // Determine which element is currently under the mouse cursor.
            var mousePos  = mouse.Position;
            var mouseView = mouse.Window == Window ? this : null;

            elementUnderMousePopupPrev = elementUnderMousePopup;
            elementUnderMousePrev      = elementUnderMouse;
            elementUnderMouse          = (mouseView == null) ? null : mouseView.HitTestInternal((Point2)mousePos, out elementUnderMousePopup) as UIElement;
            elementUnderMouse          = RedirectMouseInput(elementUnderMouse);

            if (!IsElementValidForInput(elementUnderMouse))
                elementUnderMouse = null;

            if (mouseCaptureMode != CaptureMode.None && !IsElementValidForInput(elementWithMouseCapture))
                ReleaseMouse();

            // Handle mouse motion events
            if (elementUnderMouse != elementUnderMousePrev)
            {
                UpdateIsMouseOver(elementUnderMousePrev as UIElement, elementUnderMousePopup != elementUnderMousePopupPrev);

                if (elementUnderMousePrev != null)
                {
                    var uiElement = elementUnderMousePrev as UIElement;
                    if (uiElement != null)
                        uiElement.IsMouseDirectlyOver = false;
                }

                if (elementUnderMouse != null)
                {
                    var uiElement = elementUnderMouse as UIElement;
                    if (uiElement != null)
                        uiElement.IsMouseDirectlyOver = true;
                }

                UpdateIsMouseOver(elementUnderMouse as UIElement);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified element is valid for receiving input.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element is valid for input; otherwise, <c>false</c>.</returns>
        private Boolean IsElementValidForInput(IInputElement element)
        {
            var uiElement = element as UIElement;
            if (uiElement == null)
                return false;

            return uiElement.IsHitTestVisible && uiElement.IsVisible && uiElement.IsEnabled;
        }

        /// <summary>
        /// Gets a value indicating whether the specified element has captured the mouse
        /// or is within the capture subtree.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element has captured the mouse; otherwise, <c>false</c>.</returns>
        private Boolean IsMouseCapturedByElement(IInputElement element)
        {
            var uiElement = element as UIElement;
            if (uiElement == null)
                return false;

            if (mouseCaptureMode == CaptureMode.None)
                return false;

            if (mouseCaptureMode == CaptureMode.Element)
                return uiElement == elementWithMouseCapture;

            var current = (DependencyObject)element;
            while (current != null)
            {
                if (current == elementWithMouseCapture)
                    return true;

                current = VisualTreeHelper.GetParent(current) ?? LogicalTreeHelper.GetParent(current);
            }
            return false;
        }

        /// <summary>
        /// Redirects mouse input to the element with mouse capture, if necessary.
        /// </summary>
        /// <param name="recipient">The element that will be the target of the input event prior to considering mouse capture.</param>
        /// <returns>The element that will be the target of the input event after considering mouse capture.</returns>
        private IInputElement RedirectMouseInput(IInputElement recipient)
        {
            if (mouseCaptureMode == CaptureMode.None)
                return recipient;

            return IsMouseCapturedByElement(recipient) ? recipient : elementWithMouseCapture;
        }

        /// <summary>
        /// Sets the value of the <see cref="IInputElement.IsKeyboardFocusWithin"/> property on the specified element
        /// and all of its ancestors to the specified value.
        /// </summary>
        /// <param name="element">The element on which to set the property value.</param>
        /// <param name="value">The value to set on the element and its ancestors.</param>
        private void SetIsKeyboardFocusWithin(IInputElement element, Boolean value)
        {
            var visual = element as Visual;

            var focused = element as UIElement;
            if (focused != null)
                focused.IsKeyboardFocused = value;

            while (visual != null)
            {
                var uiElement = visual as UIElement;
                if (uiElement != null)
                {
                    uiElement.IsKeyboardFocusWithin = value;
                }

                visual = VisualTreeHelper.GetParent(visual) as Visual;
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
                var dobj = elementWithFocus as DependencyObject;
                if (dobj != null)
                {
                    var keyDownData = new RoutedEventData(dobj);
                    Keyboard.RaisePreviewKeyDown(dobj, device, key, ctrl, alt, shift, repeat, ref keyDownData);
                    Keyboard.RaiseKeyDown(dobj, device, key, ctrl, alt, shift, repeat, ref keyDownData);
                }
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
                var dobj = elementWithFocus as DependencyObject;
                if (dobj != null)
                {
                    var keyUpData = new RoutedEventData(dobj);
                    Keyboard.RaisePreviewKeyUp(dobj, device, key, ref keyUpData);
                    Keyboard.RaiseKeyUp(dobj, device, key, ref keyUpData);
                }
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
                var dobj = elementWithFocus as DependencyObject;
                if (dobj != null)
                {
                    var textInputData = new RoutedEventData(dobj);
                    Keyboard.RaisePreviewTextInput(dobj, device, ref textInputData);
                    Keyboard.RaiseTextInput(dobj, device, ref textInputData);
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.Moved"/> event.
        /// </summary>
        private void mouse_Moved(IUltravioletWindow window, MouseDevice device, Int32 x, Int32 y, Int32 dx, Int32 dy)
        {
            if (window != Window)
                return;

            var recipient = elementUnderMouse;
            if (recipient != null)
            {
                var dipsX      = Display.PixelsToDips(x);
                var dipsY      = Display.PixelsToDips(y);
                var dipsDeltaX = Display.PixelsToDips(dx);
                var dipsDeltaY = Display.PixelsToDips(dy);

                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var mouseMoveData = new RoutedEventData(dobj);
                    Mouse.RaisePreviewMouseMove(dobj, device, dipsX, dipsY, dipsDeltaX, dipsDeltaY, ref mouseMoveData);
                    Mouse.RaiseMouseMove(dobj, device, dipsX, dipsY, dipsDeltaX, dipsDeltaY, ref mouseMoveData);
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.ButtonPressed"/> event.
        /// </summary>
        private void mouse_ButtonPressed(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (window != Window)
                return;

            UpdateElementUnderMouse();

            var recipient = elementUnderMouse;
            if (recipient != elementWithFocus)
            {
                if (elementWithFocus != null)
                {
                    BlurElement(elementWithFocus);
                }

                if (recipient != null && recipient.Focusable)
                {
                    FocusElement(recipient);
                }
            }

            if (recipient != null)
            {
                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var mouseDownData = new RoutedEventData(dobj);
                    Mouse.RaisePreviewMouseDown(dobj, device, button, ref mouseDownData);
                    Mouse.RaiseMouseDown(dobj, device, button, ref mouseDownData);
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.ButtonReleased"/> event.
        /// </summary>
        private void mouse_ButtonReleased(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (window != Window)
                return;

            var recipient = elementUnderMouse;
            if (recipient != null)
            {
                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var mouseUpData = new RoutedEventData(dobj);
                    Mouse.RaisePreviewMouseUp(dobj, device, button, ref mouseUpData);
                    Mouse.RaiseMouseUp(dobj, device, button, ref mouseUpData);
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.Click"/> event.
        /// </summary>
        private void mouse_Click(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (window != Window)
                return;

            var recipient = elementUnderMouse;
            if (recipient != null)
            {
                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var mouseClickData = new RoutedEventData(dobj);
                    Mouse.RaisePreviewMouseClick(dobj, device, button, ref mouseClickData);
                    Mouse.RaiseMouseClick(dobj, device, button, ref mouseClickData);
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.DoubleClick"/> event.
        /// </summary>
        private void mouse_DoubleClick(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (window != Window)
                return;

            var recipient = elementUnderMouse;
            if (recipient != null)
            {
                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var mouseDoubleClickData = new RoutedEventData(dobj);
                    Mouse.RaisePreviewMouseDoubleClick(dobj, device, button, ref mouseDoubleClickData);
                    Mouse.RaiseMouseDoubleClick(dobj, device, button, ref mouseDoubleClickData);
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.WheelScrolled"/> event.
        /// </summary>
        private void mouse_WheelScrolled(IUltravioletWindow window, MouseDevice device, Int32 x, Int32 y)
        {
            if (window != Window)
                return;

            var recipient = elementUnderMouse;
            if (recipient != null)
            {
                var dipsX = Display.PixelsToDips(x);
                var dipsY = Display.PixelsToDips(y);

                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var mouseWheelData = new RoutedEventData(dobj);
                    Mouse.RaisePreviewMouseWheel(dobj, device, dipsX, dipsY, ref mouseWheelData);
                    Mouse.RaiseMouseWheel(dobj, device, dipsX, dipsY, ref mouseWheelData);
                }
            }
        }

        /// <summary>
        /// Called when the Presentation Foundation's global style sheet is changed.
        /// </summary>
        private void PresentationFoundationView_GlobalStyleSheetChanged(Object sender, EventArgs e)
        {
            SetStyleSheet(localStyleSheet);
        }

        // Property values.
        private readonly Namescope namescope;
        private readonly PresentationFoundationViewResources resources;
        private readonly UvssDocument combinedStyleSheet;
        private UvssDocument localStyleSheet;
        private Grid layoutRoot;

        // State values.
        private readonly HashSet<DependencyObject> isMouseOverSet = new HashSet<DependencyObject>();
        private readonly DrawingContext drawingContext;
        private IInputElement elementUnderMousePrev;
        private IInputElement elementUnderMouse;
        private IInputElement elementWithMouseCapture;
        private IInputElement elementWithFocus;
        private CaptureMode mouseCaptureMode;
        private Boolean hookedGlobalStyleSheetChanged;

        // Popup handling.
        private readonly PopupQueue popupQueue = new PopupQueue();
        private Popup elementUnderMousePopupPrev;
        private Popup elementUnderMousePopup;
    }
}
