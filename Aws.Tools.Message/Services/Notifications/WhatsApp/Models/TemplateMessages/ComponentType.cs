using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TemplateMessages
{
    public class ComponentType
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("parameters")]
        public List<ParameterType> Parameters { get; set; }

        public ComponentType(string type)
        {
            Type = type;
            Parameters = new List<ParameterType>();
        }
    }
}
