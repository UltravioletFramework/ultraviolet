using System;
using System.Collections;
using System.ComponentModel;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a type converter for the <see cref="Size3D"/> structure.
    /// </summary>
    public class Size3DTypeConverter : UltravioletExpandableObjectConverter<Size3D>
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
            var depth = (Double)propertyValues["Depth"];

            return new Size3D(width, height, depth);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var size = (Size3D)value;
            return String.Format("{0}, {1}, {2}", size.Width, size.Height, size.Depth);
        }
    }
}
