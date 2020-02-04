using System;
using System.Collections;
using System.ComponentModel;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a type converter for the <see cref="CircleD"/> structure.
    /// </summary>
    public class CircleDTypeConverter : UltravioletExpandableObjectConverter<CircleD>
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
            var radius = (Double)propertyValues["Radius"];

            return new CircleD(x, y, radius);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var circle = (CircleD)value;
            return String.Format("{0}, {1}, {2}", circle.X, circle.Y, circle.Radius);
        }
    }
}
