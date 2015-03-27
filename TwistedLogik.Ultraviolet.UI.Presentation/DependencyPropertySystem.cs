using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the global state of the dependency property system.
    /// </summary>
    internal static class DependencyPropertySystem
    {
        /// <summary>
        /// Initializes the specified object's dependency properties.
        /// </summary>
        /// <param name="dobj">The object to initialize.</param>
        public static void InitializeObject(DependencyObject dobj)
        {
            Contract.Require(dobj, "dobj");

            var type = dobj.GetType();
            while (type != null && typeof(DependencyObject).IsAssignableFrom(type))
            {
                var domain = GetPropertyDomain(type);

                foreach (var kvp in domain)
                {
                    dobj.InitializeDependencyProperty(kvp.Value);
                }

                type = type.BaseType;
            }
        }

        /// <summary>
        /// Adds an additional owning type to the specified dependency property.
        /// </summary>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to update.</param>
        /// <param name="ownerType">The type to add as an owner for the specified dependency property.</param>
        public static void AddOwner(DependencyProperty dp, Type ownerType)
        {
            Contract.Require(ownerType, "ownerType");
            Contract.Require(dp, "dp");

            var propertyDomain = GetPropertyDomain(ownerType);
            if (propertyDomain.ContainsKey(dp.Name))
            {
                throw new ArgumentException(PresentationStrings.DependencyPropertyAlreadyRegistered);
            }
            propertyDomain[dp.Name] = dp;
        }

        /// <summary>
        /// Registers a new dependency property.
        /// </summary>
        /// <param name="name">The dependency property's name.</param>
        /// <param name="propertyType">The dependency property's value type.</param>
        /// <param name="ownerType">The dependency property's owner type.</param>
        /// <param name="metadata">The dependency property's metadata.</param>
        /// <returns>A <see cref="DependencyProperty"/> instance which represents the registered dependency property.</returns>
        public static DependencyProperty Register(String name, Type propertyType, Type ownerType, PropertyMetadata metadata = null)
        {
            Contract.Require(name, "name");
            Contract.Require(propertyType, "propertyType");
            Contract.Require(ownerType, "ownerType");

            var dp = new DependencyProperty(dpid++, name, propertyType, ownerType, metadata);
            RegisterInternal(dp);
            return dp;
        }

        /// <summary>
        /// Registers a new read-only dependency property.
        /// </summary>
        /// <param name="name">The dependency property's name.</param>
        /// <param name="propertyType">The dependency property's value type.</param>
        /// <param name="ownerType">The dependency property's owner type.</param>
        /// <param name="metadata">The dependency property's metadata.</param>
        /// <returns>A <see cref="DependencyPropertyKey"/> instance which provides access to the read-only dependency property.</returns>
        public static DependencyPropertyKey RegisterReadOnly(String name, Type propertyType, Type ownerType, PropertyMetadata metadata = null)
        {
            Contract.Require(name, "name");
            Contract.Require(propertyType, "propertyType");
            Contract.Require(ownerType, "ownerType");

            var dp = new DependencyProperty(dpid++, name, propertyType, ownerType, metadata, isReadOnly: true);
            RegisterInternal(dp);
            return new DependencyPropertyKey(dp);
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
        /// Registers the specified dependency property.
        /// </summary>
        /// <param name="dp">The dependency property to register.</param>
        private static void RegisterInternal(DependencyProperty dp)
        {
            var propertyDomain = GetPropertyDomain(dp.OwnerType);
            if (propertyDomain.ContainsKey(dp.Name))
            {
                throw new ArgumentException(PresentationStrings.DependencyPropertyAlreadyRegistered);
            }
            propertyDomain[dp.Name] = dp;
        }

        /// <summary>
        /// Gets the dependency property domain associated with the specified owner type.
        /// </summary>
        /// <param name="type">The type for which to retrieve a dependency property domain.</param>
        /// <returns>The dependency property domain associated with the specified owner type.</returns>
        private static Dictionary<String, DependencyProperty> GetPropertyDomain(Type type)
        {
            Dictionary<String, DependencyProperty> propertyDomain;
            if (!dependencyPropertyRegistry.TryGetValue(type, out propertyDomain))
            {
                propertyDomain = new Dictionary<String, DependencyProperty>();
                dependencyPropertyRegistry[type] = propertyDomain;
            }
            return propertyDomain;
        }

        // State values.
        private static readonly Dictionary<Type, Dictionary<String, DependencyProperty>> dependencyPropertyRegistry =
            new Dictionary<Type, Dictionary<String, DependencyProperty>>();
        private static Int64 dpid = 1;
    }
}
