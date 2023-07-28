using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a standard set of JSON serializer settings for loading Ultraviolet objects.
    /// </summary>
    public class UltravioletJsonSerializerSettings : CoreJsonSerializerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletJsonSerializerSettings"/> class.
        /// </summary>
        public UltravioletJsonSerializerSettings()
        { }

        /// <summary>
        /// Gets the singleton instance of the <see cref="UltravioletJsonSerializerSettings"/> class.
        /// </summary>
        public new static UltravioletJsonSerializerSettings Instance { get; } = new UltravioletJsonSerializerSettings();

        /// <inheritdoc/>
        protected override void SetContractResolver()
        {
            ContractResolver = new UltravioletJsonContractResolver();
        }

        /// <inheritdoc/>
        protected override void SetConverters()
        {
            Converters.Add(new UltravioletJsonConverter());
        }
    }
}
