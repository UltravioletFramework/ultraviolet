using System;
using System.Reflection;
using System.Xml.Linq;

namespace TwistedLogik.Nucleus.Xml
{
    /// <summary>
    /// Contains extension methods for XElement.
    /// </summary>
    public static class XElementExtensions
    {
        /// <summary>
        /// Gets the value of the specified attribute as a string.
        /// </summary>
        /// <param name="element">The XElement from which to retrieve the attribute.</param>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <returns>The string value of the attribute if it exists, or null if it does not exist.</returns>
        public static String AttributeValueString(this XElement element, XName name)
        {
            var attr = element.Attribute(name);
            if (attr != null)
                return attr.Value;
            return null;
        }

        /// <summary>
        /// Gets the value of the specified element as a string.
        /// </summary>
        /// <param name="element">The XElement from which to retrieve the element.</param>
        /// <param name="name">The name of the element to retrieve.</param>
        /// <returns>The string value of the element if it exists, or null if it does not exist.</returns>
        public static String ElementValueString(this XElement element, XName name)
        {
            var e = element.Element(name);
            if (e != null)
                return e.Value;
            return null;
        }

        /// <summary>
        /// Gets the value of the specified attribute as an Int32.
        /// </summary>
        /// <param name="element">The XElement from which to retrieve the attribute.</param>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <returns>The Int32 value of the attribute if it exists, or null if it does not exist.</returns>
        public static Int32? AttributeValueInt32(this XElement element, XName name)
        {
            var attr = element.Attribute(name);
            if (attr != null)
            {
                int value;
                if (Int32.TryParse(attr.Value, out value))
                    return value;
            }
            return null;
        }

        /// <summary>
        /// Gets the value of the specified element as an Int32.
        /// </summary>
        /// <param name="element">The XElement from which to retrieve the element.</param>
        /// <param name="name">The name of the element to retrieve.</param>
        /// <returns>The Int32 value of the element if it exists, or null if it does not exist.</returns>
        public static Int32? ElementValueInt32(this XElement element, XName name)
        {
            var e = element.Element(name);
            if (e != null)
            {
                int value;
                if (Int32.TryParse(e.Value, out value))
                    return value;
            }
            return null;
        }

        /// <summary>
        /// Gets the value of the specified attribute as a Single.
        /// </summary>
        /// <param name="element">The XElement from which to retrieve the attribute.</param>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <returns>The Single value of the attribute if it exists, or null if it does not exist.</returns>
        public static Single? AttributeValueSingle(this XElement element, XName name)
        {
            var attr = element.Attribute(name);
            if (attr != null)
            {
                float value;
                if (Single.TryParse(attr.Value, out value))
                    return value;
            }
            return null;
        }

        /// <summary>
        /// Gets the value of the specified element as a Single.
        /// </summary>
        /// <param name="element">The XElement from which to retrieve the element.</param>
        /// <param name="name">The name of the element to retrieve.</param>
        /// <returns>The Single value of the element if it exists, or null if it does not exist.</returns>
        public static Single? ElementValueSingle(this XElement element, XName name)
        {
            var e = element.Element(name);
            if (e != null)
            {
                float value;
                if (Single.TryParse(e.Value, out value))
                    return value;
            }
            return null;
        }

        /// <summary>
        /// Gets the value of the specified attribute as a Double.
        /// </summary>
        /// <param name="element">The XElement from which to retrieve the attribute.</param>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <returns>The Double value of the attribute if it exists, or null if it does not exist.</returns>
        public static Double? AttributeValueDouble(this XElement element, XName name)
        {
            var attr = element.Attribute(name);
            if (attr != null)
            {
                double value;
                if (Double.TryParse(attr.Value, out value))
                    return value;
            }
            return null;
        }

        /// <summary>
        /// Gets the value of the specified element as a Double.
        /// </summary>
        /// <param name="element">The XElement from which to retrieve the element.</param>
        /// <param name="name">The name of the element to retrieve.</param>
        /// <returns>The Double value of the element if it exists, or null if it does not exist.</returns>
        public static Double? ElementValueDouble(this XElement element, XName name)
        {
            var e = element.Element(name);
            if (e != null)
            {
                double value;
                if (Double.TryParse(e.Value, out value))
                    return value;
            }
            return null;
        }

