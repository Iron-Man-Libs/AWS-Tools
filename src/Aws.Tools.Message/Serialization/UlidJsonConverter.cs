using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NUlid;

namespace Aws.Tools.Message.Serialization
{
    public sealed class UlidJsonConverter : JsonConverter<Ulid>
    {
        public override Ulid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Expected a string for ULID value.");
            }

            var ulidString = reader.GetString();
            if (!Ulid.TryParse(ulidString, out Ulid ulid))
            {
                throw new JsonException("Invalid ULID format.");
            }

            return ulid;
        }

        public override void Write(Utf8JsonWriter writer, Ulid value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
