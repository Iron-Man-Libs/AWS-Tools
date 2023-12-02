using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Aws.Tools.Message.Serialization;
using Aws.Tools.Message.Services.Messages.Configuration;
using Aws.Tools.Message.Services.Messages.SES;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly ILogger<NotificationService> _logger;
        private readonly string _baseSNSARN;

        public NotificationService(IAmazonSimpleNotificationService snsClient, ILogger<NotificationService> logger, IOptions<MessageConfiguration> messageConfiguration)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "MESSAGE_HAS_NOT_BEEN_SENT");
            }
        }

        public async Task PublishSMSMessageAsync(string number, string message)
        {
            var request = new PublishRequest
            {
                Message = message,
                PhoneNumber = number
            };

            await _snsClient.PublishAsync(request);
        }

        public async Task SendEmailAsync(SESMessage message)
        {
            using AmazonSimpleEmailServiceClient client = new(RegionEndpoint.USEast1);
            SendEmailRequest request = new()
            {
                Source = message.SenderAddress,
                Destination = new Destination
                {
                    ToAddresses = message.ReceiversAddress
                },
                Message = new Amazon.SimpleEmail.Model.Message
                {
                    Subject = new Content(message.Subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = message.Body
                        }
                    }
                }
            };

            _ = await client.SendEmailAsync(request);
        }

        public async Task SendEmailAsync<T>(SESTemplateMessage<T> templateMessage)
        {
            using AmazonSimpleEmailServiceClient client = new(RegionEndpoint.USEast1);
            SendTemplatedEmailRequest request = new()
            {
                Source = templateMessage.SenderAddress,
                Destination = new Destination
                {
                    ToAddresses = templateMessage.ReceiversAddress
                },
                Template = templateMessage.TemplateName,
                TemplateData = JsonSerializer.Serialize(templateMessage.TemplateModel, new JsonSerializerOptions().Default())
            };

            _ = await client.SendTemplatedEmailAsync(request);
        }
    }
}
