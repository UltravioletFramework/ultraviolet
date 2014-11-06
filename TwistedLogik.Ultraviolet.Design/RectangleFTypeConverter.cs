using System;
using System.Collections;
using System.ComponentModel;

namespace TwistedLogik.Ultraviolet.Design
{
    /// <summary>
    /// Represents a type converter for the <see cref="RectangleF"/> structure.
    /// </summary>
    public class RectangleFTypeConverter : UltravioletExpandableObjectConverter<RectangleF>
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
            var width  = (Single)propertyValues["Width"];
            var height = (Single)propertyValues["Height"];

            return new RectangleF(x, y, width, height);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var rect = (RectangleF)value;
            return String.Format("({0}, {1}, {2}, {3})", rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
