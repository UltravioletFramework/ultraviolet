using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
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
        /// <param name="container">The style's container.</param>
        /// <param name="name">The style's name.</param>
        /// <param name="value">The style's value.</param>
        /// <param name="isImportant">A value indicating whether the style has the !important qualifier.</param>
        internal UvssStyle(UvssStyleArgumentsCollection arguments, String container, String name, String value, Boolean isImportant)
        {
            Contract.Require(arguments, "arguments");

            this.arguments     = arguments;
            this.qualifiedName = GetQualifiedName(arguments, container, name);
            this.container     = container;
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
        /// Gets the style's qualified name, including its container.
        /// </summary>
        public String QualifiedName
        {
            get { return qualifiedName; }
        }

        /// <summary>
        /// Gets the name of the container which defines the attached property that this style represents.
        /// </summary>
        public String Container
        {
            get { return container; }
        }

        /// <summary>
        /// Gets the style's name.
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
        /// Gets the qualified name for a style with the specified parameters.
        /// </summary>
        /// <param name="arguments">The style's list of arguments.</param>
        /// <param name="container">The style's container.</param>
        /// <param name="name">The style's name.</param>
        /// <returns>The qualified name for a style with the specified parameters.</returns>
        private static String GetQualifiedName(UvssStyleArgumentsCollection arguments, String container, String name)
        {
            var part1 = (container == null) ? name : String.Format("{0}.{1}", container, name);
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
