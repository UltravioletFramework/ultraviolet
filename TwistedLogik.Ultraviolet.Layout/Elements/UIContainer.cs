using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents an interface element which can contain other elements.
    /// </summary>
    public abstract class UIContainer : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIContainer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its layout.</param>
        public UIContainer(UltravioletContext uv, String id)
            : base(uv, id)
        {
            this.children = new UIElementCollection(this);
        }

        /// <summary>
        /// Recursively clears the local values of all of the container's dependency properties
        /// and all of the dependency properties of the container's descendents.
        /// </summary>
        public void ClearLocalValuesRecursive()
        {
            ClearLocalValues();

            foreach (var child in children)
            {
                child.ClearLocalValues();
            }
        }

        /// <summary>
        /// Recursively clears the styled values of all of the container's dependency properties
        /// and all of the dependency properties of the container's descendents.
        /// </summary>
        public void ClearStyledValuesRecursive()
        {
            ClearStyledValues();

            foreach (var child in children)
            {
                child.ClearStyledValues();
            }
        }

        /// <summary>
        /// Requests that a layout be performed during the next call to <see cref="UIElement.Update(UltravioletTime)"/>.
        /// </summary>
        public void RequestLayout()
        {
            layoutRequested = true;
        }

        /// <summary>
        /// Immediately recalculates the layout of the container and all of its children.
        /// If layout is currently suspended, this operation is deferred until layout is resumed.
        /// </summary>
        public void PerformLayout()
        {
            OnPerformingLayout();

            foreach (var child in children)
            {
                PerformLayoutInternal(child);
            }

            OnPerformedLayout();
        }

        /// <summary>
        /// Immediately recalculates the layout of the specified child element.
        /// </summary>
        /// <param name="child">The child element for which to calculate a layout.</param>
        public void PerformLayout(UIElement child)
        {
            Contract.Require(child, "child");
            Contract.Ensure<ArgumentException>(Children.Contains(child), "child");

            PerformLayoutInternal(child);
        }

        /// <summary>
        /// Called when the element and its children should reload their content.
        /// </summary>
        public void ReloadContentRecursively()
        {
            ReloadContent();

            foreach (var child in children)
            {
                var container = child as UIContainer;
                if (container != null)
                {
                    container.ReloadContentRecursively();
                }
                else
                {
                    container.ReloadContent();
                }
            }
        }

        /// <summary>
        /// Gets the container's collection of child elements.
        /// </summary>
        public UIElementCollection Children
        {
            get { return children; }
        }

        /// <summary>
        /// Called before the container performs a layout.
        /// </summary>
        protected virtual void OnPerformingLayout()
        {

        }

        /// <summary>
        /// Called after the container has performed a layout.
        /// </summary>
        protected virtual void OnPerformedLayout()
        {

        }

        /// <summary>
        /// Calculates the container-relative layout area of the specified child element.
        /// </summary>
        /// <param name="child">The child element for which to calculate a layout area.</param>
        /// <returns>The container-relative layout area of the specified child element.</returns>
        protected abstract Rectangle CalculateLayoutArea(UIElement child);

        /// <inheritdoc/>
        internal override void UpdateInternal(UltravioletTime time)
        {
            base.UpdateInternal(time);

            foreach (var child in children)
            {
                child.Update(time);
            }

            if (layoutRequested)
            {
                layoutRequested = false;
                PerformLayout();
            }
        }

        /// <summary>
        /// Updates the view model associated with this element.
        /// </summary>
        /// <param name="viewModel">The view model to associate with this element.</param>
        internal override void UpdateViewModel(Object viewModel)
        {
            base.UpdateViewModel(viewModel);

            foreach (var child in children)
            {
                child.UpdateViewModel(viewModel);
            }
        }

        /// <inheritdoc/>
        internal override void UpdateViewport(UIViewport viewport)
        {
            base.UpdateViewport(viewport);

            foreach (var child in children)
            {
                child.UpdateViewport(viewport);
            }
        }

        /// <summary>
        /// Immediately recalculates the layout of the specified child element.
        /// </summary>
        /// <param name="child">The child element for which to calculate a layout.</param>
        private void PerformLayoutInternal(UIElement child)
        {
            var layout = CalculateLayoutArea(child);
            child.ContainerRelativeLayout = layout;

            var container = child as UIContainer;
            if (container != null)
            {
                container.PerformLayout();
            }
        }

        // Property values.
        private readonly UIElementCollection children;

        // State values.
        private Boolean layoutRequested;
    }
}
