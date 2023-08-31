using System.Text.Json;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Aws.Tools.Message.Serializationn;
using Microsoft.Extensions.Logging;

namespace Aws.Tools.Message.SNS
{
    public class SNSClient : ISNSClient
    {
        private readonly IAmazonSimpleNotificationService _snsClient;

        private readonly ILogger<SNSClient> _logger;

        public SNSClient(IAmazonSimpleNotificationService snsClient, ILogger<SNSClient> logger)
        {
            _snsClient = snsClient;
            _logger = logger;
        }


        public async Task PublishMessageAsync<T>(string topic, T message)
        {
            try
            {
                var request = new PublishRequest
                {
                    TopicArn = topic,
                    Message = JsonSerializer.Serialize<T>(message, new JsonSerializerOptions().Default())
                };

                await _snsClient.PublishAsync(request);

                _logger.LogInformation("MESSAGE_HAS_BEEN_SENT");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "MESSAGE_HAS_NOT_BEEN_SENT");
            }
        }
    }
}