        /// <summary>
        /// Gets the value of the specified attribute as a Boolean.
        /// </summary>
        /// <param name="element">The XElement from which to retrieve the attribute.</param>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <returns>The Boolean value of the attribute if it exists, or null if it does not exist.</returns>
        public static Boolean? AttributeValueBoolean(this XElement element, XName name)
        {
            var attr = element.Attribute(name);
            if (attr != null)
            {
                bool value;
                if (Boolean.TryParse(attr.Value, out value))
                    return value;
            }
            return null;
        }

        /// <summary>
        /// Gets the value of the specified element as a Boolean.
        /// </summary>
        /// <param name="element">The XElement from which to retrieve the element.</param>
        /// <param name="name">The name of the element to retrieve.</param>
        /// <returns>The Boolean value of the element if it exists, or null if it does not exist.</returns>
        public static Boolean? ElementValueBoolean(this XElement element, XName name)
        {
            var e = element.Element(name);
            if (e != null)
            {
                bool value;
                if (Boolean.TryParse(e.Value, out value))
                    return value;
            }
            return null;
        }

        /// <summary>
        /// Gets the value of the specified attribute as the specified type.
        /// </summary>
        /// <param name="element">The XElement from which to retrieve the attribute.</param>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <returns>The value of the attribute if it exists, or null if it does not exist.</returns>
        public static T AttributeValue<T>(this XElement element, XName name)
        {
            var str = AttributeValueString(element, name);
            if (str == null)
                return default(T);

            var type = IsNullableType(typeof(T)) ? GetNullableBaseType(typeof(T)) : typeof(T);
            var parse = type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(String) }, null);
            if (parse != null)
            {
                return (T)parse.Invoke(null, new object [] { str });
            }
            return (T)Convert.ChangeType(str, typeof(T));
        }

        /// <summary>
        /// Gets the value of the specified element as the specified type.
        /// </summary>
        /// <param name="element">The XElement from which to retrieve the element.</param>
        /// <param name="name">The name of the element to retrieve.</param>
        /// <returns>The value of the element if it exists, or null if it does not exist.</returns>
        public static T ElementValue<T>(this XElement element, XName name)
        {
            var str = ElementValueString(element, name);
            if (str == null)
                return default(T);

            if (typeof(T).IsEnum)
                return (T)Enum.Parse(typeof(T), str);
            
            var type = IsNullableType(typeof(T)) ? GetNullableBaseType(typeof(T)) : typeof(T);
            var parse = type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(String) }, null);
            if (parse != null)
            {
                return (T)parse.Invoke(null, new object[] { str });
            }
            return (T)Convert.ChangeType(str, typeof(T));
        }

        /// <summary>
        /// Gets the value of the specified attribute as an enumeration value.
        /// </summary>
        /// <param name="element">The XElement from which to retrieve the attribute.</param>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <returns>The enumeration value of the attribute if it exists, or null if it does not exist.</returns>
        public static T? AttributeValueEnum<T>(this XElement element, XName name) where T : struct
        {
            var str = AttributeValueString(element, name);
            if (!String.IsNullOrWhiteSpace(str))
            {
                return (T)Enum.Parse(typeof(T), str);
            }
            return null;
        }

        /// <summary>
        /// Gets the value of the specified element as an enumeration value.
        /// </summary>
        /// <param name="element">The XElement from which to retrieve the element.</param>
        /// <param name="name">The name of the element to retrieve.</param>
        /// <returns>The enumeration value of the element if it exists, or null if it does not exist.</returns>
        public static T? ElementValueEnum<T>(this XElement element, XName name) where T : struct
        {
            var str = ElementValueString(element, name);
            if (!String.IsNullOrWhiteSpace(str))
            {
                return (T)Enum.Parse(typeof(T), str);
            }
            return null;
        }

        /// <summary>
        /// Gets a value indicating whether the specified type is an implementation of the Nullable type.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        /// <returns>true if the specified type is a nullable; otherwise, false.</returns>
        private static Boolean IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Gets the base type of the specified nullable type.
        /// </summary>
        /// <param name="type">The nullable type to evaluate.</param>
        /// <returns>The base type of the specified nullable type.</returns>
        private static Type GetNullableBaseType(Type type)
        {
            return type.GetGenericArguments()[0];
        }
    }
}
