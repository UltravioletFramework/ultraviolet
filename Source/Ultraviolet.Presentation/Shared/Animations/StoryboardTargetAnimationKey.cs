using System;
using Ultraviolet.Presentation.Styles;

namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents the key used to identify a particular animation within a storyboard target's collection of animations.
    /// </summary>
    public partial struct StoryboardTargetAnimationKey : IEquatable<StoryboardTargetAnimationKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardTargetAnimationKey"/> structure.
        /// </summary>
        /// <param name="propertyName">The name of the animated property.</param>
        /// <param name="navigationExpression">The navigation expression for the animated property, if one was specified.</param>
        public StoryboardTargetAnimationKey(DependencyName propertyName, NavigationExpression? navigationExpression = null)
        {
            this.propertyName = propertyName;
            this.navigationExpression = navigationExpression;
        }

        /// <inheritdoc/>
        public override String ToString() => navigationExpression.HasValue ?
            $"{propertyName.QualifiedName} | {navigationExpression}" : propertyName.QualifiedName;

        /// <summary>
        /// Gets the name of the animated property.
        /// </summary>
        public DependencyName PropertyName
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
        private readonly DependencyName propertyName;
        private readonly NavigationExpression? navigationExpression;
    }
}
