using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains extension methods for the <see cref="Type"/> class.
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Initializes the <see cref="TypeExtensions"/> type.
        /// </summary>
        static TypeExtensions()
        {
            InitializeNumericTypes();
            InitializeNumericConversions();
        }

        /// <summary>
        /// Gets the type of the element contained by the specified generic list type.
        /// </summary>
        /// <param name="type">The generic list type to evaluate.</param>
        /// <returns>The type of the list's elements.</returns>
        public static Type GetGenericListElementType(this Type type)
        {
            var ifaces = type.GetInterfaces();
            foreach (var iface in ifaces)
            {
                if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    return iface.GetGenericArguments()[0];
                }
            }
            return null;
        }

        /// <summary>
        /// Gets a value indicating whether the specified type is a primitive numeric type.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        /// <returns><see langword="true"/> if the specified type is a primitive numeric type; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsNumericType(this Type type)
        {
            return numericTypes.Contains(type);
        }

        /// <summary>
        /// Gets a value indicating whether the specified type can be implicitly or explicitly converted
        /// to the specified type.
        /// </summary>
        /// <param name="typeFrom">The type from which to convert.</param>
        /// <param name="typeTo">The type to which to convert.</param>
        /// <returns><see langword="true"/> if the specified conversion is defined; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsConvertibleTo(this Type typeFrom, Type typeTo)
        {
            if (typeTo.IsAssignableFrom(typeFrom))
                return true;

            if (typeFrom.IsEnum && IsNumericType(typeTo) ||
                typeTo.IsEnum && IsNumericType(typeFrom))
            {
                return true;
            }

            Dictionary<Type, Boolean> supportedCasts;
            lock (castabilityRegistry)
            {
                if (!castabilityRegistry.TryGetValue(typeTo, out supportedCasts))
                {
                    supportedCasts = new Dictionary<Type, Boolean>();
                    castabilityRegistry[typeTo] = supportedCasts;
                }
            }

            lock (supportedCasts)
            {
                Boolean castable;
                if (!supportedCasts.TryGetValue(typeFrom, out castable))
                {
                    var casts = from m in typeFrom.GetMethods(BindingFlags.Public | BindingFlags.Static)
                                where
                                    (m.Name == "op_Explicit" || m.Name == "op_Implicit") && m.ReturnType == typeTo
                                select m;

                    castable = casts.Any();
                    supportedCasts[typeFrom] = castable;
                }
                return castable;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified type can be implicitly or explicitly converted
        /// to the specified type, and vice versa.
        /// </summary>
        /// <param name="typeFrom">The type from which to convert.</param>
        /// <param name="typeTo">The type to which to convert.</param>
        /// <returns><see langword="true"/> if the specified conversions are defined; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsMutuallyConvertibleTo(this Type typeFrom, Type typeTo)
        {
            return typeFrom.IsConvertibleTo(typeTo) && typeTo.IsConvertibleTo(typeFrom);
        }

        /// <summary>
        /// Initializes the list of numeric types.
        /// </summary>
        private static void InitializeNumericTypes()
        {
            numericTypes.Clear();
            numericTypes.Add(typeof(sbyte));
            numericTypes.Add(typeof(byte));
            numericTypes.Add(typeof(short));
            numericTypes.Add(typeof(ushort));
            numericTypes.Add(typeof(int));
            numericTypes.Add(typeof(uint));
            numericTypes.Add(typeof(long));
            numericTypes.Add(typeof(ulong));
            numericTypes.Add(typeof(char));
            numericTypes.Add(typeof(float));
            numericTypes.Add(typeof(double));
            numericTypes.Add(typeof(decimal));
        }

        /// <summary>
        /// Initializes the lists of valid numeric type conversions.
        /// </summary>
        private static void InitializeNumericConversions()
        {
            // As defined by §6.1.2 (Implicit numeric conversions) of the C# Language Specification
            AddConversions<sbyte>(typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal));
            AddConversions<byte>(typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal));
            AddConversions<short>(typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal));
            AddConversions<ushort>(typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal));
            AddConversions<int>(typeof(long), typeof(float), typeof(double), typeof(decimal));
            AddConversions<uint>(typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal));
            AddConversions<long>(typeof(float), typeof(double), typeof(decimal));
            AddConversions<ulong>(typeof(float), typeof(double), typeof(decimal));
            AddConversions<char>(typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal));
            AddConversions<float>(typeof(double));

            // As defined by §6.2.1 (Explicit numeric conversions) of the C# Language Specification
            AddConversions<sbyte>(typeof(byte), typeof(ushort), typeof(uint), typeof(ulong), typeof(char));
            AddConversions<byte>(typeof(sbyte), typeof(char));
            AddConversions<short>(typeof(sbyte), typeof(byte), typeof(ushort), typeof(uint), typeof(ulong), typeof(char));
            AddConversions<ushort>(typeof(sbyte), typeof(byte), typeof(short), typeof(char));
            AddConversions<int>(typeof(sbyte), typeof(byte), typeof(short), typeof(char));
            AddConversions<uint>(typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(char));
            AddConversions<long>(typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(ulong), typeof(char));
            AddConversions<ulong>(typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(char));
            AddConversions<char>(typeof(sbyte), typeof(byte), typeof(short));
            AddConversions<float>(typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(char), typeof(decimal));
            AddConversions<double>(typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(char), typeof(float), typeof(decimal));
            AddConversions<decimal>(typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(char), typeof(float), typeof(double));
        }

        /// <summary>
        /// Adds a set of valid conversions for the specified type.
        /// </summary>
        /// <typeparam name="T">The type for which to add conversions.</typeparam>
        /// <param name="types">The list of types to which the specified type can be converted.</param>
        private static void AddConversions<T>(params Type[] types)
        {
            Dictionary<Type, Boolean> castability;
            if (!castabilityRegistry.TryGetValue(typeof(T), out castability))
            {
                castability = new Dictionary<Type, Boolean>();
                castabilityRegistry[typeof(T)] = castability;
            }

            foreach (var type in types)
            {
                castability[type] = true;
            }
        }

        // State values.
        private static readonly HashSet<Type> numericTypes = new HashSet<Type>();
        private static readonly Dictionary<Type, Dictionary<Type, Boolean>> castabilityRegistry = 
            new Dictionary<Type, Dictionary<Type, Boolean>>();
    }
}
