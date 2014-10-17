using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a hierarchical data definition element which can be queried by the Nucleus Object Loader.
    /// </summary>
    public abstract class DataElement
    {
        /// <summary>
        /// Initializes a new instance of the DataElement class.
        /// </summary>
        /// <param name="parent">The parent element.</param>
        public DataElement(DataElement parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Creates a data element from the specified file.
        /// </summary>
        /// <param name="file">The file from which to create a data element.</param>
        /// <returns>The data element that was created.</returns>
        public static DataElement CreateFromFile(String file)
        {
            Contract.Require(file, "file");

            var extension = Path.GetExtension(file).ToUpper();
            switch (extension)
            {
                case ".XML":
                    return CreateFromXml(XDocument.Load(file));

                case ".JSON":
                case ".JS":
                    using (var sreader = new StreamReader(file))
                    using (var jreader = new Newtonsoft.Json.JsonTextReader(sreader))
                    {
                        return CreateFromJson(JObject.Load(jreader));
                    }
            }

            throw new InvalidOperationException(NucleusStrings.UnrecognizedDataType);
        }

        /// <summary>
        /// Creates a data element from the specified JSON object.
        /// </summary>
        /// <param name="jobj">The JSON object from which to create an element.</param>
        /// <returns>The data element that was created.</returns>
        public static DataElement CreateFromJson(JObject jobj)
        {
            Contract.Require(jobj, "jobj");

            return CreateFromJson(jobj.Root);
        }

        /// <summary>
        /// Creates a data element from the specified JSON token.
        /// </summary>
        /// <param name="jtoken">The JSON token from which to create an element.</param>
        /// <returns>The data element that was created.</returns>
        public static DataElement CreateFromJson(JToken jtoken)
        {
            Contract.Require(jtoken, "jtoken");

            return new JsonDataElement(null, String.Empty, jtoken);
        }

        /// <summary>
        /// Creates a data element from the specified XML document.
        /// </summary>
        /// <param name="xml">The XML document from which to create an element.</param>
        /// <returns>The data element that was created.</returns>
        public static DataElement CreateFromXml(XDocument xml)
        {
            Contract.Require(xml, "xml");

            return CreateFromXml(xml.Root);
        }

        /// <summary>
        /// Creates a data element from the specified XML element.
        /// </summary>
        /// <param name="xml">The XML element from which to create an element.</param>
        /// <returns>The data element that was created.</returns>
        public static DataElement CreateFromXml(XElement xml)
        {
            Contract.Require(xml, "xml");

            return new XmlDataElement(null, xml);
        }

        /// <summary>
        /// Gets the value of the attribute with the specified name.
        /// </summary>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <returns>The specified attribute, or null if the attribute does not exist.</returns>
        public abstract DataAttribute Attribute(String name);

        /// <summary>
        /// Gets the values of the element's attributes.
        /// </summary>
        /// <returns>The element's attributes.</returns>
        public abstract IEnumerable<DataAttribute> Attributes();

        /// <summary>
        /// Gets the values of the element's attributes which match the specified name.
        /// </summary>
        /// <param name="name">The name of the attributes to retrieve.</param>
        /// <returns>The element's attributes.</returns>
        public abstract IEnumerable<DataAttribute> Attributes(String name);

        /// <summary>
        /// Gets the value of the attribute with the specified name.
        /// </summary>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <returns>The value of the specified attribute, or null if the attribute does not exist.</returns>
        public T AttributeValue<T>(String name)
        {
            var attr = Attribute(name);
            if (attr == null)
                return default(T);

            return (T)ObjectResolver.FromString(attr.Value, typeof(T));
        }

        /// <summary>
        /// Gets the values of the element's attributes.
        /// </summary>
        /// <returns>The values of the element's attributes.</returns>
        public IEnumerable<T> AttributeValues<T>()
        {
            return Attributes().Select(x => (T)ObjectResolver.FromString(x.Value, typeof(T)));
        }

        /// <summary>
        /// Gets the values of the element's attributes which match the specified name.
        /// </summary>
        /// <param name="name">The name of the attributes to retrieve.</param>
        /// <returns>The values of the element's attributes.</returns>
        public IEnumerable<T> AttributeValues<T>(String name)
        {
            return Attributes(name).Select(x => (T)ObjectResolver.FromString(x.Value, typeof(T)));
        }

        /// <summary>
        /// Gets the child element with the specified name.
        /// </summary>
        /// <param name="name">The name of the child element to retrieve.</param>
        /// <returns>The child element with the specified name, or null if the element does not exist.</returns>
        public abstract DataElement Element(String name);

        /// <summary>
        /// Gets the child elements of this element.
        /// </summary>
        /// <returns>The element's child elements.</returns>
        public abstract IEnumerable<DataElement> Elements();

        /// <summary>
        /// Gets the child elements of this element which match the specified name.
        /// </summary>
        /// <param name="name">The name of the elements to retrieve.</param>
        /// <returns>The element's child elements.</returns>
        public abstract IEnumerable<DataElement> Elements(String name);

        /// <summary>
        /// Gets the element's parent element.
        /// </summary>
        public DataElement Parent
        {
            get { return parent; }
        }

        /// <summary>
        /// Gets the element's name.
        /// </summary>
        public abstract String Name
        {
            get;
        }

        /// <summary>
        /// Gets the element's value as a string.
        /// </summary>
        public abstract String Value
        {
            get;
        }

        // Property values.
        private readonly DataElement parent;
    }
}
