using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an attribute which specifies which type is wrapped by a particular data source wrapper.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class WrappedDataSourceAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrappedDataSourceAttribute"/> class.
        /// </summary>
        /// <param name="wrappedType">The type which is wrapped by this data source wrapper.</param>
        public WrappedDataSourceAttribute(Type wrappedType)
        {
            Contract.Require(wrappedType, nameof(wrappedType));

            this.WrappedType = wrappedType;
        }

        /// <summary>
        /// Gets the type which is wrapped by this data source wrapper.
        /// </summary>
        public Type WrappedType { get; }
    }
}
