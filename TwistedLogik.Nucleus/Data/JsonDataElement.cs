using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a data definition element based on JSON.
    /// </summary>
    internal class JsonDataElement : DataElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataElement"/> class.
        /// </summary>
        /// <param name="parent">The element's parent element.</param>
        /// <param name="name">The element's name.</param>
        /// <param name="jtoken">The JSON token from which to create the data element.</param>
        public JsonDataElement(JsonDataElement parent, String name, JToken jtoken)
            : base(parent)
        {
            Contract.Require(name, "name");
            Contract.Require(jtoken, "jtoken");

            this.name = name;
            this.jtoken = jtoken;
        }

        /// <summary>
        /// Gets the value of the attribute with the specified name.
        /// </summary>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <returns>The value of the specified attribute, or <c>null</c> if the attribute does not exist.</returns>
        public override DataAttribute Attribute(String name)
        {
            var token = jtoken[name];

            if (token == null || !IsPrimitiveType(token))
                return null;

            return new JsonDataAttribute(name, token.Value<String>());
        }

        /// <summary>
        /// Gets the values of the element's attributes.
        /// </summary>
        /// <returns>The values of the element's attributes.</returns>
        public override IEnumerable<DataAttribute> Attributes()
        {
            return Attributes(null);
        }

        /// <summary>
        /// Gets the values of the element's attributes which match the specified name.
        /// </summary>
        /// <param name="name">The name of the attributes to retrieve.</param>
        /// <returns>The values of the element's attributes.</returns>
        public override IEnumerable<DataAttribute> Attributes(String name)
        {
            if (!jtoken.HasValues)
                return Enumerable.Empty<DataAttribute>();

            var enumerable = (IEnumerable<KeyValuePair<String, JToken>>)jtoken;
            return from kvp in enumerable
                   where (name == null || kvp.Key == name) && IsPrimitiveType(kvp.Value)
                   select new JsonDataAttribute(kvp.Key, kvp.Value.Value<String>());
        }

        /// <summary>
        /// Gets the child element with the specified name.
        /// </summary>
        /// <param name="name">The name of the child element to retrieve.</param>
        /// <returns>The child element with the specified name, or <c>null</c> if the element does not exist.</returns>
        public override DataElement Element(String name)
        {
            return Elements(name).SingleOrDefault();
        }

        /// <summary>
        /// Gets the child elements of this element.
        /// </summary>
        /// <returns>The element's child elements.</returns>
        public override IEnumerable<DataElement> Elements()
        {
            return Elements(null);
        }

        /// <summary>
        /// Gets the child elements of this element which match the specified name.
        /// </summary>
        /// <param name="name">The name of the elements to retrieve.</param>
        /// <returns>The element's child elements.</returns>
        public override IEnumerable<DataElement> Elements(String name)
        {
            if (!jtoken.HasValues)
                return Enumerable.Empty<DataElement>();

            var elements = new List<DataElement>();
            foreach (var kvp in (IEnumerable<KeyValuePair<String, JToken>>)jtoken)
            {
                if (name != null && name != kvp.Key)
                    continue;

                var token = kvp.Value;
                if (token.Type == JTokenType.Array)
                {
                    elements.AddRange(token.ToArray().Select(x => new JsonDataElement(this, kvp.Key, x)));
                }
                else
                {
                    elements.Add(new JsonDataElement(this, kvp.Key, token));
                }
            }
            return elements;
        }

        /// <summary>
        /// Gets the element's name.
        /// </summary>
        public override String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the element's value as a string.
        /// </summary>
        public override String Value
        {
            get 
            {
                if (jtoken.Type == JTokenType.Object)
                {
                    var value = jtoken["Value"];
                    if (value != null && IsPrimitiveType(value))
                        return value.Value<String>();
                }
                return jtoken.Value<String>(); 
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified token has a primitive type.
        /// </summary>
        private static Boolean IsPrimitiveType(JToken token)
        {
            return
                token.Type == JTokenType.String ||
                token.Type == JTokenType.Boolean ||
                token.Type == JTokenType.Float ||
                token.Type == JTokenType.Integer;
        }

        // The underlying JSON token.
        private readonly String name;
        private readonly JToken jtoken;
    }
}
