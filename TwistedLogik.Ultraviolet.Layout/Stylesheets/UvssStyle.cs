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
        /// <param name="name">The style's name.</param>
        /// <param name="value">The style's value.</param>
        internal UvssStyle(String name, String value)
        {
            this.name  = name;
            this.value = value;
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return String.Format("{0}: {1}", name, value);
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

        // Property values.
        private readonly String name;
        private readonly String value;
    }
}
