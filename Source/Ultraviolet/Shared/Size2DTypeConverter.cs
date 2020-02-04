using System;
using System.Collections;
using System.ComponentModel;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a type converter for the <see cref="Size2D"/> structure.
    /// </summary>
    public class Size2DTypeConverter : UltravioletExpandableObjectConverter<Size2D>
    {
        /// <inheritdoc/>
        public override Boolean GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override Object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            var width = (Double)propertyValues["Width"];
            var height = (Double)propertyValues["Height"];

            return new Size2D(width, height);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var size = (Size2D)value;
            return String.Format("{0}, {1}", size.Width, size.Height);
        }
    }
}
