using System;

namespace TwistedLogik.Nucleus.Text
{
    /// <summary>
    /// Represents a localizable string resource.
    /// </summary>
    public sealed class StringResource
    {
        /// <summary>
        /// Initializes a new instance of the StringResource class.
        /// </summary>
        /// <param name="key">The string's localization key.</param>
        public StringResource(String key)
            : this(null, key)
        {

        }

        /// <summary>
        /// Initializes a new instance of the StringResource class.
        /// </summary>
        /// <param name="database">The localization database that contains the string resource.</param>
        /// <param name="key">The string's localization key.</param>
        public StringResource(LocalizationDatabase database, String key)
        {
            this.database = database ?? Localization.Strings;
            this.key = key;
        }

        /// <summary>
        /// Implicitly converts a .NET string to an Ultraviolet string resource.
        /// </summary>
        public static implicit operator StringResource(String key)
        {
            return new StringResource(key);
        }

        /// <summary>
        /// Implicitly converts an Ultraviolet string resource to a .NET string.
        /// </summary>
        public static implicit operator String(StringResource msg)
        {
            return (msg == null) ? null : msg.Value;
        }

        /// <summary>
        /// Formats the exception message with the specified arguments.
        /// </summary>
        /// <param name="args">The arguments with which to format the exception message.</param>
        /// <returns>The formatted exception message.</returns>
        public String Format(params Object[] args)
        {
            return String.Format(Value, args);
        }

        /// <summary>
        /// The exception message's localized value.
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
