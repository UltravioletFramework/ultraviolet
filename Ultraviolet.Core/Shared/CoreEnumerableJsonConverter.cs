using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Represents a custom JSON converter which supports enumerable types.
    /// </summary>
    public class CoreEnumerableJsonConverter<TItem> : JsonConverter
    {
        /// <inheritdoc/>
        public override Boolean CanConvert(Type objectType)
        {
            return objectType.GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<TItem>));
        }
        
        /// <inheritdoc/>
        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                return token.ToObject(typeof(List<TItem>), serializer);
            }
            else
            {
                return new List<TItem>(new[] { (TItem)token.ToObject(typeof(TItem), serializer) });
            }
        }
        
        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                var enumerable = (IEnumerable<TItem>)value;
                if (enumerable.Count() == 1)
                {
                    serializer.Serialize(writer, enumerable.First());
                }
                else
                {
                    serializer.Serialize(writer, enumerable);
                }
            }
        }

        /// <inheritdoc/>
        public override Boolean CanRead => true;

        /// <inheritdoc/>
        public override Boolean CanWrite => true;
    }
}
