using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Layout
{
    /// <summary>
    /// Represents the base class for dependency property value wrappers.
    /// </summary>
    internal abstract class DependencyPropertyValueBase<T> : IDependencyPropertyValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPropertyValueBase"/> class.
        /// </summary>
        /// <param name="owner">The dependency object that owns the property value.</param>
        /// <param name="property">The dependency property which has its value represented by this object.</param>
        public DependencyPropertyValueBase(DependencyObject owner, DependencyProperty property)
        {
            Contract.Require(owner, "owner");
            Contract.Require(property, "property");

            this.owner    = owner;
            this.property = property;
        }

        /// <inheritdoc/>
        public abstract void Digest();

        /// <inheritdoc/>
        public abstract void ClearLocalValue();

        /// <inheritdoc/>
        public DependencyObject Owner
        {
            get { return owner; }
        }

        /// <inheritdoc/>
        public DependencyProperty Property
        {
            get { return property; }
        }

        // Property values.
        private readonly DependencyObject owner;
        private readonly DependencyProperty property;
    }
}
