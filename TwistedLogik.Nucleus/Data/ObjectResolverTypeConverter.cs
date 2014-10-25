using System;
using System.ComponentModel;
using System.Globalization;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a type converter which makes use of the Nucleus Object Resolver.
    /// </summary>
    /// <typeparam name="T">The type of object to convert.</typeparam>
    public class ObjectResolverTypeConverter<T> : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        public override Object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, Object value)
        {
            if (value is String)
            {
                if (String.IsNullOrEmpty((String)value))
                    return default(T);

                var type = (context == null) ? typeof(T) : context.PropertyDescriptor.PropertyType;
                return ObjectResolver.FromString((String)value, type);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}