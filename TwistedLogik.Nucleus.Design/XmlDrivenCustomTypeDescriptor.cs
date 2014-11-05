using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TwistedLogik.Nucleus.Design
{
    /// <summary>
    /// Represents a custom type descriptor which reads type metadata from an XML file.
    /// </summary>
    internal sealed class XmlDrivenCustomTypeDescriptor : CustomTypeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDrivenCustomTypeDescriptor"/> class.
        /// </summary>
        /// <param name="parent">The parent type descriptor.</param>
        /// <param name="propertyDescriptors">The property descriptors for this type.</param>
        internal XmlDrivenCustomTypeDescriptor(ICustomTypeDescriptor parent, IEnumerable<PropertyDescriptor> propertyDescriptors)
            : base(parent)
        {
            if (propertyDescriptors != null)
                this.propertyDescriptors.AddRange(propertyDescriptors);
        }

        /// <summary>
        /// Returns a collection of property descriptors for the object represented by this type descriptor.
        /// </summary>
        /// <returns>A <see cref="PropertyDescriptorCollection"/> containing the property descriptions for the object represented by this type descriptor.</returns>
        public override PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }

        /// <summary>
        /// Returns a filtered collection of property descriptors for the object represented by this type descriptor.
        /// </summary>
        /// <param name="attributes">An array of attributes to use as a filter. This can be <c>null</c>.</param>
        /// <returns>A <see cref="PropertyDescriptorCollection"/> containing the property descriptions for the object represented by this type descriptor.</returns>
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var parent = base.GetProperties(attributes);
            var properties = new Dictionary<String, PropertyDescriptor>();
            foreach (var p in parent)
            {
                var pd = (PropertyDescriptor)p;
                properties[pd.Name] = pd;
            }
            foreach (var pd in propertyDescriptors)
            {
                properties[pd.Name] = pd;
            }
            return new PropertyDescriptorCollection(properties.Values.Where(x => !ExcludedByFilter(x, attributes)).ToArray());
        }

        /// <summary>
        /// Gets a value indicating whether the specified member is excluded by the specified attribute filter.
        /// </summary>
        /// <param name="descriptor">The descriptor to evaluate.</param>
        /// <param name="attributes">The attribute filter.</param>
        /// <returns><c>true</c> if the member is excluded; otherwise, <c>false</c>.</returns>
        private static Boolean ExcludedByFilter(MemberDescriptor descriptor, Attribute[] attributes)
        {
            if (attributes == null)
                return false;

            for (int i = 0; i < attributes.Length; i++)
            {
                var attributeInFilter = attributes[i];
                var attributeOnMember = descriptor.Attributes[attributeInFilter.GetType()];

                if (attributeOnMember == null && !attributeInFilter.IsDefaultAttribute())
                    return true;

                if (!attributeInFilter.Match(attributeOnMember))
                    return true;
            }
            return false;
        }

        // State values.
        private readonly List<PropertyDescriptor> propertyDescriptors = new List<PropertyDescriptor>();
    }
}
