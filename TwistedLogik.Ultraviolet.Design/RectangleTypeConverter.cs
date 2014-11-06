using System;
using System.Collections;
using System.ComponentModel;

namespace TwistedLogik.Ultraviolet.Design
{
    /// <summary>
    /// Represents a type converter for the <see cref="Rectangle"/> structure.
    /// </summary>
    public class RectangleTypeConverter : UltravioletExpandableObjectConverter<Rectangle>
    {
        /// <inheritdoc/>
        public override Boolean GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override Object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            var x      = (Int32)propertyValues["X"];
            var y      = (Int32)propertyValues["Y"];
            var width  = (Int32)propertyValues["Width"];
            var height = (Int32)propertyValues["Height"];

            return new Rectangle(x, y, width, height);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var rect = (Rectangle)value;
            return String.Format("{0}, {1}, {2}, {3}", rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
