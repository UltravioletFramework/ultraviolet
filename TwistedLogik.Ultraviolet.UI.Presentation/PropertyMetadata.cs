using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the method that is invoked when a dependency property's value changes.
    /// </summary>
    /// <typeparam name="TValue">The type of value being changed.</typeparam>
    /// <param name="dependencyObject">The dependency object that raised the event.</param>
    /// <param name="oldValue">The old value of the dependency property.</param>
    /// <param name="newValue">The new value of the dependency property.</param>
    public delegate void PropertyChangedCallback<TValue>(DependencyObject dependencyObject, TValue oldValue, TValue newValue);

    /// <summary>
    /// Represents the method that is invoked to coerce the value of a dependency property.
    /// </summary>
    /// <typeparam name="TValue">The type of value being coerced.</typeparam>
    /// <param name="dependencyObject">The dependency property that raised the event.</param>
    /// <param name="value">The raw value of the dependency property prior to coercion.</param>
    /// <returns>The coerced value of the dependency property.</returns>
    public delegate TValue CoerceValueCallback<TValue>(DependencyObject dependencyObject, TValue value);

    /// <summary>
    /// Represents the metadata for a dependency property.
    /// </summary>
    public abstract class PropertyMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="flags">A collection of <see cref="PropertyMetadataOptions"/> values specifying the dependency property's options.</param>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        /// <param name="coerceValueCallback">A delegate which is invoked to coerce the dependency property's value.</param>
        private PropertyMetadata(PropertyMetadataOptions flags, Delegate propertyChangedCallback, Delegate coerceValueCallback)
        {
            this.flags                   = flags;
            this.propertyChangedCallback = propertyChangedCallback;
            this.coerceValueCallback     = coerceValueCallback;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        public PropertyMetadata()
            : this(PropertyMetadataOptions.None, null, null)
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
        public PropertyMetadata(Delegate propertyChangedCallback)
            : this(PropertyMetadataOptions.None, propertyChangedCallback, null)
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
        public PropertyMetadata(Object defaultValue, Delegate propertyChangedCallback)
            : this(defaultValue, PropertyMetadataOptions.None, propertyChangedCallback, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        /// <param name="coerceValueCallback">A delegate which is invoked to coerce the dependency property's value.</param>
        public PropertyMetadata(Delegate propertyChangedCallback, Delegate coerceValueCallback)
            : this(PropertyMetadataOptions.None, propertyChangedCallback, coerceValueCallback)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="defaultValue">The dependency property's default value.</param>
        /// <param name="flags">A collection of <see cref="PropertyMetadataOptions"/> values specifying the dependency property's options.</param>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        public PropertyMetadata(Object defaultValue, PropertyMetadataOptions flags, Delegate propertyChangedCallback)
            : this(defaultValue, flags, propertyChangedCallback, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="defaultValue">The dependency property's default value.</param>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        /// <param name="coerceValueCallback">A delegate which is invoked to coerce the dependency property's value.</param>
        public PropertyMetadata(Object defaultValue, Delegate propertyChangedCallback, Delegate coerceValueCallback)
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
        public PropertyMetadata(Object defaultValue, PropertyMetadataOptions flags, Delegate propertyChangedCallback, Delegate coerceValueCallback)
            : this(flags, propertyChangedCallback, coerceValueCallback)
        {
            DefaultValue = defaultValue;
        }

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
        /// <param name="dp">The dependency property which was changed.</param>
        /// <param name="oldValue">The dependency property's old value.</param>
        /// <param name="newValue">The dependency property's new value.</param>
        internal void HandleChanged<T>(DependencyObject dobj, DependencyProperty dp, T oldValue, T newValue)
        {
            dobj.OnPropertyChanged<T>(dp, oldValue, newValue);

            if (ChangedCallback != null)
            {
                ((PropertyChangedCallback<T>)ChangedCallback)(dobj, oldValue, newValue);
            }

            dp.RaiseChangeNotification(dobj);

            if (IsMeasureAffecting)
            {
                dobj.OnMeasureAffectingPropertyChanged();
            }

            if (IsArrangeAffecting)
            {
                dobj.OnArrangeAffectingPropertyChanged();
            }

            if (IsVisualBoundsAffecting)
            {
                dobj.OnVisualBoundsAffectingPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the property's default value.
        /// </summary>
        internal Object DefaultValue
        {
            get { return defaultValue; }
            private set
            {
                defaultValue    = value;
                hasDefaultValue = true;
            }
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
        internal Delegate ChangedCallback
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
        /// Gets a value indicating whether this metadata specified a default value.
        /// </summary>
        internal Boolean HasDefaultValue
        {
            get { return hasDefaultValue; }
        }

        /// <summary>
        /// Gets a value indicating whether the dependency property's value is inherited.
        /// </summary>
        internal Boolean IsInherited
        {
            get
            {
                return (flags & PropertyMetadataOptions.Inherits) == PropertyMetadataOptions.Inherits;
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
        /// Gets a value indicating whether this dependency property potentially affects the visual bounds of its object.
        /// </summary>
        internal Boolean IsVisualBoundsAffecting
        {
            get
            {
                return (flags & PropertyMetadataOptions.AffectsVisualBounds) == PropertyMetadataOptions.AffectsVisualBounds;
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

        /// <summary>
        /// Merges this metadata with the specified base metadata.
        /// </summary>
        /// <param name="baseMetadata">The base metadata to merge with this metadata.</param>
        /// <param name="dp">The dependency property to which this metadata belongs.</param>
        protected internal virtual void Merge(PropertyMetadata baseMetadata, DependencyProperty dp)
        {
            flags = flags | baseMetadata.flags;

            if (!hasDefaultValue)
            {
                defaultValue = baseMetadata.defaultValue;
                hasDefaultValue = true;
            }

            if (baseMetadata.propertyChangedCallback != null)
            {
                if (propertyChangedCallback != null)
                {
                    propertyChangedCallback = Delegate.Combine(propertyChangedCallback, baseMetadata.propertyChangedCallback);
                }
                else
                {
                    propertyChangedCallback = baseMetadata.propertyChangedCallback;
                }
            }

            if (coerceValueCallback == null)
            {
                coerceValueCallback = baseMetadata.coerceValueCallback;
            }
        }

        // Property values.
        private Object defaultValue;
        private PropertyMetadataOptions flags;
        private Delegate propertyChangedCallback;
        private Delegate coerceValueCallback;

        // State values.
        private Boolean hasDefaultValue;
    }
}
