using System;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a data definition attribute based on XML.
    /// </summary>
    internal class JsonDataAttribute : DataAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataAttribute"/> class.
        /// </summary>
        /// <param name="name">The attribute's name.</param>
        /// <param name="value">The attribute's value.</param>
        public JsonDataAttribute(String name, String value)
        {
            Contract.Require(name, "name");

            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// Gets the attribute's name.
        /// </summary>
        public override String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the attribute's value.
        /// </summary>
        public override String Value
        {
            get { return value; }
        }

        // Property values.
        private readonly String name;
        private readonly String value;
    }
}
