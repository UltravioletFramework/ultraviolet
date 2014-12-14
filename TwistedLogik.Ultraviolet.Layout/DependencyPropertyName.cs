using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Layout
{
    /// <summary>
    /// Represents a fully-qualified dependency property name.
    /// </summary>
    public struct DependencyPropertyName : IEquatable<DependencyPropertyName>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPropertyName"/> structure.
        /// </summary>
        /// <param name="name">The dependency property's name.</param>
        public DependencyPropertyName(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            var ixDelimiter = name.IndexOf('.');
            if (ixDelimiter >= 0)
            {
                this.qualifiedName = name;
                this.container     = name.Substring(0, ixDelimiter);
                this.name          = name.Substring(ixDelimiter + 1);
            }
            else
            {
                this.qualifiedName = name;
                this.container     = null;
                this.name          = name;
            }
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return qualifiedName;
        }

        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            return qualifiedName.GetHashCode();
        }

        /// <inheritdoc/>
        public override Boolean Equals(Object obj)
        {
            if (obj is DependencyPropertyName)
            {
                return Equals((DependencyPropertyName)obj);
            }
            return false;
        }

        /// <inheritdoc/>
        public Boolean Equals(DependencyPropertyName obj)
        {
            return this.qualifiedName == obj.qualifiedName;
        }

        /// <summary>
        /// Gets the dependency property's fully-qualified name.
        /// </summary>
        public String QualifiedName
        {
            get { return qualifiedName; }
        }

        /// <summary>
        /// Gets the name of the attached property's container, if this
        /// is an attached property name.
        /// </summary>
        public String Container
        {
            get { return container; }
        }

        /// <summary>
        /// Gets the name of the dependency property without its container.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets a value indicating whether this property name represents an attached property.
        /// </summary>
        public Boolean IsAttachedProperty
        {
            get { return container != null; }
        }

        // Property values.
        private readonly String qualifiedName;
        private readonly String container;
        private readonly String name;
    }
}
