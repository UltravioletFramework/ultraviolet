using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the method that is invoked when a dependency property's value changes.
    /// </summary>
    /// <param name="dependencyObject">The dependency object that raised the event.</param>
    public delegate void PropertyChangedCallback(DependencyObject dependencyObject);

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
    public class PropertyMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        public PropertyMetadata()
            : this(null, PropertyMetadataOptions.None, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="defaultValue">The dependency property's default value.</param>
        public PropertyMetadata(Object defaultValue)
            : this(defaultValue, PropertyMetadataOptions.None, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        public PropertyMetadata(PropertyChangedCallback propertyChangedCallback)
            : this(null, PropertyMetadataOptions.None, propertyChangedCallback, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="defaultValue">The dependency property's default value.</param>
        /// <param name="flags">A collection of <see cref="PropertyMetadataOptions"/> values specifying the dependency property's options.</param>
        public PropertyMetadata(Object defaultValue, PropertyMetadataOptions flags)
            : this(defaultValue, flags, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="defaultValue">The dependency property's default value.</param>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        public PropertyMetadata(Object defaultValue, PropertyChangedCallback propertyChangedCallback)
            : this(defaultValue, PropertyMetadataOptions.None, propertyChangedCallback, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        /// <param name="coerceValueCallback">A delegate which is invoked to coerce the dependency property's value.</param>
        public PropertyMetadata(PropertyChangedCallback propertyChangedCallback, Delegate coerceValueCallback)
            : this(null, PropertyMetadataOptions.None, propertyChangedCallback, coerceValueCallback)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="defaultValue">The dependency property's default value.</param>
        /// <param name="flags">A collection of <see cref="PropertyMetadataOptions"/> values specifying the dependency property's options.</param>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        public PropertyMetadata(Object defaultValue, PropertyMetadataOptions flags, PropertyChangedCallback propertyChangedCallback)
            : this(defaultValue, flags, propertyChangedCallback, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="defaultValue">The dependency property's default value.</param>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        /// <param name="coerceValueCallback">A delegate which is invoked to coerce the dependency property's value.</param>
        public PropertyMetadata(Object defaultValue, PropertyChangedCallback propertyChangedCallback, Delegate coerceValueCallback)
            : this(defaultValue, PropertyMetadataOptions.None, propertyChangedCallback, coerceValueCallback)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="defaultValue">The dependency property's default value.</param>
        /// <param name="flags">A collection of <see cref="PropertyMetadataOptions"/> values specifying the dependency property's options.</param>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        /// <param name="coerceValueCallback">A delegate which is invoked to coerce the dependency property's value.</param>
        public PropertyMetadata(Object defaultValue, PropertyMetadataOptions flags, PropertyChangedCallback propertyChangedCallback, Delegate coerceValueCallback)
        {
            this.defaultValue            = defaultValue;
            this.flags                   = flags;
            this.propertyChangedCallback = propertyChangedCallback;
            this.coerceValueCallback     = coerceValueCallback;
        }

        /// <summary>
        /// Represents an empty dependency property metadata object.
        /// </summary>
        public static readonly PropertyMetadata Empty = new PropertyMetadata(null, PropertyMetadataOptions.None);

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
        /// Gets the property's default value.
        /// </summary>
        internal Object DefaultValue
        {
            get { return defaultValue; }
        }

        /// <summary>
        /// Gets the dependency property's option flags.
        /// </summary>
        internal PropertyMetadataOptions Flags
        {
            get { return flags; }
        }

        /// <summary>
        /// Gets the callback that is invoked when the property's value changes.
        /// </summary>
        internal PropertyChangedCallback ChangedCallback
        {
            get { return propertyChangedCallback; }
        }

        /// <summary>
        /// Gets the callback that is invoked to coerce the property's value.
        /// </summary>
        internal Delegate CoerceValueCallback
        {
            get { return coerceValueCallback; }
        }

        /// <summary>
        /// Gets a value indicating whether the dependency property's value is inherited.
        /// </summary>
        internal Boolean IsInherited
        {
            get
            {
                return (flags & PropertyMetadataOptions.Inherited) == PropertyMetadataOptions.Inherited;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this dependency property potentially affects the arrangement state of its object.
        /// </summary>
        internal Boolean IsArrangeAffecting
        {
            get
            {
                return (flags & PropertyMetadataOptions.AffectsArrange) == PropertyMetadataOptions.AffectsArrange;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this dependency property potentially affects the measurement state of its object.
        /// </summary>
        internal Boolean IsMeasureAffecting
        {
            get 
            {
                return (flags & PropertyMetadataOptions.AffectsMeasure) == PropertyMetadataOptions.AffectsMeasure;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this dependency property should coerce its values to
        /// strings if a valid type conversion cannot be found.
        /// </summary>
        internal Boolean CoerceObjectToString
        {
            get { return ((flags & PropertyMetadataOptions.CoerceObjectToString) == PropertyMetadataOptions.CoerceObjectToString); }
        }

        // Property values.
        private readonly Object defaultValue;
        private readonly PropertyMetadataOptions flags;
        private readonly PropertyChangedCallback propertyChangedCallback;
        private readonly Delegate coerceValueCallback;
    }
}
