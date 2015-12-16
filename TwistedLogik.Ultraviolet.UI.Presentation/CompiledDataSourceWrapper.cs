using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Ultraviolet.UI.Presentation
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
            Contract.Require(namescope, "namescope");

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
        /// Converts the specified string to an instance of the specified type using the Nucleus deserializer.
        /// </summary>
        /// <typeparam name="T">The type of value to produce.</typeparam>
        /// <param name="value">The string to convert.</param>
        /// <param name="example">An example value which will cause the compiler to perform generic type inference.</param>
        /// <returns>The value that was created.</returns>
        protected T __UPF_ConvertFromString<T>(String value, T example)
        {
            return (T)BindingConversions.ConvertValue(value, typeof(String), typeof(T));
        }

        /// <summary>
        /// Gets the element in the current namescope with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of element to retrieve.</typeparam>
        /// <param name="name">The name of the element to retrieve.</param>
        /// <returns>The element with the specified name.</returns>
        protected T __UPF_GetElementByName<T>(String name) where T : FrameworkElement
        {
            return namescope.GetElementByName(name) as T;
        }

        // State values.
        private readonly Namescope namescope;
    }
}
