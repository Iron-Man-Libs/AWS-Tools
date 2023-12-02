using System.Text.Json.Serialization;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TextMessages
{
    public class TextMessageModel : WhatsAppMessage
    {
        [JsonPropertyName("text")]
        public BodyMessageModel Text { get; set; }

        public TextMessageModel(string to)
        {
            To = to;
        }
    }
}
