using System;
using System.Reflection;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents an interface element with non-client components.
    /// </summary>
    public abstract class Control : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public Control(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        public override void ClearLocalValuesRecursive()
        {
            base.ClearLocalValuesRecursive();

            if (componentRoot != null)
                componentRoot.ClearLocalValuesRecursive();
        }

        /// <inheritdoc/>
        public override void ClearStyledValuesRecursive()
        {
            base.ClearStyledValuesRecursive();

            if (componentRoot != null)
                componentRoot.ClearStyledValuesRecursive();
        }

        /// <inheritdoc/>
        public override void ClearVisualStateTransitionsRecursive()
        {
            base.ClearVisualStateTransitionsRecursive();

            if (componentRoot != null)
                componentRoot.ClearVisualStateTransitionsRecursive();
        }

        /// <inheritdoc/>
        public override void ReloadContentRecursive()
        {
            base.ReloadContentRecursive();

            if (componentRoot != null)
                componentRoot.ReloadContentRecursive();
        }

        /// <inheritdoc/>
        public override void PerformLayout()
        {
            OnPerformingLayout();

            if (componentRoot != null)
                PerformComponentLayout();

            PerformContentLayout();
            UpdateAbsoluteScreenPosition(AbsoluteScreenX, AbsoluteScreenY);

            OnPerformedLayout();
        }

        /// <summary>
        /// Populates any private fields of this control which match 
        /// the control's components.
        /// </summary>
        internal void PopulateFieldsFromRegisteredElements()
        {
            ComponentRegistry.PopulateFieldsFromRegisteredElements(this);
        }

        /// <inheritdoc/>
        internal override void UpdateAbsoluteScreenPosition(Int32 x, Int32 y, Boolean requestLayout = false)
        {
            base.UpdateAbsoluteScreenPosition(x, y, requestLayout);

            if (componentRoot != null)
                componentRoot.UpdateAbsoluteScreenPosition(x, y, requestLayout);
        }

        /// <inheritdoc/>
        internal override void ApplyStyles(UvssDocument stylesheet)
        {
            base.ApplyStyles(stylesheet);

            if (componentRoot != null)
                componentRoot.ApplyStyles(stylesheet);
        }

        /// <inheritdoc/>
        internal override void ApplyStoryboard(Storyboard storyboard, StoryboardClock clock, UIElement root)
        {
            base.ApplyStoryboard(storyboard, clock, root);

            if (componentRoot != null)
                componentRoot.ApplyStoryboard(storyboard, clock, root);
        }

        /// <inheritdoc/>
        internal override Boolean Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            if (View == null || Visibility != Visibility.Visible)
                return false;

            if (!base.Draw(time, spriteBatch))
                return false;

            if (componentRoot != null)
                componentRoot.Draw(time, spriteBatch);

            return true;
        }

        /// <inheritdoc/>
        internal override void Update(UltravioletTime time)
        {
            base.Update(time);

            if (componentRoot != null)
                componentRoot.Update(time);
        }

        /// <inheritdoc/>
        internal override void UpdateViewModel(Object viewModel)
        {
            base.UpdateViewModel(viewModel);

            if (componentRoot != null)
                componentRoot.UpdateViewModel(viewModel);
        }

        /// <inheritdoc/>
        internal override void UpdateView(UIView view)
        {
            base.UpdateView(view);

            if (componentRoot != null)
                componentRoot.UpdateView(view);
        }

        /// <inheritdoc/>
        internal override void UpdateContainer(UIElement container)
        {
            base.UpdateContainer(container);

            if (componentRoot != null)
                componentRoot.UpdateContainer(this);
        }

        /// <inheritdoc/>
        internal override UIElement GetElementAtPointInternal(Int32 x, Int32 y, Boolean hitTest)
        {
            if (!Bounds.Contains(x, y))
                return null;

            if (componentRoot != null)
            {
                var component = componentRoot.GetElementAtPointInternal(x, y, hitTest);
                if (component != null)
                    return component;
            }

            return base.GetElementAtPointInternal(x, y, hitTest);
        }

        /// <summary>
        /// Gets the control's component registry.
        /// </summary>
        internal UIElementRegistry ComponentRegistry
        {
            get { return componentRegistry; }
        }

        /// <summary>
        /// Gets the container which is the root of the control's component hierarchy.
        /// </summary>
        internal UIElement ComponentRoot
        {
            get { return componentRoot; }
            set
            {
                if (componentRoot != value)
                {
                    if (componentRoot != null)
                        componentRoot.UpdateContainer(null);

                    componentRoot = value;

                    if (componentRoot != null)
                        componentRoot.UpdateContainer(this);

                    PerformLayout();
                }
            }
        }

        /// <summary>
        /// Loads a component template from a manifest resource stream.
        /// </summary>
        /// <param name="asm">The assembly that contains the manifest resource stream.</param>
        /// <param name="resource">The name of the manifest resource stream to load.</param>
        /// <returns>The component template that was loaded.</returns>
        protected static XDocument LoadComponentTemplateFromManifestResourceStream(Assembly asm, String resource)
        {
            Contract.Require(asm, "asm");
            Contract.RequireNotEmpty(resource, "resource");

            using (var stream = asm.GetManifestResourceStream(resource))
            {
                if (stream == null)
                    return null;

                return XDocument.Load(stream);
            }
        }

        /// <summary>
        /// Gets the component within the control which has the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the component to retrieve.</param>
        /// <returns>The component with the specified identifier, or <c>null</c> if no such component exists.</returns>
        protected UIElement GetComponentByID(String id)
        {
            Contract.RequireNotEmpty(id, "id");

            return componentRegistry.GetElementByID(id);
        }

        /// <summary>
        /// Loads the control's component root from the specified template.
        /// </summary>
        /// <param name="template">The template from which to load the component root.</param>
        /// <param name="viewModelType">The type of view model to which the element will be bound.</param>
        /// <param name="bindingContext">The binding context to apply to the element which is instantiated.</param>
        protected void LoadComponentRoot(XDocument template, Type viewModelType, String bindingContext = null)
        {
            if (componentRoot != null)
                throw new InvalidOperationException(UltravioletStrings.ComponentRootAlreadyLoaded);

            UIViewLoader.LoadComponentRoot(this, template, viewModelType, bindingContext);
        }

        /// <summary>
        /// Immediately recalculates the layout of the container's components.
        /// </summary>
        private void PerformComponentLayout()
        {
            ComponentRoot.ParentRelativeArea = new Rectangle(0, 0, ActualWidth, ActualHeight);
            ComponentRoot.UpdateAbsoluteScreenPosition(AbsoluteScreenX, AbsoluteScreenY, true);
        }

        // Property values.
        private readonly UIElementRegistry componentRegistry = new UIElementRegistry();
        private UIElement componentRoot;
    }
}
