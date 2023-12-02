using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp.Models
{
    public class WhatsAppMessage
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; } = "whatsapp";

        [JsonPropertyName("recipient_type")]
        public string RecipientType { get; set; } = "individual";

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("type")]
        public WhatsAppMessageType Type { get; set; }

        public StringContent Serialize()
        {
            string jsonString = JsonSerializer.Serialize(this);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            return content;
        }
    }

    public enum WhatsAppMessageType
    {
        text,
        template
    }
}
