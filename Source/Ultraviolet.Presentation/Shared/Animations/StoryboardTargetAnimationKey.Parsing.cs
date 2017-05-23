using System;

namespace Ultraviolet.Presentation.Animations
{
    partial struct StoryboardTargetAnimationKey
    {
        /// <inheritdoc/>
        public override String ToString()
        {
            return ToString(null);
        }

        /// <summary>
        /// Converts the object to a human-readable string using the specified culture information.
        /// </summary>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A human-readable string that represents the object.</returns>
        public String ToString(IFormatProvider provider)
        {
            return String.Format(provider, navigationExpression.HasValue ? "{0} | {1}" : "{0}",
                propertyName.QualifiedName, navigationExpression);
        }
    }
}
