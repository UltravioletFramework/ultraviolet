using System;
using System.Collections.Generic;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents a variant form of a localized string.
    /// </summary>
    public class LocalizedStringVariant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedStringVariant"/> class.
        /// </summary>
        /// <param name="parent">The string variant's parent string.</param>
        /// <param name="group">The name of the string variant's variant group.</param>
        /// <param name="value">The value of the string variant.</param>
        /// <param name="properties">A collection of properties to attach to the string variant.</param>
        internal LocalizedStringVariant(LocalizedString parent, String group, String value, IEnumerable<String> properties = null)
        {
            Contract.Require(parent, nameof(parent));
            Contract.Require(group, nameof(group));
            Contract.Require(value, nameof(value));

            this.parent = parent;
            this.group = group;
            this.value = value;
            this.properties.Add(group);

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    this.properties.Add(property);
                }
            }
        }

        /// <summary>
        /// Implicitly converts the object to a string.
        /// </summary>
        /// <param name="variant">The object to convert.</param>
        /// <returns>The converted object.</returns>
        public static implicit operator String(LocalizedStringVariant variant)
        {
            return variant.Value;
        }

        /// <inheritdoc/>
        public override String ToString() => value;

        /// <summary>
        /// Gets a value indicating whether the string variant has the specified property.
        /// </summary>
        /// <param name="prop">The name of the property to evaluate.</param>
        /// <returns><see langword="true"/> if the string variant has the specified property; otherwise, <see langword="false"/>.</returns>
        public Boolean HasProperty(String prop)
        {
            return properties.Contains(prop) || parent.HasProperty(prop);
        }
         
        /// <summary>
        /// Gets a value indicating whether the string variant has the specified property.
        /// </summary>
        /// <param name="prop">The name of the property to evaluate.</param>
        /// <returns><see langword="true"/> if the string variant has the specified property; otherwise, <see langword="false"/>.</returns>
        public Boolean HasProperty(StringSegment prop)
        {
            foreach (var property in properties)
            {
                if (prop.Equals(property))
                    return true;
            }
            return parent.HasPropertyRef(ref prop);
        }

        /// <summary>
        /// Gets a value indicating whether the string variant has the specified property.
        /// </summary>
        /// <param name="prop">The name of the property to evaluate.</param>
        /// <returns><see langword="true"/> if the string variant has the specified property; otherwise, <see langword="false"/>.</returns>
        public Boolean HasPropertyRef(ref StringSegment prop)
        {
            foreach (var property in properties)
            {
                if (prop.Equals(property))
                    return true;
            }
            return parent.HasPropertyRef(ref prop);
        }

        /// <summary>
        /// Gets the string that owns this variant.
        /// </summary>
        public LocalizedString Parent
        {
            get { return parent; }
        }

        /// <summary>
        /// Gets the string variant's localization key.
        /// </summary>
        public String Key
        {
            get { return parent.Key; }
        }

        /// <summary>
        /// Gets the string variant's associated culture.
        /// </summary>
        public String Culture
        {
            get { return parent.Culture; }
        }

        /// <summary>
        /// Gets the name of the string variant's variant group.
        /// </summary>
        public String Group
        {
            get { return group; }
        }

        /// <summary>
        /// Gets the value of the string variant.
        /// </summary>
        public String Value
        {
            get { return value; }
        }

        /// <summary>
        /// Gets the string variant's collection of properties.
        /// </summary>
        public HashSet<String> Properties
        {
            get { return properties; }
        }

        // Property values.
        private readonly LocalizedString parent;
        private readonly String group;
        private readonly String value;
        private readonly HashSet<String> properties = new HashSet<String>();
    }
}
