using System.Text.Json.Serialization;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TemplateMessages
{
    public class ParameterType
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        public ParameterType(string type, string text)
        {
            Type = type;
            Text = text;
        }
    }
}
