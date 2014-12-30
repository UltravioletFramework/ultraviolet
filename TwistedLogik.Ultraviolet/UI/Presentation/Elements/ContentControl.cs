using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a control which has a single child element.
    /// </summary>
    public abstract class ContentControl : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentControl"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        protected ContentControl(UltravioletContext uv, String id)
            : base(uv, id)
        { }

        /// <inheritdoc/>
        public sealed override void ClearLocalValuesRecursive()
        {
            base.ClearLocalValuesRecursive();

            if (Content != null)
                Content.ClearLocalValuesRecursive();
        }

        /// <inheritdoc/>
        public sealed override void ClearStyledValuesRecursive()
        {
            base.ClearStyledValuesRecursive();

            if (Content != null)
                Content.ClearStyledValuesRecursive();
        }

        /// <inheritdoc/>
        public sealed override void ClearVisualStateTransitionsRecursive()
        {
            base.ClearVisualStateTransitionsRecursive();

            if (Content != null)
                Content.ClearVisualStateTransitionsRecursive();
        }

        /// <inheritdoc/>
        public sealed override void ReloadContentRecursive()
        {
            base.ReloadContentRecursive();

            if (Content != null)
                Content.ReloadContentRecursive();
        }

        /// <inheritdoc/>
        public sealed override void PerformLayout()
        {
            base.PerformLayout();

            if (Content != null)
            {
                Content.ContainerRelativeArea = new Rectangle(0, 0, ContentElement.ActualWidth, ContentElement.ActualHeight);
                Content.PerformLayout();
            }
        }

        /// <inheritdoc/>
        public sealed override void PerformPartialLayout(UIElement content)
        {
            Contract.Require(content, "content");

            if (Content == null)
                throw new ArgumentException("content");

            if (Content == content)
            {
                PerformLayout();
            }
            else
            {
                Content.PerformPartialLayout(content);
            }
        }

        /// <summary>
        /// Gets or sets the control's content object. This is usually a container which can hold child elements.
        /// </summary>
        public UIElement Content
        {
            get { return content; }
            set
            {
                if (content != value)
                {
                    if (content != null)
                        content.UpdateContainer(null);

                    content = value;

                    if (content != null)
                        content.UpdateContainer(this);

                    PerformLayout();

                    OnContentChanged();
                }
            }
        }

        /// <summary>
        /// Occurs when the value of the control's <see cref="Content"/> property changes.
        /// </summary>
        public event UIElementEventHandler ContentChanged;

        /// <inheritdoc/>
        internal override Boolean RemoveContent(UIElement element)
        {
            Contract.Require(element, "element");

            if (Content == element)
            {
                Content = null;
                return true;
            }

            if (Content != null)
                return Content.RemoveContent(element);

            return false;
        }

        /// <inheritdoc/>
        internal override void UpdateAbsoluteScreenPosition(Int32 x, Int32 y)
        {
            base.UpdateAbsoluteScreenPosition(x, y);

            if (Content != null)
            {
                Content.UpdateAbsoluteScreenPosition(
                    ContentElement.AbsoluteScreenX,
                    ContentElement.AbsoluteScreenY);
            }
        }

        /// <inheritdoc/>
        internal override void ApplyStyles(UvssDocument stylesheet)
        {
            base.ApplyStyles(stylesheet);

            if (Content != null)
                Content.ApplyStyles(stylesheet);
        }

        /// <inheritdoc/>
        internal override void ApplyStoryboard(Storyboard storyboard, StoryboardClock clock, UIElement root)
        {
            base.ApplyStoryboard(storyboard, clock, root);

            if (Content != null)
                Content.ApplyStoryboard(storyboard, clock, root);
        }

        /// <inheritdoc/>
        internal override Boolean Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            if (!base.Draw(time, spriteBatch))
                return false;

            if (Content != null)
                Content.Draw(time, spriteBatch);

            OnContentDrawn(time, spriteBatch);

            return true;
        }

        /// <inheritdoc/>
        internal override void Update(UltravioletTime time)
        {
            if (Content != null)
                Content.Update(time);

            base.Update(time);
        }

        /// <inheritdoc/>
        internal override void UpdateViewModel(Object viewModel)
        {
            base.UpdateViewModel(viewModel);

            if (Content != null)
                Content.UpdateViewModel(viewModel);
        }

        /// <inheritdoc/>
        internal override void UpdateView(UIView view)
        {
            base.UpdateView(view);

            if (Content != null)
                Content.UpdateView(view);
        }

        /// <inheritdoc/>
        internal override void UpdateContainer(UIElement container)
        {
            base.UpdateContainer(container);

            if (Content != null)
                Content.UpdateContainer(container);
        }

        /// <inheritdoc/>
        internal override void UpdateControl()
        {
            base.UpdateControl();

            if (Content != null)
                Content.UpdateControl();
        }

        /// <inheritdoc/>
        internal override UIElement GetContentElementInternal(int ix)
        {
            if (Content == null || ix != 0)
                throw new ArgumentOutOfRangeException("ix");

            return Content;
        }

        /// <inheritdoc/>
        internal override UIElement GetElementAtPointInternal(Int32 x, Int32 y, Boolean hitTest)
        {
            if (!Bounds.Contains(x, y))
                return null;
            
            var contentX = x - ContentElement.ContainerRelativeX;
            var contentY = y - ContentElement.ContainerRelativeY;
            if (Content != null && ContentElement.Bounds.Contains(contentX, contentY))
            {
                var content = Content.GetElementAtPointInternal(
                    contentX - Content.ContainerRelativeX, 
                    contentY - Content.ContainerRelativeY, hitTest);

                if (content != null)
                {
                    return content;
                }
            }

            return base.GetElementAtPointInternal(x, y, hitTest);
        }

        /// <inheritdoc/>
        internal override UIElement FindContentPanel()
        {
            if (Content != null)
            {
                var panel = Content.FindContentPanel();
                if (panel != null)
                    return panel;
            }

            return null;
        }

        /// <inheritdoc/>
        internal override Int32 ContentElementCountInternal
        {
            get { return Content == null ? 0 : 1; }
        }

        /// <summary>
        /// Raises the <see cref="ContentChanged"/> event.
        /// </summary>
        protected virtual void OnContentChanged()
        {
            var temp = ContentChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Called after the control draws its content.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The sprite batch with which to draw the view.</param>
        protected virtual void OnContentDrawn(UltravioletTime time, SpriteBatch spriteBatch)
        {

        }

        // Property values.
        private UIElement content;
    }
}
