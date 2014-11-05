using System;
using System.ComponentModel;
using System.Globalization;

namespace TwistedLogik.Ultraviolet.Design
{
    public abstract class UltravioletExpandableObjectConverter<T> : ExpandableObjectConverter
    {
        /// <inheritdoc/>
        public override Boolean CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(T))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value.GetType() == typeof(T))
            {
                return GetStringRepresentation(value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Gets the string representation of the specified value.
        /// </summary>
        /// <param name="value">The value for which to retrieve a string representation.</param>
        /// <returns>The string representation of the specified value.</returns>
        protected virtual String GetStringRepresentation(Object value)
        {
            return (value == null) ? "(null)" : value.ToString();
        }
    }
}
