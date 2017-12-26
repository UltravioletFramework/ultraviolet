using System;
using System.Globalization;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a raw dependency property value plus the culture with which it should be parsed. 
    /// </summary>
    public partial struct DependencyValue : IEquatable<DependencyValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyValue"/> structure.
        /// </summary>
        /// <param name="value">The raw value.</param>
        /// <param name="culture">The culture with which to parse the value.</param>
        public DependencyValue(String value, CultureInfo culture)
        {
            Contract.Require(culture, nameof(culture));

            this.Value = value;
            this.Culture = culture;
        }

        /// <inheritdoc/>
        public override String ToString() => Value;

        /// <summary>
        /// Gets a value indicating whether this is an empty value.
        /// </summary>
        /// <value><see langword="true"/> if this is an empty value; otherwise, <see langword="false"/>.</value>
        public Boolean IsEmpty { get { return String.IsNullOrEmpty(Value); } }

        /// <summary>
        /// Gets the raw value.
        /// </summary>
        /// <value>A <see cref="String"/> that represents the raw value.</value>
        public String Value { get; }

        /// <summary>
        /// Gets the culture with which to parse the value.
        /// </summary>
        /// <value>The <see cref="CultureInfo"/> with which the <see cref="Value"/> property should be parsed.</value>
        public CultureInfo Culture { get; }		
    }
}
