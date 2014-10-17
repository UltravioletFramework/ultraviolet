using System;
using System.ComponentModel;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a resolved reference to a Nucleus Data Object.
    /// </summary>
    [TypeConverter(typeof(ObjectResolverTypeConverter<ResolvedDataObjectReference>))]
    public struct ResolvedDataObjectReference : IComparable<ResolvedDataObjectReference>, IEquatable<ResolvedDataObjectReference>
    {
        /// <summary>
        /// Initializes a new instance of the ResolvedDataObjectReference structure.
        /// </summary>
        /// <param name="value">The data object identifier's value.</param>
        public ResolvedDataObjectReference(Guid value)
        {
            this.Value = value;
            this.Source = null;
        }

        /// <summary>
        /// Initializes a new instance of the ResolvedDataObjectReference structure.
        /// </summary>
        /// <param name="value">The data object identifier's value.</param>
        /// <param name="source">A string identifying the source of the identifier, used for debugging.</param>
        public ResolvedDataObjectReference(Guid value, String source)
        {
            this.Value = value;
            this.Source = source;
        }

        /// <summary>
        /// Explicitly converts an ResolvedDataObjectReference structure to its underlying globally-unique value.
        /// </summary>
        public static explicit operator Guid(ResolvedDataObjectReference id)
        {
            return id.Value;
        }

        /// <summary>
        /// Explicitly converts an ResolvedDataObjectReference structure to its underlying globally-unique value.
        /// </summary>
        public static explicit operator Guid?(ResolvedDataObjectReference id)
        {
            return id.Value;
        }

        /// <summary>
        /// Explicitly converts an integer to an ResolvedDataObjectReference structure.
        /// </summary>
        public static explicit operator ResolvedDataObjectReference(Guid id)
        {
            return new ResolvedDataObjectReference(id);
        }

        /// <summary>
        /// Evaluates two data object identifiers for equality.
        /// </summary>
        public static bool operator ==(ResolvedDataObjectReference m1, ResolvedDataObjectReference m2)
        {
            return m1.Value == m2.Value;
        }

        /// <summary>
        /// Evaluates two data object identifiers for inequality.
        /// </summary>
        public static bool operator !=(ResolvedDataObjectReference m1, ResolvedDataObjectReference m2)
        {
            return m1.Value != m2.Value;
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
        public override string ToString()
        {
            return Value.Equals(Guid.Empty) ? "(none)" : Source ?? String.Format("Object #{0}", Value.ToString());
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for the current ResolvedDataObjectReference structure.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ResolvedDataObjectReference))
                return false;
            return Equals((ResolvedDataObjectReference)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(ResolvedDataObjectReference other)
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
        public int CompareTo(ResolvedDataObjectReference other)
        {
            return this.Value.CompareTo(other.Value);
        }

        /// <summary>
        /// The data object identifier's underlying value.
        /// </summary>
        public readonly Guid Value;

        /// <summary>
        /// A string identifying the source of the identifier, used for debugging.
        /// </summary>
        public readonly String Source;
    }
}
