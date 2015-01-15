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

        protected override void UpdateLayoutDepthCore(int layoutDepth)
        {
            foreach (var child in Children)
            {
                child.UpdateLayoutDepth(layoutDepth + 1);
            }
            base.UpdateLayoutDepthCore(layoutDepth);
        }

        protected override void UpdateAbsoluteParentOriginCore(int x, int y)
        {
            // todo
            var thisOriginX = x + 0;
            var thisOriginY = y + 0;
            foreach (var child in Children)
            {
                child.UpdateAbsoluteParentOrigin(thisOriginX, thisOriginY);
            }
            base.UpdateAbsoluteParentOriginCore(x, y);
        }




        /// <inheritdoc/>
        public sealed override void ClearAnimationsRecursive()
        {
            base.ClearAnimationsRecursive();

            foreach (var child in children)
            {
                child.ClearAnimationsRecursive();
            }
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

        /// <summary>
        /// Gets the container's collection of child elements.
        /// </summary>
        public UIElementCollection Children
        {
            get { return children; }
        }

        /// <inheritdoc/>
        internal override Boolean RemoveContent(UIElement element)
        {
            Contract.Require(element, "element");

            return children.Remove(element);
        }

        /// <inheritdoc/>
        internal override Boolean Draw(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            if (!base.Draw(time, spriteBatch, opacity))
                return false;

            DrawChildren(time, spriteBatch, opacity);

            return true;
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
        internal override UIElement GetElementAtPointInternal(Int32 x, Int32 y, Boolean hitTest)
        {
            if (!Bounds.Contains(x, y) || !ElementIsDrawn(this))
                return null;

            // TODO

            return base.GetElementAtPointInternal(x, y, hitTest);
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
        /// Draws the container's children.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The sprite batch with which to draw the view.</param>
        /// <param name="opacity">The cumulative opacity of all of the element's parent elements.</param>
        protected virtual void DrawChildren(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            var cumulativeOpacity = Opacity * opacity;
            foreach (var child in children)
            {
                if (!ElementIsDrawn(child))
                    continue;

                child.Draw(time, spriteBatch, cumulativeOpacity);
            }
        }

        // Property values.
        private readonly UIElementCollection children;
    }
}
