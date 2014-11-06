using System;
using System.Collections;
using System.ComponentModel;

namespace TwistedLogik.Ultraviolet.Design
{
    /// <summary>
    /// Represents a type converter for the <see cref="Size3F"/> structure.
    /// </summary>
    public class Size3FTypeConverter : UltravioletExpandableObjectConverter<Size3F>
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
            var depth  = (Single)propertyValues["Depth"];

            return new Size3F(width, height, depth);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var size = (Size3F)value;
            return String.Format("({0}, {1}, {2})", size.Width, size.Height, size.Depth);
        }
    }
}
