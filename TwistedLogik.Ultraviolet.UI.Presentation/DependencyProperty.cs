using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
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
        /// <param name="isReadOnly">A value indicating whether this is a read-only dependency property.</param>
        internal DependencyProperty(Int64 id, String name, Type propertyType, Type ownerType, PropertyMetadata metadata, Boolean isReadOnly = false)
        {
            this.id              = id;
            this.name            = name;
            this.propertyType    = propertyType;
            this.ownerType       = ownerType;
            this.defaultMetadata = metadata ?? PropertyMetadata.Empty;
            this.isReadOnly      = isReadOnly;
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
            return DependencyPropertySystem.Register(name, propertyType, ownerType, metadata);
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
            return DependencyPropertySystem.RegisterReadOnly(name, propertyType, ownerType, metadata);
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
        /// Adds a new owning type to this dependency property.
        /// </summary>
        /// <param name="ownerType">The owner type to add to this dependency property.</param>
        /// <returns>A reference to this dependency property instance.</returns>
        public DependencyProperty AddOwner(Type ownerType)
        {
            return AddOwner(ownerType, null);
        }

        /// <summary>
        /// Adds a new owning type to this dependency property.
        /// </summary>
        /// <param name="ownerType">The owner type to add to this dependency property.</param>
        /// <param name="typeMetadata">The property metadata for this owning type, which will override the default metadata.</param>
        /// <returns>A reference to this dependency property instance.</returns>
        public DependencyProperty AddOwner(Type ownerType, PropertyMetadata typeMetadata)
        {
            Contract.Require(ownerType, "ownerType");

            DependencyPropertySystem.AddOwner(this, ownerType);

            if (typeMetadata != null)
                OverrideMetadata(ownerType, typeMetadata);

            return this;
        }

        /// <summary>
        /// Overrides the property's metadata for the specified type.
        /// </summary>
        /// <param name="forType">The type for which to override property metadata.</param>
        /// <param name="typeMetadata">The property metadata for the specified type.</param>
        public void OverrideMetadata(Type forType, PropertyMetadata typeMetadata)
        {
            Contract.Require(ownerType, "ownerType");
            Contract.Require(typeMetadata, "typeMetadata");

            if (metadataOverrides.ContainsKey(forType))
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyAlreadyRegistered);

            var merged = false;

            var currentType = forType.BaseType;
            while (currentType != null)
            {
                PropertyMetadata currentTypeMetadata;
                if (metadataOverrides.TryGetValue(currentType, out currentTypeMetadata))
                {
                    typeMetadata.Merge(currentTypeMetadata, this);
                    merged = true;
                    break;
                }
                currentType = currentType.BaseType;
            }

            if (!merged)
            {
                var baseMetadata = GetMetadataForOwner(ownerType);
                typeMetadata.Merge(baseMetadata, this);
                merged = true;
            }

            metadataOverrides[forType] = typeMetadata;
        }

        /// <summary>
        /// Gets the dependency property's metadata for the specified owning type.
        /// </summary>
        /// <param name="type">The owning type for which to retrieve metadata.</param>
        internal PropertyMetadata GetMetadataForOwner(Type type)
        {
            if (metadataOverrides.Count > 0)
            {
                var currentType = type;
                while (currentType != null)
                {
                    PropertyMetadata metadata;
                    if (metadataOverrides.TryGetValue(currentType, out metadata))
                        return metadata;

                    currentType = currentType.BaseType;
                }
            }
            return defaultMetadata;
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
        internal Type PropertyType
        {
            get { return propertyType; }
        }

        /// <summary>
        /// Gets the dependency property's owner type.
        /// </summary>
        internal Type OwnerType
        {
            get { return ownerType; }
        }

        /// <summary>
        /// Gets a value indicating whether this is a read-only dependency property.
        /// </summary>
        internal Boolean IsReadOnly
        {
            get { return isReadOnly; }
        }

        // Property values.
        private readonly Int64 id;
        private readonly String name;
        private readonly Type propertyType;
        private readonly Type ownerType;
        private readonly PropertyMetadata defaultMetadata;
        private readonly Boolean isReadOnly;
        private readonly Dictionary<Type, PropertyMetadata> metadataOverrides = 
            new Dictionary<Type, PropertyMetadata>();
    }
}
