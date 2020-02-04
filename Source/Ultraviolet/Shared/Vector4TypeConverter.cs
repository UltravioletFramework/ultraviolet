using System;
using System.Collections;
using System.ComponentModel;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a type converter for the <see cref="Vector4"/> structure.
    /// </summary>
    public class Vector4TypeConverter : UltravioletExpandableObjectConverter<Vector4>
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
            var z = (Single)propertyValues["Z"];
            var w = (Single)propertyValues["W"];

            return new Vector4(x, y, z, w);
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var vector = (Vector4)value;
            return String.Format("{0}, {1}, {2}, {3}", vector.X, vector.Y, vector.Z, vector.W);
        }
    }
}
