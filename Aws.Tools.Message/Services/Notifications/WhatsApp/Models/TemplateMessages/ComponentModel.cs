using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TemplateMessages
{
    public class ComponentModel
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("sub_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SubType { get; set; } = null;

        [JsonPropertyName("index")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Index { get; set; } = null;

        [JsonPropertyName("parameters")]
        public List<ParameterModel> Parameters { get; set; }

        public ComponentModel(string type, string index = null, string subType = null)
        {
            Type = type;
            Index = index;
            SubType = subType;
            Parameters = new List<ParameterModel>();
        }
    }
}
