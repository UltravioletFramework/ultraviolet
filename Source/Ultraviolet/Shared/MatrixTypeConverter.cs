using System;
using System.Collections;
using System.ComponentModel;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a type converter for the <see cref="Matrix"/> structure.
    /// </summary>
    public class MatrixTypeConverter : UltravioletExpandableObjectConverter<Matrix>
    {
        /// <inheritdoc/>
        public override Boolean GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override Object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            var m11 = (Single)propertyValues["M11"];
            var m12 = (Single)propertyValues["M12"];
            var m13 = (Single)propertyValues["M13"];
            var m14 = (Single)propertyValues["M14"];

            var m21 = (Single)propertyValues["M21"];
            var m22 = (Single)propertyValues["M22"];
            var m23 = (Single)propertyValues["M23"];
            var m24 = (Single)propertyValues["M24"];

            var m31 = (Single)propertyValues["M31"];
            var m32 = (Single)propertyValues["M32"];
            var m33 = (Single)propertyValues["M33"];
            var m34 = (Single)propertyValues["M34"];

            var m41 = (Single)propertyValues["M41"];
            var m42 = (Single)propertyValues["M42"];
            var m43 = (Single)propertyValues["M43"];
            var m44 = (Single)propertyValues["M44"];

            return new Matrix(
                m11, m12, m13, m14,
                m21, m22, m23, m24,
                m31, m32, m33, m34,
                m41, m42, m43, m44
            );
        }

        /// <inheritdoc/>
        protected override String GetStringRepresentation(Object value)
        {
            return "(Matrix)";
        }
    }
}
