using System;
using System.Reflection;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
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
        /// Gets the desired content region for this control as of the last call to <see cref="UIElement.Measure(Size2D)"/>.
        /// </summary>
        public RectangleD DesiredContentRegion
        {
            get { return desiredContentRegion; }
        }

        /// <summary>
        /// Gets the final rendered content region for this control as of the last call to <see cref="UIElement.Arrange(RectangleD, ArrangeOptions)"/>.
        /// </summary>
        public RectangleD RenderContentRegion
        {
            get { return renderContentRegion; }
        }

        /// <summary>
        /// Gets the final rendered content region for this control in absolute screen coordinates as 
        /// of the last call to <see cref="UIElement.Position(Point2D)"/>.
        /// </summary>
        public RectangleD AbsoluteContentRegion
        {
            get { return absoluteContentRegion; }
        }

        /// <summary>
        /// Gets or sets the amount of padding between the edges of the control
        /// and its content region.
        /// </summary>
        public Thickness Padding
        {
            get { return GetValue<Thickness>(PaddingProperty); }
            set { SetValue<Thickness>(PaddingProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Padding"/> property changes.
        /// </summary>
        public event UIElementEventHandler PaddingChanged;

        /// <summary>
        /// Identifies the <see cref="Padding"/> dependency property.
        /// </summary>
        [Styled("padding")]
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(ContentControl),
            new DependencyPropertyMetadata(HandlePaddingChanged, () => Thickness.Zero, DependencyPropertyOptions.AffectsMeasure));

        /// <inheritdoc/>
        internal override void PreMeasureOverride(Size2D availableSize)
        {
            this.desiredContentRegion = GetContentRegion(availableSize);
        }

        /// <inheritdoc/>
        internal override void PreArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            this.renderContentRegion = GetContentRegion(finalSize);
        }

        /// <inheritdoc/>
        internal override void PrePositionOverride(Point2D position)
        {
            this.absoluteContentRegion = new RectangleD(
                AbsoluteBounds.X + renderContentRegion.X,
                AbsoluteBounds.Y + renderContentRegion.Y,
                renderContentRegion.Width, renderContentRegion.Height);
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
            var padding = Padding;
            if (contentPresenter == null)
            {
                return new Point2D(padding.Left, padding.Top);
            }              
            var x = padding.Left + contentPresenter.AbsoluteBounds.X - AbsoluteBounds.X;
            var y = padding.Top + contentPresenter.AbsoluteBounds.Y - AbsoluteBounds.Y;            
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
                return finalSize - Padding;
            }
            return contentPresenter.RenderSize - Padding;
        }

        /// <inheritdoc/>
        internal sealed override RectangleD GetComponentRegion(Size2D finalSize)
        {
            return new RectangleD(0, 0, finalSize.Width, finalSize.Height);
        }

        /// <inheritdoc/>
        internal sealed override RectangleD GetContentRegion(Size2D finalSize)
        {
            var padding = Padding;
            if (contentPresenter == null)
            {
                finalSize -= padding;
                return new RectangleD(padding.Left, padding.Top, finalSize.Width, finalSize.Height);
            }
            
            var x      = contentPresenter.AbsoluteBounds.X - AbsoluteBounds.X;
            var y      = contentPresenter.AbsoluteBounds.Y - AbsoluteBounds.Y;
            var width  = contentPresenter.RenderSize.Width;
            var height = contentPresenter.RenderSize.Height;
            var region = new RectangleD(x, y, width, height);

            return Padding + region;
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

        /// <summary>
        /// Raises the <see cref="PaddingChanged"/> event.
        /// </summary>
        protected virtual void OnPaddingChanged()
        {
            var temp = PaddingChanged;
            if (temp != null)
            {
                temp(this);
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
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        protected void DrawComponents(UltravioletTime time, DrawingContext dc)
        {
            if (componentRoot != null)
            {
                componentRoot.Draw(time, dc);
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
        /// <param name="availableSize">The size of the area which the element's parent has 
        /// specified is available for the element's layout.</param>
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

        /// <summary>
        /// Occurs when the value of the <see cref="Padding"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandlePaddingChanged(DependencyObject dobj)
        {
            var control = (ContentControl)dobj;
            control.OnPaddingChanged();
        }

        // Property values.
        private RectangleD desiredContentRegion;
        private RectangleD renderContentRegion;
        private RectangleD absoluteContentRegion;
        private UIElement componentRoot;
        private ContentPresenter contentPresenter;

        // The registry of components belonging to this control.
        private readonly UIElementRegistry componentRegistry = new UIElementRegistry();
    }
}
