using System;
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
    public abstract class Container : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Container"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public Container(UltravioletContext uv, String id)
            : base(uv, id)
        {
            this.children = new UIElementCollection(this);
        }

        /// <inheritdoc/>
        public sealed override void ClearLocalValuesRecursive()
        {
            base.ClearLocalValuesRecursive();

            foreach (var child in children)
            {
                child.ClearLocalValuesRecursive();
            }
        }

        /// <inheritdoc/>
        public sealed override void ClearStyledValuesRecursive()
        {
            base.ClearStyledValuesRecursive();

            foreach (var child in children)
            {
                child.ClearStyledValuesRecursive();
            }
        }

        /// <inheritdoc/>
        public sealed override void ClearVisualStateTransitionsRecursive()
        {
            base.ClearVisualStateTransitionsRecursive();

            foreach (var child in children)
            {
                child.ClearVisualStateTransitionsRecursive();
            }
        }

        /// <inheritdoc/>
        public sealed override void ReloadContentRecursive()
        {
            base.ReloadContentRecursive();

            foreach (var child in children)
            {
                child.ReloadContentRecursive();
            }
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

            foreach (var child in children)
            {
                child.ApplyStyles(stylesheet);
            }
        }

        /// <inheritdoc/>
        internal override void ApplyStoryboard(Storyboard storyboard, StoryboardClock clock, UIElement root)
        {
            base.ApplyStoryboard(storyboard, clock, root);

            foreach (var child in children)
            {
                child.ApplyStoryboard(storyboard, clock, root);
            }
        }

        /// <inheritdoc/>
        internal override Boolean Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            if (!base.Draw(time, spriteBatch))
                return false;

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

            return true;
        }

        /// <inheritdoc/>
        internal override void Update(UltravioletTime time)
        {
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

            foreach (var child in children)
            {
                child.UpdateViewModel(viewModel);
            }
        }

        /// <inheritdoc/>
        internal override void UpdateView(UIView view)
        {
            base.UpdateView(view);

            foreach (var child in children)
            {
                child.UpdateView(view);
            }
        }

        /// <inheritdoc/>
        internal override void UpdateContainer(UIElement container)
        {
            base.UpdateContainer(container);

            foreach (var child in children)
            {
                child.UpdateContainer(this);
            }
        }

        /// <inheritdoc/>
        internal override void UpdateControl()
        {
            base.UpdateControl();

            foreach (var child in children)
            {
                child.UpdateControl();
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
        private Boolean requiresScissorRectangle;
    }
}
