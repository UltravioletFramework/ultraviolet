using System;

namespace TwistedLogik.Ultraviolet.Layout
{
    /// <summary>
    /// Represents the method that is invoked when a dependency property's value changes.
    /// </summary>
    /// <param name="dependencyObject">The dependency object that raised the event.</param>
    public delegate void PropertyChangedCallback(DependencyObject dependencyObject);

    /// <summary>
    /// Represents the method that is invoked to determine a dependency property's default value.
    /// </summary>
    /// <returns>The default value of the dependency property.</returns>
    public delegate Object PropertyDefaultCallback();

    /// <summary>
    /// Represents the metadata for a dependency property.
    /// </summary>
    public class DependencyPropertyMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPropertyMetadata"/> class.
        /// </summary>
        /// <param name="changedCallback">The <see cref="PropertyChangedCallback"/> that is invoked when the property's value changes.</param>
        /// <param name="defaultCallback">The <see cref="PropertyDefaultCallback"/> that is invoked to determine the property's default value.</param>
        public DependencyPropertyMetadata(
            PropertyChangedCallback changedCallback,
            PropertyDefaultCallback defaultCallback)
        {
            this.changedCallback = changedCallback;
            this.defaultCallback = defaultCallback;
        }

        /// <summary>
        /// Represents an empty dependency property metadata object.
        /// </summary>
        public static readonly DependencyPropertyMetadata Empty = new DependencyPropertyMetadata(null, null);

        /// <summary>
        /// Gets the callback that is invoked when the property's value changes.
        /// </summary>
        internal PropertyChangedCallback ChangedCallback
        {
            get { return changedCallback; }
        }

        /// <summary>
        /// Gets the callback that is invoked to determine the property's default value.
        /// </summary>
        internal PropertyDefaultCallback DefaultCallback
        {
            get { return defaultCallback; }
        }

        // Property values.
        private readonly PropertyChangedCallback changedCallback;
        private readonly PropertyDefaultCallback defaultCallback;
    }
}
