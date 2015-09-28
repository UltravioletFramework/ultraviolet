using System;
using System.Collections.Generic;
using System.IO;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Media;
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
            if (uv.IsRunningInServiceMode)
                throw new NotSupportedException(UltravioletStrings.NotSupportedInServiceMode);

            this.combinedStyleSheet = new UvssDocument(null, null);

            this.namescope      = new Namescope();
            this.resources      = new PresentationFoundationViewResources(this);
            this.drawingContext = new DrawingContext();

            this.layoutRoot = new PresentationFoundationViewRoot(uv, null);
            this.layoutRoot.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.layoutRoot.VerticalAlignment = VerticalAlignment.Stretch;
            this.layoutRoot.View = this;
            this.layoutRoot.CacheLayoutParameters();
            this.layoutRoot.InvalidateMeasure();

            HookKeyboardEvents();
            HookMouseEvents();
            HookTouchEvents();
            HookGamePadEvents();
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

            var view = UvmlLoader.Load(uv, uiPanelDefinition);

            var uvss    = String.Join(Environment.NewLine, uiPanelDefinition.StyleSheets);
            var uvssdoc = UvssDocument.Parse(uvss);

            view.SetStyleSheet(uvssdoc);

            if (!String.IsNullOrEmpty(uiPanelDefinition.AssetFilePath))
            {
                view.viewModelWrapperName = GetDataSourceWrapperNameForView(uiPanelDefinition.AssetFilePath);
            }

            return view;
        }
        
        /// <summary>
        /// Gets the name of the data source wrapper for a view which is defined in a file with the specified asset path.
        /// </summary>
        /// <param name="path">The asset path of the UVML file that defines the view.</param>
        /// <returns>The name of the data source wrapper for the specified view.</returns>
        public static String GetDataSourceWrapperNameForView(String path)
        {
            Contract.RequireNotEmpty(path, "path");

            var pathComponents = path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });
            pathComponents[pathComponents.Length - 1] = Path.GetFileNameWithoutExtension(pathComponents[pathComponents.Length - 1]);

            return String.Format("{0}_VM_Impl", String.Join("_", pathComponents));
        }
        
        /// <summary>
        /// Gets the name of the data source wrapper for a component template which is associated with the specified control type.
        /// </summary>
        /// <param name="type">The type of control with which the component template is associated.</param>
        /// <returns>The name of the data source wrapper for the specified control type.</returns>
        public static String GetDataSourceWrapperNameForComponentTemplate(Type type)
        {
            Contract.Require(type, "type");

            return String.Format("{0}_Template_Impl", type.FullName.Replace('.', '_'));
        }

        /// <summary>
        /// Gets the namespace that contains data source wrappers for views.
        /// </summary>
        public static String DataSourceWrapperNamespaceForViews
        {
            get { return "TwistedLogik.Ultraviolet.UI.Presentation.CompiledExpressions"; }
        }

        /// <summary>
        /// Gets the namespace that contains data source wrappers for component templates.
        /// </summary>
        public static String DataSourceWrapperNamespaceForComponentTemplates
        {
            get { return "TwistedLogik.Ultraviolet.UI.Presentation.CompiledExpressions"; }
        }
        
        /// <inheritdoc/>
        public override void Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            Contract.Require(time, "time");
            Contract.Require(spriteBatch, "spriteBatch");
            
            if (Ultraviolet.IsRunningInServiceMode)
                throw new NotSupportedException(UltravioletStrings.NotSupportedInServiceMode);
            
            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.BeginDraw();

            popupQueue.Clear();

            if (Window == null)
                return;

            EnsureIsLoaded();

            var previousSpriteBatchState = spriteBatch.GetCurrentState();
            spriteBatch.End();

            drawingContext.Reset(Display);

            drawingContext.SpriteBatch = spriteBatch;
            drawingContext.Begin();
            
            layoutRoot.Draw(time, drawingContext);
            popupQueue.Draw(time, drawingContext);

            drawingContext.End();

            if (Diagnostics.DrawDiagnosticsVisuals)
            {
                drawingContext.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, Matrix.Identity);
                DrawDiagnosticsVisuals(drawingContext, layoutRoot);
                drawingContext.End();
            }

            drawingContext.SpriteBatch = null;

            spriteBatch.Begin(previousSpriteBatchState);

            upf.PerformanceStats.EndDraw();
        }
        
        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            Contract.Require(time, "time");

            if (Window == null)
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.BeginUpdate();

            HandleUserInput();
            layoutRoot.Update(time);

            upf.PerformanceStats.EndUpdate();
        }
        
        /// <inheritdoc/>
        public override void SetViewModel(Object viewModel)
        {
            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            var wrapper = upf.CreateDataSourceWrapper(viewModelWrapperName, viewModel);

            base.SetViewModel(wrapper);
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
        /// <returns><c>true</c> if the element was successfully focused; otherwise, <c>false</c>.</returns>
        public Boolean FocusElement(IInputElement element)
        {
            Contract.Require(element, "element");

            if (elementWithFocus == element || !Keyboard.IsFocusable(element))
                return false;

            var keyboard = Ultraviolet.GetInput().GetKeyboard();
            var oldFocus = elementWithFocus;
            var newFocus = element;

            var elementWithFocusAgreesToChange = true;
            if (elementWithFocus != null)
            {
                var data = new RoutedEventData((DependencyObject)elementWithFocus);
                Keyboard.RaisePreviewLostKeyboardFocus((DependencyObject)elementWithFocus, keyboard, oldFocus, newFocus, ref data);

                if (data.Handled)
                    elementWithFocusAgreesToChange = false;
            }
            
            var elementAgreesToChange = true;
            if (element != null)
            {
                var data = new RoutedEventData((DependencyObject)element);
                Keyboard.RaisePreviewGotKeyboardFocus((DependencyObject)element, keyboard, oldFocus, newFocus, ref data);

                if (data.Handled)
                    elementAgreesToChange = false;
            }

            if (!elementWithFocusAgreesToChange || !elementAgreesToChange)
                return false;

            if (elementWithFocus != null)
            {
                BlurElement(elementWithFocus);
            }
            
            elementWithFocus = element;

            if (elementWithFocus != null)
            {
                var focusScope = FocusManager.GetFocusScope((DependencyObject)elementWithFocus);
                if (focusScope != null && focusScope != elementWithFocus)
                {
                    FocusManager.SetFocusedElement(focusScope, elementWithFocus);
                }
            }
            
            SetIsKeyboardFocusWithin(elementWithFocus, true);

            var dobj = elementWithFocus as DependencyObject;
            if (dobj != null)
            {
                var gotFocusData = new RoutedEventData(dobj);
                Keyboard.RaiseGotKeyboardFocus(dobj, keyboard, oldFocus, newFocus, ref gotFocusData);
            }

            return true;
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
                var keyboard = Ultraviolet.GetInput().GetKeyboard();

                var lostFocusData = new RoutedEventData(dobj);
                Keyboard.RaiseLostKeyboardFocus(dobj, keyboard, elementWithFocusOld, null, ref lostFocusData);
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
        /// Gets the root element of the view's layout.
        /// </summary>
        public PresentationFoundationViewRoot LayoutRoot
        {
            get { return layoutRoot; }
        }

        /// <summary>
        /// Gets the view's global resource collection.
        /// </summary>
        public PresentationFoundationViewResources Resources
        {
            get { return resources; }
        }

        /// <summary>
        /// Ensures that the view and its elements have been loaded.
        /// </summary>
        internal void EnsureIsLoaded()
        {
            if (layoutRoot.IsLoaded)
                return;

            layoutRoot.EnsureIsLoaded(true);
            Ultraviolet.GetUI().GetPresentationFoundation().PerformLayout();
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
        protected override void OnOpened()
        {

        }

        /// <inheritdoc/>
        protected override void OnClosed()
        {
            layoutRoot.Cleanup();
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && !Ultraviolet.Disposed)
            {
                UnhookGlobalStyleSheetChanged();
                UnhookKeyboardEvents();
                UnhookMouseEvents();
                UnhookTouchEvents();
                UnhookGamePadEvents();
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
        /// Draws the diagnostics visuals for the specified element.
        /// </summary>
        /// <param name="drawingContext">The drawing context with which to draw the visuals.</param>
        /// <param name="element">The element for which to draw visuals.</param>
        private static void DrawDiagnosticsVisuals(DrawingContext drawingContext, UIElement element)
        {
            if (Diagnostics.GetDrawVisualBounds(element))
            {
                var display  = element.View.Display;
                var bounds   = element.TransformedVisualBounds;
                var position = (Vector2)(Point2)display.DipsToPixels(bounds.Location);
                var width    = (Int32)display.DipsToPixels(bounds.Width);
                var height   = (Int32)display.DipsToPixels(bounds.Height);
                var color    = Diagnostics.GetDrawVisualBoundsColor(element);
                drawingContext.DrawImage(Diagnostics.BoundingBoxImage, position, width, height, color);
            }

            var popup = element as Popup;
            if (popup != null)
            {
                if (popup.IsOpen)
                {
                    DrawDiagnosticsVisuals(drawingContext, ((Popup)element).Root);
                }
            }
            else
            {
                VisualTreeHelper.ForEachChild<UIElement>(element, drawingContext, (child, state) =>
                {
                    DrawDiagnosticsVisuals((DrawingContext)state, child);
                });
            }
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
        /// Hooks into Ultraviolet's touch input events.
        /// </summary>
        private void HookTouchEvents()
        {
            var input = Ultraviolet.GetInput();
            if (input.IsTouchSupported())
            {
                var touch           = input.GetTouchDevice();
                touch.Tap          += touch_Tap;
                touch.FingerUp     += touch_FingerUp;
                touch.FingerDown   += touch_FingerDown;
                touch.FingerMotion += touch_FingerMotion;
            }
        }

        /// <summary>
        /// Hooks into Ultraviolet's game pad input events.
        /// </summary>
        private void HookGamePadEvents()
        {
            var input = Ultraviolet.GetInput();
            if (input.IsGamePadSupported())
            {
                HookFirstPlayerGamePadEvents();

                input.GamePadConnected += gamePad_GamePadConnected;
                input.GamePadDisconnected += gamePad_GamePadDisconnected;
            }
        }

        /// <summary>
        /// Hooks into Ultraviolet's game pad input events for the first player's game pad.
        /// </summary>
        private void HookFirstPlayerGamePadEvents()
        {
            var gamePad = Ultraviolet.GetInput().GetGamePadForPlayer(0);
            if (gamePad == null)
                return;

            gamePad.AxisChanged += gamePad_AxisChanged;
            gamePad.AxisPressed += gamePad_AxisPressed;
            gamePad.AxisReleased += gamePad_AxisReleased;
            gamePad.ButtonPressed += gamePad_ButtonPressed;
            gamePad.ButtonReleased += gamePad_ButtonReleased;

            hookedFirstPlayerGamePad = true;
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
        /// Unhooks from Ultraviolet's touch input events.
        /// </summary>
        private void UnhookTouchEvents()
        {
            var input = Ultraviolet.GetInput();
            if (input.IsTouchSupported())
            {
                var touch           = input.GetTouchDevice();
                touch.Tap          -= touch_Tap;
                touch.FingerUp     -= touch_FingerUp;
                touch.FingerDown   -= touch_FingerDown;
                touch.FingerMotion -= touch_FingerMotion;
            }
        }

        /// <summary>
        /// Unhooks from Ultraviolet's game pad input events.
        /// </summary>
        private void UnhookGamePadEvents()
        {
            var input = Ultraviolet.GetInput();
            if (input.IsGamePadSupported())
            {
                input.GamePadConnected -= gamePad_GamePadConnected;
                input.GamePadDisconnected -= gamePad_GamePadDisconnected;

                if (hookedFirstPlayerGamePad)
                {
                    UnhookFirstPlayerGamePadEvents();
                }
            }
        }

        /// <summary>
        /// Unhooks from Ultraviolet's game pad input events for the first player's game pad.
        /// </summary>
        private void UnhookFirstPlayerGamePadEvents()
        {
            hookedFirstPlayerGamePad = false;

            var gamePad = Ultraviolet.GetInput().GetGamePadForPlayer(0);
            if (gamePad == null)
                return;

            gamePad.AxisChanged -= gamePad_AxisChanged;
            gamePad.AxisPressed -= gamePad_AxisPressed;
            gamePad.AxisReleased -= gamePad_AxisReleased;
            gamePad.ButtonPressed -= gamePad_ButtonPressed;
            gamePad.ButtonReleased -= gamePad_ButtonReleased;
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
            var mousePos  = Display.PixelsToDips(mouse.Position);
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
        /// Converts normalized touch device coordinates to view-relative coordinates.
        /// </summary>
        private Point2D GetTouchCoordinates(Single x, Single y)
        {
            var windowSize = Window.ClientSize;

            var xRelativeToWindow = windowSize.Width * x;
            var yRelativeToWindow = windowSize.Height * y;

            var xRelativeToView = xRelativeToWindow - this.X;
            var yRelativeToView = yRelativeToWindow - this.Y;

            return (Point2D)Display.PixelsToDips(new Vector2(xRelativeToView, yRelativeToView));
        }

        /// <summary>
        /// Converts a normalized touch delta to device-independent pixels.
        /// </summary>
        private Point2D GetTouchDelta(Single x, Single y)
        {
            var windowSize = Window.ClientSize;

            var xPixels = windowSize.Width * x;
            var yPixels = windowSize.Height * y;

            return (Point2D)Display.PixelsToDips(new Vector2(xPixels, yPixels));
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
        /// Starts at the specified element and moves up the visual tree to find
        /// the nearest ancestor which is focusable.
        /// </summary>
        /// <param name="start">The element at which to begin searching.</param>
        /// <returns>The nearest focusable element, or <c>null</c> if no focusable element was found.</returns>
        private IInputElement GetNearestFocusableElement(IInputElement start)
        {
            var current = start as DependencyObject;

            while (current != null)
            {
                var element = current as UIElement;
                if (element != null && element.Focusable)
                    return element;

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
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

            var suppressKeyNav = false;

            if (elementWithFocus != null)
            {
                var dobj = elementWithFocus as DependencyObject;
                if (dobj != null)
                {
                    var keyDownData = new RoutedEventData(dobj);
                    Keyboard.RaisePreviewKeyDown(dobj, device, key, ctrl, alt, shift, repeat, ref keyDownData);
                    Keyboard.RaiseKeyDown(dobj, device, key, ctrl, alt, shift, repeat, ref keyDownData);

                    suppressKeyNav = keyDownData.Handled;
                }
            }
            
            if (!suppressKeyNav)
                KeyboardNavigator.PerformNavigation(this, device, key, ctrl, alt, shift, repeat);
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
            if (recipient != null)
            {
                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var mouseDownData = new RoutedEventData(dobj);
                    Mouse.RaisePreviewMouseDown(dobj, device, button, ref mouseDownData);
                    Mouse.RaiseMouseDown(dobj, device, button, ref mouseDownData);

                    if (!Generic.IsTouchDeviceAvailable && button == MouseButton.Left)
                    {
                        var genericInteractionData = new RoutedEventData(dobj);
                        Generic.RaisePreviewGenericInteraction(dobj, device, ref genericInteractionData);
                        Generic.RaiseGenericInteraction(dobj, device, ref genericInteractionData);
                    }
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
        /// Handles the <see cref="TouchDevice.Tap"/> event.
        /// </summary>
        private void touch_Tap(TouchDevice device, Int64 fingerID, Single x, Single y)
        {
            if (Window == null)
                return;

            var position = GetTouchCoordinates(x, y);

            var recipient = elementUnderMouse as DependencyObject;
            if (recipient == null)
                return;

            var tapData = new RoutedEventData(recipient);
            Touch.RaisePreviewTap(recipient, device, fingerID, position.X, position.Y, ref tapData);
            Touch.RaiseTap(recipient, device, fingerID, position.X, position.Y, ref tapData);

            if (fingerID == 0)
            {
                var genericInteractionData = new RoutedEventData(recipient);
                Generic.RaisePreviewGenericInteraction(recipient, device, ref genericInteractionData);
                Generic.RaiseGenericInteraction(recipient, device, ref genericInteractionData);
            }
        }

        /// <summary>
        /// Handles the <see cref="TouchDevice.FingerDown"/> event.
        /// </summary>
        private void touch_FingerDown(TouchDevice device, Int64 fingerID, Single x, Single y, Single pressure)
        {
            if (Window == null)
                return;

            var position = GetTouchCoordinates(x, y);

            var recipient = HitTest(position) as DependencyObject;
            if (recipient == null)
                return;

            var fingerDownData = new RoutedEventData(recipient);
            Touch.RaisePreviewFingerDown(recipient, device, fingerID, position.X, position.Y, pressure, ref fingerDownData);
            Touch.RaiseFingerDown(recipient, device, fingerID, position.X, position.Y, pressure, ref fingerDownData);

            elementLastTouched = recipient as IInputElement;
        }

        /// <summary>
        /// Handles the <see cref="TouchDevice.FingerUp"/> event.
        /// </summary>
        private void touch_FingerUp(TouchDevice device, Int64 fingerID, Single x, Single y, Single pressure)
        {
            if (Window == null)
                return;

            var position = GetTouchCoordinates(x, y);

            var recipient = elementLastTouched as DependencyObject;
            if (recipient == null)
                return;

            var fingerUpData = new RoutedEventData(recipient);
            Touch.RaisePreviewFingerUp(recipient, device, fingerID, position.X, position.Y, pressure, ref fingerUpData);
            Touch.RaiseFingerUp(recipient, device, fingerID, position.X, position.Y, pressure, ref fingerUpData);

            elementLastTouched = null;
        }

        /// <summary>
        /// Handles the <see cref="TouchDevice.FingerMotion"/> event.
        /// </summary>
        private void touch_FingerMotion(TouchDevice device, Int64 fingerID, Single x, Single y, Single dx, Single dy, Single pressure)
        {
            if (Window == null)
                return;

            var position = GetTouchCoordinates(x, y);

            var recipient = elementLastTouched as DependencyObject;
            if (recipient == null)
                return;

            var delta = GetTouchDelta(dx, dy);

            var fingerMotionData = new RoutedEventData(recipient);
            Touch.RaisePreviewFingerMotion(recipient, device, fingerID, position.X, position.Y, delta.X, delta.Y, pressure, ref fingerMotionData);
            Touch.RaiseFingerMotion(recipient, device, fingerID, position.X, position.Y, delta.X, delta.Y, pressure, ref fingerMotionData);
        }

        /// <summary>
        /// Handles the <see cref="IUltravioletInput.GamePadConnected"/> event.
        /// </summary>
        private void gamePad_GamePadConnected(GamePadDevice device, Int32 playerIndex)
        {
            if (device.PlayerIndex != 0)
                return;

            HookFirstPlayerGamePadEvents();
        }

        /// <summary>
        /// Handles the <see cref="IUltravioletInput.GamePadDisconnected"/> event.
        /// </summary>
        private void gamePad_GamePadDisconnected(GamePadDevice device, Int32 playerIndex)
        {
            if (device.PlayerIndex != 0)
                return;

            UnhookFirstPlayerGamePadEvents();
        }
        
        /// <summary>
        /// Handles the <see cref="GamePadDevice.AxisChanged"/> event.
        /// </summary>
        private void gamePad_AxisChanged(GamePadDevice device, GamePadAxis axis, Single value)
        {
            if (device.PlayerIndex != 0)
                return;

            var recipient = elementWithFocus as DependencyObject;
            if (recipient != null)
            {
                var gamePadAxisChangedData = new RoutedEventData(recipient);
                GamePad.RaisePreviewAxisChanged(recipient, device, axis, value, ref gamePadAxisChangedData);
                GamePad.RaiseAxisChanged(recipient, device, axis, value, ref gamePadAxisChangedData);
            }
        }

        /// <summary>
        /// Handles the <see cref="GamePadDevice.AxisPressed"/> event.
        /// </summary>
        private void gamePad_AxisPressed(GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat)
        {
            if (device.PlayerIndex != 0)
                return;

            var suppressGamePadNav = false;

            var recipient = elementWithFocus as DependencyObject;
            if (recipient != null)
            {
                var gamePadAxisPressedData = new RoutedEventData(recipient);
                GamePad.RaisePreviewAxisDown(recipient, device, axis, value, repeat, ref gamePadAxisPressedData);
                GamePad.RaiseAxisDown(recipient, device, axis, value, repeat, ref gamePadAxisPressedData);

                suppressGamePadNav = gamePadAxisPressedData.Handled;
            }

            if (!suppressGamePadNav)
            { /* TODO: Directional nav with game pad axes */ }
        }

        /// <summary>
        /// Handles the <see cref="GamePadDevice.AxisReleased"/> event.
        /// </summary>
        private void gamePad_AxisReleased(GamePadDevice device, GamePadAxis axis, Single value)
        {
            if (device.PlayerIndex != 0)
                return;
            
            var recipient = elementWithFocus as DependencyObject;
            if (recipient != null)
            {
                var gamePadAxisPressedData = new RoutedEventData(recipient);
                GamePad.RaisePreviewAxisUp(recipient, device, axis, ref gamePadAxisPressedData);
                GamePad.RaiseAxisUp(recipient, device, axis, ref gamePadAxisPressedData);
            }
        }

        /// <summary>
        /// Handles the <see cref="GamePadDevice.ButtonPressed"/> event.
        /// </summary>
        private void gamePad_ButtonPressed(GamePadDevice device, GamePadButton button, Boolean repeat)
        {
            if (device.PlayerIndex != 0)
                return;

            var suppressGamePadNav = false;

            var recipient = elementWithFocus as DependencyObject;
            if (recipient != null)
            {
                var gamePadAxisChangedData = new RoutedEventData(recipient);
                GamePad.RaisePreviewButtonDown(recipient, device, button, repeat, ref gamePadAxisChangedData);
                GamePad.RaiseButtonDown(recipient, device, button, repeat, ref gamePadAxisChangedData);

                suppressGamePadNav = gamePadAxisChangedData.Handled;
            }

            if (!suppressGamePadNav)
                KeyboardNavigator.PerformNavigation(this, device, button);
        }

        /// <summary>
        /// Handles the <see cref="GamePadDevice.ButtonReleased"/> event.
        /// </summary>
        private void gamePad_ButtonReleased(GamePadDevice device, GamePadButton button)
        {
            if (device.PlayerIndex != 0)
                return;
            
            var recipient = elementWithFocus as DependencyObject;
            if (recipient != null)
            {
                var gamePadAxisChangedData = new RoutedEventData(recipient);
                GamePad.RaisePreviewButtonUp(recipient, device, button, ref gamePadAxisChangedData);
                GamePad.RaiseButtonUp(recipient, device, button, ref gamePadAxisChangedData);
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
        private PresentationFoundationViewRoot layoutRoot;

        // State values.
        private readonly HashSet<DependencyObject> isMouseOverSet = new HashSet<DependencyObject>();
        private readonly DrawingContext drawingContext;
        private IInputElement elementUnderMousePrev;
        private IInputElement elementUnderMouse;
        private IInputElement elementWithMouseCapture;
        private IInputElement elementWithFocus;
        private IInputElement elementLastTouched;
        private CaptureMode mouseCaptureMode;
        private Boolean hookedGlobalStyleSheetChanged;
        private Boolean hookedFirstPlayerGamePad;

        // Popup handling.
        private readonly PopupQueue popupQueue = new PopupQueue();
        private Popup elementUnderMousePopupPrev;
        private Popup elementUnderMousePopup;

        // View model wrapping.
        private String viewModelWrapperName;
    }
}
