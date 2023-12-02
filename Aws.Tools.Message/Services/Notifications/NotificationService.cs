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
        private readonly string _snsTopicArnPrefix;

        public NotificationService(IAmazonSimpleNotificationService snsClient, ILogger<NotificationService> logger, IOptions<MessageConfiguration> config)
        {
            _snsClient = snsClient;
            _logger = logger;
            _snsTopicArnPrefix = $"arn:aws:sns:{config.Value.Region}:{config.Value.AccountId}:";
        }

        private string ConstructTopicArn(string topicName) => _snsTopicArnPrefix + topicName;

        public async Task PublishMessageAsync<T>(string topicName, T message, string entityName = null, string apiName = null)
        {
            try
            {
                var publishRequest = new PublishRequest
                {
                    TopicArn = ConstructTopicArn(topicName),
                    Message = JsonSerializer.Serialize(message, new JsonSerializerOptions().Default()),
                    MessageAttributes = ConstructMessageAttributes(entityName, apiName)
                };

                await _snsClient.PublishAsync(publishRequest);
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
            var attributes = new Dictionary<string, MessageAttributeValue>();
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
                var request = new PublishRequest
                {
                    PhoneNumber = phoneNumber,
                    Message = message
                };

                await _snsClient.PublishAsync(request);
                _logger.LogInformation($"SMS sent to {phoneNumber}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS");
                throw;
            }
        }

        public async Task SendEmailAsync(SESMessage emailMessage)
        {
            try
            {
                using var emailClient = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1);
                var request = ConstructEmailRequest(emailMessage);

                await emailClient.SendEmailAsync(request);
                _logger.LogInformation("Email sent to: " + string.Join(", ", emailMessage.ReceiversAddress));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                throw;
            }
        }

        private SendEmailRequest ConstructEmailRequest(SESMessage emailMessage)
        {
            return new SendEmailRequest
            {
                Source = emailMessage.SenderAddress,
                Destination = new Destination { ToAddresses = emailMessage.ReceiversAddress },
                Message = new Amazon.SimpleEmail.Model.Message
                {
                    Subject = new Content(emailMessage.Subject),
                    Body = new Body { Html = new Content { Charset = "UTF-8", Data = emailMessage.Body } }
                }
            };
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
