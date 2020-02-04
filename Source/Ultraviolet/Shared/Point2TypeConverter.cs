using System;
using System.Collections;
using System.ComponentModel;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a type converter for the <see cref="Point2"/> structure.
    /// </summary>
    public class Point2TypeConverter : UltravioletExpandableObjectConverter<Point2>
    {
        /// <inheritdoc/>
        public override Boolean GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override Object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            var x = (Int32)propertyValues["X"];
            var y = (Int32)propertyValues["Y"];

            return new Point2(x, y);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var size = (Point2)value;
            return String.Format("{0}, {1}", size.X, size.Y);
        }
    }
}
