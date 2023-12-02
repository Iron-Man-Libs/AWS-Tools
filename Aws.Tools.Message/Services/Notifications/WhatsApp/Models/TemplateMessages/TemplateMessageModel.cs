using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TemplateMessages
{
    public class TemplateMessageModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("language")]
        public LanguageModel Language { get; set; } = new LanguageModel();

        [JsonPropertyName("components")]
        public List<ComponentModel> Components { get; set; } = new List<ComponentModel>();
    }
}
