using System.Text.Json;
using System.Text.Json.Serialization;
using Aws.Tools.Message.Serialization;

namespace Aws.Tools.Message.Serializationn
{
    public static class ApiJsonSerializerOptions
    {
        public static JsonSerializerOptions Default(this JsonSerializerOptions options)
        {
            options.PropertyNameCaseInsensitive = true;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Converters.Add(new UlidJsonConverter());
            options.Converters.Add(new JsonStringEnumConverter());

            return options;
        }
    }
}
