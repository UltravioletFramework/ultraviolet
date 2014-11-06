using System;
using System.Collections;
using System.ComponentModel;

namespace TwistedLogik.Ultraviolet.Design
{
    /// <summary>
    /// Represents a type converter for the <see cref="CircleF"/> structure.
    /// </summary>
    public class CircleFTypeConverter : UltravioletExpandableObjectConverter<CircleF>
    {
        /// <inheritdoc/>
        public override Boolean GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override Object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            var x      = (Single)propertyValues["X"];
            var y      = (Single)propertyValues["Y"];
            var radius = (Single)propertyValues["Radius"];

            return new CircleF(x, y, radius);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var circle = (CircleF)value;
            return String.Format("({0}, {1}) radius {2}", circle.X, circle.Y, circle.Radius);
        }
    }
}
