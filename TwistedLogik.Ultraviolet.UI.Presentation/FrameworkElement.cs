using System;
using System.ComponentModel;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the base class for standard Ultraviolet Presentation Foundation elements.
    /// </summary>
    [UvmlKnownType("element")]
    public abstract class FrameworkElement : UIElement, ISupportInitialize
    {
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
        public void BeginInit()
        {
            Contract.EnsureNot(isInitializing, PresentationStrings.BeginInitAlreadyCalled);

            isInitializing = true;
        }

        /// <inheritdoc/>
        public void EndInit()
        {
            Contract.Ensure(isInitializing, PresentationStrings.BeginInitNotCalled);

            isInitializing = false;

            if (!isInitialized)
            {
                isInitialized = true;
                OnInitialized();
            }
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
            set { SetValue<Double>(WidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's minimum width in device independent pixels.
        /// </summary>
        public Double MinWidth
        {
            get { return GetValue<Double>(MinWidthProperty); }
            set { SetValue<Double>(MinWidthProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets the element's maximum width in device independent pixels.
        /// </summary>
        public Double MaxWidth
        {
            get { return GetValue<Double>(MaxWidthProperty); }
            set { SetValue<Double>(MaxWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's height in device-independent pixels.
        /// </summary>
        public Double Height
        {
            get { return GetValue<Double>(HeightProperty); }
            set { SetValue<Double>(HeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's minimum height in device independent pixels.
        /// </summary>
        public Double MinHeight
        {
            get { return GetValue<Double>(MinHeightProperty); }
            set { SetValue<Double>(MinHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's maximum height in device independent pixels.
        /// </summary>
        public Double MaxHeight
        {
            get { return GetValue<Double>(MaxHeightProperty); }
            set { SetValue<Double>(MaxHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's outer margin.
        /// </summary>
        public Thickness Margin
        {
            get { return GetValue<Thickness>(MarginProperty); }
            set { SetValue<Thickness>(MarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the amount of padding between the edges of the element and its content.
        /// </summary>
        public Thickness Padding
        {
            get { return GetValue<Thickness>(PaddingProperty); }
            set { SetValue<Thickness>(PaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's horizontal alignment.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment
        {
            get { return GetValue<HorizontalAlignment>(HorizontalAlignmentProperty); }
            set { SetValue<HorizontalAlignment>(HorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's vertical alignment.
        /// </summary>
        public VerticalAlignment VerticalAlignment
        {
            get { return GetValue<VerticalAlignment>(VerticalAlignmentProperty); }
            set { SetValue<VerticalAlignment>(VerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the element has been fully initialized.
        /// </summary>
        public Boolean IsInitialized
        {
            get { return isInitialized; }
        }

        /// <summary>
        /// Gets a value indicating whether the element has been loaded and laid out.
        /// </summary>
        public Boolean IsLoaded
        {
            get { return isLoaded; }
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
            typeof(HorizontalAlignment), typeof(FrameworkElement), new PropertyMetadata<HorizontalAlignment>(PresentationBoxedValues.HorizontalAlignment.Left, PropertyMetadataOptions.AffectsArrange));
        
        /// <summary>
        /// Identifies the <see cref="VerticalAlignment"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'valign'.</remarks>
        public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.Register("VerticalAlignment", "valign",
            typeof(VerticalAlignment), typeof(FrameworkElement), new PropertyMetadata<VerticalAlignment>(PresentationBoxedValues.VerticalAlignment.Top, PropertyMetadataOptions.AffectsArrange));

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

            var evt     = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(routedEvent);
            var evtData = new RoutedEventData(this);
            evt(this, ref evtData);

            VisualTreeHelper.ForEachChild<FrameworkElement>(this, CommonBoxedValues.Boolean.FromValue(loaded), (child, state) =>
            {
                child.RaiseLoadedOrUnloaded((Boolean)state);
            });
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

        /// <inheritdoc/>
        internal override void OnVisualParentChangedInternal()
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

            base.OnVisualParentChangedInternal();
        }

        /// <inheritdoc/>
        internal override void ApplyStyledVisualStateTransition(UvssStyle style)
        {
            Contract.Require(style, "style");

            if (View != null && View.StyleSheet != null)
            {
                var value = (style.CachedResolvedValue != null && style.CachedResolvedValue is String) ?
                    (String)style.CachedResolvedValue : style.Value.Trim();

                style.CachedResolvedValue = value;

                var storyboard = View.StyleSheet.InstantiateStoryboardByName(Ultraviolet, value);
                if (storyboard != null)
                {
                    var group = default(String);
                    var from  = default(String);
                    var to    = default(String);

                    switch (style.Arguments.Count)
                    {
                        case 2:
                            group = style.Arguments[0];
                            from  = null;
                            to    = style.Arguments[1];
                            break;

                        case 3:
                            group = style.Arguments[0];
                            from  = style.Arguments[1];
                            to    = style.Arguments[2];
                            break;

                        default:
                            throw new NotSupportedException();
                    }

                    VisualStateGroups.SetVisualStateTransition(group, from, to, storyboard);
                }
            }
        }

        /// <summary>
        /// Registers the element with the specified namescope.
        /// </summary>
        /// <param name="namescope">The namescope with which to register the element.</param>
        internal void RegisterElementWithNamescope(Namescope namescope)
        {
            if (String.IsNullOrEmpty(Name))
                return;

            namescope.RegisterElement(this);
        }

        /// <summary>
        /// Unregisters the element from the specified namescope.
        /// </summary>
        /// <param name="namescope">The namescope from which to unregister the element.</param>
        internal void UnregisterElementFromNamescope(Namescope namescope)
        {
            if (String.IsNullOrEmpty(Name))
                return;

            namescope.UnregisterElement(this);
        }

        /// <inheritdoc/>
        internal override Object DependencyDataSource
        {
            get { return TemplatedParent ?? ViewModel; }
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
        protected override void StyleCore(UvssDocument styleSheet)
        {
            base.StyleCore(styleSheet);

            VisualStateGroups.ReapplyStates();
        }

        /// <inheritdoc/>
        protected sealed override Size2D MeasureCore(Size2D availableSize)
        {
            var margin = this.Margin;

            var xMargin = margin.Left + margin.Right;
            var yMargin = margin.Top + margin.Bottom;

            double minWidth, maxWidth;
            LayoutUtil.GetBoundedMeasure(Width, MinWidth, MaxWidth, out minWidth, out maxWidth);

            double minHeight, maxHeight;
            LayoutUtil.GetBoundedMeasure(Height, MinHeight, MaxHeight, out minHeight, out maxHeight);

            var availableWidthSansMargin  = Math.Max(0, availableSize.Width - xMargin);
            var availableHeightSansMargin = Math.Max(0, availableSize.Height - yMargin);

            var tentativeWidth  = Math.Max(minWidth, Math.Min(maxWidth, availableWidthSansMargin));
            var tentativeHeight = Math.Max(minHeight, Math.Min(maxHeight, availableHeightSansMargin));
            var tentativeSize   = new Size2D(tentativeWidth, tentativeHeight);

            var measuredSize   = MeasureOverride(tentativeSize);
            var measuredWidth  = measuredSize.Width;
            var measuredHeight = measuredSize.Height;
            
            measuredWidth  = xMargin + Math.Max(minWidth, Math.Min(maxWidth, measuredWidth));
            measuredHeight = yMargin + Math.Max(minHeight, Math.Min(maxHeight, measuredHeight));

            var finalWidth  = Math.Max(0, measuredWidth);
            var finalHeight = Math.Max(0, measuredHeight);

            return new Size2D(finalWidth, finalHeight);
        }

        /// <inheritdoc/>
        protected sealed override Size2D ArrangeCore(RectangleD finalRect, ArrangeOptions options)
        {
            var margin = Margin;

            var finalRectSansMargins = finalRect - margin;

            var desiredWidth = DesiredSize.Width;
            var desiredHeight = DesiredSize.Height;

            var fill   = (options & ArrangeOptions.Fill) == ArrangeOptions.Fill;
            var hAlign = fill ? HorizontalAlignment.Stretch : HorizontalAlignment;
            var vAlign = fill ? VerticalAlignment.Stretch : VerticalAlignment;

            if (Double.IsNaN(Width) && hAlign == HorizontalAlignment.Stretch)
                desiredWidth = finalRect.Width;

            if (Double.IsNaN(Height) && vAlign == VerticalAlignment.Stretch)
                desiredHeight = finalRect.Height;

            var desiredSize   = new Size2D(desiredWidth, desiredHeight);

            var candidateSize = desiredSize - margin;
            var usedSize      = ArrangeOverride(candidateSize, options);

            var usedWidth  = Math.Min(usedSize.Width, candidateSize.Width);
            var usedHeight = Math.Min(usedSize.Height, candidateSize.Height);

            usedSize = new Size2D(usedWidth, usedHeight);

            var xOffset = margin.Left + LayoutUtil.PerformHorizontalAlignment(finalRectSansMargins.Size, usedSize, fill ? HorizontalAlignment.Left : hAlign);
            var yOffset = margin.Top + LayoutUtil.PerformVerticalAlignment(finalRectSansMargins.Size, usedSize, fill ? VerticalAlignment.Top : vAlign);

            RenderOffset = new Point2D(xOffset, yOffset);

            return usedSize;
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
        protected override void CacheLayoutParametersCore()
        {
            var templatedParentControl = TemplatedParent as Control;
            if (templatedParentControl != null)
            {
                templatedParentControl.ComponentTemplateNamescope.UnregisterElement(this);
                templatedParentControl.ComponentTemplateNamescope.RegisterElement(this);
            }

            base.CacheLayoutParametersCore();
        }

        /// <inheritdoc/>
        protected override void OnGotKeyboardFocus(ref RoutedEventData data)
        {
            if (data.OriginalSource == this)
            {
                VisualStateGroups.GoToState("focus", "focused");
            }
            base.OnGotKeyboardFocus(ref data);
        }

        /// <inheritdoc/>
        protected override void OnLostKeyboardFocus(ref RoutedEventData data)
        {
            if (data.OriginalSource == this)
            {
                VisualStateGroups.GoToState("focus", "blurred");
            }
            base.OnLostKeyboardFocus(ref data);
        }

        /// <summary>
        /// Raises the <see cref="Initialized"/> event.
        /// </summary>
        protected virtual void OnInitialized()
        {
            var temp = Initialized;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// When overridden in a derived class, draws the element using the 
        /// specified <see cref="SpriteBatch"/> for a <see cref="FrameworkElement"/> derived class.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        protected virtual void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            var children = VisualTreeHelper.GetChildrenCount(this);
            for (int i = 0; i < children; i++)
            {
                var child = VisualTreeHelper.GetChild(this, i) as UIElement;
                if (child != null)
                {
                    child.Draw(time, dc);
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, updates the element's state for
        /// a <see cref="FrameworkElement"/> derived class.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        protected virtual void UpdateOverride(UltravioletTime time)
        {
            VisualTreeHelper.ForEachChild<UIElement>(this, time, (child, state) =>
            {
                child.Update((UltravioletTime)state);
            });
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
        /// Gets the element's collection of visual state groups.
        /// </summary>
        protected VisualStateGroupCollection VisualStateGroups
        {
            get { return visualStateGroups; }
        }

        // Standard visual state groups.
        private static readonly String[] VSGFocus = new[] { "blurred", "focused" };

        // Property values.
        private DependencyObject templatedParent;
        private readonly String name;
        private readonly VisualStateGroupCollection visualStateGroups;
        private Boolean isInitialized;
        private Boolean isLoaded;

        // State values.
        private Boolean isInitializing;
    }
}
