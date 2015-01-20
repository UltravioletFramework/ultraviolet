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

        /// <inheritdoc/>
        public override RectangleD DesiredContentRegion
        {
            get { return desiredContentRegion; }
        }

        /// <inheritdoc/>
        public override RectangleD RenderContentRegion
        {
            get { return renderContentRegion; }
        }

        /// <inheritdoc/>
        public override RectangleD RelativeContentRegion
        {
            get { return relativeContentRegion; }
        }

        /// <inheritdoc/>
        public override RectangleD AbsoluteContentRegion
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
        internal override void PostDrawOverride(UltravioletTime time, DrawingContext dc)
        {
            if (componentRoot != null)
            {
                componentRoot.Draw(time, dc);
            }
            base.PostDrawOverride(time, dc);
        }

        /// <inheritdoc/>
        internal override void PostUpdateOverride(UltravioletTime time)
        {
            if (componentRoot != null)
            {
                componentRoot.Update(time);
            }
            base.PostUpdateOverride(time);
        }

        /// <summary>
        /// Called when the control's content presenter (if it has one) is measured.
        /// </summary>
        /// <param name="availableSize">The size of the area which the element's parent has 
        /// specified is available for the element's layout.</param>
        /// <returns>The desired size of the control's content.</returns>
        internal Size2D OnContentPresenterMeasure(Size2D availableSize)
        {
            CacheDesiredContentRegion(availableSize);
            return MeasureContent(availableSize);
        }

        /// <summary>
        /// Called when the control's content presenter (if it has one) is arranged.
        /// </summary>
        /// <param name="finalSize">The element's final size.</param>
        /// <param name="options">A set of <see cref="ArrangeOptions"/> values specifying the options for this arrangement.</param>
        /// <returns>The amount of space that was actually used by the control's content.</returns>
        internal Size2D OnContentPresenterArrange(Size2D finalSize, ArrangeOptions options)
        {
            CacheRenderContentRegion(finalSize);
            return ArrangeContent(finalSize, options);
        }

        /// <summary>
        /// Called when the control's content presenter (if it has one) is positioned.
        /// </summary>
        /// <param name="position">The position of the element's parent element in absolute screen space.</param>
        internal void OnContentPresenterPosition(Point2D position)
        {
            CacheRelativeAndAbsoluteContentRegion();
            PositionContent(position);
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
        internal UIElement ContentPresenter
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
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (componentRoot == null)
            {
                CacheDesiredContentRegion(availableSize);
                return MeasureContent(availableSize);
            }
            return MeasureComponents(availableSize);
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            if (componentRoot == null)
            {
                CacheRenderContentRegion(finalSize);
                return ArrangeContent(finalSize, options);
            }
            return ArrangeComponents(finalSize, options);
        }

        /// <inheritdoc/>
        protected override void PositionOverride(Point2D position)
        {
            if (componentRoot == null)
            {
                CacheRelativeAndAbsoluteContentRegion();
                PositionContent(AbsolutePosition);
            }
            else
            {
                PositionComponents(AbsolutePosition);
            }
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
        /// Gets the total padding around the control's content.
        /// </summary>
        /// <returns>A <see cref="Thickness"/> that describes the total padding around the control's content.</returns>
        protected virtual Thickness GetTotalContentPadding()
        {
            return Padding;
        }

        /// <summary>
        /// Measures the control's content.
        /// </summary>
        /// <param name="availableSize">The size of the area which the element's parent has 
        /// specified is available for the element's layout.</param>
        /// <returns></returns>
        protected virtual Size2D MeasureContent(Size2D availableSize)
        {
            return availableSize;
        }

        /// <summary>
        /// Arranges the control's content.
        /// </summary>
        /// <param name="finalRect">The element's final position and size relative to its parent element.</param>
        /// <param name="options">A set of <see cref="ArrangeOptions"/> values specifying the options for this arrangement.</param>
        /// <returns></returns>
        protected virtual Size2D ArrangeContent(Size2D finalSize, ArrangeOptions options)
        {
            return finalSize;
        }

        /// <summary>
        /// Positions the control's content in absolute screen space.
        /// </summary>
        /// <param name="position">The position of the element's parent element in absolute screen space.</param>
        protected virtual void PositionContent(Point2D position)
        {

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
        /// Measures the control's components.
        /// </summary>
        /// <param name="availableSize">The size of the area which the element's parent has 
        /// specified is available for the element's layout.</param>
        /// <returns>The component root's desired size, considering the size of any content elements.</returns>
        protected Size2D MeasureComponents(Size2D availableSize)
        {
            if (componentRoot == null)
                return Size2D.Zero;

            componentRoot.Measure(availableSize);
            return componentRoot.DesiredSize;
        }

        /// <summary>
        /// Arranges the control's components.
        /// </summary>
        /// <param name="finalRect">The element's final position and size relative to its parent element.</param>
        /// <param name="options">A set of <see cref="ArrangeOptions"/> values specifying the options for this arrangement.</param>
        /// <returns>The amount of space that was actually used by the component root.</returns>
        protected Size2D ArrangeComponents(Size2D finalSize, ArrangeOptions options)
        {
            if (componentRoot == null)
                return Size2D.Zero;

            var finalRect = new RectangleD(Point2D.Zero, finalSize);
            componentRoot.Arrange(finalRect, options);
            return componentRoot.RenderSize;
        }

        /// <summary>
        /// Positions the control's components.
        /// </summary>
        /// <param name="position">The position of the element's parent element in absolute screen space.</param>
        protected void PositionComponents(Point2D position)
        {
            if (componentRoot == null)
                return;

            componentRoot.Position(position);
        }

        /// <summary>
        /// Updates the cached value of the <see cref="DesiredContentRegion"/> property.
        /// </summary>
        protected void CacheDesiredContentRegion(Size2D availableSize)
        {
            this.desiredContentRegion = new RectangleD(Point2D.Zero, availableSize - GetTotalContentPadding());
        }

        /// <summary>
        /// Updates the cached value of the <see cref="RenderContentRegion"/> property.
        /// </summary>
        protected void CacheRenderContentRegion(Size2D finalSize)
        {
            this.renderContentRegion = new RectangleD(Point2D.Zero, finalSize - GetTotalContentPadding());
        }

        /// <summary>
        /// Updates the cached value of the <see cref="RelativeContentRegion"/> and <see cref="AbsoluteContentRegion"/> properties.
        /// </summary>
        protected void CacheRelativeAndAbsoluteContentRegion()
        {
            var contentRegionOffset = (contentPresenter == null) ? 
                Point2D.Zero : (contentPresenter.AbsolutePosition - AbsolutePosition);

            var padding = GetTotalContentPadding();

            var contentRegionOffsetWithPadding = new Point2D(
                padding.Left + contentRegionOffset.X,
                padding.Top + contentRegionOffset.Y);

            this.relativeContentRegion = new RectangleD(contentRegionOffsetWithPadding, renderContentRegion.Size);
            this.absoluteContentRegion = this.relativeContentRegion + AbsolutePosition;
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
        private RectangleD relativeContentRegion;
        private RectangleD absoluteContentRegion;
        private UIElement componentRoot;
        private UIElement contentPresenter;

        // The registry of components belonging to this control.
        private readonly UIElementRegistry componentRegistry = new UIElementRegistry();
    }
}
