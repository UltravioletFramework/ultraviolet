using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents the key used to identify a particular animation within a storyboard target's collection of animations.
    /// </summary>
    public struct StoryboardTargetAnimationKey : IEquatable<StoryboardTargetAnimationKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardTargetAnimationKey"/> structure.
        /// </summary>
        /// <param name="propertyName">The name of the animated property.</param>
        /// <param name="navigationExpression">The navigation expression for the animated property, if one was specified.</param>
        public StoryboardTargetAnimationKey(UvmlName propertyName, NavigationExpression? navigationExpression = null)
        {
            this.propertyName = propertyName;
            this.navigationExpression = navigationExpression;
        }

        /// <summary>
        /// Compares two <see cref="StoryboardTargetAnimationKey"/> values for equality.
        /// </summary>
        /// <param name="stk1">The first <see cref="StoryboardTargetAnimationKey"/> to compare.</param>
        /// <param name="stk2">The second <see cref="StoryboardTargetAnimationKey"/> to compare.</param>
        /// <returns><c>true</c> if the specified keys are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(StoryboardTargetAnimationKey stk1, StoryboardTargetAnimationKey stk2)
        {
            return stk1.Equals(stk2);
        }

        /// <summary>
        /// Compares two <see cref="StoryboardTargetAnimationKey"/> values for inequality.
        /// </summary>
        /// <param name="stk1">The first <see cref="StoryboardTargetAnimationKey"/> to compare.</param>
        /// <param name="stk2">The second <see cref="StoryboardTargetAnimationKey"/> to compare.</param>
        /// <returns><c>true</c> if the specified keys are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(StoryboardTargetAnimationKey stk1, StoryboardTargetAnimationKey stk2)
        {
            return !stk1.Equals(stk2);
        }

        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + propertyName.GetHashCode();
                hash = hash * 23 + navigationExpression.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>>
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
                propertyName.QualifiedName,
                navigationExpression);
        }

        /// <inheritdoc/>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is StoryboardTargetAnimationKey))
            {
                return false;
            }
            return Equals((StoryboardTargetAnimationKey)obj);
        }

        /// <inheritdoc/>
        public Boolean Equals(StoryboardTargetAnimationKey other)
        {
            return
                propertyName.Equals(other.propertyName) &&
                navigationExpression.HasValue == other.navigationExpression.HasValue &&
                navigationExpression.GetValueOrDefault().Equals(other.navigationExpression.GetValueOrDefault());
        }

        /// <summary>
        /// Gets the name of the animated property.
        /// </summary>
        public UvmlName PropertyName
        {
            get { return propertyName; }
        }

        /// <summary>
        /// Gets the navigation expression for the animation property, if one was specified.
        /// </summary>
        public NavigationExpression? NavigationExpression
        {
            get { return navigationExpression; }
        }

        // Property values.
        private readonly UvmlName propertyName;
        private readonly NavigationExpression? navigationExpression;
    }
}
