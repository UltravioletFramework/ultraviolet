using System;
using System.ComponentModel;
using System.Globalization;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a type converter for the <see cref="VersionedStringSource"/> structure.
    /// </summary>
    public class VersionedStringSourceTypeConverter : TypeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionedStringSourceTypeConverter"/> class.
        /// </summary>
        public VersionedStringSourceTypeConverter()
        {
            stringTypeConverter = TypeDescriptor.GetConverter(typeof(String));
        }

        /// <inheritdoc/>
        public override Boolean CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(VersionedStringSource))
            {
                return true;
            }
            return stringTypeConverter.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value.GetType() == typeof(VersionedStringSource))
            {
                return value.ToString();
            }
            return stringTypeConverter.ConvertTo(context, culture, ((VersionedStringSource)value).ToString(), destinationType);
        }

        /// <inheritdoc/>
        public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
            {
                return true;
            }
            return stringTypeConverter.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc/>
        public override Object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, Object value)
        {
            if (!(value is String))
                value = stringTypeConverter.ConvertFrom(context, culture, value);

            return new VersionedStringSource((String)value);
        }
        
        // If we don't know how to do a conversion, fall back to String.
        private readonly TypeConverter stringTypeConverter;
    }
}
