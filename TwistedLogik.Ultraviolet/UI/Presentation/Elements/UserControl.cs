using System;
using System.Linq;
using System.Reflection;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a control which is defined by the application using a layout definition.
    /// </summary>
    public abstract class UserControl : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserControl"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        protected UserControl(UltravioletContext uv, String id)
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
            if (Content != null)
            {
                Content.ContainerRelativeArea = new Rectangle(0, 0, ActualWidth, ActualHeight);
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
                    {
                        content.UpdateIsComponent(false, true);
                        content.UpdateContainer(this);
                    }

                    PerformLayout();

                    OnContentChanged();
                }
            }
        }

        /// <summary>
        /// Occurs when the value of the control's <see cref="Content"/> property changes.
        /// </summary>
        public event UIElementEventHandler ContentChanged;

        /// <summary>
        /// Populates any private fields of this object which match components
        /// of the user control.
        /// </summary>
        internal void PopulateFieldsFromRegisteredElements()
        {
            ComponentRegistry.PopulateFieldsFromRegisteredElements(this);
        }

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
                Content.UpdateAbsoluteScreenPosition(x, y);
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
        internal override void Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            base.Draw(time, spriteBatch);

            if (Content != null)
                Content.Draw(time, spriteBatch);
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
        internal override UIElement GetElementAtPointInternal(Int32 x, Int32 y)
        {
            if (!Bounds.Contains(x, y))
                return null;

            if (Content != null)
                return Content.GetElementAtPointInternal(x, y);

            return null;
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

        /// <summary>
        /// Gets the user control's component registry.
        /// </summary>
        internal ElementRegistry ComponentRegistry
        {
            get { return componentRegistry; }
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
        /// Gets the element within the user control which has the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the element to retrieve.</param>
        /// <returns>The element with the specified identifier, or <c>null</c> if no such element exists.</returns>
        protected UIElement GetComponentByID(String id)
        {
            Contract.RequireNotEmpty(id, "id");

            return componentRegistry.GetElementByID(id);
        }

        // Property values.
        private UIElement content;

        // State values.
        private readonly ElementRegistry componentRegistry = 
            new ElementRegistry();
    }
}
