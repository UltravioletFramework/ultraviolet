using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace TwistedLogik.Nucleus
{
    /// <summary>
    /// Represents a custom JSON converter which supports Nucleus's core types.
    /// </summary>
    public class NucleusEnumJsonConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override Boolean CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }

        /// <inheritdoc/>
        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String || !objectType.IsDefined(typeof(FlagsAttribute), false))
            {
                return Enum.Parse(objectType, (String)reader.Value);
            }
            else
            {
                var enumNames = (String[])serializer.Deserialize(reader, typeof(String[]));
                var enumValue = 0ul;

                for (int i = 0; i < enumNames.Length; i++)
                {
                    enumValue |= Convert.ToUInt64(Enum.Parse(objectType, enumNames[i]));
                }

                return Enum.ToObject(objectType, enumValue);
            }
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var enumType = value.GetType();
            if (enumType.IsDefined(typeof(FlagsAttribute), false))
            {
                var enumValues = Enum.GetValues(enumType);
                var enumNames = Enum.GetNames(enumType);

                Array.Sort(enumValues, enumNames);

                var inputValue = Convert.ToUInt64(value);

                var outputValue = 0ul;
                var outputNames = new List<String>();

                for (int i = enumValues.Length - 1; i >= 0; i--)
                {
                    var currentValue = Convert.ToUInt64(enumValues.GetValue(i), CultureInfo.InvariantCulture);
                    if (currentValue == 0 || (outputValue & currentValue) == currentValue)
                        continue;

                    if ((inputValue & currentValue) == currentValue)
                    {
                        outputNames.Add((String)enumNames.GetValue(i));
                        outputValue |= currentValue;
                    }
                }

                if (outputValue != inputValue)
                {
                    serializer.Serialize(writer, inputValue.ToString());
                }
                else
                {
                    if (outputNames.Count == 1)
                    {
                        serializer.Serialize(writer, outputNames[0]);
                    }
                    else
                    {
                        serializer.Serialize(writer, outputNames);
                    }
                }
            }
            else
            {
                serializer.Serialize(writer, value.ToString());
            }
        }
        
        /// <inheritdoc/>
        public override Boolean CanRead => true;

        /// <inheritdoc/>
        public override Boolean CanWrite => true;
    }
}
