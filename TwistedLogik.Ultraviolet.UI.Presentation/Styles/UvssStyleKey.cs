using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents the key used to identify a combination of a style name and a pseudo-class.
    /// </summary>
    internal struct UvssStyleKey : IEquatable<UvssStyleKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStyleKey"/> structure.
        /// </summary>
        /// <param name="style">The key's style.</param>
        /// <param name="pseudoClass">The key's pseudoclass.</param>
        public UvssStyleKey(String style, String pseudoClass)
        {
            Contract.Require(style, "style");

            this.style       = style;
            this.pseudoClass = pseudoClass;
        }

        /// <summary>
        /// Returns <c>true</c> if the specified style keys are equal.
        /// </summary>
        /// <param name="id1">The first <see cref="UvssStyleKey"/> to compare.</param>
        /// <param name="id2">The second <see cref="UvssStyleKey"/> to compare.</param>
        /// <returns><c>true</c> if the specified style keys are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(UvssStyleKey id1, UvssStyleKey id2)
        {
            return id1.Equals(id2);
        }

        /// <summary>
        /// Returns <c>true</c> if the specified style keys are not equal.
        /// </summary>
        /// <param name="id1">The first <see cref="UvssStyleKey"/> to compare.</param>
        /// <param name="id2">The second <see cref="UvssStyleKey"/> to compare.</param>
        /// <returns><c>true</c> if the specified style keys are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(UvssStyleKey id1, UvssStyleKey id2)
        {
            return !id1.Equals(id2);
        }

        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + style.GetHashCode();
                hash = hash * 23 + (pseudoClass == null ? 0 : pseudoClass.GetHashCode());
                return hash;
            }
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            if (String.IsNullOrEmpty(pseudoClass))
            {
                return "{" + style + "}";
            }
            return String.Format("{{{0}, {1}}}", style, pseudoClass);
        }

        /// <inheritdoc/>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is UvssStyleKey)) return false;
            return Equals((UvssStyleKey)obj);
        }

        /// <inheritdoc/>
        public Boolean Equals(UvssStyleKey other)
        {
            return this.style == other.style && this.pseudoClass == other.pseudoClass;
        }

        /// <summary>
        /// Gets the key's style.
        /// </summary>
        public String Style
        {
            get { return style; }
        }

        /// <summary>
        /// Gets the key's pseudo-class.
        /// </summary>
        public String PseudoClass
        {
            get { return pseudoClass; }
        }

        // Property values.
        private readonly String style;
        private readonly String pseudoClass;
    }
}
