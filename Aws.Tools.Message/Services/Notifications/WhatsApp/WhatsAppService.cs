using Aws.Tools.Message.Serialization;
using Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TemplateMessages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WhatsAppService> _logger;

        public WhatsAppService(IHttpClientFactory httpClientFactory, ILogger<WhatsAppService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("WhatsApp");
            _logger = logger;
        }

        public async Task SendWhatsAppMessage(WhatsAppTemplateMessageModel message)
        {
            _logger.LogInformation($"Sending message to {message.To}");

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("messages", message, new JsonSerializerOptions().SnakeCase());
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError($"WhatsApp Error: {response.StatusCode} | ErrorMessage: {errorMessage}");
                return;
            }

            _logger.LogInformation($"Message sent to {message.To}");
        }
    }
}
