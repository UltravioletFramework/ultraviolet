using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ultraviolet.Core.Data;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Represents a method which reads an object from a JSON stream.
    /// </summary>
    /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The object value.</returns>
    public delegate Object ReadJsonDelegate(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer);

    /// <summary>
    /// Represents a method which writes an object to a JSON stream.
    /// </summary>
    /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public delegate void WriteJsonDelegate(JsonWriter writer, Object value, JsonSerializer serializer);

    /// <summary>
    /// Represents a custom JSON converter which supports Ultraviolet's core types.
    /// </summary>
    public class CoreJsonConverter : JsonConverter
    {
        /// <summary>
        /// Initializes the <see cref="CoreJsonConverter"/> type.
        /// </summary>
        static CoreJsonConverter()
        {
            readers = new Dictionary<Type, ReadJsonDelegate>();
            readers[typeof(ResolvedDataObjectReference)] = ReadJson_ResolvedDataObjectReference;
            readers[typeof(ResolvedDataObjectReference?)] = ReadJson_NullableResolvedDataObjectReference;
            readers[typeof(MaskedUInt32)] = ReadJson_MaskedUInt32;
            readers[typeof(MaskedUInt32?)] = ReadJson_NullableMaskedUInt32;
            readers[typeof(MaskedUInt64)] = ReadJson_MaskedUInt64;
            readers[typeof(MaskedUInt64?)] = ReadJson_NullableMaskedUInt64;
            readers[typeof(StringResource)] = ReadJson_StringResource;

            writers = new Dictionary<Type, WriteJsonDelegate>();
            writers[typeof(ResolvedDataObjectReference)] = WriteJson_ResolvedDataObjectReference;
            writers[typeof(ResolvedDataObjectReference?)] = WriteJson_NullableResolvedDataObjectReference;
            writers[typeof(MaskedUInt32)] = WriteJson_MaskedUInt32;
            writers[typeof(MaskedUInt32?)] = WriteJson_NullableMaskedUInt32;
            writers[typeof(MaskedUInt64)] = WriteJson_MaskedUInt64;
            writers[typeof(MaskedUInt64?)] = WriteJson_NullableMaskedUInt64;
            writers[typeof(StringResource)] = WriteJson_StringResource;
        }

        /// <inheritdoc/>
        public override Boolean CanConvert(Type objectType)
        {
            return
                readers.ContainsKey(objectType) && writers.ContainsKey(objectType);
        }
        
        /// <inheritdoc/>
        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            ReadJsonDelegate fn;
            if (readers.TryGetValue(objectType, out fn))
            {
                return fn(reader, objectType, existingValue, serializer);
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Reads a <see cref="ResolvedDataObjectReference"/> value.
        /// </summary>
        private static Object ReadJson_ResolvedDataObjectReference(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            var reference = (String)serializer.Deserialize(reader, typeof(String));
            return DataObjectRegistries.ResolveReference(reference);
        }

        /// <summary>
        /// Reads a <see cref="Nullable{ResolvedDataObjectReference}"/> value.
        /// </summary>
        private static Object ReadJson_NullableResolvedDataObjectReference(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            return ReadJson_ResolvedDataObjectReference(reader, objectType, existingValue, serializer);
        }

        /// <summary>
        /// Reads a <see cref="MaskedUInt32"/> value.
        /// </summary>
        private static Object ReadJson_MaskedUInt32(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var value = UInt32.Parse((String)reader.Value);
                return new MaskedUInt32(value);
            }
            else
            {
                return new MaskedUInt32((UInt32)serializer.Deserialize(reader, typeof(UInt32)));
            }
        }

        /// <summary>
        /// Reads a <see cref="Nullable{MaskedUInt32}"/> value.
        /// </summary>
        private static Object ReadJson_NullableMaskedUInt32(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            return ReadJson_MaskedUInt32(reader, objectType, existingValue, serializer);
        }

        /// <summary>
        /// Reads a <see cref="MaskedUInt64"/> value.
        /// </summary>
        private static Object ReadJson_MaskedUInt64(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var value = UInt64.Parse((String)reader.Value);
                return new MaskedUInt64(value);
            }
            else
            {
                return new MaskedUInt64((UInt64)serializer.Deserialize(reader, typeof(UInt64)));
            }
        }

        /// <summary>
        /// Reads a <see cref="Nullable{MaskedUInt64}"/> value.
        /// </summary>
        private static Object ReadJson_NullableMaskedUInt64(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            return ReadJson_MaskedUInt64(reader, objectType, existingValue, serializer);
        }

        /// <summary>
        /// Reads a <see cref="StringResource"/> value.
        /// </summary>
        private static Object ReadJson_StringResource(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            return new StringResource((String)serializer.Deserialize(reader, typeof(String)));
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
                WriteJsonDelegate fn;
                if (writers.TryGetValue(value.GetType(), out fn))
                {
                    fn(writer, value, serializer);
                    return;
                }
            }       
            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes a <see cref="ResolvedDataObjectReference"/> value.
        /// </summary>
        private static void WriteJson_ResolvedDataObjectReference(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var reference = (ResolvedDataObjectReference)value;
            if (String.IsNullOrEmpty(reference.Source))
            {
                serializer.Serialize(writer, reference.Value.ToString());
            }
            else
            {
                serializer.Serialize(writer, reference.ToString());
            }
        }

        /// <summary>
        /// Writes a <see cref="Nullable{ResolvedDataObjectReference}"/> value.
        /// </summary>
        private static void WriteJson_NullableResolvedDataObjectReference(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            if (value == null || !((ResolvedDataObjectReference?)value).HasValue)
                serializer.Serialize(writer, null);
            else
                WriteJson_ResolvedDataObjectReference(writer, value, serializer);
        }

        /// <summary>
        /// Writes a <see cref="MaskedUInt32"/> value.
        /// </summary>
        private static void WriteJson_MaskedUInt32(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var m = (MaskedUInt32)value;
            serializer.Serialize(writer, m.Value);
        }

        /// <summary>
        /// Writes a <see cref="Nullable{MaskedUInt32}"/> value.
        /// </summary>
        private static void WriteJson_NullableMaskedUInt32(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            if (value == null || !((MaskedUInt32?)value).HasValue)
                serializer.Serialize(writer, null);
            else
                WriteJson_MaskedUInt32(writer, value, serializer);
        }

        /// <summary>
        /// Writes a <see cref="MaskedUInt64"/> value.
        /// </summary>
        private static void WriteJson_MaskedUInt64(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var m = (MaskedUInt64)value;
            serializer.Serialize(writer, m.Value);
        }

        /// <summary>
        /// Writes a <see cref="Nullable{MaskedUInt64}"/> value.
        /// </summary>
        private static void WriteJson_NullableMaskedUInt64(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            if (value == null || !((MaskedUInt64?)value).HasValue)
                serializer.Serialize(writer, null);
            else
                WriteJson_MaskedUInt64(writer, value, serializer);
        }

        /// <summary>
        /// Writes a <see cref="StringResource"/> value.
        /// </summary>
        private static void WriteJson_StringResource(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var s = (StringResource)value;
            if (s.Database != Localization.Strings)
                throw new JsonWriterException(CoreStrings.JsonCannotWriteNonGlobalStringResource);

            serializer.Serialize(writer, s.Key);
        }

        /// <inheritdoc/>
        public override Boolean CanRead => true;

        /// <inheritdoc/>
        public override Boolean CanWrite => true;

        // Registered readers and writers for supported types.
        private static readonly Dictionary<Type, ReadJsonDelegate> readers;
        private static readonly Dictionary<Type, WriteJsonDelegate> writers;
    }
}
