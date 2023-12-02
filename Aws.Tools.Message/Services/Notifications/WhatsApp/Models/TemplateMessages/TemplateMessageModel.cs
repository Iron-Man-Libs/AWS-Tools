using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TemplateMessages
{
    public class TemplateMessageModel : WhatsAppMessage
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("language")]
        public LanguageType Language { get; set; } = new LanguageType();

        [JsonPropertyName("components")]
        public List<ComponentType> Components { get; set; } = new List<ComponentType>();

        protected TemplateMessageModel(string to)
        {
            To = to;
        }
    }
}
