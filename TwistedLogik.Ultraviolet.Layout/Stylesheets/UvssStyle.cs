using System;

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
        /// <param name="container">The style's container.</param>
        /// <param name="name">The style's name.</param>
        /// <param name="value">The style's value.</param>
        /// <param name="isImportant">A value indicating whether the style has the !important qualifier.</param>
        /// <param name="isGlobal">A value indicating whether the style has the !global qualifier.</param>
        internal UvssStyle(String container, String name, String value, Boolean isImportant, Boolean isGlobal)
        {
            this.qualifiedName = (container == null) ? name : String.Format("{0}.{1}", container, name);
            this.container     = container;
            this.name          = name;
            this.value         = value;
            this.isImportant   = isImportant;
            this.isGlobal      = isGlobal;
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return String.Format("{0}: {1}", qualifiedName, value);
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
        /// Gets a value indicating whether the style has the !global qualifier.
        /// </summary>
        public Boolean IsGlobal
        {
            get { return isGlobal; }
        }

        // Property values.
        private readonly String qualifiedName;
        private readonly String container;
        private readonly String name;
        private readonly String value;
        private readonly Boolean isImportant;
        private readonly Boolean isGlobal;
    }
}
