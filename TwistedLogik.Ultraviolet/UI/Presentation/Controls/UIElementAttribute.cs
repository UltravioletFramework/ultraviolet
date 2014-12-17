using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
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
        public UIElementAttribute(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            this.name = name;
        }

        /// <summary>
        /// Gets the name of the XML element that represents this class.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        // Property values.
        private readonly String name;
    }
}
