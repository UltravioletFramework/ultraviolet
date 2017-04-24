using System;
using System.Collections;
using System.ComponentModel;

namespace Ultraviolet.Design
{
    /// <summary>
    /// Represents a type converter for the <see cref="Vector3"/> structure.
    /// </summary>
    public class Vector3TypeConverter : UltravioletExpandableObjectConverter<Vector3>
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

            return new Vector3(x, y, z);
        }
        
        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            var vector = (Vector3)value;
            return String.Format("{0}, {1}, {2}", vector.X, vector.Y, vector.Z);
        }
    }
}
