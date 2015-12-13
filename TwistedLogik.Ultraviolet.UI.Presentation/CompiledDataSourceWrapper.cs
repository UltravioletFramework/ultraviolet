using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the base class for compiled data source wrappers.
    /// </summary>
    [CLSCompliant(false)]
    public abstract class CompiledDataSourceWrapper : IDataSourceWrapper
    {
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
            return String.Format(format, value);
        }        
    }
}
