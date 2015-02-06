using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a value which is bound to a dependency property.
    /// </summary>
    /// <typeparam name="TDependency">The type of the dependency property.</typeparam>
    internal interface IDependencyBoundValue<TDependency>
    {
        /// <summary>
        /// Checks to see whether the underlying bound value has changed.
        /// </summary>
        /// <returns><c>true</c> if the value has changed; otherwise, <c>false</c>.</returns>
        Boolean CheckHasChanged();

        /// <summary>
        /// Gets the cached bound value.
        /// </summary>
        /// <returns>The value that was retrieved.</returns>
        TDependency Get();

        /// <summary>
        /// Gets the underlying bound value, ignoring any cached value.
        /// </summary>
        /// <returns>The value that was retrieved.</returns>
        TDependency GetFresh();

        /// <summary>
        /// Sets the format string used to convert the bound value to a string.
        /// </summary>
        /// <param name="formatString">The format string used to convert the bound value to a string.</param>
        void SetFormatString(String formatString);

        /// <summary>
        /// Sets the underlying bound value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        void Set(TDependency value);

        /// <summary>
        /// Gets a value indicating whether the bound property can be read.
        /// </summary>
        Boolean IsReadable { get; }

        /// <summary>
        /// Gets a value indicating whether the bound property can be written.
        /// </summary>
        Boolean IsWritable { get; }
    }
}
