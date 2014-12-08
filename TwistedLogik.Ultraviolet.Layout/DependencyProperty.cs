using System;

namespace TwistedLogik.Ultraviolet.Layout
{
    /// <summary>
    /// Represents a dependency property.
    /// </summary>
    public class DependencyProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyProperty"/> class.
        /// </summary>
        /// <param name="id">The dependency property's unique identifier.</param>
        /// <param name="name">The dependency property's name.</param>
        /// <param name="propertyType">The dependency property's value type.</param>
        /// <param name="ownerType">The dependency property's owner type.</param>
        /// <param name="metadata">The dependency property's metadata.</param>
        internal DependencyProperty(Int64 id, String name, RuntimeTypeHandle propertyType, RuntimeTypeHandle ownerType, DependencyPropertyMetadata metadata)
        {
            this.id           = id;
            this.name         = name;
            this.propertyType = propertyType;
            this.ownerType    = ownerType;
            this.metadata     = metadata ?? DependencyPropertyMetadata.Empty;
        }

        /// <summary>
        /// Registers a new dependency property.
        /// </summary>
        /// <param name="name">The dependency property's name.</param>
        /// <param name="propertyType">The dependency property's value type.</param>
        /// <param name="ownerType">The dependency property's owner type.</param>
        /// <param name="metadata">The dependency property's metadata.</param>
        /// <returns>A <see cref="DependencyProperty"/> instance which represents the registered dependency property.</returns>
        public static DependencyProperty Register(String name, Type propertyType, Type ownerType, DependencyPropertyMetadata metadata = null)
        {
            return DependencyPropertySystem.Register(name, propertyType, ownerType, metadata);
        }

        /// <summary>
        /// Finds the dependency property with the specified name.
        /// </summary>
        /// <param name="name">The name of the dependency property for which to search.</param>
        /// <param name="ownerType">The dependency property's owner type.</param>
        /// <returns>A <see cref="DependencyProperty"/> instance which represents the specified dependency property, 
        /// or <c>null</c> if no such dependency property exists.</returns>
        public static DependencyProperty FindByName(String name, Type ownerType)
        {
            return DependencyPropertySystem.FindByName(name, ownerType);
        }

        /// <summary>
        /// Gets the dependency property's unique identifier.
        /// </summary>
        internal Int64 ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the dependency property's name.
        /// </summary>
        internal String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the dependency property's value type.
        /// </summary>
        internal RuntimeTypeHandle PropertyType
        {
            get { return propertyType; }
        }

        /// <summary>
        /// Gets the dependency property's owner type.
        /// </summary>
        internal RuntimeTypeHandle OwnerType
        {
            get { return ownerType; }
        }

        /// <summary>
        /// Gets the dependency property's metadata.
        /// </summary>
        internal DependencyPropertyMetadata Metadata
        {
            get { return metadata; }
        }

        // Property values.
        private readonly Int64 id;
        private readonly String name;
        private readonly RuntimeTypeHandle propertyType;
        private readonly RuntimeTypeHandle ownerType;
        private readonly DependencyPropertyMetadata metadata;
    }
}
