using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a key identifying and providing write access to a particular read-only dependency property.
    /// </summary>
    public sealed class DependencyPropertyKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPropertyKey"/> class.
        /// </summary>
        /// <param name="dependencyProperty">The <see cref="DependencyProperty"/> instance to which this key provides access.</param>
        internal DependencyPropertyKey(DependencyProperty dependencyProperty)
        {
            Contract.Require(dependencyProperty, nameof(dependencyProperty));

            this.dependencyProperty = dependencyProperty;
        }

        /// <summary>
        /// Gets the dependency property to which this key provides access.
        /// </summary>
        public DependencyProperty DependencyProperty
        {
            get { return dependencyProperty; }
        }

        // Property values.
        private readonly DependencyProperty dependencyProperty;
    }
}
