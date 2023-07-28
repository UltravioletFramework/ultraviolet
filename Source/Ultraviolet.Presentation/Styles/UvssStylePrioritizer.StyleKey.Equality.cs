using System;

namespace Ultraviolet.Presentation.Styles
{
    partial class UvssStylePrioritizer
    {
        partial struct StyleKey
        {
            /// <inheritdoc/>
            public override Int32 GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + name?.GetHashCode() ?? 0;
                    hash = hash * 23 + navigationExpression?.GetHashCode() ?? 0;
                    return hash;
                }
            }

            /// <summary>
            /// Compares two objects to determine whether they are equal.
            /// </summary>
            /// <param name="v1">The first value to compare.</param>
            /// <param name="v2">The second value to compare.</param>
            /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
            public static Boolean operator ==(StyleKey v1, StyleKey v2)
            {
                return v1.Equals(v2);
            }

            /// <summary>
            /// Compares two objects to determine whether they are unequal.
            /// </summary>
            /// <param name="v1">The first value to compare.</param>
            /// <param name="v2">The second value to compare.</param>
            /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
            public static Boolean operator !=(StyleKey v1, StyleKey v2)
            {
                return !v1.Equals(v2);
            }

            /// <inheritdoc/>
            public override Boolean Equals(Object other)
            {
                return (other is StyleKey x) ? Equals(x) : false;
            }

            /// <inheritdoc/>
            public Boolean Equals(StyleKey other)
            {
                return
                    name == other.name &&
                    navigationExpression.HasValue == other.navigationExpression.HasValue &&
                    navigationExpression.GetValueOrDefault().Equals(other.navigationExpression.GetValueOrDefault());
            }
        }
    }
}