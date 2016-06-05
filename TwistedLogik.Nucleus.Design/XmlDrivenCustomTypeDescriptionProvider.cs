using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TwistedLogik.Nucleus.Design
{
    /// <summary>
    /// Represents a <see cref="TypeDescriptionProvider"/> which creates instances of <see cref="XmlDrivenCustomTypeDescriptor"/>.
    /// </summary>
    internal sealed class XmlDrivenCustomTypeDescriptionProvider : TypeDescriptionProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDrivenCustomTypeDescriptor"/> class.
        /// </summary>
        /// <param name="parent">The parent provider.</param>
        /// <param name="propertyDescriptors">The property descriptors for this type.</param>
        internal XmlDrivenCustomTypeDescriptionProvider(TypeDescriptionProvider parent, IEnumerable<PropertyDescriptor> propertyDescriptors)
            : base(parent)
        {
            if (propertyDescriptors != null)
                this.propertyDescriptors.AddRange(propertyDescriptors);
        }

        /// <summary>
        /// Gets the type descriptor for the given type and object.
        /// </summary>
        /// <param name="objectType">The type of object for which to retrieve the type descriptor.</param>
        /// <param name="instance">An instance of the type, or <see langword="null"/>.</param>
        /// <returns>The type descriptor for the specified type.</returns>
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, Object instance)
        {
            return new XmlDrivenCustomTypeDescriptor(base.GetTypeDescriptor(objectType, instance), propertyDescriptors);
        }

        // State values.
        private readonly List<PropertyDescriptor> propertyDescriptors = new List<PropertyDescriptor>();
    }
}
