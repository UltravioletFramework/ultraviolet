using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the base class for compiled data source wrappers.
    /// </summary>
    [CLSCompliant(false)]
    public abstract class CompiledDataSourceWrapper : IDataSourceWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledDataSourceWrapper"/> class.
        /// </summary>
        /// <param name="namescope">The namescope for this data source wrapper.</param>
        protected CompiledDataSourceWrapper(Namescope namescope)
        {
            Contract.Require(namescope, nameof(namescope));

            this.namescope = namescope;
        }

        /// <inheritdoc/>
        public abstract Object WrappedDataSource { get; }

        /// <summary>
        /// Converts the specified value to a <see cref="String"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of value to convert.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="format">The format string with which to convert the value.</param>
        /// <returns>The <see cref="String"/> instance that was created.</returns>
        protected String __UPF_ConvertToString<T>(T value, String format)
        {
            if (format == null)
            {
                return (value == null) ? null : value.ToString();
            }
            return (String)BindingConversions.ConvertValue(value, typeof(T), typeof(String), format, true);
        }

        /// <summary>
        /// Converts the specified string to an instance of the specified type using the Ultraviolet deserializer.
        /// </summary>
        /// <typeparam name="T">The type of value to convert.</typeparam>
        /// <typeparam name="U">The type of value to produce.</typeparam>
        /// <param name="value">The string to convert.</param>
        /// <param name="example">An example value which will cause the compiler to perform type inference.</param>
        /// <returns>The value that was created.</returns>
        protected U __UPF_ConvertFromString<T, U>(T value, U example)
        {
            return (U)BindingConversions.ConvertValue(value?.ToString(), typeof(String), typeof(U));
        }

        /// <summary>
        /// Performs a pass-through conversion for an instance of the <see cref="String"/> class.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="example">An example value which will cause the compiler to perform type inference.</param>
        /// <returns>The value that was created.</returns>
        protected String __UPF_ConvertFromString(String value, String example)
        {
            return value;
        }

        /// <summary>
        /// Performs a pass-through conversion for an instance of the <see cref="VersionedStringSource"/> structure.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="example">An example value which will cause the compiler to perform type inference.</param>
        /// <returns>The value that was created.</returns>
        protected VersionedStringSource __UPF_ConvertFromString(VersionedStringSource value, VersionedStringSource example)
        {
            return value;
        }

        /// <summary>
        /// Performs a pass-through conversion for an instance of the <see cref="VersionedStringBuilder"/> class.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="example">An example value which will cause the compiler to perform type inference.</param>
        /// <returns>The value that was created.</returns>
        protected VersionedStringBuilder __UPF_ConvertFromString(VersionedStringBuilder value, VersionedStringBuilder example)
        {
            return value;
        }

        /// <summary>
        /// Gets the object in the current namescope with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of object to retrieve.</typeparam>
        /// <param name="name">The name of the object to retrieve.</param>
        /// <returns>The object with the specified name, or <see langword="null"/> if no such object exists.</returns>
        protected T __UPF_FindName<T>(String name) where T : class
        {
            return namescope.FindName(name) as T;
        }

        // State values.
        private readonly Namescope namescope;
    }
}
