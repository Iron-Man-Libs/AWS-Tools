using System.Text.Json.Serialization;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TextMessages
{
    public class BodyMessageModel
    {
        [JsonPropertyName("preview_url")]
        public bool PreviewUrl { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
