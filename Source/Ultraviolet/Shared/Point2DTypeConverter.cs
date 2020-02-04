using System;
using System.Collections;
using System.ComponentModel;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a type converter for the <see cref="Point2D"/> structure.
    /// </summary>
    public class Point2DTypeConverter : UltravioletExpandableObjectConverter<Point2D>
    {
        /// <inheritdoc/>
        public override Boolean GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override Object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            var x = (Double)propertyValues["X"];
            var y = (Double)propertyValues["Y"];

            return new Point2D(x, y);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var size = (Point2D)value;
            return String.Format("{0}, {1}", size.X, size.Y);
        }
    }
}
