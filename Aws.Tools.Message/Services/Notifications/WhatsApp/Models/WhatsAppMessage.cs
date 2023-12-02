using System.Text.Json.Serialization;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp.Models
{
    public abstract class WhatsAppMessage
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; } = "whatsapp";

        [JsonPropertyName("recipient_type")]
        public string RecipientType { get; set; } = "individual";

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("type")]
        public WhatsAppMessageType Type { get; set; }

        public WhatsAppMessage(string to)
        {
            To = to;
        }
    }

    public enum WhatsAppMessageType
    {
        text,
        template
    }
}
