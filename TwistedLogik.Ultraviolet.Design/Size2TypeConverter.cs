using System;
using System.Collections;
using System.ComponentModel;

namespace TwistedLogik.Ultraviolet.Design
{
    /// <summary>
    /// Represents a type converter for the <see cref="Size2"/> structure.
    /// </summary>
    public class Size2TypeConverter : UltravioletExpandableObjectConverter<Size2>
    {
        /// <inheritdoc/>
        public override Boolean GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override Object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            var width  = (Int32)propertyValues["Width"];
            var height = (Int32)propertyValues["Height"];

            return new Size2(width, height);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var size = (Size2)value;
            return String.Format("{0}, {1}", size.Width, size.Height);
        }
    }
}
