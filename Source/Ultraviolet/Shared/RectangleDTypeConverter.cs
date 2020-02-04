using System;
using System.Collections;
using System.ComponentModel;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a type converter for the <see cref="RectangleD"/> structure.
    /// </summary>
    public class RectangleDTypeConverter : UltravioletExpandableObjectConverter<RectangleD>
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
            var width = (Double)propertyValues["Width"];
            var height = (Double)propertyValues["Height"];

            return new RectangleD(x, y, width, height);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var rect = (RectangleD)value;
            return String.Format("{0}, {1}, {2}, {3}", rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
