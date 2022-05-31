using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a styling rule defined by an Ultraviolet Style Sheet (UVSS) document.
    /// </summary>
    public sealed class UvssRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssRule"/> class.
        /// </summary>
        /// <param name="arguments">The styling rule's argument list.</param>
        /// <param name="owner">The name of the owner of type of the attached property that this 
        /// styling rule modifies, if it modifies an attached property.</param>
        /// <param name="name">The name of the dependency property that this styling rule modifies.</param>
        /// <param name="value">The styling rule's value.</param>
        /// <param name="isImportant">A value indicating whether the styling rule has the !important qualifier.</param>
        internal UvssRule(UvssRuleArgumentsCollection arguments, String owner, String name, String value, Boolean isImportant)
        {
            Contract.Require(arguments, nameof(arguments));

            this.arguments = arguments;
            this.canonicalName = GetCanonicalName(arguments, owner, name);
            this.container = owner;
            this.name = name;
            this.value = value;
            this.isImportant = isImportant;
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return arguments.Count > 0 ?
                String.Format("{0} ({1}): {2}{3}", canonicalName, arguments, value, isImportant ? " !important" : "") :                
                String.Format("{0}: {1}{2}", canonicalName, value, isImportant ? " !important" : "");
        }

        /// <summary>
        /// Gets the style's collection of arguments.
        /// </summary>
        public UvssRuleArgumentsCollection Arguments
        {
            get { return arguments; }
        }

        /// <summary>
        /// Gets the canonical name that uniquely identifies this style.
        /// </summary>
        public String CanonicalName
        {
            get { return canonicalName; }
        }

        /// <summary>
        /// Gets the name of the owner type of the attached property that this style modifies, if
        /// it modifies an attached property.
        /// </summary>
        public String Owner
        {
            get { return container; }
        }

        /// <summary>
        /// Gets the name of the dependency property that this style modifies.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the style's value.
        /// </summary>
        public String Value
        {
            get { return value; }
        }

        /// <summary>
        /// Gets a value indicating whether the style has the !important qualifier.
        /// </summary>
        public Boolean IsImportant
        {
            get { return isImportant; }
        }

        /// <summary>
        /// Gets or sets the last value that was resolved for this style.
        /// </summary>
        internal Object CachedResolvedValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the canonical name of a style with the specified parameters.
        /// </summary>
        private static String GetCanonicalName(UvssRuleArgumentsCollection arguments, String owner, String name)
        {
            var part1 = (owner == null) ? name : String.Format("{0}.{1}", owner, name);
            var part2 = (arguments.Count > 0) ? String.Format(" ({0})", arguments) : null;
            return part1 + part2;
        }

        // Property values.
        private readonly UvssRuleArgumentsCollection arguments;
        private readonly String canonicalName;
        private readonly String container;
        private readonly String name;
        private readonly String value;
        private readonly Boolean isImportant;
    }
}
