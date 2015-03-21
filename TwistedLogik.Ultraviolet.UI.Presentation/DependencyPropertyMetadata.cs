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
    /// Represents the method that is invoked to coerce the value of a dependency property.
    /// </summary>
    /// <typeparam name="T">The type of value being coerced.</typeparam>
    /// <param name="dependencyObject">The dependency property that raised the event.</param>
    /// <param name="value">The raw value of the dependency property prior to coercion.</param>
    /// <returns>The coerced value of the dependency property.</returns>
    public delegate T CoerceValueCallback<T>(DependencyObject dependencyObject, T value);

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
            : this(null, null, null, options)
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
            : this(changedCallback, defaultCallback, null, options)
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
            PropertyDefaultCallback defaultCallback, 
            Delegate coerceValueCallback, DependencyPropertyOptions options)
        {
            this.changedCallback     = changedCallback;
            this.defaultCallback     = defaultCallback;
            this.coerceValueCallback = coerceValueCallback;
            this.options             = options;
        }

        /// <summary>
        /// Represents an empty dependency property metadata object.
        /// </summary>
        public static readonly DependencyPropertyMetadata Empty = new DependencyPropertyMetadata(DependencyPropertyOptions.None);

        /// <summary>
        /// Coerces the specified value by invoking the coercion callback associated with this property.
        /// </summary>
        /// <typeparam name="T">The type of value which is being coerced.</typeparam>
        /// <param name="dobj">The dependency object which owns the property.</param>
        /// <param name="value">The raw value of the dependency property to coerce.</param>
        /// <returns>The coerced value.</returns>
        internal T CoerceValue<T>(DependencyObject dobj, T value)
        {
            var callback = coerceValueCallback as CoerceValueCallback<T>;
            if (callback == null)
                return value;

            return callback(dobj, value);
        }

        /// <summary>
        /// Indicates that the value of the dependency property has changed for the specified object.
        /// </summary>
        /// <param name="dobj">The dependency object for which the dependency property value has changed.</param>
        internal void HandleChanged(DependencyObject dobj)
        {
            if (ChangedCallback != null)
            {
                ChangedCallback(dobj);
            }

            if (IsMeasureAffecting)
            {
                dobj.OnMeasureAffectingPropertyChanged();
            }

            if (IsArrangeAffecting)
            {
                dobj.OnArrangeAffectingPropertyChanged();
            }
        }

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
        /// Gets the callback that is invoked to coerce the property's value.
        /// </summary>
        internal Delegate CoerceValueCallback
        {
            get { return coerceValueCallback; }
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

        /// <summary>
        /// Gets a value indicating whether this dependency property potentially affects the arrangement state of its object.
        /// </summary>
        internal Boolean IsArrangeAffecting
        {
            get
            {
                return (options & DependencyPropertyOptions.AffectsArrange) == DependencyPropertyOptions.AffectsArrange;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this dependency property potentially affects the measurement state of its object.
        /// </summary>
        internal Boolean IsMeasureAffecting
        {
            get 
            {
                return (options & DependencyPropertyOptions.AffectsMeasure) == DependencyPropertyOptions.AffectsMeasure;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this dependency property should coerce its values to
        /// strings if a valid type conversion cannot be found.
        /// </summary>
        internal Boolean CoerceObjectToString
        {
            get { return ((options & DependencyPropertyOptions.CoerceObjectToString) == DependencyPropertyOptions.CoerceObjectToString); }
        }

        // Property values.
        private readonly PropertyChangedCallback changedCallback;
        private readonly PropertyDefaultCallback defaultCallback;
        private readonly Delegate coerceValueCallback;
        private readonly DependencyPropertyOptions options;
    }
}
