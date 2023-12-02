using System.Text.Json.Serialization;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TemplateMessages
{
    public class WhatsAppTemplateMessageModel : WhatsAppMessage
    {
        [JsonPropertyName("template")]
        public TemplateMessageModel Template { get; set; }

        public WhatsAppTemplateMessageModel(string to) : base(to)
        {
            Template = new();
        }
    }
}
