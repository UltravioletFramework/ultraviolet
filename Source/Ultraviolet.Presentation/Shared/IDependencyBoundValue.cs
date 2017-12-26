using System;

namespace Ultraviolet.Presentation
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
        /// <returns><see langword="true"/> if the value has changed; otherwise, <see langword="false"/>.</returns>
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
        /// Called when the data source attached to the object which owns this value changes.
        /// </summary>
        /// <param name="dataSource">The new value of the <see cref="UIElement.DependencyDataSource"/> property.</param>
        void HandleDataSourceChanged(Object dataSource);

        /// <summary>
        /// Invalidates the cached display value for the dependency property. This will cause
        /// the interface to display the property's actual value, rather than the value most recently
        /// entered by the user (which may be different if coercion is involved).
        /// </summary>
        void InvalidateDisplayCache();

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

        /// <summary>
        /// Gets a value indicating whether this binding should suppress digestion, even if it would
        /// otherwise need to be part of the digest cycle.
        /// </summary>
        Boolean SuppressDigestForDataBinding { get; }
    }
}
