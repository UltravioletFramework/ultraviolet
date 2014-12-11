using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a method which resolves a string value to an object.
    /// </summary>
    /// <param name="value">The string value to resolve.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The resolved object.</returns>
    public delegate Object CustomObjectResolver(String value, IFormatProvider provider);

    /// <summary>
    /// Contains methods for resolving CLR objects out of XML data.
    /// </summary>
    public static class ObjectResolver
    {
        /// <summary>
        /// Initializes the <see cref="ObjectResolver"/> type.
        /// </summary>
        static ObjectResolver()
        {
            RegisterValueResolver<MaskedUInt32>  ((value, provider) => new MaskedUInt32(UInt32.Parse(value, provider)));
            RegisterValueResolver<MaskedUInt64>  ((value, provider) => new MaskedUInt64(UInt64.Parse(value, provider)));
            RegisterValueResolver<StringResource>((value, provider) => new StringResource(value));
        }

        /// <summary>
        /// Registers a custom value resolver with the object resolution system.
        /// </summary>
        /// <typeparam name="T">The type for which to register a value resolver.</typeparam>
        /// <param name="resolver">The custom value resolver to register.</param>
        public static void RegisterValueResolver<T>(CustomObjectResolver resolver)
        {
            Contract.Require(resolver, "resolver");

            registeredCustomResolvers[typeof(T)] = resolver;
        }

        /// <summary>
        /// Creates an object from the specified value string.
        /// </summary>
        /// <param name="value">The value string from which to create the object.</param>
        /// <param name="type">The type of object to create.</param>
        /// <returns>The object that was created.</returns>
        public static Object FromString(String value, Type type)
        {
            return FromString(value, type, Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Creates an object from the specified value string.
        /// </summary>
        /// <param name="value">The value string from which to create the object.</param>
        /// <param name="type">The type of object to create.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>The object that was created.</returns>
        public static Object FromString(String value, Type type, IFormatProvider provider)
        {
            // Handle some special cases...
            if (type == typeof(Char?))
            {
                if (value == null)
                    return null;
                if (value.Length > 1)
                    throw new FormatException();
                return value[0];
            }
            if (type == typeof(Char))
            {
                if (value.Length > 1)
                    throw new FormatException();
                return value[0];
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (value == null)
                    return null;
                type = type.GetGenericArguments()[0];
            }

            // Handle custom resolvers.
            CustomObjectResolver customResolver;
            if (registeredCustomResolvers.TryGetValue(type, out customResolver))
            {
                return customResolver(value, provider);
            }

            // Handle enumerations.
            if (type.IsEnum)
            {
                return ParseEnum(type, value, false);
            }

            // Handle object references.
            if (type.Equals(typeof(Guid)))
            {
                return DataObjectRegistries.ResolveReference(value).Value;
            }
            if (type.Equals(typeof(ResolvedDataObjectReference)))
            {
                if (value == "(none)" || String.IsNullOrEmpty(value))
                {
                    return new ResolvedDataObjectReference();
                }
                return DataObjectRegistries.ResolveReference(value); 
            }

            // Handle the general case by calling the type's Parse() method, if it exists,
            // and doing a type conversion if it does not.
            Object result;
            if (AttemptCultureAwareParse(value, type, provider, out result))
            {
                return result;
            }
            if (AttemptCultureIgnorantParse(value, type, out result))
            {
                return result;
            }
            return Convert.ChangeType(value, type, provider);
        }

        /// <summary>
        /// Parses a string into a set of enumeration values.
        /// </summary>
        /// <param name="type">The type of enumeration to parse.</param>
        /// <param name="value">The string to parse into a set of values.</param>
        /// <param name="ignoreCase">A value indicating whether to ignore case.</param>
        /// <returns>The parsed value.</returns>
        public static Object ParseEnum(Type type, String value, Boolean ignoreCase)
        {
            Contract.Require(type, "type");
            Contract.Require(value, "value");
            Contract.Ensure<ArgumentException>(type.IsEnum, "type");

            if (value == String.Empty)
                throw new FormatException();

            var values = value.Split('|');
            var numeric = 0;
            foreach (var v in values)
            {
                try
                {
                    numeric |= (int)Enum.Parse(type, v, ignoreCase);
                }
                catch (ArgumentException e)
                {
                    throw new FormatException(e.Message);
                }
            }
            return Enum.Parse(type, numeric.ToString());
        }

        /// <summary>
        /// Attempts to parse the specified string into the specified type using a culture-aware version of
        /// the Parse() method, if one exists for the type.
        /// </summary>
        private static Boolean AttemptCultureAwareParse(String value, Type type, IFormatProvider provider, out Object result)
        {
            MethodInfo method;
            if (!cachedCultureAwareParse.TryGetValue(type, out method))
            {
                method = type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(String), typeof(IFormatProvider) }, null);
                cachedCultureAwareParse[type] = method;
            }

            if (method != null)
            {
                try
                {
                    result = method.Invoke(null, new object[] { value, provider });
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException;
                }
                return true;
            }
            result = null;
            return false;
        }

        /// <summary>
        /// Attempts to parse the specified string into the specified type using the culture-ignorant version of
        /// the Parse() method, if one exists for the type.
        /// </summary>
        private static Boolean AttemptCultureIgnorantParse(String value, Type type, out Object result)
        {
            MethodInfo method;
			if (!cachedCultureIgnorantParse.TryGetValue(type, out method))
            {
                method = type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(String) }, null);
				cachedCultureIgnorantParse[type] = method;
            }

            if (method != null)
            {
                try
                {
                    result = method.Invoke(null, new object[] { value });
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException;
                }
                return true;
            }
            result = null;
            return false;
        }

        // Custom resolvers that have been registered with the object resolution system.
        private static readonly Dictionary<Type, CustomObjectResolver> registeredCustomResolvers =
            new Dictionary<Type, CustomObjectResolver>();

        // Cached parse methods.
        private static readonly Dictionary<Type, MethodInfo> cachedCultureAwareParse = 
            new Dictionary<Type, MethodInfo>();
        private static readonly Dictionary<Type, MethodInfo> cachedCultureIgnorantParse =
            new Dictionary<Type, MethodInfo>();
    }
}
