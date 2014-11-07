using System;
using System.ComponentModel;
using System.Globalization;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Nucleus.Design.Text
{
    /// <summary>
    /// Represents a type converter for the <see cref="StringResource"/> class.
    /// </summary>
    public class StringResourceTypeConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override Boolean CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(StringResource))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)
        {
            if (destinationType == typeof(String))
            {
                if (value == null)
                {
                    return null;
                }
                if (value.GetType() == typeof(StringResource))
                {
                    return ((StringResource)value).Key;
                }
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
                return new StringResource((String)value);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
