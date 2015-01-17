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

        /// <inheritdoc/>
        internal sealed override Point2D GetComponentRegionOffset()
        {
            return new Point2D(0, 0);
        }

        /// <inheritdoc/>
        internal sealed override Point2D GetContentRegionOffset()
        {
            if (contentPresenter == null)
            {
                return new Point2D(0, 0);
            }
            var x = contentPresenter.AbsoluteBounds.X - AbsoluteBounds.X;
            var y = contentPresenter.AbsoluteBounds.Y - AbsoluteBounds.Y;
            return new Point2D(x, y);
        }

        /// <inheritdoc/>
        internal sealed override Size2D GetComponentRegionSize(Size2D finalSize)
        {
            return finalSize;
        }

        /// <inheritdoc/>
        internal sealed override Size2D GetContentRegionSize(Size2D finalSize)
        {
            if (contentPresenter == null)
            {
                return finalSize;
            }
            return contentPresenter.RenderSize;
        }

        /// <inheritdoc/>
        internal sealed override RectangleD GetComponentRegion(Size2D finalSize)
        {
            return new RectangleD(0, 0, finalSize.Width, finalSize.Height);
        }

        /// <inheritdoc/>
        internal sealed override RectangleD GetContentRegion(Size2D finalSize)
        {
            if (contentPresenter == null)
            {
                return new RectangleD(0, 0, finalSize.Width, finalSize.Height);
            }
            var x = contentPresenter.AbsoluteBounds.X - AbsoluteBounds.X;
            var y = contentPresenter.AbsoluteBounds.Y - AbsoluteBounds.Y;
            return new RectangleD(x, y, contentPresenter.RenderSize.Width, contentPresenter.RenderSize.Height);
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
        internal ContentPresenter ContentPresenter
        {
            get { return contentPresenter; }
            set
            {
                if (value != null && value.Control != this)
                    throw new ArgumentException(UltravioletStrings.ContentPresenterIsNotAComponent);

                contentPresenter = value;

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
        /// Loads the control's component root from the specified template.
        /// </summary>
        /// <param name="template">The component template from which to load the control's component root.</param>
        protected void LoadComponentRoot(XDocument template)
        {
            if (componentRoot != null)
                throw new InvalidOperationException(UltravioletStrings.ComponentRootAlreadyLoaded);

            if (template == null)
                return;

            UvmlLoader.LoadComponentRoot(this, template);
        }

        /// <summary>
        /// Draws the control's components, if it has any.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which to render the element.</param>
        /// <param name="opacity">The cumulative opacity of all of the element's ancestor elements.</param>
        protected void DrawComponents(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            if (componentRoot != null)
            {
                componentRoot.Draw(time, spriteBatch, opacity);
            }
        }

        /// <summary>
        /// Updates the control's components, if it has any.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        protected void UpdateComponents(UltravioletTime time)
        {
            if (componentRoot != null)
            {
                componentRoot.Update(time);
            }
        }

        /// <summary>
        /// Measures the control's components, if it has any.
        /// </summary>
        /// <param name="componentSize">The amount of space provided for the control's components.</param>
        protected Size2D MeasureComponents(Size2D availableSize, Size2D componentSize)
        {
            var clampedComponentWidth  = Math.Min(availableSize.Width, componentSize.Width);
            var clampedComponentHeight = Math.Min(availableSize.Height, componentSize.Height);
            var clampedComponentSize   = new Size2D(clampedComponentWidth, clampedComponentHeight);

            if (componentRoot != null)
            {
                if (contentPresenter != null)
                    contentPresenter.ContentSize = clampedComponentSize;

                componentRoot.Measure(new Size2D(Double.PositiveInfinity, Double.PositiveInfinity));

                var desiredComponentWidth  = Math.Min(componentRoot.DesiredSize.Width, availableSize.Width);
                var desiredComponentHeight = Math.Min(componentRoot.DesiredSize.Height, availableSize.Height);
                var desiredComponentSize   = new Size2D(desiredComponentWidth, desiredComponentHeight);

                return desiredComponentSize;
            }

            return clampedComponentSize;
        }

        /// <summary>
        /// Arranges the control's components, if it has any.
        /// </summary>
        /// <param name="componentSize">The amount of space provided for the control's components.</param>
        protected void ArrangeComponents(Size2D componentSize)
        {
            if (componentRoot != null)
            {
                componentRoot.Arrange(new RectangleD(0, 0, componentSize.Width, componentSize.Height), ArrangeOptions.Fill);
            }
        }

        /// <summary>
        /// Positions the control's components, if it has any.
        /// </summary>
        /// <param name="position">The position of the element's parent element in absolute screen space.</param>
        protected void PositionComponents(Point2D position)
        {
            if (componentRoot != null)
            {
                componentRoot.Position(AbsolutePosition);
            }
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
       
        // Property values.
        private UIElement componentRoot;
        private ContentPresenter contentPresenter;

        // The registry of components belonging to this control.
        private readonly UIElementRegistry componentRegistry = new UIElementRegistry();
    }
}
