using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a control which displays a single item of content.
    /// </summary>
    public abstract class ContentControl : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentControl"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The unique identifier of this element within its layout.</param>
        public ContentControl(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Gets or sets the control's content element.
        /// </summary>
        public UIElement Content
        {
            get { return content; }
            set
            {
                if (content == value)
                    return;

                if (content != null && content.Parent != null)
                    content.Parent.RemoveChild(content);

                content = value;
                content.Parent = this;
            }
        }

        /// <inheritdoc/>
        protected internal override void RemoveChild(UIElement child)
        {
            if (Content == child)
            {
                Content = null;
            }
            base.RemoveChild(child);
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            if (Content != null)
            {
                Content.Draw(time, spriteBatch, opacity);
            }
            base.DrawOverride(time, spriteBatch, opacity);
        }

        /// <inheritdoc/>
        protected override void UpdateOverride(UltravioletTime time)
        {
            if (Content != null)
            {
                Content.Update(time);
            }
            base.UpdateOverride(time);
        }

        /// <inheritdoc/>
        protected override void ReloadContentCore(Boolean recursive)
        {
            if (recursive && Content != null)
            {
                Content.ReloadContent(true);
            }
            base.ReloadContentCore(recursive);
        }

        /// <inheritdoc/>
        protected override void ClearAnimationsCore(Boolean recursive)
        {
            if (recursive && Content != null)
            {
                Content.ClearAnimations(true);
            }
            base.ClearAnimationsCore(recursive);
        }

        /// <inheritdoc/>
        protected override void ClearLocalValuesCore(Boolean recursive)
        {
            if (recursive && Content != null)
            {
                Content.ClearLocalValues(true);
            }
            base.ClearLocalValuesCore(recursive);
        }

        /// <inheritdoc/>
        protected override void ClearStyledValuesCore(Boolean recursive)
        {
            if (recursive && Content != null)
            {
                Content.ClearStyledValues(true);
            }
            base.ClearStyledValuesCore(recursive);
        }

        /// <inheritdoc/>
        protected override void CleanupCore()
        {
            if (Content != null)
            {
                Content.Cleanup();
            }
            base.CleanupCore();
        }

        /// <inheritdoc/>
        protected override void CacheLayoutParametersCore()
        {
            if (Content != null)
            {
                Content.CacheLayoutParameters();
            }
            base.CacheLayoutParametersCore();
        }

        /// <inheritdoc/>
        protected override void AnimateCore(Storyboard storyboard, StoryboardClock clock, UIElement root)
        {
            if (Content != null)
            {
                Content.Animate(storyboard, clock, root);
            }
            base.AnimateCore(storyboard, clock, root);
        }

        /// <inheritdoc/>        
        protected override void StyleOverride(UvssDocument stylesheet)
        {
            if (Content != null)
            {
                Content.Style(stylesheet);
            }
            base.StyleOverride(stylesheet);
        }

        /// <inheritdoc/>
        protected override UIElement GetElementAtPointCore(Double x, Double y, Boolean isHitTest)
        {
            if (Content != null)
            {
                var contentRelX = x - content.RelativeBounds.X;
                var contentRelY = y - content.RelativeBounds.Y;

                var contentMatch = Content.GetElementAtPoint(contentRelX, contentRelY, isHitTest);
                if (contentMatch != null)
                {
                    return contentMatch;
                }
            }
            return base.GetElementAtPointCore(x, y, isHitTest);
        }

        // Property values.
        private UIElement content;
    }
}
