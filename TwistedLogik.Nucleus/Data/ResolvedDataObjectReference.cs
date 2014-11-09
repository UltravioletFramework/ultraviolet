using System;
using System.ComponentModel;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a resolved reference to a Nucleus Data Object.
    /// </summary>
    /// <remarks>A resolved reference is one in which the string value of the reference has been associated with
    /// the globally-unique identifier of the referenced object.</remarks>
    [TypeConverter(typeof(ObjectResolverTypeConverter<ResolvedDataObjectReference>))]
    public struct ResolvedDataObjectReference : IComparable<ResolvedDataObjectReference>, IEquatable<ResolvedDataObjectReference>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResolvedDataObjectReference"/> structure.
        /// </summary>
        /// <param name="value">The data object identifier's value.</param>
        public ResolvedDataObjectReference(Guid value)
        {
            this.value = value;
            this.source = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolvedDataObjectReference"/> structure.
        /// </summary>
        /// <param name="value">The data object identifier's value.</param>
        /// <param name="source">A string identifying the source of the identifier, used for debugging.</param>
        public ResolvedDataObjectReference(Guid value, String source)
        {
            this.value = value;
            this.source = source;
        }

        /// <summary>
        /// Explicitly converts an <see cref="ResolvedDataObjectReference"/> structure to its underlying globally-unique value.
        /// </summary>
        /// <param name="reference">The <see cref="ResolvedDataObjectReference"/> to convert.</param>
        /// <returns>The converted <see cref="Guid"/>.</returns>
        public static explicit operator Guid(ResolvedDataObjectReference reference)
        {
            return reference.Value;
        }

        /// <summary>
        /// Explicitly converts an <see cref="ResolvedDataObjectReference"/> structure to its underlying globally-unique value.
        /// </summary>
        /// <param name="reference">The <see cref="ResolvedDataObjectReference"/> to convert.</param>
        /// <returns>The converted <see cref="Nullable{Guid}"/>.</returns>
        public static explicit operator Guid?(ResolvedDataObjectReference reference)
        {
            return reference.Value;
        }

        /// <summary>
        /// Explicitly converts an integer to an <see cref="ResolvedDataObjectReference"/> structure.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/> to convert.</param>
        /// <returns>The converted <see cref="ResolvedDataObjectReference"/>.</returns>
        public static explicit operator ResolvedDataObjectReference(Guid id)
        {
            return new ResolvedDataObjectReference(id);
        }

        /// <summary>
        /// Evaluates two data object identifiers for equality.
        /// </summary>
        /// <param name="ref1">The first <see cref="ResolvedDataObjectReference"/> to compare.</param>
        /// <param name="ref2">The second <see cref="ResolvedDataObjectReference"/> to compare.</param>
        /// <returns><c>true</c> if the specified references are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(ResolvedDataObjectReference ref1, ResolvedDataObjectReference ref2)
        {
            return ref1.Value == ref2.Value;
        }

        /// <summary>
        /// Evaluates two data object identifiers for inequality.
        /// </summary>
        /// <param name="ref1">The first <see cref="ResolvedDataObjectReference"/> to compare.</param>
        /// <param name="ref2">The second <see cref="ResolvedDataObjectReference"/> to compare.</param>
        /// <returns><c>true</c> if the specified references are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(ResolvedDataObjectReference ref1, ResolvedDataObjectReference ref2)
        {
            return ref1.Value != ref2.Value;
        }

        /// <summary>
        /// Gets the name of the registry that contains the resolved object.
        /// </summary>
        /// <returns>The name of the registry that contains the resolved object.</returns>
        public String GetObjectRegistry()
        {
            if (Source != null)
            {
                var ix = Source.LastIndexOf(":");
                if (ix >= 0)
                {
                    return Source.Substring(1, ix);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the name of the resolved data object.
        /// </summary>
        /// <returns>The name of the resolved data object.</returns>
        public String GetObjectName()
        {
            if (Source != null)
            {
                var ix = Source.LastIndexOf(":");
                if (ix >= 0)
                {
                    return Source.Substring(ix + 1);
                }
            }
            return Value.ToString();
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return !IsValid ? "@INVALID" : Source ?? String.Format("Object #{0}", Value.ToString());
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for the current <see cref="ResolvedDataObjectReference"/> structure.</returns>
        public override Int32 GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object other)
        {
            if (!(other is ResolvedDataObjectReference))
                return false;
            return Equals((ResolvedDataObjectReference)other);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public Boolean Equals(ResolvedDataObjectReference other)
        {
            return this.Value == other.Value;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that 
        /// indicates whether the current instance precedes, follows, or occurs in the same position in 
        /// the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public Int32 CompareTo(ResolvedDataObjectReference other)
        {
            return this.Value.CompareTo(other.Value);
        }

        /// <summary>
        /// Gets an invalid data object reference.
        /// </summary>
        public static ResolvedDataObjectReference Invalid
        {
            get { return new ResolvedDataObjectReference(); }
        }

        /// <summary>
        /// Gets a value indicating whether this is a valid object reference.
        /// </summary>
        public Boolean IsValid
        {
            get { return Value != Guid.Empty; }
        }

        /// <summary>
        /// The data object identifier's underlying value.
        /// </summary>
        public Guid Value
        {
            get { return value; } 
        }

        /// <summary>
        /// A string identifying the source of the identifier, used for debugging.
        /// </summary>
        public String Source
        {
            get { return source; }
        }

        // Property values.
        private Guid value;
        private String source;
    }
}
