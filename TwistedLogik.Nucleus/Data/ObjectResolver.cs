using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
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
            miResolveLazilyLoadedDataObject = typeof(ObjectResolver).GetMethod(
                "ResolveLazilyLoadedDataObject", BindingFlags.NonPublic | BindingFlags.Static);

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
			return FromString(value, type, Thread.CurrentThread.CurrentCulture, false);
		}

        /// <summary>
        /// Creates an object from the specified value string.
        /// </summary>
        /// <param name="value">The value string from which to create the object.</param>
        /// <param name="type">The type of object to create.</param>
        /// <param name="ignoreCase">A value indicating whether to ignore casing whenever relevant (particularly, when converting enum values).</param>
        /// <returns>The object that was created.</returns>
        public static Object FromString(String value, Type type, Boolean ignoreCase)
        {
            return FromString(value, type, Thread.CurrentThread.CurrentCulture, ignoreCase);
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
            return FromString(value, type, provider, false);
        }

		/// <summary>
		/// Creates an object from the specified value string.
		/// </summary>
		/// <param name="value">The value string from which to create the object.</param>
		/// <param name="type">The type of object to create.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="ignoreCase">A value indicating whether to ignore casing whenever relevant (particularly, when converting enum values).</param>
        /// <returns>The object that was created.</returns>
		public static Object FromString(String value, Type type, IFormatProvider provider, Boolean ignoreCase)
		{
            // Ensure that the static constructor for this class has been run, as it
            // might need to register custom resolvers.
            RuntimeHelpers.RunClassConstructor(type.TypeHandle);

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
				return ParseEnum(type, value, ignoreCase);
			}

            // Handle lazy loaders.
            Type dataObjectType;
            if (IsLazilyLoadedDataObjectType(type, out dataObjectType))
            {
                return miResolveLazilyLoadedDataObject.MakeGenericMethod(dataObjectType)
                    .Invoke(null, new object[] { value });
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

        /// <summary>
        /// Gets a value indicating whether the specified type is a lazily-loaded Nucleus data object.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        /// <param name="dataObjectType">The type of data object to load.</param>
        /// <returns><c>true</c> if the specified type is a lazily-loaded Nucleus data object; otherwise, <c>false</c>.</returns>
        private static Boolean IsLazilyLoadedDataObjectType(Type type, out Type dataObjectType)
        {
            dataObjectType = null;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Lazy<>))
            {
                dataObjectType = type.GetGenericArguments()[0];
                if (typeof(DataObject).IsAssignableFrom(dataObjectType))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Resolves a value to an instance of <see cref="Lazy{T}"/>, where the lazily-loaded
        /// object is an instance of <see cref="DataObject"/>.
        /// </summary>
        private static Object ResolveLazilyLoadedDataObject<T>(String value) where T : DataObject
        {
            var registry = DataObjectRegistries.Get<T>();
            var reference = DataObjectRegistries.ResolveReference(value);
            return new Lazy<T>(() => registry.GetObject(reference));
        }

		// Custom resolvers that have been registered with the object resolution system.
		private static readonly Dictionary<Type, CustomObjectResolver> registeredCustomResolvers =
			new Dictionary<Type, CustomObjectResolver>();

		// Cached parse methods.
		private static readonly Dictionary<Type, MethodInfo> cachedCultureAwareParse = 
			new Dictionary<Type, MethodInfo>();
		private static readonly Dictionary<Type, MethodInfo> cachedCultureIgnorantParse =
			new Dictionary<Type, MethodInfo>();

        // Cached resolution methods.
        private static readonly MethodInfo miResolveLazilyLoadedDataObject;
	}
}
