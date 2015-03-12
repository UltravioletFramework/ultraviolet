using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
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
        /// <param name="id">The unique identifier of this element within its layout.</param>
        public Control(UltravioletContext uv, String id)
            : base(uv, id)
        {
            LoadComponentRoot();
        }

        /// <summary>
        /// Populates any fields of this object which represent references
        /// to components in the current component tree.
        /// </summary>
        internal void PopulateFieldsFromRegisteredElements()
        {
            componentRegistry.PopulateFieldsFromRegisteredElements(this);
        }

        /// <summary>
        /// Gets the control's component registry, which is used to associate
        /// component elements with their unique identifiers within the context of this control.
        /// </summary>
        internal UIElementRegistry ComponentRegistry
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

        /// <inheritdoc/>
        protected override void InitializeDependencyPropertiesCore(Boolean recursive)
        {
            if (componentRoot != null)
            {
                componentRoot.InitializeDependencyProperties(true);
            }
            base.InitializeDependencyPropertiesCore(recursive);
        }

        /// <inheritdoc/>
        protected override void ReloadContentCore(Boolean recursive)
        {
            if (componentRoot != null)
            {
                componentRoot.ReloadContent(true);
            }
            base.ReloadContentCore(recursive);
        }

        /// <inheritdoc/>
        protected override void ClearAnimationsCore(Boolean recursive)
        {
            if (componentRoot != null)
            {
                componentRoot.ClearAnimations(true);
            }
            base.ClearAnimationsCore(recursive);
        }

        /// <inheritdoc/>
        protected override void ClearLocalValuesCore(Boolean recursive)
        {
            if (componentRoot != null)
            {
                componentRoot.ClearLocalValues(true);
            }
            base.ClearLocalValuesCore(recursive);
        }

        /// <inheritdoc/>
        protected override void ClearStyledValuesCore(Boolean recursive)
        {
            if (componentRoot != null)
            {
                componentRoot.ClearStyledValues(true);
            }
            base.ClearStyledValuesCore(recursive);
        }

        /// <inheritdoc/>
        protected override void CleanupCore()
        {
            if (componentRoot != null)
            {
                componentRoot.Cleanup();
            }
            base.CleanupCore();
        }

        /// <inheritdoc/>
        protected override void CacheLayoutParametersCore()
        {
            if (componentRoot != null)
            {
                componentRoot.CacheLayoutParameters();
            }
            base.CacheLayoutParametersCore();
        }
        
        /// <inheritdoc/>
        protected override void AnimateCore(Storyboard storyboard, StoryboardClock clock, UIElement root)
        {
            if (componentRoot != null)
            {
                componentRoot.Animate(storyboard, clock, root);
            }
            base.AnimateCore(storyboard, clock, root);
        }

        /// <inheritdoc/>
        protected override void StyleOverride(UvssDocument stylesheet)
        {
            if (componentRoot != null)
            {
                componentRoot.Style(stylesheet);
            }
            base.StyleOverride(stylesheet);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (componentRoot == null)
                return Size2D.Zero;

            componentRoot.Measure(availableSize);
            return componentRoot.DesiredSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            if (componentRoot == null)
                return finalSize;

            var finalRect = new RectangleD(Point2D.Zero, finalSize);
            componentRoot.Arrange(finalRect, options);
            return componentRoot.RenderSize;
        }

        /// <inheritdoc/>
        protected override UIElement GetElementAtPointCore(Double x, Double y, Boolean isHitTest)
        {
            if (componentRoot != null)
            {
                var componentRelX = x - componentRoot.RelativeBounds.X;
                var componentRelY = y - componentRoot.RelativeBounds.Y;

                var componentMatch = componentRoot.GetElementAtPoint(componentRelX, componentRelY, isHitTest);
                if (componentMatch != null)
                {
                    return componentMatch;
                }
            }
            return base.GetElementAtPointCore(x, y, isHitTest);
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
        private readonly UIElementRegistry componentRegistry = new UIElementRegistry();
    }
}
