using System;
using System.Collections;
using System.ComponentModel;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a type converter for the <see cref="Vector2"/> structure.
    /// </summary>
    public class Vector2TypeConverter : UltravioletExpandableObjectConverter<Vector2>
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

            return new Vector2(x, y);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var vector = (Vector2)value;
            return String.Format("{0}, {1}", vector.X, vector.Y);
        }
    }
}
