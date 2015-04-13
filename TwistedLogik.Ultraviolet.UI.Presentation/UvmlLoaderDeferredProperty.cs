using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a property whose population has been deferred until after the UVML loader finishes
    /// loading the entire layout that contains the property's owner.
    /// </summary>
    internal struct UvmlLoaderDeferredProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlLoaderDeferredProperty"/> structure.
        /// </summary>
        /// <param name="uiElement">The <see cref="UIElement"/> that is being populated.</param>
        /// <param name="name">The property's name.</param>
        /// <param name="value">The property's value.</param>
        public UvmlLoaderDeferredProperty(UIElement uiElement, String name, String value)
        {
            this.uiElement = uiElement;
            this.name      = name;
            this.value     = value;
        }

        /// <summary>
        /// Gets the <see cref="UIElement"/> that is being populated.
        /// </summary>
        public UIElement UIElement
        {
            get { return uiElement; }
        }

        /// <summary>
        /// Gets the property's name.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the property's value.
        /// </summary>
        public String Value
        {
            get { return value; }
        }

        // Property values.
        private readonly UIElement uiElement;
        private readonly String name;
        private readonly String value;
    }
}
