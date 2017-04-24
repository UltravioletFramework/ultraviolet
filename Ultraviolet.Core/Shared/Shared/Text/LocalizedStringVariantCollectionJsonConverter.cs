using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents a custom JSON converter which supports collections of localized string variants.
    /// </summary>
    public sealed class LocalizedStringVariantCollectionJsonConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override Boolean CanConvert(Type objectType)
        {
            return objectType == typeof(IDictionary<String, LocalizedStringVariantCollectionDescription>);
        }

        /// <inheritdoc/>
        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var token = JToken.Load(reader);
                return new LocalizedStringVariantCollectionDescription()
                {
                    Items = new List<LocalizedStringVariantDescription>()
                    {
                        new LocalizedStringVariantDescription()
                        {
                            Text = token.ToObject<String>()
                        }
                    }
                };
            }

            if (reader.TokenType == JsonToken.StartArray)
            {
                var token = JToken.Load(reader);
                return new LocalizedStringVariantCollectionDescription()
                {
                    Items = token.ToObject<List<LocalizedStringVariantDescription>>()
                };
            }

            if (reader.TokenType == JsonToken.StartObject)
            {
                var description = existingValue ?? new LocalizedStringVariantCollectionDescription();
                serializer.Populate(reader, description);
                return description;
            }

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            throw new JsonReaderException(CoreStrings.JsonCannotReadStringVariantCollection);
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        /// <inheritdoc/>
        public override Boolean CanRead => true;

        /// <inheritdoc/>
        public override Boolean CanWrite => true;
    }
}
