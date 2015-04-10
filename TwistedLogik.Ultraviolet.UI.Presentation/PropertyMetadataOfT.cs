using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the metadata for a dependency property.
    /// </summary>
    public class PropertyMetadata<T> : PropertyMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata{T}"/> class.
        /// </summary>
        public PropertyMetadata()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata{T}"/> class.
        /// </summary>
        /// <param name="defaultValue">The dependency property's default value.</param>
        public PropertyMetadata(Object defaultValue)
            : base(defaultValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata{T}"/> class.
        /// </summary>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        public PropertyMetadata(PropertyChangedCallback<T> propertyChangedCallback)
            : base(propertyChangedCallback)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata{T}"/> class.
        /// </summary>
        /// <param name="defaultValue">The dependency property's default value.</param>
        /// <param name="flags">A collection of <see cref="PropertyMetadataOptions"/> values specifying the dependency property's options.</param>
        public PropertyMetadata(Object defaultValue, PropertyMetadataOptions flags)
            : base(defaultValue, flags)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata{T}"/> class.
        /// </summary>
        /// <param name="defaultValue">The dependency property's default value.</param>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        public PropertyMetadata(Object defaultValue, PropertyChangedCallback<T> propertyChangedCallback)
            : base(defaultValue, propertyChangedCallback)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata{T}"/> class.
        /// </summary>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        /// <param name="coerceValueCallback">A delegate which is invoked to coerce the dependency property's value.</param>
        public PropertyMetadata(PropertyChangedCallback<T> propertyChangedCallback, CoerceValueCallback<T> coerceValueCallback)
            : base(propertyChangedCallback, coerceValueCallback)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata{T}"/> class.
        /// </summary>
        /// <param name="defaultValue">The dependency property's default value.</param>
        /// <param name="flags">A collection of <see cref="PropertyMetadataOptions"/> values specifying the dependency property's options.</param>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        public PropertyMetadata(Object defaultValue, PropertyMetadataOptions flags, PropertyChangedCallback<T> propertyChangedCallback)
            : base(defaultValue, flags, propertyChangedCallback)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata{T}"/> class.
        /// </summary>
        /// <param name="defaultValue">The dependency property's default value.</param>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        /// <param name="coerceValueCallback">A delegate which is invoked to coerce the dependency property's value.</param>
        public PropertyMetadata(Object defaultValue, PropertyChangedCallback<T> propertyChangedCallback, CoerceValueCallback<T> coerceValueCallback)
            : base(defaultValue, propertyChangedCallback, coerceValueCallback)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata{T}"/> class.
        /// </summary>
        /// <param name="defaultValue">The dependency property's default value.</param>
        /// <param name="flags">A collection of <see cref="PropertyMetadataOptions"/> values specifying the dependency property's options.</param>
        /// <param name="propertyChangedCallback">A delegate which is invoked when the dependency property's value changes.</param>
        /// <param name="coerceValueCallback">A delegate which is invoked to coerce the dependency property's value.</param>
        public PropertyMetadata(Object defaultValue, PropertyMetadataOptions flags, PropertyChangedCallback<T> propertyChangedCallback, CoerceValueCallback<T> coerceValueCallback)
            : base(defaultValue, flags, propertyChangedCallback, coerceValueCallback)
        {

        }

        /// <summary>
        /// Represents an empty dependency property metadata object.
        /// </summary>
        public static readonly PropertyMetadata<T> Empty = new PropertyMetadata<T>(null, PropertyMetadataOptions.None);
    }
}
