using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an attribute which specifies a type's default property.
    /// </summary>
    /// <remarks>Ultraviolet uses this class instead of <see cref="System.ComponentModel.DefaultPropertyAttribute"/> because
    /// that type is removed by the Xamarin linker on iOS.</remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class UvmlDefaultPropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlDefaultPropertyAttribute"/> class.
        /// </summary>
        /// <value>The name of the default property for this attribute's type.</value>
        public UvmlDefaultPropertyAttribute (String name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name of the default property for this attribute's type.
        /// </summary>
        public String Name { get; private set; }
    }
}

