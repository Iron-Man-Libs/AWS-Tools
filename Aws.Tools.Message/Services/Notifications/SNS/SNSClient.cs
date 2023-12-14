using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Aws.Tools.Message.Serialization;
using Aws.Tools.Message.Services.Notifications.SES.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Notifications.SNS
{
    public class SNSClient : ISNSClient
    {
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly ILogger<SNSClient> _logger;
        private readonly string _snsTopicArnPrefix;

        public SNSClient(IAmazonSimpleNotificationService snsClient, ILogger<SNSClient> logger, IOptions<MessageConfiguration> config)
        {
            _ = config.Value.Region ?? config.Value.AccountId ?? throw new ArgumentNullException("MessageConfigurationPath is null");

            _snsTopicArnPrefix = $"arn:aws:sns:{config.Value.Region}:{config.Value.AccountId}:";
            _snsClient = snsClient;
            _logger = logger;
        }

        private string ConstructTopicArn(string topicName)
        {
            return _snsTopicArnPrefix + topicName;
        }

        public async Task PublishMessageAsync<T>(string topicName, T message, string entityName = null, string apiName = null)
        {
            try
            {
                PublishRequest publishRequest = new()
                {
                    TopicArn = ConstructTopicArn(topicName),
                    Message = JsonSerializer.Serialize(message, new JsonSerializerOptions().Default()),
                    MessageAttributes = ConstructMessageAttributes(entityName, apiName)
                };

                _ = await _snsClient.PublishAsync(publishRequest);
                _logger.LogInformation($"Message sent to {topicName} with Entity: {entityName}, API: {apiName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to SNS");
                throw;
            }
        }

        private Dictionary<string, MessageAttributeValue> ConstructMessageAttributes(string entityName, string apiName)
        {
            Dictionary<string, MessageAttributeValue> attributes = new();
            if (!string.IsNullOrEmpty(entityName))
            {
                attributes.Add("EntityName", new MessageAttributeValue { DataType = "String", StringValue = entityName });
            }
            attributes.Add("ApiName", new MessageAttributeValue { DataType = "String", StringValue = apiName ?? "Default" });
            return attributes;
        }

        public async Task PublishSMSMessageAsync(string phoneNumber, string message)
        {
            try
            {
                PublishRequest request = new()
                {
                    PhoneNumber = phoneNumber,
                    Message = message
                };

                _ = await _snsClient.PublishAsync(request);
                _logger.LogInformation($"SMS sent to {phoneNumber}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS");
                throw;
            }
        }
    }
}
