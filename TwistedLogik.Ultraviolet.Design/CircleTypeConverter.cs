using System;
using System.Collections;
using System.ComponentModel;

namespace TwistedLogik.Ultraviolet.Design
{
    /// <summary>
    /// Represents a type converter for the <see cref="Circle"/> structure.
    /// </summary>
    public class CircleTypeConverter : UltravioletExpandableObjectConverter<Circle>
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
            var radius = (Int32)propertyValues["Radius"];

            return new Circle(x, y, radius);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var circle = (Circle)value;
            return String.Format("{0}, {1} radius {2}", circle.X, circle.Y, circle.Radius);
        }
    }
}
