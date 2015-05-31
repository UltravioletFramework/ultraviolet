using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the options that can be applied to a dependency property.
    /// </summary>
    [Flags]
    public enum PropertyMetadataOptions
    {
        /// <summary>
        /// No special options.
        /// </summary>
        None = 0,

        /// <summary>
        /// The dependency property's value is inherited from its container.
        /// </summary>
        Inherits = 1,

        /// <summary>
        /// The dependency property's value influences the arrangement of its object.
        /// </summary>
        AffectsArrange = 2,

        /// <summary>
        /// The dependency property's value influences the measurement of its object.
        /// </summary>
        AffectsMeasure = 4,

        /// <summary>
        /// Indicates that dependency properties of type Object should be coerced into strings
        /// if no valid type converter exists for the original type.
        /// </summary>
        CoerceObjectToString = 8,
    }
}
