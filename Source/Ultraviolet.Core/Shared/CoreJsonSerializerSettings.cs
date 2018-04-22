using Newtonsoft.Json;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Represents a standard set of JSON serializer settings for loading Ultraviolet Core objects.
    /// </summary>
    public class CoreJsonSerializerSettings : JsonSerializerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreJsonSerializerSettings"/> class.
        /// </summary>
        public CoreJsonSerializerSettings()
        {
            SetContractResolver();
            SetConverters();
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="CoreJsonSerializerSettings"/> class.
        /// </summary>
        public static CoreJsonSerializerSettings Instance { get; } = new CoreJsonSerializerSettings();

        /// <summary>
        /// Sets the contract resolver specified by this settings object.
        /// </summary>
        protected virtual void SetContractResolver()
        {
            ContractResolver = new CoreJsonContractResolver();
        }

        /// <summary>
        /// Sets the converters specified by this settings object.
        /// </summary>
        protected virtual void SetConverters()
        {
            Converters.Add(new CoreJsonConverter());
        }
    }
}
