using Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TemplateMessages;
using Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TextMessages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
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

        public async Task SendTemplateMessage(TemplateMessageModel message)
        {
            _logger.LogInformation($"Sending message to {message.To}");

            var response = await _httpClient.PostAsJsonAsync("messages", message.Serialize());
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError($"WhatsApp Error: {response.StatusCode} | ErrorMessage: {errorMessage}");
                return;
            }

            _logger.LogInformation($"Message sent to {message.To}");
        }

        public async Task SendTextMessage(TextMessageModel message)
        {
            _logger.LogInformation($"Sending message to {message.To}");

            var response = await _httpClient.PostAsJsonAsync("messages", message.Serialize());
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"WhatsApp Error: {response.StatusCode}");
                return;
            }

            _logger.LogInformation($"Message sent to {message.To}");
        }
    }
}
