using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Aws.Tools.Message.Serialization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Messages.SNS
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

        public async Task PublishMessageAsync<T>(string topic, T message, string entityName = null, string apiName = null)
        {
            Dictionary<string, MessageAttributeValue> attributes = new();

            if (!string.IsNullOrEmpty(entityName))
            {
                attributes.Add("EntityName", new MessageAttributeValue() { DataType = "String", StringValue = entityName });
            }

            if (!string.IsNullOrEmpty(apiName))
            {
                attributes.Add("ApiName", new MessageAttributeValue() { DataType = "String", StringValue = apiName });
            }

            try
            {
                PublishRequest request = new()
                {
                    TopicArn = topic,
                    Message = JsonSerializer.Serialize(message, new JsonSerializerOptions().Default()),
                    MessageAttributes = attributes ?? null
                };

                _ = await _snsClient.PublishAsync(request);

                _logger.LogInformation("MESSAGE_HAS_BEEN_SENT");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "MESSAGE_HAS_NOT_BEEN_SENT");
            }
        }
    }
}
