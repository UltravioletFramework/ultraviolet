using System;
using Newtonsoft.Json;

namespace Ultraviolet.Core.Data
{
    /// <summary>
    /// Represents a JSON converter which uses the <see cref="ObjectResolver"/> class
    /// to parse strings into objects.
    /// </summary>
    public sealed class ObjectResolverJsonConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override Boolean CanConvert(Type objectType) => true;

        /// <inheritdoc/>
        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
                throw new JsonReaderException(CoreStrings.JsonObjectResolverRequiresString.Format(reader.TokenType));

            return ObjectResolver.FromString((String)reader.Value, objectType);
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override Boolean CanRead => true;

        /// <inheritdoc/>
        public override Boolean CanWrite => false;
    }
}
