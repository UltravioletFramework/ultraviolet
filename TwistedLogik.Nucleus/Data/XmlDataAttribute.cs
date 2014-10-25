using System;
using System.Xml.Linq;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a data definition attribute based on XML.
    /// </summary>
    internal class XmlDataAttribute : DataAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataAttribute"/> class.
        /// </summary>
        /// <param name="attribute">The underlying XML attribute.</param>
        public XmlDataAttribute(XAttribute attribute)
        {
            Contract.Require(attribute, "attribute");

            this.attribute = attribute;
        }

        /// <summary>
        /// Gets the attribute's name.
        /// </summary>
        public override String Name
        {
            get { return attribute.Name.LocalName; }
        }

        /// <summary>
        /// Gets the attribute's value.
        /// </summary>
        public override String Value
        {
            get { return attribute.Value; }
        }

        // The underlying XML attribute.
        private readonly XAttribute attribute;
    }
}
