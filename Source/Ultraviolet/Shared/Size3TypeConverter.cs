using System;
using System.Collections;
using System.ComponentModel;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a type converter for the <see cref="Size3"/> structure.
    /// </summary>
    public class Size3TypeConverter : UltravioletExpandableObjectConverter<Size3>
    {
        /// <inheritdoc/>
        public override Boolean GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override Object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            var width = (Int32)propertyValues["Width"];
            var height = (Int32)propertyValues["Height"];
            var depth = (Int32)propertyValues["Depth"];

            return new Size3(width, height, depth);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var size = (Size3)value;
            return String.Format("{0}, {1}, {2}", size.Width, size.Height, size.Depth);
        }
    }
}
