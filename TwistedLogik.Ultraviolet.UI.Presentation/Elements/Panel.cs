using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a framework element with child elements.
    /// </summary>
    public abstract class Panel : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The unique identifier of this element within its layout.</param>
        public Panel(UltravioletContext uv, String id)
            : base(uv, id)
        {
            this.children = new UIElementCollection(this);
        }

        /// <inheritdoc/>
        public override UIElement GetLogicalChild(int ix)
        {
            if(ix < 0 || ix >= children.Count)
                throw new ArgumentOutOfRangeException("ix");

            return children[ix];
        }

        /// <summary>
        /// Gets the panel's collection of children.
        /// </summary>
        public UIElementCollection Children
        {
            get { return children; }
        }

        /// <inheritdoc/>
        public override Int32 LogicalChildren
        {
            get { return children.Count; }
        }

        /// <summary>
        /// Occurs when children are added to or removed from this panel.
        /// </summary>
        public event UIElementEventHandler ChildrenChanged;

        /// <inheritdoc/>
        protected internal override void RemoveChild(UIElement child)
        {
            if (child != null)
            {
                children.Remove(child);
            }
            base.RemoveChild(child);
        }

        /// <summary>
        /// Raises the <see cref="ChildrenChanged"/> event.
        /// </summary>
        protected internal virtual void OnChildrenChanged()
        {
            var temp = ChildrenChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            DrawChildren(time, dc);

            base.DrawOverride(time, dc);
        }

        /// <inheritdoc/>
        protected override void UpdateOverride(UltravioletTime time)
        {
            foreach (var child in children)
            {
                child.Update(time);
            }
            base.UpdateOverride(time);
        }

        /// <inheritdoc/>
        protected override void InitializeDependencyPropertiesCore(Boolean recursive)
        {
            if (recursive)
            {
                foreach (var child in children)
                    child.InitializeDependencyProperties(recursive);
            }
            base.InitializeDependencyPropertiesCore(recursive);
        }

        /// <inheritdoc/>
        protected override void ReloadContentCore(Boolean recursive)
        {
            if (recursive)
            {
                foreach (var child in children)
                    child.ReloadContent(recursive);
            }
            base.ReloadContentCore(recursive);
        }

        /// <inheritdoc/>
        protected override void ClearAnimationsCore(Boolean recursive)
        {
            if (recursive)
            {
                foreach (var child in children)
                    child.ClearAnimations(recursive);
            }
            base.ClearAnimationsCore(recursive);
        }

        /// <inheritdoc/>
        protected override void ClearLocalValuesCore(Boolean recursive)
        {
            if (recursive)
            {
                foreach (var child in children)
                    child.ClearLocalValues(recursive);
            }
            base.ClearLocalValuesCore(recursive);
        }

        /// <inheritdoc/>
        protected override void ClearStyledValuesCore(Boolean recursive)
        {
            if (recursive)
            {
                foreach (var child in children)
                    child.ClearStyledValues(recursive);
            }
            base.ClearStyledValuesCore(recursive);
        }

        /// <inheritdoc/>
        protected override void CleanupCore()
        {
            foreach (var child in children)
                child.Cleanup();

            base.CleanupCore();
        }

        /// <inheritdoc/>
        protected override void CacheLayoutParametersCore()
        {
            foreach (var child in children)
                child.CacheLayoutParameters();

            base.CacheLayoutParametersCore();
        }

        /// <inheritdoc/>
        protected override void AnimateCore(Storyboard storyboard, StoryboardClock clock, UIElement root)
        {
            foreach (var child in children)
                child.Animate(storyboard, clock, root);

            base.AnimateCore(storyboard, clock, root);
        }

        /// <inheritdoc/>
        protected override void StyleOverride(UvssDocument stylesheet)
        {
            foreach (var child in children)
                child.Style(stylesheet);

            base.StyleOverride(stylesheet);
        }

        /// <inheritdoc/>
        protected override RectangleD? ClipContentCore()
        {
            foreach (var child in children)
            {
                if (child.RelativeBounds.Left < 0 || child.RelativeBounds.Top < 0 ||
                    child.RelativeBounds.Right > RenderSize.Width || child.RelativeBounds.Bottom > RenderSize.Height)
                {
                    return AbsoluteContentRegion;
                }
            }
            return null;
        }

        /// <inheritdoc/>
        protected override UIElement GetElementAtPointCore(Double x, Double y, Boolean isHitTest)
        {
            var childMatch = GetChildAtPoint(x, y, isHitTest);
            if (childMatch != null)
            {
                return childMatch;
            }
            return base.GetElementAtPointCore(x, y, isHitTest);
        }

        /// <summary>
        /// Gets the child element at the specified device-independent coordinates relative to this element's bounds.
        /// </summary>
        /// <param name="x">The element-relative x-coordinate of the point to evaluate.</param>
        /// <param name="y">The element-relative y-coordinate of the point to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="UIElement.IsHitTestVisible"/> property.</param>
        /// <returns>The child element at the specified coordinates, or <c>null</c> if no such element exists.</returns>
        protected virtual UIElement GetChildAtPoint(Double x, Double y, Boolean isHitTest)
        {
            for (int i = children.Count - 1; i >= 0; i--)
            {
                var child = children[i];

                var childRelX = x - child.RelativeBounds.X;
                var childRelY = y - child.RelativeBounds.Y;

                var childMatch = child.GetElementAtPoint(childRelX, childRelY, isHitTest);
                if (childMatch != null)
                {
                    return childMatch;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Draws the panel's children.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        protected virtual void DrawChildren(UltravioletTime time, DrawingContext dc)
        {
            var clip = ClipContentRectangle;
            if (clip != null)
                dc.PushClipRectangle(clip.Value);

            foreach (var child in children)
            {
                child.Draw(time, dc);
            }

            if (clip != null)
                dc.PopClipRectangle();
        }

        // Property values.
        private readonly UIElementCollection children;
    }
}
