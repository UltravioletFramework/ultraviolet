using System;

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
        /// Calculates the layout of the container and all of its children.
        /// </summary>
        public void PerformLayout()
        {
            OnPerformingLayout();

            foreach (var child in children)
            {
                var layout = CalculateLayoutArea(child);
                child.ContainerRelativeLayout = layout;

                var container = child as UIContainer;
                if (container != null)
                {
                    container.PerformLayout();
                }
            }

            OnPerformedLayout();
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

        // Property values.
        private readonly UIElementCollection children;
    }
}
