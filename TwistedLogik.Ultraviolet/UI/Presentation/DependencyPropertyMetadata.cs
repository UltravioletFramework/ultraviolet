using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
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
        /// <param name="options">A collection of <see cref="DependencyPropertyOptions"/> values specifying the dependency property's options.</param>
        public DependencyPropertyMetadata(DependencyPropertyOptions options)
            : this(null, null, options)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPropertyMetadata"/> class.
        /// </summary>
        /// <param name="changedCallback">The <see cref="PropertyChangedCallback"/> that is invoked when the property's value changes.</param>
        /// <param name="defaultCallback">The <see cref="PropertyDefaultCallback"/> that is invoked to determine the property's default value.</param>
        /// <param name="options">A collection of <see cref="DependencyPropertyOptions"/> values specifying the dependency property's options.</param>
        public DependencyPropertyMetadata(
            PropertyChangedCallback changedCallback,
            PropertyDefaultCallback defaultCallback, DependencyPropertyOptions options)
        {
            this.changedCallback = changedCallback;
            this.defaultCallback = defaultCallback;
            this.options         = options;
        }

        /// <summary>
        /// Represents an empty dependency property metadata object.
        /// </summary>
        public static readonly DependencyPropertyMetadata Empty = new DependencyPropertyMetadata(DependencyPropertyOptions.None);

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

        /// <summary>
        /// Gets the dependency property's options.
        /// </summary>
        internal DependencyPropertyOptions Options
        {
            get { return options; }
        }

        /// <summary>
        /// Gets a value indicating whether the dependency property's value is inherited.
        /// </summary>
        internal Boolean IsInherited
        {
            get
            {
                return (options & DependencyPropertyOptions.Inherited) == DependencyPropertyOptions.Inherited;
            }
        }

        // Property values.
        private readonly PropertyChangedCallback changedCallback;
        private readonly PropertyDefaultCallback defaultCallback;
        private readonly DependencyPropertyOptions options;
    }
}
