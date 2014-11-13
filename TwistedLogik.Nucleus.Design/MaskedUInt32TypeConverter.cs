using System;
using System.ComponentModel;
using System.Globalization;

namespace TwistedLogik.Nucleus.Design
{
    /// <summary>
    /// Represents a type converter for the <see cref="MaskedUInt32"/> structure.
    /// </summary>
    public class MaskedUInt32TypeConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override Boolean CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(MaskedUInt32))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value.GetType() == typeof(MaskedUInt32))
            {
                return value.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc/>
        public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc/>
        public override Object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, Object value)
        {
            if (value is String)
            {
                return new MaskedUInt32(UInt32.Parse((String)value));
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
