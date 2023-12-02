using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Aws.Tools.Message.Serialization;
using System.Net.Mail;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;
using Aws.Tools.Message.Services.Notifications.SES.Templates;

namespace Aws.Tools.Message.Services.Messages.SES
{
    public class SESClient : ISESClient
    {
        private readonly ILogger<SESClient> _logger;

        public SESClient(ILogger<SESClient> logger)
        {
            _logger = logger;
        }


        public async Task SendEmailAsync(SESMessage emailMessage)
        {
            try
            {
                using AmazonSimpleEmailServiceClient emailClient = new(RegionEndpoint.USEast1);
                SendEmailRequest request = ConstructEmailRequest(emailMessage);

                _ = await emailClient.SendEmailAsync(request);
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
