using System;
using Newtonsoft.Json;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents a localizable string resource.
    /// </summary>
    public sealed class StringResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringResource"/> class.
        /// </summary>
        /// <param name="key">The string's localization key.</param>
        public StringResource(String key)
            : this(null, key)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringResource"/> class.
        /// </summary>
        /// <param name="database">The localization database that contains the string resource.</param>
        /// <param name="key">The string's localization key.</param>
        public StringResource(LocalizationDatabase database, String key)
        {
            this.database = database ?? Localization.Strings;
            this.key = key;
        }

        /// <summary>
        /// Implicitly converts a .NET string to a string resource.
        /// </summary>
        /// <param name="key">The <see cref="String"/> to convert.</param>
        /// <returns>The converted <see cref="StringResource"/>.</returns>
        public static implicit operator StringResource(String key)
        {
            return new StringResource(key);
        }

        /// <summary>
        /// Implicitly converts a string resource resource to a .NET string.
        /// </summary>
        /// <param name="resource">The <see cref="StringResource"/> to convert.</param>
        /// <returns>The converted <see cref="String"/>.</returns>
        public static implicit operator String(StringResource resource)
        {
            return (resource == null) ? null : resource.Value;
        }

        /// <summary>
        /// Formats the string with the specified arguments.
        /// </summary>
        /// <param name="args">The arguments with which to format the string.</param>
        /// <returns>The formatted string.</returns>
        public String Format(params Object[] args)
        {
            return String.Format(Value, args);
        }

        /// <summary>
        /// Gets the string resource's associated localization database.
        /// </summary>
        public LocalizationDatabase Database
        {
            get { return database; }
        }

        /// <summary>
        /// Gets the string resource's localization key.
        /// </summary>
        public String Key
        {
            get { return key; }
        }

        /// <summary>
        /// Gets the string resource's localized value.
        /// </summary>
        public String Value
        {
            get 
            {
                if (cachedValue != null && cachedCulture == Localization.CurrentCulture)
                {
                    return cachedValue;
                }
                cachedValue   = database.Get(key);
                cachedCulture = Localization.CurrentCulture;
                return cachedValue;
            }
        }

        // Property values.
        private readonly LocalizationDatabase database;
        private readonly String key;

        // Cached values.
        private String cachedValue;
        private String cachedCulture;
    }
}
