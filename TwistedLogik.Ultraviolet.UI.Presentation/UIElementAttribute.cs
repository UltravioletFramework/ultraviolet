using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents an attribute which marks a class as a UI element and associates it with
    /// a name which can be used to instantiate it via XML.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class UIElementAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIElementAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the XML element that represents this class.</param>
        /// <param name="componentTemplate">The manifest resource name of the element's default component template, if it has one.</param>
        public UIElementAttribute(String name, String componentTemplate = null)
        {
            Contract.RequireNotEmpty(name, "name");

            this.name              = name;
            this.componentTemplate = componentTemplate;
        }

        /// <summary>
        /// Gets the name of the XML element that represents this class.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the manifest resource name of the element's default component template, if it has one.
        /// </summary>
        public String ComponentTemplate
        {
            get { return componentTemplate; }
        }

        // Property values.
        private readonly String name;
        private readonly String componentTemplate;
    }
}
