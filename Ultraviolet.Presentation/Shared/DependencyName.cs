using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a fully-qualified dependency property or routed event name.
    /// </summary>
    public struct DependencyName : IEquatable<DependencyName>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyName"/> structure.
        /// </summary>
        /// <param name="qualifiedName">The dependency property or routed event's qualified name.</param>
        public DependencyName(String qualifiedName)
        {
            Contract.RequireNotEmpty(qualifiedName, nameof(qualifiedName));

            var ixDelimiter = qualifiedName.IndexOf('.');
            if (ixDelimiter >= 0)
            {
                this.QualifiedName = qualifiedName;
                this.Owner = qualifiedName.Substring(0, ixDelimiter);
                this.Name = qualifiedName.Substring(ixDelimiter + 1);
            }
            else
            {
                this.QualifiedName = qualifiedName;
                this.Owner = null;
                this.Name = qualifiedName;
            }
        }

        /// <inheritdoc/>
        public override String ToString() => QualifiedName;

        /// <inheritdoc/>
        public override Int32 GetHashCode() => QualifiedName.GetHashCode();

        /// <inheritdoc/>
        public override Boolean Equals(Object obj)
        {
            if (obj is DependencyName)
            {
                return Equals((DependencyName)obj);
            }
            return false;
        }

        /// <inheritdoc/>
        public Boolean Equals(DependencyName other)
        {
            return this.QualifiedName == other.QualifiedName;
        }

        /// <summary>
        /// Gets the dependency property or routed event's fully-qualified name.
        /// </summary>
        /// <value>A <see cref="String"/> that contains the fully-qualified name of the dependency property or routed event.</value>
        public String QualifiedName { get; }

        /// <summary>
        /// Gets the name of the type which owns this property or event, if it is an attached property or event.
        /// </summary>
        /// <value>A <see cref="String"/> that contains the name of the attached property or event's owner type.</value>
        public String Owner { get; }

        /// <summary>
        /// Gets the name of the dependency property or routed event without its owner type.
        /// </summary>
        /// <value>A <see cref="String"/> that contains the unqualified name of the property or event.</value>
        public String Name { get; }

        /// <summary>
        /// Gets a value indicating whether this property name represents an attached property or event.
        /// </summary>
        public Boolean IsAttached
        {
            get { return Owner != null; }
        }
    }
}
