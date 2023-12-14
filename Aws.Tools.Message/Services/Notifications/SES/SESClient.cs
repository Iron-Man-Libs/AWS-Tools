using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Aws.Tools.Message.Serialization;
using Aws.Tools.Message.Services.Notifications.SES.Templates;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Notifications.SES
{
    public class SESClient : ISESClient
    {
        private readonly ILogger<SESClient> _logger;
        private readonly AmazonSimpleEmailServiceClient _sesClient;

        public SESClient(ILogger<SESClient> logger)
        {
            _logger = logger;
            _sesClient = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1);
        }


        public async Task SendEmailAsync(SESMessage emailMessage)
        {
            try
            {
                SendEmailRequest request = ConstructEmailRequest(emailMessage);

                _ = await _sesClient.SendEmailAsync(request);
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

            _ = await _sesClient.SendTemplatedEmailAsync(request);
        }

        public async Task CreateTemplateAsync(string templateName, string subjectPart, string htmlPart)
        {
            Template template = new()
            {
                TemplateName = templateName,
                SubjectPart = subjectPart,
                HtmlPart = htmlPart
            };

            try
            {
                CreateTemplateRequest request = new() { Template = template };
                _ = await _sesClient.CreateTemplateAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating template");
            }
        }

        public async Task UpdateTemplateAsync(string templateName, string subjectPart, string htmlPart)
        {
            Template template = new()
            {
                TemplateName = templateName,
                SubjectPart = subjectPart,
                HtmlPart = htmlPart
            };

            try
            {
                UpdateTemplateRequest request = new() { Template = template };
                _ = await _sesClient.UpdateTemplateAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating template");
            }
        }

        public async Task DeleteTemplateAsync(string templateName)
        {
            try
            {
                DeleteTemplateRequest request = new() { TemplateName = templateName };
                _ = await _sesClient.DeleteTemplateAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting template");
            }
        }

        public async Task<GetTemplateResponse> GetTemplateAsync(string templateName)
        {
            try
            {
                GetTemplateRequest request = new() { TemplateName = templateName };
                return await _sesClient.GetTemplateAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting template");
            }

            return null;
        }
    }
}
