using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a data definition element based on XML.
    /// </summary>
    internal class XmlDataElement : DataElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataElement"/> class.
        /// </summary>
        /// <param name="parent">The element's parent element.</param>
        /// <param name="element">The XML element from which to create the data element.</param>
        public XmlDataElement(XmlDataElement parent, XElement element)
            : base(parent)
        {
            Contract.Require(element, "element");

            this.element = element;
        }

        /// <summary>
        /// Gets the value of the attribute with the specified name.
        /// </summary>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <returns>The value of the specified attribute, or <c>null</c> if the attribute does not exist.</returns>
        public override DataAttribute Attribute(String name)
        {
            var attr = element.Attribute(name);
            return (attr == null) ? null : new XmlDataAttribute(attr);
        }

        /// <summary>
        /// Gets the values of the element's attributes.
        /// </summary>
        /// <returns>The values of the element's attributes.</returns>
        public override IEnumerable<DataAttribute> Attributes()
        {
            return element.Attributes().Select(x => new XmlDataAttribute(x));
        }

        /// <summary>
        /// Gets the values of the element's attributes which match the specified name.
        /// </summary>
        /// <param name="name">The name of the attributes to retrieve.</param>
        /// <returns>The values of the element's attributes.</returns>
        public override IEnumerable<DataAttribute> Attributes(String name)
        {
            return element.Attributes(name).Select(x => new XmlDataAttribute(x));
        }

        /// <summary>
        /// Gets the child element with the specified name.
        /// </summary>
        /// <param name="name">The name of the child element to retrieve.</param>
        /// <returns>The child element with the specified name, or <c>null</c> if the element does not exist.</returns>
        public override DataElement Element(String name)
        {
            var child = element.Element(name);
            return (child == null) ? null : new XmlDataElement(this, child);
        }

        /// <summary>
        /// Gets the child elements of this element.
        /// </summary>
        /// <returns>The element's child elements.</returns>
        public override IEnumerable<DataElement> Elements()
        {
            return element.Elements().Select(x => new XmlDataElement(this, x));
        }

        /// <summary>
        /// Gets the child elements of this element which match the specified name.
        /// </summary>
        /// <param name="name">The name of the elements to retrieve.</param>
        /// <returns>The element's child elements.</returns>
        public override IEnumerable<DataElement> Elements(String name)
        {
            return element.Elements(name).Select(x => new XmlDataElement(this, x));
        }

        /// <summary>
        /// Gets the element's name.
        /// </summary>
        public override String Name
        {
            get { return element.Name.LocalName; }
        }

        /// <summary>
        /// Gets the element's value as a string.
        /// </summary>
        public override String Value
        {
            get { return element.Value; }
        }

        // The underlying XML element.
        private readonly XElement element;
    }
}
