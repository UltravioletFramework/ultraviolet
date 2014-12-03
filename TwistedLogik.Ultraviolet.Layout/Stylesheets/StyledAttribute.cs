using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
{
    /// <summary>
    /// Represents an attribute which indicates that a property value can be set by a stylesheet.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class StyledAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StyledAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the property when specified on a stylesheet.</param>
        public StyledAttribute(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            this.name = name;
        }

        /// <summary>
        /// Gets the name of the property when specified on a stylesheet.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        // Property values.
        private readonly String name;
    }
}
