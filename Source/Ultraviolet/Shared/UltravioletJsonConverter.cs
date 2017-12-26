using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a custom JSON converter which supports Ultraviolet's core types.
    /// </summary>
    public class UltravioletJsonConverter : CoreJsonConverter
    {
        /// <summary>
        /// Initializes the <see cref="UltravioletJsonConverter"/> type.
        /// </summary>
        static UltravioletJsonConverter()
        {
            readers = new Dictionary<Type, ReadJsonDelegate>();
            readers[typeof(Radians)] = ReadJson_Radians;
            readers[typeof(Radians?)] = ReadJson_NullableRadians;
            readers[typeof(Color)] = ReadJson_Color;
            readers[typeof(Color?)] = ReadJson_NullableColor;
            readers[typeof(Matrix)] = ReadJson_Matrix;
            readers[typeof(Matrix?)] = ReadJson_NullableMatrix;
            readers[typeof(AssetID)] = ReadJson_AssetID;
            readers[typeof(AssetID?)] = ReadJson_NullableAssetID;
            readers[typeof(SpriteAnimationID)] = ReadJson_SpriteAnimationID;
            readers[typeof(SpriteAnimationID?)] = ReadJson_NullableSpriteAnimationID;

            writers = new Dictionary<Type, WriteJsonDelegate>();
            writers[typeof(Radians)] = WriteJson_Radians;
            writers[typeof(Radians?)] = WriteJson_NullableRadians;
            writers[typeof(Color)] = WriteJson_Color;
            writers[typeof(Color?)] = WriteJson_NullableColor;
            writers[typeof(Matrix)] = WriteJson_Matrix;
            writers[typeof(Matrix?)] = WriteJson_NullableMatrix;
            writers[typeof(AssetID)] = WriteJson_AssetID;
            writers[typeof(AssetID?)] = WriteJson_NullableAssetID;
            writers[typeof(SpriteAnimationID)] = WriteJson_SpriteAnimationID;
            writers[typeof(SpriteAnimationID?)] = WriteJson_NullableSpriteAnimationID;
        }

        /// <inheritdoc/>
        public override Boolean CanConvert(Type objectType)
        {
            return
                (readers.ContainsKey(objectType) && writers.ContainsKey(objectType)) || base.CanConvert(objectType);
        }

        /// <inheritdoc/>
        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            ReadJsonDelegate fn;
            if (readers.TryGetValue(objectType, out fn))
            {
                return fn(reader, objectType, existingValue, serializer);
            }            
            return base.ReadJson(reader, objectType, existingValue, serializer);
        }

        /// <summary>
        /// Reads a <see cref="Radians"/> value.
        /// </summary>
        private static Object ReadJson_Radians(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            return new Radians((Single)serializer.Deserialize(reader, typeof(Single)));
        }

        /// <summary>
        /// Reads a <see cref="Nullable{Radians}"/> value.
        /// </summary>
        private static Object ReadJson_NullableRadians(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            return ReadJson_Radians(reader, objectType, existingValue, serializer);
        }

        /// <summary>
        /// Reads a <see cref="Color"/> value.
        /// </summary>
        private static Object ReadJson_Color(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            var values = (Int32[])serializer.Deserialize(reader, typeof(Int32[]));
            if (values.Length != 3 && values.Length != 4)
                throw new JsonReaderException(CoreStrings.JsonIncorrectArrayLengthForType.Format(objectType.Name));

            var r = values[0];
            var g = values[1];
            var b = values[2];
            var a = values.Length == 4 ? values[3] : (Int32)Byte.MaxValue;

            return new Color(r, g, b, a);
        }

        /// <summary>
        /// Reads a <see cref="Nullable{Color}"/> value.
        /// </summary>
        private static Object ReadJson_NullableColor(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            return ReadJson_Color(reader, objectType, existingValue, serializer);
        }

        /// <summary>
        /// Reads a <see cref="Matrix"/> value.
        /// </summary>
        private static Object ReadJson_Matrix(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            var values = (Single[])serializer.Deserialize(reader, typeof(Single[]));
            if (values.Length != 16)
                throw new JsonReaderException(CoreStrings.JsonIncorrectArrayLengthForType.Format(objectType.Name));

            return new Matrix(
                values[00], values[01], values[02], values[03],
                values[04], values[05], values[06], values[07],
                values[08], values[09], values[10], values[11],
                values[12], values[13], values[14], values[15]);
        }

        /// <summary>
        /// Reads a <see cref="Matrix"/> value.
        /// </summary>
        private static Object ReadJson_NullableMatrix(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            return ReadJson_Matrix(reader, objectType, existingValue, serializer);
        }

        /// <summary>
        /// Reads a <see cref="AssetID"/> value.
        /// </summary>
        private static Object ReadJson_AssetID(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            var value = (String)serializer.Deserialize(reader, typeof(String));
            return AssetID.Parse(value);
        }

        /// <summary>
        /// Reads a <see cref="Nullable{AssetID}"/> value.
        /// </summary>
        private static Object ReadJson_NullableAssetID(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            return ReadJson_AssetID(reader, objectType, existingValue, serializer);
        }

        /// <summary>
        /// Reads a <see cref="SpriteAnimationID"/> value.
        /// </summary>
        private static Object ReadJson_SpriteAnimationID(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            var value = (String)serializer.Deserialize(reader, typeof(String));
            return SpriteAnimationID.Parse(value);
        }

        /// <summary>
        /// Reads a <see cref="Nullable{SpriteAnimationID}"/> value.
        /// </summary>
        private static Object ReadJson_NullableSpriteAnimationID(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            return ReadJson_SpriteAnimationID(reader, objectType, existingValue, serializer);
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            if (value != null)
            {
                WriteJsonDelegate fn;
                if (writers.TryGetValue(value.GetType(), out fn))
                {
                    fn(writer, value, serializer);
                    return;
                }
            }
            base.WriteJson(writer, value, serializer);
        }

        /// <summary>
        /// Writes a <see cref="Radians"/> value.
        /// </summary>
        private static void WriteJson_Radians(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var r = (Radians)value;
            serializer.Serialize(writer, r.Value);
        }

        /// <summary>
        /// Writes a <see cref="Nullable{Radians}"/> value.
        /// </summary>
        private static void WriteJson_NullableRadians(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            if (value == null || !((Radians?)value).HasValue)
                serializer.Serialize(writer, null);
            else
                WriteJson_Radians(writer, value, serializer);
        }

        /// <summary>
        /// Writes a <see cref="Color"/> value.
        /// </summary>
        private static void WriteJson_Color(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var c = (Color)value;
            serializer.Serialize(writer, new Int32[] { c.R, c.G, c.B, c.A });
        }

        /// <summary>
        /// Writes a <see cref="Nullable{Color}"/> value.
        /// </summary>
        private static void WriteJson_NullableColor(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            if (value == null || !((Color?)value).HasValue)
                serializer.Serialize(writer, null);
            else
                WriteJson_Color(writer, value, serializer);
        }

        /// <summary>
        /// Writes a <see cref="Matrix"/> value.
        /// </summary>
        private static void WriteJson_Matrix(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var m = (Matrix)value;
            serializer.Serialize(writer, new Single[] 
            {
                m.M11, m.M12, m.M13, m.M14,
                m.M21, m.M22, m.M23, m.M24,
                m.M31, m.M32, m.M33, m.M34,
                m.M41, m.M42, m.M43, m.M44
            });
        }

        /// <summary>
        /// Writes a <see cref="Nullable{Matrix}"/> value.
        /// </summary>
        private static void WriteJson_NullableMatrix(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            if (value == null || !((Matrix?)value).HasValue)
                serializer.Serialize(writer, null);
            else
                WriteJson_Matrix(writer, value, serializer);
        }

        /// <summary>
        /// Writes a <see cref="AssetID"/> value.
        /// </summary>
        private static void WriteJson_AssetID(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var id = (AssetID)value;
            serializer.Serialize(writer, id.ToString());
        }

        /// <summary>
        /// Writes a <see cref="Nullable{AssetID}"/> value.
        /// </summary>
        private static void WriteJson_NullableAssetID(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            if (value == null || !((AssetID?)value).HasValue)
                serializer.Serialize(writer, null);
            else
                WriteJson_AssetID(writer, value, serializer);
        }

        /// <summary>
        /// Writes a <see cref="SpriteAnimationID"/> value.
        /// </summary>
        private static void WriteJson_SpriteAnimationID(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var id = (SpriteAnimationID)value;
            serializer.Serialize(writer, id.ToString());
        }

        /// <summary>
        /// Writes a <see cref="Nullable{SpriteAnimationID}"/> value.
        /// </summary>
        private static void WriteJson_NullableSpriteAnimationID(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            if (value == null || !((SpriteAnimationID?)value).HasValue)
                serializer.Serialize(writer, null);
            else
                WriteJson_SpriteAnimationID(writer, value, serializer);
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
