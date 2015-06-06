using System;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media
{
    /// <summary>
    /// Represents a two-dimensional transformation.
    /// </summary>
    public abstract class Transform : DependencyObject
    {
        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static Transform Identity
        {
            get { return identity; }
        }

        /// <summary>
        /// Gets the <see cref="Matrix"/> that represents this transformation.
        /// </summary>
        /// <returns>A <see cref="Matrix"/> that represents the transformation.</returns>
        public abstract Matrix GetValue();

        /// <summary>
        /// Gets the <see cref="Matrix"/> that represents this transformation when applied 
        /// in the screen space of the specified display.
        /// </summary>
        /// <param name="display">The display in which the transformation will be applied.</param>
        /// <returns>A <see cref="Matrix"/> that represents the transformation.</returns>
        public abstract Matrix GetValueForDisplay(IUltravioletDisplay display);

        /// <inheritdoc/>>
        internal sealed override Object DependencyDataSource
        {
            get { return null; }
        }

        /// <inheritdoc/>
        internal sealed override DependencyObject DependencyContainer
        {
            get { return null; }
        }

        /// Property values.
        private static readonly Transform identity = new IdentityTransform();
    }
}
