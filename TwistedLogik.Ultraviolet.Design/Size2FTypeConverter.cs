using System;
using System.Collections;
using System.ComponentModel;

namespace TwistedLogik.Ultraviolet.Design
{
    /// <summary>
    /// Represents a type converter for the <see cref="Size2F"/> structure.
    /// </summary>
    public class Size2FTypeConverter : UltravioletExpandableObjectConverter<Size2F>
    {
        /// <inheritdoc/>
        public override Boolean GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override Object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            var width  = (Single)propertyValues["Width"];
            var height = (Single)propertyValues["Height"];

            return new Size2F(width, height);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var size = (Size2F)value;
            return String.Format("{0}, {1}", size.Width, size.Height);
        }
    }
}
