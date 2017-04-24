using System;
using System.Collections;
using System.ComponentModel;

namespace Ultraviolet.Design
{
    /// <summary>
    /// Represents a type converter for the <see cref="Color"/> structure.
    /// </summary>
    public class ColorTypeConverter : UltravioletExpandableObjectConverter<Color>
    {
        /// <inheritdoc/>
        public override Boolean GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override Object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            var a = (Byte)propertyValues["A"];
            var r = (Byte)propertyValues["R"];
            var g = (Byte)propertyValues["G"];
            var b = (Byte)propertyValues["B"];

            return new Color(r, g, b, a);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var color = (Color)value;
            return String.Format("{0}, {1}, {2}, {3}", color.A, color.R, color.G, color.B);
        }
    }
}
