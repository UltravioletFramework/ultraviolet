using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents one of the styles defined by an Ultraviolet Stylesheet (UVSS) socument.
    /// </summary>
    public sealed class UvssStyle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStyle"/> class.
        /// </summary>
        /// <param name="arguments">The style's argument list.</param>
        /// <param name="ownerType">The name of the owner of type of the attached property that this style modifies, if
        /// it modifies an attached property.</param>
        /// <param name="name">The name of the dependency property that this style modifies.</param>
        /// <param name="value">The style's value.</param>
        /// <param name="isImportant">A value indicating whether the style has the !important qualifier.</param>
        internal UvssStyle(UvssStyleArgumentsCollection arguments, String ownerType, String name, String value, Boolean isImportant)
        {
            Contract.Require(arguments, "arguments");

            this.arguments     = arguments;
            this.qualifiedName = GetCanonicalName(arguments, ownerType, name);
            this.container     = ownerType;
            this.name          = name;
            this.value         = value;
            this.isImportant   = isImportant;
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return arguments.Count > 0 ?
                String.Format("{0} ({1}): {2}{3}", qualifiedName, arguments, value, isImportant ? " !important" : "") :                
                String.Format("{0}: {1}{2}", qualifiedName, value, isImportant ? " !important" : "");
        }

        /// <summary>
        /// Gets the style's collection of arguments.
        /// </summary>
        public UvssStyleArgumentsCollection Arguments
        {
            get { return arguments; }
        }

        /// <summary>
        /// Gets the canonical name that uniquely identifies this style.
        /// </summary>
        public String CanonicalName
        {
            get { return qualifiedName; }
        }

        /// <summary>
        /// Gets the name of the owner type of the attached property that this style modifies, if
        /// it modifies an attached property.
        /// </summary>
        public String OwnerType
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
        private static String GetCanonicalName(UvssStyleArgumentsCollection arguments, String ownerType, String name)
        {
            var part1 = (ownerType == null) ? name : String.Format("{0}.{1}", ownerType, name);
            var part2 = (arguments.Count > 0) ? String.Format(" ({0})", arguments) : null;
            return part1 + part2;
        }

        // Property values.
        private readonly UvssStyleArgumentsCollection arguments;
        private readonly String qualifiedName;
        private readonly String container;
        private readonly String name;
        private readonly String value;
        private readonly Boolean isImportant;
    }
}
