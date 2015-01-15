using System;
using System.Xml.Linq;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
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
        /// Gets or sets the root of the control's component tree.
        /// </summary>
        internal UIElement ComponentRoot
        {
            get { return componentRoot; }
            set
            {
                if (componentRoot == value)
                    return;

                if (componentRoot != null && componentRoot.Parent != null)
                    componentRoot.Parent.RemoveChild(componentRoot);

                componentRoot = value;

                if (componentRoot != null)
                    componentRoot.Parent = this;

                InvalidateMeasure();
            }
        }

        /// <summary>
        /// Gets or sets the element which is used to specify the position and bounds
        /// of the control's content view area.
        /// </summary>
        internal UIElement ComponentContentViewer
        {
            get { return componentContentViewer ?? this; }
            set
            {
                if (componentContentViewer != null && componentContentViewer.Parent != null)
                    componentContentViewer.Parent.RemoveChild(componentContentViewer);

                componentContentViewer = value;

                if (componentContentViewer != null)
                    componentContentViewer.Parent = this;

                InvalidateMeasure();
            }
        }

        /// <inheritdoc/>
        protected internal override void RemoveChild(UIElement child)
        {
            if (child == ComponentRoot)
            {
                ComponentRoot = null;
            }
            base.RemoveChild(child);
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            if (ComponentRoot != null)
            {
                ComponentRoot.Draw(time, spriteBatch, opacity);
            }
            base.DrawOverride(time, spriteBatch, opacity);
        }

        /// <inheritdoc/>
        protected override void UpdateOverride(UltravioletTime time)
        {
            if (ComponentRoot != null)
            {
                ComponentRoot.Update(time);
            }
            base.UpdateOverride(time);
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

        /// <summary>
        /// Loads the control's component root from the specified template.
        /// </summary>
        /// <param name="template">The component template from which to load the control's component root.</param>
        protected void LoadComponentRoot(XDocument template)
        {
            // TODO
        }

        // Property values.
        private UIElement componentRoot;
        private UIElement componentContentViewer;

        // The registry of components belonging to this control.
        private readonly UIElementRegistry componentRegistry = new UIElementRegistry();
    }
}
