using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a UVML placeholder attribute.
    /// </summary>
    /// <remarks>Placeholder attributes are used to associate placeholder values in UVML (such as "ItemsPanel") with concrete types (such as "StackPanel").</remarks> 
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class UvmlPlaceholderAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlPlaceholderAttribute"/> class.
        /// </summary>
        /// <param name="placeholder">The placeholder text.</param>
        /// <param name="type">The type which is substituted for the placeholder.</param>
        public UvmlPlaceholderAttribute(String placeholder, Type type)
        {
            Contract.RequireNotEmpty(placeholder, nameof(placeholder));
            Contract.Require(type, nameof(type));

            this.placeholder = placeholder;
            this.type = type;
        }

        /// <summary>
        /// Gets the placeholder text.
        /// </summary>
        public String Placeholder
        {
            get { return placeholder; }
        }

        /// <summary>
        /// Gets the type which is substituted for the placeholder.
        /// </summary>
        public Type Type
        {
            get { return type; }
        }

        // Property values.
        private readonly String placeholder;
        private readonly Type type;
    }
}
