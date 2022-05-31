using System;
using System.ComponentModel;
using Ultraviolet.Core;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Animations;
using Ultraviolet.Presentation.Controls;
using Ultraviolet.Presentation.Input;
using Ultraviolet.Presentation.Media;
using Ultraviolet.Presentation.Styles;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the method that is called when an element requests that it be brought into view.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="targetRectangle">The target rectangle to bring into view.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfRequestBringIntoViewEventHandler(DependencyObject element, RectangleD targetRectangle, RoutedEventData data);

    /// <summary>
    /// Represents the base class for standard Ultraviolet Presentation Foundation elements.
    /// </summary>
    [UvmlKnownType]
    public abstract class FrameworkElement : UIElement, ISupportInitialize
    {
        /// <summary>
        /// Initializes the <see cref="FrameworkElement"/> type.
        /// </summary>
        static FrameworkElement()
        {
            EventManager.RegisterClassHandler(typeof(FrameworkElement), Mouse.QueryCursorEvent, new UpfQueryCursorEventHandler(HandleQueryCursor), true);

            EventManager.RegisterClassHandler(typeof(FrameworkElement), Keyboard.PreviewGotKeyboardFocusEvent, new UpfKeyboardFocusChangedEventHandler(HandlePreviewGotKeyboardFocus));
            EventManager.RegisterClassHandler(typeof(FrameworkElement), Keyboard.GotKeyboardFocusEvent, new UpfKeyboardFocusChangedEventHandler(HandleGotKeyboardFocus));

            EventManager.RegisterClassHandler(typeof(FrameworkElement), ToolTipService.ToolTipOpeningEvent, new UpfToolTipEventHandler(OnToolTipOpeningProxy));
            EventManager.RegisterClassHandler(typeof(FrameworkElement), ToolTipService.ToolTipClosingEvent, new UpfToolTipEventHandler(OnToolTipClosingProxy));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkElement"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The identifying name of this element within its layout.</param>
        public FrameworkElement(UltravioletContext uv, String name)
            : base(uv)
        {
            this.name = name;

            this.visualStateGroups = new VisualStateGroupCollection(this);
            this.visualStateGroups.Create("focus", VSGFocus);
        }

        /// <inheritdoc/>
        public override sealed DependencyObject PredictFocus(FocusNavigationDirection direction)
        {
            return FocusNavigator.PredictNavigation(View, this, direction, false) as DependencyObject;
        }

        /// <inheritdoc/>
        public override Boolean MoveFocus(FocusNavigationDirection direction)
        {
            return FocusNavigator.PerformNavigation(View, this, direction, false);
        }

        /// <inheritdoc/>
        public void BeginInit()
        {
            Contract.Ensure(initializationState == FrameworkElementInitializationState.Uninitialized, PresentationStrings.BeginInitAlreadyCalled);

            initializationState = FrameworkElementInitializationState.Initializing;
        }

        /// <inheritdoc/>
        public void EndInit()
        {
            Contract.Ensure(initializationState == FrameworkElementInitializationState.Initializing, PresentationStrings.BeginInitNotCalled);

            initializationState = FrameworkElementInitializationState.InitializingRaisingEvents;

            RaisePendingChangeEvents();

            initializationState = FrameworkElementInitializationState.Initialized;

            OnInitialized();
        }

        /// <summary>
        /// Builds the current template's visual tree if necessary, and returns a value that indicates whether
        /// the visual tree was rebuilt by this call.
        /// </summary>
        /// <remarks>The Ultraviolet Presentation Foundation does not currently implement the same templating
        /// system used by the Windows Presentation Foundation, so this method doesn't do much beyond signaling
        /// to certain primitives when to perform binding. You probably don't need to call this.</remarks>
        /// <returns><see langword="true"/> if visuals were added to the tree; otherwise, <see langword="false"/>.</returns>
        public Boolean ApplyTemplate()
        {
            OnPreApplyTemplate();

            // NOTE: UPF does not implement the WPF templating system at this time, so
            // this method doesn't actually do anything beyond signaling to primitives
            // when to perform binding.

            OnPostApplyTemplate();

            return false;
        }

        /// <summary>
        /// Finds an element that has the provided identifier name.
        /// </summary>
        /// <param name="name">The name of the requested element.</param>
        /// <returns>The requested element. This can be <see langword="null"/> if no matching element was found.</returns>
        public Object FindName(String name)
        {
            Contract.Require(name, nameof(name));

            var namescope = FindCurrentNamescope();
            if (namescope != null)
            {
                return namescope.FindName(name);
            }
            return null;
        }

        /// <summary>
        /// Registers the specified object within this element's namescope.
        /// </summary>
        /// <param name="name">The identifying name under which to register the object.</param>
        /// <param name="scopedElement">The object to register within this element's namescope.</param>
        public void RegisterName(String name, Object scopedElement)
        {
            var namescope = FindCurrentNamescope();
            if (namescope == null)
                throw new InvalidOperationException(PresentationStrings.ElementDoesNotHaveNamescope);

            namescope.RegisterName(name, scopedElement);
        }

        /// <summary>
        /// Unregisters a name from this element's namescope.
        /// </summary>
        /// <param name="name">The name to unregister from this element's namescope.</param>
        public void UnregisterName(String name)
        {
            var namescope = FindCurrentNamescope();
            if (namescope == null)
                throw new InvalidOperationException(PresentationStrings.ElementDoesNotHaveNamescope);

            namescope.UnregisterName(name);
        }

        /// <summary>
        /// Attempts to bring this element into view.
        /// </summary>
        public void BringIntoView()
        {
            BringIntoView(RectangleD.Empty);
        }

        /// <summary>
        /// Attempts to bring the specified rectangle within this element into view.
        /// </summary>
        /// <param name="targetRectangle">The rectangle to bring into view.</param>
        public void BringIntoView(RectangleD targetRectangle)
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRequestBringIntoViewEventHandler>(RequestBringIntoViewEvent);
            evtDelegate(this, targetRectangle, evtData);
        }

        /// <summary>
        /// Gets the template parent of this element. The template parent is the control which caused
        /// this element to be created, if applicable.
        /// </summary>
        public DependencyObject TemplatedParent
        {
            get { return templatedParent; }
            internal set { templatedParent = value; }
        }

        /// <summary>
        /// Gets the element's identifying name.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets or sets the element's width in device-independent pixels.
        /// </summary>
        public Double Width
        {
            get { return GetValue<Double>(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's minimum width in device independent pixels.
        /// </summary>
        public Double MinWidth
        {
            get { return GetValue<Double>(MinWidthProperty); }
            set { SetValue(MinWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's maximum width in device independent pixels.
        /// </summary>
        public Double MaxWidth
        {
            get { return GetValue<Double>(MaxWidthProperty); }
            set { SetValue(MaxWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's height in device-independent pixels.
        /// </summary>
        public Double Height
        {
            get { return GetValue<Double>(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's minimum height in device independent pixels.
        /// </summary>
        public Double MinHeight
        {
            get { return GetValue<Double>(MinHeightProperty); }
            set { SetValue(MinHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's maximum height in device independent pixels.
        /// </summary>
        public Double MaxHeight
        {
            get { return GetValue<Double>(MaxHeightProperty); }
            set { SetValue(MaxHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's outer margin.
        /// </summary>
        public Thickness Margin
        {
            get { return GetValue<Thickness>(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the amount of padding between the edges of the element and its content.
        /// </summary>
        public Thickness Padding
        {
            get { return GetValue<Thickness>(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's horizontal alignment.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment
        {
            get { return GetValue<HorizontalAlignment>(HorizontalAlignmentProperty); }
            set { SetValue(HorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's vertical alignment.
        /// </summary>
        public VerticalAlignment VerticalAlignment
        {
            get { return GetValue<VerticalAlignment>(VerticalAlignmentProperty); }
            set { SetValue(VerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the direction in which the element flows its content.
        /// </summary>
        public FlowDirection FlowDirection
        {
            get { return GetValue<FlowDirection>(FlowDirectionProperty); }
            set { SetValue(FlowDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the transformation which is applied to this element during layout.
        /// </summary>
        public Transform LayoutTransform
        {
            get { return GetValue<Transform>(LayoutTransformProperty); }
            set { SetValue(LayoutTransformProperty, value); }
        }

        /// <summary>
        /// Gets or sets the cursor which is set by this element.
        /// </summary>
        public SourcedCursor Cursor
        {
            get { return GetValue<SourcedCursor>(CursorProperty); }
            set { SetValue(CursorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this element forces its cursor to be displayed, overriding
        /// any cursor which has been set by its child elements.
        /// </summary>
        public Boolean ForceCursor
        {
            get { return GetValue<Boolean>(ForceCursorProperty); }
            set { SetValue(ForceCursorProperty, value); }
        }

        /// <summary>
        /// Gets or sets an object that contains information about the element.
        /// </summary>
        public Object Tag
        {
            get { return GetValue<Object>(TagProperty); }
            set { SetValue(TagProperty, value); }
        }
        
        /// <summary>
        /// Gets a value indicating whether the element has been fully initialized.
        /// </summary>
        public Boolean IsInitialized
        {
            get { return initializationState == FrameworkElementInitializationState.Initialized; }
        }

        /// <summary>
        /// Gets a value indicating whether the element has been loaded and laid out.
        /// </summary>
        public Boolean IsLoaded
        {
            get { return isLoaded; }
        }

        /// <inheritdoc/>
        public override Boolean IsDeferringChangeEvents
        {
            get { return initializationState == FrameworkElementInitializationState.Initializing; }
        }

        /// <summary>
        /// Occurs when the element is initialized.
        /// </summary>
        public event UpfEventHandler Initialized;

        /// <summary>
        /// Identifies the <see cref="Width"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'width'.</remarks>
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(Double), typeof(FrameworkElement),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.NaN, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="MinWidth"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'min-width'.</remarks>
        public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(Double), typeof(FrameworkElement),
            new PropertyMetadata<Double>(null, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="MaxWidth"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'max-width'.</remarks>
        public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(Double), typeof(FrameworkElement),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.PositiveInfinity, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="Height"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'height'.</remarks>
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(Double), typeof(FrameworkElement),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.NaN, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="MinHeight"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'min-height'.</remarks>
        public static readonly DependencyProperty MinHeightProperty = DependencyProperty.Register("MinHeight", typeof(Double), typeof(FrameworkElement),
            new PropertyMetadata<Double>(null, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="MaxHeight"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'max-height'.</remarks>
        public static readonly DependencyProperty MaxHeightProperty = DependencyProperty.Register("MaxHeight", typeof(Double), typeof(FrameworkElement),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.PositiveInfinity, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="Margin"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'margin'.</remarks>
        public static readonly DependencyProperty MarginProperty = DependencyProperty.Register("Margin", typeof(Thickness), typeof(FrameworkElement),
            new PropertyMetadata<Double>(null, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="Padding"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'padding'.</remarks>
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(FrameworkElement),
            new PropertyMetadata<Double>(null, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="HorizontalAlignment"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'halign'.</remarks>
        public static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.Register("HorizontalAlignment", "halign",
            typeof(HorizontalAlignment), typeof(FrameworkElement), new PropertyMetadata<HorizontalAlignment>(PresentationBoxedValues.HorizontalAlignment.Stretch, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="VerticalAlignment"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'valign'.</remarks>
        public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.Register("VerticalAlignment", "valign",
            typeof(VerticalAlignment), typeof(FrameworkElement), new PropertyMetadata<VerticalAlignment>(PresentationBoxedValues.VerticalAlignment.Stretch, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="FlowDirection"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'flow-direction'.</remarks>
        public static readonly DependencyProperty FlowDirectionProperty = DependencyProperty.RegisterAttached("FlowDirection", typeof(FlowDirection), typeof(FrameworkElement),
            new PropertyMetadata<FlowDirection>(FlowDirection.LeftToRight, PropertyMetadataOptions.AffectsArrange | PropertyMetadataOptions.Inherits));

        /// <summary>
        /// Identifies the <see cref="LayoutTransform"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'layout-transform'.</remarks>
        public static readonly DependencyProperty LayoutTransformProperty = DependencyProperty.Register("LayoutTransform",
            typeof(Transform), typeof(FrameworkElement), new PropertyMetadata<Transform>(Transform.Identity, PropertyMetadataOptions.AffectsMeasure, HandleLayoutTransformChanged));

        /// <summary>
        /// Identifies the <see cref="Cursor"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty CursorProperty = DependencyProperty.Register("Cursor",
            typeof(SourcedCursor), typeof(FrameworkElement), new PropertyMetadata<SourcedCursor>(null, PropertyMetadataOptions.None, HandleCursorChanged));

        /// <summary>
        /// Identifies the <see cref="ForceCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ForceCursorProperty = DependencyProperty.Register("ForceCursor",
            typeof(Boolean), typeof(FrameworkElement), new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleForceCursorChanged));

        /// <summary>
        /// Identifies the <see cref="Tag"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TagProperty = DependencyProperty.Register("Tag",
            typeof(Object), typeof(FrameworkElement), new PropertyMetadata<Object>(null, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the ToolTip dependency property.
        /// </summary>
        public static readonly DependencyProperty ToolTipProperty = ToolTipService.ToolTipProperty.AddOwner(typeof(FrameworkElement));

        /// <summary>
        /// Occurs when the element is loaded.
        /// </summary>
        public event UpfRoutedEventHandler Loaded
        {
            add { AddHandler(LoadedEvent, value); }
            remove { RemoveHandler(LoadedEvent, value); }
        }

        /// <summary>
        /// Occurs when the element is unloaded.
        /// </summary>
        public event UpfRoutedEventHandler Unloaded
        {
            add { AddHandler(UnloadedEvent, value); }
            remove { RemoveHandler(UnloadedEvent, value); }
        }

        /// <summary>
        /// Occurs when an element requests that it be brought into view.
        /// </summary>
        public event UpfRequestBringIntoViewEventHandler RequestBringIntoView
        {
            add { AddHandler(RequestBringIntoViewEvent, value); }
            remove { RemoveHandler(RequestBringIntoViewEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Loaded"/> routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is 'loaded'.</remarks>
        public static readonly RoutedEvent LoadedEvent = EventManager.RegisterRoutedEvent("Loaded", RoutingStrategy.Direct,
            typeof(UpfRoutedEventHandler), typeof(FrameworkElement));

        /// <summary>
        /// Identifies the <see cref="Unloaded"/> routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is 'unloaded'.</remarks>
        public static readonly RoutedEvent UnloadedEvent = EventManager.RegisterRoutedEvent("Unloaded", RoutingStrategy.Direct,
            typeof(UpfRoutedEventHandler), typeof(FrameworkElement));

        /// <summary>
        /// Identifies the <see cref="RequestBringIntoView"/> routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is 'request-bring-into-view'.</remarks>
        public static readonly RoutedEvent RequestBringIntoViewEvent = EventManager.RegisterRoutedEvent("RequestBringIntoView", RoutingStrategy.Bubble,
            typeof(UpfRequestBringIntoViewEventHandler), typeof(FrameworkElement));

        /// <summary>
        /// Sets the value of the <see cref="IsLoaded"/> property, propagating it downward through the
        /// element's visual descendants.
        /// </summary>
        /// <param name="value">The new value to set for the <see cref="IsLoaded"/> property.</param>
        private void SetIsLoaded(Boolean value)
        {
            if (IsLoaded == value)
                return;

            isLoaded = value;

            if (!value)
            {
                var upf = Ultraviolet.GetUI().GetPresentationFoundation();
                upf.RemoveFromQueues(this);
            }

            VisualTreeHelper.ForEachChild<FrameworkElement>(this, CommonBoxedValues.Boolean.FromValue(value), (child, state) =>
            {
                child.SetIsLoaded((Boolean)state);
            });
        }

        /// <summary>
        /// Raises the <see cref="Loaded"/> or <see cref="Unloaded"/> events on this element
        /// and all of its visual descendants.
        /// </summary>
        /// <param name="loaded">A value indicating whether to raise the <see cref="Loaded"/> or <see cref="Unloaded"/> event.</param>
        private void RaiseLoadedOrUnloaded(Boolean loaded)
        {
            var routedEvent = loaded ? LoadedEvent : UnloadedEvent;

            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(routedEvent);
            var evtData = RoutedEventData.Retrieve(this);
            evtDelegate(this, evtData);

            VisualTreeHelper.ForEachChild<FrameworkElement>(this, CommonBoxedValues.Boolean.FromValue(loaded), (child, state) =>
            {
                child.RaiseLoadedOrUnloaded((Boolean)state);
            });
        }

        /// <summary>
        /// Ensures that the element has been initialized.
        /// </summary>
        internal void EnsureIsInitialized()
        {
            if (initializationState != FrameworkElementInitializationState.Uninitialized)
                return;

            BeginInit();
            EndInit();
        }

        /// <summary>
        /// Ensures that the value of the element's <see cref="IsLoaded"/> property matches the specified value.
        /// </summary>
        /// <param name="value">A value indicating whether the element should be loaded or unloaded.</param>
        internal void EnsureIsLoaded(Boolean value)
        {
            if (IsLoaded == value)
                return;

            SetIsLoaded(value);
            RaiseLoadedOrUnloaded(value);
        }

        /// <summary>
        /// Called at the beginning of the <see cref="ApplyTemplate"/> method.
        /// </summary>
        internal virtual void OnPreApplyTemplate()
        {

        }

        /// <summary>
        /// Called at the end of the <see cref="ApplyTemplate"/> method.
        /// </summary>
        internal virtual void OnPostApplyTemplate()
        {

        }

        /// <inheritdoc/>
        internal override void OnVisualParentChangedInternal(Visual oldParent, Visual newParent)
        {
            var parent = VisualParent as FrameworkElement;
            if (parent != null)
            {
                if (parent.IsLoaded != IsLoaded)
                {
                    if (parent.IsLoaded)
                    {
                        EnsureIsLoaded(true);
                    }
                    else
                    {
                        EnsureIsLoaded(false);
                    }
                }
            }
            else
            {
                if (IsLoaded)
                {
                    EnsureIsLoaded(false);
                }
            }

            base.OnVisualParentChangedInternal(oldParent, newParent);
        }

        /// <inheritdoc/>
        internal override void ApplyStyledVisualStateTransition(UvssRule style)
        {
            Contract.Require(style, nameof(style));

            if (View != null && View.StyleSheet != null)
            {
                var value = (style.CachedResolvedValue != null && style.CachedResolvedValue is String) ?
                    (String)style.CachedResolvedValue : style.Value.Trim();

                style.CachedResolvedValue = value;

                var storyboard = View.StyleSheet.GetStoryboardInstance(value);
                if (storyboard != null)
                {
                    var group = default(String);
                    var from = default(String);
                    var to = default(String);

                    switch (style.Arguments.Count)
                    {
                        case 2:
                            group = style.Arguments[0];
                            from = null;
                            to = style.Arguments[1];
                            break;

                        case 3:
                            group = style.Arguments[0];
                            from = style.Arguments[1];
                            to = style.Arguments[2];
                            break;

                        default:
                            throw new NotSupportedException();
                    }

                    VisualStateGroups.SetVisualStateTransition(group, from, to, storyboard);
                }
            }
        }

        /// <inheritdoc/>
        internal override Object DependencyDataSource
        {
            get { return DeclarativeViewModelOrTemplate ?? TemplatedParent ?? ViewModel; }
        }

        /// <inheritdoc/>
        internal override Boolean HasLayoutTransform
        {
            get { return isLayoutTransformed; }
        }
        
        /// <summary>
        /// Gets or sets the namescope which this element provides as a result of being created
        /// by a framework template, if it provides such a namescope.
        /// </summary>
        internal Namescope TemplatedNamescope
        {
            get { return templatedNamescope; }
            set { templatedNamescope = value; }
        }

        /// <summary>
        /// Gets the specified logical child of this element.
        /// </summary>
        /// <param name="childIndex">The index of the logical child to retrieve.</param>
        /// <returns>The logical child of this element with the specified index.</returns>
        protected internal virtual UIElement GetLogicalChild(Int32 childIndex)
        {
            throw new ArgumentOutOfRangeException("childIndex");
        }

        /// <summary>
        /// Gets the number of logical children which belong to this element.
        /// </summary>
        protected internal virtual Int32 LogicalChildrenCount
        {
            get { return 0; }
        }

        /// <inheritdoc/>
        protected override Matrix GetTransformMatrix(Boolean inDevicePixels = false)
        {
            if (isLayoutTransformed)
            {
                return base.GetTransformMatrix() * layoutTransformUsedDuringLayout;
            }
            return base.GetTransformMatrix(inDevicePixels);
        }

        /// <inheritdoc/>
        protected sealed override void DrawCore(UltravioletTime time, DrawingContext dc)
        {
            if (!LayoutUtil.IsDrawn(this))
                return;

            DrawOverride(time, dc);
        }

        /// <inheritdoc/>
        protected sealed override void UpdateCore(UltravioletTime time)
        {
            UpdateOverride(time);
        }

        /// <inheritdoc/>
        protected sealed override void InitializeDependencyPropertiesCore(Boolean recursive)
        {
            base.InitializeDependencyPropertiesCore(recursive);
        }

        /// <inheritdoc/>
        protected sealed override void ReloadContentCore(Boolean recursive)
        {
            ReloadCursor();
            ReloadContentOverride(recursive);
        }

        /// <inheritdoc/>
        protected sealed override void ClearBindingsCore(Boolean recursive)
        {
            base.ClearBindingsCore(recursive);
        }

        /// <inheritdoc/>
        protected sealed override void ClearAnimationsCore(Boolean recursive)
        {
            base.ClearAnimationsCore(recursive);
        }

        /// <inheritdoc/>
        protected sealed override void ClearLocalValuesCore(Boolean recursive)
        {
            base.ClearLocalValuesCore(recursive);
        }

        /// <inheritdoc/>
        protected sealed override void ClearStyledValuesCore(Boolean recursive)
        {
            base.ClearStyledValuesCore(recursive);
        }

        /// <inheritdoc/>
        protected sealed override void ClearTriggeredValuesCore(Boolean recursive)
        {
            base.ClearTriggeredValuesCore(recursive);
        }

        /// <inheritdoc/>
        protected sealed override void PrepareCore()
        {
            PrepareOverride();

            base.PrepareCore();
        }

        /// <inheritdoc/>
        protected sealed override void CleanupCore()
        {
            CleanupOverride();

            base.CleanupCore();
        }

        /// <inheritdoc/>
        protected sealed override void CacheLayoutParametersCore()
        {
            namescope = FindCurrentNamescope();

            if (namescope != null)
            {
                var name = Name;
                if (!String.IsNullOrEmpty(name))
                    namescope.RegisterName(name, this);
            }

            CacheLayoutParametersOverride();

            base.CacheLayoutParametersCore();
        }

        /// <inheritdoc/>
        protected sealed override void AnimateCore(StoryboardInstance storyboardInstance)
        {
            base.AnimateCore(storyboardInstance);
        }

        /// <inheritdoc/>
        protected sealed override void StyleCore(UvssDocument styleSheet)
        {
            base.StyleCore(styleSheet);

            VisualStateGroups.ReapplyStates();

            StyleOverride(styleSheet);
        }

        /// <inheritdoc/>
        protected sealed override Size2D MeasureCore(Size2D availableSize)
        {
            ApplyTemplate();

            var margin = PerformLayoutRounding(this.Margin);

            var xMargin = margin.Left + margin.Right;
            var yMargin = margin.Top + margin.Bottom;

            double minWidth, maxWidth;
            LayoutUtil.GetBoundedMeasure(Width, MinWidth, MaxWidth, out minWidth, out maxWidth);

            double minHeight, maxHeight;
            LayoutUtil.GetBoundedMeasure(Height, MinHeight, MaxHeight, out minHeight, out maxHeight);

            var availableWidthSansMargin = Math.Max(0, availableSize.Width - xMargin);
            var availableHeightSansMargin = Math.Max(0, availableSize.Height - yMargin);
            var availableSizeSansMargin = new Size2D(availableWidthSansMargin, availableHeightSansMargin);

            isLayoutTransformed = !Transform.IsIdentityTransform(LayoutTransform);
            if (isLayoutTransformed)
            {
                layoutTransformUsedDuringLayout = LayoutTransform.Value;

                availableSizeSansMargin = CalculateMaximumAvailableSizeBeforeLayoutTransform(
                    availableWidthSansMargin, availableHeightSansMargin, layoutTransformUsedDuringLayout);
            }

            var tentativeWidth = Math.Max(minWidth, Math.Min(maxWidth, availableSizeSansMargin.Width));
            var tentativeHeight = Math.Max(minHeight, Math.Min(maxHeight, availableSizeSansMargin.Height));
            var tentativeSize = new Size2D(tentativeWidth, tentativeHeight);
            tentativeSize = PerformLayoutRounding(tentativeSize);

            var measuredSize = MeasureOverride(tentativeSize);
            measuredSize = PerformLayoutRounding(measuredSize);

            var measuredWidth = measuredSize.Width;
            var measuredHeight = measuredSize.Height;

            measuredWidth = Math.Max(minWidth, Math.Min(maxWidth, measuredWidth));
            measuredHeight = Math.Max(minHeight, Math.Min(maxHeight, measuredHeight));
            measuredSize = new Size2D(measuredWidth, measuredHeight);

            layoutTransformSizeDesiredBeforeTransform = measuredSize;

            if (isLayoutTransformed)
            {
                var transform = layoutTransformUsedDuringLayout;
                RectangleD area = new RectangleD(0, 0, measuredWidth, measuredHeight);
                RectangleD.TransformAxisAligned(ref area, ref transform, out area);
                measuredSize = new Size2D(area.Width, area.Height);
            }

            var finalWidth = Math.Max(0, xMargin + measuredSize.Width);
            var finalHeight = Math.Max(0, yMargin + measuredSize.Height);

            return PerformLayoutRounding(new Size2D(finalWidth, finalHeight));
        }

        /// <inheritdoc/>
        protected sealed override Size2D ArrangeCore(RectangleD finalRect, ArrangeOptions options)
        {
            var margin = PerformLayoutRounding(Margin);

            var finalRectSansMargins = finalRect - margin;

            var desiredWidth = DesiredSize.Width;
            var desiredHeight = DesiredSize.Height;

            var hAlign = HorizontalAlignment;
            var vAlign = VerticalAlignment;

            if (Double.IsNaN(Width) && hAlign == HorizontalAlignment.Stretch)
                desiredWidth = Math.Max(MinWidth, Math.Min(MaxWidth, finalRect.Width));

            if (Double.IsNaN(Height) && vAlign == VerticalAlignment.Stretch)
                desiredHeight = Math.Max(MinHeight, Math.Min(MaxHeight, finalRect.Height));

            if (isLayoutTransformed)
            {
                desiredWidth = layoutTransformSizeDesiredBeforeTransform.Width;
                desiredHeight = layoutTransformSizeDesiredBeforeTransform.Height;

                var arrangedSizeAfterLayoutTransform =
                    CalculateMaximumAvailableSizeBeforeLayoutTransform(desiredWidth, desiredHeight, layoutTransformUsedDuringLayout);

                if (MathUtil.IsApproximatelyGreaterThanOrEqual(arrangedSizeAfterLayoutTransform.Width, layoutTransformSizeDesiredBeforeTransform.Width) &&
                    MathUtil.IsApproximatelyGreaterThanOrEqual(arrangedSizeAfterLayoutTransform.Height, layoutTransformSizeDesiredBeforeTransform.Height))
                {
                    desiredWidth = arrangedSizeAfterLayoutTransform.Width;
                    desiredHeight = arrangedSizeAfterLayoutTransform.Height;
                }

                desiredWidth = desiredWidth + margin.Left + margin.Right;
                desiredHeight = desiredHeight + margin.Top + margin.Bottom;
            }

            var desiredSize = new Size2D(desiredWidth, desiredHeight);

            var candidateSize = desiredSize - margin;
            candidateSize = PerformLayoutRounding(candidateSize);

            var usedSize = ArrangeOverride(candidateSize, options);
            usedSize = PerformLayoutRounding(usedSize);

            var usedWidth = Math.Min(usedSize.Width, candidateSize.Width);
            var usedHeight = Math.Min(usedSize.Height, candidateSize.Height);

            usedSize = new Size2D(usedWidth, usedHeight);
            var untransformedUsedSize = usedSize;

            var xOffset = 0.0;
            var yOffset = 0.0;

            if (isLayoutTransformed)
            {
                RectangleD area = new RectangleD(0, 0, usedWidth, usedHeight);
                RectangleD.TransformAxisAligned(ref area, ref layoutTransformUsedDuringLayout, out area);
                usedSize = new Size2D(area.Width, area.Height);

                xOffset -= area.X;
                yOffset -= area.Y;
            }

            xOffset += margin.Left + LayoutUtil.PerformHorizontalAlignment(finalRectSansMargins.Size, usedSize, hAlign);
            yOffset += margin.Top + LayoutUtil.PerformVerticalAlignment(finalRectSansMargins.Size, usedSize, vAlign);

            RenderOffset = new Point2D(xOffset, yOffset);

            return untransformedUsedSize;
        }

        /// <inheritdoc/>
        protected sealed override void PositionCore()
        {
            PositionOverride();

            base.PositionCore();
        }

        /// <inheritdoc/>
        protected sealed override void PositionChildrenCore()
        {
            PositionChildrenOverride();
        }

        /// <inheritdoc/>
        protected sealed override RectangleD? ClipCore()
        {
            return ClipOverride();
        }

        /// <inheritdoc/>
        protected override void OnLogicalParentChanged()
        {
            base.OnLogicalParentChanged();

            EnsureIsInitialized();
        }

        /// <inheritdoc/>
        protected override void OnVisualParentChanged(Visual oldParent, Visual newParent)
        {
            base.OnVisualParentChanged(oldParent, newParent);

            EnsureIsInitialized();
        }

        /// <inheritdoc/>
        protected override void OnGotFocus(RoutedEventData data)
        {
            if (data.OriginalSource == this)
            {
                VisualStateGroups.GoToState("focus", "focused");
                BringIntoView();
            }
            base.OnGotFocus(data);
        }

        /// <inheritdoc/>
        protected override void OnLostFocus(RoutedEventData data)
        {
            if (data.OriginalSource == this)
            {
                VisualStateGroups.GoToState("focus", "blurred");
            }
            base.OnLostFocus(data);
        }

        /// <summary>
        /// Raises the <see cref="Initialized"/> event.
        /// </summary>
        protected virtual void OnInitialized() =>
            Initialized?.Invoke(this);

        /// <summary>
        /// When overridden in a derived class, draws the element using the 
        /// specified <see cref="Graphics.Graphics2D.SpriteBatch"/> for a <see cref="FrameworkElement"/> derived class.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        protected virtual void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            var children = VisualChildrenCount;
            for (int i = 0; i < children; i++)
            {
                GetVisualChildByZOrder(i).Draw(time, dc);
            }
        }

        /// <summary>
        /// When overridden in a derived class, updates the element's state for
        /// a <see cref="FrameworkElement"/> derived class.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        protected virtual void UpdateOverride(UltravioletTime time)
        {
            var children = VisualChildrenCount;
            for (int i = 0; i < children; i++)
            {
                GetVisualChild(i).Update(time);
            }
        }

        /// <summary>
        /// When overridden in a derived class, reloads this element's content 
        /// and, optionally, the content of any children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to reload content recursively.</param>
        protected virtual void ReloadContentOverride(Boolean recursive)
        {
            if (recursive)
            {
                VisualTreeHelper.ForEachChild<UIElement>(this, CommonBoxedValues.Boolean.FromValue(recursive), (child, state) =>
                {
                    child.ReloadContent((Boolean)state);
                });
            }
        }

        /// <summary>
        /// When overridden in a derived class, applies the specified style sheet
        /// to this element and to any child elements.
        /// </summary>
        /// <param name="styleSheet">The style sheet to apply to this element and its children.</param>
        protected virtual void StyleOverride(UvssDocument styleSheet)
        {

        }

        /// <summary>
        /// When overridden in a derived class, calculates the element's desired 
        /// size and the desired sizes of any child elements for a <see cref="FrameworkElement"/> derived class.
        /// </summary>
        /// <param name="availableSize">The size of the area which the element's parent has 
        /// specified is available for the element's layout.</param>
        /// <returns>The element's desired size, considering the size of any content elements.</returns>
        protected virtual Size2D MeasureOverride(Size2D availableSize)
        {
            return Size2D.Zero;
        }

        /// <summary>
        /// When overridden in a derived class, sets the element's final area relative to its 
        /// parent and arranges the element's children within its layout area for
        /// a <see cref="FrameworkElement"/> derived class.
        /// </summary>
        /// <param name="finalSize">The element's final size.</param>
        /// <param name="options">A set of <see cref="ArrangeOptions"/> values specifying the options for this arrangement.</param>
        /// <returns>The amount of space that was actually used by the element.</returns>
        protected virtual Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            return finalSize;
        }

        /// <summary>
        /// When overridden in a derived class, updates the position of the element in absolute 
        /// screen space for a <see cref="FrameworkElement"/> derived class.
        /// </summary>
        protected virtual void PositionOverride()
        {

        }

        /// <summary>
        /// When overridden in a derived class, updates the positions of the element's visual children
        /// in absolute screen space for a <see cref="FrameworkElement"/> derived class.
        /// </summary>
        protected virtual void PositionChildrenOverride()
        {
            VisualTreeHelper.ForEachChild<UIElement>(this, this, (child, state) =>
            {
                child.Position(Size2D.Zero);
                child.PositionChildren();
            });
        }
        
        /// <summary>
        /// When overridden in a derived class, prepares the element for use after a call to 
        /// the <see cref="UIElement.Cleanup()"/> method has been made.
        /// </summary>
        protected virtual void PrepareOverride()
        {

        }

        /// <summary>
        /// When overridden in a derived class, performs cleanup operations and releases any 
        /// internal framework resources for this element and any child elements.
        /// </summary>
        protected virtual void CleanupOverride()
        {

        }

        /// <summary>
        /// When overridden in a derived class, caches layout parameters related to the
        /// element's position within the element hierarchy for this element and for
        /// any child elements.
        /// </summary>
        protected virtual void CacheLayoutParametersOverride()
        {

        }

        /// <summary>
        /// When overridden in a derived class, calculates the clipping rectangle for this element.
        /// </summary>
        /// <returns>The clipping rectangle for this element in absolute screen coordinates, or <see langword="null"/> to disable clipping.</returns>
        protected virtual RectangleD? ClipOverride()
        {
            return null;
        }

        /// <summary>
        /// Occurs when the control's tool tip is opened.
        /// </summary>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnToolTipOpening(RoutedEventData data)
        {

        }

        /// <summary>
        /// Occurs when the control's tool tip is closed.
        /// </summary>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnToolTipClosing(RoutedEventData data)
        {

        }

        /// <summary>
        /// Reloads the element's cursor image.
        /// </summary>
        protected void ReloadCursor()
        {
            LoadResource(Cursor);
        }
        
        /// <summary>
        /// Gets the element's collection of visual state groups.
        /// </summary>
        protected VisualStateGroupCollection VisualStateGroups
        {
            get { return visualStateGroups; }
        }
        
        /// <summary>
        /// Gets the namescope which currently contains the element.
        /// </summary>
        protected Namescope Namescope
        {
            get { return namescope; }
        }
        
        /// <summary>
        /// Given a pair of dimensions, this method calculates the size of the largest rectangle that will fit within
        /// those dimensions after having the specified transformation applied to it.
        /// </summary>
        /// <param name="xmax">The available width.</param>
        /// <param name="ymax">The available height.</param>
        /// <param name="transform">The transformation matrix to apply.</param>
        /// <returns>The size of the largest rectangle that will still fit within the available space after the specified transform is applied.</returns>
        private static Size2D CalculateMaximumAvailableSizeBeforeLayoutTransform(Double xmax, Double ymax, Matrix transform)
        {
            /* When using layout transforms, it's possible for an element to produce a desired size which, after the transform
             * is applied, will cause the element to lie outside of its maximum available layout area. To address this problem,
             * we need to shrink the available size that is passed into MeasureCore() such that, even if the element is as big
             * as it possibly can be, its post-transform bounds will still lie within the available layout area.
             *
             * To that end, we need to do a bit of calculus. Given the true maximum area (A_true) and a transformation
             * matrix (M_transform), we need to calculate the largest possible rectangle that will fit within A_true
             * after it has been subjected to M_transform. This will be the area that we pass into MeasureCore().
             *
             * For simplicity's sake, consider the case of a rotation transform. Rotating a rectangle will cause its x-dimension
             * to point partially along both the x- and y-axes of the untransformed space. If we gradually make the rectangle wider,
             * it will eventually reach a point where its size along the untransformed x-axis will exceed our maximum width, and another
             * point where its size along the untransformed y-axis will exceed our maximum width. The smallest of these two widths
             * is the largest possible width of the transformed rectangle. We can do likewise to constrain the rectangle's height;
             * the biggest possible rectangle will have a width and height somewhere below the values established by these constraints. 
             *
             * We can use trigonometry to establish that there is a simple linear relationship between the width of the transformed 
             * rectangle and its dimensions along the untransformed x- and y-axes, such that w / sin(theta) = h / cos(theta). We can
             * use this to graph a pair of lines representing our transformed rectangle's width and height. We can then take the
             * first derivative in order to find the biggest rectangle that will fit under the lines.
             *
             * Let a = w / sin(theta) and b = h / cos(theta).
             *
             * The line formed by intercepts a and b forms a right triangle with the axes, so the total area beneath it is .5ab.
             * Given that we are trying to find the area of a rectangle beneath this line with dimensions x and y, we can equivalently
             * say that the triangle's total area is .5ay + .5bx. Solving for y, we find that y = (ab - bx) / a. We can then plug
             * this into the equation for the area of a rectangle, A = xy, to get A = x((ab - bx) / a). Taking the first derivative
             * and solving for x, we find that x = a / 2. Doing the same for y reveals, likewise, that y = b / 2. Therefore, the 
             * biggest rectangle has dimensions halfway between the intercepts that form the line. */

            if (Double.IsInfinity(xmax) && Double.IsInfinity(ymax))
                return new Size2D(Double.PositiveInfinity, Double.PositiveInfinity);

            xmax = Double.IsInfinity(xmax) ? ymax : xmax;
            ymax = Double.IsInfinity(ymax) ? xmax : ymax;

            if (MathUtil.IsApproximatelyZero(xmax) || MathUtil.IsApproximatelyZero(ymax) || MathUtil.IsApproximatelyZero(transform.Determinant()))
                return Size2D.Zero;
            
            var m11 = transform.M11;
            var m12 = transform.M12;
            var m21 = transform.M21;
            var m22 = transform.M22;
            
            var w = 0.0;
            var h = 0.0;

            var xConstraintInterceptW = MathUtil.IsApproximatelyZero(m11) ? Double.NaN : Math.Abs(xmax / m11);
            var xConstraintInterceptH = MathUtil.IsApproximatelyZero(m21) ? Double.NaN : Math.Abs(xmax / m21);
            var yConstraintInterceptW = MathUtil.IsApproximatelyZero(m12) ? Double.NaN : Math.Abs(ymax / m12);
            var yConstraintInterceptH = MathUtil.IsApproximatelyZero(m22) ? Double.NaN : Math.Abs(ymax / m22);

            var xConstraintIsHorz = Double.IsNaN(xConstraintInterceptW);
            var xConstraintIsVert = Double.IsNaN(xConstraintInterceptH);
            var xConstraintIsHorzOrVert = xConstraintIsHorz || xConstraintIsVert;
            
            var yConstraintIsHorz = Double.IsNaN(yConstraintInterceptW);
            var yConstraintIsVert = Double.IsNaN(yConstraintInterceptH);
            var yConstraintIsHorzOrVert = yConstraintIsHorz || yConstraintIsVert;

            /* Below, we handle special cases where one or both of the constraint lines is vertical or horizontal due to zeroes in
             * the transformation matrix. This causes some of our intercepts to go undefined, which means their constraint lines
             * don't constrain one (or either) of our dimensions. */

            if (xConstraintIsHorzOrVert && yConstraintIsHorzOrVert)
            {
                w = xConstraintIsVert ? xConstraintInterceptW : yConstraintInterceptW;
                h = xConstraintIsVert ? yConstraintInterceptH : xConstraintInterceptH;
                return new Size2D(w, h);
            }

            if (xConstraintIsVert || yConstraintIsVert)
            {
                var slope = xConstraintIsVert ? m12 / m22 : m11 / m21;
                w = xConstraintIsVert ? Math.Min(yConstraintInterceptW * 0.5, xConstraintInterceptW) : Math.Min(xConstraintInterceptW * 0.5, yConstraintInterceptW);
                h = (xConstraintIsVert ? yConstraintInterceptH : xConstraintInterceptH) - (slope * w);
                return new Size2D(w, h);
            }

            if (xConstraintIsHorz || yConstraintIsHorz)
            {
                var slope = xConstraintIsHorz ? m21 / m11 : m22 / m12;
                h = xConstraintIsHorz ? Math.Min(yConstraintInterceptH * 0.5, xConstraintInterceptH) : Math.Min(xConstraintInterceptH * 0.5, yConstraintInterceptH);
                w = (xConstraintIsHorz ? yConstraintInterceptW : xConstraintInterceptW) - (slope * h);
                return new Size2D(w, h);
            }
            
            /* If both constraint lines have a well-defined, non-zero slope, then the dimensions of the maximized rectangle lie halfway between the smaller line's
             * intercepts, as we established above using the first derivative.
             *
             * This is only true if the lines do not cross - otherwise, there is no clear "smaller line." So what we have to do is draw a third line
             * using the smallest intercept on both axes, then maximize beneath that instead. The result will actually be too small, since it doesn't correspond
             * to either of our original constraint lines; to address this problem, we scale the resulting size upwards until it saturates our constraints. */
             
            w = Math.Min(xConstraintInterceptW, yConstraintInterceptW) * 0.5;
            h = Math.Min(xConstraintInterceptH, yConstraintInterceptH) * 0.5;

            var constraintXSlope = xConstraintInterceptH / xConstraintInterceptW;
            var constraintYSlope = yConstraintInterceptH / xConstraintInterceptW;

            var constraintLinesCross = !MathUtil.AreApproximatelyEqual(constraintXSlope, constraintYSlope);
            if (constraintLinesCross)
            {
                RectangleD area = new RectangleD(0, 0, w, h);
                RectangleD.TransformAxisAligned(ref area, ref transform, out area);

                var scale = Math.Min(xmax / area.Width, ymax / area.Height);
                w *= scale;
                h *= scale;
            }

            return new Size2D(w, h);
        }

        /// <summary>
        /// Occurs when the <see cref="Mouse.QueryCursorEvent"/> attached event is raised on an instance of <see cref="FrameworkElement"/>.
        /// </summary>
        private static void HandleQueryCursor(DependencyObject dobj, MouseDevice device, CursorQueryRoutedEventData data)
        {
            var element = (FrameworkElement)dobj;

            var elementCursor = element.Cursor;
            if (elementCursor.IsLoaded && (element.ForceCursor || !data.Handled))
            {
                data.Cursor = elementCursor.Resource.Cursor;
                data.Handled = true;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="Keyboard.PreviewGotKeyboardFocusEvent"/> attached event is raised on an instance of <see cref="FrameworkElement"/>.
        /// </summary>
        private static void HandlePreviewGotKeyboardFocus(DependencyObject dobj, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            if (data.OriginalSource != dobj)
                return;

            var element = (FrameworkElement)dobj;

            var scopeFocusedElement = FocusManager.GetFocusedElement(element);
            if (scopeFocusedElement == null || scopeFocusedElement == element)
                return;

            if (!Keyboard.IsFocusable(scopeFocusedElement as UIElement))
                return;

            var viewFocusedElementPrevious = element.View.ElementWithFocus;
            scopeFocusedElement.Focus();
            var viewFocusedElementChanged = element.View.ElementWithFocus != viewFocusedElementPrevious;

            if (viewFocusedElementChanged || element.View.ElementWithFocus == scopeFocusedElement)
            {
                data.Handled = true;
                return;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="Keyboard.GotKeyboardFocusEvent"/> attached event is raised on an instance of <see cref="FrameworkElement"/>.
        /// </summary>
        private static void HandleGotKeyboardFocus(DependencyObject dobj, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            if (data.OriginalSource != dobj)
                return;

            FocusNavigator.UpdateLastFocusedElement(dobj);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="LayoutTransform"/> dependency property changes.
        /// </summary>
        private static void HandleLayoutTransformChanged(DependencyObject dobj, Transform oldValue, Transform newValue)
        {
            var element = (FrameworkElement)dobj;
            element.OnTransformChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Cursor"/> dependency property changes.
        /// </summary>
        private static void HandleCursorChanged(DependencyObject dobj, SourcedCursor oldValue, SourcedCursor newValue)
        {
            var element = (FrameworkElement)dobj;
            element.ReloadCursor();

            if (element.IsMouseOver && element.View != null)
                element.View.InvalidateCursor();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ForceCursor"/> dependency property changes.
        /// </summary>
        private static void HandleForceCursorChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var element = (FrameworkElement)dobj;

            if (element.IsMouseOver && element.View != null)
                element.View.InvalidateCursor();
        }

        /// <summary>
        /// Invokes the <see cref="OnToolTipOpening"/> method.
        /// </summary>
        private static void OnToolTipOpeningProxy(DependencyObject dobj, RoutedEventData data)
        {
            ((FrameworkElement)dobj).OnToolTipOpening(data);
        }

        /// <summary>
        /// Invokes the <see cref="OnToolTipClosing"/> method.
        /// </summary>
        private static void OnToolTipClosingProxy(DependencyObject dobj, RoutedEventData data)
        {
            ((FrameworkElement)dobj).OnToolTipClosing(data);
        }

        /// <summary>
        /// Finds the namescope which currently contains the element.
        /// </summary>
        private Namescope FindCurrentNamescope()
        {
            if (templatedNamescope != null)
                return templatedNamescope;

            var templatedParentControl = TemplatedParent as Control;
            if (templatedParentControl != null)
                return templatedParentControl.ComponentTemplateNamescope;

            var current = VisualTreeHelper.GetParent(this);
            while (current != null)
            {
                var currentElement = current as FrameworkElement;

                if (currentElement != null && currentElement.templatedNamescope != null)
                    return currentElement.templatedNamescope;

                current = VisualTreeHelper.GetParent(current);
            }

            return View?.Namescope;
        }

        // Standard visual state groups.
        private static readonly String[] VSGFocus = new[] { "blurred", "focused" };

        // Property values.
        private DependencyObject templatedParent;
        private readonly String name;
        private readonly VisualStateGroupCollection visualStateGroups;
        private Boolean isLoaded;

        // State values.
        private FrameworkElementInitializationState initializationState;
        private Matrix layoutTransformUsedDuringLayout;
        private Size2D layoutTransformSizeDesiredBeforeTransform;
        private Boolean isLayoutTransformed;
        private Namescope templatedNamescope;
        private Namescope namescope;
    }
}
