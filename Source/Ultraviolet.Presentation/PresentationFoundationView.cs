using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.Input;
using Ultraviolet.Platform;
using Ultraviolet.Presentation.Animations;
using Ultraviolet.Presentation.Controls;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;
using Ultraviolet.Presentation.Media;
using Ultraviolet.Presentation.Media.Effects;
using Ultraviolet.Presentation.Styles;
using Ultraviolet.UI;

namespace Ultraviolet.Presentation
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
        /// <param name="panel">The panel that is creating the view.</param>
        /// <param name="viewModelType">The view's associated model type.</param>
        public PresentationFoundationView(UltravioletContext uv, UIPanel panel, Type viewModelType)
            : base(uv, panel, viewModelType)
        {
            if (uv.IsRunningInServiceMode)
                throw new NotSupportedException(UltravioletStrings.NotSupportedInServiceMode);

            var textShaperFactory = Ultraviolet.TryGetFactoryMethod<TextShaperFactory>();
            if (textShaperFactory != null)
                this.TextShaper = textShaperFactory(Ultraviolet);

            this.combinedStyleSheet = new UvssDocument(uv);

            this.namescope = new Namescope();
            this.resources = new PresentationFoundationViewResources(this);
            this.drawingContext = new DrawingContext();

            this.layoutRoot = new PresentationFoundationViewRoot(uv, null);
            this.layoutRoot.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.layoutRoot.VerticalAlignment = VerticalAlignment.Stretch;
            this.layoutRoot.View = this;
            this.layoutRoot.CacheLayoutParameters();
            this.layoutRoot.InvalidateMeasure();

            this.mouseCursorTracker = CursorTracker.ForMouse(this);

            HookKeyboardEvents();
            HookMouseEvents();
            HookGamePadEvents();

            var input = uv.GetInput();
            if (input.IsTouchDeviceRegistered())
            {
                HookTouchEvents();
            }
            else
            {
                input.TouchDeviceRegistered += Input_TouchDeviceRegistered;
            }

            SetStyleSheet(null);
        }

        /// <summary>
        /// Loads an instance of <see cref="PresentationFoundationView"/> from an XML document.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiPanel">The <see cref="UIPanel"/> that is creating the view.</param>
        /// <param name="uiPanelDefinition">The <see cref="UIPanelDefinition"/> that defines the view's containing panel.</param>
        /// <param name="vmfactory">A view model factory which is used to create the view's initial view model, or <see langword="null"/> to skip view model creation.</param>
        /// <returns>The <see cref="PresentationFoundationView"/> that was loaded from the specified XML document.</returns>
        public static PresentationFoundationView Load(UltravioletContext uv, UIPanel uiPanel, UIPanelDefinition uiPanelDefinition, UIViewModelFactory vmfactory)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(uiPanel, nameof(uiPanel));
            Contract.Require(uiPanelDefinition, nameof(uiPanelDefinition));

            if (uiPanelDefinition.ViewElement == null)
                return null;

            var view = UvmlLoader.Load(uv, uiPanel, uiPanelDefinition, vmfactory);

            var sources = uiPanelDefinition.StyleSheetSources
                .Where(x => !(x?.Cultures.Any() ?? false) || x.Cultures.Contains(Localization.CurrentCulture))
                .Where(x => !(x?.Languages.Any() ?? false) || x.Languages.Contains(Localization.CurrentLanguage))
                .Select(x => x.Source).ToList();
            if (sources.Any())
            {
                var uvss = String.Join(Environment.NewLine, sources);
                var uvssdoc = UvssDocument.Compile(uv, uvss);
                view.SetStyleSheet(uvssdoc);
            }
            else
            {
                view.SetStyleSheet(null);
            }
            return view;
        }

        /// <summary>
        /// Gets the namespace that contains data source wrappers for views.
        /// </summary>
        public static String DataSourceWrapperNamespaceForViews
        {
            get { return "Ultraviolet.Presentation.CompiledExpressions"; }
        }

        /// <summary>
        /// Gets the namespace that contains data source wrappers for component templates.
        /// </summary>
        public static String DataSourceWrapperNamespaceForComponentTemplates
        {
            get { return "Ultraviolet.Presentation.CompiledExpressions"; }
        }

        /// <inheritdoc/>
        public override void Draw(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            Contract.Require(time, nameof(time));
            Contract.Require(spriteBatch, nameof(spriteBatch));

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
            drawingContext.PushOpacity(opacity);

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
            Contract.Require(time, nameof(time));

            if (Window == null)
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.BeginUpdate();

            HandleUserInput(time);
            layoutRoot.Update(time);

            upf.PerformanceStats.EndUpdate();
        }

        /// <inheritdoc/>
        public override void SetViewModel(Object viewModel)
        {
            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            var wrapper = upf.CreateDataSourceWrapperForView(viewModel, Namescope);

            base.SetViewModel(wrapper);

            RaiseViewModelChangedEvent(layoutRoot);
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
            Contract.Require(selector, nameof(selector));
            Contract.Require(action, nameof(action));

            SelectInternal(layoutRoot, selector, state, action);
        }

        /// <summary>
        /// Performs the specified action on all objects within the view's visual tree
        /// which match the specified UVSS selector.
        /// </summary>
        /// <param name="root">The root element at which to begin evaluation, or <see langword="null"/> to begin at the layout root.</param>
        /// <param name="selector">The UVSS selector that specifies which objects should be targeted by the action.</param>
        /// <param name="state">A state value which is passed to the specified action.</param>
        /// <param name="action">The action to perform on the selected objects.</param>
        public void Select(UIElement root, UvssSelector selector, Object state, Action<UIElement, Object> action)
        {
            Contract.Require(selector, nameof(selector));
            Contract.Require(action, nameof(action));

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
        /// Invalidates the view's cursor.
        /// </summary>
        public void InvalidateCursor()
        {
            var cursor = default(Cursor);

            var uiElementUnderMouse = mouseCursorTracker.ElementUnderCursor as UIElement;
            if (uiElementUnderMouse != null)
            {
                var evtDelegate = EventManager.GetInvocationDelegate<UpfQueryCursorEventHandler>(Mouse.QueryCursorEvent);
                var evtData = CursorQueryRoutedEventData.Retrieve(uiElementUnderMouse, autorelease: false);
                evtDelegate(uiElementUnderMouse, Mouse.PrimaryDevice, evtData);
                cursor = evtData.Cursor;
                evtData.Release();
            }

            if (cursor == null && Resources.Cursor.IsLoaded)
                cursor = Resources.Cursor.Resource.Cursor;

            Ultraviolet.GetPlatform().Cursor = cursor;
        }

        /// <summary>
        /// Grants input focus within this view to the specified element.
        /// </summary>
        /// <param name="element">The element to which to grant input focus.</param>
        /// <returns><see langword="true"/> if the element was successfully focused; otherwise, <see langword="false"/>.</returns>
        public Boolean FocusElement(IInputElement element)
        {
            Contract.Require(element, nameof(element));

            if (elementWithFocus == element || !Keyboard.IsFocusable(element))
                return false;

            var keyboard = Ultraviolet.GetInput().GetKeyboard();
            var oldFocus = elementWithFocus;
            var newFocus = element;

            var elementWithFocusAgreesToChange = true;
            if (elementWithFocus != null)
            {
                var data = RoutedEventData.Retrieve((DependencyObject)elementWithFocus);
                Keyboard.RaisePreviewLostKeyboardFocus((DependencyObject)elementWithFocus, keyboard, oldFocus, newFocus, data);

                if (data.Handled)
                    elementWithFocusAgreesToChange = false;
            }

            var elementAgreesToChange = true;
            if (element != null)
            {
                var data = RoutedEventData.Retrieve((DependencyObject)element);
                Keyboard.RaisePreviewGotKeyboardFocus((DependencyObject)element, keyboard, oldFocus, newFocus, data);

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
                var gotFocusData = RoutedEventData.Retrieve(dobj);
                Keyboard.RaiseGotKeyboardFocus(dobj, keyboard, oldFocus, newFocus, gotFocusData);
            }

            UpdateIsDefaulted();

            wasFocusMostRecentlyChangedByKeyboardOrGamePad = false;

            CommandManager.InvalidateRequerySuggested();

            return true;
        }

        /// <summary>
        /// Removes input focus within this view from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove input focus.</param>
        public void BlurElement(IInputElement element)
        {
            Contract.Require(element, nameof(element));

            if (elementWithFocus != element)
                return;

            var elementWithFocusOld = elementWithFocus;
            elementWithFocus = null;

            SetIsKeyboardFocusWithin(elementWithFocusOld, false);

            var dobj = elementWithFocusOld as DependencyObject;
            if (dobj != null)
            {
                var keyboard = Ultraviolet.GetInput().GetKeyboard();

                var lostFocusData = RoutedEventData.Retrieve(dobj);
                Keyboard.RaiseLostKeyboardFocus(dobj, keyboard, elementWithFocusOld, null, lostFocusData);
            }

            UpdateIsDefaulted();

            wasFocusMostRecentlyChangedByKeyboardOrGamePad = false;

            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Releases the mouse from the element that is currently capturing it.
        /// </summary>
        public void ReleaseMouse()
        {
            mouseCursorTracker.Release();
        }

        /// <summary>
        /// Assigns mouse capture to the specified element.
        /// </summary>
        /// <param name="element">The element to which to assign mouse capture.</param>
        /// <param name="mode">The mouse capture mode to apply.</param>
        /// <returns><see langword="true"/> if the mouse was successfully captured; otherwise, <see langword="false"/>.</returns>
        public Boolean CaptureMouse(IInputElement element, CaptureMode mode)
        {
            return mouseCursorTracker.Capture(element, mode);
        }

        /// <summary>
        /// Releases a touch from the element that is currently capturing it.
        /// </summary>
        /// <param name="touchID">The unique identifier of the touch to release.</param>
        public void ReleaseTouch(Int64 touchID)
        {
            touchCursorTrackers?.GetTrackerByTouchID(touchID)?.Release();
        }

        /// <summary>
        /// Assigns touch capture to the specified element.
        /// </summary>
        /// <param name="element">The element to which to assign touch capture.</param>
        /// <param name="touchID">The unique identifier of the touch to capture.</param>
        /// <param name="mode">The touch capture mode to apply.</param>
        /// <returns><see langword="true"/> if the touch was successfully captured; otherwise, <see langword="false"/>.</returns>
        public Boolean CaptureTouch(IInputElement element, Int64 touchID, CaptureMode mode)
        {
            return touchCursorTrackers?.GetTrackerByTouchID(touchID)?.Capture(element, mode) ?? false;
        }

        /// <summary>
        /// Releases all active touches from the elements that are currently capturing them.
        /// </summary>
        public void ReleaseAllTouches()
        {
            touchCursorTrackers?.ReleaseAll();
        }

        /// <summary>
        /// Assigns touch capture for all active touches to the specified element.
        /// </summary>
        /// <param name="element">The element to which to assign touch capture.</param>
        /// <param name="mode">The touch capture mode to apply.</param>
        /// <returns><see langword="true"/> if all active touches were successfully captured; otherwise, <see langword="false"/>.</returns>
        public Boolean CaptureAllTouches(IInputElement element, CaptureMode mode)
        {
            return touchCursorTrackers?.CaptureAll(element, mode) ?? false;
        }

        /// <summary>
        /// Releases new touch capture from the element that is currently capturing it.
        /// </summary>
        public void ReleaseNewTouches()
        {
            if (elementWithNewTouchCapture == null)
                return;

            var hadCapture = elementWithNewTouchCapture;
            elementWithNewTouchCapture = null;
            newTouchCaptureMode = CaptureMode.None;

            var uiElement = hadCapture as UIElement;
            if (uiElement != null)
            {
                SetAreNewTouchesCapturedWithin(uiElement, false);

                var lostNewTouchCaptureData = RoutedEventData.Retrieve(uiElement);
                Touch.RaiseLostNewTouchCapture(uiElement, lostNewTouchCaptureData);
            }
        }

        /// <summary>
        /// Assigns new touch capture to the specified element.
        /// </summary>
        /// <param name="element">The element to which to assign new touch capture.</param>
        /// <param name="mode">The touch capture mode to apply.</param>
        /// <returns><see langword="true"/> if new touches were successfully captured; otherwise, <see langword="false"/>.</returns>
        public Boolean CaptureNewTouches(IInputElement element, CaptureMode mode)
        {
            if ((element != null && mode == CaptureMode.None) || (element == null && mode != CaptureMode.None))
                throw new ArgumentException(nameof(mode));

            if (elementWithNewTouchCapture == element)
                return true;

            if (!(element?.IsValidForInput() ?? false))
                return false;

            if (elementWithNewTouchCapture != null)
                ReleaseNewTouches();

            elementWithNewTouchCapture = element;
            newTouchCaptureMode = mode;

            var uiElement = elementWithNewTouchCapture as UIElement;
            if (uiElement != null)
            {
                SetAreNewTouchesCapturedWithin(uiElement, true);

                var gotNewTouchCaptureData = RoutedEventData.Retrieve(uiElement);
                Touch.RaiseGotNewTouchCapture(uiElement, gotNewTouchCaptureData);
            }

            return true;
        }

        /// <summary>
        /// Gets the element object the view's namescope which has the specified identifying name.
        /// </summary>
        /// <param name="name">The identifying name of the object to retrieve.</param>
        /// <returns>The object with the specified identifying name, or <see langword="null"/> if no such object exists.</returns>
        public Object FindName(String name)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            return namescope.FindName(name);
        }

        /// <summary>
        /// Performs a hit test against the view at the specified point in screen space.
        /// </summary>
        /// <param name="x">The x-coordinate in screen space to evaluate.</param>
        /// <param name="y">The y-coordinate in screen space to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> in the view which contains the specified point, or <see langword="null"/>.</returns>
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
        /// <returns>The topmost <see cref="Visual"/> in the view which contains the specified point, or <see langword="null"/>.</returns>
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
        /// <returns>The topmost <see cref="Visual"/> in the view which contains the specified point, or <see langword="null"/>.</returns>
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
        /// <returns>The topmost <see cref="Visual"/> in the view which contains the specified point, or <see langword="null"/>.</returns>
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
        /// <returns>The topmost <see cref="Visual"/> in the view which contains the specified point, or <see langword="null"/>.</returns>
        public Visual HitTest(Double x, Double y)
        {
            var popup = default(Popup);

            return HitTestInternal(new Point2D(x, y), out popup);
        }

        /// <summary>
        /// Performs a hit test against the view at the specified point in device-independent view space.
        /// </summary>
        /// <param name="point">The point in device-independent view space to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> in the view which contains the specified point, or <see langword="null"/>.</returns>
        public Visual HitTest(Point2D point)
        {
            var popup = default(Popup);

            return HitTestInternal(point, out popup);
        }

        /// <summary>
        /// Gets the element that is currently under the specified touch.
        /// </summary>
        /// <param name="touchID">The unique identifier of the touch to evaluate.</param>
        /// <returns>The element that is currently under the specified touch,
        /// or <see langword="null"/> if no element is under the touch.</returns>
        public IInputElement GetElementUnderTouch(Int64 touchID)
        {
            return touchCursorTrackers?.GetTrackerByTouchID(touchID)?.ElementUnderCursor;
        }

        /// <summary>
        /// Gets the element that is currently capturing the specified touch.
        /// </summary>
        /// <param name="touchID">The unique identifier of the touch to evaluate.</param>
        /// <returns>The element that is currently capturing the specified touch,
        /// or <see langword="null"/> if no element is capturing the touch.</returns>
        public IInputElement GetElementWithTouchCapture(Int64 touchID)
        {
            return touchCursorTrackers?.GetTrackerByTouchID(touchID)?.ElementWithCapture;
        }

        /// <summary>
        /// Sets the view's style sheet.
        /// </summary>
        /// <param name="styleSheet">The view's style sheet.</param>
        public void SetStyleSheet(UvssDocument styleSheet)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            HookOnGlobalStyleSheetChanged();

            this.localStyleSheet = styleSheet;
            UpdateCombinedStyleSheet();

            LoadViewResources(combinedStyleSheet);

            if (viewIsOpen)
            {
                layoutRoot.InvalidateStyle(true);
                layoutRoot.Style(combinedStyleSheet);
            }
        }

        /// <summary>
        /// Loads the specified resource from the global content manager.
        /// </summary>
        /// <typeparam name="TResource">The type of resource to load.</typeparam>
        /// <param name="resource">The resource to load.</param>
        /// <param name="asset">The asset identifier that specifies which resource to load.</param>
        public void LoadGlobalResource<TResource>(FrameworkResource<TResource> resource, AssetID asset) where TResource : class
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (resource == null || GlobalContent == null)
                return;

            resource.Load(GlobalContent, asset, Display?.DensityBucket ?? ScreenDensityBucket.Desktop);
        }

        /// <summary>
        /// Loads the specified resource from the local content manager.
        /// </summary>
        /// <typeparam name="TResource">The type of resource to load.</typeparam>
        /// <param name="resource">The resource to load.</param>
        /// <param name="asset">The asset identifier that specifies which resource to load.</param>
        public void LoadLocalResource<TResource>(FrameworkResource<TResource> resource, AssetID asset) where TResource : class
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (resource == null || LocalContent == null)
                return;

            resource.Load(LocalContent, asset, Display?.DensityBucket ?? ScreenDensityBucket.Desktop);
        }

        /// <summary>
        /// Loads the specified sourced image.
        /// </summary>
        /// <param name="image">The identifier of the image to load.</param>
        public void LoadImage(SourcedImage image)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (image.Resource == null)
                return;

            switch (image.Source)
            {
                case AssetSource.Global:
                    if (GlobalContent != null)
                    {
                        image.Load(GlobalContent, Display?.DensityBucket ?? ScreenDensityBucket.Desktop);
                    }
                    break;

                case AssetSource.Local:
                    if (LocalContent != null)
                    {
                        image.Load(LocalContent, Display?.DensityBucket ?? ScreenDensityBucket.Desktop);
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Loads the specified sourced cursor.
        /// </summary>
        /// <param name="cursor">The identifier of the cursor to load.</param>
        public void LoadCursor(SourcedCursor cursor)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (cursor.Resource == null)
                return;

            switch (cursor.Source)
            {
                case AssetSource.Global:
                    if (GlobalContent != null)
                    {
                        cursor.Load(GlobalContent, Display?.DensityBucket ?? ScreenDensityBucket.Desktop);
                    }
                    break;

                case AssetSource.Local:
                    if (LocalContent != null)
                    {
                        cursor.Load(LocalContent, Display?.DensityBucket ?? ScreenDensityBucket.Desktop);
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Loads the specified sourced resource.
        /// </summary>
        /// <typeparam name="TResource">The type of resource to load.</typeparam>
        /// <param name="resource">The identifier of the resource to load.</param>
        public void LoadResource<TResource>(SourcedResource<TResource> resource) where TResource : class
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (resource.Resource == null)
                return;

            switch (resource.Source)
            {
                case AssetSource.Global:
                    if (GlobalContent != null)
                    {
                        resource.Load(GlobalContent, Display?.DensityBucket ?? ScreenDensityBucket.Desktop);
                    }
                    break;

                case AssetSource.Local:
                    if (LocalContent != null)
                    {
                        resource.Load(LocalContent, Display?.DensityBucket ?? ScreenDensityBucket.Desktop);
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Loads the resource represented by the specified asset identifier.
        /// </summary>
        /// <typeparam name="TResource">The type of resource to load.</typeparam>
        /// <param name="id">The identifier of the resource to load.</param>
        /// <returns>The resource that was loaded, or <see langword="null"/> if the resource could not be loaded.</returns>
        public TResource LoadResource<TResource>(SourcedAssetID id) where TResource : class
        {
            return LoadResource<TResource>(id.AssetID, id.AssetSource);
        }

        /// <summary>
        /// Loads the resource represented by the specified asset identifier.
        /// </summary>
        /// <typeparam name="TResource">The type of resource to load.</typeparam>
        /// <param name="id">The identifier of the resource to load.</param>
        /// <param name="source">The source from which to load the asset.</param>
        /// <returns>The resource that was loaded, or <see langword="null"/> if the resource could not be loaded.</returns>
        public TResource LoadResource<TResource>(AssetID id, AssetSource source) where TResource : class
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (!id.IsValid)
                return null;

            var content = (source == AssetSource.Global) ?
                GlobalContent : LocalContent;

            if (content == null)
                return null;

            return content.Load<TResource>(id, Display?.DensityBucket ?? ScreenDensityBucket.Desktop);
        }

        /// <summary>
        /// Loads the sprite animation represented by the specified sprite animation identifier.
        /// </summary>
        /// <param name="id">The identifier of the resource to load.</param>
        /// <returns>The resource that was loaded, or <see langword="null"/> if the resource could not be loaded.</returns>
        public SpriteAnimation LoadResource(SourcedSpriteAnimationID id)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (!id.SpriteAnimationID.IsValid)
                return null;

            var content = (id.SpriteSource == AssetSource.Global) ?
                GlobalContent : LocalContent;

            if (content == null)
                return null;

            return content.Load(id.SpriteAnimationID, Display?.DensityBucket ?? ScreenDensityBucket.Desktop);
        }

        /// <inheritdoc/>
        public override TViewModel GetViewModel<TViewModel>()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var vm = ViewModel;
            if (vm is IDataSourceWrapper vmWrapper)
            {
                return vmWrapper.WrappedDataSource as TViewModel;
            }
            return vm as TViewModel;
        }

        /// <summary>
        /// Searches the view's associated style sheet for a storyboard with the specified name.
        /// </summary>
        /// <param name="name">The name of the storyboard to retrieve.</param>
        /// <returns>The <see cref="Storyboard"/> with the specified name, or <see langword="null"/> if the specified storyboard does not exist.</returns>
        public Storyboard FindStoryboard(String name)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            if (StyleSheet != null)
            {
                return StyleSheet.GetStoryboardInstance(name);
            }

            return null;
        }

        /// <summary>
        /// Gets the style sheet that is currently applied to this view.
        /// </summary>
        public UvssDocument StyleSheet => combinedStyleSheet;

        /// <summary>
        /// Gets the element that is currently under the mouse cursor.
        /// </summary>
        public IInputElement ElementUnderMouse => mouseCursorTracker.ElementUnderCursor;

        /// <summary>
        /// Gets the element that currently has mouse capture.
        /// </summary>
        public IInputElement ElementWithMouseCapture => mouseCursorTracker.ElementWithCapture;

        /// <summary>
        /// Gets the element that currently has new touch capture.
        /// </summary>
        public IInputElement ElementWithNewTouchCapture => elementWithNewTouchCapture;

        /// <summary>
        /// Gets the element that currently has focus.
        /// </summary>
        public IInputElement ElementWithFocus => elementWithFocus;

        /// <summary>
        /// Gets the root element of the view's layout.
        /// </summary>
        public PresentationFoundationViewRoot LayoutRoot => layoutRoot;

        /// <summary>
        /// Gets the view's global resource collection.
        /// </summary>
        public PresentationFoundationViewResources Resources => resources;
        
        /// <summary>
        /// Performs a hit test against the layout root and any active popup windows.
        /// </summary>
        /// <param name="point">The point in device-independent screen space to test.</param>
        /// <param name="popup">The popup that contains the resulting visual.</param>
        /// <returns>The topmost <see cref="Visual"/> that contains the specified point, 
        /// or <see langword="null"/> if none of the items in the layout contain the point.</returns>
        internal Visual HitTestInternal(Point2D point, out Popup popup)
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
        /// Updates the element for which the view is displaying a tool tip.
        /// </summary>
        /// <param name="element">The element for which the view should display a tool tip</param>
        internal void UpdateToolTipElement(IInputElement element)
        {
            toolTipElementPrev = toolTipElement;
            toolTipElement = GetToolTipElement(element);

            if (toolTipElement != toolTipElementPrev)
                HandleToolTipElementChanged(toolTipElementPrev as UIElement, toolTipElement as UIElement);
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
        /// Registers a button as one of the view's default buttons.
        /// </summary>
        /// <param name="button">The button to register.</param>
        internal void RegisterDefaultButton(Button button)
        {
            Contract.Require(button, nameof(button));

            var weakReference = WeakReferencePool.Instance.Retrieve();
            weakReference.Target = button;

            defaultButtons.Add(weakReference);
        }

        /// <summary>
        /// Registers a button as one of the view's cancel buttons.
        /// </summary>
        /// <param name="button">The button to register.</param>
        internal void RegisterCancelButton(Button button)
        {
            Contract.Require(button, nameof(button));

            var weakReference = WeakReferencePool.Instance.Retrieve();
            weakReference.Target = button;

            cancelButtons.Add(weakReference);
        }

        /// <summary>
        /// Unregisters a button as one of the view's default buttons.
        /// </summary>
        /// <param name="button">The button to unregister.</param>
        internal void UnregisterDefaultButton(Button button)
        {
            Contract.Require(button, nameof(button));

            for (int i = 0; i < defaultButtons.Count; i++)
            {
                var reference = defaultButtons[i].Target;
                if (reference == button)
                {
                    defaultButtons.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Unregisters a button as one of the view's cancel buttons.
        /// </summary>
        /// <param name="button">The button to unregister.</param>
        internal void UnregisterCancelButton(Button button)
        {
            Contract.Require(button, nameof(button));

            for (int i = 0; i < cancelButtons.Count; i++)
            {
                var reference = cancelButtons[i].Target;
                if (reference == button)
                {
                    cancelButtons.RemoveAt(i);
                    break;
                }
            }
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

        /// <summary>
        /// Gets the view's text shaper.
        /// </summary>
        internal TextShaper TextShaper { get; }

        /// <summary>
        /// Gets a value indicating whether this view's focused element was most recently changed by 
        /// a keyboard or game pad device.
        /// </summary>
        internal Boolean FocusWasMostRecentlyChangedByKeyboardOrGamePad
        {
            get { return wasFocusMostRecentlyChangedByKeyboardOrGamePad; }
        }

        /// <inheritdoc/>
        protected override void OnOpening()
        {
            viewIsOpen = true;

            layoutRoot.Prepare();
            layoutRoot.InvalidateStyle(true);
            layoutRoot.Style(combinedStyleSheet);
            UpdateLayout();

            ImmediatelyDigestVisualTree(layoutRoot);

            var defaultButton = GetFirstDefaultButton();
            if (defaultButton != null)
            {
                FocusElement(defaultButton);
            }
            else
            {
                if (elementWithFocus != null)
                {
                    BlurElement(elementWithFocus);
                }
            }

            EnsureIsLoaded();

            RaiseViewLifecycleEvent(layoutRoot, View.OpeningEvent);
        }

        /// <inheritdoc/>
        protected override void OnOpened()
        {
            RaiseViewLifecycleEvent(layoutRoot, View.OpenedEvent);
        }

        /// <inheritdoc/>
        protected override void OnClosing()
        {
            RaiseViewLifecycleEvent(layoutRoot, View.ClosingEvent);
        }

        /// <inheritdoc/>
        protected override void OnClosed()
        {
            if (viewIsOpen)
                RaiseViewLifecycleEvent(layoutRoot, View.ClosedEvent);

            Cleanup();
            layoutRoot.Cleanup();

            viewIsOpen = false;
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (!Ultraviolet.Disposed)
                {
                    OnClosed();

                    UnhookGlobalStyleSheetChanged();
                    UnhookKeyboardEvents();
                    UnhookMouseEvents();
                    UnhookTouchEvents();
                    UnhookGamePadEvents();

                    foreach (var weakReference in defaultButtons)
                        WeakReferencePool.Instance.Release(weakReference);

                    foreach (var weakReference in cancelButtons)
                        WeakReferencePool.Instance.Release(weakReference);

                    defaultButtons.Clear();
                    cancelButtons.Clear();

                    PresentationFoundation.Instance.RemoveFromQueues(this);
                }

                if (TextShaper != null)
                    TextShaper.Dispose();
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
        protected override void OnViewWindowChanged(IUltravioletWindow oldWindow, IUltravioletWindow newWindow)
        {
            if (newWindow == null)
            {
                PresentationFoundation.Instance.RemoveFromQueues(this);
            }
            else
            {
                if (oldWindow == null)
                {
                    PresentationFoundation.Instance.RestoreToQueues(this);
                }

                SetStyleSheet(localStyleSheet);
            }
            base.OnViewWindowChanged(oldWindow, newWindow);
        }

        /// <inheritdoc/>
        protected override void OnViewModelChanged()
        {
            if (layoutRoot != null && layoutRoot.IsInitialized)
            {
                layoutRoot.CacheLayoutParameters();

                if (ViewModel != null)
                    namescope.PopulateFieldsFromRegisteredElements(ViewModel);
            }
            base.OnViewModelChanged();
        }

        /// <inheritdoc/>
        protected override void OnViewSizeChanged()
        {
            UpdateLayout();
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
                var display = element.View.Display;
                var bounds = element.TransformedVisualBounds;
                var position = (Vector2)(Point2)display.DipsToPixels(bounds.Location);
                var width = (Int32)display.DipsToPixels(bounds.Width);
                var height = (Int32)display.DipsToPixels(bounds.Height);
                var color = Diagnostics.GetDrawVisualBoundsColor(element);
                drawingContext.RawDrawImage(Diagnostics.BoundingBoxImage, position, width, height, color);
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
        /// Raises the specified view lifecycle event for an object and all of its descendants.
        /// </summary>
        /// <param name="dobj">The dependency object for which to raise the event.</param>
        /// <param name="evt">The routed event to raise for the object.</param>
        private static void RaiseViewLifecycleEvent(DependencyObject dobj, RoutedEvent evt)
        {
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(evt);
            var evtData = RoutedEventData.Retrieve(dobj);
            evtDelegate(dobj, evtData);

            VisualTreeHelper.ForEachChild(dobj, evt, (child, state) =>
            {
                RaiseViewLifecycleEvent(child, (RoutedEvent)state);
            });
        }

        /// <summary>
        /// Raises the <see cref="View.ViewModelChangedEvent"/> for an object and all of its descendants.
        /// </summary>
        /// <param name="dobj">The dependency object for which to raise the event.</param>
        private static void RaiseViewModelChangedEvent(DependencyObject dobj)
        {
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(View.ViewModelChangedEvent);
            var evtData = RoutedEventData.Retrieve(dobj);
            evtDelegate(dobj, evtData);

            VisualTreeHelper.ForEachChild(dobj, null, (child, state) =>
            {
                RaiseViewModelChangedEvent(child);
            });
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
            if (this.combinedStyleSheet != null)
                this.combinedStyleSheet.Clear();

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();

            var gssDensity = Display?.DensityBucket ?? ScreenDensityBucket.Desktop;
            var gssStyleSheet = upf.ResolveGlobalStyleSheet(gssDensity);
            if (gssStyleSheet != null)
                this.combinedStyleSheet.Append(gssStyleSheet);

            if (this.localStyleSheet != null)
                this.combinedStyleSheet.Append(this.localStyleSheet);
        }

        /// <summary>
        /// Performs a measure and arrange on the layout root.
        /// </summary>
        private void UpdateLayout()
        {
            if (!viewIsOpen)
                return;

            var dipsArea = Display.PixelsToDips(Area);
            layoutRoot.Measure(dipsArea.Size);
            layoutRoot.Arrange(dipsArea);

            PresentationFoundation.Instance.PerformLayout();
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
                var keyboard = input.GetKeyboard();
                keyboard.KeyPressed += keyboard_KeyPressed;
                keyboard.KeyReleased += keyboard_KeyReleased;
                keyboard.TextInput += keyboard_TextInput;
                keyboard.TextEditing += keyboard_TextEditing;
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
                var mouse = input.GetMouse();
                mouse.Moved += mouse_Moved;
                mouse.ButtonPressed += mouse_ButtonPressed;
                mouse.ButtonReleased += mouse_ButtonReleased;
                mouse.Click += mouse_Click;
                mouse.DoubleClick += mouse_DoubleClick;
                mouse.WheelScrolled += mouse_WheelScrolled;
            }
        }

        /// <summary>
        /// Hooks into Ultraviolet's touch input events.
        /// </summary>
        private void HookTouchEvents()
        {
            var device = Ultraviolet.GetInput().GetPrimaryTouchDevice();
            if (device != null)
            {
                touchCursorTrackers = new CursorTrackerTouchCollection(this);
                device.TouchMotion += touch_TouchMotion;
                device.TouchDown += touch_TouchDown;
                device.TouchUp += touch_TouchUp;
                device.Tap += touch_Tap;
                device.LongPress += touch_LongPress;
                device.MultiGesture += touch_MultiGesture;
                device.DollarGesture += touch_DollarGesture;
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
                var keyboard = input.GetKeyboard();
                keyboard.KeyPressed -= keyboard_KeyPressed;
                keyboard.KeyReleased -= keyboard_KeyReleased;
                keyboard.TextInput -= keyboard_TextInput;
                keyboard.TextEditing -= keyboard_TextEditing;
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
                var mouse = input.GetMouse();
                mouse.Moved -= mouse_Moved;
                mouse.ButtonPressed -= mouse_ButtonPressed;
                mouse.ButtonReleased -= mouse_ButtonReleased;
                mouse.Click -= mouse_Click;
                mouse.DoubleClick -= mouse_DoubleClick;
                mouse.WheelScrolled -= mouse_WheelScrolled;
            }
        }

        /// <summary>
        /// Unhooks from Ultraviolet's touch input events.
        /// </summary>
        private void UnhookTouchEvents()
        {
            var device = Ultraviolet.GetInput().GetPrimaryTouchDevice();
            if (device != null)
            {
                touchCursorTrackers.Clear();
                touchCursorTrackers = null;

                device.TouchMotion -= touch_TouchMotion;
                device.TouchDown -= touch_TouchDown;
                device.TouchUp -= touch_TouchUp;
                device.Tap -= touch_Tap;
                device.LongPress -= touch_LongPress;
                device.MultiGesture -= touch_MultiGesture;
                device.DollarGesture -= touch_DollarGesture;
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
        private void HandleUserInput(UltravioletTime time)
        {
            var isInputPossibleThisFrame = IsInputEnabledAndAllowed;
            if (isInputPossibleThisFrame && !wasInputPossibleLastFrame)
            {
                wasInputPossibleLastFrame = isInputPossibleThisFrame;
                if (!isInputPossibleThisFrame)
                {
                    HandleInputDisabledOrDisallowed();
                }
            }

            if (!IsInputEnabledAndAllowed)
                return;

            if (Ultraviolet.GetInput().IsKeyboardSupported())
                HandleKeyboardInput();

            if (Ultraviolet.GetInput().IsMouseSupported())
                HandleMouseInput(time);

            if (Ultraviolet.GetInput().IsTouchSupported())
                HandleTouchInput();
        }

        /// <summary>
        /// Handles keyboard input.
        /// </summary>
        private void HandleKeyboardInput()
        {

        }

        /// <summary>
        /// Handles mouse input.
        /// </summary>
        private void HandleMouseInput(UltravioletTime time)
        {
            mouseCursorTracker.Update();
            UpdateToolTip(time);
        }

        /// <summary>
        /// Handles touch input.
        /// </summary>
        private void HandleTouchInput()
        {
            if (!(elementWithNewTouchCapture?.IsValidForInput() ?? false))
                ReleaseNewTouches();

            touchCursorTrackers?.Update();
        }

        /// <summary>
        /// Called when input is disabled or becomes disallowed.
        /// </summary>
        private void HandleInputDisabledOrDisallowed()
        {
            CloseToolTip();
        }
        
        /// <summary>
        /// Updates the display state of the view's tooltip.
        /// </summary>
        private void UpdateToolTip(UltravioletTime time)
        {
            if (toolTipElement != null)
            {
                if (layoutRoot.ToolTipPopup.IsOpen)
                {
                    UpdateToolTip_PopupIsOpen(time, toolTipElement);
                }
                else
                {
                    UpdateToolTip_PopupIsClosed(time, toolTipElement);
                }
            }

            if (!layoutRoot.ToolTipPopup.IsOpen)
                timeSinceToolTipWasClosed += time.ElapsedTime.TotalMilliseconds;
        }

        /// <summary>
        /// Updates the state of the view's tooltip while the tooltip popup is closed.
        /// </summary>
        private void UpdateToolTip_PopupIsClosed(UltravioletTime time, UIElement element)
        {
            if (toolTipWasShownForCurrentElement)
                return;

            if (!element.IsEnabled && !ToolTipService.GetShowOnDisabled(element))
                return;

            var content = ToolTipService.GetToolTip(element);
            if (content == null)
                return;

            var tooltipInitialDelay = ToolTipService.GetInitialShowDelay(element);
            if (timeUntilToolTipWillOpen < tooltipInitialDelay)
            {
                timeUntilToolTipWillOpen += time.ElapsedTime.TotalMilliseconds;
                if (timeUntilToolTipWillOpen >= tooltipInitialDelay)
                {
                    ShowToolTipForElement(element);
                }
            }
        }

        /// <summary>
        /// Updates the state of the view's tooltip while the tooltip popup is open.
        /// </summary>
        private void UpdateToolTip_PopupIsOpen(UltravioletTime time, UIElement element)
        {
            timeSinceToolTipWasOpened += time.ElapsedTime.TotalMilliseconds;
            if (timeSinceToolTipWasOpened >= ToolTipService.GetShowDuration(element))
            {
                CloseToolTip();
            }
        }

        /// <summary>
        /// Called when the tooltip element changes due to mouse movement.
        /// </summary>
        /// <param name="uiToolTipElementOld">The previous tooltip element.</param>
        /// <param name="uiToolTipElementNew">The new tooltip element.</param>
        private void HandleToolTipElementChanged(UIElement uiToolTipElementOld, UIElement uiToolTipElementNew)
        {
            toolTipWasShownForCurrentElement = false;

            if (uiToolTipElementNew != null && timeSinceToolTipWasClosed <= ToolTipService.GetBetweenShowDelay(uiToolTipElementNew))
            {
                if (ShowToolTipForElement(uiToolTipElementNew))
                    return;
            }

            timeUntilToolTipWillOpen = 0.0;
            timeSinceToolTipWasClosed = 0.0;

            layoutRoot.ToolTip.Content = null;
            layoutRoot.ToolTipPopup.IsOpen = false;
        }

        /// <summary>
        /// Closes the tooltip popup, if it is open.
        /// </summary>
        private void CloseToolTip()
        {
            if (!layoutRoot.ToolTipPopup.IsOpen)
                return;

            layoutRoot.ToolTipPopup.IsOpen = false;

            timeSinceToolTipWasOpened = 0.0;
            timeSinceToolTipWasClosed = 0.0;
            timeUntilToolTipWillOpen = 0.0;

            if (toolTipElementDisplayed != null)
            {
                var toolTipClosingData = RoutedEventData.Retrieve(toolTipElementDisplayed);
                ToolTipService.RaiseToolTipClosing(toolTipElementDisplayed, toolTipClosingData);

                toolTipElementDisplayed.SetValue(ToolTipService.IsOpenPropertyKey, false);
                toolTipElementDisplayed = null;
            }
        }

        /// <summary>
        /// Updates the view's list of default and cancel buttons.
        /// </summary>
        private void UpdateDefaultAndCancelButtonReferences()
        {
            for (int i = 0; i < defaultButtons.Count; i++)
            {
                if (defaultButtons[i].IsAlive)
                    continue;

                WeakReferencePool.Instance.Release(defaultButtons[i]);
                defaultButtons.RemoveAt(i--);
            }

            for (int i = 0; i < cancelButtons.Count; i++)
            {
                if (cancelButtons[i].IsAlive)
                    continue;

                WeakReferencePool.Instance.Release(cancelButtons[i]);
                cancelButtons.RemoveAt(i--);
            }
        }

        /// <summary>
        /// Updates the value of <see cref="Button.IsDefaulted"/> for the view's default buttons.
        /// </summary>
        private void UpdateIsDefaulted()
        {
            foreach (var weakReference in defaultButtons)
            {
                var button = weakReference.Target as Button;
                if (button == null)
                    continue;

                button.UpdateIsDefaulted();
            }
        }

        /// <summary>
        /// Activates the next default or cancel button from the specified list of buttons.
        /// </summary>
        /// <param name="buttons">The list of buttons that contains the button to activate.</param>
        private void ActivateDefaultOrCancelButton(List<WeakReference> buttons)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                var target = buttons[i].Target as Button;
                if (target == null)
                    continue;

                if (target.IsEnabled && target.Visibility == Visibility.Visible)
                {
                    target.HandleDefaultOrCancelActivated();
                }
            }
        }

        /// <summary>
        /// Performs an immediate digest on all dependencies properties defined by the specified 
        /// element and its visual descendants.
        /// </summary>
        /// <param name="dobj">The dependency object to digest.</param>
        private void ImmediatelyDigestVisualTree(DependencyObject dobj)
        {
            dobj.DigestImmediately();

            VisualTreeHelper.ForEachChild(dobj, this, (child, state) =>
            {
                ((PresentationFoundationView)state).ImmediatelyDigestVisualTree(child);
            });
        }

        /// <summary>
        /// Performs view cleanup after the view is closed.
        /// </summary>
        private void Cleanup()
        {
            CleanupInput();
            CleanupToolTip();
        }

        /// <summary>
        /// Cleans up mouse-related state.
        /// </summary>
        private void CleanupInput()
        {
            mouseCursorTracker.Release();
            mouseCursorTracker.Cleanup();

            if (touchCursorTrackers != null)
                touchCursorTrackers.Clear();

            if (elementWithFocus != null)
                BlurElement(elementWithFocus);

            wasInputPossibleLastFrame = false;
            wasFocusMostRecentlyChangedByKeyboardOrGamePad = false;
        }

        /// <summary>
        /// Cleans up tool-tip-related state
        /// </summary>
        private void CleanupToolTip()
        {
            CloseToolTip();

            toolTipElementDisplayed = null;
            toolTipElementPrev = null;
            toolTipElement = null;

            timeUntilToolTipWillOpen = 0;
            timeSinceToolTipWasOpened = 0;
            timeSinceToolTipWasOpened = 0;

            toolTipWasShownForCurrentElement = false;
        }

        /// <summary>
        /// Converts normalized touch device coordinates to view-relative coordinates.
        /// </summary>
        private Point2D GetTouchCoordinates(Single x, Single y)
        {
            var windowSize = Window.DrawableSize;

            var xRelativeToWindow = windowSize.Width * x;
            var yRelativeToWindow = windowSize.Height * y;

            var posRelativeToCompositor =
                Window.Compositor.WindowToPoint((Int32)xRelativeToWindow, (Int32)yRelativeToWindow);

            var xRelativeToView = posRelativeToCompositor.X - this.X;
            var yRelativeToView = posRelativeToCompositor.Y - this.Y;

            return (Point2D)Display.PixelsToDips(new Vector2(xRelativeToView, yRelativeToView));
        }

        /// <summary>
        /// Converts a normalized touch delta to device-independent pixels.
        /// </summary>
        private Point2D GetTouchDelta(Single x, Single y)
        {
            var windowSize = Window.DrawableSize;

            var xPixels = windowSize.Width * x;
            var yPixels = windowSize.Height * y;

            return (Point2D)Display.PixelsToDips(new Vector2(xPixels, yPixels));
        }
        
        /// <summary>
        /// Displays the specified element's tooltip.
        /// </summary>
        /// <param name="uiToolTipElement">The element for which to display a tooltip.</param>
        /// <returns><see langword="true"/> if the element's tooltip was displayed; otherwise, <see langword="false"/>.</returns>
        private Boolean ShowToolTipForElement(UIElement uiToolTipElement)
        {
            if (Ultraviolet.Platform == UltravioletPlatform.Android || Ultraviolet.Platform == UltravioletPlatform.iOS)
                return false;

            var content = ToolTipService.GetToolTip(uiToolTipElement);
            if (content == null)
                return false;

            var toolTipOpeningData = RoutedEventData.Retrieve(uiToolTipElement, autorelease: false);
            ToolTipService.RaiseToolTipOpening(uiToolTipElement, toolTipOpeningData);
            var toolTipSuppressed = toolTipOpeningData.Handled;
            toolTipOpeningData.Release();

            if (!toolTipSuppressed)
            {
                layoutRoot.ToolTip.Content = content;
                layoutRoot.ToolTipPopup.VerticalOffset = ToolTipService.GetVerticalOffset(uiToolTipElement);
                layoutRoot.ToolTipPopup.HorizontalOffset = ToolTipService.GetHorizontalOffset(uiToolTipElement);
                layoutRoot.ToolTipPopup.Placement = ToolTipService.GetPlacement(uiToolTipElement);
                layoutRoot.ToolTipPopup.PlacementRectangle = ToolTipService.GetPlacementRectangle(uiToolTipElement);
                layoutRoot.ToolTipPopup.PlacementTarget = ToolTipService.GetPlacementTarget(uiToolTipElement);
                layoutRoot.ToolTipPopup.Effect = ToolTipService.GetHasDropShadow(uiToolTipElement) ? toolTipDropShadow : null;
                layoutRoot.ToolTipPopup.IsOpen = false;
                layoutRoot.ToolTipPopup.IsOpen = true;

                timeSinceToolTipWasClosed = 0.0;
                timeSinceToolTipWasOpened = 0.0;
                timeUntilToolTipWillOpen = 0.0;

                toolTipElementDisplayed = uiToolTipElement;
                toolTipElementDisplayed.SetValue(ToolTipService.IsOpenPropertyKey, true);
            }

            toolTipWasShownForCurrentElement = true;

            return true;
        }

        /// <summary>
        /// Starts at the specified element and moves up the visual tree to find
        /// the nearest ancestor which is focusable.
        /// </summary>
        /// <param name="start">The element at which to begin searching.</param>
        /// <returns>The nearest focusable element, or <see langword="null"/> if no focusable element was found.</returns>
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
        /// Navigates the visual tree starting at the specified element until an element is found which has a valid tooltip.
        /// </summary>
        /// <param name="start">The element at which to start navigating the visual tree.</param>
        /// <returns>The nearest ancestor of <paramref name="start"/> which has a valid tooltip that is ready for display.</returns>
        private UIElement GetToolTipElement(IInputElement start)
        {
            var current = start as DependencyObject;
            while (current != null)
            {
                var uiElement = current as UIElement;
                if (uiElement != null && ToolTipService.GetToolTip(current) != null && (uiElement.IsEnabled || ToolTipService.GetShowOnDisabled(uiElement)))
                {
                    return uiElement;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        /// <summary>
        /// Gets the first visible, enabled default button.
        /// </summary>
        private Button GetFirstDefaultButton()
        {
            return GetFirstDefaultOrCancelButton(defaultButtons);
        }

        /// <summary>
        /// Gets the first visible, enabled cancel button.
        /// </summary>
        private Button GetFirstCancelButton()
        {
            return GetFirstDefaultOrCancelButton(cancelButtons);
        }

        /// <summary>
        /// Gets the first visible, enabled button in the specified list of default or cancel buttons.
        /// </summary>
        private Button GetFirstDefaultOrCancelButton(List<WeakReference> buttons)
        {
            foreach (var weakReference in defaultButtons)
            {
                var defaultButton = weakReference.Target as Button;
                if (defaultButton == null)
                    continue;

                if (defaultButton.Visibility == Visibility.Visible && defaultButton.IsEnabled)
                {
                    return defaultButton;
                }
            }
            return null;
        }

        /// <summary>
        /// Sets the value of the <see cref="IInputElement.IsKeyboardFocusWithin"/> property on the specified element
        /// and all of its ancestors to the specified value.
        /// </summary>
        /// <param name="element">The element on which to set the property value.</param>
        /// <param name="value">The value to set on the element and its ancestors.</param>
        private void SetIsKeyboardFocusWithin(IInputElement element, Boolean value)
        {
            var focused = element as UIElement;
            if (focused != null)
                focused.IsKeyboardFocused = value;

            var current = element as DependencyObject;
            while (current != null)
            {
                var uiElement = current as UIElement;
                if (uiElement != null)
                    uiElement.IsKeyboardFocusWithin = value;

                current = VisualTreeHelper.GetParent(current);
            }
        }

        /// <summary>
        /// Sets the value of the <see cref="IInputElement.AreNewTouchesCapturedWithin"/> property on the specified element
        /// and all of its ancestors to the specified value.
        /// </summary>
        /// <param name="element">The element on which to set the property value.</param>
        /// <param name="value">The value to set on the element and its ancestors.</param>
        private void SetAreNewTouchesCapturedWithin(IInputElement element, Boolean value)
        {
            var captured = element as UIElement;
            if (captured != null)
                captured.AreNewTouchesCaptured = value;

            var current = element as DependencyObject;
            while (current != null)
            {
                var uiElement = current as UIElement;
                if (uiElement != null)
                    uiElement.AreNewTouchesCapturedWithin = value;

                current = VisualTreeHelper.GetParent(current);
            }
        }

        /// <summary>
        /// Handles the <see cref="KeyboardDevice.KeyPressed"/> event.
        /// </summary>
        private void keyboard_KeyPressed(IUltravioletWindow window, KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat)
        {
            if (!IsInputEnabledAndAllowed)
                return;

            var originalFocus = elementWithFocus;
            var performKeyNav = true;

            if (elementWithFocus != null)
            {
                var dobj = elementWithFocus as DependencyObject;
                if (dobj != null)
                {
                    var keyDownData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Keyboard.RaisePreviewKeyDown(dobj, device, key, ctrl, alt, shift, repeat, keyDownData);
                    Keyboard.RaiseKeyDown(dobj, device, key, ctrl, alt, shift, repeat, keyDownData);

                    performKeyNav = !keyDownData.Handled;

                    keyDownData.Release();
                }
            }

            if (performKeyNav)
            {
                if (!FocusNavigator.PerformNavigation(this, device, key, ctrl, alt, shift, repeat))
                {
                    switch (key)
                    {
                        case Key.Return:
                            ActivateDefaultOrCancelButton(defaultButtons);
                            break;

                        case Key.Escape:
                            ActivateDefaultOrCancelButton(cancelButtons);
                            break;
                    }
                }
            }

            if (originalFocus != elementWithFocus)
                wasFocusMostRecentlyChangedByKeyboardOrGamePad = true;
        }

        /// <summary>
        /// Handles the <see cref="KeyboardDevice.KeyReleased"/> event.
        /// </summary>
        private void keyboard_KeyReleased(IUltravioletWindow window, KeyboardDevice device, Key key)
        {
            if (!IsInputEnabledAndAllowed)
                return;

            if (elementWithFocus != null)
            {
                var originalFocus = elementWithFocus;

                var dobj = elementWithFocus as DependencyObject;
                if (dobj != null)
                {
                    var keyUpData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Keyboard.RaisePreviewKeyUp(dobj, device, key, keyUpData);
                    Keyboard.RaiseKeyUp(dobj, device, key, keyUpData);
                    keyUpData.Release();
                }

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = true;
            }

            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Handles the <see cref="KeyboardDevice.TextInput"/> event.
        /// </summary>
        private void keyboard_TextInput(IUltravioletWindow window, KeyboardDevice device)
        {
            if (!IsInputEnabledAndAllowed)
                return;

            if (elementWithFocus != null)
            {
                var originalFocus = elementWithFocus;

                var dobj = elementWithFocus as DependencyObject;
                if (dobj != null)
                {
                    var textInputData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Keyboard.RaisePreviewTextInput(dobj, device, textInputData);
                    Keyboard.RaiseTextInput(dobj, device, textInputData);
                    textInputData.Release();
                }

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = true;
            }
        }

        /// <summary>
        /// Handles the <see cref="KeyboardDevice.TextEditing"/> event.
        /// </summary>
        private void keyboard_TextEditing(IUltravioletWindow window, KeyboardDevice device)
        {
            if (!IsInputEnabledAndAllowed)
                return;

            if (elementWithFocus != null)
            {
                var originalFocus = elementWithFocus;

                var dobj = elementWithFocus as DependencyObject;
                if (dobj != null)
                {
                    var textEditingData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Keyboard.RaisePreviewTextEditing(dobj, device, textEditingData);
                    Keyboard.RaiseTextEditing(dobj, device, textEditingData);
                    textEditingData.Release();
                }

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = true;
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.Moved"/> event.
        /// </summary>
        private void mouse_Moved(IUltravioletWindow window, MouseDevice device, Int32 x, Int32 y, Int32 dx, Int32 dy)
        {
            if (window != Window || !IsInputEnabledAndAllowed)
                return;

            var recipient = mouseCursorTracker.ElementUnderCursor;
            if (recipient != null)
            {
                var originalFocus = elementWithFocus;

                var dipsX = Display.PixelsToDips(x);
                var dipsY = Display.PixelsToDips(y);
                var dipsDeltaX = Display.PixelsToDips(dx);
                var dipsDeltaY = Display.PixelsToDips(dy);

                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var mouseMoveData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Mouse.RaisePreviewMouseMove(dobj, device, dipsX, dipsY, dipsDeltaX, dipsDeltaY, mouseMoveData);
                    Mouse.RaiseMouseMove(dobj, device, dipsX, dipsY, dipsDeltaX, dipsDeltaY, mouseMoveData);
                    mouseMoveData.Release();
                }

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = false;
            }

            InvalidateCursor();
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.ButtonPressed"/> event.
        /// </summary>
        private void mouse_ButtonPressed(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (window != Window || !IsInputEnabledAndAllowed)
                return;

            CloseToolTip();

            mouseCursorTracker.Update();

            var recipient = mouseCursorTracker.ElementUnderCursor;
            if (recipient != null)
            {
                var originalFocus = elementWithFocus;

                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var mouseDownData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Mouse.RaisePreviewMouseDown(dobj, device, button, mouseDownData);
                    Mouse.RaiseMouseDown(dobj, device, button, mouseDownData);
                    mouseDownData.Release();                    
                }

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = false;
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.ButtonReleased"/> event.
        /// </summary>
        private void mouse_ButtonReleased(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (window != Window || !IsInputEnabledAndAllowed)
                return;

            var recipient = mouseCursorTracker.ElementUnderCursor;
            if (recipient != null)
            {
                var originalFocus = elementWithFocus;

                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var mouseUpData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Mouse.RaisePreviewMouseUp(dobj, device, button, mouseUpData);
                    Mouse.RaiseMouseUp(dobj, device, button, mouseUpData);
                    mouseUpData.Release();
                }

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = false;
            }

            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.Click"/> event.
        /// </summary>
        private void mouse_Click(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (window != Window || !IsInputEnabledAndAllowed)
                return;

            var recipient = mouseCursorTracker.ElementUnderCursor;
            if (recipient != null)
            {
                var originalFocus = elementWithFocus;

                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var mouseClickData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Mouse.RaisePreviewMouseClick(dobj, device, button, mouseClickData);
                    Mouse.RaiseMouseClick(dobj, device, button, mouseClickData);
                    mouseClickData.Release();
                }

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = false;
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.DoubleClick"/> event.
        /// </summary>
        private void mouse_DoubleClick(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (window != Window || !IsInputEnabledAndAllowed)
                return;

            var recipient = mouseCursorTracker.ElementUnderCursor;
            if (recipient != null)
            {
                var originalFocus = elementWithFocus;

                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var mouseDoubleClickData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Mouse.RaisePreviewMouseDoubleClick(dobj, device, button, mouseDoubleClickData);
                    Mouse.RaiseMouseDoubleClick(dobj, device, button, mouseDoubleClickData);
                    mouseDoubleClickData.Release();
                }

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = false;
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.WheelScrolled"/> event.
        /// </summary>
        private void mouse_WheelScrolled(IUltravioletWindow window, MouseDevice device, Int32 x, Int32 y)
        {
            if (window != Window || !IsInputEnabledAndAllowed)
                return;

            var recipient = mouseCursorTracker.ElementUnderCursor;
            if (recipient != null)
            {
                var originalFocus = elementWithFocus;

                var dipsX = Display.PixelsToDips(x);
                var dipsY = Display.PixelsToDips(y);

                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var mouseWheelData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Mouse.RaisePreviewMouseWheel(dobj, device, dipsX, dipsY, mouseWheelData);
                    Mouse.RaiseMouseWheel(dobj, device, dipsX, dipsY, mouseWheelData);
                    mouseWheelData.Release();
                }

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = false;
            }
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
            if (device.PlayerIndex != 0 || !IsInputEnabledAndAllowed)
                return;

            var recipient = elementWithFocus as DependencyObject;
            if (recipient != null)
            {
                var originalFocus = elementWithFocus;

                var gamePadAxisChangedData = RoutedEventData.Retrieve(recipient, autorelease: false);
                GamePad.RaisePreviewAxisChanged(recipient, device, axis, value, gamePadAxisChangedData);
                GamePad.RaiseAxisChanged(recipient, device, axis, value, gamePadAxisChangedData);
                gamePadAxisChangedData.Release();

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = true;
            }
        }

        /// <summary>
        /// Handles the <see cref="GamePadDevice.AxisPressed"/> event.
        /// </summary>
        private void gamePad_AxisPressed(GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat)
        {
            if (device.PlayerIndex != 0 || !IsInputEnabledAndAllowed)
                return;

            var originalFocus = elementWithFocus;

            var recipient = elementWithFocus as DependencyObject;
            if (recipient != null)
            {
                var gamePadAxisPressedData = RoutedEventData.Retrieve(recipient, autorelease: false);
                GamePad.RaisePreviewAxisDown(recipient, device, axis, value, repeat, gamePadAxisPressedData);
                GamePad.RaiseAxisDown(recipient, device, axis, value, repeat, gamePadAxisPressedData);
                gamePadAxisPressedData.Release();
            }

            if (originalFocus != elementWithFocus)
                wasFocusMostRecentlyChangedByKeyboardOrGamePad = true;
        }

        /// <summary>
        /// Handles the <see cref="GamePadDevice.AxisReleased"/> event.
        /// </summary>
        private void gamePad_AxisReleased(GamePadDevice device, GamePadAxis axis, Single value)
        {
            if (device.PlayerIndex != 0 || !IsInputEnabledAndAllowed)
                return;

            var recipient = elementWithFocus as DependencyObject;
            if (recipient != null)
            {
                var originalFocus = elementWithFocus;

                var gamePadAxisPressedData = RoutedEventData.Retrieve(recipient, autorelease: false);
                GamePad.RaisePreviewAxisUp(recipient, device, axis, gamePadAxisPressedData);
                GamePad.RaiseAxisUp(recipient, device, axis, gamePadAxisPressedData);
                gamePadAxisPressedData.Release();

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = true;
            }

            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Handles the <see cref="GamePadDevice.ButtonPressed"/> event.
        /// </summary>
        private void gamePad_ButtonPressed(GamePadDevice device, GamePadButton button, Boolean repeat)
        {
            if (device.PlayerIndex != 0 || !IsInputEnabledAndAllowed)
                return;

            var originalFocus = elementWithFocus;
            var suppressGamePadNav = false;

            var recipient = elementWithFocus as DependencyObject;
            if (recipient != null)
            {
                var gamePadButtonDownData = RoutedEventData.Retrieve(recipient, autorelease: false);
                GamePad.RaisePreviewButtonDown(recipient, device, button, repeat, gamePadButtonDownData);
                GamePad.RaiseButtonDown(recipient, device, button, repeat, gamePadButtonDownData);

                suppressGamePadNav = gamePadButtonDownData.Handled;

                gamePadButtonDownData.Release();
            }

            if (!suppressGamePadNav)
            {
                if (!FocusNavigator.PerformNavigation(this, device, button))
                {
                    var buttons =
                        (GamePad.ConfirmButton == button) ? defaultButtons :
                        (GamePad.CancelButton == button) ? cancelButtons : null;

                    if (buttons != null)
                        ActivateDefaultOrCancelButton(buttons);
                }
            }

            if (originalFocus != elementWithFocus)
                wasFocusMostRecentlyChangedByKeyboardOrGamePad = true;
        }

        /// <summary>
        /// Handles the <see cref="GamePadDevice.ButtonReleased"/> event.
        /// </summary>
        private void gamePad_ButtonReleased(GamePadDevice device, GamePadButton button)
        {
            if (device.PlayerIndex != 0 || !IsInputEnabledAndAllowed)
                return;

            var recipient = elementWithFocus as DependencyObject;
            if (recipient != null)
            {
                var originalFocus = elementWithFocus;

                var gamePadAxisChangedData = RoutedEventData.Retrieve(recipient, autorelease: false);
                GamePad.RaisePreviewButtonUp(recipient, device, button, gamePadAxisChangedData);
                GamePad.RaiseButtonUp(recipient, device, button, gamePadAxisChangedData);
                gamePadAxisChangedData.Release();

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = true;
            }

            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Handles the <see cref="TouchDevice.TouchMotion"/> event.
        /// </summary>
        private void touch_TouchMotion(TouchDevice device, Int64 touchID, Int64 fingerID, Single x, Single y, Single dx, Single dy, Single pressure)
        {
            if (device.BoundWindow != Window || !IsInputEnabledAndAllowed)
                return;

            var tracker = touchCursorTrackers?.GetTrackerByTouchID(touchID);
            if (tracker == null)
                return;

            var recipient = tracker.ElementUnderCursor;
            if (recipient != null)
            {
                var originalFocus = elementWithFocus;

                var position = device.DenormalizeCoordinates(x, y);
                var positionDips = Display.PixelsToDips(position);

                var delta = device.DenormalizeCoordinates(dx, dy);
                var deltaDips = Display.PixelsToDips(delta);

                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var touchMoveData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Touch.RaisePreviewTouchMove(dobj, device, touchID, positionDips.X, positionDips.Y, deltaDips.X, deltaDips.Y, pressure, touchMoveData);
                    Touch.RaiseTouchMove(dobj, device, touchID, positionDips.X, positionDips.Y, deltaDips.X, deltaDips.Y, pressure, touchMoveData);
                    touchMoveData.Release();
                }

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = false;
            }
        }

        /// <summary>
        /// Handles the <see cref="TouchDevice.TouchDown"/> event.
        /// </summary>
        private void touch_TouchDown(TouchDevice device, Int64 touchID, Int64 fingerID, Single x, Single y, Single pressure)
        {
            if (device.BoundWindow != Window || !IsInputEnabledAndAllowed)
                return;

            if (!touchCursorTrackers?.StartTracking(touchID, elementWithNewTouchCapture, newTouchCaptureMode) ?? false)
                return;

            var tracker = touchCursorTrackers.GetTrackerByTouchID(touchID);
            
            var recipient = tracker.ElementUnderCursor;
            if (recipient != null)
            {
                var originalFocus = elementWithFocus;

                var position = device.DenormalizeCoordinates(x, y);
                var positionDips = Display.PixelsToDips(position);

                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var touchDownData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Touch.RaisePreviewTouchDown(dobj, device, touchID, positionDips.X, positionDips.Y, pressure, touchDownData);
                    Touch.RaiseTouchDown(dobj, device, touchID, positionDips.X, positionDips.Y, pressure, touchDownData);
                    touchDownData.Release();
                }

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = false;
            }
        }

        /// <summary>
        /// Handles the <see cref="TouchDevice.TouchUp"/> event.
        /// </summary>
        private void touch_TouchUp(TouchDevice device, Int64 touchID, Int64 fingerID)
        {
            if (device.BoundWindow != Window || !IsInputEnabledAndAllowed)
                return;

            var tracker = touchCursorTrackers?.GetTrackerByTouchID(touchID);
            if (tracker == null)
                return;

            var recipient = tracker.ElementUnderCursor;
            if (recipient != null)
            {
                var originalFocus = elementWithFocus;

                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var touchUpData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Touch.RaisePreviewTouchUp(dobj, device, touchID, touchUpData);
                    Touch.RaiseTouchUp(dobj, device, touchID, touchUpData);
                    touchUpData.Release();
                }

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = false;
            }

            touchCursorTrackers.FinishTracking(touchID);            
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Handles the <see cref="TouchDevice.Tap"/> event.
        /// </summary>
        private void touch_Tap(TouchDevice device, Int64 touchID, Int64 fingerID, Single x, Single y)
        {
            if (device.BoundWindow != Window || !IsInputEnabledAndAllowed)
                return;

            var position = device.DenormalizeCoordinates(x, y);
            var positionDips = Display.PixelsToDips(position);

            var recipient = HitTest(positionDips);
            if (recipient != null)
            {
                var originalFocus = elementWithFocus;

                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var touchTapData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Touch.RaisePreviewTouchTap(dobj, device, touchID, positionDips.X, positionDips.Y, touchTapData);
                    Touch.RaiseTouchTap(dobj, device, touchID, positionDips.X, positionDips.Y, touchTapData);
                    touchTapData.Release();
                }

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = false;
            }
        }

        /// <summary>
        /// Handles the <see cref="TouchDevice.LongPress"/> event.
        /// </summary>
        private void touch_LongPress(TouchDevice device, Int64 touchID, Int64 fingerID, Single x, Single y, Single pressure)
        {
            if (device.BoundWindow != Window || !IsInputEnabledAndAllowed)
                return;

            var position = device.DenormalizeCoordinates(x, y);
            var positionDips = Display.PixelsToDips(position);

            var tracker = touchCursorTrackers?.GetTrackerByTouchID(touchID);
            if (tracker == null)
                return;

            var recipient = tracker.ElementUnderCursor;
            if (recipient != null)
            {
                var originalFocus = elementWithFocus;

                var dobj = recipient as DependencyObject;
                if (dobj != null)
                {
                    var touchLongPressData = RoutedEventData.Retrieve(dobj, autorelease: false);
                    Touch.RaisePreviewTouchLongPress(dobj, device, touchID, positionDips.X, positionDips.Y, pressure, touchLongPressData);
                    Touch.RaiseTouchLongPress(dobj, device, touchID, positionDips.X, positionDips.Y, pressure, touchLongPressData);
                    touchLongPressData.Release();
                }

                if (originalFocus != elementWithFocus)
                    wasFocusMostRecentlyChangedByKeyboardOrGamePad = false;
            }
        }

        /// <summary>
        /// Handles the <see cref="TouchDevice.MultiGesture"/> event.
        /// </summary>
        private void touch_MultiGesture(TouchDevice device, Single x, Single y, Single theta, Single distance, Int32 fingers)
        {
            if (device.BoundWindow != Window || !IsInputEnabledAndAllowed)
                return;

            // This isn't associated with any particular touch, so it always goes to whichever
            // element is directly under the centroid.
            var centroidPixs = device.DenormalizeCoordinates(x, y);
            var centroidDips = Display.PixelsToDips(centroidPixs);

            var recipient = HitTest(centroidDips);
            if (recipient != null)
            {
                var multiGestureData = RoutedEventData.Retrieve(recipient, autorelease: false);
                Touch.RaisePreviewMultiGesture(recipient, device, centroidDips.X, centroidDips.Y, theta, distance, fingers, multiGestureData);
                Touch.RaiseMultiGesture(recipient, device, centroidDips.X, centroidDips.Y, theta, distance, fingers, multiGestureData);
                multiGestureData.Release();
            }
        }

        /// <summary>
        /// Handles the <see cref="TouchDevice.DollarGesture"/> event.
        /// </summary>
        private void touch_DollarGesture(TouchDevice device, Int64 gestureID, Single x, Single y, Single error, Int32 fingers)
        {
            if (device.BoundWindow != Window || !IsInputEnabledAndAllowed)
                return;

            // This isn't associated with any particular touch, so it always goes to whichever
            // element is directly under the centroid.
            var centroidPixs = device.DenormalizeCoordinates(x, y);
            var centroidDips = Display.PixelsToDips(centroidPixs);

            var recipient = HitTest(centroidDips);
            if (recipient != null)
            {
                var dollarGestureData = RoutedEventData.Retrieve(recipient, autorelease: false);
                Touch.RaisePreviewDollarGesture(recipient, device, gestureID, centroidDips.X, centroidDips.Y, error, fingers, dollarGestureData);
                Touch.RaiseDollarGesture(recipient, device, gestureID, centroidDips.X, centroidDips.Y, error, fingers, dollarGestureData);
                dollarGestureData.Release();
            }
        }

        /// <summary>
        /// Handles the <see cref="IUltravioletInput.TouchDeviceRegistered"/> event.
        /// </summary>
        private void Input_TouchDeviceRegistered(TouchDevice device)
        {
            Ultraviolet.GetInput().TouchDeviceRegistered -= Input_TouchDeviceRegistered;
            HookTouchEvents();
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
        private readonly PopupQueue popupQueue = new PopupQueue();
        private readonly DrawingContext drawingContext;
        private CursorTracker mouseCursorTracker;
        private CursorTrackerTouchCollection touchCursorTrackers;
        private IInputElement elementWithFocus;
        private IInputElement elementWithNewTouchCapture;
        private CaptureMode newTouchCaptureMode;
        private Boolean viewIsOpen;
        private Boolean hookedGlobalStyleSheetChanged;
        private Boolean hookedFirstPlayerGamePad;
        private Boolean wasInputPossibleLastFrame;
        private Boolean wasFocusMostRecentlyChangedByKeyboardOrGamePad;

        // Tooltip handling.
        private readonly DropShadowEffect toolTipDropShadow = new DropShadowEffect();
        private UIElement toolTipElementDisplayed;
        private UIElement toolTipElementPrev;
        private UIElement toolTipElement;
        private Double timeUntilToolTipWillOpen;
        private Double timeSinceToolTipWasOpened;
        private Double timeSinceToolTipWasClosed;
        private Boolean toolTipWasShownForCurrentElement;

        // Default/cancel buttons for the view.
        private readonly List<WeakReference> defaultButtons = new List<WeakReference>(0);
        private readonly List<WeakReference> cancelButtons = new List<WeakReference>(0);
    }
}
 