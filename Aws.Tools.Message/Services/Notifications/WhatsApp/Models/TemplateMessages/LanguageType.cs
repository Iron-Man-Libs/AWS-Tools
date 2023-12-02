using System.Text.Json.Serialization;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TemplateMessages
{
    public class LanguageType
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = "pt_BR";
    }
}
