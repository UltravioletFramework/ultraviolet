using System;
using System.Reflection;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents an interface element which can contain other elements.
    /// </summary>
    [UIElement("Container")]
    public abstract class UIContainer : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIContainer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public UIContainer(UltravioletContext uv, String id)
            : base(uv, id)
        {
            this.children = new UIElementCollection(this);
        }

        /// <inheritdoc/>
        public sealed override void ClearLocalValuesRecursive()
        {
            base.ClearLocalValuesRecursive();

            if (componentRoot != null)
                componentRoot.ClearLocalValuesRecursive();

            foreach (var child in children)
            {
                child.ClearLocalValuesRecursive();
            }
        }

        /// <inheritdoc/>
        public sealed override void ClearStyledValuesRecursive()
        {
            base.ClearStyledValuesRecursive();

            if (componentRoot != null)
                componentRoot.ClearStyledValuesRecursive();

            foreach (var child in children)
            {
                child.ClearStyledValuesRecursive();
            }
        }

        /// <inheritdoc/>
        public sealed override void ClearVisualStateTransitionsRecursive()
        {
            base.ClearVisualStateTransitionsRecursive();

            if (componentRoot != null)
                componentRoot.ClearVisualStateTransitionsRecursive();

            foreach (var child in children)
            {
                child.ClearVisualStateTransitionsRecursive();
            }
        }

        /// <inheritdoc/>
        public sealed override void ReloadContentRecursive()
        {
            base.ReloadContentRecursive();

            if (componentRoot != null)
                componentRoot.ReloadContentRecursive();

            foreach (var child in children)
            {
                child.ReloadContentRecursive();
            }
        }

        /// <inheritdoc/>
        public sealed override void PerformLayout()
        {
            OnPerformingLayout();

            if (componentRoot != null)
                PerformComponentLayout();

            PerformContentLayout();
            UpdateAbsoluteScreenPosition(AbsoluteScreenX, AbsoluteScreenY);

            OnPerformedLayout();
        }

        /// <inheritdoc/>
        public sealed override void PerformContentLayout()
        {
            foreach (var child in children)
            {
                PerformLayoutInternal(child, false);
            }
            DetermineIfScissorRectangleIsRequired();
        }

        /// <inheritdoc/>
        public sealed override void PerformPartialLayout(UIElement content)
        {
            Contract.Require(content, "content");

            // TODO
//            if (content.Owner != this && !Children.Contains(content))
//                throw new ArgumentException("content");

            PerformLayoutInternal(content, true);
        }

        /// <summary>
        /// Gets the container's collection of child elements.
        /// </summary>
        public UIElementCollection Children
        {
            get { return children; }
        }

        /// <summary>
        /// Gets the root element of the container's component hierarchy.
        /// </summary>
        public UIContainer ComponentRoot
        {
            get { return componentRoot; }
            internal set
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
        /// Calculates the container-relative layout area of the specified child element.
        /// </summary>
        /// <param name="child">The child element for which to calculate a layout area.</param>
        /// <returns>The container-relative layout area of the specified child element.</returns>
        protected abstract Rectangle CalculateLayoutArea(UIElement child);

        /// <summary>
        /// Gets or sets a value indicating whether this container requires that a scissor
        /// rectangle be applied prior to rendering its children.
        /// </summary>
        protected Boolean RequiresScissorRectangle
        {
            get { return requiresScissorRectangle; }
            set { requiresScissorRectangle = value; }
        }

        /// <summary>
        /// Populates any private fields of this object which match components
        /// of the container.
        /// </summary>
        internal void PopulateFieldsFromRegisteredElements()
        {
            ComponentRegistry.PopulateFieldsFromRegisteredElements(this);
        }

        /// <summary>
        /// Attempts to remove the specified child or content element from this element.
        /// </summary>
        /// <param name="element">The child or content element to remove.</param>
        /// <returns><c>true</c> if the child or content element was removed; otherwise, <c>false</c>.</returns>
        internal override Boolean RemoveContent(UIElement element)
        {
            Contract.Require(element, "element");

            return children.Remove(element);
        }

        /// <inheritdoc/>
        internal override void UpdateAbsoluteScreenPosition(Int32 x, Int32 y)
        {
            base.UpdateAbsoluteScreenPosition(x, y);

            if (componentRoot != null)
                componentRoot.UpdateAbsoluteScreenPosition(x, y);

            foreach (var child in children)
            {
                child.UpdateAbsoluteScreenPosition(
                    ContentElement.AbsoluteScreenX + child.ContainerRelativeX, 
                    ContentElement.AbsoluteScreenY + child.ContainerRelativeY);
            }
        }

        /// <inheritdoc/>
        internal override void ApplyStyles(UvssDocument stylesheet)
        {
            base.ApplyStyles(stylesheet);

            if (componentRoot != null)
                componentRoot.ApplyStyles(stylesheet);

            foreach (var child in children)
            {
                child.ApplyStyles(stylesheet);
            }
        }

        /// <inheritdoc/>
        internal override void ApplyStoryboard(Storyboard storyboard, StoryboardClock clock, UIElement root)
        {
            base.ApplyStoryboard(storyboard, clock, root);

            if (componentRoot != null)
                componentRoot.ApplyStoryboard(storyboard, clock, root);

            foreach (var child in children)
            {
                child.ApplyStoryboard(storyboard, clock, root);
            }
        }

        /// <inheritdoc/>
        internal override void Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            if (View == null || !Visible)
                return;

            base.Draw(time, spriteBatch);

            if (componentRoot != null)
                componentRoot.Draw(time, spriteBatch);

            var scissor     = RequiresScissorRectangle;
            var scissorRect = default(Rectangle?);

            if (scissor)
            {
                ApplyScissorRectangle(spriteBatch, out scissorRect);
            }

            foreach (var child in children)
            {
                if (child.Visible)
                {
                    child.Draw(time, spriteBatch);
                }
            }

            if (scissor)
            {
                RestoreScissorRectangle(spriteBatch, scissorRect);
            }
        }

        /// <inheritdoc/>
        internal override void Update(UltravioletTime time)
        {
            if (componentRoot != null)
                componentRoot.Update(time);

            foreach (var child in children)
            {
                child.Update(time);
            }

            base.Update(time);
        }

        /// <inheritdoc/>
        internal override void UpdateViewModel(Object viewModel)
        {
            base.UpdateViewModel(viewModel);

            if (componentRoot != null)
                componentRoot.UpdateViewModel(viewModel);

            foreach (var child in children)
            {
                child.UpdateViewModel(viewModel);
            }
        }

        /// <inheritdoc/>
        internal override void UpdateView(UIView view)
        {
            base.UpdateView(view);

            if (componentRoot != null)
                componentRoot.UpdateView(view);

            foreach (var child in children)
            {
                child.UpdateView(view);
            }
        }

        /// <inheritdoc/>
        internal override void UpdateContainer(UIElement container)
        {
            base.UpdateContainer(container);

            if (componentRoot != null)
                componentRoot.UpdateContainer(this);

            foreach (var child in children)
            {
                child.UpdateContainer(this);
            }
        }

        /// <inheritdoc/>
        internal override void UpdateIsComponent(Boolean isContainerComponent, Boolean isUserControlComponent)
        {
            base.UpdateIsComponent(isContainerComponent, isUserControlComponent);

            foreach (var child in children)
            {
                child.UpdateIsComponent(isContainerComponent, isUserControlComponent);
            }
        }

        /// <inheritdoc/>
        internal override UIElement GetElementAtPointInternal(Int32 x, Int32 y)
        {
            if (!Bounds.Contains(x, y))
                return null;

            var contentX = x - ContentElement.ContainerRelativeX;
            var contentY = y - ContentElement.ContainerRelativeY;
            if (ContentElement.Bounds.Contains(contentX, contentY))
            {
                for (int i = children.Count - 1; i >= 0; i--)
                {
                    var child   = children[i];
                    var element = child.GetElementAtPointInternal(contentX - child.ContainerRelativeX, contentY - child.ContainerRelativeY);

                    if (element != null)
                    {
                        return element;
                    }
                }
            }

            if (componentRoot != null)
            {
                var component = componentRoot.GetElementAtPointInternal(x, y);
                if (component != null)
                    return component;
            }

            return base.GetElementAtPointInternal(x, y);
        }

        /// <inheritdoc/>
        internal override UIElement FindContentPanel()
        {
            foreach (var child in children)
            {
                var panel = child.FindContentPanel();
                if (panel != null)
                    return panel;
            }

            return null;
        }

        /// <summary>
        /// Gets the container's component registry.
        /// </summary>
        internal ElementRegistry ComponentRegistry
        {
            get { return componentRegistry; }
        }

        /// <summary>
        /// Gets the element within the user control which has the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the element to retrieve.</param>
        /// <returns>The element with the specified identifier, or <c>null</c> if no such element exists.</returns>
        protected UIElement GetComponentByID(String id)
        {
            Contract.RequireNotEmpty(id, "id");

            return componentRegistry.GetElementByID(id);
        }

        /// <summary>
        /// Immediately recalculates the layout of the container's components.
        /// </summary>
        private void PerformComponentLayout()
        {
            ComponentRoot.ContainerRelativeArea = new Rectangle(0, 0, ActualWidth, ActualHeight);

            ComponentRoot.PerformLayout();
            ComponentRoot.UpdateAbsoluteScreenPosition(AbsoluteScreenX, AbsoluteScreenY);
        }

        /// <summary>
        /// Immediately recalculates the layout of the specified child element.
        /// </summary>
        /// <param name="child">The child element for which to calculate a layout.</param>
        /// <param name="single">A value indicating whether this is the only element being laid out.</param>
        private void PerformLayoutInternal(UIElement child, Boolean single)
        {
            var layout = (child == ComponentRoot) ? new Rectangle(0, 0, ActualWidth, ActualHeight) : CalculateLayoutArea(child);
            child.ContainerRelativeArea = layout;

            child.PerformLayout();
            child.UpdateAbsoluteScreenPosition(
                ContentElement.AbsoluteScreenX + child.ContainerRelativeX,
                ContentElement.AbsoluteScreenY + child.ContainerRelativeY);

            if (single)
                DetermineIfScissorRectangleIsRequired();
        }

        /// <summary>
        /// Determines whether a scissor rectangle must be applied to this container.
        /// </summary>
        private void DetermineIfScissorRectangleIsRequired()
        {
            var required = false;
            foreach (var child in children)
            {
                if (child.ContainerRelativeX < 0 || 
                    child.ContainerRelativeY < 0 ||
                    child.ContainerRelativeX + child.ActualWidth > ContentElement.ActualWidth ||
                    child.ContainerRelativeY + child.ActualHeight > ContentElement.ActualHeight)
                {
                    required = true;
                    break;
                }
            }
            RequiresScissorRectangle = required;
        }

        /// <summary>
        /// Applies the container's scissor rectangle to the graphics device.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which the container is being rendered.</param>
        /// <param name="previous">The previous scissor rectangle, or <c>null</c> if the scissor test is disabled.</param>
        private void ApplyScissorRectangle(SpriteBatch spriteBatch, out Rectangle? previous)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, Graphics.BlendState.AlphaBlend);

            var scissorX      = ContentElement.AbsoluteScreenX;
            var scissorY      = ContentElement.AbsoluteScreenY;
            var scissorWidth  = ContentElement.ActualWidth;
            var scissorHeight = ContentElement.ActualHeight;

            var scissorRectContainer = Ultraviolet.GetGraphics().GetScissorRectangle() ?? View.Area;
            var scissorRectElement   = new Rectangle(scissorX, scissorY, scissorWidth, scissorHeight);
            var scissorRectIntersect = default(Rectangle);
            Rectangle.Intersect(ref scissorRectContainer, ref scissorRectElement, out scissorRectIntersect);

            Ultraviolet.GetGraphics().SetScissorRectangle(scissorRectIntersect);

            previous = scissorRectContainer;
        }

        /// <summary>
        /// Restores the previous scissor rectangle after the container is done rendering its children.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which the container is being rendered.</param>
        /// <param name="rect">The scissor rectangle to apply to the graphics device.</param>
        private void RestoreScissorRectangle(SpriteBatch spriteBatch, Rectangle? rect)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            Ultraviolet.GetGraphics().SetScissorRectangle(rect);
        }

        // Property values.
        private readonly UIElementCollection children;
        private UIContainer componentRoot;
        private Boolean requiresScissorRectangle;

        // State values.
        private readonly ElementRegistry componentRegistry = 
            new ElementRegistry();
    }
}
