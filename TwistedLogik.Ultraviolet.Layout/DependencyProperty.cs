using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

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
        private DependencyProperty(Int64 id, String name, RuntimeTypeHandle propertyType, RuntimeTypeHandle ownerType, DependencyPropertyMetadata metadata)
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
            Contract.Require(name, "name");
            Contract.Require(propertyType, "propertyType");
            Contract.Require(ownerType, "ownerType");

            var propertyDomain = GetPropertyDomain(ownerType);
            if (propertyDomain.ContainsKey(name))
            {
                throw new ArgumentException(LayoutStrings.DependencyPropertyAlreadyRegistered);
            }
            var dp = new DependencyProperty(dpid++, name, propertyType.TypeHandle, ownerType.TypeHandle, metadata);
            propertyDomain[name] = dp;
            return dp;
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
            Contract.Require(name, "name");
            Contract.Require(ownerType, "ownerType");

            if (!ownerType.IsSubclassOf(typeof(DependencyObject)))
                throw new InvalidOperationException(LayoutStrings.IsNotDependencyObject);
            
            var type = ownerType;
            while (type != null)
            {
                var property = default(DependencyProperty);
                var propertyDomain = GetPropertyDomain(type);
                if (propertyDomain.TryGetValue(name, out property))
                    return property;
                type = type.BaseType;
            }
            return null;
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

        /// <summary>
        /// Gets the dependency property domain associated with the specified owner type.
        /// </summary>
        /// <param name="type">The type for which to retrieve a dependency property domain.</param>
        /// <returns>The dependency property domain associated with the specified owner type.</returns>
        private static Dictionary<String, DependencyProperty> GetPropertyDomain(Type type)
        {
            Dictionary<String, DependencyProperty> propertyDomain;
            if (!DependencyPropertyRegistry.TryGetValue(type.TypeHandle, out propertyDomain))
            {
                propertyDomain = new Dictionary<String, DependencyProperty>();
                DependencyPropertyRegistry[type.TypeHandle] = propertyDomain;
            }
            return propertyDomain;
        }

        // The dependency property registry.
        private static readonly Dictionary<RuntimeTypeHandle, Dictionary<String, DependencyProperty>> DependencyPropertyRegistry =
            new Dictionary<RuntimeTypeHandle, Dictionary<String, DependencyProperty>>();
        private static Int64 dpid = 1;

        // Property values.
        private readonly Int64 id;
        private readonly String name;
        private readonly RuntimeTypeHandle propertyType;
        private readonly RuntimeTypeHandle ownerType;
        private readonly DependencyPropertyMetadata metadata;
    }
}
