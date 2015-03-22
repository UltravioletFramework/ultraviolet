using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a framework element which consists of multiple component elements.
    /// </summary>
    public abstract class Control : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Control(UltravioletContext uv, String name)
            : base(uv, name)
        {
            LoadComponentRoot();
        }

        /// <summary>
        /// Gets or sets a value indicating whether auto-nav is enabled for this control.
        /// If enabled, auto-nav will attempt to automatically determine the next nav control in the
        /// up, down, left, and right directions, even if the values of the <see cref="NavUp"/>, <see cref="NavDown"/>,
        /// <see cref="NavLeft"/>, and <see cref="NavRight"/> properties have not been set.
        /// </summary>
        public Boolean AutoNav
        {
            get { return GetValue<Boolean>(AutoNavProperty); }
            set { SetValue<Boolean>(AutoNavProperty, value); }
        }

        /// <summary>
        /// Gets or sets the identifier of the element which is navigated to when focus is
        /// moved "up," usually by pressing up on the directional pad of a game controller.
        /// </summary>
        public String NavUp
        {
            get { return GetValue<String>(NavUpProperty); }
            set { SetValue<String>(NavUpProperty, value); }
        }

        /// <summary>
        /// Gets or sets the identifier of the element which is navigated to when focus is
        /// moved "down," usually by pressing down on the directional pad of a game controller.
        /// </summary>
        public String NavDown
        {
            get { return GetValue<String>(NavDownProperty); }
            set { SetValue<String>(NavDownProperty, value); }
        }

        /// <summary>
        /// Gets or sets the identifier of the element which is navigated to when focus is
        /// moved "left," usually by pressing down on the directional pad of a game controller.
        /// </summary>
        public String NavLeft
        {
            get { return GetValue<String>(NavLeftProperty); }
            set { SetValue<String>(NavLeftProperty, value); }
        }

        /// <summary>
        /// Gets or sets the identifier of the element which is navigated to when focus is
        /// moved "right," usually by pressing down on the directional pad of a game controller.
        /// </summary>
        public String NavRight
        {
            get { return GetValue<String>(NavRightProperty); }
            set { SetValue<String>(NavRightProperty, value); }
        }

        /// <summary>
        /// Gets a value which indicates the element's relative position within the tab order
        /// of its parent element.
        /// </summary>
        public Int32 TabIndex
        {
            get { return GetValue<Int32>(TabIndexProperty); }
            set { SetValue<Int32>(TabIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this element participates in tab navigation.
        /// </summary>
        public Boolean IsTabStop
        {
            get { return GetValue<Boolean>(IsTabStopProperty); }
            set { SetValue<Boolean>(IsTabStopProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AutoNav"/> property changes.
        /// </summary>
        public event UpfEventHandler AutoNavChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="NavUp"/> property changes.
        /// </summary>
        public event UpfEventHandler NavUpChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="NavDown"/> property changes.
        /// </summary>
        public event UpfEventHandler NavDownChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="NavLeft"/> property changes.
        /// </summary>
        public event UpfEventHandler NavLeftChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="NavRight"/> property changes.
        /// </summary>
        public event UpfEventHandler NavRightChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="TabIndex"/> property changes.
        /// </summary>
        public event UpfEventHandler TabIndexChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="IsTabStop"/> property changes.
        /// </summary>
        public event UpfEventHandler IsTabStopChanged;

        /// <summary>
        /// Identifies the <see cref="AutoNav"/> dependency property.
        /// </summary>
        [Styled("autonav")]
        public static readonly DependencyProperty AutoNavProperty = DependencyProperty.Register("AutoNav", typeof(Boolean), typeof(Control),
            new PropertyMetadata(CommonBoxedValues.Boolean.True, HandleAutoNavChanged));

        /// <summary>
        /// Identifies the <see cref="NavUp"/> dependency property.
        /// </summary>
        [Styled("nav-up")]
        public static readonly DependencyProperty NavUpProperty = DependencyProperty.Register("NavUp", typeof(String), typeof(Control),
            new PropertyMetadata(HandleNavUpChanged));

        /// <summary>
        /// Identifies the <see cref="NavDown"/> dependency property.
        /// </summary>
        [Styled("nav-down")]
        public static readonly DependencyProperty NavDownProperty = DependencyProperty.Register("NavDown", typeof(String), typeof(Control),
            new PropertyMetadata(HandleNavDownChanged));

        /// <summary>
        /// Identifies the <see cref="NavLeft"/> dependency property.
        /// </summary>
        [Styled("nav-left")]
        public static readonly DependencyProperty NavLeftProperty = DependencyProperty.Register("NavLeft", typeof(String), typeof(Control),
            new PropertyMetadata(HandleNavLeftChanged));

        /// <summary>
        /// Identifies the <see cref="NavRight"/> dependency property.
        /// </summary>
        [Styled("nav-right")]
        public static readonly DependencyProperty NavRightProperty = DependencyProperty.Register("NavRight", typeof(String), typeof(Control),
            new PropertyMetadata(HandleNavRightChanged));

        /// <summary>
        /// Identifies the <see cref="TabIndex"/> dependency property.
        /// </summary>
        [Styled("tab-index")]
        public static readonly DependencyProperty TabIndexProperty = KeyboardNavigation.TabIndexProperty.AddOwner(typeof(Control));

        /// <summary>
        /// Identifies the <see cref="IsTabStop"/> dependency property.
        /// </summary>
        [Styled("tab-stop")]
        public static readonly DependencyProperty IsTabStopProperty = KeyboardNavigation.IsTabStopProperty.AddOwner(typeof(Control));

        /// <summary>
        /// Populates any fields of this object which represent references
        /// to components in the current component tree.
        /// </summary>
        internal void PopulateFieldsFromRegisteredElements()
        {
            componentRegistry.PopulateFieldsFromRegisteredElements(this);
        }

        /// <summary>
        /// Gets the namescope for the control's component definition.
        /// </summary>
        internal Namescope ComponentNamescope
        {
            get { return componentRegistry; }
        }

        /// <summary>
        /// Gets or sets the root of the control's component tree.
        /// </summary>
        internal UIElement ComponentRoot
        {
            get { return componentRoot; }
            set
            {
                if (componentRoot == value)
                    return;

                if (componentRoot != null)
                    componentRoot.ChangeLogicalAndVisualParents(null, null);

                componentRoot = value;

                if (componentRoot != null)
                    componentRoot.ChangeLogicalAndVisualParents(this, this);

                InvalidateMeasure();
            }
        }

        /// <inheritdoc/>
        protected internal override void RemoveLogicalChild(UIElement child)
        {
            if (child == ComponentRoot)
            {
                ComponentRoot = null;
            }
            base.RemoveLogicalChild(child);
        }

        /// <inheritdoc/>
        protected internal override UIElement GetLogicalChild(Int32 childIndex)
        {
            if (ComponentRoot == null || childIndex != 0)
                throw new ArgumentOutOfRangeException("childIndex");

            return ComponentRoot;
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(Int32 childIndex)
        {
            if (ComponentRoot == null || childIndex != 0)
                throw new ArgumentOutOfRangeException("childIndex");

            return ComponentRoot;
        }

        /// <inheritdoc/>
        protected internal override Int32 LogicalChildrenCount
        {
            get
            {
                return ComponentRoot == null ? 0 : 1;
            }
        }

        /// <inheritdoc/>
        protected internal override Int32 VisualChildrenCount
        {
            get
            {
                return ComponentRoot == null ? 0 : 1;
            }
        }

        /// <summary>
        /// Raises the <see cref="AutoNavChanged"/> event.
        /// </summary>
        protected virtual void OnAutoNavChanged()
        {
            var temp = AutoNavChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="NavUpChanged"/> event.
        /// </summary>
        protected virtual void OnNavUpChanged()
        {
            var temp = NavUpChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="NavDownChanged"/> event.
        /// </summary>
        protected virtual void OnNavDownChanged()
        {
            var temp = NavDownChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="NavLeftChanged"/> event.
        /// </summary>
        protected virtual void OnNavLeftChanged()
        {
            var temp = NavLeftChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="NavRightChanged"/> event.
        /// </summary>
        protected virtual void OnNavRightChanged()
        {
            var temp = NavRightChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="TabIndexChanged"/> event.
        /// </summary>
        protected virtual void OnTabIndexChanged()
        {
            var temp = TabIndexChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="IsTabStopChanged"/> event.
        /// </summary>
        protected virtual void OnIsTabStopChanged()
        {
            var temp = IsTabStopChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (componentRoot == null)
            {
                var desiredWidth = Double.IsPositiveInfinity(availableSize.Width) ? 0 : availableSize.Width;
                var desiredHeight = Double.IsPositiveInfinity(availableSize.Height) ? 0 : availableSize.Height;
                return new Size2D(desiredWidth, desiredHeight);
            }
            componentRoot.Measure(availableSize);
            return componentRoot.DesiredSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            if (componentRoot == null)
            {
                var finalWidth = Double.IsPositiveInfinity(finalSize.Width) ? 0 : finalSize.Width;
                var finalHeight = Double.IsPositiveInfinity(finalSize.Height) ? 0 : finalSize.Height;
                return new Size2D(finalWidth, finalHeight);
            }
            var finalRect = new RectangleD(Point2D.Zero, finalSize);
            componentRoot.Arrange(finalRect, options);
            return componentRoot.RenderSize;
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="element">The element used to display the specified item.</param>
        /// <param name="item">The item being displayed by the specified element.</param>
        protected virtual void PrepareContainerForItemOverride(DependencyObject element, Object item)
        {
            var container = element as IItemContainer;
            if (container != null)
            {
                container.PrepareItemContainer(item);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AutoNav"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleAutoNavChanged(DependencyObject dobj)
        {
            var control = (Control)dobj;
            control.OnAutoNavChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="NavUp"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleNavUpChanged(DependencyObject dobj)
        {
            var control = (Control)dobj;
            control.OnNavUpChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="NavDown"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleNavDownChanged(DependencyObject dobj)
        {
            var control = (Control)dobj;
            control.OnNavDownChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="NavLeft"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleNavLeftChanged(DependencyObject dobj)
        {
            var control = (Control)dobj;
            control.OnNavLeftChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="NavRight"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleNavRightChanged(DependencyObject dobj)
        {
            var control = (Control)dobj;
            control.OnNavRightChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TabIndex"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleTabIndexChanged(DependencyObject dobj)
        {
            var control = (Control)dobj;
            control.OnTabIndexChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsTabStop"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleIsTabStopChanged(DependencyObject dobj)
        {
            var control = (Control)dobj;
            control.OnIsTabStopChanged();
        }

        /// <summary>
        /// Loads the control's component root from the control's associated template.
        /// </summary>
        private void LoadComponentRoot()
        {
            if (componentRoot != null)
                throw new InvalidOperationException(PresentationStrings.ComponentRootAlreadyLoaded);

            var template = Ultraviolet.GetUI().GetPresentationFoundation().ComponentTemplates.Get(this);
            if (template == null)
                return;

            UvmlLoader.LoadComponentRoot(this, template);
        }

        // Property values.
        private UIElement componentRoot;

        // The registry of components belonging to this control.
        private readonly Namescope componentRegistry = new Namescope();
    }
}
