using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Aws.Tools.Message.Serialization;
using Aws.Tools.Message.Services.Messages.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Messages.SNS
{
    public class SNSClient : ISNSClient
    {
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly ILogger<SNSClient> _logger;
        private readonly string _baseSNSARN;

        public SNSClient(IAmazonSimpleNotificationService snsClient, ILogger<SNSClient> logger, IOptions<MessageConfiguration> messageConfiguration)
        {
            string region = messageConfiguration.Value.Region;
            string accountId = messageConfiguration.Value.AccountId;

            _baseSNSARN = $"arn:aws:sns:{region}:{accountId}:";
            _snsClient = snsClient;
            _logger = logger;
        }

        private string GetTopicArn(string topicName) => _baseSNSARN + topicName;

        public async Task PublishMessageAsync<T>(string topicName, T message, string entityName = null, string apiName = null)
        {
            Dictionary<string, MessageAttributeValue> attributes = new();

            if (!string.IsNullOrEmpty(entityName))
                attributes.Add("EntityName", new MessageAttributeValue() { DataType = "String", StringValue = entityName });

            attributes.Add("ApiName", new MessageAttributeValue() { DataType = "String", StringValue = apiName ?? "Default" });

            try
            {
                PublishRequest request = new()
                {
                    TopicArn = GetTopicArn(topicName),
                    Message = JsonSerializer.Serialize(message, new JsonSerializerOptions().Default()),
                    MessageAttributes = attributes ?? null
                };

                _ = await _snsClient.PublishAsync(request);

                _logger.LogInformation("MESSAGE_HAS_BEEN_SENT TO: TopicName: {topicName} | EntityName: {entityName} | ApiName: {apiName}", topicName, entityName, apiName);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "MESSAGE_HAS_NOT_BEEN_SENT");
            }
        }
    }
}
