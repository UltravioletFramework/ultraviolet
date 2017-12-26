using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an attribute which manually associates a view model with its wrapper type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal sealed class ViewModelWrapperAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelWrapperAttribute"/> class.
        /// </summary>
        /// <param name="wrapperType">The wrapper type associated with this view model type.</param>
        public ViewModelWrapperAttribute(Type wrapperType)
        {
            Contract.Require(wrapperType, nameof(wrapperType));

            this.WrapperType = wrapperType;
        }

        /// <summary>
        /// Gets the wrapper type associated with this view model type.
        /// </summary>
        public Type WrapperType
        {
            get;
            private set;
        }
    }
}
