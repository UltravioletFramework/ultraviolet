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
        public override void CalculateContentSize(ref Int32? width, ref Int32? height)
        {
            if (Content != null)
            {
                Content.CalculateContentSize(ref width, ref height);
            }
            else
            {
                base.CalculateContentSize(ref width, ref height);
            }
        }

        /// <inheritdoc/>
        public sealed override void ClearAnimationsRecursive()
        {
            base.ClearAnimationsRecursive();

            if (Content != null)
                Content.ClearAnimationsRecursive();
        }

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
        public sealed override void PerformContentLayout()
        {
            if (Content != null)
            {
                var margin = ConvertThicknessToPixels(Content.Margin, 0);

                var availableWidth  = ContentPanelWidth  - (Int32)(margin.Left + margin.Right);
                var availableHeight = ContentPanelHeight - (Int32)(margin.Top  + margin.Bottom);

                int? contentWidth  = Double.IsNaN(Content.Width)  ? availableWidth  : (int?)null;
                int? contentHeight = Double.IsNaN(Content.Height) ? availableHeight : (int?)null;

                Content.CalculateActualSize(ref contentWidth, ref contentHeight);

                var contentX = (Int32)margin.Left;
                var contentY = (Int32)margin.Top;

                Content.ParentRelativeArea = new Rectangle(contentX, contentY, contentWidth ?? 0, contentHeight ?? 0);
                Content.PerformLayout();

                UpdateContentElementPosition(Content);
            }

            base.PerformContentLayout();
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
        internal override Boolean Draw(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            if (!base.Draw(time, spriteBatch, opacity))
                return false;

            if (Content != null && ElementIsDrawn(Content))
            {
                var scissor = 
                    Content.ParentRelativeArea.Left < 0 || 
                    Content.ParentRelativeArea.Top < 0 ||
                    Content.ParentRelativeArea.Right > ContentPanel.ActualWidth ||
                    Content.ParentRelativeArea.Bottom > ContentPanel.ActualHeight;

                if (scissor)
                {
                    var scissorRectangle = new Rectangle(
                        AbsoluteScreenX + ContentPanelX,
                        AbsoluteScreenY + ContentPanelY,
                        ContentPanel.ActualWidth,
                        ContentPanel.ActualHeight);

                    if (!ApplyScissorRectangle(spriteBatch, scissorRectangle))
                    {
                        return false;
                    }
                }

                var cumulativeOpacity = Opacity * opacity;
                Content.Draw(time, spriteBatch, cumulativeOpacity);
                OnContentDrawn(time, spriteBatch, opacity);

                if (scissor)
                {
                    RevertScissorRectangle(spriteBatch);
                }
            }

            return true;
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
        internal override void Update(UltravioletTime time)
        {
            if (Content != null)
            {
                UpdateContentElement(time, Content);
            }
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
        internal override Boolean UpdateAbsoluteScreenPosition(Int32 x, Int32 y, Boolean force = false)
        {
            if (!base.UpdateAbsoluteScreenPosition(x, y, force))
                return false;

            if (Content != null)
            {
                UpdateContentElementPosition(Content, force);
            }

            return true;
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
            if (!Bounds.Contains(x, y) || !ElementIsDrawn(this))
                return null;
            
            var contentX = x - ContentPanelX;
            var contentY = y - ContentPanelY;
            if (Content != null && ContentPanel.Bounds.Contains(contentX, contentY))
            {
                var content = Content.GetElementAtPointInternal(
                    contentX - (Content.ParentRelativeX - ContentScrollX), 
                    contentY - (Content.ParentRelativeY - ContentScrollY), hitTest);

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
        /// <param name="opacity">The cumulative opacity of all of the element's parent elements.</param>
        protected virtual void OnContentDrawn(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {

        }

        // Property values.
        private UIElement content;
    }
}
