using System;
using System.Collections;
using System.ComponentModel;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a type converter for the <see cref="Point2F"/> structure.
    /// </summary>
    public class Point2FTypeConverter : UltravioletExpandableObjectConverter<Point2F>
    {
        /// <inheritdoc/>
        public override Boolean GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override Object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            var x = (Single)propertyValues["X"];
            var y = (Single)propertyValues["Y"];

            return new Point2F(x, y);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var size = (Point2F)value;
            return String.Format("{0}, {1}", size.X, size.Y);
        }
    }
}
